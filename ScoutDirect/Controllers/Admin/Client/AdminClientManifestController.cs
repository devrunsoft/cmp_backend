using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Client.Manifest
{
    public class AdminClientManifestController : BaseAdminClientApiController
    {
        public AdminClientManifestController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("OperationalAddress/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ClientGetPaginateManifestCommand command, [FromRoute] long OperationalAddressId)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

