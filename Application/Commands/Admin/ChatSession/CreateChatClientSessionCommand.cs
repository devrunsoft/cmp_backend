using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CreateChatClientSessionCommand : IRequest<CommandResponse<ChatClientSession>>
    {
        public long ClientId { get; set; }
        public List<long> OperationalAddressId { get; set; }
    }
}

