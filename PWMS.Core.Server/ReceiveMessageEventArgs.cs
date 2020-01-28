namespace PWMS.Core.Server
{
    public delegate void ReceiveMessageEventHandler(object sender, ReceiveMessageEventArgs e);
    public class ReceiveMessageEventArgs
    {
        public string FromID { get; set; }
        public string Message{ get; set; }
    }
}