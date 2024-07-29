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
    public class GetAllShopNotificationHandler : IRequestHandler<GetAllShopNotificationQuery, List<NotificationResponse>>
    {
        private readonly INotificationRepository _inboxRepo;
        private readonly IShopUserRepository _shopUserRepository;

        public GetAllShopNotificationHandler(INotificationRepository inboxRepository, IShopUserRepository shopUserRepository)
        {
            _inboxRepo = inboxRepository;
            _shopUserRepository = shopUserRepository;
        }
 
        public async Task<List<NotificationResponse>> Handle(GetAllShopNotificationQuery request, CancellationToken cancellationToken)
        {
            var ShopUsers_PersonIds = (await _shopUserRepository.GetAsync(p => p.ShopId == request.ShopId)).Select(p => p.PersonId);
 
            var Result = await _inboxRepo.GetPagedAsync(request,p => ShopUsers_PersonIds.Contains(p.ReciverPersonId));
            return Result.Select(p => NotificationMapper.Mapper.Map<NotificationResponse>(p)).ToList();
        }
    }
}