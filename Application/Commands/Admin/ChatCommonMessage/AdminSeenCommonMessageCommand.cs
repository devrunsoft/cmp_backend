using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminSeenCommonMessageCommand : IRequest<CommandResponse<ChatCommonMessage>>
    {
        public long AdminId { get; set; }
        public long ChatCommonMessageId { get; set; }
    }
}