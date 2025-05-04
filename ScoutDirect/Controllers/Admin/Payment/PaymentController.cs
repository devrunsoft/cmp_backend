using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Payment
{
    public class AdminPaymentController : BaseAdminApiController
    {
        public AdminPaymentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

