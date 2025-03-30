using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.OperationalAddress;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Auth.Register
{
    [Route("api/[controller]")]
    public class BusinessTypeController : BaseApiController
    {
        protected readonly IMediator _mediator;

        public BusinessTypeController(IMediator mediator) : base (mediator)
        {
            _mediator = mediator;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            //SendTestAdmin("salam","khobi?");
            return Ok(true);
        }

    }
}

