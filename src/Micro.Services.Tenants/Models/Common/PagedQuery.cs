using System;
using MediatR;

namespace Micro.Services.Tenants.Models.Common
{
    public abstract class PagedRequest<TEnvelope> : IRequest<TEnvelope>
    {
        protected PagedRequest(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            if (pageSize > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int PageSize { get; }
    }
}