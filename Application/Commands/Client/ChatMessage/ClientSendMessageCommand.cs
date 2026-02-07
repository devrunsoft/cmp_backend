using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientSendMessageCommand : IRequest<CommandResponse<ChatMessage>>
    {
        public long ClientId { get; set; }
        public string Message { get; set; } = "";
        public long OperationalAddressId { get; set; }

        /// <summary>
        /// Optional file attachment for the message.
        /// </summary>
        public IFormFile? File { get; set; }
    }
}
