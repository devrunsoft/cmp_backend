using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class AdminSendCommonMessageNoteCommand : IRequest<CommandResponse<ChatCommonMessageNote>>
    {
        public long AdminId { get; set; }
        public Guid PersonId { get; set; }
        public long ChatCommonSessionId { get; set; }
        public MessageNoteType Type { get; set; }
        public string Content { get; set; }
    }
}

