using System.Security.Claims;

namespace Micro.Services.Gateway.GraphQL
{
    public class UserContext
    {
        public ClaimsPrincipal User { get; set; }
    }
}
