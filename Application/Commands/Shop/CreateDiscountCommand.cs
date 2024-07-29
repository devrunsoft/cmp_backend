using Bazaro.Application.Responses.Base;
using Bazaro.Core.Models;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class CreateDiscountCommand : InputDiscountModel , IRequest<CommandResponse>
    {
        public int ShopId { get; set; }
    }
}
