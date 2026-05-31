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
    }
}
