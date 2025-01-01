using System;
namespace CmpNatural.CrmManagment.Command
{
	public class CreateInvoiceApiCommand
    {
        public string altId { get; set; } = "porKMjTM2U71w2EOlVrb";
        public string altType { get; set; } = "location";
        //public string status { get; set; } = "draft";
        public string name { get; set; }
		public string currency { get; set; }
        public BusinessDetailsCommand businessDetails { get; set; }
        public List<ProductItemCommand> items { get; set; }
        public ContactDetailsCommand contactDetails { get; set; }
        public string invoiceNumber { get; set; }
        public DateOnly issueDate { get; set; }
        public DateOnly dueDate { get; set; }
        public SendTo sentTo { get; set; }
        public bool liveMode { get; set; } = true;

        //public string action { get; set; } = "sent";

    }

    public class BusinessDetailsCommand
    {
        public string name { get; set; }
        public string phoneNo { get; set; }
        public string address { get; set; }
        public List<string> customValues { get; set; }
    }

    public class ProductItemCommand
    {
        public string name { get; set; }
        public string description { get; set; }
        public string productId { get; set; }
        public string priceId { get; set; }
        public string currency { get; set; }
        public double amount { get; set; }
        public int qty { get; set; } = 1;
    }


    public class ContactDetailsCommand
    {

        public string id { get; set; }
        public string name { get; set; }
        public string phoneNo { get; set; }
        public string email { get; set; }
        public string companyName { get; set; }
        public Address address { get; set; }

    }

    public class Address
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string countryCode { get; set; }
        public string postalCode { get; set; }
    }

    public class SendTo
    {
        public List<string> email { get; set; }
    }

}
