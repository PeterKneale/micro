using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Commands
{
    public class UpdateTenantCommand : IRequest<UpdateTenantResult>
    {
        public UpdateTenantCommand(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }

    public class UpdateTenantResult
    {
        public TenantModel Tenant { get; set; }
    }

    public class UpdateTenantHandler : IRequestHandler<UpdateTenantCommand, UpdateTenantResult>
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _map;

        public UpdateTenantHandler(DatabaseContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task<UpdateTenantResult> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var name = request.Name;

            // Load tenant
            var data = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new NotFoundException("tenant", "id", id);
            }

            // Check new name is unique
            var exists = await _db.Tenants.AnyAsync(x => x.Name == name && x.Id != id);
            if (exists)
            {
                throw new NotUniqueException("tenant", "name", name);
            }

            // Save tenant
            data.Name = name;
            _db.Tenants.Update(data);
            await _db.SaveChangesAsync();

            // Load tenant
            var tenant = await _db.Tenants.SingleAsync(x => x.Id == data.Id);
            return new UpdateTenantResult
            {
                Tenant = _map.Map<TenantModel>(tenant)
            };
        }
    }
}
