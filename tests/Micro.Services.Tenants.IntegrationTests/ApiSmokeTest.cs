using System.Threading.Tasks;
using Micro.Services.Tenants.IntegrationTests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Micro.Services.Tenants.IntegrationTests
{
    public class ApiSmokeTest : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _api;

        public ApiSmokeTest(ApiFixture api, ITestOutputHelper output)
        {
            api.OutputLogsToXUnit(output);
            _api = api;
        }

        [Fact]
        public async Task Verify_api_available()
        {
            var response = await _api.HttpClient.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    }
}