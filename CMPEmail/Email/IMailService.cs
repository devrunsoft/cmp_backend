using System;
namespace CMPEmail.Email
{
    public interface IEmailSender
    {
        void SendEmail(MailModel model);
    }
}

