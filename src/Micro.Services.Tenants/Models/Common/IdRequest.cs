using MediatR;

namespace Micro.Services.Tenants.Models.Common
{
    public abstract class IdRequest<TResponse> : IRequest<TResponse>
    {
        protected IdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}