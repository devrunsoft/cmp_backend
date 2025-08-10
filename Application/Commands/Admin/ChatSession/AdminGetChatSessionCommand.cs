using System;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetChatSessionCommand :IRequest<CommandResponse<ChatSession>>
    {
        public AdminGetChatSessionCommand()
        {
        }
        public long OperationalAddressId { get; set; }
    }
}

