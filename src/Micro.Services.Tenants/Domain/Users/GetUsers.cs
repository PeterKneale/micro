using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Domain.Users
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get user teams
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>the teams a user belongs to</returns>
        [HttpGet("{id}/teams")]
        public async Task<ActionResult<ListTeams.Response>> GetTeams(int id) => Ok(await _mediator.Send(new ListTeams.Request(id)));
    }

    public static class ListTeams
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
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
                var id = request.Id;

                var user = await _db
                    .Users
                    .Include(u => u.UserTeams)
                    .ThenInclude(x=>x.Team)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    throw new NotFoundException("user", "id", id);
                }

                var teams = user.UserTeams
                    .Select(x => x.Team)
                    .ToArray();

                return new Response
                {
                    Teams = _mapper.Map<Team[], TeamModel[]>(teams)
                };
            }
        }
    }
}