using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminSendMessageCommand : IRequest<CommandResponse<ChatMessage>>
    {
        public long AdminId { get; set; }
        public long ClientId { get; set; }
        public string Message { get; set; } = "";
        public long OperationalAddressId { get; set; }
        public List<AdminEntity> ChatMentionIds { get; set; } = new List<AdminEntity>();
        public IFormFile? File { get; set; }
    }
}
