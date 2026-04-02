using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Provider.Driver;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Driver
{
    public class DriverController : BaseAdminApiController
    {
        public DriverController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("CheckByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckByEmail([FromBody] CheckDriverByEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

