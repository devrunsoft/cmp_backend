using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class DiscountPriceResponse : BaseResponse<int>
    {
        public int Price { get; set; }
    }
}
