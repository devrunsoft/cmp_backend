using System;
using CMPNatural.Application.Model.ServiceAppointment;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.ServiceAppointment
{
	public class EditServiceAppointmentCommand : ServiceAppointmentInput, IRequest<CommandResponse<object>>
    {
        public long CompanyId { get; set; }

        public long Id { get; set; }
    }
}

