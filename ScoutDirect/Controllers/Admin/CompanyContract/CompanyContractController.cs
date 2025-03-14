using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.CompanyContract
{
    public class CompanyContractController : BaseAdminApiController
    {
        public CompanyContractController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetPaginateCompanyContractCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("sign/{clientId}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sign([FromRoute] long Id, [FromRoute] long clientId)
        {
            var result = await _mediator.Send(new AdminSignCompanyContractCommand() { CompanyId = clientId, CompanyContractId = Id });
            return Ok(result);
        }

    }
}

