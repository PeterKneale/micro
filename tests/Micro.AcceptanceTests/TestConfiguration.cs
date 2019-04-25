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

        public static string TenantsAPI => _config["Tenants_API"];

        public static string ContentAPI => _config["Content_API"];

        public static string TenantsDB => _config["Tenants_DB"];

        public static string ContentDB => _config["Content_DB"];
    }
}
