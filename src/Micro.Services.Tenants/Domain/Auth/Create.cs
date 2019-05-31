using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static Micro.Services.Tenants.Domain.Auth.Create;

namespace Micro.Services.Tenants.Domain.Auth
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>a token</returns>
        [HttpPost]
        public async Task<ActionResult<Response>> Post(Request request) => Ok(await _mediator.Send(request));
    }

    public static class Create
    {
        public class Request : IRequest<Response>
        {
            public string Email { get; set; }
        }

        public class Response
        {
            public string Token { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly GlobalDbContext _db;
            private readonly IConfiguration _configuration;
            private readonly ILogger<Handler> _log;

            public Handler(GlobalDbContext db, IConfiguration configuration, ILogger<Handler> log)
            {
                _db = db;
                _configuration = configuration;
                _log = log;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var email = request.Email;

                var user = await _db.Users
                    .SingleOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    throw new NotFoundException("user", "email", email);
                }

                _log.LogInformation("Authenticating user {UserId} from tenant {TenantId}", user.Id, user.TenantId);

                // generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetAuthSecret());
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(Constants.Claims.UserIdClaim, user.Id.ToString()),
                        new Claim(Constants.Claims.TenantIdClaim, user.TenantId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                
                // find permissions
                var permissions = await _db.UserTeams
                    .Where(x => x.UserId == user.Id)
                    .Select(t => t.Team)
                    .SelectMany(t => t.TeamRoles)
                    .Select(p => p.Role)
                    .SelectMany(x => x.RolePermissions)
                    .Select(rp => rp.Name)
                    .ToListAsync();

                _log.LogInformation("Issuing user {UserId} permissions {Permissions}", user.Id, permissions);

                foreach (var permission in permissions)
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(Constants.Claims.PermissionClaim, permission));
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new Response
                {
                    Token = tokenHandler.WriteToken(token)
                };
            }
        }
    }
}