using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class ParticipantSendCommonMessageNoteCommand : IRequest<CommandResponse<ChatCommonMessageNote>>
    {
        public long ParticipantId { get; set; }
        public Guid PersonId { get; set; }
        public MessageNoteType Type { get; set; }
        public string Content { get; set; }
        public ParticipantType ParticipantType { get; set; }
    }
}

