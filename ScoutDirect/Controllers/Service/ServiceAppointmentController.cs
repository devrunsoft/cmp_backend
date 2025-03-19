
using CMPNatural.Application;
using CMPNatural.Application.Commands.OperationalAddress;
using CMPNatural.Application.Commands.ServiceAppointment;
using CMPNatural.Application.Model;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.ServiceAppointment
{

    [ApiController]
    [Route("api/[controller]")]
    public class ServiceAppointmentController : BaseClientApiController
    {
        public ServiceAppointmentController(IMediator mediator) : base(mediator)
        {
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ServiceAppointmentInput request)
        {
            var checkData = await _mediator.Send(new GetServiceAppointmentByOprAddressCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = request.OperationalAddressId
            });

            if ((checkData.Data.Any(p=>p.ProductPriceId==request.ProductPriceId)) || (!checkData.IsSucces()))
            {
                return Ok(new NoAcess() { Message="You have an active service at the current operational address"});
            }
            var result = await _mediator.Send(new AddServiceAppointmentCommand()
            {
                CompanyId = rCompanyId,
                FrequencyType = request.FrequencyType,
                ServiceTypeId = request.ServiceTypeId,
                StartDate = request.StartDate,
                OperationalAddressId = request.OperationalAddressId

            });
            return Ok(result);
        }


        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetServiceAppointmentCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }

        [HttpGet("OperationalAddress/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetByOprAddress([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetServiceAppointmentByOprAddressCommand()
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
            var result = await _mediator.Send(new GetServiceAppointmentByOprAddressAndServiceTypeCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = Id,
                ServiceTypeId=ServiceTypeId

            });

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteServiceAppointmentCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }
    }
}

