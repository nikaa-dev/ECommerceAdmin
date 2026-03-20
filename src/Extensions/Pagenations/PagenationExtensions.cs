using Microsoft.EntityFrameworkCore;

namespace src.Extensions.Pagenations;

public static class PagenationExtensions
{
    public static PagedResult<T> ToPagedResultAsync<T>(this IQueryable<T> source, int page , int pageSize)
    {

        var count = source.Count();
        var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        
        return new PagedResult<T>(items, page, pageSize, count);
    }
}