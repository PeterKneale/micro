using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace Micro.AcceptanceTests
{
    public static class TestConfiguration
    {
        private static readonly IConfiguration _config;

        static TestConfiguration()
        {
            _config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("testsettings.json")
              .AddEnvironmentVariables()
              .Build();

            var retryAttempts = int.Parse(_config["RetryAttempts"]);
            var retryInterval = int.Parse(_config["RetryInterval"]);

            RetryAsync = Policy
              .Handle<Exception>()
              .WaitAndRetryAsync(
                retryAttempts,
                retryAttempt => TimeSpan.FromMilliseconds(retryInterval),
                (exception, timeSpan, retryCount, context) =>
                    Trace.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
        }

        public static string TenantsAPI => _config["Tenants_API"];

        public static string ContentAPI => _config["Content_API"];

        public static string TenantsDB => _config["Tenants_DB"];

        public static string ContentDB => _config["Content_DB"];

        public static AsyncRetryPolicy RetryAsync { get; }
    }
}
