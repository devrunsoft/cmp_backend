using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllSupportCategoryHandler : IRequestHandler<GetAllSupportCategoryQuery, List<SupportCategoryResponse>>
    {
        private readonly ISupportCategoryRepository _supportCategoryRepository;

        public GetAllSupportCategoryHandler(ISupportCategoryRepository supportCategoryRepository)
        {
            _supportCategoryRepository = supportCategoryRepository;
        }

        public async Task<List<SupportCategoryResponse>> Handle(GetAllSupportCategoryQuery request, CancellationToken cancellationToken)
        {
            var parentIds = await _supportCategoryRepository.GetAllParentIdsAsync();
            var Result = await _supportCategoryRepository.GetAllSupportCategoryByParentIdAsync(request, request.IsShop ?? false, request.ParentId);

            return Result.Select(p => new SupportCategoryResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Title = p.Title,
                ParentId = p.ParentId,
                hasChild = parentIds.Any(x => x == p.Id)
            }).ToList();
        }
    }
}
