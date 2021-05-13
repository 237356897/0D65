using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.ToolKit;

namespace CommonLibrary
{
    public class SocketClient
    {
        private NetworkStream networkStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private TcpClient client;

        public Common.void_StringDelegate AppendLog { get; set; }

        public string SocketDeviceName { get; set; }

        /// <summary>
        /// 创建连接（本地客户端）
        /// </summary>
        /// <param name="ipAddress">目标IP地址</param>
        /// <param name="portNo">目标端口</param>
        /// <param name="receiveTimeout"></param>
        /// <param name="sendTimeout"></param>
        /// <returns></returns>
        public bool Connect(string ipAddress, string portNo, int receiveTimeout, int sendTimeout)
        {
            bool isPass = false;
            try
            {
                IPAddress ip = IPAddress.Parse(ipAddress);
                int port = int.Parse(portNo);
                IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
                client = new TcpClient(AddressFamily.InterNetwork);
                client.Connect(ip, port);
                client.ReceiveTimeout = receiveTimeout;
                client.SendTimeout = sendTimeout;
                networkStream = client.GetStream();
                streamReader = new StreamReader(networkStream, Encoding.GetEncoding("gb2312"));
                streamWriter = new StreamWriter(networkStream, Encoding.GetEncoding("gb2312"));
                isPass = true;
            }
            catch (Exception e)
            {
                AppendLog(string.Format("Socket连接异常：{0}", e.Message));
                MessageBox.Show(string.Format("Socket连接异常：{0}", e.Message));
            }
            return isPass;
        }

        /// <summary>
        /// 断开当前连接
        /// </summary>
        public void DisConnect()
        {
            networkStream.Close();
            streamReader.Close();
            streamWriter.Close();
            client.Close();
        }

        /// <summary>
        /// 触发一次
        /// </summary>
        /// <returns>返回接收的字符串数据</returns>
        private string triggerOnce()
        {
            string data = "TimeOut";
            string cmd = "+";
            send(cmd);
            data = streamReader.ReadLine();

            return data;
        }

        /// <summary>
        /// 触发
        /// </summary>
        /// <returns>返回接收的字符串数据</returns>
        public string Trigger()
        {
            string data = "TimeOut";
            int retries = 3;
            for (int i = 0; i <= retries; ++i)
            {
                try
                {
                    data = triggerOnce();
                    if (data != "TimeOut")
                    { break; }
                }
                catch
                { Thread.Sleep(10); }
            }

            return data;
        }

        /// <summary>
        /// 清空接收缓冲区
        /// </summary>
        private void clearReceiveBuff()
        {
            byte[] buffer = new byte[4096];
            int available = client.Available;           
            while (available > 0)
            {
                client.Client.Receive(buffer, SocketFlags.None);
                available = client.Available;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="str">字符串数据</param>
        private void send(string str)
        {
            clearReceiveBuff();
            //向当前的数据流写入一行字符串数据
            streamWriter.WriteLine(str);
            //刷新当前数据流中的数据
            streamWriter.Flush();
        }
    }
}
