using System.Threading.Tasks;
using Micro.Services.Content.IntegrationTests.Fixtures;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Micro.Services.Content.IntegrationTests.SmokeTests
{
    [Order(-1)]
    public class ApiSmokeTest : IClassFixture<HttpClientFixture>
    {
        private readonly HttpClientFixture _api;

        public ApiSmokeTest(HttpClientFixture api, ITestOutputHelper output)
        {
            api.OutputLogsToXUnit(output);
            _api = api;
        }

        [Fact]
        public async Task Verify_api_available()
        {
            await TestSettings.RetryAsync.ExecuteAsync(async () =>
                (await _api.HttpClient.GetAsync("/"))
                    .EnsureSuccessStatusCode()
            );
        }
    }
}
