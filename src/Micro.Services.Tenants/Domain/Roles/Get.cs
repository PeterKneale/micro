using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get role
        /// </summary>
        /// <returns>a role</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModel>> GetAsync([FromRoute]IdRequest<RoleModel> request) => Ok(await _mediator.Send(request));
    }

    public static class Get
    {
        public class Handler : IRequestHandler<IdRequest<RoleModel>, RoleModel>
        {
            private readonly TenantDbContext _db;
            private readonly IMapper _mapper;

            public Handler(TenantDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<RoleModel> Handle(IdRequest<RoleModel> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var id = request.Id;

                var role = await _db
                    .Roles
                    .Include(x=>x.RolePermissions)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (role == null)
                {
                    throw new NotFoundException("role","id", id);
                }

                return _mapper.Map<Role, RoleModel>(role);
            }
        }
    }
}