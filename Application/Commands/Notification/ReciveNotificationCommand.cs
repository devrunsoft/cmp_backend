using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class ReciveNotificationCommand : IRequest<CommandResponse>
    {
        public long Id { get; set; }
        public long ReciverPersonId { get; set; }
    }
}
