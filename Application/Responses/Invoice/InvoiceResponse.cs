using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class InvoiceResponse : BaseResponse<long>
    {
        //public int Percent { get; set; }
        public int Price { get; set; }
       // public string Description { get; set; } 
        public DiscountResponse Discount { get; set; }
        public DeliveryResponse Delivery { get; set; }
    }
}
