using MediatR;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.Property;

namespace Million.API.RealEstate.Application.Features.Property.Requests.Commands
{
    public class CreatePropertyCommand : IRequest<BaseCommandResponse>
    {
        public PropertyDto PropertyDto { get; set; }
    }
}