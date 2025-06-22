using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminProcessingManifestCommand : IRequest<CommandResponse<Manifest>>
    {
		public AdminProcessingManifestCommand()
		{
		}
		public long Id { get; set; }
	}
}

