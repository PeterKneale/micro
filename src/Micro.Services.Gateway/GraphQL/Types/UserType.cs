using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class UserType : ObjectGraphType<UserModel>
    {
        public UserType(ITenantsApi api)
        {
            Name = "User";

            Field(h => h.Id).Description("The id of the user");

            Field(h => h.Name, nullable: true).Description("The name of the user");

            Field(h => h.FirstName, nullable: true).Description("The first name of the user");

            Field(h => h.LastName, nullable: true).Description("The last name of the user");

            Field<ListGraphType<TeamType>>("Teams", "The teams the user belongs to",
                resolve: context => api.ListTeamsByUserAsync(context.Source.Id));
        }
    }
}
