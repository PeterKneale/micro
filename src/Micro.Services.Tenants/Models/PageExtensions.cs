using System.Linq;

namespace Micro.Services.Tenants.Models.Common
{
    public static class PageExtensions
    {
        public static IQueryable<T> TakePage<T>(this IQueryable<T> source, int page, int pageSize)
        {
            var offset = CalculateOffset(page, pageSize);
            return source.Skip(offset).Take(pageSize);
        }

        public static IQueryable<T> TakePage<T>(this IQueryable<T> source, Page page)
        {
            return source.TakePage(page.PageNumber, page.PageSize);
        }

        private static int CalculateOffset(int page, int pageSize)
        {
            return (page * pageSize) - pageSize;
        }
    }
}