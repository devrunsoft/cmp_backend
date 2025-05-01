using System;
using System.Collections.Generic;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{

    public class UpdateInvoiceCommand : IRequest<CommandResponse<Invoice>>
    {
        public long CompanyId { get; set; }
        //public string InvoiceCrmId { get; set; }
        public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; }
        public double Amount { get; set; }
    }
}

