using Barbara.Application.Responses.Base; 
using MediatR;

namespace Barbara.Application.Commands
{
    public class CheckDeferredOrderCommand : IRequest<CommandResponse>
    {
        public long InboxId { get; set; }
        public long ChatId { get; set; }       
    }
}
