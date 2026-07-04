using System.Linq.Expressions;

namespace Inventory.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<(IReadOnlyList<T> Items, int TotalCount)> GetPaginatedAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        int pageNumber,
        int pageSize,
        string[]? includes = null);
}
