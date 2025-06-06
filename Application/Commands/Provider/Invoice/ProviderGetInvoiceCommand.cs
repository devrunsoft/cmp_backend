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
		public long InvoiceId { get; set; }
		public long ProviderId { get; set; }
	}
}

