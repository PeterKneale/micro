using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static Micro.Services.Tenants.Domain.Teams.Get;

namespace Micro.Services.Tenants.Domain.Teams
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get team
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a team</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> Get(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class Get
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {
            public TeamModel Team { get; set; }
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
                var id = request.Id;

                var team = await _db
                    .Teams
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (team == null)
                {
                    throw new NotFoundException("user","id", id);
                }

                return new Response
                {
                    Team = _mapper.Map<Team, TeamModel>(team)
                };
            }
        }
    }
}