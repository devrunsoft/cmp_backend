using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllShopNotificationQuery : PagedQueryRequest, IRequest<List<NotificationResponse>>
    { 
        public int? Id { get; set; }
        public int? ShopId { get; set; }
    }
}
