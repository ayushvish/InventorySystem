using Inventory.Application.Features.Unit.Commands;
using Inventory.Application.Features.Unit.DTOs;
using Inventory.Application.Features.Unit.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class UnitsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UnitResponse>>> Create([FromBody] CreateUnitRequest request)
    {
        var result = await Mediator.Send(new CreateUnitCommand(request));
        return Ok(new ApiResponse<UnitResponse>(result, "Unit created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<UnitResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetUnitsPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<UnitResponse>>(result, "Units retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<UnitResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetUnitByIdQuery(id));
        return Ok(new ApiResponse<UnitResponse>(result, "Unit retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<UnitResponse>>> Update(int id, [FromBody] UpdateUnitRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateUnitCommand(request));
        return Ok(new ApiResponse<UnitResponse>(result, "Unit updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteUnitCommand(id));
        return Ok(new ApiResponse<bool>(result, "Unit deleted successfully."));
    }
}
