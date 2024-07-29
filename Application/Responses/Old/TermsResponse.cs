using Bazaro.Application.Responses.Base;

namespace Bazaro.Core.Entities
{
    public class TermsResponse : BaseResponse<int>
    {
        public string Intro { get; set; }
        public string Box { get; set; }
        public string body { get; set; }
        public int Conditions { get; set; }
    }
}
