using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Supplier.DTOs;
using MediatR;

namespace Inventory.Application.Features.Supplier.Commands;

public record CreateSupplierCommand(CreateSupplierRequest Request) : IRequest<SupplierResponse>;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SupplierResponse> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Supplier>();

        var exists = await repository.ExistsAsync(s => s.SupplierName == command.Request.SupplierName);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("SupplierName", "Supplier name already exists.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.Supplier>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SupplierResponse>(entity);
    }
}

public record UpdateSupplierCommand(UpdateSupplierRequest Request) : IRequest<SupplierResponse>;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SupplierResponse> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Supplier>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Supplier), command.Request.Id);
        }

        var exists = await repository.ExistsAsync(s => s.SupplierName == command.Request.SupplierName && s.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("SupplierName", "Supplier name already exists.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SupplierResponse>(entity);
    }
}

public record DeleteSupplierCommand(int Id) : IRequest<bool>;

public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSupplierCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteSupplierCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Supplier>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Supplier), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
