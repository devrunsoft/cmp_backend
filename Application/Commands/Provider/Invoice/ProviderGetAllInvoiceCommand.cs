using System;
using System.Collections.Generic;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Base;
using CMPNatural.Core.Base;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class ProviderGetAllInvoiceCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        public InvoiceStatus? Status { get; set; }
        public long ProviderId { get; set; }
    }
}

