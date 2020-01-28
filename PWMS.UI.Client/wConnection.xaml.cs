using PWMS.Core.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private wLog LogWindow = new wLog();

        private void ConnectHandler(object sender, RoutedEventArgs e)
        {
            tb_IPAddress.Background = new SolidColorBrush(Colors.LightGray);
            tb_Port.Background = new SolidColorBrush(Colors.LightGray);
            bool error = false;

            if (!Regex.IsMatch(tb_IPAddress.Text, @"^((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))\.((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))\.((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))\.((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))$$"))
            {
                error = true;
                tb_IPAddress.Background = new SolidColorBrush(Colors.Red);
            }
            if (!IPAddress.TryParse(tb_IPAddress.Text, out IPAddress ip))
            {
                error = true;
                tb_IPAddress.Background = new SolidColorBrush(Colors.Red);
            }
            if (!int.TryParse(tb_Port.Text, out int port))
            {
                error = true;
                tb_Port.Background = new SolidColorBrush(Colors.Red);
            }
            if (port < 0 || port >= 65536)
            {
                error = true;
                tb_Port.Background = new SolidColorBrush(Colors.Red);
            }

            Global.Client = new Core.Client.Client();
            LogWindow = new wLog();
            Global.Client.OnMessageSendToServer += delegate (object o, ClientSendMessageEventArgs args) { LogWindow.Dispatcher.Invoke(delegate () { LogWindow.WriteLog(args.Message); }); };
            Global.Client.OnConnected += delegate (object o, EventArgs args) { LogWindow.Dispatcher.Invoke(delegate () { LogWindow.WriteLog("Клиент успешно подключился к серверу"); }); };
            Global.Client.OnMessageReceivedFromServer += delegate (object o, ClientSendMessageEventArgs args) { LogWindow.Dispatcher.Invoke(delegate () { LogWindow.WriteLog(args.Message); }); };
            Global.Client.OnServerShutdown += delegate (object o, EventArgs args) { LogWindow.Dispatcher.Invoke(delegate () { LogWindow.WriteLog("Сервер отключился..."); }); };
            Global.Client.OnInformationLog += delegate (object o, ClientSendMessageEventArgs args) { LogWindow.Dispatcher.Invoke(delegate () { LogWindow.WriteLog(args.Message); }); };
            Exception connectException = Global.Client.Connect(ip, port);

            if (connectException != null)
            {
                error = true;
                tb_Port.Background = new SolidColorBrush(Colors.Red);
                tb_IPAddress.Background = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Ошибка при подключении к серверу: " + connectException.Message);
            }

            if (error) return;

            LogWindow.Show();
            this.Close();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Global.ServerIP) || string.IsNullOrWhiteSpace(Global.ServerPort))
                return;

            tb_IPAddress.Text = Global.ServerIP;
            tb_Port.Text = Global.ServerPort;
            b_Connect.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
