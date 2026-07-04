using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Warehouse.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.Warehouse.Queries;

public record GetWarehouseByIdQuery(int Id) : IRequest<WarehouseResponse>;

public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseResponse> Handle(GetWarehouseByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Domain.Entities.Warehouse>().GetByIdAsync(query.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Warehouse), query.Id);
        }

        return _mapper.Map<WarehouseResponse>(entity);
    }
}

public record GetWarehousesPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<WarehouseResponse>>;

public class GetWarehousesPaginatedQueryHandler : IRequestHandler<GetWarehousesPaginatedQuery, PagedResult<WarehouseResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWarehousesPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<WarehouseResponse>> Handle(GetWarehousesPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Warehouse>();

        Expression<Func<Domain.Entities.Warehouse, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = w => w.WarehouseName.ToLower().Contains(search) || 
                         (w.City != null && w.City.ToLower().Contains(search)) || 
                         (w.State != null && w.State.ToLower().Contains(search)) || 
                         (w.Country != null && w.Country.ToLower().Contains(search));
        }

        Func<IQueryable<Domain.Entities.Warehouse>, IOrderedQueryable<Domain.Entities.Warehouse>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "warehousename" => q => isDesc ? q.OrderByDescending(x => x.WarehouseName) : q.OrderBy(x => x.WarehouseName),
            "city" => q => isDesc ? q.OrderByDescending(x => x.City) : q.OrderBy(x => x.City),
            "state" => q => isDesc ? q.OrderByDescending(x => x.State) : q.OrderBy(x => x.State),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize);

        var dtos = _mapper.Map<List<WarehouseResponse>>(items);
        return new PagedResult<WarehouseResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
