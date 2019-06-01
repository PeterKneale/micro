using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class TeamType : ObjectGraphType<TeamModel>
    {
        public TeamType(ITenantsApi api)
        {
            Name = "Team";

            Field(h => h.Id).Description("The id of the Team.");
            Field(h => h.Name, nullable: true).Description("The name of the Team.");
            Field<ListGraphType<UserType>>("Users", "The users that belong to the team",
                resolve: context => api.ListUsersByTeamAsync(context.Source.Id));
        }
    }
}
