using System.Threading.Tasks;
using Micro.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Micro.ServiceB.IntegrationTests
{
    public class ApiSmokeTest : IClassFixture<ApiFixture<Startup>>
    {
        private readonly ApiFixture<Startup> _api;

        public ApiSmokeTest(ApiFixture<Startup> api, ITestOutputHelper output)
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
