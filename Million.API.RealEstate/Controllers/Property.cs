using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;
using Million.API.RealEstate.Application.Features.Property.Requests.Queries;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear una nueva propiedad
        /// </summary>
        [HttpPost("CreateProperty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreateProperty([FromBody] CreatePropertyCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Obtener una propiedad por ID
        /// </summary>
        [HttpGet("GetPropertyById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetPropertyById(string id)
        {
            var response = await _mediator.Send(new GetPropertyByIdQuery { Id = id });

            return Ok(response);
        }

        /// <summary>
        /// Obtener todas las propiedades
        /// </summary>
        [HttpGet("GetAllProperties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetAllProperties()
        {
            var response = await _mediator.Send(new GetAllPropertiesQuery());

            return Ok(response);
        }

        /// <summary>
        /// Obtener todas las propiedades por filtro
        /// </summary>
        [HttpGet("GetPropertiesWithImageByFilter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetPropertiesWithImages([FromQuery] PropertyFilterDto filters)
        {
            var response = await _mediator.Send(new GetPropertiesWithImagesQuery
            {
                Filters = filters
            });


            return Ok(response);
        }

        [HttpGet("GetPropertiesWithImageById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetPropertiesWithImagesById(string id)
        {
            var response = await _mediator.Send(new GetPropertiesWithImagesByIdQuery { Id = id });

            return Ok(response);
        }

        /// <summary>
        /// Actualizar una propiedad existente
        /// </summary>
        [HttpPut("UpdateProperty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateProperty([FromBody] UpdatePropertyCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Eliminar una propiedad por ID
        /// </summary>
        [HttpDelete("DeleteProperty/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteProperty(string id)
        {
            var response = await _mediator.Send(new DeletePropertyCommand { Id = id });

            return Ok(response);
        }
    }
}