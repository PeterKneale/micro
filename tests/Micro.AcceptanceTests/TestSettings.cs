using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace Micro.AcceptanceTests
{
    public static class TestSettings
    {
        private static readonly IConfiguration _config;

        static TestSettings()
        {
            _config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("testsettings.json")
              .AddEnvironmentVariables()
              .Build();

            var retryAttempts = int.Parse(RetryAttempts);
            var retryInterval = int.Parse(RetryInterval);

            RetryAsync = Policy
              .Handle<Exception>()
              .WaitAndRetryAsync(
                retryAttempts,
                retryAttempt => TimeSpan.FromMilliseconds(retryInterval),
                (exception, timeSpan, retryCount, context) =>
                    Trace.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
        }

        public static string TenantsAPI => _config["Tenants_API"] ?? throw new Exception($"Tenants API not configured");

        public static string ContentAPI => _config["Content_API"] ?? throw new Exception($"Content API not configured");

        public static string TenantsDB => _config["Tenants_DB"] ?? throw new Exception($"Tenants DB not configured");

        public static string ContentDB => _config["Content_DB"] ?? throw new Exception($"Content DB not configured");

        public static string RetryAttempts => _config["RETRY_ATTEMPTS"] ?? throw new Exception($"Retry attempts not configured");

        public static string RetryInterval => _config["RETRY_INTERVAL"] ?? throw new Exception($"Retry interval not configured");

        public static AsyncRetryPolicy RetryAsync { get; }
    }
}
