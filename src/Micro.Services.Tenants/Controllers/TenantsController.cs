using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Micro.Services.Tenants.Commands;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Services.Tenants.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public TenantsController(IMediator mediator)
        {
            _mediator = mediator;
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
        [ProducesResponseType(404)]
        public async Task<ActionResult<TenantModel>> GetAsync(int id) 
            => new JsonResult(await _mediator.Send(new GetTenantQuery(id)));

        // POST api/tenants
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostAsync([FromBody] CreateTenantCommand request) 
            => new JsonResult(await _mediator.Send(request));

        // PUT api/values/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TenantModel>> PutAsync(int id, [FromBody] string name)
            => new JsonResult(await _mediator.Send(new UpdateTenantCommand(id, name)));

        // DELETE api/values/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAsync(int id)
            => new JsonResult(await _mediator.Send(new DeleteTenantCommand(id)));
    }
}

