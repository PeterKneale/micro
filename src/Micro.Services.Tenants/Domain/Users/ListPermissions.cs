using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Micro.Services.Tenants.Domain.Users.ListPermissions;

namespace Micro.Services.Tenants.Domain.Users
{
    public partial class Api : ControllerBase
    {
        [HttpGet("{id}/permissions")]
        [AllowAnonymous]
        public async Task<ActionResult<string[]>> ListPermissionsAsync([FromRoute]Request request) => Ok(await _mediator.Send(request));
    }

    public static class ListPermissions
    {
        public class Request : IRequest<string[]>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, string[]>
        {
            private readonly GlobalDbContext _db;

            public Handler(GlobalDbContext db)
            {
                _db = db;
            }

            public async Task<string[]> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var id = request.Id;

                var user = await _db.Users
                    .Include(c => c.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    throw new NotFoundException("user", "id", id);
                }

                // get permissions
                return await _db.UserTeams
                    .Where(x => x.UserId == user.Id)
                    .Select(t => t.Team)
                    .SelectMany(t => t.TeamRoles)
                    .Select(p => p.Role)
                    .SelectMany(x => x.RolePermissions)
                    .Select(rp => rp.Name)
                    .ToArrayAsync();
            }
        }
    }
}