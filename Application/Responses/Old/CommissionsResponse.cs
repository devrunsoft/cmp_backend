using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class CommissionsResponse : BaseResponse<int>
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
