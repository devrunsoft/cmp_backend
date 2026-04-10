using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ParticipantSendCommonMessageCommand : IRequest<CommandResponse<ChatCommonMessage>>
    {
        public long ParticipantId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public string Message { get; set; } = "";
        public IFormFile? File { get; set; }
    }
}
