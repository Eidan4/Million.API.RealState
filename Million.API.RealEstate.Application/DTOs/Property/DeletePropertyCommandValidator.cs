using FluentValidation;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;

namespace Million.API.RealEstate.Application.DTOs.Property
{
    public class DeletePropertyCommandValidator : AbstractValidator<DeletePropertyCommand>
    {
        public DeletePropertyCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

        }
    }
}