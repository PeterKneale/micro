using System;
using System.Data;
using System.Data.SqlClient;

namespace Micro.Services.Tenants.IntegrationTests.Fixtures
{
    public class DatabaseConnectionFixture : IDisposable
    {
        public DatabaseConnectionFixture()
        {
            Connection = new SqlConnection(TestConfiguration.ConnectionString);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public IDbConnection Connection { get; }
    }
}
