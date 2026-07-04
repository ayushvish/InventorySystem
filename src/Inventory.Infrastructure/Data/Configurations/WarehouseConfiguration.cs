using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.WarehouseName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Address)
            .HasMaxLength(250);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .HasMaxLength(100);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.Phone)
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(100);

        builder.Property(e => e.Version)
            .IsRowVersion();

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
