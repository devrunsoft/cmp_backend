using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class PollsResponse : BaseResponse<int>
    {
        public string Title { get; set; }
        public string Logo { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int PollStatus { get; set; }
    }
}
