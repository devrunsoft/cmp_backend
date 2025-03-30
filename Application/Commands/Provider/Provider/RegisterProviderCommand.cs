using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class RegisterProviderCommand: IRequest<CommandResponse<Provider>>
	{
		public RegisterProviderCommand()
		{
		}
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Password { get; set; }
	}
}

