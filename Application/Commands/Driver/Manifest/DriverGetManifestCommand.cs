using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class DriverGetManifestCommand : IRequest<CommandResponse<Manifest>>
    {
		public long DriverId { get; set; }
		public long Id { get; set; }
	}
}

