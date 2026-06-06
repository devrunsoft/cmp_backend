using System;
using CMPEmail.Email;
using CMPEmail.EmailTemplate;
using Newtonsoft.Json;
using System.Text;
using CMPNatural.Core.Models;

namespace CMPEmail
{
	public class SendGride
	{
        private readonly MailSettings _mailSettings;
        public SendGride(MailSettings _mailSettings)
        {
            this._mailSettings = _mailSettings;
        }
        public void SendEmail(MailModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_mailSettings.SendGridApiKey))
                {
                    throw new InvalidOperationException("MailSettings:SendGridApiKey is not configured.");
                }

                if (string.IsNullOrWhiteSpace(_mailSettings.EmailId))
                {
                    throw new InvalidOperationException("MailSettings:EmailId is not configured.");
                }

                var payload = new
                {
                    from = new
                    {
                        email = _mailSettings.EmailId,
                        name = string.IsNullOrWhiteSpace(model.FromName) ? _mailSettings.Name : model.FromName
                    },
                    personalizations = new[]
                    {
                        new
                        {
                            to = new[]
                            {
                                new { email = model.toEmail, name = model.Name }
                            },
                            subject = model.Subject
                        }
                    },
                    content = new[]
                    {
                        new
                        {
                            type = "text/html",
                            value = Template1.create(model)
                        }
                    }
                };

                using var client = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, _mailSettings.SendGridApiUrl);
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _mailSettings.SendGridApiKey);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                using var response = client.SendAsync(request).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new InvalidOperationException(
                        $"SendGrid request failed with status {(int)response.StatusCode}: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

