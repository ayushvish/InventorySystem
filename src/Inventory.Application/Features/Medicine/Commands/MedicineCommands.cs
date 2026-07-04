using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Medicine.DTOs;
using MediatR;

namespace Inventory.Application.Features.Medicine.Commands;

public record CreateMedicineCommand(CreateMedicineRequest Request) : IRequest<MedicineResponse>;

public class CreateMedicineCommandHandler : IRequestHandler<CreateMedicineCommand, MedicineResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMedicineCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MedicineResponse> Handle(CreateMedicineCommand command, CancellationToken cancellationToken)
    {
        // Validate references
        var categoryExists = await _unitOfWork.Repository<Domain.Entities.Category>().ExistsAsync(c => c.Id == command.Request.CategoryId);
        if (!categoryExists) throw new NotFoundException(nameof(Domain.Entities.Category), command.Request.CategoryId);

        var manufacturerExists = await _unitOfWork.Repository<Domain.Entities.Manufacturer>().ExistsAsync(m => m.Id == command.Request.ManufacturerId);
        if (!manufacturerExists) throw new NotFoundException(nameof(Domain.Entities.Manufacturer), command.Request.ManufacturerId);

        var unitExists = await _unitOfWork.Repository<Domain.Entities.Unit>().ExistsAsync(u => u.Id == command.Request.UnitId);
        if (!unitExists) throw new NotFoundException(nameof(Domain.Entities.Unit), command.Request.UnitId);

        var repository = _unitOfWork.Repository<Domain.Entities.Medicine>();

        var exists = await repository.ExistsAsync(m => m.MedicineName == command.Request.MedicineName);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("MedicineName", "Medicine name already exists.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.Medicine>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch again to include navigation properties for the Response
        var result = await repository.GetByIdAsync(entity.Id);
        return _mapper.Map<MedicineResponse>(result!);
    }
}

public record UpdateMedicineCommand(UpdateMedicineRequest Request) : IRequest<MedicineResponse>;

public class UpdateMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand, MedicineResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMedicineCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MedicineResponse> Handle(UpdateMedicineCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Medicine>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Medicine), command.Request.Id);
        }

        // Validate references
        var categoryExists = await _unitOfWork.Repository<Domain.Entities.Category>().ExistsAsync(c => c.Id == command.Request.CategoryId);
        if (!categoryExists) throw new NotFoundException(nameof(Domain.Entities.Category), command.Request.CategoryId);

        var manufacturerExists = await _unitOfWork.Repository<Domain.Entities.Manufacturer>().ExistsAsync(m => m.Id == command.Request.ManufacturerId);
        if (!manufacturerExists) throw new NotFoundException(nameof(Domain.Entities.Manufacturer), command.Request.ManufacturerId);

        var unitExists = await _unitOfWork.Repository<Domain.Entities.Unit>().ExistsAsync(u => u.Id == command.Request.UnitId);
        if (!unitExists) throw new NotFoundException(nameof(Domain.Entities.Unit), command.Request.UnitId);

        var exists = await repository.ExistsAsync(m => m.MedicineName == command.Request.MedicineName && m.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("MedicineName", "Medicine name already exists.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch again to include navigation properties
        var result = await repository.GetByIdAsync(entity.Id);
        return _mapper.Map<MedicineResponse>(result!);
    }
}

public record DeleteMedicineCommand(int Id) : IRequest<bool>;

public class DeleteMedicineCommandHandler : IRequestHandler<DeleteMedicineCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMedicineCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteMedicineCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Medicine>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Medicine), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
