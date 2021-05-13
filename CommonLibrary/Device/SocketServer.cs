using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ToolKit;
using System.Windows.Forms;
using System.Threading;

namespace CommonLibrary
{
    public class SocketServer : MySocket
    {
        private Socket socket;
        TcpClient client;
        private IPEndPoint ipEnd;
        private string deviceName="";
        private char endMark = '\r';
        private bool isClose = false;
        private object thisObj = new object();
        private string ServerPort = "60000";
        private string MySocketType = "Server";

        public SocketServer(string name) : base()
        {
            deviceName = name;
        }
        protected override string ipAddress { get { return getIPAddress(); } set { } }

        protected override string port
        {
            get { return ServerPort; }
            set { ServerPort = value; }
        }
        protected override bool isSending { get; set; }
        protected override void connect()
        {
            loadIPEndPoint();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(ipEnd);
            socket.Listen(100);

            asyncAccept();

            AppendLog.Invoke(string.Format("{0}开启监听。。。", deviceName));
        }

        protected override void disconnect()
        {
            isClose = true;
            string[] ipPort = ipEnd.ToString().Split(':');
            client = new TcpClient(AddressFamily.InterNetwork);
            client.Connect(IPAddress.Parse(ipPort[0]), int.Parse(ipPort[1]));
        }


        #region Private Method
        private string getIPAddress()
        {
            string ipAddress = "";
            string hostName = Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(hostName);
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                    break;
                }
            }

            return "127.0.0.1"/*ipAddress*/;
        }

        private void loadIPEndPoint()
        {
            string ip = ipAddress;
            if (ip != "")
            {
                ipEnd = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            }
            else
            {
                MessageBox.Show("获取本地IP地址失败！");
                AppendLog.Invoke("获取本地IP地址失败！");
            }
        }

        private void asyncAccept()
        {
            socket.BeginAccept(asyncResult =>
            {
                if (socket != null)
                {
                    Socket client = socket.EndAccept(asyncResult);
                    if (isClose)
                    {
                        isClose = false;
                        socket.Close();
                        client.Close();
                    }
                    else
                    {
                        asyncAccept();
                        asyncReceiving(client);
                    }
                }
            }, null);
        }
        private void asyncReceiving(Socket client)
        {
            byte[] data = new byte[4096];
            try
            {
                client.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    lock (thisObj)
                    {
                        int length = client.EndReceive(asyncResult);
                        string str = Encoding.UTF8.GetString(data).Split(endMark)[0];
                        DataReceived.Invoke(client, str);
                        if (!DeleteSockets.Contains(client))
                        { asyncReceiving(client); }
                    }
                }, null);
            }
            catch (Exception ex)
            {
                AppendLog.Invoke(string.Format("({0}接收异常) {1}", deviceName, ex.ToString()));
            }
        }
        private void asyncSending(Socket client, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message + endMark);
            try
            {
                client.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    //完成发送消息
                    int length = client.EndSend(asyncResult);
                    AppendLog.Invoke(string.Format("({0}-{1}发送完成) {2}", deviceName, client.RemoteEndPoint.ToString(), message));
                    isSending = false;
                }, null);
            }
            catch (Exception ex)
            {
                AppendLog.Invoke(string.Format("({0}发送异常) {1}", deviceName, ex.ToString()));
            }
        }

        #endregion

        #region AsyncCall
        protected override void asyncSend(Socket client, string message)
        { asyncSending(client, message); }
        #endregion
    }
}
