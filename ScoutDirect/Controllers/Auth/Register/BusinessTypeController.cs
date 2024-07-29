using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.OperationalAddress;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Auth.Register
{
    [Route("api/[controller]")]
    public class BusinessTypeController : Controller
    {
        protected readonly IMediator _mediator;

        public BusinessTypeController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllBusinessTypeCommand()
            {
            });
            return Ok(result);
        }

    }
}

