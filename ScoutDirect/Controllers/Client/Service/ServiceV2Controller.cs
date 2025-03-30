using System;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Service;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Service
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceV2Controller : BaseClientApiController
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

