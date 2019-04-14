using System.IO;
using Microsoft.Extensions.Configuration;

namespace Micro.AcceptanceTests
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
        }

        public static string ServiceAEndpoint => _config["SERVICE_A_API_ENDPOINT"];

        public static string ServiceBEndpoint => _config["SERVICE_B_API_ENDPOINT"];

        public static string ServiceAConnectionString => _config["SERVICE_A_CONNECTION_STRING"];

        public static string ServiceBConnectionString => _config["SERVICE_B_CONNECTION_STRING"];
    }
}
