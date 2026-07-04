using FluentValidation;
using Inventory.Application.Features.Medicine.DTOs;

namespace Inventory.Application.Features.Medicine.Validators;

public class CreateMedicineRequestValidator : AbstractValidator<CreateMedicineRequest>
{
    public CreateMedicineRequestValidator()
    {
        RuleFor(x => x.MedicineName)
            .NotEmpty().WithMessage("Medicine name is required.")
            .MaximumLength(150).WithMessage("Medicine name must not exceed 150 characters.");

        RuleFor(x => x.GenericName)
            .NotEmpty().WithMessage("Generic name is required.")
            .MaximumLength(150).WithMessage("Generic name must not exceed 150 characters.");

        RuleFor(x => x.BrandName)
            .NotEmpty().WithMessage("Brand name is required.")
            .MaximumLength(100).WithMessage("Brand name must not exceed 100 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid CategoryId is required.");

        RuleFor(x => x.ManufacturerId)
            .GreaterThan(0).WithMessage("Valid ManufacturerId is required.");

        RuleFor(x => x.UnitId)
            .GreaterThan(0).WithMessage("Valid UnitId is required.");

        RuleFor(x => x.GSTPercentage)
            .InclusiveBetween(0, 100).WithMessage("GST Percentage must be between 0 and 100.");
    }
}

public class UpdateMedicineRequestValidator : AbstractValidator<UpdateMedicineRequest>
{
    public UpdateMedicineRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.MedicineName)
            .NotEmpty().WithMessage("Medicine name is required.")
            .MaximumLength(150).WithMessage("Medicine name must not exceed 150 characters.");

        RuleFor(x => x.GenericName)
            .NotEmpty().WithMessage("Generic name is required.")
            .MaximumLength(150).WithMessage("Generic name must not exceed 150 characters.");

        RuleFor(x => x.BrandName)
            .NotEmpty().WithMessage("Brand name is required.")
            .MaximumLength(100).WithMessage("Brand name must not exceed 100 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid CategoryId is required.");

        RuleFor(x => x.ManufacturerId)
            .GreaterThan(0).WithMessage("Valid ManufacturerId is required.");

        RuleFor(x => x.UnitId)
            .GreaterThan(0).WithMessage("Valid UnitId is required.");

        RuleFor(x => x.GSTPercentage)
            .InclusiveBetween(0, 100).WithMessage("GST Percentage must be between 0 and 100.");
    }
}
