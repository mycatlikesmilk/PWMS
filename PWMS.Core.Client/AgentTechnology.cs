using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Client
{
    [XmlRoot("Technology")]
    public class Technology
    {
        [XmlAttribute("Id")]
        public string WorkflowID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("ip_address")]
        public string IPAddress { get; set; }

        [XmlElement("Configuration")]
        public Configuration Configuration { get; set; }

        [XmlArray("Plan")]
        [XmlArrayItem("WorkflowProcess")]
        public List<WorkflowProcess> Plan { get; set; }
    }

    public class Configuration
    {
        [XmlAttribute("DetailsCount")]
        public int DetailsCount { get; set; }

        [XmlIgnore]
        public DateTime? TimeStart { get; set; }

        [XmlAttribute("TimeStart")]
        public string TimeStartString
        {
            get => TimeStart.HasValue ? TimeStart.Value.ToString("dd.MM.yyyy HH:mm:ss") : null;
            set
            {
                if (!DateTime.TryParse(value, out DateTime res))
                {
                    TimeStart = null;
                }
            }
        }

        [XmlIgnore]
        public DateTime? TimeEnd { get; set; }

        [XmlAttribute("TimeEnd")]
        public string TimeEndString
        {
            get => TimeEnd.HasValue ? TimeEnd.Value.ToString("dd.MM.yyyy HH:mm:ss") : null;
            set
            {
                if (!DateTime.TryParse(value, out DateTime res))
                {
                    TimeEnd = null;
                }
            }
        }
    }

    [XmlRoot("WorkflowProcess")]
    public class WorkflowProcess
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("EstimatedTime")]
        public int EstimatedTime { get; set; }

        [XmlAttribute("OperationName")]
        public string OperationName { get; set; }

        [XmlElement("Resource")]
        public List<TechnologyResource> Resources { get; set; }
    }

    [XmlRoot("Resource")]
    public class TechnologyResource
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Count")]
        public int Count { get; set; }
    }
}
