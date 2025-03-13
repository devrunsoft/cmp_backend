using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Contract
{
    public class ContractController : BaseAdminApiController
    {
        public ContractController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetAllContractCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ContractInput request)
        {
            var result = await _mediator.Send(new AdminAddContractCommand(request));
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminDeleteContractCommand()
            {
                Id = Id
            });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] ContractInput request)
        {
            var result = await _mediator.Send(new AdminEditContractCommand(request,Id));
            return Ok(result);
        }

    }
}

