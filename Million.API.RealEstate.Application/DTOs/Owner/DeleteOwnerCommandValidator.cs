using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using SharpCompress.Crypto;

namespace Million.API.RealEstate.Application.DTOs.Owner
{
    public class DeleteOwnerCommandValidator : AbstractValidator<DeleteOwnerCommand>
    {
        public DeleteOwnerCommandValidator() {

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
