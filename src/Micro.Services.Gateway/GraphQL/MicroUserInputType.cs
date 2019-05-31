using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public class MicroUserInputType : InputObjectGraphType<UserModel>
    {
        public MicroUserInputType()
        {
            Name = "UserInput";
            Field(x => x.Name);
        }
    }
}
