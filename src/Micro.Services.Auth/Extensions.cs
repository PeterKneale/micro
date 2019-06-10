using FluentMigrator.Runner;
using HealthChecks.UI.Client;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Micro.Services.Auth.Database;
using Micro.Services.Auth.DataContext;
using Micro.Services.Auth.Services;
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
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Micro.Services.Auth
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IUserApi, UserApi>()
                .AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
                .AddTransient<IProfileService, ProfileService>();
        }
        
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connection)
        {
            return services
                .AddDbContext<DatabaseContext>(ctx => ctx.UseSqlServer(connection, opt => opt.EnableRetryOnFailure()))
                .AddScoped<IDatabaseMigrator, DatabaseMigrator>()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connection)
                .ScanIn(typeof(Migration1_Schema).Assembly).For.All());
        }

        public static void CreateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var log = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

                var masterConnection = configuration.GetMasterSqlConnectionString();
                var dbConnection = configuration.GetSqlConnectionString();
                var dbName = configuration.GetSqlDatabaseName();
                var dbServer = configuration.GetSqlServerName();

                log.LogInformation($"Database Connection: {dbConnection}");

                var retryAttempts = 30;
                var retryInterval = 1000;
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
        {
            return app
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
        }

        public static IApplicationBuilder UseCustomMetaEndpoints(this IApplicationBuilder app)
        {
            return app
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
                });
        }

        public static string GetSqlConnectionString(this IConfiguration configuration)
        {
            return configuration["ConnectionString"];
        }

        public static string GetMasterSqlConnectionString(this IConfiguration configuration)
        {
            return new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()) { InitialCatalog = "master" }.ToString();
        }

        public static string GetSqlDatabaseName(this IConfiguration configuration)
        {
            return new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).InitialCatalog;
        }

        public static string GetSqlServerName(this IConfiguration configuration)
        {
            return new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).DataSource;
        }

        public static string GetSeqUrl(this IConfiguration configuration)
        {
            return configuration["SeqUrl"];
        }

        public static string GetTenantsUrl(this IConfiguration configuration)
        {
            return configuration["TENANTS_URL"];
        }
    }
}
