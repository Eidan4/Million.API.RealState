using MediatR;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands
{
    public class CreatePropertyTraceCommand : IRequest<BaseCommandResponse>
    {
        public PropertyTraceDto PropertyTraceDto { get; set; }
    }
}
