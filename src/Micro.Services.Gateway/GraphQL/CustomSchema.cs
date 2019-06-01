using GraphQL;
using GraphQL.Types;

namespace Micro.Services.Gateway.GraphQL
{
    public class CustomSchema : Schema
    {
        public CustomSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<CustomQuery>();
            Mutation = resolver.Resolve<CustomMutation>();
        }
    }
}
