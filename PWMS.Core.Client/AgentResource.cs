using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Client
{
    [XmlRoot("Agent")]
    public class Resource
    {
        [XmlAttribute("Id")]
        public string WorkflowID { get; set; }

        [XmlAttribute("View")]
        public string View { get; set; }

        [XmlAttribute("Group")]
        public string Group { get; set; }

        [XmlAttribute("Ip_address")]
        public string IPAddress { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Parameter")]
        public List<AgentParameter> Parameters { get; set; }

        public string GetParameter(string key)
        {
            return Parameters.Where(x => x.Title == key).Select(x => x.Value).FirstOrDefault();
        }
    }

    [XmlRoot("Paramter")]
    public class AgentParameter
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
