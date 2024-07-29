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
    public class GetAllDiscountPercentHandler : IRequestHandler<GetAllDiscountPercentQuery, List<DiscountPercentResponse>>
    {
        private readonly IDiscountPercentRepository _discountPercentRepository;

        public GetAllDiscountPercentHandler(IDiscountPercentRepository discountPercentRepository)
        {
            _discountPercentRepository = discountPercentRepository;
        }

        public async Task<List<DiscountPercentResponse>> Handle(GetAllDiscountPercentQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _discountPercentRepository.GetAllAsync();
            return discounts.Select(p => DiscountPercentMapper.Mapper.Map<DiscountPercentResponse>(p)).ToList();
        }
    }
}
