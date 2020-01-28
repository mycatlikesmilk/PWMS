using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Server
{
    [XmlRoot("AgentNet")]
    public class AgentNet
    {
        [XmlElement("Agent")]
        public List<AgentNetElement> Agents;
    }

    [XmlRoot("Agent")]
    public class AgentNetElement
    {
        [XmlAttribute("WorkflowID")]
        public string WorkflowID { get; set; }

        [XmlAttribute("SystemID")]
        public string SystemID { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
}
