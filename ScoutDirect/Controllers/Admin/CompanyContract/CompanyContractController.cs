using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


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

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetById([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminGetCompanyContractCommand() { Id = Id });
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

        [HttpPut("send/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Send([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminSendCompanyContractCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("{Id}/contract/{contractId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromRoute] long contractId)
        {
            var result = await _mediator.Send(new AdminUpdateCompanyContractCommand() { ContractId = contractId, CompanyContractId = Id });
            return Ok(result);
        }
    }
}

