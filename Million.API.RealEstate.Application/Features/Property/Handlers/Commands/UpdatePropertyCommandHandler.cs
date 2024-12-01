using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Property;
using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Commands
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePropertyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new UpdatePropertyDtoValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.PropertyDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors occurred";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var existingProperty = await _unitOfWork.Repository<PropertyEntity>().GetAsync(request.PropertyDto.Id);

                if (existingProperty == null)
                {
                    response.Success = false;
                    response.Message = "Property not found";
                    return response;
                }

                _mapper.Map(request.PropertyDto, existingProperty);
                await _unitOfWork.Repository<PropertyEntity>().UpdateAsync(request.PropertyDto.Id, existingProperty);

                response.Success = true;
                response.Message = "Property updated successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating Property";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}