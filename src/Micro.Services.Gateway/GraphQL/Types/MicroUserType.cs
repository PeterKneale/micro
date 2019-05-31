using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class MicroUserType : ObjectGraphType<UserModel>
    {
        public MicroUserType(MicroData data)
        {
            Name = "User";

            Field(h => h.Id).Description("The id of the user");
            Field(h => h.Name, nullable: true).Description("The name of the user");
            Field<ListGraphType<MicroTeamType>>("Teams", "The teams the user belongs to",
                resolve: context => data.ListTeamsByUserAsync(context.Source.Id));
        }
    }
}
