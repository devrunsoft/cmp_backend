using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class SupportCategoryResponse : BaseResponse<int>
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool hasChild { get; set; }
    }
}
