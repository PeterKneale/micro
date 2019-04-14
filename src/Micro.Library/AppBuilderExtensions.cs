using System;
using System.Data.SqlClient;
using Dapper;
using Micro.Library.Internals;
using Microsoft.AspNetCore.Builder;
using Polly;
using Polly.Retry;

namespace Micro.Library
{
    public static class AppBuilderExtensions
    {
        private static readonly RetryPolicy _retry = Policy
                .Handle<Exception>()
                .WaitAndRetry(50, (a) => TimeSpan.FromMilliseconds(200));

        public static IApplicationBuilder WaitForDatabaseServer(this IApplicationBuilder builder, string connectionString)
        {
            var master = ConnectionStrings.ConvertToMasterConnectionString(connectionString);

            _retry.Execute(() =>
            {
                using (var connection = new SqlConnection(master))
                {
                    connection.Open();
                    connection.Close();
                }
            });

            return builder;
        }

        public static IApplicationBuilder UseDatabase(this IApplicationBuilder builder, string connectionString)
        {
            var database = ConnectionStrings.GetDatabaseName(connectionString);
            var server = ConnectionStrings.GetServerName(connectionString);
            var master = ConnectionStrings.ConvertToMasterConnectionString(connectionString);

            _retry.Execute(() =>
            {
                using (var connection = new SqlConnection(master))
                {
                    var exists = connection.ExecuteScalar<bool>($"SELECT COUNT(1) FROM [SYSDATABASES] WHERE [NAME]='{database}'");
                    if (!exists)
                    {
                        connection.Execute($"CREATE DATABASE [{database}]");
                    }
                }
            });

            return builder;
        }
    }
}
