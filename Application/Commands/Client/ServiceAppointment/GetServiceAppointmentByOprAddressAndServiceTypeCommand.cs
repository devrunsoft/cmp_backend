using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class GetServiceAppointmentByOprAddressAndServiceTypeCommand : IRequest<CommandResponse<List<ServiceAppointment>>>
    {
        public long CompanyId { get; set; }

        public long OperationalAddressId { get; set; }

        public string ServiceTypeId { get; set; }

    }
}

