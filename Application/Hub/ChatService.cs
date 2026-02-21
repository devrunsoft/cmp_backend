using System;
using CMPNatural.Api;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using Stripe.Forwarding;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Hub
{
    [Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChatEnum
    {
        [Description("liveLocation")]
        liveLocation,

        [Description("message")]
        message,

        [Description("seen")]
        seen,

        [Description("isTyping")]
        isTyping,

        [Description("mention")]
        mention,
    }

    public interface IChatService
    {
        Task SendMessageToClient(long clientId, ChatMessage message, ChatEnum type = ChatEnum.message);
        Task SendToClient(long clientId, string message, ChatEnum type = ChatEnum.message);
        Task SendMessageToAdmin(long clientId, ChatMessage message, ChatEnum type = ChatEnum.message);
        Task SendToPerson(string personId, string message, ChatEnum type = ChatEnum.message);
        Task SendToAllAdmins(ChatMessage message, ChatEnum type = ChatEnum.message);
        Task SendObjectToAllAdmins(string message, ChatEnum type = ChatEnum.message);
        Task AdminUserTyping(string currentPersonId  , UserTypingPayload payload);
        Task ClientUserTyping(UserTypingPayload payload);
        Task DriverLocation(string currentPersonId, LocationPayload payload);
    }

    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;
        private readonly ICompanyRepository _companyRepository;
        private readonly IProviderReposiotry providerReposiotry;
        private readonly IAdminRepository _adminRepository;

        public ChatService(IHubContext<ChatHub, IChatClient> hubContext , IAdminRepository _adminRepository, ICompanyRepository _companyRepository)
        {
            _hubContext = hubContext;
            this._adminRepository = _adminRepository;
            this._companyRepository = _companyRepository;
        }

        public async Task SendToPerson(string personId, string message, ChatEnum type = ChatEnum.message)
        {

            if (ChatHub.ConnectedUsers.TryGetValue(personId, out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId.ConnectionId).SendMessage(type.GetDescription(), message);
            }
        }
        public async Task SendToClient(long clientId, string message, ChatEnum type = ChatEnum.message)
        {
            var company = (await _companyRepository.GetAsync(x => x.Id == clientId)).FirstOrDefault();

            await SendToPerson(company.PersonId.ToString(), message, type);
        }

        public async Task SendToProvider(long provider, string message, ChatEnum type = ChatEnum.liveLocation)
        {
            var company = (await providerReposiotry.GetAsync(x => x.Id == provider)).FirstOrDefault();

            await SendToPerson(company.PersonId.ToString(), message, type);
        }

        public async Task SendObjectToAllAdmins(string message, ChatEnum type = ChatEnum.message)
        {
            var adminConnections = ChatHub.ConnectedUsers
                .Where(kvp => kvp.Value.IsAdmin)
                .Select(kvp => kvp.Value.ConnectionId)
                .ToList();

            foreach (var connId in adminConnections)
            {
                await _hubContext.Clients.Client(connId).SendMessage(type.GetDescription(), message);
            }
        }

        #region chat

        private async Task SendMessageToPerson(string personId, ChatMessage message, ChatEnum type = ChatEnum.message)
        {
            await SendToPerson(personId, JsonConvert.SerializeObject(message, new StringEnumConverter()), type);
        }

        public async Task SendMessageToClient(long clientId, ChatMessage message, ChatEnum type = ChatEnum.message)
        {
            var company = (await _companyRepository.GetAsync(x => x.Id == clientId)).FirstOrDefault();

            await SendMessageToPerson(company.PersonId.ToString(), message , type);
        }

        public async Task SendMessageToAdmin(long adminId, ChatMessage message, ChatEnum type = ChatEnum.message)
        {
            var company = (await _adminRepository.GetAsync(x => x.Id == adminId)).FirstOrDefault();

            await SendMessageToPerson(company.PersonId.ToString(), message, type);
        }

        public async Task SendToAllAdmins(ChatMessage message, ChatEnum type = ChatEnum.message)
        {
            var m = JsonConvert.SerializeObject(message, new StringEnumConverter());
            await SendObjectToAllAdmins(m, type);
        }

        public async Task AdminUserTyping(string currentPersonId ,UserTypingPayload payload)
        {
            var personId = currentPersonId;
            payload.PersonId = personId;
            var m = JsonConvert.SerializeObject(payload, new StringEnumConverter());
            await SendToClient(payload.ClientId, m, ChatEnum.isTyping);
            await SendObjectToAllAdmins(m, ChatEnum.isTyping);
        }

        public async Task ClientUserTyping(UserTypingPayload payload)
        {
            var m = JsonConvert.SerializeObject(payload, new StringEnumConverter());
            await SendObjectToAllAdmins(m, ChatEnum.isTyping);
        }
        #endregion

        public async Task DriverLocation(string currentPersonId, LocationPayload payload)
        {
            var personId = currentPersonId;
            payload.PersonId = personId;
            var m = JsonConvert.SerializeObject(payload, new StringEnumConverter());
            //await SendToClient(payload.ProviderId, m, ChatEnum.liveLocation);
            await SendObjectToAllAdmins(m, ChatEnum.liveLocation);
            //await SendToProvider(payload.ProviderId, m, ChatEnum.liveLocation);
        }
    }
}

