using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Service;
using CMPNatural.Application.Commands;

namespace CMPNatural.Application
{

    public class AdminUpdateProductPriceHandler : IRequestHandler<AdminUpdateProductPriceCommand, CommandResponse<ProductPrice>>
    {
        private readonly IProductPriceRepository _repository;
        public AdminUpdateProductPriceHandler(IProductPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ProductPrice>> Handle(AdminUpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            entity.Name = request.Name;
            entity.Amount = request.Amount;
            entity.MinimumAmount = request.MinimumAmount;
            entity.BillingPeriod = request.BillingPeriod;
            entity.NumberofPayments = request.NumberofPayments;
            entity.SetupFee = request.SetupFee;
            entity.Enable = request.Enable;
            entity.Order = request.Order;

            await _repository.UpdateAsync(entity);
            return new Success<ProductPrice>() { Data = entity };
        }
    }
}

