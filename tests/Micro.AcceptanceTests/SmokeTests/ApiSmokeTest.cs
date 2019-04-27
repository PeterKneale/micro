using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Micro.AcceptanceTests
{
    [Order(-1)]
    public class ApiSmokeTest : IClassFixture<HttpClientsFixture>
    {
        private readonly HttpClientsFixture _api;

        public ApiSmokeTest(HttpClientsFixture api)
        {
            _api = api;
        }

        [Fact]
        public async Task Verify_tenants_api_is_available()
        {
            await TestConfiguration.RetryAsync.ExecuteAsync(async () =>
                (await _api.TenantsHttpClient.GetAsync("/"))
                    .EnsureSuccessStatusCode()
            );
        }

        [Fact]
        public async Task Verify_content_api_is_available()
        {
            await TestConfiguration.RetryAsync.ExecuteAsync(async () =>
                (await _api.ContentHttpClient.GetAsync("/"))
                    .EnsureSuccessStatusCode()
            );
        }
    }
}
