using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.CategoryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

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
