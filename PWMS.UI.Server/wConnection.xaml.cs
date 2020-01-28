using PWMS.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PWMS.UI.Server
{
    public partial class MainWindow : Window
    {
        private wLog LogWindow = new wLog();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateServerHandler(object sender, RoutedEventArgs e)
        {
            tb_IPAddress.Background = new SolidColorBrush(Colors.LightGray);
            tb_Port.Background = new SolidColorBrush(Colors.LightGray);
            bool error = false;

            if (!Regex.IsMatch(tb_IPAddress.Text, @"^((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))\.((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))\.((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))\.((25[0-5])|(2[0-4][0-9])|([0-1]?[0-9]?[0-9]))$"))
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

            Global.Server = new Core.Server.Server();

            Global.Server.OnNewConnectionCreated += delegate (object o, ClientConnectedEventArgs args) { LogWindow.Dispatcher.Invoke(delegate() { LogWindow.WriteLog("Подключен новый агент"); }); };
            Global.Server.OnServerStarted += delegate (object o, EventArgs args) { LogWindow.WriteLog("Сервер настроен и начинает прослушивание"); };
            Global.Server.OnMessageReceived += delegate (object o, ReceiveMessageEventArgs args) { LogWindow.Dispatcher.Invoke(delegate () { LogWindow.WriteLog($"[{args.FromID}] >> {args.Message}"); }); };
            Global.Server.OnClientDisconnected += delegate (object o, EventArgs args) { ClientElement cl = (ClientElement)o; LogWindow.Dispatcher.Invoke(delegate() { LogWindow.WriteLog("Клиент отключился"); }); };

            Exception connectException = Global.Server.CreateServer(ip, port);

            if (connectException != null)
            {
                error = true;
                tb_Port.Background = new SolidColorBrush(Colors.Red);
                tb_IPAddress.Background = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Ошибка при подключении к серверу: " + connectException.Message);
            }

            if (error) return;

            LogWindow.Title = "Сервер [" + tb_IPAddress.Text + ":" + tb_Port.Text+ "]";
            
            LogWindow.Show();
            this.Close();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Global.ServerIP) || string.IsNullOrWhiteSpace(Global.ServerPort))
                return;

            tb_IPAddress.Text = Global.ServerIP;
            tb_Port.Text = Global.ServerPort;
            b_CreateServer.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
