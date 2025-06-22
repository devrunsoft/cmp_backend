using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminCancelManifestCommand : IRequest<CommandResponse<Manifest>>
    {
		public long Id { get; set; }
	}
}

