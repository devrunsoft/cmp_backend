using Bazaro.Application.Responses.Base;
using Bazaro.Core.Models;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class CreateOrOpenInboxCommand : InputInboxModel , IRequest<CommandResponse>
    { 
        public long CustomerId { get; set; }
        public int InboxStatusId { get; set; }
    }
}
