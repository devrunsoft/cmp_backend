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

namespace CMPNatural.Application.Hub
{
    public interface IChatService
    {
        Task SendMessageToClient(long clientId, ChatMessage message, string type = "message");
        Task SendMessageToAdmin(long clientId, ChatMessage message, string type = "message");
        Task SendToAllAdmins(ChatMessage message, string type = "message");
    }

    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAdminRepository _adminRepository;

        public ChatService(IHubContext<ChatHub, IChatClient> hubContext , IAdminRepository _adminRepository, ICompanyRepository _companyRepository)
        {
            _hubContext = hubContext;
            this._adminRepository = _adminRepository;
            this._companyRepository = _companyRepository;
        }

        private async Task SendMessageToPerson(string personId, ChatMessage message, string type = "message")
        {
            if (ChatHub.ConnectedUsers.TryGetValue(personId, out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId.ConnectionId).SendMessage(type, JsonConvert.SerializeObject(message , new StringEnumConverter()));
            }
        }

        public async Task SendMessageToClient(long clientId, ChatMessage message, string type = "message")
        {
            var company = (await _companyRepository.GetAsync(x => x.Id == clientId)).FirstOrDefault();

            await SendMessageToPerson(company.PersonId.ToString(), message , type);
        }

        public async Task SendMessageToAdmin(long adminId, ChatMessage message, string type = "message")
        {
            var company = (await _adminRepository.GetAsync(x => x.Id == adminId)).FirstOrDefault();

            await SendMessageToPerson(company.PersonId.ToString(), message, type);
        }

        public async Task SendToAllAdmins(ChatMessage message, string type = "message")
        {
            var adminConnections = ChatHub.ConnectedUsers
                .Where(kvp => kvp.Value.IsAdmin)
                .Select(kvp => kvp.Value.ConnectionId)
                .ToList();
            var m = JsonConvert.SerializeObject(message, new StringEnumConverter());
            foreach (var connId in adminConnections)
            {
                await _hubContext.Clients.Client(connId).SendMessage(type, m);
            }
        }
    }

}

