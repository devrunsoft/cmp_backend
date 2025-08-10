using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class AdminSendMessageManualNoteCommand : IRequest<CommandResponse<ChatMessageManualNote>>
    {
        public long AdminId { get; set; }
        public long ClientId { get; set; }
        public string Message { get; set; }
        public long OperationalAddressId { get; set; }
    }
}

