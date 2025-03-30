using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class ProviderGetAllManifestCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Manifest>>>
    {
		public ProviderGetAllManifestCommand()
		{
		}
		public ManifestStatus? Status { get; set; }
		public long ProviderId { get; set; }
	}
}

