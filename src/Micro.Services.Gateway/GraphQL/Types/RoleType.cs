using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class RoleType : ObjectGraphType<RoleModel>
    {
        public RoleType(ITenantsApi api)
        {
            Name = "Role";

            Field(h => h.Id).Description("The id of the role.");

            Field(h => h.Name, nullable: true).Description("The name of the role.");

            Field<ListGraphType<StringGraphType>>("Permissions", "The permissions belonging to this role",
                resolve: context => api.ListPermissionsByRoleAsync(context.Source.Id));

            Field<ListGraphType<TeamType>>("Teams", "The teams that have this role",
                resolve: context => api.ListTeamsByRoleAsync(context.Source.Id));
        }
    }
}
