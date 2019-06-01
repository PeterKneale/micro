using System.Collections.Generic;
using System.Net.Http;
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

        [Theory]
        [MemberData(nameof(Endpoints))]
        public async Task Verify_Tenants_URL_is_available(string url) => await Verify_url_is_available(_api.TenantsHttpClient, url);

        [Theory]
        [MemberData(nameof(Endpoints))]
        public async Task Verify_Content_URL_is_available(string url) => await Verify_url_is_available(_api.ContentHttpClient, url);

        private async Task Verify_url_is_available(HttpClient client, string url) => 
            await TestSettings.RetryAsync.ExecuteAsync(async () => (await client.GetAsync(url)).EnsureSuccessStatusCode());

        public static IEnumerable<object[]> Endpoints => new List<string[]>
        {
            new string[] { "/" },
            new string[] { "/health/alive" },
            new string[] { "/health/ready" },
            new string[] { "/app/name" },
            new string[] { "/app/version" },
        };
    }
}
