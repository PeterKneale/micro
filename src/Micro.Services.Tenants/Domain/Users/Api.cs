using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Services.Tenants.Domain.Users
{
    [Route("api/users")]
    [Produces("application/json")]
    [ApiController]
    public partial class Api : ControllerBase
    {
        private readonly IMediator _mediator;

        public Api(IMediator requests)
        {
            _mediator = requests;
        }
    }
}