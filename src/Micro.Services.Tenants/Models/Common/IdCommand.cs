using MediatR;

namespace Micro.Services.Tenants.Models.Common
{
    public abstract class IdCommand : IRequest
    {
        protected IdCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}