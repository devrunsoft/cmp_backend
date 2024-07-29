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
    public class ReloadShopInfoCacheHandler : IRequestHandler<ReloadShopInfoCacheCommand, CommandResponse>
    {
        private readonly IShopInfoRepository _shopInfoRepository;

        public ReloadShopInfoCacheHandler(IShopInfoRepository shopInfoRepository)
        {
            _shopInfoRepository = shopInfoRepository;
        }

        public async Task<CommandResponse> Handle(ReloadShopInfoCacheCommand request, CancellationToken cancellationToken)
        {
            var shopInfo = await _shopInfoRepository.AddOrUpdateNewSearchShopByIdAsync(request.ShopId);
            return new Success() { Id = shopInfo.Id };
        }
    }
}
