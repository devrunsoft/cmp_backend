using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Service;
using CMPNatural.Application.Model;
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


        [HttpPost("Product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ProductInput input)
        {
            var result = await _mediator.Send(new AdminAddProductCommand(input));
            return Ok(result);
        }

        [HttpPut("Product/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id,[FromBody] ProductInput input)
        {
            var result = await _mediator.Send(new AdminUpdateProductCommand(input, Id));
            return Ok(result);
        }

        [HttpPost("ProductPrice/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PostPrice([FromRoute] long productId, [FromBody] ProductPriceInput input)
        {
            var result = await _mediator.Send(new AdminAddProductPriceCommand(input, productId));
            return Ok(result);
        }

        [HttpPut("ProductPrice/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutPrice([FromRoute] long Id, [FromBody] ProductPriceInput input)
        {
            var result = await _mediator.Send(new AdminUpdateProductPriceCommand(input, Id));
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

