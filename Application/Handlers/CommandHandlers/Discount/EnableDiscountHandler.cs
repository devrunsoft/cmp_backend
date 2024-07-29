using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class EnableDiscountHandler : IRequestHandler<EnableDiscountCommand, CommandResponse>
    {
        private readonly IDiscountRepository _discountRepository;

        public EnableDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<CommandResponse> Handle(EnableDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = await _discountRepository.GetByIdAsync(request.Id);
            if (discount == null)
                return new ResponseNotFound();

            discount.IsEnable = request.Enable;
            await _discountRepository.UpdateAsync(discount);

            return new Success() { Data = discount };
        }
    }
}
