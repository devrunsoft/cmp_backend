using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class FeedBackResponse : BaseResponse<long>
    {
        public long InboxId { get; set; }
        public long CreatorPersonId { get; set; }
        public long TargetPersonId { get; set; }
        public int Value { get; set; }
        public string Body { get; set; }
    }
}
