using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Client.Payment
{
	public class ClientInvoicePaidCommand : IRequest<CommandResponse<bool>>
    {
		public ClientInvoicePaidCommand()
		{
		}
		public string CheckoutSessionId { get; set; }
	}
}

