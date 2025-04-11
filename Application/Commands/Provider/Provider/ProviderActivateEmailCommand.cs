using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class ProviderActivateEmailCommand : IRequest<CommandResponse<Provider>>
    {
        public Guid activationLink { get; set; }
    }
}

