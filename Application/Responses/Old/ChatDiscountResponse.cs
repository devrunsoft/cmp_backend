using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class ChatDiscountResponse : BaseResponse<int>
    {
        public int Percent { get; set; }
        public int Price { get; set; }
        public int DeliveryCost { get; set; }
        public float TotalCost { get; set; }
        public bool DiscountStatus { get; set; }
    }
}
