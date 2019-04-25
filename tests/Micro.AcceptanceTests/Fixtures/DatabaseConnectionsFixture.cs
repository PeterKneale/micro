using System;
using System.Data;
using System.Data.SqlClient;
using Micro.AcceptanceTests;

namespace Micro.Tests.Fixtures
{
    public class DatabaseConnectionsFixture : IDisposable
    {
        public DatabaseConnectionsFixture()
        {
            TenantsDB = new SqlConnection(TestConfiguration.TenantsDB);
            ContentDB = new SqlConnection(TestConfiguration.ContentDB);
        }

        public void Dispose()
        {
            TenantsDB?.Dispose();
            ContentDB?.Dispose();
        }

        public IDbConnection TenantsDB { get; }
        public IDbConnection ContentDB { get; }
    }
}
