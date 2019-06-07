using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Tenants.List;

namespace Micro.Services.Tenants.Domain.Tenants
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// List tenants
        /// </summary>
        /// <returns>a list of tenants</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Response>> ListAsync() => Ok(await _mediator.Send(new Request()));
    }
    
    public static class List
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public IEnumerable<TenantModel> Tenants { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly GlobalDbContext _db;
            private readonly IMapper _map;

            public Handler(GlobalDbContext db, IMapper map)
            {
                _db = db;
                _map = map;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                // Load tenant
                var data = await _db.Tenants.ToListAsync();

                // Map to model
                var model = _map.Map<IEnumerable<TenantModel>>(data);

                return new Response
                {
                    Tenants = model
                };
            }
        }
    }
}
