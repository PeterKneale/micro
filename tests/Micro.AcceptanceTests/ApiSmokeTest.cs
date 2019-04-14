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
        public async Task Verify_service_a_api_available()
        {
            // arrange
            var path = "/";

            // act
            var response = await _api.ServiceA.GetAsync(path);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Verify_service_b_api_available()
        {
            // arrange
            var path = "/";

            // act
            var response = await _api.ServiceB.GetAsync(path);

            // assert
            response.EnsureSuccessStatusCode();
        }
    }
}
