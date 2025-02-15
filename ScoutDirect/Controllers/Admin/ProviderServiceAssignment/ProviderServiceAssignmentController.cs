using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Invoice;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.ProviderServiceAssignment
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProviderServiceAssignmentController : BaseAdminApiController
    {
        public ProviderServiceAssignmentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllProviderServiceAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost("Assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Assign([FromBody] AdminPostProviderServiceAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("ChangeStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ChangeStatus([FromBody] AdminChangeStatusProviderServiceAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

