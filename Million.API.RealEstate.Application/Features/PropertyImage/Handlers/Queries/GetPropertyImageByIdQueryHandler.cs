using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyImage;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Queries
{
    public class GetPropertyImageByIdQueryHandler : IRequestHandler<GetPropertyImageByIdQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPropertyImageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetPropertyImageByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var propertyImage = await _unitOfWork.Repository<PropertyImageEntity>().GetAsync(request.Id);

                if (propertyImage == null)
                {
                    response.Success = false;
                    response.Message = "Property Image not found";
                    return response;
                }

                response.Success = true;
                response.Message = "Property Image retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyImageDetails",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyImage)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Property Image";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}