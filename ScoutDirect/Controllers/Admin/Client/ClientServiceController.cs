using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientServiceController : BaseAdminClientApiController
    {
        public ClientServiceController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllBaseServiceAppointmentCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }

        [HttpGet("OperationalAddress/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetByOprAddress([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetServiceAppointmentByOprAddressCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = Id

            });

            return Ok(result);
        }
    }
}

