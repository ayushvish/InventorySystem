using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Supplier : BaseEntity
{
    public string SupplierName { get; set; } = string.Empty;
    public string? GSTNumber { get; set; }
    public string? DrugLicense { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public bool Status { get; set; } = true;
}
