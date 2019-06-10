using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Micro.Services.Auth.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserApi _api;
        private readonly ILogger<ProfileService> _log;

        public ProfileService(IUserApi api, ILogger<ProfileService> log)
        {
            _api = api;
            _log = log;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            _log.LogInformation("GetProfileDataAsync");
            var sub = context.Subject.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var id = int.Parse(sub);

            var claims = new List<Claim>();

            // User Claims
            var user = await _api.GetUserAsync(id);
            claims.Add(new Claim(Claims.UserIdClaim, user.Id.ToString()));
            claims.Add(new Claim(Claims.TenantIdClaim, user.TenantId.ToString()));

            // Permission Claims
            var permissions = await _api.GetUserPermissions(id);
            foreach (var permission in permissions)
            {
                claims.Add(new Claim(Claims.PermissionClaim, permission));
            }
            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            _log.LogInformation("IsActiveAsync");
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
