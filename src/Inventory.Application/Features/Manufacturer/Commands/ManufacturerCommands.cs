using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Manufacturer.DTOs;
using MediatR;

namespace Inventory.Application.Features.Manufacturer.Commands;

public record CreateManufacturerCommand(CreateManufacturerRequest Request) : IRequest<ManufacturerResponse>;

public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, ManufacturerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateManufacturerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManufacturerResponse> Handle(CreateManufacturerCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Manufacturer>();

        var exists = await repository.ExistsAsync(m => m.ManufacturerName == command.Request.ManufacturerName);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("ManufacturerName", "Manufacturer name already exists.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.Manufacturer>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ManufacturerResponse>(entity);
    }
}

public record UpdateManufacturerCommand(UpdateManufacturerRequest Request) : IRequest<ManufacturerResponse>;

public class UpdateManufacturerCommandHandler : IRequestHandler<UpdateManufacturerCommand, ManufacturerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateManufacturerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManufacturerResponse> Handle(UpdateManufacturerCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Manufacturer>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Manufacturer), command.Request.Id);
        }

        var exists = await repository.ExistsAsync(m => m.ManufacturerName == command.Request.ManufacturerName && m.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("ManufacturerName", "Manufacturer name already exists.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ManufacturerResponse>(entity);
    }
}

public record DeleteManufacturerCommand(int Id) : IRequest<bool>;

public class DeleteManufacturerCommandHandler : IRequestHandler<DeleteManufacturerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteManufacturerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteManufacturerCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Manufacturer>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Manufacturer), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
