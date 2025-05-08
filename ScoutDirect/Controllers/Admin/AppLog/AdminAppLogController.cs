using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.AppLog
{
    public class AdminAppLogController : BaseAdminApiController
    {
        public AdminAppLogController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetAllLogCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

