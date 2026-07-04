using FluentValidation;
using Inventory.Application.Features.Warehouse.DTOs;

namespace Inventory.Application.Features.Warehouse.Validators;

public class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseRequest>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(x => x.WarehouseName)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(100).WithMessage("Warehouse name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}

public class UpdateWarehouseRequestValidator : AbstractValidator<UpdateWarehouseRequest>
{
    public UpdateWarehouseRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.WarehouseName)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(100).WithMessage("Warehouse name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
