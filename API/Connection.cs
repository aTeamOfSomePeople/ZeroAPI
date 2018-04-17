using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace API
{
    public class Connection
    {
        internal static HubConnection hub;
        internal static IHubProxy hubProxy;

        public static void Connect(Users.User user, Action<Messages.Message> newMessage, Action<Chats.Chat> newChat, Accounts.Account account)
        {
            if (hub == null)
            {
                hub = new HubConnection("https://localhost:44364/");
                hubProxy = hub.CreateHubProxy("ZeroMessenger");
                hub.Start().Wait();
                hubProxy.On("newMessage", newMessage);
                hubProxy.On("newChat", newChat);
                hubProxy.Invoke("connect", user.id , Users.GetChats(account.accessToken));
            }

        }

        public static void Disconnect()
        {
            hub.Stop();
        }
    }
}
