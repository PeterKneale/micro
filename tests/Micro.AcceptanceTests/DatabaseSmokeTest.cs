using Dapper;
using FluentAssertions;
using Micro.Tests.Fixtures;
using Xunit;

namespace Micro.AcceptanceTests
{
    public class DatabaseSmokeTest : IClassFixture<DatabaseConnectionsFixture>
    {
        private readonly DatabaseConnectionsFixture _db;

        public DatabaseSmokeTest(DatabaseConnectionsFixture db)
        {
            _db = db;
        }

        [Fact]
        public void Verify_service_a_database_available()
        {
            _db.ServiceA.ExecuteScalar("SELECT 1;")
                .Should().NotBeNull().And
                .Should().BeOfType<int>()
                .Should().Be(1);
        }

        [Fact]
        public void Verify_service_b_database_available()
        {
            _db.ServiceB.ExecuteScalar("SELECT 1;")
                .Should().NotBeNull().And
                .Should().BeOfType<int>()
                .Should().Be(1);
        }
    }
}
