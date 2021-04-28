using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatR.Interfaces
{
    public interface IChatHub
    {
        Task BroadcastMessage(string client, string message, bool isVIP);
        Task IsTyping(string client, bool isTyping);
        Task AddedToVIPGroup(string client);
        Task RemovedFromVIPGroup(string client);
        Task ClientJoined(int count);
        Task ClientLeft(int count);
    }
}
