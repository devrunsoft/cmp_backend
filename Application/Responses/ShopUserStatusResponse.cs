
namespace Bazaro.Application.Responses
{
    public class ShopUserStatusResponse  
    {
        public int? ShopId { get; set; }
        public long? ShopUserId { get; set; }
        public bool Registered { get; set; }
        public bool IsActive { get; set; }
    }
}
