using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using static Micro.Services.Tenants.Domain.Roles.Create;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Create a team
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>a team</returns>
        [HttpPost]
        public async Task<ActionResult<RoleModel>> PostAsync(Request request) => Ok(await _mediator.Send(request));
    }

    public static class Create
    {
        public class Request : IRequest<RoleModel>
        {
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Request, RoleModel>
        {
            private readonly TenantDbContext _db;
            private readonly IMapper _mapper;

            public Handler(TenantDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<RoleModel> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var name = request.Name;

                using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
                {
                    if(_db.Roles.Any(x=>x.Name == name))
                    {
                        throw new NotUniqueException("role", "role", name);
                    }

                    var data = new Role
                    {
                        Name = name
                    };
                    await _db.Roles.AddAsync(data, cancellationToken);

                    await _db.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return _mapper.Map<Role, RoleModel>(data);
                }
            }
        }
    }
}