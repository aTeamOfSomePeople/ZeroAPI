using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Connection
    {
        internal static HubConnection hub;
        internal static IHubProxy hubProxy;

        public static void Connect(User user, Action<Message> newMessage, Action<Chat> newChat)
        {
            if (hub == null)
            {
                hub = new HubConnection(Resources.ServerUrl);
                hubProxy = hub.CreateHubProxy("ZeroMessenger");
                hub.Start().Wait();
                hubProxy.On("newMessage", newMessage);
                hubProxy.On("newChat", newChat);
                hubProxy.Invoke("connect", user.Id, user.GetChats());
            }
        }
        public static void Disconnect()
        {
            hub.Stop();
        }
    }
}
