using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.CompanyContract
{
    public class CompanyContractController : BaseApiController
    {
        public CompanyContractController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllCompanyContractCommand() { CompanyId = rCompanyId});
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetAllCompanyContractCommand() { CompanyId = rCompanyId });
            return Ok(result);
        }

        [HttpPut("sign/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sign([FromRoute] long Id , [FromBody] SignCompanyContractCommand command)
        {
            var result = await _mediator.Send(new SignCompanyContractCommand() { CompanyId = rCompanyId , CompanyContractId = Id  , Sign = command .Sign});
            return Ok(result);
        }
    }
}

