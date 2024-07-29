using Bazaro.Application.Responses.Base;
using MediatR; 

namespace Bazaro.Application.Commands
{
    public class SeenAllChatIdsCommand : IRequest<CommandResponse>
    {
        public long[] Ids { get; set; }
    }
}
