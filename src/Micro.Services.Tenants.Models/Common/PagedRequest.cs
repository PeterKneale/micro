using MediatR;

namespace Micro.Services.Tenants.Models.Common
{
    public class PagedRequest<T> : IRequest<T>
    {
        /// <summary>
        /// Parameterless constructor required for binding to querystring
        /// </summary>
        public PagedRequest() : this(1, 10)
        {
        }

        public PagedRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public static implicit operator Page(PagedRequest<T> request)
        {
            return new Page(request.PageNumber, request.PageSize);
        }
    }
}