using System.Linq;

namespace Micro.Services.Tenants
{
    public static class Constants
    {
        public static class Claims
        {
            public const string TenantIdClaim = "tenant_id";
            public const string UserIdClaim = "user_id";
            public const string TeamIdClaim = "team_id";

            public const string ProvisionPolicy = "provision-policy";
        }

        public class Permissions
        {
            public const string UserCreate = "user.create";
            public const string UserDelete = "user.delete";
            public const string UserEdit = "user.edit";
            public const string UserView = "user.view";

            public const string TeamCreate = "team.create";
            public const string TeamDelete = "team.delete";
            public const string TeamEdit = "team.edit";
            public const string TeamView = "team.view";
        }

        public static class PermissionHelper
        {
            public static string[] GetAllPermissions()
            {
                return GetAllTeamPermissions().Union(GetAllUserPermissions()).ToArray();
            }

            public static string[] GetAllTeamPermissions()
            {
                return new string[] { Permissions.TeamCreate, Permissions.TeamDelete, Permissions.TeamEdit, Permissions.TeamView };
            }

            public static string[] GetAllUserPermissions()
            {
                return new string[] { Permissions.UserCreate, Permissions.UserDelete, Permissions.UserEdit, Permissions.UserView };
            }
        }
    }
}
