using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class DriverCompleteServiceAppointmentLocationCommand : IRequest<CommandResponse<List<ServiceAppointmentLocation>>>
    {
        public long DriverId { get; set; }
        public long RouteId { get; set; }
        public List<ServiceQtyCompleting> Services { get; set; }
        public string Comment { get; set; } = string.Empty;


    }

    public class ServiceQtyCompleting
    {
        public int? Id { get; set; }

        public int? FactQty { get; set; }

        public OilQualityEnum OilQuality { get; set; }

    }
}

