using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using static Micro.Services.Tenants.Domain.Users.Create;

namespace Micro.Services.Tenants.Domain.Users
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>a user</returns>
        [HttpPost]
        public async Task<ActionResult<Response>> PostAsync(Request request)
        {
            return Ok(await _mediator.Send(request));
        }
    }

    public static class Create
    {
        public class Request : IRequest<Response>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class Response
        {
            public UserModel User { get; set; }
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
                using (var transaction = await _db.Database.BeginTransactionAsync())
                {
                    var user = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Password = Guid.NewGuid().ToString()
                    };
                    await _db.Users.AddAsync(user);

                    await _db.SaveChangesAsync();

                    transaction.Commit();

                    return new Response
                    {
                        User = _mapper.Map<User, UserModel>(user)
                    };
                }
            }
        }
    }
}