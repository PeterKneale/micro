using System.Data;
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
            // arrange
            var connection = _db.ServiceA;
            var command = connection.CreateCommand();
            command.CommandText = "SELECT 1;";
            command.CommandType = CommandType.Text;

            // act
            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<int>();
            result.Should().Be(1);
        }

        [Fact]
        public void Verify_service_b_database_available()
        {
            // arrange
            var connection = _db.ServiceB;
            var command = connection.CreateCommand();
            command.CommandText = "SELECT 1;";
            command.CommandType = CommandType.Text;

            // act
            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<int>();
            result.Should().Be(1);
        }
    }
}
