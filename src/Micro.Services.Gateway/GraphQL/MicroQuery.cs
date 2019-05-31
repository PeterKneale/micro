using GraphQL.Types;
using Micro.Services.Gateway.GraphQL.Types;

namespace Micro.Services.Gateway.GraphQL
{
    public class MicroQuery : ObjectGraphType<object>
    {
        public MicroQuery(MicroData data)
        {
            Name = "Query";

            Field<MicroUserType>(
                "User",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the User" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("id");
                    return data.GetUserAsync(id);
                }
            );

            Field<MicroTeamType>(
                "Team",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the Team" }
                ),
                resolve: context => data.GetTeamAsync(context.GetArgument<string>("id"))
            );
        }
    }
}
