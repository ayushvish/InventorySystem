using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Category.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.Category.Queries;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResponse>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Domain.Entities.Category>().GetByIdAsync(query.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Category), query.Id);
        }

        return _mapper.Map<CategoryResponse>(entity);
    }
}

public record GetCategoriesPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<CategoryResponse>>;

public class GetCategoriesPaginatedQueryHandler : IRequestHandler<GetCategoriesPaginatedQuery, PagedResult<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<CategoryResponse>> Handle(GetCategoriesPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Category>();

        Expression<Func<Domain.Entities.Category, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = c => c.CategoryName.ToLower().Contains(search) || 
                         (c.Description != null && c.Description.ToLower().Contains(search));
        }

        Func<IQueryable<Domain.Entities.Category>, IOrderedQueryable<Domain.Entities.Category>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "categoryname" => q => isDesc ? q.OrderByDescending(x => x.CategoryName) : q.OrderBy(x => x.CategoryName),
            "description" => q => isDesc ? q.OrderByDescending(x => x.Description) : q.OrderBy(x => x.Description),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize);

        var dtos = _mapper.Map<List<CategoryResponse>>(items);
        return new PagedResult<CategoryResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
