using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class InboxStatusResponse : BaseResponse<int>
    {
        public string ShopTitleStatus { get; set; }
        public string ShopNameStatus { get; set; }
        public string CustomerTitleStatus { get; set; }
        public string CustomerNameStatus { get; set; }
        public string TextOnChat { get; set; }
        public string Description { get; set; }
    }
}
