using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.Biling
{
	public class ProviderGetBilingCommand : IRequest<CommandResponse<BillingInformationProvider>>
    {
        public long ProviderId { get; set; }
    }
}

