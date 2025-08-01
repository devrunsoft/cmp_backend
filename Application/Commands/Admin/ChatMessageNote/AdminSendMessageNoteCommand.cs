using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class AdminSendMessageNoteCommand : IRequest<CommandResponse<ChatMessageNote>>
    {
        public long AdminId { get; set; }
        public long ClientId { get; set; }
        public MessageNoteType Type { get; set; }
        public string Content { get; set; }
    }
}

