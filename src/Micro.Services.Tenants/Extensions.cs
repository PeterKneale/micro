using System.Data.SqlClient;
using Micro.Services.Tenants.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Polly;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Micro.Services.Tenants
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
