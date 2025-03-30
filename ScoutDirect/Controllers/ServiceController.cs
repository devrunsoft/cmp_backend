using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Service;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Service
{
    [Authorize]
    public class ServiceProductController : BaseApiController
    {
        public ServiceProductController(IMediator mediator) : base(mediator)
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


        [HttpGet("GetPaging")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetPaging([FromQuery] GetAllProductPaginateCommand command)
        {
            var result = await _mediator.Send(command);

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

        [HttpGet("GetPaging/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetPricePaging([FromRoute] int Id, [FromQuery] GetProductPricePaginateCommand command)
        {
            command.ProductId = Id;
            var result = await _mediator.Send(command);

            return Ok(result);
        }

    }
}

