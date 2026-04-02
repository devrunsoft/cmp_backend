using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminResetProviderPasswordCommand : IRequest<CommandResponse<Provider>>
    {
		public long ProviderId { get; set; }
        public string Password { get; set; }
    }
}

