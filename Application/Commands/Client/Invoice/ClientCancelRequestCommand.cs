using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientCancelRequestCommand : IRequest<CommandResponse<bool>>
    {
        public ClientCancelRequestCommand()
        {
        }
        public string RequestNumber { get; set; }
        public long CompanyId { get; set; }
    }
}

