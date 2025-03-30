using System;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AddServiceAppointmentCommand : ServiceAppointmentInput, IRequest<CommandResponse<ServiceAppointment>>
    {
        public long CompanyId { get; set; }
        public long InvoiceId { get; set; }
    }
}

