using FluentValidation;
using Inventory.Application.Features.Unit.DTOs;

namespace Inventory.Application.Features.Unit.Validators;

public class CreateUnitRequestValidator : AbstractValidator<CreateUnitRequest>
{
    public CreateUnitRequestValidator()
    {
        RuleFor(x => x.UnitName)
            .NotEmpty().WithMessage("Unit name is required.")
            .MaximumLength(100).WithMessage("Unit name must not exceed 100 characters.");

        RuleFor(x => x.ShortName)
            .NotEmpty().WithMessage("Short name is required.")
            .MaximumLength(20).WithMessage("Short name must not exceed 20 characters.");
    }
}

public class UpdateUnitRequestValidator : AbstractValidator<UpdateUnitRequest>
{
    public UpdateUnitRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.UnitName)
            .NotEmpty().WithMessage("Unit name is required.")
            .MaximumLength(100).WithMessage("Unit name must not exceed 100 characters.");

        RuleFor(x => x.ShortName)
            .NotEmpty().WithMessage("Short name is required.")
            .MaximumLength(20).WithMessage("Short name must not exceed 20 characters.");
    }
}
