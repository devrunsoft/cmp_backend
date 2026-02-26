using CMPNatural.Application;
using CMPNatural.Application.Commands.OperationalAddress;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientLocationController : BaseAdminClientApiController
    {
        public ClientLocationController(IMediator mediator , IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
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
        public async Task<ActionResult> GetAll([FromQuery] GetAllOperationalAddressCommand command)
        {
            command.CompanyId = rCompanyId;
            var result = await _mediator.Send(command);
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
                LocationDateTimeInputs = request.LocationDateTimeInputs,
                Password = request.Password,
                Username = request.Username

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
                Id = Id,
                LocationDateTimeInputs = request.LocationDateTimeInputs,
                Password = request.Password,
                Username = request.Username

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

