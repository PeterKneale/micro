using Micro.Services.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Micro.Services.Web
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IGatewayApi, GatewayApi>();
        }

        public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient();
            return services;
        }

        public static string GetGatewayUrl() => "http://localhost:5000";
        public static string GetGatewayEndpoint() => "/query";
    }
}
