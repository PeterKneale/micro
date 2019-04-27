using System.Threading.Tasks;
using Xunit;

namespace Micro.AcceptanceTests
{
    public class ApiSmokeTest : IClassFixture<ApiClientsFixture>
    {
        private readonly ApiClientsFixture _api;

        public ApiSmokeTest(ApiClientsFixture api)
        {
            _api = api;
        }

        [Fact]
        public async Task Verify_tenants_api_is_available()
        {
            // arrange
            var path = "/";

            // act
            var response = await _api.TenantsHttpClient.GetAsync(path);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Verify_content_api_is_available()
        {
            // arrange
            var path = "/";

            // act
            var response = await _api.ContentHttpClient.GetAsync(path);

            // assert
            response.EnsureSuccessStatusCode();
        }
    }
}
