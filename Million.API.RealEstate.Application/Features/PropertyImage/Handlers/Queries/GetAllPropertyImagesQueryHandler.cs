using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyImage;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Queries
{
    public class GetAllPropertyImagesQueryHandler : IRequestHandler<GetAllPropertyImagesQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPropertyImagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(GetAllPropertyImagesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var propertyImages = await _unitOfWork.Repository<PropertyImageEntity>().GetAllAsync();

                response.Success = true;
                response.Message = "Property Images retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyImageList",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyImages)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Property Images";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
