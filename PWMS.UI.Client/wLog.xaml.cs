using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace PWMS.UI.Client

{
    public partial class wLog : Window
    {
        private string lastCommand;

        public wLog()
        {
            InitializeComponent();
        }

        public void WriteLog(string value)
        {
            tb_Log.Text += $"\n[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}]: {value}";
        }

        private void SendMessageHandler(object sender, RoutedEventArgs e)
        {
            SendMessage(tb_Command.Text);
        }

        private void SendMessage(string message, bool clearCommandBox = true)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            lastCommand = message;
            object data = Global.Client.HandleLocalMessage(message);
            if (data == null) Global.Client.SendMessageToServer(message);
            if (clearCommandBox) tb_Command.Clear();
            tb_Log.ScrollToEnd();
        }

        private void SendMessageByKeyboardHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage(tb_Command.Text);
            if (e.Key == Key.Up && !string.IsNullOrWhiteSpace(lastCommand))
                tb_Command.Text = lastCommand;
        }

        private void TextChangedHandler(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tb_Command.Text.Length > 2)
                {
                    using (StreamReader reader = new StreamReader("AvailableCommands.xml"))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(XmlDocumentation));
                        XmlDocumentation doc = (XmlDocumentation)serializer.Deserialize(reader);
                        XmlDocumentationCommand cmd = doc.Commands.Where(x => x.Keyword.Contains(tb_Command.Text.Split(' ')[0])).FirstOrDefault();
                        if (cmd == null)
                        {
                            p_CommandHelper.IsOpen = false;
                            return;
                        }

                        CommandHelperElement element = new CommandHelperElement(cmd.Name, cmd.Description, cmd.Format);

                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            XmlDocumentationCommandParameter prm = cmd.Parameters[i];
                            CommandHelperParameterElement parameter = new CommandHelperParameterElement(prm.Name, prm.Details, prm.IsRequired);
                            element.AddParameter(parameter);
                        }

                        p_CommandHelper.Child = element;

                        p_CommandHelper.IsOpen = true;
                    }
                }
                else
                {
                    p_CommandHelper.IsOpen = false;
                }
            }
            catch
            {

            }
        }

        private void OnClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
