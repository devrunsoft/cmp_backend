using System;
namespace CmpNatural.CrmManagment.Command
{
    public class SendInvoiceCommand
    {
        public string altId { get; set; } = "porKMjTM2U71w2EOlVrb";
        public string altType { get; set; } = "location";
        public string action { get; set; } = "email";
        public bool liveMode { get; set; } = true;
        public string userId { get; set; } = "Ud6v3oprv0NOkURRQrmn";
        public AutoPayment autoPayment { get; set; } = new AutoPayment() { enable = true , type = "customer_card" };
        public SentFrom sentFrom { get; set; } = new SentFrom() { fromName = "App-CMP office", fromEmail = "" };
        public SentTo sentTo { get; set; }
    }

    public class AutoPayment
    {
        public bool enable { get; set; }
        public string type { get; set; }
    }

    public class SentFrom
    {
        public string fromName { get; set; }
        public string fromEmail { get; set; }
    }

    public class SentTo
    {
        public List<string> email { get; set; }
        public List<string> emailCc { get; set; }
        public List<string> emailBcc { get; set; }
    }

}

