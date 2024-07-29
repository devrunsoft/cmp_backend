using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetAssistantStatusCommand : IRequest<CommandResponse>
    {
        public long ShopUserId { get; set; }
        public bool IsEnable { get; set; }
    }
}
