using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Commands
{
    public class DeleteTenantCommand : IRequest<DeleteTenantResult>
    {
        public DeleteTenantCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class DeleteTenantResult
    {
        
    }

    public class DeleteTenantHandler : IRequestHandler<DeleteTenantCommand, DeleteTenantResult>
    {
        private readonly DatabaseContext _db;

        public DeleteTenantHandler(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<DeleteTenantResult> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            // Load
            var data = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new NotFoundException("tenant", "id", id);
            }

            // Delete
            _db.Tenants.Remove(data);
            await _db.SaveChangesAsync();

            return new DeleteTenantResult();
        }
    }
}
