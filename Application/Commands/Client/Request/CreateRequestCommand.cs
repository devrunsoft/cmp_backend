using System;
using CMPNatural.Application.Model.ServiceAppointment;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities; 

namespace CMPNatural.Application
{
    public class CreateRequestCommand : IRequest<CommandResponse<RequestEntity>>
    {
        public long CompanyId { get; set; }
        //public string InvoiceCrmId { get; set; }
        public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; }
        public List<ServiceAppointmentInput> Services { get; set; }
        public double Amount { get; set; }
        public List<long> LocationCompanyIds { get; set; }
        public long OperationalAddressId { get; set; }
        public long BillingInformationId { get; set; }
        public string Address { get; set; } = "";
    }
}

