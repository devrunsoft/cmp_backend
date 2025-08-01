using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientSendMessageCommand : IRequest<CommandResponse<ChatMessage>>
    {
        public long ClientId { get; set; }
        public string Message { get; set; }
    }
}

