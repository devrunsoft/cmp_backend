using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{

    public class AddServiceShoppingCardHandler : IRequestHandler<AddServiceShoppingCardCommand, CommandResponse<ShoppingCard>>
    {
        private readonly IShoppingCardRepository _shoppingCardRepository;

        public AddServiceShoppingCardHandler(IShoppingCardRepository shoppingCardRepository)
        {
            _shoppingCardRepository = shoppingCardRepository;
        }

        public async Task<CommandResponse<ShoppingCard>> Handle(AddServiceShoppingCardCommand request, CancellationToken cancellationToken)
        {
            var entity = new ShoppingCard()
            {
                CompanyId = request.CompanyId,
                ServicePriceCrmId = request.ServicePriceId,
                ServiceCrmId = request.ServiceId,
                StartDate = request.StartDate,
                OperationalAddressId = request.OperationalAddressId,
                FrequencyType = request.FrequencyType,
                Name = request.Name,
                AddressName = request.AddressName,
                PriceName = request.PriceName

            };
            var result = await _shoppingCardRepository.AddAsync(entity);

            return new Success<ShoppingCard>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

