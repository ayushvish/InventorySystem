using System.Linq.Expressions;
using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Medicine.DTOs;
using Inventory.Shared.Pagination;
using MediatR;

namespace Inventory.Application.Features.Medicine.Queries;

public record GetMedicineByIdQuery(int Id) : IRequest<MedicineResponse>;

public class GetMedicineByIdQueryHandler : IRequestHandler<GetMedicineByIdQuery, MedicineResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMedicineByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MedicineResponse> Handle(GetMedicineByIdQuery query, CancellationToken cancellationToken)
    {
        // To include Category, Unit, Manufacturer, we can call GetPaginatedAsync or implement an include pattern.
        // Let's implement an include pattern in the repository or just fetch and populate.
        // Since GetPaginatedAsync allows includes, let's use it or fetch via repository predicate search.
        var repository = _unitOfWork.Repository<Domain.Entities.Medicine>();
        var (items, _) = await repository.GetPaginatedAsync(
            m => m.Id == query.Id,
            null,
            1,
            1,
            new[] { "Category", "Unit", "Manufacturer" });

        var entity = items.FirstOrDefault();
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Medicine), query.Id);
        }

        return _mapper.Map<MedicineResponse>(entity);
    }
}

public record GetMedicinesPaginatedQuery(QueryParameters Parameters) : IRequest<PagedResult<MedicineResponse>>;

public class GetMedicinesPaginatedQueryHandler : IRequestHandler<GetMedicinesPaginatedQuery, PagedResult<MedicineResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMedicinesPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<MedicineResponse>> Handle(GetMedicinesPaginatedQuery query, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Medicine>();

        Expression<Func<Domain.Entities.Medicine, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query.Parameters.Search))
        {
            var search = query.Parameters.Search.ToLower();
            filter = m => m.MedicineName.ToLower().Contains(search) || 
                         m.GenericName.ToLower().Contains(search) || 
                         m.BrandName.ToLower().Contains(search) || 
                         (m.HSNCode != null && m.HSNCode.ToLower().Contains(search));
        }

        Func<IQueryable<Domain.Entities.Medicine>, IOrderedQueryable<Domain.Entities.Medicine>>? orderBy = null;
        var sortCol = query.Parameters.SortColumn?.ToLower() ?? "id";
        var isDesc = query.Parameters.SortDirection.ToLower() == "desc";

        orderBy = sortCol switch
        {
            "medicinename" => q => isDesc ? q.OrderByDescending(x => x.MedicineName) : q.OrderBy(x => x.MedicineName),
            "genericname" => q => isDesc ? q.OrderByDescending(x => x.GenericName) : q.OrderBy(x => x.GenericName),
            "brandname" => q => isDesc ? q.OrderByDescending(x => x.BrandName) : q.OrderBy(x => x.BrandName),
            _ => q => isDesc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id)
        };

        var (items, totalCount) = await repository.GetPaginatedAsync(
            filter,
            orderBy,
            query.Parameters.PageNumber,
            query.Parameters.PageSize,
            new[] { "Category", "Unit", "Manufacturer" });

        var dtos = _mapper.Map<List<MedicineResponse>>(items);
        return new PagedResult<MedicineResponse>(dtos, totalCount, query.Parameters.PageNumber, query.Parameters.PageSize);
    }
}
