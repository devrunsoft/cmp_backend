using CMPNatural.Api.Attribute;
using CMPNatural.Application.Commands.Provider;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;


namespace CMPNatural.Api.Controllers.Provider
{
    [ProviderAuthorize()]
    public class ProviderHomeController : BaseProviderApiController
    {
        public ProviderHomeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetReport()
        {
            var result = await _mediator.Send(new ProviderGetReportCommand() { ProviderId = rProviderId });
            return Ok(result);
        }
    }
}

