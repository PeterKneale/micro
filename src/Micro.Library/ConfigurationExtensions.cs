using Microsoft.Extensions.Configuration;

namespace Micro.Library
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfiguration config)
        {
            return config["CONNECTION_STRING"];
        }
    }
}
