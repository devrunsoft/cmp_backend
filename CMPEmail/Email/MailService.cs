using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using CMPEmail.EmailTemplate;
using CmpNatural.CrmManagment.Webhook;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Pkcs;

namespace CMPEmail.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public void SendEmail(MailModel model)
        {
            EmailWebHook.send(model);


            //SmtpClient client = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
            //client.EnableSsl = _mailSettings.UseSSL;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password);

            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress(_mailSettings.EmailId);
            //mailMessage.To.Add(model.toEmail);
            //mailMessage.Subject = model.Subject;
            //mailMessage.IsBodyHtml = true;
            //mailMessage.Body = Template1.create(model);

            //// Send email
            //client.Send(mailMessage);
        }
    }
}

