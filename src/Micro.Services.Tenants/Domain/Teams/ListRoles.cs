using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Teams.ListRoles;

namespace Micro.Services.Tenants.Domain.Teams
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get team roles
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a list of roles</returns>
        [HttpGet("{id}/roles")]
        public async Task<ActionResult<Response>> ListRolesAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class ListRoles
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {
            public RoleModel[] Roles { get; set; }
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

                var team = await _db
                    .Teams
                    .Include(u => u.TeamRoles)
                    .ThenInclude(x=>x.Role)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (team == null)
                {
                    throw new NotFoundException("team", "id", id);
                }

                var roles = team.TeamRoles
                    .Select(x => x.Role)
                    .ToArray();

                return new Response
                {
                    Roles = _mapper.Map<Role[], RoleModel[]>(roles)
                };
            }
        }
    }
}