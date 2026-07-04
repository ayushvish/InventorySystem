namespace Inventory.Application.Features.Manufacturer.DTOs;

public record ManufacturerDto(
    int Id,
    string ManufacturerName,
    string? Country,
    string? LicenseNumber,
    bool Status,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public record CreateManufacturerRequest(
    string ManufacturerName,
    string? Country,
    string? LicenseNumber,
    bool Status = true);

public record UpdateManufacturerRequest(
    int Id,
    string ManufacturerName,
    string? Country,
    string? LicenseNumber,
    bool Status);

public record ManufacturerResponse(
    int Id,
    string ManufacturerName,
    string? Country,
    string? LicenseNumber,
    bool Status);
