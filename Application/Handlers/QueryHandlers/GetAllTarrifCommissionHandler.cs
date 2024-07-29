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
    public class GetAllTarrifCommissionHandler : IRequestHandler<GetAllTarrifCommissionQuery, List<TarrifCommissionResponse>>
    {
        private readonly ITarrifCommissionRepository _tarrifCommissionRepository;

        public GetAllTarrifCommissionHandler(ITarrifCommissionRepository tarrifCommissionRepository)
        {
            _tarrifCommissionRepository = tarrifCommissionRepository;
        }

        public async Task<List<TarrifCommissionResponse>> Handle(GetAllTarrifCommissionQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _tarrifCommissionRepository.GetAllCommissionAsync();
            return discounts.Select(p => CommissionTarrifMapper.Mapper.Map<TarrifCommissionResponse>(p)).ToList();
        }
    }
}