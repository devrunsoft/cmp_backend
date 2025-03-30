using System;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BilingInformationController : BaseClientApiController
    {
        public BilingInformationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {

            var result = await _mediator.Send(new GetBilingInformationCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] BilingInformationInput input)
        {
            
            var result = await _mediator.Send(new AddBilingInformationCommand()
            {
                Address=input.Address,
                CardholderName=input.CardholderName,
                CardNumber=input.CardNumber,
                City=input.City,
                CompanyId=rCompanyId,
                CVC=input.CVC,
                Expiry=input.Expiry,
                IsPaypal=input.IsPaypal,
                State=input.State,
                ZIPCode=input.ZIPCode

            });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id,[FromForm] BilingInformationInput input)
        {

            var result = await _mediator.Send(new EditBilingInformationCommand()
            {
                Id= Id,
                Address = input.Address,
                CardholderName = input.CardholderName,
                CardNumber = input.CardNumber,
                City = input.City,
                CompanyId = rCompanyId,
                CVC = input.CVC,
                Expiry = input.Expiry,
                IsPaypal = input.IsPaypal,
                State = input.State,
                ZIPCode = input.ZIPCode

            });
            return Ok(result);
        }


        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {

            var result = await _mediator.Send(new DeleteBilingInformationCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }
    }
}

