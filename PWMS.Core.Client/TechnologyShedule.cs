using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Client
{
    [XmlRoot("Plan")]
    public class TechnologyShedule
    {
        [XmlAttribute("From")]
        public DateTime FromTime { get; set; }

        [XmlAttribute("To")]
        public DateTime ToTime { get; set; }

        [XmlAttribute("Name")]
        public string OperationName { get; set; }

        [XmlAttribute("TechprocessRef")]
        public string TechprocessRefID { get; set; }

        [XmlAttribute("ResourceRef")]
        public string ResourceRefID { get; set; }

        public TechnologyShedule() { }

        public TechnologyShedule(DateTime from, DateTime to, string operationName)
        {
            this.FromTime = from;
            this.ToTime = to;
            this.OperationName = operationName;
        }
    }
}
