using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class GetAllInvoiceRequestCommand : IRequest<CommandResponse<List<InvoiceResponse>>>
    {
        public long CompanyId { get; set; }
    }
}

