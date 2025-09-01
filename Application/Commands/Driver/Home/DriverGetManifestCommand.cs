using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver.Home
{
	public class DriverGetManifestDateCommand : IRequest<CommandResponse<List<Manifest>>>
    {
        public long DriverId { get; set; }
        public DateTime Date { get; set; }
    }
}