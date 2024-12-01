using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Queries;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear una nueva imagen de propiedad
        /// </summary>
        [HttpPost("CreatePropertyImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreatePropertyImage([FromBody] CreatePropertyImageCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Obtener una imagen de propiedad por ID
        /// </summary>
        [HttpGet("GetPropertyImageById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<BaseCommandResponse>> GetPropertyImageById(string id)
        {
            var response = await _mediator.Send(new GetPropertyImageByIdQuery { Id = id });

            return Ok(response);
        }

        /// <summary>
        /// Obtener todas las imágenes de propiedades
        /// </summary>
        [HttpGet("GetAllPropertyImages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetAllPropertyImages()
        {
            var response = await _mediator.Send(new GetAllPropertyImagesQuery());

            return Ok(response);
        }

        /// <summary>
        /// Actualizar una imagen de propiedad existente
        /// </summary>
        [HttpPut("UpdatePropertyImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdatePropertyImage([FromBody] UpdatePropertyImageCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Eliminar una imagen de propiedad por ID
        /// </summary>
        [HttpDelete("DeletePropertyImage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeletePropertyImage(string id)
        {
            var response = await _mediator.Send(new DeletePropertyImageCommand { Id = id });

            return Ok(response);
        }
    }
}