using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Client
{
    [XmlRoot("AgentInformation")]
    public class AgentInfo
    {
        [XmlAttribute("SystemID")]
        public string SystemID { get; set; }

        [XmlAttribute("OPID")]
        public string WorkflowID { get; set; }

        [XmlArray("Plan")]
        [XmlArrayItem("WorkflowData")]
        public List<AgentInfoWorkflowData> Plan { get; set; }
    }

    [XmlRoot("WorkflowData")]
    public class AgentInfoWorkflowData
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("From")]
        public DateTime From { get; set; }

        [XmlAttribute("Duration")]
        public int Duration { get; set; }

        [XmlAttribute("ParentTechprocID")]
        public string ParentTechnologyWorkflowID { get; set; }

        [XmlAttribute("ParentTechprocSystemID")]
        public string ParentTechnologySystemID { get; set; }

        [XmlAttribute("State")]
        public string State { get; set; }
    }
}
