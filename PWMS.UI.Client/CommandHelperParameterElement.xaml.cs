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
    public partial class CommandHelperParameterElement : UserControl
    {
        private bool isRequired;

        public string ParameterName
        {
            get
            {
                return this.tb_ParameterName.Text;
            }
            set
            {
                this.tb_ParameterName.Text = value;
            }
        }
        public string ParameterDetails
        {
            get
            {
                return tb_ParameterDetails.Text;
            }
            set
            {
                tb_ParameterDetails.Text = value;
            }
        }
        public bool IsRequired
        {
            get
            {
                return isRequired;
            }
            set
            {
                isRequired = value;
            }
        }

        public CommandHelperParameterElement(string name = "", string details = "", bool required = false)
        {
            InitializeComponent();
            ParameterName = name;
            ParameterDetails = details;
            IsRequired = required;
        }
    }
}
