using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class ShopDeleteInboxCommand : IRequest<CommandResponse>
    {
        public long InboxId { get; set; }
        public long? ShopId { get; set; }
        public long? CustomerId { get; set; }
    }
}
