using FluentValidation;

namespace Million.API.RealEstate.Application.DTOs.Property
{
    public class UpdatePropertyDtoValidator : AbstractValidator<PropertyDto>
    {
        public UpdatePropertyDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The property name is required");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("The address is required");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The price must be greater than 0");

            RuleFor(x => x.CodeInternal)
                .NotEmpty().WithMessage("The internal code is required");

            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("The year must be valid");

            RuleFor(x => x.IdOwner)
                .NotEmpty().WithMessage("The owner ID is required");
        }
    }
}