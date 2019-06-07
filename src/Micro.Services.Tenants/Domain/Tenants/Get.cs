using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Tenants.Get;

namespace Micro.Services.Tenants.Domain.Tenants
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get tenant
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a team</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TenantModel>> GetAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }
   
    public static class Get
    {
        public class Request : IRequest<Result>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Result
        {
            public TenantModel Tenant { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result>
        {
            private readonly GlobalDbContext _db;
            private readonly IMapper _map;

            public Handler(GlobalDbContext db, IMapper map)
            {
                _db = db;
                _map = map;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var id = request.Id;

                // Load tenant
                var data = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    throw new NotFoundException("tenant", "id", id);
                }

                // Map to model
                var model = _map.Map<TenantModel>(data);

                return new Result
                {
                    Tenant = model
                };
            }
        }
    }
}
