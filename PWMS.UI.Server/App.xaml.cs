using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PWMS.UI.Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void StartUpHandler(object sender, StartupEventArgs e)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < e.Args.Length; i++)
            {
                try
                {
                    parameters.Add(e.Args[i].Split('=')[0].ToLower(), e.Args[i].Split('=')[1].ToLower());
                }
                catch
                {
                    parameters.Add(e.Args[i].ToLower(), e.Args[i].ToLower());
                }
            }

            if (parameters.ContainsKey("ip"))
            {
                Global.ServerIP = parameters["ip"];
            }
            if (parameters.ContainsKey("port"))
            {
                Global.ServerPort = parameters["port"];
            }
        }
    }
}
