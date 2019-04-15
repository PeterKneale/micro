using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Micro.ServiceB.Controllers
{
    [Route("api/databases")]
    [ApiController]
    public class DatabasesController : ControllerBase
    {
        private readonly IConnectionFactory _connections;
        private readonly ISqlGenerator _sql;
        private readonly IUrlHelper _urls;

        public DatabasesController(IConnectionFactory connections, ISqlGenerator sql, IUrlHelper urls)
        {
            _connections = connections;
            _sql = sql;
            _urls = urls;
        }

        // GET api/databases/{database}
        [HttpGet("{database}", Name = nameof(GetDatabaseAsync))]
        public async Task<IActionResult> GetDatabaseAsync(string database)
        {
            return Ok();
        }
    }
}