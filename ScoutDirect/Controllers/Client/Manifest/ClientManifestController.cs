using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Client.Manifest
{
    public class ClientManifestController : BaseClientApiController
    {
        public ClientManifestController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("OperationalAddress/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ClientGetPaginateManifestCommand command)
        {
            command.CompanyId = rCompanyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{OperationalAddressId}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetId([FromRoute] long Id)
        {
            var result = await _mediator.Send(new ClientGetManifestCommand() { Id = Id });
            return Ok(result);
        }
    }
}

