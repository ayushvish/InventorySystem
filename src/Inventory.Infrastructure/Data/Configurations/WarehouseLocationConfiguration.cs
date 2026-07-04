using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class WarehouseLocationConfiguration : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Rack)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Shelf)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Bin)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(100);

        builder.Property(e => e.Version)
            .IsRowVersion();

        // Relationship
        builder.HasOne(e => e.Warehouse)
            .WithMany(w => w.Locations)
            .HasForeignKey(e => e.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
