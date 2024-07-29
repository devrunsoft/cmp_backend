using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetShopUserStatusHandler : IRequestHandler<GetShopUserStatusQuery, ShopUserStatusResponse>
    {
        private readonly IShopUserRepository _shopUserRepository;

        public GetShopUserStatusHandler(IShopUserRepository shopUserRepository)
        {
            _shopUserRepository = shopUserRepository;
        }

        public async Task<ShopUserStatusResponse> Handle(GetShopUserStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _shopUserRepository.GetShopUserByPersonId(request.PersonId, true);

            return new ShopUserStatusResponse() { Registered = user != null, ShopId = user?.ShopId, ShopUserId = user?.Id, IsActive = user?/*.Person?*/.IsActive == true };
        }

        //public async Task<List<NotificationResponse>> Handle(GetAllShopNotificationQuery request, CancellationToken cancellationToken)
        //{
        //    var ShopUsers_PersonIds = (await _shopUserRepository.GetAsync(p => p.ShopId == request.ShopId)).Select(p => p.PersonId);

        //    var Result = await _inboxRepo.GetPagedAsync(request, p => ShopUsers_PersonIds.Contains(p.ReciverPersonId));
        //    return Result.Select(p => NotificationMapper.Mapper.Map<NotificationResponse>(p)).ToList();
        //}
    }
}
