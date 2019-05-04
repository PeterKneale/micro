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
    public class CreateTenantCommand : IRequest<CreateTenantResult>
    {
        public string Name { get; set; }
    }

    public class CreateTenantResult
    {
        public TenantModel Tenant { get; set; }
    }

    public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, CreateTenantResult>
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _map;

        public CreateTenantHandler(DatabaseContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task<CreateTenantResult> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name;

            // Check name is unique
            var exists = await _db.Tenants.AnyAsync(x => x.Name == name);
            if (exists)
            {
                throw new NotUniqueException("tenant", "name", name);
            }
            
            // Save tenant
            var data = new TenantData { Name = name };
            await _db.Tenants.AddAsync(data);
            await _db.SaveChangesAsync();
            
            // Load tenant
            return new CreateTenantResult
            {
                Tenant = _map.Map<TenantModel>(await _db.Tenants.SingleAsync(x => x.Id == data.Id))
            };
        }
    }
}
