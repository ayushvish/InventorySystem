using Inventory.Application.Features.Manufacturer.Commands;
using Inventory.Application.Features.Manufacturer.DTOs;
using Inventory.Application.Features.Manufacturer.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class ManufacturersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ManufacturerResponse>>> Create([FromBody] CreateManufacturerRequest request)
    {
        var result = await Mediator.Send(new CreateManufacturerCommand(request));
        return Ok(new ApiResponse<ManufacturerResponse>(result, "Manufacturer created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ManufacturerResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetManufacturersPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<ManufacturerResponse>>(result, "Manufacturers retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<ManufacturerResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetManufacturerByIdQuery(id));
        return Ok(new ApiResponse<ManufacturerResponse>(result, "Manufacturer retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<ManufacturerResponse>>> Update(int id, [FromBody] UpdateManufacturerRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateManufacturerCommand(request));
        return Ok(new ApiResponse<ManufacturerResponse>(result, "Manufacturer updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteManufacturerCommand(id));
        return Ok(new ApiResponse<bool>(result, "Manufacturer deleted successfully."));
    }
}
