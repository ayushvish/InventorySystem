using Inventory.Application.Features.Warehouse.Commands;
using Inventory.Application.Features.Warehouse.DTOs;
using Inventory.Application.Features.Warehouse.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class WarehousesController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<WarehouseResponse>>> Create([FromBody] CreateWarehouseRequest request)
    {
        var result = await Mediator.Send(new CreateWarehouseCommand(request));
        return Ok(new ApiResponse<WarehouseResponse>(result, "Warehouse created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<WarehouseResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetWarehousesPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<WarehouseResponse>>(result, "Warehouses retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<WarehouseResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetWarehouseByIdQuery(id));
        return Ok(new ApiResponse<WarehouseResponse>(result, "Warehouse retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<WarehouseResponse>>> Update(int id, [FromBody] UpdateWarehouseRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateWarehouseCommand(request));
        return Ok(new ApiResponse<WarehouseResponse>(result, "Warehouse updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteWarehouseCommand(id));
        return Ok(new ApiResponse<bool>(result, "Warehouse deleted successfully."));
    }
}
