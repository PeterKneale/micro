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
using static Micro.Services.Tenants.Domain.Roles.ListTeams;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get role permissions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a list of permissions</returns>
        [HttpGet("{id}/teams")]
        public async Task<ActionResult<Response>> ListTeamsAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class ListTeams
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {
            public TeamModel[] Teams { get; set; }
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

                var role = await _db
                    .Roles
                    .Include(u => u.TeamRoles)
                    .ThenInclude(u => u.Team)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (role == null)
                {
                    throw new NotFoundException("role", "id", id);
                }

                var teams = role.TeamRoles
                    .Select(x => x.Team)
                    .ToArray();

                return new Response
                {
                    Teams = _mapper.Map<Team[], TeamModel[]>(teams)
                };
            }
        }
    }
}