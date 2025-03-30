using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class LoginProviderCommand : IRequest<CommandResponse<Provider>>
    {
		public string Email { get; set; }
		public string Password { get; set; }
	}
}

