using Inventory.Application.Features.Supplier.Commands;
using Inventory.Application.Features.Supplier.DTOs;
using Inventory.Application.Features.Supplier.Queries;
using Inventory.Shared;
using Inventory.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Authorize]
public class SuppliersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SupplierResponse>>> Create([FromBody] CreateSupplierRequest request)
    {
        var result = await Mediator.Send(new CreateSupplierCommand(request));
        return Ok(new ApiResponse<SupplierResponse>(result, "Supplier created successfully."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<SupplierResponse>>>> GetPaginated([FromQuery] QueryParameters parameters)
    {
        var result = await Mediator.Send(new GetSuppliersPaginatedQuery(parameters));
        return Ok(new ApiResponse<PagedResult<SupplierResponse>>(result, "Suppliers retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierResponse>>> GetById(int id)
    {
        var result = await Mediator.Send(new GetSupplierByIdQuery(id));
        return Ok(new ApiResponse<SupplierResponse>(result, "Supplier retrieved successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierResponse>>> Update(int id, [FromBody] UpdateSupplierRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest, "Mismatched ID in route and request body."));
        }

        var result = await Mediator.Send(new UpdateSupplierCommand(request));
        return Ok(new ApiResponse<SupplierResponse>(result, "Supplier updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteSupplierCommand(id));
        return Ok(new ApiResponse<bool>(result, "Supplier deleted successfully."));
    }
}
