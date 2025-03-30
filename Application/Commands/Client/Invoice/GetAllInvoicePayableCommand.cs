using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class GetAllInvoicePayableCommand : IRequest<CommandResponse<List<Invoice>>>
    {
        public long CompanyId { get; set; }
    }
}

