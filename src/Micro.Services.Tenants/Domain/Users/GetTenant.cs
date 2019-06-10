using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Users.GetTenant;

namespace Micro.Services.Tenants.Domain.Users
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a user</returns>
        [HttpGet("{id}/tenant")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> GetTenantAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class GetTenant
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {
            public TenantModel Tenant { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly TenantDbContext _db;
            private readonly IMapper _mapper;

            public Handler(TenantDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var id = request.Id;

                var user = await _db
                    .Users
                    .Include(x=>x.Tenant)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    throw new NotFoundException("user", "id", id);
                }

                return new Response
                {
                    Tenant = _mapper.Map<Tenant, TenantModel>(user.Tenant)
                };
            }
        }
    }
}