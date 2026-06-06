using System;
namespace CMPEmail
{
	public class MailSettings
    {
        public string SendGridApiKey { get; set; } = string.Empty;
        public string SendGridApiUrl { get; set; } = "https://api.sendgrid.com/v3/mail/send";
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
