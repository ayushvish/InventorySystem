using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.SupplierName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.GSTNumber)
            .HasMaxLength(50);

        builder.Property(e => e.DrugLicense)
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasMaxLength(100);

        builder.Property(e => e.Phone)
            .HasMaxLength(50);

        builder.Property(e => e.Address)
            .HasMaxLength(250);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .HasMaxLength(100);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20);

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
