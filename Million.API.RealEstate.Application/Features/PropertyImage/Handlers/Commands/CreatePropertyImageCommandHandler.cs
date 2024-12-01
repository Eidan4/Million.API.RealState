using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyImage;
using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Commands
{
    public class CreatePropertyImageCommandHandler : IRequestHandler<CreatePropertyImageCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePropertyImageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new PropertyImageDtoValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.PropertyImageDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors occurred";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var propertyImage = _mapper.Map<PropertyImageEntity>(request.PropertyImageDto);
                await _unitOfWork.Repository<PropertyImageEntity>().AddAsync(propertyImage);

                response.Success = true;
                response.Message = "Property Image created successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "CreatedPropertyImage",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyImage)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error creating Property Image";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}