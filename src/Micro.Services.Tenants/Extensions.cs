using FluentMigrator.Runner;
using HealthChecks.UI.Client;
using Micro.Services.Tenants.Database;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Micro.Services.Tenants
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<IUserContext, RequestContext>()
                .AddScoped<ITenantContext, RequestContext>();
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization()
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5004";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api";
                    options.ApiSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                });
            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = Program.AppName, Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(i => i.FullName);
                c.DescribeAllEnumsAsStrings();
            });
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                // For postman integration
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    // if postman is in the query, generate a postman compatible swagger file
                    if (httpReq.Query.ContainsKey("postman"))
                    {
                        swaggerDoc.Host = "{{host}}";
                        swaggerDoc.Schemes = new string[] { "http" };
                    }
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Program.AppName} {Program.AppVersion}");
                c.RoutePrefix = "";
            });
            return app;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connection) =>
            services
                .AddDbContext<GlobalDbContext>(ctx => ctx.UseSqlServer(connection, opt => opt.EnableRetryOnFailure()))
                .AddDbContext<TenantDbContext>(ctx => ctx.UseSqlServer(connection, opt => opt.EnableRetryOnFailure()))
                .AddScoped<IDatabaseMigrator, DatabaseMigrator>()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connection)
                    .ScanIn(typeof(Migration1_Schema).Assembly).For.All());

        public static void CreateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var db = scope.ServiceProvider.GetRequiredService<GlobalDbContext>();
                var log = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

                var masterConnection = configuration.GetMasterSqlConnectionString();
                var dbConnection = configuration.GetSqlConnectionString();
                var dbName = configuration.GetSqlDatabaseName();
                var dbServer = configuration.GetSqlServerName();

                log.LogInformation($"Database Connection: {dbConnection}");

                int retryAttempts = 30;
                int retryInterval = 1000;
                var sql = $"IF NOT EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE name = N'{dbName}') CREATE DATABASE [{dbName}]";
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(
                        retryAttempts,
                        retryAttempt => TimeSpan.FromMilliseconds(retryInterval),
                        (exception, timeSpan, retryCount, context) =>
                            log.LogWarning($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"))
                    .Execute(() =>
                    {
                        using (var conn = new SqlConnection(masterConnection))
                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            log.LogInformation("Connecting to {dbServer}", dbServer);
                            conn.Open();
                            log.LogInformation("Creating database {dbName} on {dbServer}", dbName, dbServer);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    });
            }
        }

        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var migrator = scope.ServiceProvider.GetRequiredService<IDatabaseMigrator>();
                migrator.MigrateUp();
            }
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecksUI();
            services.AddHealthChecks()
                .AddCheck("api", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString, name: "db");
            return services;
        }

        public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
            => app
                .UseHealthChecksUI()
                .UseHealthChecks("/health/alive", new HealthCheckOptions
                {
                    Predicate = r => r.Name == "api",
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                })
                .UseHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = r => r.Name == "db",
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

        public static IApplicationBuilder UseCustomMetaEndpoints(this IApplicationBuilder app)
            => app
                .Map("/app/name", appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        await context.Response.WriteAsync(Program.AppName);
                    });
                })
                .Map("/app/version", appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        await context.Response.WriteAsync(Program.AppVersion);
                    });
                })
                .Map("/app/config", appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var d = new Dictionary<string, string>();
                        var configuration = appBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
                        foreach (var entry in configuration.AsEnumerable().OrderBy(x => x.Key))
                        {
                            d.Add(entry.Key, entry.Value);
                        }
                        var json = JsonConvert.SerializeObject(d);
                        await context.Response.WriteAsync(json);
                    });
                })
                .Map("/errors/internal", appBuilder =>
                {
                    appBuilder.Run(context =>
                    {
                        throw new Exception("ERROR!");
                    });
                })
                .Map("/errors/notfound", appBuilder =>
                {
                    appBuilder.Run(context =>
                    {
                        throw new NotFoundException("entity", "property", "value");
                    });
                })
                .Map("/errors/notunique", appBuilder =>
                {
                    appBuilder.Run(context =>
                    {
                        throw new NotUniqueException("entity", "property", "value");
                    });
                });

        public static string GetSqlConnectionString(this IConfiguration configuration) =>
            configuration["ConnectionString"];

        public static string GetMasterSqlConnectionString(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()) { InitialCatalog = "master" }.ToString();

        public static string GetSqlDatabaseName(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).InitialCatalog;

        public static string GetSqlServerName(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).DataSource;

        public static string GetSeqUrl(this IConfiguration configuration) =>
            configuration["SeqUrl"];
    }
}
