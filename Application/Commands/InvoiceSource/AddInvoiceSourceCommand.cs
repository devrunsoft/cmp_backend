using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AddInvoiceSourceCommand :  IRequest<CommandResponse<InvoiceSource>>
    {
		public AddInvoiceSourceCommand()
		{
		}

        public string InvoiceId { get; set; }
        public long CompanyId { get; set; }
    }
}

