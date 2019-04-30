using System;
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

            await CheckUnique(name);

            var id = await Save(name);

            var data = await Load(id);

            var model = Map(data);

            return new CreateTenantResult
            {
                Tenant = model
            };
        }
        
        private async Task CheckUnique(string name)
        {
            var exists = await _db.Tenants.AnyAsync(x => x.Name == name);
            if (exists)
            {
                throw new NotUniqueException("tenant", "name", name);
            }
        }

        private async Task<int> Save(string name)
        {
            var data = new TenantData { Name = name };
            await _db.Tenants.AddAsync(data);
            await _db.SaveChangesAsync();
            return data.Id;
        }

        private async Task<TenantData> Load(int id)
        {
            return await _db.Tenants.SingleAsync(x => x.Id == id);
        }

        private TenantModel Map(TenantData data)
        {
            return _map.Map<TenantModel>(data);
        }
    }
}
