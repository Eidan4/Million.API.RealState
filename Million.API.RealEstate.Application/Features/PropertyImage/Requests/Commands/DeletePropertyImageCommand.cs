using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands
{
    public class DeletePropertyImageCommand : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}