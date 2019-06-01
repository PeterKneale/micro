using GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL.Types
{
    public class UserInputType : InputObjectGraphType<UserModel>
    {
        public UserInputType()
        {
            Name = "UserInput";
            Field(x => x.Name);
        }
    }
}
