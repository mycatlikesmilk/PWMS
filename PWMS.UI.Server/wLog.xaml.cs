using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PWMS.UI.Server
{
    public partial class wLog : Window
    {
        public wLog()
        {
            InitializeComponent();
        }

        public void WriteLog(string message)
        {
            tb_Log.Text += $"\n[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] {message}";
        }
    }
}
