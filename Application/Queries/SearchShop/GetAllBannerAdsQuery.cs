using Bazaro.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllBannerAdsQuery : IRequest<List<BannerAdsResponse>>
    {
    }
}
