using Inventory.Application.Features.Medicine.Commands;
using Inventory.Application.Features.Medicine.DTOs;
using Inventory.Application.Features.Medicine.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class MedicinesController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MedicineResponse>>> Create([FromBody] CreateMedicineRequest request)
    {
        var result = await Mediator.Send(new CreateMedicineCommand(request));
        return Ok(new ApiResponse<MedicineResponse>(result, "Medicine created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<MedicineResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetMedicinesPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<MedicineResponse>>(result, "Medicines retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<MedicineResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetMedicineByIdQuery(id));
        return Ok(new ApiResponse<MedicineResponse>(result, "Medicine retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<MedicineResponse>>> Update(int id, [FromBody] UpdateMedicineRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateMedicineCommand(request));
        return Ok(new ApiResponse<MedicineResponse>(result, "Medicine updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteMedicineCommand(id));
        return Ok(new ApiResponse<bool>(result, "Medicine deleted successfully."));
    }
}
