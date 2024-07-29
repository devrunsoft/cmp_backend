using Bazaro.Application.Responses.Base;

namespace Bazaro.Core.Entities
{
    public class SettingResponse : BaseResponse<int>
    {
        public int? OrderMinPriceId { get; set; }
        public string OrderMinPriceTitle { get; set; }
        public int? DeliveryId { get; set; }
        public string DeliveryTitle { get; set; }
        public string Name { get; set; }
        public string Title { get; set; } 
        public string Logo { get; set; }
        public string Number { get; set; }
        public string Revenue { get; set; }
        public bool IsEnable { get; set; }
        public bool ReadOnly { get; set; }
    }
}
