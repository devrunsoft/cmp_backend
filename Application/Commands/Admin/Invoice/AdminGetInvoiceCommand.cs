using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGetInvoiceCommand: IRequest<CommandResponse<InvoiceResponse>>
	{
		public AdminGetInvoiceCommand()
		{
		}
		public long InvoiceId { get; set; }
	}
}

