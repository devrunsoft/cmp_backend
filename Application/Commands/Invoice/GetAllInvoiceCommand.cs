using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class GetAllInvoiceCommand : IRequest<CommandResponse<List<Invoice>>>
    {
        public long CompanyId { get; set; }
    }
}

