using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllSearchShopHandler : IRequestHandler<GetAllSearchShopQuery, List<SearchShopResponse>>
    {
        private readonly IShopInfoRepository _shopInfoRepository;
        private readonly IAddressRepository _addressRepository;

        public GetAllSearchShopHandler(IShopInfoRepository shopInfoRepository, IAddressRepository addressRepository)
        {
            _shopInfoRepository = shopInfoRepository;
            _addressRepository = addressRepository;
        }

        public async Task<List<SearchShopResponse>> Handle(GetAllSearchShopQuery request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetByIdAsync(request.AddressId);
            var shopTypes = await _shopInfoRepository.GetAllSearchShopAsync(request, request.ShopTypeId, (double)address.Lat, (double)address.Long);
            return shopTypes.Select(p =>
            {
                var response = SearchShopMapper.Mapper.Map<SearchShopResponse>(p);
                response.Distance = p.Distance((double)address.Lat, (double)address.Long).ToString("N0") + " متر";
                return response;
            }).ToList();
        }
    }
}
