using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.AcceptanceTests
{
    public class HttpClientsFixture : IDisposable
    {
        private const string gateway_http_client_name = nameof(gateway_http_client_name);
        private const string tenants_http_client_name = nameof(tenants_http_client_name);
        private const string content_http_content_name = nameof(content_http_content_name);

        private readonly ServiceProvider _provider;

        public HttpClientsFixture()
        {
            var services = new ServiceCollection();

            services.AddHttpClient(gateway_http_client_name, c =>
            {
                c.BaseAddress = new Uri(TestSettings.GatewayUrl);
            });

            services.AddHttpClient(tenants_http_client_name, c =>
                {
                    c.BaseAddress = new Uri(TestSettings.TenantsUrl);
                });

            services.AddHttpClient(content_http_content_name, c =>
            {
                c.BaseAddress = new Uri(TestSettings.ContentUrl);
            });

            _provider = services.BuildServiceProvider();
        }

        public HttpClient GatewayHttpClient => GetHttpClient(gateway_http_client_name);

        public HttpClient TenantsHttpClient => GetHttpClient(tenants_http_client_name);

        public HttpClient ContentHttpClient => GetHttpClient(content_http_content_name);

        private HttpClient GetHttpClient(string name) => _provider.GetRequiredService<IHttpClientFactory>().CreateClient(name);

        public void Dispose()
        {
            _provider?.Dispose();
        }
    }
}
