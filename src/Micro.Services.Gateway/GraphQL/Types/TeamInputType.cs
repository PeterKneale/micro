using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class TeamInputType : InputObjectGraphType<TeamModel>
    {
        public TeamInputType()
        {
            Name = "TeamInput";
            Field(x => x.Name);
        }
    }
}
