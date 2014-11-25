using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using System.Drawing;

[assembly: OwinStartup(typeof(remoteMouseServer.Startup))]

namespace remoteMouseServer
{
    public class Moving
    {
        public int x { get; set; }
        public int y { get; set; }
        public Color color { get; set; }
        public string connectionId { get; set; }
    }

    public class Message
    {
        public string text { get; set; }
        public Color color { get; set; }
        public string connectionId { get; set; }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }

    [HubName("remoteMouse")]
    public class remoteMouseHub : Hub
    {
        public void Send(Message message)
        {
            Debug.WriteLine(message.text);
            message.connectionId = Context.ConnectionId;
            Clients.Group("host").Message(message);
        }

        public void Drag(Moving moving)
        {
            moving.connectionId = Context.ConnectionId;
            Clients.Group("host").Drag(moving);
        }

        public void SetHost(){
            Groups.Add(Context.ConnectionId, "host");
        }
    }
}
