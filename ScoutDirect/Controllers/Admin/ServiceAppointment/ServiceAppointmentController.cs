using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.ServiceAppointment
{
    public class ServiceAppointmentController : BaseAdminApiController
    {
        public ServiceAppointmentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Cancel/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CancelService([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminCancelServiceCommand() { ServiceAppointmentId = Id });
            return Ok(result);
        }

    }
}

