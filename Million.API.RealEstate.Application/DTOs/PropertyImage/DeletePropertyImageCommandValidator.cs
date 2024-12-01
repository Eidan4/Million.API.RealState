using FluentValidation;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;

namespace Million.API.RealEstate.Application.DTOs.PropertyImage
{
    public class DeletePropertyImageCommandValidator : AbstractValidator<DeletePropertyImageCommand>
    {
        public DeletePropertyImageCommandValidator()
        {

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}