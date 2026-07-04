namespace Inventory.Application.Features.Supplier.DTOs;

public record SupplierDto(
    int Id,
    string SupplierName,
    string? GSTNumber,
    string? DrugLicense,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool Status,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public record CreateSupplierRequest(
    string SupplierName,
    string? GSTNumber,
    string? DrugLicense,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool Status = true);

public record UpdateSupplierRequest(
    int Id,
    string SupplierName,
    string? GSTNumber,
    string? DrugLicense,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool Status);

public record SupplierResponse(
    int Id,
    string SupplierName,
    string? GSTNumber,
    string? DrugLicense,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool Status);
