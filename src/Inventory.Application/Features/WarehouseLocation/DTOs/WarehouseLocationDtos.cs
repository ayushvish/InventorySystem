namespace Inventory.Application.Features.WarehouseLocation.DTOs;

public record WarehouseLocationDto(
    int Id,
    int WarehouseId,
    string Rack,
    string Shelf,
    string Bin,
    bool Status,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public record CreateWarehouseLocationRequest(
    int WarehouseId,
    string Rack,
    string Shelf,
    string Bin,
    bool Status = true);

public record UpdateWarehouseLocationRequest(
    int Id,
    int WarehouseId,
    string Rack,
    string Shelf,
    string Bin,
    bool Status);

public record WarehouseLocationResponse(
    int Id,
    int WarehouseId,
    string? WarehouseName,
    string Rack,
    string Shelf,
    string Bin,
    bool Status);
