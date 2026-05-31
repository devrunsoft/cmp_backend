using CMPNatural.Api.Controllers.Admin;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class WhiteLabelRequestController : BaseAdminApiController
    {
        public WhiteLabelRequestController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllWhiteLabelRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Review/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Review([FromRoute] long id, [FromBody] AdminReviewWhiteLabelRequestCommand command)
        {
            command.WhiteLabelRequestId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Accept/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Accept([FromRoute] long id, [FromBody] AdminReviewWhiteLabelRequestCommand command)
        {
            command.WhiteLabelRequestId = id;
            command.Status = CMPNatural.Core.Enums.WhiteLabelStatus.Accepted;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Reject/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Reject([FromRoute] long id, [FromBody] AdminReviewWhiteLabelRequestCommand command)
        {
            command.WhiteLabelRequestId = id;
            command.Status = CMPNatural.Core.Enums.WhiteLabelStatus.Rejected;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
