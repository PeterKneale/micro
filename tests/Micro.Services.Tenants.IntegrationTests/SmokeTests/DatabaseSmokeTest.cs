using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Micro.Services.Tenants.IntegrationTests.Fixtures;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Micro.Services.Tenants.IntegrationTests.SmokeTests
{
    [Order(-1)]
    public class DatabaseSmokeTest : IClassFixture<DatabaseConnectionFixture>
    {
        private readonly DatabaseConnectionFixture _db;

        public DatabaseSmokeTest(DatabaseConnectionFixture db)
        {
            _db = db;
        }

        [Fact]
        public async Task Verify_database_available()
        {
            await TestSettings.RetryAsync.ExecuteAsync(async () =>
                (await _db.Connection.ExecuteScalarAsync("SELECT 1;")).Should()
                .NotBeNull().And
                .BeOfType<int>().And
                .Be(1)
            );
        }
    }
}
