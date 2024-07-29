using Bazaro.Application.Responses.Base;
using Bazaro.Core.Models;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class UpdateDiscountCommand : InputDiscountModel , IRequest<CommandResponse>
    {
        public long Id { get; set; }
    }
}
