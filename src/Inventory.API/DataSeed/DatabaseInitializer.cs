using Inventory.Domain.Entities;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataSeed;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { CategoryName = "Antibiotics", Description = "Medicines that fight bacterial infections", Status = true },
                new() { CategoryName = "Analgesics", Description = "Pain relievers", Status = true },
                new() { CategoryName = "Antiseptics", Description = "Antiseptic liquids and creams", Status = true },
                new() { CategoryName = "Vaccines", Description = "Biological preparations for immunity", Status = true }
            };

            await context.Categories.AddRangeAsync(categories);
        }

        if (!await context.Units.AnyAsync())
        {
            var units = new List<Unit>
            {
                new() { UnitName = "Box", ShortName = "BX", Description = "Box container" },
                new() { UnitName = "Strip", ShortName = "ST", Description = "Pill strip containing multiple capsules" },
                new() { UnitName = "Vial", ShortName = "VL", Description = "Small glass bottle/vial" },
                new() { UnitName = "Bottle", ShortName = "BT", Description = "Liquid bottle" }
            };

            await context.Units.AddRangeAsync(units);
        }

        await context.SaveChangesAsync();
    }
}
