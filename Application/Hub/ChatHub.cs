using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CMPNatural.Api
{
    public class UserTypingPayload : ChatSession
    {
        public bool IsTyping { get; set; }
        public string Name { get; set; }
    }

    public class ConnectedUserInfo
    {
        public string ConnectionId { get; set; } = default!;
        public bool IsAdmin { get; set; }
    }


    public interface IChatClient
    {
        Task SendMessage(string type, string message);
        Task SendAsync(string method, string arg1, string arg2);
        Task SendAsync(string method, string arg1);
    }

    public class ChatHub : Hub<IChatClient>
    {
        public static ConcurrentDictionary<string, ConnectedUserInfo> ConnectedUsers = new();


        public override async Task OnConnectedAsync()
        {

            var personId = Context.User?.FindFirstValue("PersonId");
            var isAdmin = Context.User?.FindFirstValue("isAdmin") == "true";

            if (personId != null)
            {
                ConnectedUsers[personId] = new ConnectedUserInfo
                {
                    ConnectionId = Context.ConnectionId,
                    IsAdmin = isAdmin
                };
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var PersonId = Context.User?.FindFirstValue("PersonId");
            if (ConnectedUsers.TryRemove(PersonId!, out var conncetionId))
            {
                //await Clients.All.SendAsync("ReceiveMessage", "System", $"{userId} disconnected.");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessagwe(string personId, ChatMessage message , string type = "message")
        {
            if (ConnectedUsers.TryGetValue(personId, out var connectionId))
            {
                var m = JsonConvert.SerializeObject(message, new StringEnumConverter());
                await Clients.Client(connectionId.ConnectionId).SendMessage(type, m);
            }
        }
        public async Task UserTyping(UserTypingPayload payload)
        {
            await Clients.All.SendMessage("IsTyping" + payload.Id, payload.IsTyping.ToString());
        }
    }
}