using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class EnableDiscountCommand : IRequest<CommandResponse>
    {
        public int Id { get; set; }
        public bool Enable { get; set; } 
        public int? ShopId { get; set; }
    }
}
