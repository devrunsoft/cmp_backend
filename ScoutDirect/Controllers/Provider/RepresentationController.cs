using CMPNatural.Application.Commands.Provider.Representation;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Provider
{
    public class RepresentationController : BaseProviderApiController
    {
        public RepresentationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new ProviderMenuRepresentationCommand() { ProviderId = rProviderId });
            return Ok(result);
        }
    }
}

