using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class DeleteAddressCommand :  IRequest<CommandResponse>
    {
        public long Id { get; set; } 
    }
}
