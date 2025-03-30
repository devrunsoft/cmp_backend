using System;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AddServiceAppointmentEmergencyCommand : ServiceAppointmentEmergencyInput, IRequest<CommandResponse<ServiceAppointmentEmergency>>
    {
        public long CompanyId { get; set; }

    }
}

