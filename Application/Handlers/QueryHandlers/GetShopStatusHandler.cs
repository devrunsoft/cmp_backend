using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR; 
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetShopStatusHandler : IRequestHandler<GetShopStatusQuery, ShopStatusResponse>
    {
        private readonly IShopRepository _shopRepository;

        public GetShopStatusHandler(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task<ShopStatusResponse> Handle(GetShopStatusQuery request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByIdAsync(request.ShopId);
            return new ShopStatusResponse() {IsActive = shop.IsActive };
        }
    }
}
