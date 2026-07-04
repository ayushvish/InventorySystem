using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Unit.DTOs;
using MediatR;

namespace Inventory.Application.Features.Unit.Commands;

public record CreateUnitCommand(CreateUnitRequest Request) : IRequest<UnitResponse>;

public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, UnitResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UnitResponse> Handle(CreateUnitCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Unit>();

        var exists = await repository.ExistsAsync(u => u.UnitName == command.Request.UnitName || u.ShortName == command.Request.ShortName);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("UnitName", "Unit name or short name already exists.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.Unit>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UnitResponse>(entity);
    }
}

public record UpdateUnitCommand(UpdateUnitRequest Request) : IRequest<UnitResponse>;

public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, UnitResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UnitResponse> Handle(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Unit>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Unit), command.Request.Id);
        }

        var exists = await repository.ExistsAsync(u => (u.UnitName == command.Request.UnitName || u.ShortName == command.Request.ShortName) && u.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("UnitName", "Unit name or short name already exists.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UnitResponse>(entity);
    }
}

public record DeleteUnitCommand(int Id) : IRequest<bool>;

public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUnitCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUnitCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Unit>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Unit), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
