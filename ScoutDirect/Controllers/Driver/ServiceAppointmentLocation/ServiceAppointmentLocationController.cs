using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Driver.ServiceAppointmentLocation
{
    public class ServiceAppointmentLocationController : BaseDriverApiController
    {
        public ServiceAppointmentLocationController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet("CompleteQtyServices/{RouteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CompleteQtyServices([FromRoute] long RouteId)
        {
            var result = await _mediator.Send(new DriverGetServiceAppointmentLocationCommand() { DriverId = rDriverId , RouteId = RouteId });
            return Ok(result);
        }

        [HttpPost("CompleteQtyServices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CompleteService([FromBody] DriverCompleteServiceAppointmentLocationCommand command)
        {
            command.DriverId = rDriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

