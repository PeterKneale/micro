using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace Micro.Services.Tenants.IntegrationTests
{
    public static class TestConfiguration
    {
        private static readonly IConfiguration _config;

        static TestConfiguration()
        {
            _config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
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
                    Debug.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
        }

        public static string ConnectionString => _config["ConnectionString"];

        public static AsyncRetryPolicy RetryAsync { get; }
    }
}
