using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class GetInvoiceByInvoiceNumberCommand : IRequest<CommandResponse<Invoice>>
    {
        public string invoiceNumber { get; set; }
        public long CompanyId { get; set; }
    }
}

