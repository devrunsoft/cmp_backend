using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Admin.Invoice
{
    public class AdminGetAllCreatedInvoiceCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        public InvoiceStatus? Status { get; set; }
    }
}

