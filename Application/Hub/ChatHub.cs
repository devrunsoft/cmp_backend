using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using CMPNatural.Application.Hub;
using CMPNatural.Core.Entities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CMPNatural.Api
{
    public class UserTypingPayload
    {
        public bool IsTyping { get; set; }
        public string Name { get; set; }
        public string PersonId { get; set; }
        public long ClientId { get; set; }
        public long OperationalAddressId { get; set; }
    }

    public class ConnectedUserInfo
    {
        public string ConnectionId { get; set; } = default!;
        public bool IsAdmin { get; set; }
    }

    //public static class ChatGroups
    //{
    //    public static string Channel(string channelId) => $"channel:{channelId}";
    //}

    public interface IChatClient
    {
        Task SendMessage(string type, string message);
        Task SendAsync(string method, string arg1, string arg2);
        Task SendAsync(string method, string arg1);
        Task ClientUserTyping(UserTypingPayload payload);
        Task AdminUserTyping(UserTypingPayload payload);
    }

    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService chatService;
        public ChatHub(IChatService chatService)
        {
            this.chatService = chatService;

        }
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

        //public async Task JoinChannel(string channelId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, ChatGroups.Channel(channelId));
        //}

        //public async Task LeaveChannel(string channelId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChatGroups.Channel(channelId));
        //}

        public async Task ClientUserTyping(UserTypingPayload payload)
        {
            await chatService.ClientUserTyping(payload);
        }

        public async Task AdminUserTyping(UserTypingPayload payload)
        {
            await chatService.AdminUserTyping(Context.User?.FindFirstValue("PersonId"), payload);
        }
    }
}