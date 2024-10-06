using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.ServiceAppointmentEmergency
{
	public class DeleteServiceAppointmentEmergency : IRequest<CommandResponse<object>>
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }
    }
}

