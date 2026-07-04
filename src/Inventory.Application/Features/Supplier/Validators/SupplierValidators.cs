using FluentValidation;
using Inventory.Application.Features.Supplier.DTOs;

namespace Inventory.Application.Features.Supplier.Validators;

public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("Supplier name is required.")
            .MaximumLength(150).WithMessage("Supplier name must not exceed 150 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}

public class UpdateSupplierRequestValidator : AbstractValidator<UpdateSupplierRequest>
{
    public UpdateSupplierRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("Supplier name is required.")
            .MaximumLength(150).WithMessage("Supplier name must not exceed 150 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
