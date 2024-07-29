using Bazaro.Application.Responses.Base;
using System;

namespace Bazaro.Application.Responses
{
    public class BannerAdsResponse : BaseResponse<int>
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
