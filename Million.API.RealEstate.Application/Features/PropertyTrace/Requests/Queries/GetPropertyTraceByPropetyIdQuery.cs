using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries
{
    public class GetPropertyTraceByPropetyIdQuery: IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}