namespace Bazaro.Api.Models
{
    public class InputFeedBackModel
    {
        public long InboxId { get; set; }
        public int[] BenchmarkIds { get; set; }
        public string Body { get; set; }
    }
}
