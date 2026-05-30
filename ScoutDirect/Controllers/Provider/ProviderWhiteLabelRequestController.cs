using CMPNatural.Application.Commands.Provider.WhiteLabel;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderWhiteLabelRequestController : BaseProviderApiController
    {
        public ProviderWhiteLabelRequestController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new ProviderGetWhiteLabelRequestCommand()
            {
                ProviderId = rProviderId
            });
            return Ok(result);
        }

        [HttpGet("AvailableTenants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAvailableTenants()
        {
            var result = await _mediator.Send(new ProviderGetAvailableTenantAccessCommand());
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ProviderWhiteLabelRequestCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
