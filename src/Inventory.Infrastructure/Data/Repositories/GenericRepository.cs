using System.Linq.Expressions;
using Inventory.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly InventoryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(InventoryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        // By changing State to Deleted here, EF will trigger the DbContext override SaveChangesAsync,
        // which converts Deleted state to Modified and sets IsDeleted = true.
        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<(IReadOnlyList<T> Items, int TotalCount)> GetPaginatedAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        int pageNumber,
        int pageSize,
        string[]? includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        int totalCount = await query.CountAsync();

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var items = await query.Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return (items, totalCount);
    }
}
