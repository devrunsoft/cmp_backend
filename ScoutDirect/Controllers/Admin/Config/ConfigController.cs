using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace CMPNatural.Api.Controllers.Admin.Config
{
    public class ConfigController : BaseAdminApiController
    {
        protected readonly IMediator _mediator;
        public ConfigController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminConfigGetCommand());
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromBody] AdminConfigPutCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

