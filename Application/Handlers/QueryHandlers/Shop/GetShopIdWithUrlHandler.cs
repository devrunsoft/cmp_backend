using Bazaro.Application.Commands;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class GetShopIdWithUrlHandler : IRequestHandler<GetShopIdWithUrlQuery, int?>
    {
        private readonly IShopInfoRepository _shopInfoRepository;

        public GetShopIdWithUrlHandler(IShopInfoRepository shopInfoRepository)
        {
            _shopInfoRepository = shopInfoRepository; 
        }
        public async Task<int?> Handle(GetShopIdWithUrlQuery request, CancellationToken cancellationToken)
        {
            var shopInfo = (await _shopInfoRepository.GetAsync(p => p.UrlOne == request.Url)).FirstOrDefault(); 
            return shopInfo?.Id;
        }
    }
}
