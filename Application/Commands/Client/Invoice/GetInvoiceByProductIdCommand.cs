using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class GetInvoiceByProductIdCommand : IRequest<CommandResponse<Invoice>>
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    }
}

