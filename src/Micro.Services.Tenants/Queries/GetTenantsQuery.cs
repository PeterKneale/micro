using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Queries
{
    public class GetTenantsQuery : IRequest<GetTenantsResult>
    {
    }

    public class GetTenantsResult
    {
        public IEnumerable<TenantModel> Tenants { get; set; }
    }

    public class GetTenantsHandler : IRequestHandler<GetTenantsQuery, GetTenantsResult>
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _map;

        public GetTenantsHandler(DatabaseContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task<GetTenantsResult> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            // Load tenant
            var data = await _db.Tenants.ToListAsync();
            
            // Map to model
            var model = _map.Map<IEnumerable<TenantModel>>(data);

            return new GetTenantsResult
            {
                Tenants = model
            };
        }
    }
}
