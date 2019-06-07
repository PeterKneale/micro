using MediatR;

namespace Micro.Services.Tenants.Models.Common
{
    public class IdRequest<TResponse> : IRequest<TResponse>
    {
        /// <summary>
        /// Parameterless constructor required for binding to querystring
        /// </summary>
        public IdRequest()
        {
        }

        protected IdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}