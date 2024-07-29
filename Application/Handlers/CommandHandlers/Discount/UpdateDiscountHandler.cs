using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class UpdateDiscountHandler : IRequestHandler<UpdateDiscountCommand, CommandResponse>
    {
        private readonly IDiscountRepository _discountRepository;

        public UpdateDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<CommandResponse> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = await _discountRepository.GetByIdAsync(request.Id);
            if (discount == null)
                return new ResponseNotFound();

            discount.Price = request.Price;
            discount.Percent = request.Percent;
            discount.Description = request.Description;
            discount.UpdatedAt = DateTime.Now;

            await _discountRepository.UpdateAsync(discount);

            return new Success() { };
        }
    }

}
