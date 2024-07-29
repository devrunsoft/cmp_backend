using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class PendingResponse : BaseResponse<int>
    {
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
    }
}
