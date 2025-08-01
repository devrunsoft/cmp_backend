using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CreateChatSessionCommand : IRequest<CommandResponse<ChatSession>>
    {
        public long ClientId { get; set; }
    }
}

