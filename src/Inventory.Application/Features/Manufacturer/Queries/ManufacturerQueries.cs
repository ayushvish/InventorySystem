using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Manufacturer.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.Manufacturer.Queries;

public record GetManufacturerByIdQuery(int Id) : IRequest<ManufacturerResponse>;

public class GetManufacturerByIdQueryHandler : IRequestHandler<GetManufacturerByIdQuery, ManufacturerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetManufacturerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManufacturerResponse> Handle(GetManufacturerByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Domain.Entities.Manufacturer>().GetByIdAsync(query.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Manufacturer), query.Id);
        }

        return _mapper.Map<ManufacturerResponse>(entity);
    }
}

public record GetManufacturersPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<ManufacturerResponse>>;

public class GetManufacturersPaginatedQueryHandler : IRequestHandler<GetManufacturersPaginatedQuery, PagedResult<ManufacturerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetManufacturersPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ManufacturerResponse>> Handle(GetManufacturersPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Manufacturer>();

        Expression<Func<Domain.Entities.Manufacturer, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = m => m.ManufacturerName.ToLower().Contains(search) || 
                         (m.Country != null && m.Country.ToLower().Contains(search)) || 
                         (m.LicenseNumber != null && m.LicenseNumber.ToLower().Contains(search));
        }

        Func<IQueryable<Domain.Entities.Manufacturer>, IOrderedQueryable<Domain.Entities.Manufacturer>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "manufacturername" => q => isDesc ? q.OrderByDescending(x => x.ManufacturerName) : q.OrderBy(x => x.ManufacturerName),
            "country" => q => isDesc ? q.OrderByDescending(x => x.Country) : q.OrderBy(x => x.Country),
            "licensenumber" => q => isDesc ? q.OrderByDescending(x => x.LicenseNumber) : q.OrderBy(x => x.LicenseNumber),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize);

        var dtos = _mapper.Map<List<ManufacturerResponse>>(items);
        return new PagedResult<ManufacturerResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
