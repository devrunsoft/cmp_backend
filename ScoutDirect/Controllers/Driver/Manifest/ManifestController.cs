using System;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Driver.Home;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Driver.Manifest
{
    public class ManifestController : BaseDriverApiController
    {
        public ManifestController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DriverGetManifestCommand() { DriverId = rDriverId , Id = Id});
            return Ok(result);
        }

        [HttpGet("Upcoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Upcoming()
        {
            var result = await _mediator.Send(new DriverUpComingManifestCommand() { DriverId = rDriverId });
            return Ok(result);
        }

        [HttpPut("StartRoute/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> StartRoute([FromBody] long Id)
        {
            var result = await _mediator.Send(new DriverStartRouteManifestCommand() { DriverId = rDriverId , ManifestId = Id });
            return Ok(result);
        }
    }
}

