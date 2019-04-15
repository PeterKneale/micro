namespace Micro.ServiceB.Controllers
{
    public interface ISqlGenerator
    {
        string ListTables();
        string GetTable(string table);
        string CountColumnsInTable(string table);
        string CountConstraintsOnTable(string table);
    }

    public class SqlGenerator : ISqlGenerator
    {
        public string ListTables()
        {
            return $@"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
        }

        public string GetTable(string table)
        {
            return $@"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='{table}' AND TABLE_TYPE='BASE TABLE'";
        }

        public string CountColumnsInTable(string table)
        {
            return $@"SELECT COUNT(1) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}'";
        }

        public string CountConstraintsOnTable(string table)
        {
            return $"SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = '{table}'";
        }
    }
}