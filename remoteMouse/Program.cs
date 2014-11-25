using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;
using System.Drawing;
using System.Diagnostics;

namespace remoteMouse
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

    static class Program
    {
        //setting
        private const int MaxNum = 5;
        private const string url = "http://remotemouse.azurewebsites.net/"; 

        public static CorsorForm[] corsors { get; set; }
        public static LabelForm[] lfs { get; set; }
        public static Dictionary<string, int> corsorDict { get; set; }
        public static Dictionary<string, int> lfDict { get; set; }
        public static int corsorItr { get; set; }
        public static int lfItr { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            corsors = new CorsorForm[MaxNum];
            lfs = new LabelForm[MaxNum];
            corsorDict = new Dictionary<string, int>();
            lfDict = new Dictionary<string, int>();
            corsorItr = 0;
            lfItr = 0;

            for (int i=0; i < MaxNum; i++)
            {
                corsors[i] = new CorsorForm();
                corsors[i].Show();
                lfs[i] = new LabelForm();
                lfs[i].Show();
            }

            var connection = new HubConnection(url);
            var hubProxy = connection.CreateHubProxy("remoteMouse");
            
            hubProxy.On<Message>("Message", message =>
            {
                if (!corsorDict.ContainsKey(message.connectionId))
                {
                    if (corsorItr >= MaxNum) corsorItr = 0;
                    corsorDict[message.connectionId] = corsorItr;
                    corsorItr++;
                }
                if (!lfDict.ContainsKey(message.connectionId))
                {
                    if (lfItr >= MaxNum) lfItr = 0;
                    lfDict[message.connectionId] = lfItr;
                    lfItr++;
                }
                lfs[corsorDict[message.connectionId]].Text_Change(
                    message.text, message.color,
                    corsors[corsorDict[message.connectionId]].Left,
                    corsors[corsorDict[message.connectionId]].Top, false);
            });

            hubProxy.On<Moving>("Drag", moving =>
            {
                moving.x /= 10;
                moving.y /= 10;
                if (moving.x <= -5) moving.x = -5;
                if (moving.x >= 5) moving.x = 5;
                if (moving.y <= -5) moving.y = -5;
                if (moving.y >= 5) moving.y = 5;

                if (!corsorDict.ContainsKey(moving.connectionId))
                {
                    if (corsorItr >= MaxNum) corsorItr = 0;
                    corsorDict[moving.connectionId] = corsorItr;
                    corsorItr++;
                }
                corsors[corsorDict[moving.connectionId]].Corsor_Rewrite(moving.x, moving.y, moving.color);
            });

            try
            {
                connection.Start().Wait();
                hubProxy.Invoke("SetHost").Wait();
            }
            catch (System.Exception e)
            {
                Environment.FailFast("Cannot connect server.");
            }

            Application.Run(new NotifyForm());

        }
    }
}
