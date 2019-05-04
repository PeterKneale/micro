using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Commands;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DatabaseContext _db;
        private readonly IMapper _map;

        public TenantsController(IMediator mediator, DatabaseContext db, IMapper map)
        {
            _mediator = mediator;
            _db = db;
            _map = map;
        }

        // GET api/tenants
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<TenantModel>>> GetAsync() 
            => new JsonResult(await _mediator.Send(new GetTenantsQuery()));

        // GET api/tenants/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TenantModel>> GetAsync(int id) 
            => new JsonResult(await _mediator.Send(new GetTenantQuery(id)));

        // POST api/tenants
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostAsync([FromBody] CreateTenantCommand request) 
            => new JsonResult(await _mediator.Send(request));

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TenantModel>> PutAsync(int id, [FromBody] string name)
        {
            var tenant = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (tenant == null)
            {
                return NotFound();
            }
            tenant.Name = name;
            await _db.Tenants.AddAsync(tenant);
            await _db.SaveChangesAsync();

            var model = _map.Map<TenantModel>(tenant);
            return Ok(model);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var tenant = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (tenant == null)
            {
                return Ok();
            }

            _db.Tenants.Remove(tenant);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}

