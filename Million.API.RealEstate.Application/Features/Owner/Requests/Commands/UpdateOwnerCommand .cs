using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Owner.Requests.Commands
{
    public class UpdateOwnerCommand : IRequest<BaseCommandResponse>
    {
        public OwnerDto OwnerDto { get; set; }
    }
}
