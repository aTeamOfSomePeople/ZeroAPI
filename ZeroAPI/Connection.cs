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

        public static void Connect(User user, Action<Message> action)
        {
            if (hub == null)
            {
                hub = new HubConnection("http://localhost:64038");
                hubProxy = hub.CreateHubProxy("ZeroMessenger");
                hub.Start().Wait();
                hubProxy.On<Message>("newMessage", action);
                hubProxy.Invoke<string>("connect", user.GetChats());
            }
        }
    }
}
