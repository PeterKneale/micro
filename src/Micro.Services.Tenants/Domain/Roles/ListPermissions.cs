using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Roles.ListPermissions;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get role permissions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a list of permissions</returns>
        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<Response>> ListPermissionsAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class ListPermissions
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {
            public string[] Permissions { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly TenantDbContext _db;

            public Handler(TenantDbContext db)
            {
                _db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var id = request.Id;

                var role = await _db
                    .Roles
                    .Include(u => u.RolePermissions)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (role == null)
                {
                    throw new NotFoundException("role", "id", id);
                }

                var permissions = role.RolePermissions
                    .Select(x => x.Name)
                    .ToArray();

                return new Response
                {
                    Permissions = permissions
                };
            }
        }
    }
}