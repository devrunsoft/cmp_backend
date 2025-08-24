using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientCancelRequestCommand : IRequest<CommandResponse<Invoice>>
    {
        public ClientCancelRequestCommand()
        {
        }
        public string RequestNumber { get; set; }
        public long CompanyId { get; set; }
    }
}

