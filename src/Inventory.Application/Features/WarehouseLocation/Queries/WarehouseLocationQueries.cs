using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.WarehouseLocation.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.WarehouseLocation.Queries;

public record GetWarehouseLocationByIdQuery(int Id) : IRequest<WarehouseLocationResponse>;

public class GetWarehouseLocationByIdQueryHandler : IRequestHandler<GetWarehouseLocationByIdQuery, WarehouseLocationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWarehouseLocationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseLocationResponse> Handle(GetWarehouseLocationByIdQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.WarehouseLocation>();
        var (items, _) = await repository.GetPaginatedAsync(
            wl => wl.Id == query.Id,
            null,
            1,
            1,
            new[] { "Warehouse" });

        var entity = items.FirstOrDefault();
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WarehouseLocation), query.Id);
        }

        return _mapper.Map<WarehouseLocationResponse>(entity);
    }
}

public record GetWarehouseLocationsPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<WarehouseLocationResponse>>;

public class GetWarehouseLocationsPaginatedQueryHandler : IRequestHandler<GetWarehouseLocationsPaginatedQuery, PagedResult<WarehouseLocationResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWarehouseLocationsPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<WarehouseLocationResponse>> Handle(GetWarehouseLocationsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.WarehouseLocation>();

        Expression<Func<Domain.Entities.WarehouseLocation, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = wl => wl.Rack.ToLower().Contains(search) || 
                          wl.Shelf.ToLower().Contains(search) || 
                          wl.Bin.ToLower().Contains(search);
        }

        Func<IQueryable<Domain.Entities.WarehouseLocation>, IOrderedQueryable<Domain.Entities.WarehouseLocation>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "rack" => q => isDesc ? q.OrderByDescending(x => x.Rack) : q.OrderBy(x => x.Rack),
            "shelf" => q => isDesc ? q.OrderByDescending(x => x.Shelf) : q.OrderBy(x => x.Shelf),
            "bin" => q => isDesc ? q.OrderByDescending(x => x.Bin) : q.OrderBy(x => x.Bin),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize,
            new[] { "Warehouse" });

        var dtos = _mapper.Map<List<WarehouseLocationResponse>>(items);
        return new PagedResult<WarehouseLocationResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
