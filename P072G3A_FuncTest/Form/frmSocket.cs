using desay.ProductData;
using log4net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Toolkit.Helpers;
using System.ToolKit;
using System.Windows.Forms;

namespace CommonLibrary
{
    public partial class frmSocket : Form
    {
        private ILog log = LogManager.GetLogger(typeof(frmSocket));
        public frmSocket()
        {
            InitializeComponent();
        }
        public frmSocket(string deviceName)
            : this()
        {
            socketDeviceName = deviceName;
        }


        public static Common.void_StringDelegate AppendLog;
        public Common.void_SocketStringDelegate DataReceived;
        public Dictionary<string, Socket> Sockets = new Dictionary<string, Socket>();
        public Dictionary<Socket, string> StrSockets = new Dictionary<Socket, string>();
        public bool IsSending = false;

        private string socketDeviceName = "Socket";
        private MySocket mySocket;

        private void frmSocket_Load(object sender, EventArgs e)
        {
            #region ComBoxItemSelect
            bool isExecute = true;
            while (isExecute)
            {
                isExecute = false;
                string strTemp = "Server";
                if (toolStripCmbxSocketType.Items.Contains(strTemp))
                {
                    toolStripCmbxSocketType.SelectedItem = strTemp;
                }
                else
                {
                    isExecute = true;
                    MessageBox.Show("配置文件中Socket Type加载异常!");
                }
            }
            #endregion

            setParameter();

            toolStripBtnConn_Click(sender, e);
        }

        #region Private Method

        private void appendText(string str)
        {
            AppendLog.Invoke(string.Format("<<{1}界面>>:: {0}", str, socketDeviceName));
        }

        private void setTxt(TextBox txt, string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<TextBox, string>(setTxt), txt, str);
            }
            else
            {
                txt.Text = str;
            }
        }

        private void enabled(bool enable)
        {
            toolStripBtnDisconn.Enabled =
            btnSend.Enabled =
            txtSendData.Enabled =
            comboBClients.Enabled = !enable;

            toolStripBtnConn.Enabled =
            toolStripTxtPort.Enabled =
            toolStripCmbxSocketType.Enabled =
            toolStripBtnSave.Enabled = enable;

            if (mySocket is SocketServer)
            {
                toolStripTxtIP.Enabled = false;
            }
            else
            {
                toolStripTxtIP.Enabled = enable;
            }
        }

        private void setParameter()
        {
            switch (toolStripCmbxSocketType.Text)
            {
                case "Client":
                    break;

                case "Server":
                    mySocket = new SocketServer(socketDeviceName);   
                    toolStripBtnConn.ToolTipText = toolStripBtnConn.Text = "监听";
                    break;
            }

            if (AppendLog != null)
            { mySocket.AppendLog += appendText; }
            mySocket.DataReceived += dataReceived;

            toolStripTxtIP.Text = mySocket.IpAddress;
            toolStripTxtPort.Text = mySocket.Port;

            labDeviceModel.Text = string.Format("{0}-{1}", socketDeviceName, (string)toolStripCmbxSocketType.SelectedItem);
        }

        private void dataReceived(Socket client, string str)
        {
            if (btnTRG)
            {
                btnTRG = false;
                setTxt(txtReadData, str);
            }
            else
            { DataReceived.Invoke(client, str); }
            log.DebugFormat("{0}读取数据结束：{1}", socketDeviceName, str);
        }
        #endregion

        #region Public Method
        public void AddSockets(Socket client,string item)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Socket, string>(AddSockets), client, item);
            }
            else
            {
                comboBClients.Items.Add(item);
                Sockets.Add(item, client);
                StrSockets.Add(client, item);
            }
        }

        public void RemoveSocket(Socket client)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Socket>(RemoveSocket), client);
            }
            else
            {
                string deleteTemp = StrSockets[client];
                comboBClients.Items.Remove(deleteTemp);
                Sockets.Remove(deleteTemp);
                StrSockets.Remove(client);
                mySocket.DeleteSockets.Add(client);
            }
        }

        /// <summary>
        /// Socket连接
        /// </summary>
        /// <param name="sender">调用的某对象(默认为null)</param>
        /// <param name="e">调用的某事件(默认为null)</param>
        public void btnConn_Click(object sender = null, EventArgs e = null)
        {
            toolStripBtnConn_Click(sender, e);
        }

        /// <summary>
        /// 发送
        /// </summary>
        public void Send(Socket client, string msg)
        {
            IsSending = true;
            log.DebugFormat("{0}发送数据。。。", socketDeviceName);
            new Action(() =>
            {
                mySocket.AsyncSend(client, msg);
                while (mySocket.IsSending)
                { Thread.Sleep(20); }
                IsSending = false;
            }).BeginInvoke(null,null);
        }
        #endregion
        

        #region FormOperation
        private void toolStripBtnConn_Click(object sender, EventArgs e)
        {
            mySocket.Connect();
            enabled(false);
        }

        private void toolStripBtnDisconn_Click(object sender, EventArgs e)
        {
            mySocket.Disconnect();
            enabled(true);
        }

        private void toolStripBtnSave_Click(object sender, EventArgs e)
        {
            mySocket.IpAddress = toolStripTxtIP.Text;
            mySocket.Port = toolStripTxtPort.Text;
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);
            setParameter();
        }

        private bool btnTRG = false;
        private void btnTrigger_Click(object sender, EventArgs e)
        {     
            BeginInvoke(new Action(() =>
            {
                while (IsSending)
                { Thread.Sleep(20); }
                btnTRG = true;
                Send(Sockets[comboBClients.Text], txtSendData.Text);
            }));
        }

        #endregion

        #region Key Press
        private void toolStripTxtIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.IsDigitalInput(sender, e, '.');
        }

        private void toolStripTxtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.IsDigitalInput(sender, e);
        }

        #endregion

    }
}
