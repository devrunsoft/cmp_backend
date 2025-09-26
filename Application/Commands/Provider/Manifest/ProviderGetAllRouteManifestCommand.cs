using System;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class ProviderGetAllRouteManifestCommand: IRequest<CommandResponse<List<Manifest>>>
    {
		public ProviderGetAllRouteManifestCommand()
		{
		}
        public long ProviderId { get; set; }
    }
}

