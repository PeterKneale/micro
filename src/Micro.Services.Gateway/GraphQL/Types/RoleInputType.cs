using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class RoleInputType : InputObjectGraphType<RoleModel>
    {
        public RoleInputType()
        {
            Name = "RoleInput";
            Field(x => x.Name);
        }
    }
}
