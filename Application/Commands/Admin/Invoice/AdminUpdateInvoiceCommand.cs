using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminUpdateInvoiceCommand : InvoiceInput, IRequest<CommandResponse<Invoice>>
    {
		public AdminUpdateInvoiceCommand()
		{
		}

		public string InvoiceId { get; set; }
	}
}

