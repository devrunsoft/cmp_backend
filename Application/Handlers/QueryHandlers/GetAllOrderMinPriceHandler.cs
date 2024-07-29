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
    public class GetAllOrderMinPriceHandler : IRequestHandler<GetAllOrderMinPriceQuery, List<OrderMinPriceResponse>>
    {
        private readonly IOrderMinPriceRepository _orderMinPriceRepository;

        public GetAllOrderMinPriceHandler(IOrderMinPriceRepository orderMinPriceRepository)
        {
            _orderMinPriceRepository = orderMinPriceRepository;
        }

        public async Task<List<OrderMinPriceResponse>> Handle(GetAllOrderMinPriceQuery request, CancellationToken cancellationToken)
        {
               var Result = await _orderMinPriceRepository.GetPagedAsync(request);
               return Result.Select(p => OrderMinCostMapper.Mapper.Map<OrderMinPriceResponse>(p)).ToList();
        }
    }
}
