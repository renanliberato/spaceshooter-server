using Microsoft.AspNetCore.SignalR;
using SpaceshooterServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceshooterServer.Hubs
{
    public class ChatHub : Hub
    {
        private ChatList chatList;

        public ChatHub(ChatList chatList)
        {
            this.chatList = chatList;
        }

        public override async Task OnConnectedAsync()
        {
            this.chatList.ConnectionIds.Add(Context.ConnectionId);
        }

        public async Task SendMessageAsync(Guid id, string username, string message)
        {
            this.chatList.Messages.Add(new ChatMessage
            {
                Id = id,
                Username = username,
                Message = message
            });

            GetOtherChatConnections().SendAsync("MessageSent", username, message);
        }

        public IClientProxy GetOtherChatConnections()
        {
            return Clients.Clients(chatList.ConnectionIds.Except(new string[] { Context.ConnectionId }).ToArray());
        }
    }
}
