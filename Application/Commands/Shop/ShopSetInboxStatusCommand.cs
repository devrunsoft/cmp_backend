using Bazaro.Application.Responses.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bazaro.Application.Commands
{
    public class ShopSetInboxStatusCommand : IRequest<CommandResponse>
    {
        public long InboxId { get; set; }
        public int StatusId { get; set; }
        public long? ShopId { get; set; }
        public long? CustomerId { get; set; }
        public bool IsShop { get; set; }
    }
}
