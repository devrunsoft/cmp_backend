using Bazaro.Application.Responses;
using MediatR; 

namespace Bazaro.Application.Queries
{
    public class GetShopUserStatusQuery : IRequest<ShopUserStatusResponse>
    {
        public long PersonId { get; set; }
    }
}
