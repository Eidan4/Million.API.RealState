using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyTraceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyTraceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear un nuevo registro de trazabilidad de propiedad
        /// </summary>
        [HttpPost("CreatePropertyTrace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreatePropertyTrace([FromBody] CreatePropertyTraceCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Obtener un registro de trazabilidad de propiedad por ID
        /// </summary>
        [HttpGet("GetPropertyTraceById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetPropertyTraceById(string id)
        {
            var response = await _mediator.Send(new GetPropertyTraceByIdQuery { Id = id });

            return Ok(response);
        }

        [HttpGet("GetPropertyTraceByPropertyId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetPropertyTraceByPropetyIdQuery(string id)
        {
            var response = await _mediator.Send(new GetPropertyTraceByPropetyIdQuery { Id = id });

            return Ok(response);
        }

        /// <summary>
        /// Obtener todos los registros de trazabilidad de propiedades
        /// </summary>
        [HttpGet("GetAllPropertyTraces")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetAllPropertyTraces()
        {
            var response = await _mediator.Send(new GetAllPropertyTracesQuery());

            return Ok(response);
        }

        /// <summary>
        /// Actualizar un registro de trazabilidad de propiedad existente
        /// </summary>
        [HttpPut("UpdatePropertyTrace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdatePropertyTrace([FromBody] UpdatePropertyTraceCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return StatusCode(response.StatsCode, response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Eliminar un registro de trazabilidad de propiedad por ID
        /// </summary>
        [HttpDelete("DeletePropertyTrace/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeletePropertyTrace(string id)
        {
            var response = await _mediator.Send(new DeletePropertyTraceCommand { Id = id });

            return Ok(response);
        }
    }
}