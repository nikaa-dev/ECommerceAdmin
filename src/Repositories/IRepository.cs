using src.Models;
using System.Linq.Expressions;

namespace src.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task SaveAsync();
	Task<int> CountAsync();
	Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate);


}