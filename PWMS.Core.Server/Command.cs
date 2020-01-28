using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.Core.Server
{
    [XmlRoot("Command")]
    public class Command
    {
        [XmlAttribute("CommandName")]
        public string CommandName { get; set; }

        [XmlArray("Parameters")]
        [XmlArrayItem("Paramter")]
        public List<CommandParameter> Parameters { get; set; }
    }

    public class CommandParameter
    {
        [XmlAttribute("Parameter")]
        public string Key;

        [XmlAttribute("Value")]
        public string Value;
    }
}
