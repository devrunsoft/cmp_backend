using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminSentRequestCommand : IRequest<CommandResponse<RequestEntity>>
    {
        public long RequestId { get; set; }
        public long CompanyId { get; set; }
        //public InvoiceStatus Status { get; set; }
    }
}

