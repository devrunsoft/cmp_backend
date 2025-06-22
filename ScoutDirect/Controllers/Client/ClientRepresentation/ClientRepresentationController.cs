using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Client.Representation;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Representation
{
    public class ClientRepresentationController : BaseClientApiController
    {

        public ClientRepresentationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long OperationalAddressId)
        {
            var result = await _mediator.Send(new ClientMenuRepresentationCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = OperationalAddressId
            });
            return Ok(result);
        }
    }
}

