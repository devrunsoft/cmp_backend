using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Responses.Driver;

namespace CMPNatural.Application
{
    public class DriverUploadAfterServiceAppointmentLocationFileCommand : IRequest<CommandResponse<ServiceCompletingResponse>>
    {
        public long DriverId { get; set; }
        public long RouteId { get; set; }
        public string firstPic { get; set; }
        public string secondPic { get; set; }
        public string thirdPic { get; set; }
        public string forthPic { get; set; }
    }
}

