using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Features.Owner.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Owner;
using Newtonsoft.Json;


namespace Million.API.RealEstate.Application.Features.Owner.Handlers.Queries
{
    public class GetAllOwnersQueryHandler : IRequestHandler<GetAllOwnersQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOwnersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var owners = await _unitOfWork.Repository<OwnerEntity>().GetAllAsync();

                response.Success = true;
                response.Message = "Owners retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "OwnerList",
                        Value = JsonConvert.SerializeObject(owners)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Owners";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
