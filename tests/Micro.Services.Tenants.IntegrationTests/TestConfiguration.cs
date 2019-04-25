using System.IO;
using Microsoft.Extensions.Configuration;

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
        }

        public static string ConnectionString => _config["ConnectionString"];
    }
}
