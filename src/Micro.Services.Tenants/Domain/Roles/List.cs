using AutoMapper;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// List roles
        /// </summary>
        /// <returns>a list of roles</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<RoleModel>>> List([FromQuery]PagedRequest<RoleModel> request) => Ok(await _mediator.Send(request));
    }

    public static class List
    {
        public class Handler : IRequestHandler<PagedRequest<RoleModel>, PagedResponse<RoleModel>>
        {
            private readonly TenantDbContext _db;
            private readonly IMapper _mapper;

            public Handler(TenantDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<PagedResponse<RoleModel>> Handle(PagedRequest<RoleModel> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var data = await _db.Roles
                    .Include(x=>x.RolePermissions)
                    .OrderBy(x=>x.Name)
                    .TakePage(request)
                    .AsNoTracking()
                    .ToListAsync();

                var total = await _db.Roles.CountAsync();

                var models = _mapper.Map<RoleModel[]>(data);

                return new PagedResponse<RoleModel>(models, request.PageNumber, request.PageSize, total);
            }
        }
    }
}