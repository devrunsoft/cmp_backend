using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using CMPNatural.Application.Model.ServiceAppointment;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.ServiceAppointment
{

    public class ServiceAppointmentEmergencyController : CmpBaseController
    {
        public ServiceAppointmentEmergencyController(IMediator mediator) : base(mediator)
        {
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ServiceAppointmentEmergencyInput request)
        {
            var result = await _mediator.Send(new AddServiceAppointmentCommand()
            {
                CompanyId = rCompanyId,
                FrequencyType = request.FrequencyType,
                //LocationCompanyId = request.LocationCompanyId,
                ServiceTypeId = request.ServiceTypeId,
                OperationalAddressId = request.OperationalAddressId

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
    }
}

