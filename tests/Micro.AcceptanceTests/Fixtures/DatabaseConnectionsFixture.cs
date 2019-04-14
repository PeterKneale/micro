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
            ServiceA = new SqlConnection(TestConfiguration.ServiceAConnectionString);
            ServiceB = new SqlConnection(TestConfiguration.ServiceBConnectionString);
        }

        public void Dispose()
        {
            ServiceA?.Dispose();
            ServiceB?.Dispose();
        }

        public IDbConnection ServiceA { get; }
        public IDbConnection ServiceB { get; }
    }
}
