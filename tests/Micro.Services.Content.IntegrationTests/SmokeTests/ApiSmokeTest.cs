using System.Collections.Generic;
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

        [Theory]
        [MemberData(nameof(Endpoints))]
        public async Task Verify_api_available(string url)
        {
            await TestSettings.RetryAsync.ExecuteAsync(async () =>
                (await _api.HttpClient.GetAsync(url))
                    .EnsureSuccessStatusCode()
            );
        }

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
