using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class MicroTeamType : ObjectGraphType<TeamModel>
    {
        public MicroTeamType(MicroData data)
        {
            Name = "Team";

            Field(h => h.Id).Description("The id of the Team.");
            Field(h => h.Name, nullable: true).Description("The name of the Team.");
            Field<ListGraphType<MicroUserType>>("Users", "The users that belong to the team",
                resolve: context => data.ListUsersByTeamAsync(context.Source.Id));
        }
    }
}
