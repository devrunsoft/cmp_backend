using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class DeleteDiscountCommand : IRequest<CommandResponse>
    {
        public int Id { get; set; }
        public int? ShopId { get; set; } 
    }
}
