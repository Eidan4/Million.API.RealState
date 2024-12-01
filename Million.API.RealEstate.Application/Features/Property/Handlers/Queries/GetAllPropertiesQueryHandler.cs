using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.Features.Property.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Property;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Queries
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPropertiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var properties = await _unitOfWork.Repository<PropertyEntity>().GetAllAsync();

                response.Success = true;
                response.Message = "Properties retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyList",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(properties)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Properties";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}