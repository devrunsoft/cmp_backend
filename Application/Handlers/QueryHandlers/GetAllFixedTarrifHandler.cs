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
    public class GetAllFixedTarrifHandler : IRequestHandler<GetAllFixedTarrifQuery, List<FixedTariffResponse>>
    {
        private readonly IFixedTariffRepository _fixedTarrifRepository;

        public GetAllFixedTarrifHandler(IFixedTariffRepository fixedTarrifRepository)
        {
            _fixedTarrifRepository = fixedTarrifRepository;
        }

        public async Task<List<FixedTariffResponse>> Handle(GetAllFixedTarrifQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _fixedTarrifRepository.GetPagedFixedTariffAsync(request);
            return discounts.Select(p => FixedTarrifMapper.Mapper.Map<FixedTariffResponse>(p)).ToList();
        }
    }
}
