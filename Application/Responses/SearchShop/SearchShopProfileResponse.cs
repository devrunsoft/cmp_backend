using Bazaro.Application.Responses.Base;
using System.Collections.Generic;

namespace Bazaro.Application.Responses
{
    public class SearchShopProfileResponse : BaseResponse<int>
    {
        public string Logo { get; set; }
        public string Qrcode { get; set; }
        public string Distance { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public OrderMinPriceResponse OrderMinPrice { get; set; }
        public DeliveryResponse Delivery { get; set; }
        public SearchShopProfileStatusResponse Status { get; set; }
        public IEnumerable<DiscountResponse> Discounts { get; set; }
    }
}
