using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CheckVehicleByLicenseNumberCommand : IRequest<CommandResponse<List<Vehicle>>>
    {
        public string LicenseNumber { get; set; }
    }
}
