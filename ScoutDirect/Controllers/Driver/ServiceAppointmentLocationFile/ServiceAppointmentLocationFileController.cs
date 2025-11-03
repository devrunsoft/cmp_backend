using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Driver.Route;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Driver.ServiceAppointmentLocationFile
{
    public class ServiceAppointmentLocationFileController : BaseDriverApiController
    {
        public ServiceAppointmentLocationFileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("UploadBefore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Arrived([FromBody] DriverUploadBeforServiceAppointmentLocationFileCommand command)
        {
            command.DriverId = rDriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("UploadAfter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UploadAfter([FromBody] DriverUploadAfterServiceAppointmentLocationFileCommand command)
        {
            command.DriverId = rDriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

