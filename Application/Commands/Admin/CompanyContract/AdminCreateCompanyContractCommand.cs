using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminCreateCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
        public string Content { get; set; } = "";
        public long ContractId { get; set; }
        public long CompanyId { get; set; }
        public long InvoiceId { get; set; }
        public CompanyContractStatus Status { get; set; }
    }
}

