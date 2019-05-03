using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using HealthChecks.UI.Client;
using Micro.Services.Content.Data;
using Micro.Services.Content.Exceptions;
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

namespace Micro.Services.Content
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connection) =>
            services.AddDbContext<DatabaseContext>(ctx => ctx.UseSqlServer(connection, opt => opt.EnableRetryOnFailure()));

        public static void CreateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var log = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

                var master = configuration.GetMasterSqlConnectionString();
                int retryAttempts = 30;
                int retryInterval = 1000;

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(
                        retryAttempts,
                        retryAttempt => TimeSpan.FromMilliseconds(retryInterval),
                        (exception, timeSpan, retryCount, context) =>
                            log.LogWarning($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"))
                    .Execute(() =>
                    {
                        using (var conn = new SqlConnection(master))
                        {
                            conn.Open();
                            conn.Close();
                        }
                    });

                db.Database.EnsureCreated();
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

        public static IApplicationBuilder UseMetaEndpoints(this IApplicationBuilder app)
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
                .Map("/app/errors/internal", appBuilder =>
                {
                    appBuilder.Run(context =>
                    {
                        throw new Exception("ERROR!");
                    });
                })
                .Map("/app/errors/notfound", appBuilder =>
                {
                    appBuilder.Run(context =>
                    {
                        throw new NotFoundException("entity", "property", "value");
                    });
                })
                .Map("/app/errors/notunique", appBuilder =>
                {
                    appBuilder.Run(context =>
                    {
                        throw new NotUniqueException("entity", "property", "value");
                    });
                });

        public static string GetSqlConnectionString(this IConfiguration configuration) =>
            configuration["CONNECTION_STRING"];

        public static string GetMasterSqlConnectionString(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()) { InitialCatalog = "master" }.ToString();

        public static string GetSqlDatabaseName(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).InitialCatalog;

        public static string GetSqlServerName(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).DataSource;

        public static string GetSeqUrl(this IConfiguration configuration) =>
            configuration["SEQ_URL"];
    }
}
