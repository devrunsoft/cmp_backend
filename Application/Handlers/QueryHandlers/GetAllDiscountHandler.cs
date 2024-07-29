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
    public class GetAllDiscountHandler : IRequestHandler<GetAllDiscountQuery, List<DiscountResponse>>
    {
        private readonly IDiscountRepository _DiscountRepository;

        public GetAllDiscountHandler(IDiscountRepository DiscountRepository)
        {
            _DiscountRepository = DiscountRepository;
        }

        public async Task<List<DiscountResponse>> Handle(GetAllDiscountQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _DiscountRepository.GetDiscountByShopIdAsync((int)request.ShopId);
            return discounts.Select(p => DiscountMapper.Mapper.Map<DiscountResponse>(p)).ToList();
        }
    }
}

