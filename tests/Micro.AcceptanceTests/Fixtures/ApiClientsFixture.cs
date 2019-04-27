using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.AcceptanceTests
{
    public class ApiClientsFixture : IDisposable
    {
        private const string tenants_http_client_name = nameof(tenants_http_client_name);
        private const string content_http_content_name = nameof(content_http_content_name);

        private readonly ServiceProvider _provider;

        public ApiClientsFixture()
        {
            var services = new ServiceCollection();

            services.AddHttpClient(tenants_http_client_name, c =>
                {
                    c.BaseAddress = new Uri(TestConfiguration.TenantsAPI);
                });

            services.AddHttpClient(content_http_content_name, c =>
            {
                c.BaseAddress = new Uri(TestConfiguration.ContentAPI);
            });

            _provider = services.BuildServiceProvider();
        }

        public HttpClient TenantsHttpClient => GetHttpClient(tenants_http_client_name);

        public HttpClient ContentHttpClient => GetHttpClient(content_http_content_name);

        private HttpClient GetHttpClient(string name) => _provider.GetRequiredService<IHttpClientFactory>().CreateClient(name);

        public void Dispose()
        {
            _provider?.Dispose();
        }
    }
}
