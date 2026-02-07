using System;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Provider;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderServiceAppointmentLocationFileController : BaseProviderApiController
    {
        public ProviderServiceAppointmentLocationFileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("UploadBefore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Arrived([FromBody] DriverUploadBeforServiceAppointmentLocationFileCommand command)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;
            command.ProviderId = rIsDriver ? null : rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("UploadAfter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UploadAfter([FromBody] DriverUploadAfterServiceAppointmentLocationFileCommand command)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;
            command.ProviderId = rIsDriver? null: rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

