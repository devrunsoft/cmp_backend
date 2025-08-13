using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminSeenMessageCommand : IRequest<CommandResponse<ChatMessage>>
    {
        public long AdminId { get; set; }
        public long ChatMessageId { get; set; }
    }
}