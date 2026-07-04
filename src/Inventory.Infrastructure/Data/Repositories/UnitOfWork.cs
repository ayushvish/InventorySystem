using Inventory.Application.Common.Interfaces;

namespace Inventory.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InventoryDbContext _context;
    private readonly Dictionary<string, object> _repositories = new();
    private bool _disposed;

    public UnitOfWork(InventoryDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
            _repositories.Add(type, repositoryInstance!);
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
