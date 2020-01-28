using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Server
{
    public class ClientElement
    {
        private static int _NextID;

        private TcpClient client;
        private Server server;
        private string ID;

        public string WorkflowID;
        public string AgentType;
        
        public ClientElement(TcpClient client, Server server)
        {
            this.client = client;
            this.server = server;
            this.ID = _NextID.ToString();
            _NextID++;

            Task getNewMessagesTask = Task.Factory.StartNew(GetNewMessage);
        }

        public void GetNewMessage()
        {
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] data = new byte[65536];
                    int bytes = stream.Read(data, 0, data.Length);
                    string res = Encoding.UTF8.GetString(data, 0, bytes);
                    HandleCommand(res);
                    server.InvokeOnMessageReceivedEvent(this, new ReceiveMessageEventArgs() { FromID = this.ID, Message = res });
                }
            }
            catch
            {
                server.InvokeOnClientDisconnected(this, new EventArgs());
                server.Clients.Remove(this);
            }
        }

        public void SendMessageToAgent(string message)
        {
            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void HandleCommand(string message)
        {
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
                    case "send":
                        SendMessageToAnotherAgent(parameters[0], parameters[1]);
                        break;
                    case "getsystemid":
                        GetSystemID();
                        break;
                    case "register":
                        this.AgentType = parameters[0];
                        this.WorkflowID = parameters[1];
                        SendMessageToAgent($"Агент {this.ID} зарегистрирован как {(parameters[0] == "t" ? "техпроцесс" : "ресурс")} с типом {this.WorkflowID}");
                        break;
                    case "getagentnet":
                        GetAgentNet();
                        break;
                    case "getagentstatus":
                        GetAgentStatus(parameters[0]);
                        break;
                    case "getresourcedata":
                        GetResource(parameters[0]);
                        break;
                    case "gettechnologydata":
                        GetTechnology(parameters[0]);
                        break;
                    case "agentstatusout":
                        SendMessageToAnotherAgent(parameters[0], message);
                        break;
                    case "agentresourcedata":
                        SendMessageToAnotherAgent(parameters[0], message);
                        break;
                    case "agenttechnologydata":
                        SendMessageToAnotherAgent(parameters[0], message);
                        break;
                    default:
                        SendMessageToAgent("out: такой команды не существует");
                        break;
                }
            }
            catch
            {
                SendMessageToAgent("out: команда введена неправильно. Проверьте соответствие подставляемых параметров");
            }
        }

        public void SendMessageToAnotherAgent(string agentToID, string message)
        {
            ClientElement element = server.Clients.Where(x => x.ID == agentToID).FirstOrDefault();
            if (element != null)
            {
                element.SendMessageToAgent(message);
            }
            else
            {
                this.SendMessageToAgent("out: Такого агента нет в системе");
            }
        }
        public void GetSystemID() => this.SendMessageToAgent("setsystemid " + this.ID);
        public void GetAgentNet()
        {
            AgentNet net = new AgentNet();
            net.Agents = new List<AgentNetElement>();
            for (int i = 0; i < server.Clients.Count; i++)
            {
                AgentNetElement element = new AgentNetElement();
                element.SystemID = server.Clients[i].ID;
                element.WorkflowID = server.Clients[i].WorkflowID;
                element.Type = server.Clients[i].AgentType;
                net.Agents.Add(element);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(AgentNet));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, net);
            SendMessageToAgent(writer.ToString());
        }
        public void GetAgentStatus(string agentID)
        {
            SendMessageToAnotherAgent(agentID, "getstatus " + this.ID);
        }
        public void GetResource(string agentID)
        {
            SendMessageToAnotherAgent(agentID, "getresource " + this.ID);
        }
        public void GetTechnology(string agentID)
        {
            SendMessageToAnotherAgent(agentID, "gettechnology " + this.ID);
        }
    }
}