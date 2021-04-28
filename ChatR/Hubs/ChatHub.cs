using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatR.Hubs
{
    public class ChatHub : Hub<Interfaces.IChatHub>
    {
        public override Task OnConnectedAsync()
        {
            Helpers.ClientHandler.ConnectedClients++;
            Clients.All.UpdateActiveClients(Helpers.ClientHandler.ConnectedClients);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Helpers.ClientHandler.ConnectedClients--;
            Clients.All.UpdateActiveClients(Helpers.ClientHandler.ConnectedClients);
            return base.OnDisconnectedAsync(exception);
        }

        public async void SendMessage(string message, string client, bool isInVIPGroup)
        {
            if (isInVIPGroup)
            {
                await Clients.OthersInGroup(Helpers.ClientHandler.VIP_GROUP).BroadcastMessage(client, message, isInVIPGroup);
            }
            else
            {
                await Clients.Others.BroadcastMessage(client, message, isInVIPGroup);
            }
        }
        public async void AddToVIPGroup(string client)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Helpers.ClientHandler.VIP_GROUP);
            await Clients.OthersInGroup(Helpers.ClientHandler.VIP_GROUP).AddedToVIPGroup(client);
        }
        public async void RemoveFromVIPGroup(string client)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Helpers.ClientHandler.VIP_GROUP);
            await Clients.OthersInGroup(Helpers.ClientHandler.VIP_GROUP).RemovedFromVIPGroup(client);
        }
        public async void ClientIsTyping(string client, bool isTyping, bool isInVIPGroup)
        {
            if (isInVIPGroup)
            {
                await Clients.OthersInGroup(Helpers.ClientHandler.VIP_GROUP).IsTyping(client, isTyping);
            }
            else
            {
                await Clients.Others.IsTyping(client, isTyping);
            }
        }
    }
}
