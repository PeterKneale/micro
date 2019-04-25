using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.AcceptanceTests
{
    public class ApiClientsFixture : IDisposable
    {
        private const string service_tenants = "service_tenants";
        private const string service_content = "service_content";

        private static ServiceProvider _provider;

        public HttpClient TenantsService => HttpClientFactory.Value.CreateClient(service_tenants);

        public HttpClient ContentService => HttpClientFactory.Value.CreateClient(service_content);

        public void Dispose()
        {
            _provider.Dispose();
        }

        private Lazy<IHttpClientFactory> HttpClientFactory = new Lazy<IHttpClientFactory>(() =>
        {
            var services = new ServiceCollection();

            services.AddHttpClient(service_tenants, c =>
                {
                    c.BaseAddress = new Uri(TestConfiguration.TenantsAPI);
                });

            services.AddHttpClient(service_content, c =>
            {
                c.BaseAddress = new Uri(TestConfiguration.ContentAPI);
            });

            _provider = services.BuildServiceProvider();

            return _provider.GetRequiredService<IHttpClientFactory>();
        }, true);

    }
}
