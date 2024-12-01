using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Features.Property.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Newtonsoft.Json;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Queries
{
    public class GetPropertiesWithImagesQueryHandler : IRequestHandler<GetPropertiesWithImagesQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPropertiesWithImagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(GetPropertiesWithImagesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Obtener propiedades con imágenes desde el repositorio
                var propertiesWithImages = await _unitOfWork.PropertyRepository.GetPropertiesWithImagesAsync(request.Filters);

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