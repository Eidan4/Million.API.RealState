using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SharpCompress.Crypto;

namespace Million.API.RealEstate.Application.DTOs.Owner
{
    public class UpdateOwnerDtoValidator : AbstractValidator<OwnerDto>
    {
        public UpdateOwnerDtoValidator() {

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Addres is required");

            RuleFor(x => x.BirthDay)
                .NotEmpty().WithMessage("BirthDay is required");
        }
    }
}
