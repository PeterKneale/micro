using GraphQL.Types;
using Micro.Services.Gateway.GraphQL.Types;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public class CustomMutation : ObjectGraphType
    {
        public CustomMutation(ITenantsApi data)
        {
            Name = "Mutation";

            Field<UserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "User" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<UserModel>("User");
                    return data.CreateUser(user);
                });
        }
    }
}
