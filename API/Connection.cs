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

        public static async void Connect(string accessToken, Action<long> newMessage, Action<long> newChat)
        {
            if (hub == null)
            {
                hub = new HubConnection("https://localhost:44364/");
                hubProxy = hub.CreateHubProxy("ZeroMessenger");
                await hub.Start();
                hubProxy.On("newMessage", newMessage);
                hubProxy.On("newChat", newChat);
                await hubProxy.Invoke("connect", accessToken);
            }

        }

        public static void Disconnect()
        {
            hub.Stop();
        }
    }
}
