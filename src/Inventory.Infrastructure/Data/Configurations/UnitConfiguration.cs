using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UnitName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ShortName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Description)
            .HasMaxLength(250);

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
