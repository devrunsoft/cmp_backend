using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetCustomerAddressHandler : IRequestHandler<GetCustomerAddressQuery, CustomerAddressResponse>
    {
        private readonly IAddressRepository _AddressRepo;
        private readonly IShopInfoRepository _shopInfoRepository;

        public GetCustomerAddressHandler(IAddressRepository AddressRepository, IShopInfoRepository shopInfoRepository)
        {
            _AddressRepo = AddressRepository;
            _shopInfoRepository = shopInfoRepository;
        }

        public async Task<CustomerAddressResponse> Handle(GetCustomerAddressQuery request, CancellationToken cancellationToken)
        {
            var ShopAddress = await _shopInfoRepository.GetByIdAsync((int)request.ShopId);
            var customerAddress = await _AddressRepo.GetAddressByIdAsync((int)request.Id);

            return new CustomerAddressResponse()
            {
                Shop_Lat = ShopAddress.Lat,
                Shop_Long = ShopAddress.Long,
                Customer_Lat = customerAddress.Lat,
                Customer_Long = customerAddress.Long,
                Address = $"{customerAddress.Street} , {customerAddress.BuildingNumber} , {customerAddress.Unit}",
            };
        }
    }
}