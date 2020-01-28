using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PWMS.Core.Server
{
    public delegate void ClientConnectedEventHandler(object sender, ClientConnectedEventArgs e);
    public class ClientConnectedEventArgs : EventArgs
    {
        public TcpClient ConnectedClient { get; set; }
    }
}
