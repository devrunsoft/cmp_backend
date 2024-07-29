namespace Bazaro.Core.Models
{
    public class InputInvoiceModel
    { 
        public long InboxId { get; set; } 
        public long? DiscountId { get; set; }
        public int Price { get; set; }
        public int DeliveryId { get; set; }
    }
}
