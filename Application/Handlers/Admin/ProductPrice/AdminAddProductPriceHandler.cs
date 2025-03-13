using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Service;

namespace CMPNatural.Application.Handlers.CommandHandlers.Service
{

    public class AdminAddProductPriceHandler : IRequestHandler<AdminAddProductPriceCommand, CommandResponse<ProductPrice>>
    {
        private readonly IProductPriceRepository _repository;
        public AdminAddProductPriceHandler(IProductPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ProductPrice>> Handle(AdminAddProductPriceCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductPrice
            {
                Name = request.Name,
                Amount = request.Amount,
                MinimumAmount = request.MinimumAmount,
                BillingPeriod = request.BillingPeriod,
                NumberofPayments = request.NumberofPayments,
                SetupFee = request.SetupFee,
                Enable = request.Enable,
                Order = request.Order,
                ProductId = request.ProductId
            };
            var result = await _repository.AddAsync(entity);
            return new Success<ProductPrice>() { Data = result };
        }
    }
}

