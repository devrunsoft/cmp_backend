using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminCancelInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
        public long InvoiceId { get; set; }
    }
}

