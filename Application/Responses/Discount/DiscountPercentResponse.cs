using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class DiscountPercentResponse : BaseResponse<int>
    {
        public int Percent { get; set; }
    }
}
