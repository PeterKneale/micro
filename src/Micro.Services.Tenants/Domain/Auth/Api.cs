using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Services.Tenants.Domain.Auth
{
    [Route("api/auth")]
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