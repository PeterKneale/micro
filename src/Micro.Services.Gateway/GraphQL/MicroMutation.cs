using GraphQL.Types;
using Micro.Services.Gateway.GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public class MicroMutation : ObjectGraphType
    {
        public MicroMutation(MicroData data)
        {
            Name = "Mutation";

            Field<MicroUserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MicroUserInputType>> { Name = "User" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<UserModel>("User");
                    return data.CreateUser(user);
                });
        }
    }
}
