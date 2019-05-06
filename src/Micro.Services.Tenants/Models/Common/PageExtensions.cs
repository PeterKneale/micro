using System.Linq;

namespace Micro.Services.Tenants.Models.Common
{
    public static class PageExtensions
    {
        public static IQueryable<TSource> TakePage<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip(CalculateOffset(page, pageSize)).Take(pageSize);
        }

        private static int CalculateOffset(int page, int pageSize)
        {
            return (page * pageSize) - pageSize;
        }
    }
}