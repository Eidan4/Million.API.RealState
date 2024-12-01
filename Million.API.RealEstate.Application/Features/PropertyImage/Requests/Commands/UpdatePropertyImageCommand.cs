using MediatR;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.PropertyImage;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands
{
    public class UpdatePropertyImageCommand : IRequest<BaseCommandResponse>
    {
        public PropertyImageDto PropertyImageDto { get; set; }
    }
}