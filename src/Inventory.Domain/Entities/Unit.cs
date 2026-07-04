using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Unit : BaseEntity
{
    public string UnitName { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Properties
    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
