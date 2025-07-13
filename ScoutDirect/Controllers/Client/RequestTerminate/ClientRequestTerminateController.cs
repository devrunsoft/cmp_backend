using CMPNatural.Application.Command;
using CMPNatural.Application.Commands.Client;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Admin.RequestTerminate
{
	public class ClientRequestTerminateController : BaseClientApiController
    {
        public ClientRequestTerminateController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{operationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] ClientRequestTerminateGetAllCommand command, [FromRoute] long OperationalAddressId)
        {
            command.OperationalAddressId = OperationalAddressId;
            command.CompanyId = rCompanyId;
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
    }
}

