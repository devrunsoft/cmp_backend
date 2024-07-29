using Bazaro.Application.Responses.Base;
using Bazaro.Core.Models;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class CreateInvoiceCommand : InputInvoiceModel, IRequest<CommandResponse>
    {
        public int? ShopId { get; set; }
        public long ShopUserId { get; set; }
    }
}
