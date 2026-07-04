using FluentValidation;
using Inventory.Application.Features.WarehouseLocation.DTOs;

namespace Inventory.Application.Features.WarehouseLocation.Validators;

public class CreateWarehouseLocationRequestValidator : AbstractValidator<CreateWarehouseLocationRequest>
{
    public CreateWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("Valid WarehouseId is required.");

        RuleFor(x => x.Rack)
            .NotEmpty().WithMessage("Rack is required.")
            .MaximumLength(50).WithMessage("Rack must not exceed 50 characters.");

        RuleFor(x => x.Shelf)
            .NotEmpty().WithMessage("Shelf is required.")
            .MaximumLength(50).WithMessage("Shelf must not exceed 50 characters.");

        RuleFor(x => x.Bin)
            .NotEmpty().WithMessage("Bin is required.")
            .MaximumLength(50).WithMessage("Bin must not exceed 50 characters.");
    }
}

public class UpdateWarehouseLocationRequestValidator : AbstractValidator<UpdateWarehouseLocationRequest>
{
    public UpdateWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("Valid WarehouseId is required.");

        RuleFor(x => x.Rack)
            .NotEmpty().WithMessage("Rack is required.")
            .MaximumLength(50).WithMessage("Rack must not exceed 50 characters.");

        RuleFor(x => x.Shelf)
            .NotEmpty().WithMessage("Shelf is required.")
            .MaximumLength(50).WithMessage("Shelf must not exceed 50 characters.");

        RuleFor(x => x.Bin)
            .NotEmpty().WithMessage("Bin is required.")
            .MaximumLength(50).WithMessage("Bin must not exceed 50 characters.");
    }
}
