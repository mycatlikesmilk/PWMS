using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PWMS.Core.Server
{
    public class Server
    {
        public event ClientConnectedEventHandler OnNewConnectionCreated;
        public event EventHandler OnServerStarted;
        public event ReceiveMessageEventHandler OnMessageReceived;
        public event EventHandler OnClientDisconnected;

        private TcpListener Listener { get; set; }

        private Task GetNewConnectionTask { get; set; }

        public List<ClientElement> Clients { get; set; }

        public Exception CreateServer(IPAddress ip, int port)
        {
            try
            {
                Clients = new List<ClientElement>();
                Listener = new TcpListener(ip, port);
                Listener.Start();
                GetNewConnectionTask = Task.Factory.StartNew(GetNewConnection);
                OnServerStarted(this, new EventArgs());
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public void GetNewConnection()
        {
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                ClientElement element = new ClientElement(client, this);
                Clients.Add(element);
                OnNewConnectionCreated(this, new ClientConnectedEventArgs() { ConnectedClient = client });
            }
        }

        public void InvokeOnMessageReceivedEvent(object sender, ReceiveMessageEventArgs e) => OnMessageReceived?.Invoke(sender, e);
        public void InvokeOnClientDisconnected(object sedner, EventArgs e) => OnClientDisconnected?.Invoke(sedner, e);
    }
}
