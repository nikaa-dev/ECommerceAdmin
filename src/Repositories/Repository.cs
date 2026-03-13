using src.DBConnection;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace src.Repositories;

public class Repository<T>(ApplicationDbContext context):IRepository<T> where T:class
{
    public async Task<T> GetByIdAsync(Guid id) => await context.Set<T>().FindAsync(id);

    public async Task<List<T>> GetAllAsync() => await context.Set<T>().ToListAsync();

    public async Task CreateAsync(T entity) => await context.Set<T>().AddAsync(entity);
    
   public async Task UpdateAsync(T entity)
	{
    	context.Set<T>().Update(entity);
	}


    public async Task<bool> DeleteAsync(Guid id)
	{
    	var entity = await GetByIdAsync(id);
    	if (entity == null) return false;

    	context.Set<T>().Remove(entity);
    	return true;
	}


    public async Task<int> CountAsync() => await context.Set<T>().CountAsync();

	public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) 
			=> await context.Set<T>().CountAsync(predicate);

    public async Task SaveAsync()
	{
		await context.SaveChangesAsync();
	}

    public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await context.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Where(predicate)
            .OrderBy(e => EF.Property<object>(e, "Id"))
            .ToListAsync();
    }
}