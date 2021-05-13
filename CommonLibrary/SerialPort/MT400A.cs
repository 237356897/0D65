using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Framework;
using log4net;
using System.Text.RegularExpressions;

namespace System.Device
{
    /// <summary>
    /// 阿特拉斯电批规通信类
    /// </summary>
    public class MT400A : SerialPort, ISerialPortTriggerModel
    {
        private Object obj = new Object();
        private static ILog log = LogManager.GetLogger(typeof(MT400A));
        /* 
         * :TRIG:SOUR EXT/IMM<CR>
         * :INIT:CONT ON/OFF<CR>
         */

        #region 字段

        string receiveString = "";

        #endregion

        #region 构造函数

        public MT400A() : base() { }

        /// <summary>
        /// 使用指定的端口名称、波特率、奇偶校验位、数据位和停止位初始化 System.IO.Ports.SerialPort 类的新实例。
        /// </summary>
        /// <param name="portName">要使用的端口（例如 COM1）。</param>
        /// <param name="baudRate">波特率。</param>
        /// <param name="parity"> System.IO.Ports.SerialPort.Parity 值之一。</param>
        /// <param name="dataBits">数据位值。</param>
        /// <param name="stopBits">System.IO.Ports.SerialPort.StopBits 值之一。</param>
        public MT400A(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits) { }

        #endregion

        #region 方法

        #endregion

        #region 接口

        public event DataReceiveCompleteEventHandler DeviceDataReceiveCompelete;

        public string Name { get; set; }
        public string ConnectionParam { get; set; }

        public void SetConnectionParam(string str)
        {
            ConnectionParam = str;
            string[] param = ConnectionParam.Split(',');
            PortName = param[0];
            BaudRate = int.Parse(param[1]);
            Parity = (Parity)Enum.Parse(typeof(Parity), param[2]);
            DataBits = int.Parse(param[3]);
            StopBits = (StopBits)Enum.Parse(typeof(StopBits), param[4]);
            ReadTimeout = int.Parse(param[5]);
            WriteTimeout = int.Parse(param[6]);
        }

        public void DeviceOpen()
        {
            if (IsOpen)
                throw new Exception("设备已经连接\n");

            Open();
        }

        public void DeviceClose()
        {
            if (IsOpen)
            {
                Application.DoEvents();
                Close();
            }
        }

        delegate void TriggerDelegate(TriggerArgs args);
        TriggerDelegate triggerMethod;
        public IAsyncResult TriggerResult;
        public IAsyncResult BeginTrigger(TriggerArgs args)
        {
            if (triggerMethod == null)
                triggerMethod = new TriggerDelegate(Trigger);

            if (TriggerResult != null && !TriggerResult.IsCompleted)
                return TriggerResult;

            TriggerResult = triggerMethod.BeginInvoke(args, null, null);

            return TriggerResult;
        }
        
        [ExecuteInfo("TRG,通道号,命令", "读取电批数据", "TRG,0,TR")]
        public void Trigger(TriggerArgs args)
        {
            var msg = args.message.Split(',');
            for (int i = 0; i < args.tryTimes; i++)
            {
                try
                {
                    lock (obj)
                    {
                        List<byte> lst = new List<byte>();

                        //STX
                        lst.Add(0x02);

                        //通道号
                        var channel = int.Parse(msg[0]);
                        byte[] st = Comm.Asc(channel.ToString("x"));
                        lst.AddRange(st);

                        //指令
                        lst.AddRange(Comm.Asc(msg[1]));

                        //校验码
                        byte checkResult = Comm.CheckBySum(lst.GetRange(1, lst.Count - 1).ToArray());
                        lst.AddRange(Comm.Asc(checkResult.ToString("x2")));

                        //ETX  
                        lst.Add(0x03);

                        DiscardInBuffer();
                        DiscardOutBuffer();
                        Write(lst.ToArray(), 0, lst.Count);
                        receiveString = ReadTo(Comm.Chr(new byte[] { 0x03 }));
                        log.Debug("Receive Data: " + receiveString);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("电批触发异常" + ex.Message);
                    receiveString = UniversalFlags.errorStr + ex.Message;
                }
            }
            if (args.sender.GetType() != this.GetType())
            {
                if (DeviceDataReceiveCompelete != null)
                    DeviceDataReceiveCompelete(this, receiveString);
            }
        }


        public string StopTrigger()
        {
            return UniversalFlags.successStr;
        }

        public string Execute(string cmd)
        {
            string result = "";
            cmd = cmd.Trim().ToUpper();
            if (Regex.IsMatch(cmd, "^HELP"))
            {
                Attribute[] attribs = this.GetType().GetMethods().Select(s =>
                    Attribute.GetCustomAttribute(s, typeof(ExecuteInfo))
                    ).Where(s => s != null).ToArray();

                result = Environment.NewLine + "-----------------------------------" + Environment.NewLine;
                result += Environment.NewLine + "该设备支持以下指令：" + Environment.NewLine;
                foreach (Attribute attrib in attribs)
                {
                    ExecuteInfo exe = (ExecuteInfo)attrib;
                    result += Environment.NewLine + exe.Command + " - " + exe.Describe + Environment.NewLine
                        + "示例：" + exe.Example + Environment.NewLine;
                }
                result += Environment.NewLine + "-----------------------------------" + Environment.NewLine;
            }
            else if (Regex.IsMatch(cmd, "^TRG"))
            {
                string[] args = cmd.Split(',');
                Trigger(new TriggerArgs()
                {
                    sender = this,
                    tryTimes = 1,
                    message = args[1] + "," + args[2]
                });
                result = receiveString;
            }
            else
            {
                result = "不支持指令" + cmd;
            }
            return result;
        }
        #endregion
    }
}
