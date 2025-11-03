using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Route
{
    public class AdminRouteController : BaseAdminApiController
    {
        public AdminRouteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminGetRouteCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoutes([FromBody] ProviderAddRouteCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetAllRouteCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

