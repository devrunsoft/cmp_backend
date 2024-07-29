using Bazaro.Application.Responses.Base; 

namespace Bazaro.Application.Responses
{
    public class OrderMinPriceResponse : BaseResponse<int>
    {
        public string Title { get; set; }
        public int Price { get; set; }
    }
}
