using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetCustomerProfileStatusCommand : IRequest<CommandResponse>
    {
        public long Id { get; set; }
        public int? ShopId { get; set; }
        public bool Blocked { get; set; }
    }
}
