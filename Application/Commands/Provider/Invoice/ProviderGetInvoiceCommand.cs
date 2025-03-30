using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class ProviderGetInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
	{
		public ProviderGetInvoiceCommand()
		{
		}
		public string InvoiceId { get; set; }
		public long ProviderId { get; set; }
	}
}

