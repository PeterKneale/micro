using System.Data.SqlClient;

namespace Micro.Library.Internals
{
    internal static class DatabaseHelper
    {
        public static string ConvertToMasterConnectionString(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            }.ToString();
        }

        public static string GetDatabaseName(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        public static string GetServerName(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).DataSource;
        }
    }
}
