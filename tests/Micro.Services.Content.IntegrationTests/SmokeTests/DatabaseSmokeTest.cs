using Dapper;
using FluentAssertions;
using Micro.Services.Content.IntegrationTests.Fixtures;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Micro.Services.Content.IntegrationTests.SmokeTests
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
        public void Verify_database_available()
        {
            _db.Connection.ExecuteScalar("SELECT 1;").Should()
                .NotBeNull().And
                .BeOfType<int>().And
                .Be(1);
        }
    }
}
