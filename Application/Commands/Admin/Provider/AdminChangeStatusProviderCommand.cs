using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminChangeStatusProviderCommand : IRequest<CommandResponse<Provider>>
    {
		public ProviderStatus Status { get; set; }
		public long ProviderId { get; set; }
	}
}

