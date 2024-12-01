using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands
{
    public class DeletePropertyTraceCommand : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}