using CMPNatural.Core.Enums;

public static class EmailLinkHelper
{
    public static EmailDetails GetEmailDetails(EmailLinkEnum emailLinkEnum, dynamic id)
    {
        switch (emailLinkEnum)
        {
            case EmailLinkEnum.AdminInvoices:
                return new EmailDetails
                {
                    LinkPattern = $"/invoice/{id}",
                    Subject = "Service Registration",
                    Body = $"\n\nPlease review the invoice. You can access the invoice details using the button below:\n\nThank you."
                };

            case EmailLinkEnum.ClientHasSigned:
                return new EmailDetails
                {
                    LinkPattern = $"/invoice/{id}",
                    Subject = "Sign Notification",
                    Body = $"\n\nPlease review the contract. You can access the contract details using the button below:\n\nThank you."
                };

            case EmailLinkEnum.AdminHasCreateContract:
                return new EmailDetails
                {
                    LinkPattern = $"/dashboard/contract?id={id}",
                    Subject = "Sign Notification",
                    Body = $"\n\nPlease review your contract. You can access the contract details using the button below:\n\nThank you."
                };

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Helper method to get the base URL (assuming you have a method for that)
    private static string GetBaseUrl()
    {
        return "https://yourwebsite.com";  // Replace with actual logic to fetch the base URL
    }
}


public class EmailDetails
{
    public string LinkPattern { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}