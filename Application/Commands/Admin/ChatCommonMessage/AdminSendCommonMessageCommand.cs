using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminSendCommonMessageCommand : IRequest<CommandResponse<ChatCommonMessage>>
    {
        public long AdminId { get; set; }
        public long ChatCommonSessionId { get; set; }
        public string Message { get; set; } = "";
        public IFormFile? File { get; set; }
    }
}
