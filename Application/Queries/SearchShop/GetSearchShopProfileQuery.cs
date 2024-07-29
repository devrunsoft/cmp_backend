using Bazaro.Application.Responses;
using MediatR;

namespace Bazaro.Application.Queries
{
    public class GetSearchShopProfileQuery : IRequest<SearchShopProfileResponse>
    {
        public int ShopId { get; set; }
    }

}
