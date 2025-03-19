
using CMPNatural.Application;
using CMPNatural.Application.Commands.OperationalAddress;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationalAddressDashboardController : BaseClientApiController
    {
        public OperationalAddressDashboardController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetByIdServiceOperationalAddressCommand()
            {
                CompanyId = rCompanyId,
                Id = Id

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
                Long = request.Long,
                Lat = request.Lat,
                Name = request.Name,

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
                Name = request.Name,
                Id = Id

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

