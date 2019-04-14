using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.AcceptanceTests
{
    public class ApiClientsFixture : IDisposable
    {
        private const string service_a = "service_a";
        private const string service_b = "service_b";

        private static ServiceProvider _provider;

        public HttpClient ServiceA => HttpClientFactory.Value.CreateClient(service_a);

        public HttpClient ServiceB => HttpClientFactory.Value.CreateClient(service_b);

        public void Dispose()
        {
            _provider.Dispose();
        }

        private Lazy<IHttpClientFactory> HttpClientFactory = new Lazy<IHttpClientFactory>(() =>
        {
            var services = new ServiceCollection();

            services.AddHttpClient(service_a, c =>
                {
                    c.BaseAddress = new Uri(TestConfiguration.ServiceAEndpoint);
                });

            services.AddHttpClient(service_b, c =>
            {
                c.BaseAddress = new Uri(TestConfiguration.ServiceBEndpoint);
            });

            _provider = services.BuildServiceProvider();

            return _provider.GetRequiredService<IHttpClientFactory>();
        }, true);

    }
}
