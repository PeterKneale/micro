using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Micro.Services.Tenants.Domain.Teams.Create;

namespace Micro.Services.Tenants.Domain.Teams
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Create a team
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>a team</returns>
        [HttpPost]
        public async Task<ActionResult<Response>> PostAsync(Request request) => Ok(await _mediator.Send(request));
    }

    public static class Create
    {
        public class Request : IRequest<Response>
        {
            public string Name { get; set; }
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
                var name = request.Name;

                using (var transaction = await _db.Database.BeginTransactionAsync())
                {
                    if(_db.Teams.Any(x=>x.Name == name))
                    {
                        throw new NotUniqueException("team", "name", name);
                    }

                    var data = new Team
                    {
                        Name = name
                    };
                    await _db.Teams.AddAsync(data);

                    await _db.SaveChangesAsync();

                    transaction.Commit();

                    return new Response
                    {
                        Team = _mapper.Map<Team, TeamModel>(data)
                    };
                }
            }
        }
    }
}