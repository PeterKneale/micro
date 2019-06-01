using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
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

        public static string GatewayUrl => _config["GatewayUrl"] ?? throw new Exception($"GatewayUrl not configured");

        public static string TenantsUrl => _config["TenantsUrl"] ?? throw new Exception($"TenantsUrl not configured");

        public static string ContentUrl => _config["ContentUrl"] ?? throw new Exception($"ContentUrl not configured");
        
        public static string TenantsDb => _config["TenantsDb"] ?? throw new Exception($"TenantsDb not configured");

        public static string ContentDb => _config["ContentDb"] ?? throw new Exception($"ContentDb not configured");

        public static string RetryAttempts => _config["RetryAttempts"] ?? throw new Exception($"RetryAttempts not configured");

        public static string RetryInterval => _config["RetryInterval"] ?? throw new Exception($"RetryInterval not configured");

        public static AsyncRetryPolicy RetryAsync { get; }
    }
}
