namespace Micro.Services.Tenants.Models.Common
{
    public abstract class PagedResult<T>
    {
        protected PagedResult(T[] items, int page, int pageSize, int total)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            Total = total;
        }

        public T[] Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int Total { get; }
    }
}