namespace Inventory.Application.Features.Category.DTOs;

public record CategoryDto(
    int Id,
    string CategoryName,
    string? Description,
    bool Status,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public record CreateCategoryRequest(
    string CategoryName,
    string? Description,
    bool Status = true);

public record UpdateCategoryRequest(
    int Id,
    string CategoryName,
    string? Description,
    bool Status);

public record CategoryResponse(
    int Id,
    string CategoryName,
    string? Description,
    bool Status);
