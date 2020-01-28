using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PWMS.UI.Client
{
    [XmlRoot("Documentation")]
    public class XmlDocumentation
    {
        [XmlArray("Commands")]
        [XmlArrayItem("Command")]
        public List<XmlDocumentationCommand> Commands { get; set; }
    }

    [XmlRoot("Command")]
    public class XmlDocumentationCommand
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Keyword")]
        public string Keyword { get; set; }

        [XmlElement("Details")]
        public string Description { get; set; }

        [XmlElement("Format")]
        public string Format { get; set; }

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<XmlDocumentationCommandParameter> Parameters { get; set; }
    }

    [XmlRoot("Parameter")]
    public class XmlDocumentationCommandParameter
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Details")]
        public string Details { get; set; }

        [XmlElement("Required")]
        public bool IsRequired { get; set; }
    }
}
