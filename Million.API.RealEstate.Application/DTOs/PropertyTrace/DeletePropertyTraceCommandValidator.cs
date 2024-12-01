using FluentValidation;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;

namespace Million.API.RealEstate.Application.DTOs.PropertyTrace
{
    public class DeletePropertyTraceCommandValidator : AbstractValidator<DeletePropertyTraceCommand>
    {
        public DeletePropertyTraceCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}