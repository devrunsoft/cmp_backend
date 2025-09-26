using CMPNatural.Application;
using CMPNatural.Application.Commands.Driver.Home;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Driver.Home
{
    public class HomeController : BaseDriverApiController
    {
        public HomeController(IMediator mediator) : base(mediator)
        {
        }

        //[HttpGet("Manifest/Dates")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[EnableCors("AllowOrigin")]
        //public async Task<ActionResult> Get([FromQuery] DriverGetAllManifestDatesCommand command)
        //{
        //    command.DriverId = rDriverId;
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

    }
}

