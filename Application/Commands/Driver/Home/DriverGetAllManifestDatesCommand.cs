using System;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver.Home
{
	public class DriverGetAllManifestDatesCommand : IRequest<CommandResponse<List<ManifestDatesResponse>>>
    {
        public long DriverId { get; set; }
        public DateTime Date { get; set; }
    }
}

