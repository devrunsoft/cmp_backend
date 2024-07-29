using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class SearchShopResponse : BaseResponse<int>
    {
        public int OrderMinPrice { get; set; }
        public int DeliveryPrice { get; set; }
        public int MaxDiscountPercent { get; set; }
        public string Logo { get; set; }
        public string Distance { get; set; } 
    }
}
