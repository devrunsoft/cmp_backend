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
    public class GetAllDiscountPriceHandler : IRequestHandler<GetAllDiscountPriceQuery, List<DiscountPriceResponse>>
    {
        private readonly IDiscountPriceRepository _discountPriceRepository;

        public GetAllDiscountPriceHandler(IDiscountPriceRepository discountPriceRepository)
        {
            _discountPriceRepository = discountPriceRepository;
        }

        public async Task<List<DiscountPriceResponse>> Handle(GetAllDiscountPriceQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _discountPriceRepository.GetAllAsync();
            return discounts.Select(p => DiscountPriceMapper.Mapper.Map<DiscountPriceResponse>(p)).ToList();
        }
    }
}


