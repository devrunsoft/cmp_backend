using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class GetEmergencyServiceAppointmentByOprAddressAndServiceTypeIdCommand : IRequest<CommandResponse<List<ServiceAppointmentEmergency>>>
    {
        public long CompanyId { get; set; }

        public long OperationalAddressId { get; set; }

        public string ServiceTypeId { get; set; }

    }
}

