using System;
namespace CMPEmail.Email
{
	public class MailModel
	{
		public string toEmail { get; set; }
		public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public string Name { get; set; } = "";
        public string Link { get; set; } = "";
        public string CompanyName { get; set; } = "";
    }
}

