using System.Net.Http;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Micro.Services.Content.IntegrationTests.Fixtures
{
    public class HttpClientFixture : WebApplicationFactory<Startup>
    {
        public HttpClientFixture()
        {
            HttpClient = CreateClient();
        }

        public HttpClient HttpClient { get; }

        public void OutputLogsToXUnit(ITestOutputHelper value)
        {
            Server.Host.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = value;
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((p) => p.AddXUnit());
            return base.CreateServer(builder);
        }
    }
}
