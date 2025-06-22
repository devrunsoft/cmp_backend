using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientTerminateContractCommand : IRequest<CommandResponse<bool>>
    {
        public ClientTerminateContractCommand()
        {
        }
        public string InvoiceNumber { get; set; }
        public long CompanyId { get; set; }
    }
}

