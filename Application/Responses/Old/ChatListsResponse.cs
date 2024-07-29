using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class ChatListsResponse : BaseResponse<int>
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string UpdatedAt { get; set; }
        public int LastBody { get; set; }
        public bool Unread { get; set; }
        public string Status { get; set; }
    }
}
