using Inventory.Application.Features.WarehouseLocation.Commands;
using Inventory.Application.Features.WarehouseLocation.DTOs;
using Inventory.Application.Features.WarehouseLocation.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class WarehouseLocationsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<WarehouseLocationResponse>>> Create([FromBody] CreateWarehouseLocationRequest request)
    {
        var result = await Mediator.Send(new CreateWarehouseLocationCommand(request));
        return Ok(new ApiResponse<WarehouseLocationResponse>(result, "Warehouse location created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<WarehouseLocationResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetWarehouseLocationsPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<WarehouseLocationResponse>>(result, "Warehouse locations retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<WarehouseLocationResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetWarehouseLocationByIdQuery(id));
        return Ok(new ApiResponse<WarehouseLocationResponse>(result, "Warehouse location retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<WarehouseLocationResponse>>> Update(int id, [FromBody] UpdateWarehouseLocationRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateWarehouseLocationCommand(request));
        return Ok(new ApiResponse<WarehouseLocationResponse>(result, "Warehouse location updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteWarehouseLocationCommand(id));
        return Ok(new ApiResponse<bool>(result, "Warehouse location deleted successfully."));
    }
}
