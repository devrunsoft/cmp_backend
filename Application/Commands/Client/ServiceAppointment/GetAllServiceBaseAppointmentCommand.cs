using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Base;
using ScoutDirect.Core.Base;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class GetAllBaseServiceAppointmentCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<BaseServiceAppointment>>>
    {
        public long CompanyId { get; set; }
        public LogOfServiceEnum? Status { get; set; }
    }
}

