using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class GetProviderByEmailCommand : IRequest<CommandResponse<Provider>>
    {
        public string Email { get; set; }
    }
}

