using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyImage;
using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Commands
{
    public class UpdatePropertyImageCommandHandler : IRequestHandler<UpdatePropertyImageCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePropertyImageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new UpdatePropertyImageDtoValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.PropertyImageDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors occurred";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var existingPropertyImage = await _unitOfWork.Repository<PropertyImageEntity>().GetAsync(request.PropertyImageDto.Id);

                if (existingPropertyImage == null)
                {
                    response.Success = false;
                    response.Message = "Property Image not found";
                    return response;
                }

                _mapper.Map(request.PropertyImageDto, existingPropertyImage);
                await _unitOfWork.Repository<PropertyImageEntity>().UpdateAsync(request.PropertyImageDto.Id, existingPropertyImage);

                response.Success = true;
                response.Message = "Property Image updated successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating Property Image";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}