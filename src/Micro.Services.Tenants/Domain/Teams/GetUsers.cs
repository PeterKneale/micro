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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Domain.Teams
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Get team users
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a a list of users in a team</returns>
        [HttpGet("{id}/users")]
        public async Task<ActionResult<GetUsers.Response>> GetUsers(int id) => Ok(await _mediator.Send(new GetUsers.Request(id)));
    }

    public static class GetUsers
    {
        public class Request : IdRequest<Response>
        {
            public Request(int id) : base(id)
            {
            }
        }

        public class Response
        {
            public UserModel[] Users { get; set; }
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
                    .Include(u => u.UserTeams)
                    .ThenInclude(x=>x.User)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (team == null)
                {
                    throw new NotFoundException("team", "id", id);
                }

                var users = team.UserTeams
                    .Select(x => x.User)
                    .ToArray();

                return new Response
                {
                    Users = _mapper.Map<User[], UserModel[]>(users)
                };
            }
        }
    }
}