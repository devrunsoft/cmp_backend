using Bazaro.Application.ViewModels.Base;

namespace Bazaro.Application.ViewModel
{
    public class ChatViewModel : BaseViewModel<long>
    {
        public string Body { get; set; }
        public string Street { get; set; }
        public string HomeNumber { get; set; }
        public byte[] File { get; set; }
        public float Discount { get; set; }
    }
}
