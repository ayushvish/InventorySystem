using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Supplier.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.Supplier.Queries;

public record GetSupplierByIdQuery(int Id) : IRequest<SupplierResponse>;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSupplierByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SupplierResponse> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Domain.Entities.Supplier>().GetByIdAsync(query.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Supplier), query.Id);
        }

        return _mapper.Map<SupplierResponse>(entity);
    }
}

public record GetSuppliersPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<SupplierResponse>>;

public class GetSuppliersPaginatedQueryHandler : IRequestHandler<GetSuppliersPaginatedQuery, PagedResult<SupplierResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSuppliersPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<SupplierResponse>> Handle(GetSuppliersPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Supplier>();

        Expression<Func<Domain.Entities.Supplier, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = s => s.SupplierName.ToLower().Contains(search) || 
                         (s.GSTNumber != null && s.GSTNumber.ToLower().Contains(search)) || 
                         (s.DrugLicense != null && s.DrugLicense.ToLower().Contains(search)) || 
                         (s.Email != null && s.Email.ToLower().Contains(search)) || 
                         (s.Phone != null && s.Phone.ToLower().Contains(search));
        }

        Func<IQueryable<Domain.Entities.Supplier>, IOrderedQueryable<Domain.Entities.Supplier>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "suppliername" => q => isDesc ? q.OrderByDescending(x => x.SupplierName) : q.OrderBy(x => x.SupplierName),
            "gstnumber" => q => isDesc ? q.OrderByDescending(x => x.GSTNumber) : q.OrderBy(x => x.GSTNumber),
            "druglicense" => q => isDesc ? q.OrderByDescending(x => x.DrugLicense) : q.OrderBy(x => x.DrugLicense),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize);

        var dtos = _mapper.Map<List<SupplierResponse>>(items);
        return new PagedResult<SupplierResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
