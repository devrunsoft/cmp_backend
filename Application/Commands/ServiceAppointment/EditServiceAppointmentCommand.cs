using System;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{

    public class EditerviceAppointmentCommand : IRequest<CommandResponse<ServiceAppointment>>
    {
        public long CompanyId { get; set; }

        public string ServiceId { get; set; }

        public ServiceStatus Status { get; set; }
    }
}

