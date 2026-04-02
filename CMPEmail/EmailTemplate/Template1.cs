using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CMPEmail.Email;

namespace CMPEmail.EmailTemplate
{
	public class Template1
	{
	public static string create(MailModel model)
		{
            var companyName = WebUtility.HtmlEncode(model.CompanyName ?? string.Empty);
            var recipientName = WebUtility.HtmlEncode(model.Name ?? string.Empty);
            var subject = WebUtility.HtmlEncode(model.Subject ?? string.Empty);
            var body = FormatBody(model.Body);
            var buttonText = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(model.buttonText) ? "View Details" : model.buttonText);
            var link = WebUtility.HtmlEncode(model.Link ?? string.Empty);
            StringBuilder mailBody = new StringBuilder();

            mailBody.AppendLine("<!DOCTYPE html>");
            mailBody.AppendLine("<html>");
            mailBody.AppendLine("<head>");
            mailBody.AppendLine("    <meta charset='UTF-8'>");
            mailBody.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            mailBody.AppendLine($"    <title>{subject}</title>");
            mailBody.AppendLine("    <style>");
            mailBody.AppendLine("        body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }");
            mailBody.AppendLine("        .container { max-width: 600px; margin: 20px auto; background: #ffffff; padding: 20px; border-radius: 8px;");
            mailBody.AppendLine("                     box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); }");
            mailBody.AppendLine("        .header { text-align: center; padding: 10px 0; border-bottom: 2px solid #eee; }");
            mailBody.AppendLine("        .content { padding: 20px; font-size: 16px; color: #333; }");
            mailBody.AppendLine("        .content p { margin: 0 0 12px; }");
            mailBody.AppendLine("        .footer { text-align: center; padding: 15px; font-size: 14px; color: #777; border-top: 2px solid #eee; }");
            mailBody.AppendLine("        .button { display: inline-block; padding: 10px 20px; margin-top: 15px; color: #fff !important; background: #007bff;");
            mailBody.AppendLine("                   text-decoration: none; border-radius: 5px; }");
            mailBody.AppendLine("        .button:hover { background: #0056b3; }");
            mailBody.AppendLine("    </style>");
            mailBody.AppendLine("</head>");
            mailBody.AppendLine("<body>");

            mailBody.AppendLine("    <div class='container'>");
            mailBody.AppendLine("        <div class='header'>");
            mailBody.AppendLine($"            <h2>{companyName}</h2>");
            mailBody.AppendLine("        </div>");

            mailBody.AppendLine("        <div class='content'>");
            if (!string.IsNullOrWhiteSpace(recipientName))
            {
                mailBody.AppendLine($"            <p>Dear {recipientName},</p>");
            }

            mailBody.AppendLine($"            {body}");
            //mailBody.AppendLine("            <p><strong>Order Number:</strong> #{1}</p>");
            //mailBody.AppendLine("            <p>If you have any questions, feel free to contact us.</p>");
           if(!string.IsNullOrWhiteSpace(link)) mailBody.AppendLine($"            <a href='{link}' class='button'>{buttonText}</a>");
            mailBody.AppendLine("        </div>");

            //mailBody.AppendLine("        <div class='footer'>");
            //mailBody.AppendLine("            <p>Best regards, <br> Your Company Team</p>");
            //mailBody.AppendLine("            <p><a href='https://yourwebsite.com'>Visit Our Website</a> | <a href='mailto:support@yourcompany.com'>Contact Support</a></p>");
            //mailBody.AppendLine("        </div>");
            mailBody.AppendLine("    </div>");

            mailBody.AppendLine("</body>");
            mailBody.AppendLine("</html>");

            string emailContent = mailBody.ToString();

            return emailContent;
        }

        private static string FormatBody(string? body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return "<p></p>";
            }

            if (LooksLikeHtml(body))
            {
                return body;
            }

            var encoded = WebUtility.HtmlEncode(body).Replace("\r\n", "\n").Replace("\n", "<br/>");
            return $"<p>{encoded}</p>";
        }

        private static bool LooksLikeHtml(string value)
        {
            return Regex.IsMatch(value, "<[a-zA-Z][^>]*>");
        }
    }
}
