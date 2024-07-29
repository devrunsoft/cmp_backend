using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class RegisterShopsResponse : BaseResponse<int>
    {
        public string ShopName { get; set; }
        public string AgentName { get; set; }
        public int IdentifierMobile { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }
}
