using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using Million.API.RealEstate.Application.Features.Owner.Requests.Queries;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateOwner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreateOwner([FromBody] OwnerCommand ownerCommand)
        {
            var response = await _mediator.Send(ownerCommand);

            return Ok(response);
        }

        [HttpGet("GetOwnerById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetOwnerById(string id)
        {
            var response = await _mediator.Send(new GetOwnerByIdQuery { Id = id });

            return Ok(response);
        }

        [HttpGet("GetAllOwners")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetAllOwners()
        {
            var response = await _mediator.Send(new GetAllOwnersQuery());

            return Ok(response);
        }

        [HttpPut("UpdateOwner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateOwner([FromBody] UpdateOwnerCommand updateOwnerCommand)
        {
            var response = await _mediator.Send(updateOwnerCommand);

            return Ok(response);
        }

        [HttpDelete("DeleteOwner/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteOwner(string id)
        {
            var response = await _mediator.Send(new DeleteOwnerCommand { Id = id });

            return Ok(response);
        }
    }
}
