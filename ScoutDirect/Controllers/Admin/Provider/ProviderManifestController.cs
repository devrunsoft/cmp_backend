using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Manifest
{
    public class AdminProviderManifestController : BaseAdminProviderApiController
    {
        public AdminProviderManifestController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new ProviderGetAllActiveManifestCommand() { ProviderId = rProviderId});
            return Ok(result);
        }

        [HttpGet("Complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetComplete()
        {
            var result = await _mediator.Send(new ProviderGetAllCompleteManifestCommand() { ProviderId = rProviderId });
            return Ok(result);
        }

        [HttpPut("Complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Complete()
        {
            var result = await _mediator.Send(new ProviderGetAllCompleteManifestCommand() { ProviderId = rProviderId });
            return Ok(result);
        }

        [HttpPut("EditComplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> EditComplete()
        {
            var result = await _mediator.Send(new ProviderGetAllCompleteManifestCommand() { ProviderId = rProviderId });
            return Ok(result);
        }
    }
}

