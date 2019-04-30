using System;
using Micro.Services.Tenants.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.UnitTests.Fixtures
{
    public class DatabaseContextFixture : IDisposable
    {
        private readonly SqliteConnection _connection;

        public DatabaseContextFixture()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(_connection)
                .Options;

            DatabaseContext = new DatabaseContext(options);
            DatabaseContext.Database.EnsureCreated();

            VerificationDatabaseContext = new DatabaseContext(options);
        }

        public DatabaseContext DatabaseContext { get; }

        public DatabaseContext VerificationDatabaseContext { get; }

        public void Dispose()
        {
            _connection?.Dispose();
            DatabaseContext?.Dispose();
            VerificationDatabaseContext?.Dispose();
        }
    }
}
