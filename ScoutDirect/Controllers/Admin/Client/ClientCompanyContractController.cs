using System;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientCompanyContractController : BaseAdminClientApiController
    {
        public ClientCompanyContractController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("OperationalAddress/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetOpr([FromRoute] long OperationalAddressId , [FromQuery] GetAllCompanyContractCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
