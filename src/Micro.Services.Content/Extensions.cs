using System.Data.SqlClient;
using Micro.Services.Content.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
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

                var master = configuration.GetMasterSqlConnectionString();
                int retryAttempts = 30;
                int retryInterval = 1000;

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(
                        retryAttempts,
                        retryAttempt => TimeSpan.FromMilliseconds(retryInterval),
                        (exception, timeSpan, retryCount, context) =>
                            Trace.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"))
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
