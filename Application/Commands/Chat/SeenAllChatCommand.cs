using Bazaro.Application.Responses.Base;
using MediatR; 

namespace Bazaro.Application.Commands
{
    public class SeenAllChatCommand : IRequest<CommandResponse>
    {
        public long InboxId { get; set; }
    }
}
