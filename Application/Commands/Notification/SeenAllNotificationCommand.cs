using Bazaro.Application.Responses.Base;
using MediatR; 

namespace Bazaro.Application.Commands
{
    public class SeenAllNotificationCommand : IRequest<CommandResponse>
    {
        public long ReciverPersonId { get; set; }
    }
}
