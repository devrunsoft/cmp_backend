using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CMPEmail.Email;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.Core.Repositories.ChatCommon;
using Hangfire;
using ScoutDirect.Core.Repositories;

namespace CMPNatural.Api.Services
{
    public class HangfireUnseenMessageEmailScheduler : IUnseenMessageEmailScheduler
    {
        private static readonly TimeSpan Delay = TimeSpan.FromMinutes(5);
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireUnseenMessageEmailScheduler(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void ScheduleChatMessageEmail(long messageId)
        {
            _backgroundJobClient.Schedule<IUnseenMessageEmailJob>(
                job => job.SendChatMessageEmailIfUnseen(messageId),
                Delay);
        }

        public void ScheduleCommonMessageEmail(long messageId)
        {
            _backgroundJobClient.Schedule<IUnseenMessageEmailJob>(
                job => job.SendCommonMessageEmailIfUnseen(messageId),
                Delay);
        }
    }

    public interface IUnseenMessageEmailJob
    {
        Task SendChatMessageEmailIfUnseen(long messageId);
        Task SendCommonMessageEmailIfUnseen(long messageId);
    }

    public class UnseenMessageEmailJob : IUnseenMessageEmailJob
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatCommonMessageRepository _chatCommonMessageRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAppInformationRepository _appInformationRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IEmailSender _emailSender;
        private readonly AppSetting _appSetting;

        public UnseenMessageEmailJob(
            IChatMessageRepository chatMessageRepository,
            IChatCommonMessageRepository chatCommonMessageRepository,
            ICompanyRepository companyRepository,
            IAppInformationRepository appInformationRepository,
            IEmailSender emailSender,
            IAdminRepository adminRepository,
              AppSetting appSetting)
        {
            _chatMessageRepository = chatMessageRepository;
            _chatCommonMessageRepository = chatCommonMessageRepository;
            _companyRepository = companyRepository;
            _appInformationRepository = appInformationRepository;
            _emailSender = emailSender;
            _appSetting = appSetting;
            _adminRepository = adminRepository;
        }

        public async Task SendChatMessageEmailIfUnseen(long messageId)
        {
            var message = await _chatMessageRepository.GetByIdAsync(messageId);
            if (message == null || message.IsSeen)
                return;

            if (message.SenderType == ParticipantType.Admin)
            {
                var company = await _companyRepository.GetByIdAsync(message.ClientId);
                if (string.IsNullOrWhiteSpace(company?.BusinessEmail))
                    return;

                SendEmail(
                    company.BusinessEmail,
                    "Unread chat message",
                    BuildBody("You have an unread message from ScoutDirect.", message),
                    _appSetting.clientHost);
                return;
            }

            if (message.SenderType == ParticipantType.Client)
            {
                var appInformation = await GetAppInformation();
                if (string.IsNullOrWhiteSpace(appInformation?.CompanyEmail))
                    return;

                SendEmail(
                    appInformation.CompanyEmail,
                    "Unread client chat message",
                    BuildBody("A client sent a chat message that has not been seen.", message),
                     $"{_appSetting.adminHost}/Conversation/{message.SenderId}/{message.OperationalAddressId}");
            }
        }

        public async Task SendCommonMessageEmailIfUnseen(long messageId)
        {
            var message = await _chatCommonMessageRepository.GetByIdAsync(messageId);
            if (message == null || message.IsSeen || message.SenderType == ParticipantType.Admin)
                return;

            var appInformation = await GetAppInformation();
            if (string.IsNullOrWhiteSpace(appInformation?.CompanyEmail))
                return;

            SendEmail(
                appInformation.CompanyEmail,
                "Unread provider chat message",
                BuildBody("A provider/driver sent a chat message that has not been seen.", message),
                $"{_appSetting.adminHost}/Provider-Conversation/{message.SenderId}");
        }

        private async Task<AppInformation?> GetAppInformation()
        {
            return (await _appInformationRepository.GetAllAsync()).LastOrDefault();
        }

        private async Task<AdminEntity?> GetAdmin(long AdminId)
        {
            return (await _adminRepository.GetAsync(x=>x.Id == AdminId)).FirstOrDefault();
        }

        private void SendEmail(string toEmail, string subject, string body, string link)
        {
            _emailSender.SendEmail(new MailModel
            {
                toEmail = toEmail,
                Subject = subject,
                Body = body,
                Link = link,
                buttonText = "Open Chat"
            });
        }

        private static string BuildBody(string intro, ChatMessage message)
        {
            return BuildBody(intro, message.Content, message.Type, message.SentAt);
        }

        private static string BuildBody(string intro, ChatCommonMessage message)
        {
            return BuildBody(intro, message.Content, message.Type, message.SentAt);
        }

        private static string BuildBody(string intro, string content, MessageType type, DateTime sentAt)
        {
            var preview = string.IsNullOrWhiteSpace(content)
                ? $"[{type}]"
                : WebUtility.HtmlEncode(content.Length > 180 ? $"{content[..180]}..." : content);

            return $"<p>{WebUtility.HtmlEncode(intro)}</p><p><strong>Message:</strong> {preview}</p><p><strong>Sent at:</strong> {sentAt:g}</p>";
        }
    }
}
