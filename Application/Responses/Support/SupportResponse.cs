using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class SupportResponse : BaseResponse<int>
    {
        public string Title { get; set; }
        public int SupportCategoryId { get; set; }
    }
}
