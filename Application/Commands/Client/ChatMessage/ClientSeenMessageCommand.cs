using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientSeenMessageCommand : IRequest<CommandResponse<ChatMessage>>
    {
        public long ClientId { get; set; }
        public long ChatMessageId { get; set; }
    }
}