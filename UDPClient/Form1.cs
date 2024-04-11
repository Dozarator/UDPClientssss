using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace udpClient
{
    public partial class Form1 : Form
    {
        private static IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.220.236"), 35486);
        private static UdpClient udpClient = new UdpClient(34285);
        private static string ipFrom = "192.168.220.225";
        private static string ipTo = "";
        private System.Threading.Timer _timer = null;
        public Form1()
        {
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            //ipFrom = addr[1].ToString();

            InitializeComponent();

            _timer = new System.Threading.Timer(TimerCallback, null, 0, 10000);
        }

        private void button1_Click(object sender, EventArgs e)
        {


            Message message = new Message(Command.Text, Text.Text, ipTo, ipFrom);
            string messageJson = message.ToJSON();
            byte[] messageData = Encoding.UTF8.GetBytes(messageJson);
            udpClient.Send(messageData, ep);
        }


        private static void TimerCallback(Object o)
        {
            if (ipTo != "")
            {
                Message message = new Message("Update", "", ipTo, ipFrom);
                string messageJson = message.ToJSON();

                byte[] messageData = Encoding.UTF8.GetBytes(messageJson);
                udpClient.Send(messageData, ep);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            ipTo = IpToBox.Text;
            Updater();
            await ClientListner();
        }

        private async void Updater()
        {
            var curDir = Directory.GetCurrentDirectory();
            curDir = curDir.Replace("\\", "/");
            string newFileName = String.Format($"{curDir}/history/{ipFrom + " = " + ipTo}.txt");
            if (File.Exists(newFileName))
            {
                string fileText = File.ReadAllText(newFileName);
                textBox1.Text = fileText;
            }
        }
        public static async Task ClientListner()
        {
            while (true)
            {
                var curDir = Directory.GetCurrentDirectory();
                curDir = curDir.Replace("\\", "/" );
                string newFileName = String.Format($"{curDir}/history/{ipFrom + " = " + ipTo}.txt");
                var receiveResult = await udpClient.ReceiveAsync();
                byte[] answerData = receiveResult.Buffer;
                File.WriteAllBytes(newFileName, answerData);


            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }
    }
}
