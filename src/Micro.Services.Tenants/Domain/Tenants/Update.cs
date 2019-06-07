using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Tenants.Update;

namespace Micro.Services.Tenants.Domain.Tenants
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Update tenant
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">name</param>
        /// <returns>a tenant</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Response>> UpdateAsync(int id, [FromBody] string name) => Ok(await _mediator.Send(new Request(id, name)));
    }
   
    public static class Update
    {
        public class Request : IRequest<Response>
        {
            public Request(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }
            public string Name { get; }
        }

        public class Response
        {
            public TenantModel Tenant { get; set; }
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
                var id = request.Id;
                var name = request.Name;

                // Load tenant
                var data = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    throw new NotFoundException("tenant", "id", id);
                }

                // Check new name is unique
                var exists = await _db.Tenants.AnyAsync(x => x.Name == name && x.Id != id);
                if (exists)
                {
                    throw new NotUniqueException("tenant", "name", name);
                }

                // Save tenant
                data.Name = name;
                _db.Tenants.Update(data);
                await _db.SaveChangesAsync();

                // Load tenant
                var tenant = await _db.Tenants.SingleAsync(x => x.Id == data.Id);
                return new Response
                {
                    Tenant = _map.Map<TenantModel>(tenant)
                };
            }
        }
    }

}
