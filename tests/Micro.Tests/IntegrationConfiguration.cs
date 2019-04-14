using System.IO;
using Microsoft.Extensions.Configuration;

namespace Micro.Tests
{
    public static class IntegrationConfiguration
    {
        private static readonly IConfiguration _config;

        static IntegrationConfiguration()
        {
            _config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .AddEnvironmentVariables()
              .Build();
        }

        public static string ConnectionString => _config["ConnectionString"];
    }
}
