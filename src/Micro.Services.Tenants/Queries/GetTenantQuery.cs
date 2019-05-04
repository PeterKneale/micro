using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Queries
{
    public class GetTenantQuery : IRequest<GetTenantResult>
    {
        public GetTenantQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class GetTenantResult
    {
        public TenantModel Tenant { get; set; }
    }

    public class GetTenantHandler : IRequestHandler<GetTenantQuery, GetTenantResult>
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _map;

        public GetTenantHandler(DatabaseContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task<GetTenantResult> Handle(GetTenantQuery request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            // Load tenant
            var data = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new NotFoundException("tenant", "id", id);
            }

            // Map to model
            var model = _map.Map<TenantModel>(data);

            return new GetTenantResult
            {
                Tenant = model
            };
        }
    }
}
