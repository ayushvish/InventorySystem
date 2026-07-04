using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Medicine : BaseEntity
{
    public string MedicineName { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string? Strength { get; set; }
    public string? DosageForm { get; set; }
    public int CategoryId { get; set; }
    public int ManufacturerId { get; set; }
    public int UnitId { get; set; }
    public string? HSNCode { get; set; }
    public decimal GSTPercentage { get; set; }
    public string? StorageCondition { get; set; }
    public bool PrescriptionRequired { get; set; }
    public bool Status { get; set; } = true;

    // Navigation Properties
    public virtual Category? Category { get; set; }
    public virtual Manufacturer? Manufacturer { get; set; }
    public virtual Unit? Unit { get; set; }
}
