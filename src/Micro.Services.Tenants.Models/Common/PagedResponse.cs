using System.Collections.Generic;

namespace Micro.Services.Tenants.Models.Common
{
    public class PagedResponse<T>
    {
        public PagedResponse(T[] items, int pageNumber, int pageSize, int total)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = total;
            TotalPages = (total + pageSize - 1) / pageSize;
        }

        public IReadOnlyList<T> Items { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public int TotalPages { get; }

        public static implicit operator Page(PagedResponse<T> response)
        {
            return new Page(response.PageNumber, response.PageSize);
        }
    }
}