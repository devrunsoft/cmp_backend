using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminProviderUpdateInvoiceCommand : InvoiceInput, IRequest<CommandResponse<Invoice>>
    {
		public AdminProviderUpdateInvoiceCommand()
		{
		}

		public long InvoiceId { get; set; }
	}
}

