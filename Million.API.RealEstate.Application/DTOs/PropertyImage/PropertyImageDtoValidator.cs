using FluentValidation;

namespace Million.API.RealEstate.Application.DTOs.PropertyImage
{
    public class PropertyImageDtoValidator : AbstractValidator<PropertyImageDto>
    {
        public PropertyImageDtoValidator()
        {
            RuleFor(x => x.IdProperty)
                .NotEmpty().WithMessage("The property ID is required");

            RuleFor(x => x.File)
                .NotEmpty().WithMessage("The file field is required")
                .Matches(@"^(http|https):\/\/").WithMessage("The file must be a valid URL");

            RuleFor(x => x.Enabled)
                .NotNull().WithMessage("The enabled field must not be null");
        }
    }
}