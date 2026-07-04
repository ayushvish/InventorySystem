using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Unit.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.Unit.Queries;

public record GetUnitByIdQuery(int Id) : IRequest<UnitResponse>;

public class GetUnitByIdQueryHandler : IRequestHandler<GetUnitByIdQuery, UnitResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUnitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UnitResponse> Handle(GetUnitByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Domain.Entities.Unit>().GetByIdAsync(query.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Unit), query.Id);
        }

        return _mapper.Map<UnitResponse>(entity);
    }
}

public record GetUnitsPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<UnitResponse>>;

public class GetUnitsPaginatedQueryHandler : IRequestHandler<GetUnitsPaginatedQuery, PagedResult<UnitResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUnitsPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<UnitResponse>> Handle(GetUnitsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Unit>();

        Expression<Func<Domain.Entities.Unit, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = u => u.UnitName.ToLower().Contains(search) || 
                         u.ShortName.ToLower().Contains(search) || 
                         (u.Description != null && u.Description.ToLower().Contains(search));
        }

        Func<IQueryable<Domain.Entities.Unit>, IOrderedQueryable<Domain.Entities.Unit>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "unitname" => q => isDesc ? q.OrderByDescending(x => x.UnitName) : q.OrderBy(x => x.UnitName),
            "shortname" => q => isDesc ? q.OrderByDescending(x => x.ShortName) : q.OrderBy(x => x.ShortName),
            "description" => q => isDesc ? q.OrderByDescending(x => x.Description) : q.OrderBy(x => x.Description),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize);

        var dtos = _mapper.Map<List<UnitResponse>>(items);
        return new PagedResult<UnitResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
