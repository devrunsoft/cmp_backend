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
    public class GetAllBenchmarkHandler : IRequestHandler<GetAllBenchmarkQuery, List<BenchmarkResponse>>
    {
        private readonly IBenchmarkRepository _benchmarkRepository;

        public GetAllBenchmarkHandler(IBenchmarkRepository benchmarkRepository)
        {
            _benchmarkRepository = benchmarkRepository;
        }

        public async Task<List<BenchmarkResponse>> Handle(GetAllBenchmarkQuery request, CancellationToken cancellationToken)
        {
            var shopUsers = await _benchmarkRepository.GetAsync(p => p.FormKey == request.FormKey);
            return shopUsers.Select(p => BenchmarkMapper.Mapper.Map<BenchmarkResponse>(p)).ToList();
        }
    }
}
