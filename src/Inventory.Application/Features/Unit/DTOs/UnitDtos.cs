namespace Inventory.Application.Features.Unit.DTOs;

public record UnitDto(
    int Id,
    string UnitName,
    string ShortName,
    string? Description,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public record CreateUnitRequest(
    string UnitName,
    string ShortName,
    string? Description);

public record UpdateUnitRequest(
    int Id,
    string UnitName,
    string ShortName,
    string? Description);

public record UnitResponse(
    int Id,
    string UnitName,
    string ShortName,
    string? Description);
