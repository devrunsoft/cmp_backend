using CMPNatural.Api.Controllers.Admin;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class TenantController : BaseAdminApiController
    {
        public TenantController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllTenantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{tenantId}/VerifyHost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> VerifyHost([FromRoute] long tenantId)
        {
            var result = await _mediator.Send(new AdminVerifyTenantHostCommand
            {
                TenantId = tenantId
            });
            return Ok(result);
        }

        [HttpGet("Domain/{tenantDomainId}/VerifyHost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> VerifyDomainHost([FromRoute] long tenantDomainId)
        {
            var result = await _mediator.Send(new AdminVerifyTenantDomainHostCommand
            {
                TenantDomainId = tenantDomainId
            });
            return Ok(result);
        }
    }
}
