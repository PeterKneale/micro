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
        public void Verify_tenants_database_available()
        {
            _db.TenantsDB.ExecuteScalar("SELECT 1;").Should()
                .NotBeNull().And
                .BeOfType<int>().And
                .Be(1);
        }

        [Fact]
        public void Verify_content_database_available()
        {
            _db.ContentDB.ExecuteScalar("SELECT 1;").Should()
                .NotBeNull().And
                .BeOfType<int>().And
                .Be(1);
        }
    }
}
