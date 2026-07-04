using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.WarehouseLocation.DTOs;
using MediatR;

namespace Inventory.Application.Features.WarehouseLocation.Commands;

public record CreateWarehouseLocationCommand(CreateWarehouseLocationRequest Request) : IRequest<WarehouseLocationResponse>;

public class CreateWarehouseLocationCommandHandler : IRequestHandler<CreateWarehouseLocationCommand, WarehouseLocationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateWarehouseLocationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseLocationResponse> Handle(CreateWarehouseLocationCommand command, CancellationToken cancellationToken)
    {
        // Verify warehouse exists
        var warehouseExists = await _unitOfWork.Repository<Domain.Entities.Warehouse>().ExistsAsync(w => w.Id == command.Request.WarehouseId);
        if (!warehouseExists)
        {
            throw new NotFoundException(nameof(Domain.Entities.Warehouse), command.Request.WarehouseId);
        }

        var repository = _unitOfWork.Repository<Domain.Entities.WarehouseLocation>();

        // Check duplicate location in the same warehouse
        var exists = await repository.ExistsAsync(wl => wl.WarehouseId == command.Request.WarehouseId && 
                                                        wl.Rack == command.Request.Rack && 
                                                        wl.Shelf == command.Request.Shelf && 
                                                        wl.Bin == command.Request.Bin);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Bin", "Warehouse location (Rack/Shelf/Bin) already exists in this warehouse.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.WarehouseLocation>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch again to include Warehouse name
        var result = await repository.GetPaginatedAsync(wl => wl.Id == entity.Id, null, 1, 1, new[] { "Warehouse" });
        return _mapper.Map<WarehouseLocationResponse>(result.Items.First());
    }
}

public record UpdateWarehouseLocationCommand(UpdateWarehouseLocationRequest Request) : IRequest<WarehouseLocationResponse>;

public class UpdateWarehouseLocationCommandHandler : IRequestHandler<UpdateWarehouseLocationCommand, WarehouseLocationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateWarehouseLocationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseLocationResponse> Handle(UpdateWarehouseLocationCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.WarehouseLocation>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WarehouseLocation), command.Request.Id);
        }

        // Verify warehouse exists
        var warehouseExists = await _unitOfWork.Repository<Domain.Entities.Warehouse>().ExistsAsync(w => w.Id == command.Request.WarehouseId);
        if (!warehouseExists)
        {
            throw new NotFoundException(nameof(Domain.Entities.Warehouse), command.Request.WarehouseId);
        }

        // Check duplicate location in the same warehouse
        var exists = await repository.ExistsAsync(wl => wl.WarehouseId == command.Request.WarehouseId && 
                                                        wl.Rack == command.Request.Rack && 
                                                        wl.Shelf == command.Request.Shelf && 
                                                        wl.Bin == command.Request.Bin &&
                                                        wl.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Bin", "Warehouse location (Rack/Shelf/Bin) already exists in this warehouse.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch again to include Warehouse name
        var result = await repository.GetPaginatedAsync(wl => wl.Id == entity.Id, null, 1, 1, new[] { "Warehouse" });
        return _mapper.Map<WarehouseLocationResponse>(result.Items.First());
    }
}

public record DeleteWarehouseLocationCommand(int Id) : IRequest<bool>;

public class DeleteWarehouseLocationCommandHandler : IRequestHandler<DeleteWarehouseLocationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWarehouseLocationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteWarehouseLocationCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.WarehouseLocation>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WarehouseLocation), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
