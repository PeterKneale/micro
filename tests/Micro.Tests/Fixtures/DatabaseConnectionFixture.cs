using System;
using System.Data;
using System.Data.SqlClient;

namespace Micro.Tests.Fixtures
{
    public class DatabaseConnectionFixture : IDisposable
    {
        public DatabaseConnectionFixture()
        {
            Connection = new SqlConnection(IntegrationConfiguration.ConnectionString);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public IDbConnection Connection { get; private set; }
    }
}
