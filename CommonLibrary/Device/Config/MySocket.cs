using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace CommonLibrary
{
    public abstract class MySocket
    {
        public Common.void_StringDelegate AppendLog;
        public Common.void_SocketStringDelegate DataReceived;
        public List<Socket> DeleteSockets = new List<Socket>();
        public string IpAddress { get { return ipAddress; }set { ipAddress = value; } }
        public string Port { get { return port; }set { port = value; } }
        public bool IsSending { get { return isSending; } set{ isSending = value; }  }
        public void Connect()
        { connect(); }

        public void Disconnect()
        { disconnect(); }

        public void AsyncSend(Socket client, string message)
        {
            IsSending = true;
            asyncSend(client, message);
        }

        protected abstract string ipAddress { get; set; }
        protected abstract string port { get; set; }
        protected abstract bool isSending { get; set; }
        protected abstract void connect();
        protected abstract void disconnect();
        protected abstract void asyncSend(Socket client, string message);  

    }
}
