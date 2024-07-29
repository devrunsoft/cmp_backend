using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bazaro.Application.Commands
{
    public class CreateChatCommand : IRequest<CommandResponse>
    {
        public long InboxId { get; set; }
        public int TypeId { get; set; }
        public string Body { get; set; }
        public IFormFile File { get; set; }
        public long? ShopUserId { get; set; }
    }
}
