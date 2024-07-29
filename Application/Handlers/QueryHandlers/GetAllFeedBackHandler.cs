using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using BazaroApp.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
 
namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllFeedBackHandler : IRequestHandler<GetAllFeedBackQuery, List<FeedBackResponse>>
    {
        private readonly IFeedBackRepository _FeedBackRepository;

        public GetAllFeedBackHandler(IFeedBackRepository FeedBackRepository)
        {
            _FeedBackRepository = FeedBackRepository;
        }

        public async Task<List<FeedBackResponse>> Handle(GetAllFeedBackQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _FeedBackRepository.GetPagedAsync(request,p => p.);
            return discounts.Select(p => FeedBackMapper.Mapper.Map<FeedBackResponse>(p)).ToList();
        }
    }
}

