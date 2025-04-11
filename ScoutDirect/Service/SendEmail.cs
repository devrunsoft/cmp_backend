using System;
using CMPEmail.Email;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Api.Controllers.Service
{
	public static class SendEmail
	{

        public static void SendClient(this IEmailSender emailSender, long CompanyId ,string subject, string body, IServiceScopeFactory serviceScopeFactory, string? link , string buttonText = "")
		{
            Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope()) // Create a new DI scope
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _cache = cache(CacheTech.Memory);
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var to = (await _mediator.Send(new GetCompanyCommand() { CompanyId = CompanyId })).Data;
                    var appinformation = await GetInformation(_cache, _mediator);
                    MailModel model = new MailModel()
                    {
                        toEmail = to.BusinessEmail,
                        Body = body,
                        Subject = subject,
                        Name = to.PrimaryFirstName,
                        CompanyName = appinformation.CompanyTitle,
                        Link = $"https://client.app-cmp.com{link}",
                        buttonText = buttonText

                    };
                    emailSender.SendEmail(model);
                }
            });
        }
        public static void SendAdmin(this IEmailSender emailSender, string subject, string body, IServiceScopeFactory serviceScopeFactory, string? link, string buttonText = "")
        {
            Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope()) // Create a new DI scope
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _cache = cache(CacheTech.Memory);
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var appinformation = await GetInformation(_cache, _mediator);

                    MailModel model = new MailModel()
                    {
                        toEmail = appinformation.CompanyEmail,
                        Body = body,
                        Subject = subject,
                        Name = $"{appinformation.CompanyCeoFirstName}",
                        CompanyName = appinformation.CompanyTitle,
                        Link = $"https://admin.app-cmp.com{link}",
                        buttonText = buttonText
                    };
                    emailSender.SendEmail(model);
                }
            });
        }

        public static void SendToProvider(this IEmailSender emailSender, string subject, string body, string email, string buttonText = "")
        {
            Send(emailSender, subject, body, email, $"https://provider.app-cmp.com", buttonText);
        }

        public static void Send(this IEmailSender emailSender, string subject, string body, string email, string link = "", string buttonText = "")
        {
            Task.Run(async () =>
            {
                    MailModel model = new MailModel()
                    {
                        toEmail = email,
                        Body = body,
                        Subject = subject,
                        Name = $"",
                        CompanyName = "",
                        buttonText= buttonText,
                        Link = link
                    };
                    emailSender.SendEmail(model);
            });
        }

        //public static void SendTestAdmin(this IEmailSender emailSender, string subject, string body, IServiceScopeFactory serviceScopeFactory, string? link)
        //{
        //    Task.Run(async () =>
        //    {
        //        using (var scope = serviceScopeFactory.CreateScope()) // Create a new DI scope
        //        {
        //            try
        //            {
        //                var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
        //                var _cache = cache(CacheTech.Memory);
        //                var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        //                var appinformation = await GetInformation(_cache, _mediator);

        //                MailModel model = new MailModel()
        //                {
        //                    toEmail = appinformation.CompanyEmail,
        //                    Body = body,
        //                    Subject = subject,
        //                    Name = $"{appinformation.CompanyCeoFirstName}",
        //                    CompanyName = appinformation.CompanyTitle,
        //                    Link = $"https://admin.app-cmp.com{link}"
        //                };
        //                emailSender.SendEmail(model);
        //                Console.WriteLine($"Email Sent");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Exeption {ex.Message}");
        //            }
        //        }
        //    });
        //}

        public static async Task<AppInformation> GetInformation(ICacheService _cache, IMediator _mediator)
        {
            if (!_cache.TryGet("CompanyEmail", out AppInformation to))
            {
                var result = await _mediator.Send(new AdminAppInformationGetCommand());
                to = result.Data;
                _cache.Set("CompanyEmail", to);
            }
            return to;
        }
    }
}

