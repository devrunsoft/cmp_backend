using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers
{
    public class DriverVehicleController : BaseDriverApiController
    {
        public DriverVehicleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new DriverGetAllVehicleCommand() { DriverId = rDriverId});
            return Ok(result);
        }
    }
}

