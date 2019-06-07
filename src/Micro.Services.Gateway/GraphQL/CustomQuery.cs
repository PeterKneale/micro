using GraphQL.Types;
using Micro.Services.Gateway.GraphQL.Types;

namespace Micro.Services.Gateway.GraphQL
{
    public class CustomQuery : ObjectGraphType<object>
    {
        public CustomQuery(ITenantsApi data)
        {
            Name = nameof(CustomQuery);

            Field<UserType>(
                "User",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the User" }
                ),
                resolve: context => data.GetUserAsync(context.GetArgument<string>("id")));

            Field<TeamType>(
                "Team",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the Team" }
                ),
                resolve: context => data.GetTeamAsync(context.GetArgument<string>("id"))
            );

            Field<RoleType>(
                "Role",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the Role" }
                ),
                resolve: context => data.GetRoleAsync(context.GetArgument<string>("id"))
            );

            Field< ListGraphType<TeamType>>(
                "Teams",
                resolve: context => data.ListTeamsAsync()
            );

            Field<ListGraphType<UserType>>(
                "Users",
                resolve: context => data.ListUsersAsync()
            );

            Field<ListGraphType<RoleType>>(
                "Roles",
                resolve: context => data.ListRolesAsync()
            );
        }
    }
}
