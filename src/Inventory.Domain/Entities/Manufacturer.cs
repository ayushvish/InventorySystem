using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Manufacturer : BaseEntity
{
    public string ManufacturerName { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? LicenseNumber { get; set; }
    public bool Status { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
