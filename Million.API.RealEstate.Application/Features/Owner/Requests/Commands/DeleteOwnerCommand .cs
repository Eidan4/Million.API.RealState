using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Owner.Requests.Commands
{
    public class DeleteOwnerCommand : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}