using FluentValidation;

namespace Million.API.RealEstate.Application.DTOs.PropertyTrace
{
    public class PropertyTraceDtoValidator : AbstractValidator<PropertyTraceDto>
    {
        public PropertyTraceDtoValidator()
        {
            RuleFor(x => x.IdProperty)
                .NotEmpty().WithMessage("The property ID is required");

            RuleFor(x => x.DateSale)
                .NotEmpty().WithMessage("The sale date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("The sale date cannot be in the future");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The name is required");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("The value must be greater than 0");

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0).WithMessage("The tax must be 0 or greater");
        }
    }
}