using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Domain.Roles
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// List roles
        /// </summary>
        /// <returns>a list of roles</returns>
        [HttpGet]
        public async Task<ActionResult<List.Response>> List() => Ok(await _mediator.Send(new List.Request()));
    }

    public static class List
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public RoleModel[] Roles { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly TenantDbContext _db;
            private readonly IMapper _mapper;

            public Handler(TenantDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var data = await _db.Roles
                    .AsNoTracking()
                    .ToArrayAsync();

                return new Response
                {
                    Roles = _mapper.Map<Role[], RoleModel[]>(data)
                };
            }
        }
    }
}