using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ParticipantSeenCommonMessageCommand : IRequest<CommandResponse<ChatCommonMessage>>
    {
        public long ParticipantId { get; set; }
        public long ChatMessageId { get; set; }
    }
}