using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Micro.Services.Gateway.GraphQL
{
    public class CustomUserContext
    {
        public ClaimsPrincipal User { get; set; }
        public IHeaderDictionary Headers { get; set; }
    }
}
