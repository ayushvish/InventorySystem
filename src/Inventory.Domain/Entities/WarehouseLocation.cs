using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class WarehouseLocation : BaseEntity
{
    public int WarehouseId { get; set; }
    public string Rack { get; set; } = string.Empty;
    public string Shelf { get; set; } = string.Empty;
    public string Bin { get; set; } = string.Empty;
    public bool Status { get; set; } = true;

    // Navigation Properties
    public virtual Warehouse? Warehouse { get; set; }
}
