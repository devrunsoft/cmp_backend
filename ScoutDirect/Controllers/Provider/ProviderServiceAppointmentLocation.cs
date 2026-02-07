using System;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Provider;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderServiceAppointmentLocation : BaseProviderApiController
    {
        public ProviderServiceAppointmentLocation(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("CompleteQtyServices/{RouteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CompleteQtyServices([FromRoute] long RouteId)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = RouteId });
            var result = await _mediator.Send(new DriverGetServiceAppointmentLocationCommand() { DriverId = route.Data.DriverId, RouteId = RouteId });
            return Ok(result);
        }

        [HttpPost("CompleteQtyServices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CompleteService([FromBody] DriverCompleteServiceAppointmentLocationCommand command)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

