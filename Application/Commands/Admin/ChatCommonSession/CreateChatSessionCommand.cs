using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CreateChatCommonSessionCommand : IRequest<CommandResponse<ChatCommonSession>>
    {
        public long ParticipantId { get; set; }
        public ParticipantType ParticipantType { get; set; }
    }
}

