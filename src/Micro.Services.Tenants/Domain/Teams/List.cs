using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static Micro.Services.Tenants.Domain.Teams.List;

namespace Micro.Services.Tenants.Domain.Teams
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// List teams
        /// </summary>
        /// <returns>a list of teams</returns>
        [HttpGet]
        public async Task<ActionResult<Response>> ListAsync() => Ok(await _mediator.Send(new Request()));
    }

    public static class List
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public TeamModel[] Teams { get; set; }
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
                var data = await _db.Teams
                    .AsNoTracking()
                    .ToArrayAsync();

                return new Response
                {
                    Teams = _mapper.Map<Team[], TeamModel[]>(data)
                };
            }
        }
    }
}