using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Micro.Services.Tenants.Services
{
    public class RequestContext : IUserContext, ITenantContext
    {
        private readonly IHttpContextAccessor _http;

        public RequestContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public bool IsAuthenticated => _http.HttpContext.User.Identity.IsAuthenticated;

        public int TenantId => GetIdClaim(_http.HttpContext.User, Constants.Claims.TenantIdClaim);

        public int UserId => GetIdClaim(_http.HttpContext.User, Constants.Claims.UserIdClaim);

        private static int GetIdClaim(ClaimsPrincipal user, string type)
        {
            var claim = user.Claims.SingleOrDefault(x => x.Type == type);
            if (claim == null)
            {
                throw new Exception($"No {type} claim found");

            }
            var id = int.Parse(claim.Value);
            return id;
        }
    }
}
