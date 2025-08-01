using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class ClientSendMessageNoteCommand : IRequest<CommandResponse<ChatMessageNote>>
    {
        public long ClientId { get; set; }
        public MessageNoteType Type { get; set; }
        public string Content { get; set; }
    }
}

