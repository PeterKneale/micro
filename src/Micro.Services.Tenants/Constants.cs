using System.Linq;

namespace Micro.Services.Tenants
{
    public static class Constants
    {
        public static class Claims
        {
            public const string TenantIdClaim = "tenant_id";
            public const string UserIdClaim = "user_id";
            public const string PermissionClaim = "permission";
        }

        public class Roles
        {
            public const string DefaultAdministratorRole = "administrator";
        }

        public class Teams
        {
            public const string DefaultAdministratorsTeam = "administrators";
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

            public const string ContentCreate = "content.create";
            public const string ContentDelete = "content.delete";
            public const string ContentEdit = "content.edit";
            public const string ContentView = "content.view";
        }

        public static class PermissionHelper
        {
            public static string[] AllPermissions => new string[] { }
                .Union(AllTeamPermissions)
                .Union(AllUserPermissions)
                .Union(AllContentPermissions)
                .ToArray();

            public static string[] AllTeamPermissions => new string[] { Permissions.TeamCreate, Permissions.TeamDelete, Permissions.TeamEdit, Permissions.TeamView };

            public static string[] AllUserPermissions => new string[] { Permissions.UserCreate, Permissions.UserDelete, Permissions.UserEdit, Permissions.UserView };

            public static string[] AllContentPermissions => new string[] { Permissions.ContentCreate, Permissions.ContentDelete, Permissions.ContentEdit, Permissions.ContentView };
        }
    }
}
