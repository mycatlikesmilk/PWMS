using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMS.UI.Server
{
    class Global
    {
        public static Core.Server.Server Server { get; set; }
        public static string ServerIP { get; set; }
        public static string ServerPort { get; set; }
    }
}
