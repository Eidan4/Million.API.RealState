using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.Features.Property.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Property;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Queries
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var property = await _unitOfWork.Repository<PropertyEntity>().GetAsync(request.Id);

                if (property == null)
                {
                    response.Success = false;
                    response.Message = "Property not found";
                    return response;
                }

                response.Success = true;
                response.Message = "Property retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyDetails",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(property)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Property";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}