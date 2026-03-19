using Microsoft.EntityFrameworkCore;

namespace src.Extensions.Pagenations;

public static class PagenationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, int page, int pageSize)
    {
        page = 1;
        pageSize = 10;

        var count = await source.CountAsync();
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PagedResult<T>(items, page, pageSize, count);
    }
}