using System;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminCheckInvoiceProviderCommand : IRequest<CommandResponse<List<Provider>>>
    {
		public AdminCheckInvoiceProviderCommand()
		{
		}
		public long InvoiceId { get; set; }
	}
}

