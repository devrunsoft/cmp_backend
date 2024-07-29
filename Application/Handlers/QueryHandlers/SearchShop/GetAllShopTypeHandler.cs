using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllShopTypeHandler : IRequestHandler<GetAllShopTypeQuery, List<ShopTypeResponse>>
    {
        private readonly IShopTypeRepository _shopTypeRepository;

        public GetAllShopTypeHandler(IShopTypeRepository shopTypeRepository)
        {
            _shopTypeRepository = shopTypeRepository;
        }

        public async Task<List<ShopTypeResponse>> Handle(GetAllShopTypeQuery request, CancellationToken cancellationToken)
        {
            var shopTypes = await _shopTypeRepository.GetAllAsync();
            return shopTypes.Select(p => ShopTypeMapper.Mapper.Map<ShopTypeResponse>(p)).ToList();
        }
    }
}
