using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Base;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class GetAllInvoiceRequestCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        public long CompanyId { get; set; }
        public long OperationalAddressId { get; set; }
    }
}

