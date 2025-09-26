using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Driver.Home;
using CMPNatural.Application.Commands.Driver.Route;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Driver.Route
{
    public class DriverRouteController : BaseDriverApiController
    {
        public DriverRouteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("Dates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Route([FromQuery] DriverGetAllRouteDatesCommand command)
        {
            command.DriverId = rDriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("ByDay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> RouteByDay([FromQuery] DriverGetAllRouteByDayCommand command)
        {
            command.DriverId = rDriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetCurrent()
        {
            var result = await _mediator.Send(new DriverCurrentRouteMapCommand() { DriverId = rDriverId });
            return Ok(result);
        }

        [HttpPost("Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Start([FromBody] DriverStartRouteCommand command)
        {
            command.DriverId = rDriverId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

