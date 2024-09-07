using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.OperationalAddress;
using CMPNatural.Application.Handlers;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Auth.Register
{
    [Route("api/[controller]")]
    public class OperationalAddressController : BaseApiController
    {
        public OperationalAddressController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetOperationalAddressCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOperationalAddressCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] OperationalAddressInput request)
        {
            var result = await _mediator.Send(new AddOperationalAddressCommand()
            {
                Address = request.Address,
                BusinessId = request.BusinessId,
                CompanyId = rCompanyId,
                County = request.County,
                CrossStreet = request.CrossStreet,
                FirstName = request.FirstName,
                LastName = request.LastName,
                LocationPhone = request.LocationPhone,
                Long=request.Long,
                Lat=request.Lat,

            });

            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] OperationalAddressInput request)
        {
            var result = await _mediator.Send(new EditOperationalAddressCommand()
            {
                Address = request.Address,
                BusinessId = request.BusinessId,
                CompanyId = rCompanyId,
                County = request.County,
                CrossStreet = request.CrossStreet,
                FirstName = request.FirstName,
                LastName = request.LastName,
                LocationPhone = request.LocationPhone,
                Long = request.Long,
                Lat = request.Lat,
                Id =Id

            });

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteOperationalAddressCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }

    }
}

