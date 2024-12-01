using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Features.Property.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Newtonsoft.Json;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Queries
{
    public class GetPropertiesWithImagesByIdQueryHandler : IRequestHandler<GetPropertiesWithImagesByIdQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPropertiesWithImagesByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(GetPropertiesWithImagesByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Obtener propiedades con imágenes desde el repositorio
                var propertiesWithImages = await _unitOfWork.PropertyRepository.GetPropertyWithImagesByIdAsync(request.Id);

                response.Success = true;
                response.Message = "Properties with images retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertiesWithImages",
                        Value = JsonConvert.SerializeObject(propertiesWithImages)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving properties with images";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}