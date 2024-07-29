using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class DiscountResponse : BaseResponse<long>
    {
        public bool IsEnable { get; set; }
        public int Percent { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}
