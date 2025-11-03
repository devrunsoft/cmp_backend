using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminCreateFinalInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
		public AdminCreateFinalInvoiceCommand()
		{
		}
		public List<long> ManifestIds { get; set; }
		public long CompanyId { get; set; }
	}
}

