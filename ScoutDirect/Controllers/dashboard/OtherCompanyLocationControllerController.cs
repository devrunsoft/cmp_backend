using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Auth.dashboard
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtherCompanyLocationController : CmpBaseController
    {
        public OtherCompanyLocationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetLocationCompanyCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }


        [HttpGet("OperationalAddress/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> OperationalAddress([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetLocationCompanyCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = Id
            });
            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] LocationCompanyInput input)
        {
            var result = await _mediator.Send(new AddLocationCompanyCommand()
            {
                Capacity = input.Capacity,
                Comment = input.Comment,
                CompanyId = rCompanyId,
                Lat = input.Lat,
                Long = input.Long,
                Name = input.Name,
                PrimaryFirstName = input.PrimaryFirstName,
                PrimaryLastName = input.PrimaryLastName,
                PrimaryPhonNumber = input.PrimaryPhonNumber,
                Type = input.Type,
                OperationalAddressId = input.OperationalAddressId,
                CapacityId = input.CapacityId

            });

            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] LocationCompanyInput input)
        {
            var result = await _mediator.Send(new EditLocationCompanyCommand()
            {
                Id = Id,
                Capacity = input.Capacity,
                Comment = input.Comment,
                CompanyId = rCompanyId,
                Lat = input.Lat,
                Long = input.Long,
                Name = input.Name,
                PrimaryFirstName = input.PrimaryFirstName,
                PrimaryLastName = input.PrimaryLastName,
                PrimaryPhonNumber = input.PrimaryPhonNumber,
                Type = input.Type,
                CapacityId = input.CapacityId

            });

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteLocationCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }
    }
}

