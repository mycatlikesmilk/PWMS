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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PWMS.UI.Client
{
    public partial class CommandHelperElement : UserControl
    {
        public CommandHelperElement(string name = "", string details = "", string format = "")
        {
            InitializeComponent();
            tb_CommandName.Text = name;
            tb_CommandDetails.Text = details;
            tb_CommandFormat.Text = format;
        }

        public void AddParameter(CommandHelperParameterElement parameter)
        {
            sp_Parameters.Children.Add(parameter);
        }
    }
}
