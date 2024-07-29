using Bazaro.Application.ViewModels.Base;

namespace Bazaro.Application.ViewModels
{
    public class InboxViewModel : BaseViewModel<int>
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string UpdatedAt { get; set; }
        public int LastBody { get; set; }
        public bool Unread { get; set; }
        //public string Status { get; set; }

        public ChatStatusModel Status { get; set; }
    }
}
