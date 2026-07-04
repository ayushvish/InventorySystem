using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Category : BaseEntity
{
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Status { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
