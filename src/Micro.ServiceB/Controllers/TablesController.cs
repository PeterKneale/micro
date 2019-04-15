using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Micro.ServiceB.Models;
using Microsoft.AspNetCore.Mvc;

namespace Micro.ServiceB.Controllers
{

    [Route("api/databases/{database}/tables")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly IConnectionFactory _connections;
        private readonly ISqlGenerator _sql;
        private readonly IUrlHelper _urls;

        public TablesController(IConnectionFactory connections, ISqlGenerator sql, IUrlHelper urls)
        {
            _connections = connections;
            _sql = sql;
            _urls = urls;
        }

        // GET api/databases/{database}/tables
        [HttpGet(Name = nameof(ListTables))]
        public async Task<IActionResult> ListTables(string database)
        {
            var connection = await GetConnectionTo(database);
            var tables = await connection.QueryAsync<InfoSchemaTable>(_sql.ListTables());
            var models = tables.Select(_ => new TableSummaryModel
            {
                Database = _.TABLE_CATALOG,
                Name = _.TABLE_NAME,
                Schema = _.TABLE_SCHEMA
            }).ToList();
            foreach (var model in models)
            {
                model.AddLinks(_urls);
            }
            return Ok(models);
        }


        // GET api/databases/{database}/sc/tables/{table}
        [HttpGet("{table}", Name = nameof(GetTableAsync))]
        public async Task<IActionResult> GetTableAsync(string database, string table)
        {
            var connection = await GetConnectionTo(database);
            var details = await connection.QuerySingleAsync<InfoSchemaTable>(_sql.GetTable(table));
            var columns = await connection.ExecuteScalarAsync<int>(_sql.CountColumnsInTable(table));
            var constraints = await connection.ExecuteScalarAsync<int>(_sql.CountConstraintsOnTable(table));
            var model = new TableDetailsModel
            {
                Database = details.TABLE_CATALOG,
                Name = details.TABLE_NAME,
                Schema = details.TABLE_SCHEMA,
                Columns = columns,
                Constraints = constraints,
            };
            model.AddLinks(_urls);
            return Ok(model);
        }

        private async Task<IDbConnection> GetConnectionTo(string database)
        {
            return await _connections.GetConnectionAsync(database);
        }
    }
}