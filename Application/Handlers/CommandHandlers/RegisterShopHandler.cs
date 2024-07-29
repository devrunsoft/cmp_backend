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
    public class RegisterShopHandler : IRequestHandler<RegisterShopCommand, RegisterShopResponse>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IShopInfoRepository _shopInfoRepository;
        private readonly IPersonRepository _personRepository;

        public RegisterShopHandler(IShopRepository shopRepository,
            IShopInfoRepository shopInfoRepository,
            IPersonRepository personRepository)
        {
            _shopRepository = shopRepository;
            _shopInfoRepository = shopInfoRepository;
            _personRepository = personRepository;
        }

        public async Task<RegisterShopResponse> Handle(RegisterShopCommand request, CancellationToken cancellationToken)
        {
            var shop = new Shop()
            {
                TypeId = 1,
                OwnerId = (long)request.OwnerId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                IsEnable = true
            };

            await _shopRepository.AddAsync(shop);

            var shopInfo = new ShopInfo()
            {
                Id = shop.Id,
                Name = request.ShopName,
                Title = request.ShopName,
                IntroducerMobile = request.IdentifierMobile,
                Lat = request.Latitudes,
                Long = request.Longitudes,

                PhoneOne = request.PhoneOne,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _shopInfoRepository.AddAsync(shopInfo);

            var person = await _personRepository.GetByIdAsync(shop.OwnerId);
            person.Name = request.AgentName;
            await _personRepository.UpdateAsync(person); 

            return new RegisterShopResponse() { Id = shop.Id };
        }
    }
}
