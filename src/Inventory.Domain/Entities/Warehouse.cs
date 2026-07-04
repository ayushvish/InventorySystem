using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string WarehouseName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool Status { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<WarehouseLocation> Locations { get; set; } = new List<WarehouseLocation>();
}
