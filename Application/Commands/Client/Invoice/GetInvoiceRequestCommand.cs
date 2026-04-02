using System;
using CMPNatural.Core.Base;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Client.Invoice
{
    public class GetInvoiceRequestCommand : PagedQueryRequest, IRequest<CommandResponse<RequestResponse>>
    {
        public long CompanyId { get; set; }
        public long OperationalAddressId { get; set; }
        public long Id { get; set; }
    }
}

