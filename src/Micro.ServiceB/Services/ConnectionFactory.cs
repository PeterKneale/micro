using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Micro.ServiceB.Controllers
{
    public interface IConnectionFactory
    {
        Task<IDbConnection> GetConnectionAsync(string database);
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly Func<IDbConnection> _connection;
        private readonly ISqlGenerator _sql;

        public ConnectionFactory(Func<IDbConnection> connection, ISqlGenerator sql)
        {
            _connection = connection;
            _sql = sql;
        }

        public async Task<IDbConnection> GetConnectionAsync(string database)
        {
            var connectionstring = new SqlConnectionStringBuilder(_connection().ConnectionString)
            {
                InitialCatalog = database
            }.ToString();
            return new SqlConnection(connectionstring);
        }
    }
}