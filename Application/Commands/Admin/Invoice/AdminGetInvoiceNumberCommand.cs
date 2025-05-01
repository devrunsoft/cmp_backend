using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{

    public class AdminGetInvoiceNumberCommand : IRequest<CommandResponse<Invoice>>
    {
        public long invoiceNumber { get; set; }
    }
}

