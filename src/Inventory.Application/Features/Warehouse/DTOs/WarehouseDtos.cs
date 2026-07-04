namespace Inventory.Application.Features.Warehouse.DTOs;

public record WarehouseDto(
    int Id,
    string WarehouseName,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? Phone,
    string? Email,
    bool Status,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public record CreateWarehouseRequest(
    string WarehouseName,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? Phone,
    string? Email,
    bool Status = true);

public record UpdateWarehouseRequest(
    int Id,
    string WarehouseName,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? Phone,
    string? Email,
    bool Status);

public record WarehouseResponse(
    int Id,
    string WarehouseName,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? Phone,
    string? Email,
    bool Status);
