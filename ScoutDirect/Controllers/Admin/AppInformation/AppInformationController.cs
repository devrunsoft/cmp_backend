using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.AppInformation
{
    [Authorize(Roles = "SuperAdmin")]
    public class AppInformationController : BaseAdminApiController
    {
        protected readonly IMediator _mediator;
        public AppInformationController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminAppInformationGetCommand());
            return Ok(result);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromBody] AdminAppInformationPutCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}

