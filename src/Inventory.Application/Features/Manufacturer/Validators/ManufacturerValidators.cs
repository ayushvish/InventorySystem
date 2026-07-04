using FluentValidation;
using Inventory.Application.Features.Manufacturer.DTOs;

namespace Inventory.Application.Features.Manufacturer.Validators;

public class CreateManufacturerRequestValidator : AbstractValidator<CreateManufacturerRequest>
{
    public CreateManufacturerRequestValidator()
    {
        RuleFor(x => x.ManufacturerName)
            .NotEmpty().WithMessage("Manufacturer name is required.")
            .MaximumLength(100).WithMessage("Manufacturer name must not exceed 100 characters.");
    }
}

public class UpdateManufacturerRequestValidator : AbstractValidator<UpdateManufacturerRequest>
{
    public UpdateManufacturerRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.ManufacturerName)
            .NotEmpty().WithMessage("Manufacturer name is required.")
            .MaximumLength(100).WithMessage("Manufacturer name must not exceed 100 characters.");
    }
}
