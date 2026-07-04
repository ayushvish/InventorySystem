using Inventory.Application.Features.Category.Commands;
using Inventory.Application.Features.Category.DTOs;
using Inventory.Application.Features.Category.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class CategoriesController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> Create([FromBody] CreateCategoryRequest request)
    {
        var result = await Mediator.Send(new CreateCategoryCommand(request));
        return Ok(new ApiResponse<CategoryResponse>(result, "Category created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CategoryResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetCategoriesPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<CategoryResponse>>(result, "Categories retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery(id));
        return Ok(new ApiResponse<CategoryResponse>(result, "Category retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> Update(int id, [FromBody] UpdateCategoryRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateCategoryCommand(request));
        return Ok(new ApiResponse<CategoryResponse>(result, "Category updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteCategoryCommand(id));
        return Ok(new ApiResponse<bool>(result, "Category deleted successfully."));
    }
}
