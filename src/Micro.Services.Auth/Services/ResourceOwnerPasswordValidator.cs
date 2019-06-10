using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Micro.Services.Auth.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserApi _api;
        private readonly ILogger<ResourceOwnerPasswordValidator> _log;

        public ResourceOwnerPasswordValidator(IUserApi api, ILogger<ResourceOwnerPasswordValidator> log)
        {
            _api = api;
            _log = log;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            _log.LogInformation("ValidateAsync");

            var username = context.UserName;
            var password = context.Password;

            var user = await _api.AuthAsync(username, password);

            var subject = user.Id.ToString();

            context.Result = new GrantValidationResult(subject, AuthenticationMethods.Password);
        }
    }

    public static class Claims
    {
        public const string TenantIdClaim = "tenant_id";
        public const string UserIdClaim = "user_id";
        public const string PermissionClaim = "permission";
    }

}
