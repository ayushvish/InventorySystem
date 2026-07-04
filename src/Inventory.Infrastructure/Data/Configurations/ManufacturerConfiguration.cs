using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ManufacturerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.LicenseNumber)
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
