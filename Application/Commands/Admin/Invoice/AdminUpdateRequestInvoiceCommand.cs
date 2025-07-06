using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminUpdateRequestInvoiceCommand : InvoiceInput, IRequest<CommandResponse<Invoice>>
    {
		public AdminUpdateRequestInvoiceCommand()
		{
		}

		public long InvoiceId { get; set; }
	}
}

