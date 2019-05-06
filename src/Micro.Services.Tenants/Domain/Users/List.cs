using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Data;
using System.Threading;
using System.Threading.Tasks;
using Micro.Services.Tenants.Models;

namespace Micro.Services.Tenants.Domain.Users
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Lists users
        /// </summary>
        /// <param name="page">page number</param>
        /// <returns>a list of users</returns>
        [HttpGet]
        public async Task<IActionResult> List(int page = 1) => Ok(await _mediator.Send(new List.Request()));
    }

    public static class List
    {
        public class Request : IRequest<Response>
        {
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
                var data = await _db.Users
                    .AsNoTracking()
                    .ToArrayAsync();

                return new Response
                {
                    Users = _mapper.Map<User[], UserModel[]>(data)
                };
            }
        }
    }
}