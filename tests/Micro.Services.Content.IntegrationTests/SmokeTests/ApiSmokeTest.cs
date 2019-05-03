using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task Verify_api_available(string url, HttpStatusCode httpStatusCode)
        {
            //await TestSettings.RetryAsync.ExecuteAsync(async () =>
            (await _api.HttpClient.GetAsync(url))
                .StatusCode.Should().Be(httpStatusCode);
            //);
        }

        public static IEnumerable<object[]> Endpoints => new List<object[]>
        {
            // root
            new object[] { "/", HttpStatusCode.OK },
            // health
            new object[] { "/health/alive", HttpStatusCode.OK },
            new object[] { "/health/ready", HttpStatusCode.OK },
            // app
            new object[] { "/app/name", HttpStatusCode.OK },
            new object[] { "/app/version", HttpStatusCode.OK },
            // app errors
            new object[] { "/app/errors/internal", HttpStatusCode.InternalServerError },
            new object[] { "/app/errors/notfound", HttpStatusCode.NotFound },
            new object[] { "/app/errors/notunique", HttpStatusCode.BadRequest },
        };
    }
}
