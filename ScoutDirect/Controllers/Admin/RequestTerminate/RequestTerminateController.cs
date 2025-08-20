using CMPNatural.Application.Command;
using CMPNatural.Application.Commands.Client;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.RequestTerminate
{
	public class AdminRequestTerminateController : BaseAdminApiController
    {
        public AdminRequestTerminateController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminRequestTerminateGetAllCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{clientId}/OperationalAddress/{operationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAllByOpr([FromQuery] ClientRequestTerminateGetAllCommand command, [FromRoute] long clientId, [FromRoute] long OperationalAddressId)
        {
            command.OperationalAddressId = OperationalAddressId;
            command.CompanyId = clientId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Client")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PostClient([FromBody] ClientRequestTerminateAddCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ClientRequestTerminateAddCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminRequestTerminateGetCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPost("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Terminate([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminRequestTerminatePostCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Updated([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminRequestTerminatePutCommand() { Id = Id });
            return Ok(result);
        }
    }
}

