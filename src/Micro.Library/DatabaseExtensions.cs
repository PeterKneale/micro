using System;
using System.Data.SqlClient;
using Dapper;
using Micro.Library.Internals;
using Microsoft.AspNetCore.Hosting;
using Polly;
using Polly.Retry;

namespace Micro.Library
{
    public static class DatabaseExtensions
    {
        private static readonly RetryPolicy _retry = Policy
                .Handle<Exception>()
                .WaitAndRetry(50, (a) => TimeSpan.FromMilliseconds(200));

        public static IWebHostBuilder UseDatabase(this IWebHostBuilder builder, string connectionString)
        {
            var database = DatabaseHelper.GetDatabaseName(connectionString);
            var server = DatabaseHelper.GetServerName(connectionString);
            var master = DatabaseHelper.ConvertToMasterConnectionString(connectionString);

            _retry.Execute(() =>
            {
                using (var connection = new SqlConnection(master))
                {
                    connection.Open();
                    connection.Close();
                }
            });

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
