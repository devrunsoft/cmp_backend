using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DriverUploadBeforServiceAppointmentLocationFileCommand : IRequest<CommandResponse<List<ServiceAppointmentLocationFile>>>
    {
        public long? ProviderId { get; set; }
        public long DriverId { get; set; }
        public long RouteId { get; set; }
        public string firstPic { get; set; }
        public string secondPic { get; set; }
        public string thirdPic { get; set; }
        public string forthPic { get; set; }
    }
}

