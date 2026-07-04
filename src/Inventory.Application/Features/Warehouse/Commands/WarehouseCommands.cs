using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Warehouse.DTOs;
using MediatR;

namespace Inventory.Application.Features.Warehouse.Commands;

public record CreateWarehouseCommand(CreateWarehouseRequest Request) : IRequest<WarehouseResponse>;

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateWarehouseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseResponse> Handle(CreateWarehouseCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Warehouse>();

        var exists = await repository.ExistsAsync(w => w.WarehouseName == command.Request.WarehouseName);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("WarehouseName", "Warehouse name already exists.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.Warehouse>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WarehouseResponse>(entity);
    }
}

public record UpdateWarehouseCommand(UpdateWarehouseRequest Request) : IRequest<WarehouseResponse>;

public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, WarehouseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseResponse> Handle(UpdateWarehouseCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Warehouse>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Warehouse), command.Request.Id);
        }

        var exists = await repository.ExistsAsync(w => w.WarehouseName == command.Request.WarehouseName && w.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("WarehouseName", "Warehouse name already exists.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WarehouseResponse>(entity);
    }
}

public record DeleteWarehouseCommand(int Id) : IRequest<bool>;

public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteWarehouseCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Warehouse>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Warehouse), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
