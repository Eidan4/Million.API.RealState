using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Property.Requests.Commands
{
    public class DeletePropertyCommand : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; } // ID de la propiedad a eliminar
    }
}