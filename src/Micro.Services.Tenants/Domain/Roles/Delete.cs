using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Teams.Delete;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Delete team
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a team</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class Delete
    {
        public class Request : IdRequest<EmptyResponse>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Handler : IRequestHandler<Request, EmptyResponse>
        {
            private readonly TenantDbContext _db;

            public Handler(TenantDbContext db)
            {
                _db = db;
            }

            public async Task<EmptyResponse> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var role = await _db
                    .Roles
                    .Include(x => x.TeamRoles)
                    .Include(x => x.RolePermissions)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (role != null)
                {
                    foreach (var o in role.TeamRoles)
                    {
                        _db.Remove(o);
                    }
                    foreach (var o in role.TeamRoles)
                    {
                        _db.Remove(o);
                    }
                    _db.Remove(role);
                    await _db.SaveChangesAsync();
                }

                return new EmptyResponse();
            }
        }
    }
}