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
    public class GetOwnerByIdQueryHandler : IRequestHandler<GetOwnerByIdQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOwnerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Obtener el Owner desde el repositorio
                var owner = await _unitOfWork.Repository<OwnerEntity>().GetAsync(request.Id);

                if (owner == null)
                {
                    response.Success = false;
                    response.Message = "Owner not found";
                    return response;
                }

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Owner retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "OwnerDetails",
                        Value = JsonConvert.SerializeObject(owner)
                    }
                };
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error retrieving Owner";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}