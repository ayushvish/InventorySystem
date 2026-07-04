using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Category.DTOs;
using MediatR;

namespace Inventory.Application.Features.Category.Commands;

public record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<CategoryResponse>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Category>();

        var exists = await repository.ExistsAsync(c => c.CategoryName == command.Request.CategoryName);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("CategoryName", "Category name already exists.")
            });
        }

        var entity = _mapper.Map<Domain.Entities.Category>(command.Request);
        await repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryResponse>(entity);
    }
}

public record UpdateCategoryCommand(UpdateCategoryRequest Request) : IRequest<CategoryResponse>;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Category>();

        var entity = await repository.GetByIdAsync(command.Request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Category), command.Request.Id);
        }

        var exists = await repository.ExistsAsync(c => c.CategoryName == command.Request.CategoryName && c.Id != command.Request.Id);
        if (exists)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("CategoryName", "Category name already exists.")
            });
        }

        _mapper.Map(command.Request, entity);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryResponse>(entity);
    }
}

public record DeleteCategoryCommand(int Id) : IRequest<bool>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Category>();

        var entity = await repository.GetByIdAsync(command.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Category), command.Id);
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
