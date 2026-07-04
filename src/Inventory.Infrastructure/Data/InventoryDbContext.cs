using Inventory.Application.Common.Interfaces;
using Inventory.Domain.Common;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public InventoryDbContext(
        DbContextOptions<InventoryDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId ?? "System";
        var currentTime = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = currentTime;
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = currentTime;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    // Soft delete logic
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = currentTime;
                    entry.Entity.DeletedBy = currentUserId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
