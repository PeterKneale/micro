using AutoMapper;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static Micro.Services.Tenants.Domain.Users.List;

namespace Micro.Services.Tenants.Domain.Users
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Lists users
        /// </summary>
        /// <returns>a list of users</returns>
        [HttpGet]
        public async Task<ActionResult<Response>> ListAsync([FromQuery]Request request)
        {
            return Ok(await _mediator.Send(request));
        }
    }

    public static class List
    {
        public class Request : PagedRequest<Response>
        {
        }

        public class Response : PagedResponse<UserModel>
        {
            public Response(UserModel[] items, int pageNumber, int pageSize, int total) : base(items, pageNumber, pageSize, total)
            {
            }
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
                    .TakePage(request)
                    .ToArrayAsync();

                var total = await _db.Users.CountAsync();

                var models = _mapper.Map<UserModel[]>(data);

                return new Response(models, request.PageNumber, request.PageSize, total);
            }
        }
    }
}