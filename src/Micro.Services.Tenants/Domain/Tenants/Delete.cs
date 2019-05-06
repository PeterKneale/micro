using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Micro.Services.Tenants.DataContext;
using Micro.Services.Tenants.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Micro.Services.Tenants.Domain.Tenants.Delete;

namespace Micro.Services.Tenants.Domain.Tenants
{
    public partial class Api : ControllerBase
    {
        /// <summary>
        /// Delete team
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>a team</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAsync(int id) => Ok(await _mediator.Send(new Request(id)));
    }

    public static class Delete
    {
        public class Request : IRequest<Result>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Result
        {

        }

        public class Handler : IRequestHandler<Request, Result>
        {
            private readonly GlobalDbContext _db;

            public Handler(GlobalDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var id = request.Id;

                // Load
                var data = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    throw new NotFoundException("tenant", "id", id);
                }

                // Delete
                _db.Tenants.Remove(data);
                await _db.SaveChangesAsync();

                return new Result();
            }
        }
    }
}
