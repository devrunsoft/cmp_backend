using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.ServiceAppointmentEmergency
{
	public class DeleteServiceAppointmentEmergencyCommand : IRequest<CommandResponse<object>>
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }
    }
}

