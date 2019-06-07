using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Constants;
using static Micro.Services.Tenants.Domain.Tenants.Create;

namespace Micro.Services.Tenants.Domain.Tenants
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Create a tenant
        /// </summary>
        /// <param name="command">the command</param>
        /// <returns>a tenant</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody] Request command) => new JsonResult(await _mediator.Send(command));
    }

    public static class Create
    {
        public class Request : IRequest<Response>
        {
            public string Name { get; set; }
            public string Host { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public TenantModel Tenant { get; set; }
            public UserModel User { get; set; }
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
                // https://github.com/dotnet/docs/blob/master/docs/standard/modern-web-apps-azure-architecture/work-with-data-in-asp-net-core-apps.md
                var strategy = _db.Database.CreateExecutionStrategy();
                return await strategy.ExecuteAsync<Response>(async () =>
                {
                    return await ProvisionTenant(request);
                });
            }

            private async Task<Response> ProvisionTenant(Request request)
            {
                using (var transaction = await _db.Database.BeginTransactionAsync())
                {
                    var tenant = new Tenant
                    {
                        Name = request.Name,
                        Host = request.Host
                    };
                    await _db.Tenants.AddAsync(tenant);
                    await _db.SaveChangesAsync();

                    var user = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Password = request.Password,
                        TenantId = tenant.Id
                    };
                    await _db.Users.AddAsync(user);

                    var team = new Team
                    {
                        Name = Constants.Teams.DefaultAdministratorsTeam,
                        TenantId = tenant.Id
                    };
                    await _db.Teams.AddAsync(team);

                    var role = new Role
                    {
                        Name = Constants.Teams.DefaultAdministratorsTeam,
                        TenantId = tenant.Id
                    };
                    await _db.Roles.AddAsync(role);

                    var permissions = PermissionHelper.AllPermissions.Select(name => new RolePermission
                    {
                        Role = role,
                        Name = name
                    });
                    await _db.RolePermissions.AddRangeAsync(permissions);

                    await _db.SaveChangesAsync();

                    await _db.TeamRoles.AddAsync(new TeamRole
                    {
                        Role = role,
                        Team = team,
                        TenantId = tenant.Id
                    });
                    await _db.UserTeams.AddAsync(new UserTeam
                    {
                        User = user,
                        Team = team,
                        TenantId = tenant.Id
                    });

                    await _db.SaveChangesAsync();

                    transaction.Commit();

                    return new Response
                    {
                        Tenant = _map.Map<Tenant, TenantModel>(tenant),
                        User = _map.Map<User, UserModel>(user)
                    };
                }
            }
        }
    }
}
