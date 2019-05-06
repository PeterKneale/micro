using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Micro.Services.Tenants.DataContext;
using System.Threading;
using System.Threading.Tasks;
using Micro.Services.Tenants.Models.Common;
using static Micro.Services.Tenants.Domain.Teams.Delete;

namespace Micro.Services.Tenants.Domain.Teams
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Delete team
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a team</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> Delete(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class Delete
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {

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
                var team = await _db
                    .Teams
                    .Include(x => x.TeamRoles)
                    .Include(x => x.UserTeams)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (team != null)
                {
                    foreach (var user in team.UserTeams)
                    {
                        _db.Remove(user);
                    }
                    foreach (var role in team.TeamRoles)
                    {
                        _db.Remove(role);
                    }
                    _db.Remove(team);
                    await _db.SaveChangesAsync();
                }

                return new Response();
            }
        }
    }
}