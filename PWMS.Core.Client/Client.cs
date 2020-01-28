using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Client
{
    public class Client
    {
        public event EventHandler OnConnected;
        public event ClientSendMessageEventHandler OnMessageSendToServer;
        public event ClientSendMessageEventHandler OnMessageReceivedFromServer;
        public event EventHandler OnServerShutdown;
        public event ClientSendMessageEventHandler OnInformationLog;

        private string ID;
        private bool IsConnected = false;

        private TcpClient client;

        private Resource RAgent;
        private Technology TAgent;
        private List<SheduleElement> shedule;

        public Exception Connect(IPAddress address, int port)
        {
            try
            {
                client = new TcpClient();
                client.Connect(address, port);
                SendMessageToServer("getsystemid");
                OnConnected(this, new EventArgs());
                StartClient();
                IsConnected = true;
                shedule = new List<SheduleElement>();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public void StartClient()
        {
            Task ReceiveMessageFromServerTask = Task.Factory.StartNew(ReceiveNewMessageFromServer);
        }

        public void SendMessageToServer(string message)
        {
            if (!IsConnected) return;

            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            OnMessageSendToServer?.Invoke(this, new ClientSendMessageEventArgs() { Message = message, Time = DateTime.Now });
        }

        public string ReceiveNewMessageFromServer()
        {
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] data = new byte[65536];
                    int bytes = stream.Read(data, 0, data.Length);
                    string res = Encoding.UTF8.GetString(data, 0, bytes);
                    HandleMessage(res);
                    OnMessageReceivedFromServer?.Invoke(this, new ClientSendMessageEventArgs() { Time = DateTime.Now, Message = res });
                }
            }
            catch
            {
                OnServerShutdown?.Invoke(this, new EventArgs());
                IsConnected = false;
                return null;
            }
        }

        public object HandleLocalMessage(string message)
        {
            if (!IsConnected) return null;
            string command = message.Split(' ')[0];
            List<string> parameters = new List<string>();
            for (int i = 1; i < message.Split(' ').Length; i++)
            {
                parameters.Add(message.Split(' ')[i]);
            }

            try
            {
                switch (command)
                {
                    case "showid":
                        return this.ID;
                    case "loadsettings":
                        string type = parameters[0];
                        if (type != "t" && type != "r") return null;

                        OpenFileDialog dialog = new OpenFileDialog
                        {
                            Filter = $"Настроечный файл {(type == "t" ? "техпроцесса" : "ресурса")}|*.xml",
                            Multiselect = false
                        };
                        dialog.ShowDialog();
                        if (string.IsNullOrWhiteSpace(dialog.FileName)) return null;
                        string registerString = "register {0} {1}";
                        if (type == "t")
                        {
                            using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                            {
                                XmlSerializer XmlTechnologySerializer = new XmlSerializer(typeof(Technology));
                                TAgent = (Technology)XmlTechnologySerializer.Deserialize(fs);
                                SendMessageToServer(string.Format(registerString, "t", TAgent.WorkflowID));
                            }
                        }
                        else if (type == "r")
                        {
                            using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                            {
                                XmlSerializer XmlResourceSerializer = new XmlSerializer(typeof(Resource));
                                RAgent = (Resource)XmlResourceSerializer.Deserialize(fs);
                                SendMessageToServer(string.Format(registerString, "r", RAgent.WorkflowID));
                            }
                        }
                        return 0;
                    default: return null;
                }
            }
            catch (Exception ex)
            {
                ClientSendMessageEventArgs args = new ClientSendMessageEventArgs();
                args.Message = "";
                Exception mainException = ex;
                while (mainException != null)
                {
                    args.Message += "\nОшибка: " + mainException.Message + "\nStack Trace:\n" + mainException.StackTrace + "\n";
                    mainException = mainException.InnerException;
                }
                args.Time = DateTime.Now;
                OnInformationLog?.Invoke(this, args);
                return ex;
            }
        }

        public void HandleMessage(string message)
        {
            if (!IsConnected) return;
            string command = message.Split(' ')[0];
            List<string> parameters = new List<string>();
            for (int i = 1; i < message.Split(' ').Length; i++)
            {
                parameters.Add(message.Split(' ')[i]);
            }

            try
            {
                switch (command)
                {
                    case "setsystemid":
                        SetSystemID(parameters[0]);
                        break;
                    case "getstatus":
                        GetStatus(parameters[0]);
                        break;
                    case "getresource":
                        GetResourceData(parameters[0]);
                        break;
                    case "gettechnology":
                        GetTechnologyData(parameters[0]);
                        break;
                }
            }
            catch
            {

            }
        }

        public void SetSystemID(string id) => this.ID = id;
        public void GetStatus(string whoAskID)
        {
            AgentInfo info = new AgentInfo();
            info.SystemID = this.ID;
            if (RAgent != null) info.WorkflowID = RAgent.WorkflowID;
            if (TAgent != null) info.WorkflowID = TAgent.WorkflowID;
            info.Plan = new List<AgentInfoWorkflowData>();

            shedule.Sort((x, y) =>
            {
                if (x.From < y.From) return -1;
                if (x.From > y.From) return 1;
                if (x.From == y.From) return 0;
                return 0;
            });
            for (int i = 0; i < shedule.Count; i++)
            {
                AgentInfoWorkflowData data = new AgentInfoWorkflowData();
                data.ID = i;
                data.State = shedule[i].State;
                data.ParentTechnologySystemID = shedule[i].ParentTechID;
                data.ParentTechnologyWorkflowID = shedule[i].ParentTechWorkflowID;

                info.Plan.Add(data);
            }

            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(AgentInfo));
            serializer.Serialize(writer, info);
            SendMessageToServer("agentstatusout " + whoAskID + " " + writer.ToString());
        }

        public void GetResourceData(string whoAskID)
        {
            if (RAgent == null) return;
            XmlSerializer serializer = new XmlSerializer(typeof(Resource));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this.RAgent);
            SendMessageToServer("agentresourcedata " + whoAskID + " " + writer.ToString());
        }

        public void GetTechnologyData(string whoAskID)
        {
            if (TAgent == null) return;
            XmlSerializer serializer = new XmlSerializer(typeof(Technology));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this.TAgent);
            SendMessageToServer("agenttechnologydata " + whoAskID + " " + writer.ToString());
        }
    }
}
