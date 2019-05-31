using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public class MicroTeamInputType : InputObjectGraphType<TeamModel>
    {
        public MicroTeamInputType()
        {
            Name = "TeamInput";
            Field(x => x.Name);
        }
    }
}
