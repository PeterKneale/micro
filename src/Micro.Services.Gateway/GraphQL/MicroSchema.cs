using GraphQL;
using GraphQL.Types;

namespace Micro.Services.Gateway.GraphQL
{
    public class MicroSchema : Schema
    {
        public MicroSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<MicroQuery>();
            Mutation = resolver.Resolve<MicroMutation>();
        }
    }
}
