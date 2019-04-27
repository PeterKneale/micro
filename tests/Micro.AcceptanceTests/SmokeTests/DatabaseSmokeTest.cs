using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Micro.Tests.Fixtures;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Micro.AcceptanceTests
{
    [Order(-1)]
    public class DatabaseSmokeTest : IClassFixture<DatabaseConnectionsFixture>
    {
        private readonly DatabaseConnectionsFixture _db;

        public DatabaseSmokeTest(DatabaseConnectionsFixture db)
        {
            _db = db;
        }

        [Fact]
        public async Task Verify_tenants_database_available()
        {
            await TestSettings.RetryAsync.ExecuteAsync(async () =>
                (await _db.TenantsDB.ExecuteScalarAsync("SELECT 1;")).Should()
                .NotBeNull().And
                .BeOfType<int>().And
                .Be(1)
            );
        }

        [Fact]
        public async Task Verify_content_database_available()
        {
            await TestSettings.RetryAsync.ExecuteAsync(async () =>
                (await _db.ContentDB.ExecuteScalarAsync("SELECT 1;")).Should()
                .NotBeNull().And
                .BeOfType<int>().And
                .Be(1)
            );
        }
    }
}
