using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Responses.ClientServiceAppointment;

namespace CMPNatural.Application
{

    public class GetServiceAppointmentByOprAddressCommand : IRequest<CommandResponse<List<ClientServiceAppointment>>>
    {
        public long CompanyId { get; set; }

        public long OperationalAddressId { get; set; }

    }
}

