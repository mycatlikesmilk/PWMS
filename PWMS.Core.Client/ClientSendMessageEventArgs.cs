using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMS.Core.Client
{
    public delegate void ClientSendMessageEventHandler(object sender, ClientSendMessageEventArgs e);
    public class ClientSendMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
