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
    public class GetAllAssistantHandler : IRequestHandler<GetAllAssistantQuery, List<AssistantResponse>>
    {
        private readonly IShopUserRepository _shopUserRepository;

        public GetAllAssistantHandler(IShopUserRepository shopUserRepository)
        {
            _shopUserRepository = shopUserRepository;
        }

        public async Task<List<AssistantResponse>> Handle(GetAllAssistantQuery request, CancellationToken cancellationToken)
        {
            var shopUsers = await _shopUserRepository.GetShopAssistant((int)request.ShopId);
            return shopUsers.Select(p => AssistantMapper.Mapper.Map<AssistantResponse>(p)).ToList();
        }
    }
}