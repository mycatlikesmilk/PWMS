using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PWMS.Core.Client;

namespace PWMS.UI.Client
{
    static class Global
    {
        public static Core.Client.Client Client { get; set; }
        public static string ServerIP { get; set; }
        public static string ServerPort { get; set; }
    }
}
