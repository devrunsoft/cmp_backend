using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Service;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Service
{
    public class ServiceV2Controller : BaseAdminApiController
    {
        public ServiceV2Controller(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllProductCommand());

            return Ok(result);
        }

        [HttpGet("Product/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Product([FromRoute] int Id)
        {
            var result = await _mediator.Send(new GetProductCommand() { ProductId = Id });

            return Ok(result);
        }


        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetPrice([FromRoute] int Id)
        {
            var result = await _mediator.Send(new GetProductPriceCommand() { ProductId = Id });

            return Ok(result);
        }

    }
}

