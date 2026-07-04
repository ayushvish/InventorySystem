using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.MedicineName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.GenericName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.BrandName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Strength)
            .HasMaxLength(50);

        builder.Property(e => e.DosageForm)
            .HasMaxLength(50);

        builder.Property(e => e.HSNCode)
            .HasMaxLength(20);

        builder.Property(e => e.GSTPercentage)
            .HasPrecision(18, 2);

        builder.Property(e => e.StorageCondition)
            .HasMaxLength(200);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(100);

        builder.Property(e => e.Version)
            .IsRowVersion();

        // Relationships
        builder.HasOne(e => e.Category)
            .WithMany(c => c.Medicines)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Manufacturer)
            .WithMany(m => m.Medicines)
            .HasForeignKey(e => e.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Unit)
            .WithMany(u => u.Medicines)
            .HasForeignKey(e => e.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
