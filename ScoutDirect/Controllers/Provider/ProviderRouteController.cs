using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Driver.Route;
using CMPNatural.Application.Commands.Provider;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderRouteController : BaseProviderApiController
    {
        public ProviderRouteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoutes([FromBody] ProviderAddRouteCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminGetRouteCommand() { Id = Id });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ProviderGetAllRouteCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Start([FromBody] DriverStartRouteCommand command)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Preview/{RouteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetPreview([FromQuery] DriverPreviewRouteMapCommand command, [FromRoute] long RouteId)
        {
            command.RouteId = RouteId;
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("StartInProcces")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> StartInProcces([FromBody] DriverStartInProccessRouteCommand command)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Arrived")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Arrived([FromBody] DriverArrivedRouteCommand command)
        {
            var route = await _mediator.Send(new ProviderGetRouteCommand() { ProviderId = rProviderId, RouteId = command.RouteId });
            command.DriverId = route.Data.DriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

