using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace Micro.Services.Tenants.IntegrationTests
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

        public static string ConnectionString => _config["ConnectionString"] ?? throw new Exception($"Connection string not configured");

        public static string RetryAttempts => _config["RetryAttempts"] ?? throw new Exception($"Retry attempts not configured");

        public static string RetryInterval => _config["RetryInterval"] ?? throw new Exception($"Retry interval not configured");

        public static AsyncRetryPolicy RetryAsync { get; }
    }
}
