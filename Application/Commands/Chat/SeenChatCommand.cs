using Bazaro.Application.Responses.Base;
using MediatR; 

namespace Bazaro.Application.Commands
{
    public class SeenChatCommand : IRequest<CommandResponse>
    {
        public long Id { get; set; }
    }
}
