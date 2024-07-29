using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CreateDiscountHandler : IRequestHandler<CreateDiscountCommand, CommandResponse>
    {
        private readonly IDiscountRepository _discountRepository;

        public CreateDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<CommandResponse> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var dbModel = new Discount()
            {
                ShopId = request.ShopId,
                Price = request.Price,
                Percent = request.Percent,
                Description = request.Description,
                IsActive = true,
                IsEnable = true,
                CreatedAt = DateTime.Now
            };

            var resultModel = await _discountRepository.AddAsync(dbModel);
            return new Success() { Data = resultModel };
        }
    }

}
