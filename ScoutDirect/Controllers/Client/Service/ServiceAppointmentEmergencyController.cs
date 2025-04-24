
using CMPNatural.Application;
using CMPNatural.Application.Commands.ServiceAppointment;
using CMPNatural.Application.Commands.ServiceAppointmentEmergency;
using CMPNatural.Application.Model.ServiceAppointment;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.ServiceAppointment
{

    [ApiController]
    [Route("api/[controller]")]
    public class ServiceAppointmentEmergencyController : BaseClientApiController
    {
        public ServiceAppointmentEmergencyController(IMediator mediator) : base(mediator)
        {
        }


        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[EnableCors("AllowOrigin")]
        //public async Task<ActionResult> Post([FromBody] ServiceAppointmentEmergencyInput request)
        //{
        //    var result = await _mediator.Send(new AddServiceAppointmentCommand()
        //    {
        //        CompanyId = rCompanyId,
        //        FrequencyType = request.FrequencyType,
        //        //LocationCompanyId = request.LocationCompanyId,
        //        ServiceTypeId = request.ServiceTypeId,
        //        OperationalAddressId = request.OperationalAddressId

        //    });

        //    return Ok(result);
        //}


        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetServiceAppointmentEmergencyCommand()
            {
                CompanyId = rCompanyId,
                Id = Id

            });

            return Ok(result);
        }

        [HttpGet("OperationalAddress/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetByOprAddress([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetEmergencyServiceAppointmentByOprAddressCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = Id

            });

            return Ok(result);
        }


        [HttpGet("OperationalAddress/{Id}/{ServiceTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetByOprAddressAndServiceType([FromRoute] long Id, [FromRoute] string ServiceTypeId)
        {
            var result = await _mediator.Send(new GetEmergencyServiceAppointmentByOprAddressAndServiceTypeIdCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = Id,
                ServiceTypeId = ServiceTypeId

            });

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteServiceAppointmentEmergencyCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }
    }
}

