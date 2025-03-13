using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Capacity
{

    public class CapacityController : BaseAdminApiController
    {
        public CapacityController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetAllCapacityCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] CapacityInput request)
        {
            var result = await _mediator.Send(new AdminAddCapacityCommand()
            {
                Enable = request.Enable,
                Name = request.Name,
                Qty = request.Qty,
                ServiceType = request.ServiceType
            });
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminDeleteCapacityCommand()
            {
                Id = Id
            });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] CapacityInput request)
        {
            var result = await _mediator.Send(new AdminUpdateCapacityCommand()
            {
                Enable = request.Enable,
                Name = request.Name,
                Qty = request.Qty,
                ServiceType = request.ServiceType,
                Id = Id
            });
            return Ok(result);
        }

    }
}

