using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Services.Tenants.Domain.Roles
{
    [Route("api/roles")]
    [Produces("application/json")]
    [ApiController]
    public partial class Api : ControllerBase
    {
        private readonly IMediator _mediator;

        public Api(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}