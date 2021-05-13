using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using log4net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace CommonLibrary
{
    /// <summary>
    /// 光源控制
    /// </summary>
    public class ComPort
    {
        private Object obj = new Object();
        private static ILog log = LogManager.GetLogger(typeof(ComPort));
        /* 
         * :TRIG:SOUR EXT/IMM<CR>
         * :INIT:CONT ON/OFF<CR>
         */

        public SerialPort sp = new SerialPort();

        #region 字段

        public string receiveString = "";
        public bool receiveFinish = false;

        #endregion

        #region 构造函数

        public ComPort()
        {
            sp.DataReceived += new SerialDataReceivedEventHandler(DeviceReceive);
        }

        /// <summary>
        /// 使用指定的端口名称、波特率、奇偶校验位、数据位和停止位初始化 System.IO.Ports.SerialPort 类的新实例。
        /// </summary>
        /// <param name="portName">要使用的端口（例如 COM1）。</param>
        /// <param name="baudRate">波特率。</param>
        /// <param name="parity"> System.IO.Ports.SerialPort.Parity 值之一。</param>
        /// <param name="dataBits">数据位值。</param>
        /// <param name="stopBits">System.IO.Ports.SerialPort.StopBits 值之一。</param>
        public ComPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            sp.PortName = portName;
            sp.BaudRate = baudRate;
            sp.Parity = parity;
            sp.DataBits = dataBits;
            sp.StopBits = stopBits;
        }

        #endregion

        #region 方法

        #endregion

        #region 接口

        public event DataReceiveCompleteEventHandler DeviceDataReceiveCompelete;

        public string Name { get; set; }
        public string ConnectionParam { get; set; }

        public Stopwatch startWatch = new Stopwatch();

        public void SetConnectionParam(string str)
        {
            ConnectionParam = str;
            string[] param = ConnectionParam.Split(',');
            sp.PortName = param[0];
            sp.BaudRate = int.Parse(param[1]);
            sp.Parity = (Parity)Enum.Parse(typeof(Parity), param[2]);
            sp.DataBits = int.Parse(param[3]);
            sp.StopBits = (StopBits)Enum.Parse(typeof(StopBits), param[4]);
            sp.ReadTimeout = int.Parse(param[5]);
            sp.WriteTimeout = int.Parse(param[6]);
        }

        public void DeviceOpen()
        {
            if (sp.IsOpen)
                throw new Exception("设备已经连接\n");

            sp.Open();
        }

        public void DeviceClose()
        {
            if (sp.IsOpen)
            {
                Application.DoEvents();
                sp.Close();
            }
        }

        delegate void TriggerDelegate(string cmd = null);
        TriggerDelegate triggerMethod;
        public IAsyncResult TriggerResult;
        public IAsyncResult BeginTrigger(string cmd = null)
        {
            if (triggerMethod == null)
                triggerMethod = new TriggerDelegate(Trigger);

            if (TriggerResult != null && !TriggerResult.IsCompleted)
                return TriggerResult;

            TriggerResult = triggerMethod.BeginInvoke(cmd, null, null);

            return TriggerResult;
        }

        public IAsyncResult BeginTrigger2(string cmd = null)
        {
            if (triggerMethod == null)
                triggerMethod = new TriggerDelegate(Trigger2);

            if (TriggerResult != null && !TriggerResult.IsCompleted)
                return TriggerResult;

            TriggerResult = triggerMethod.BeginInvoke(cmd, null, null);

            return TriggerResult;
        }

        [ExecuteInfo("TRG,从设备地址 功能码 起始寄存器地址 寄存器个数 CRC-L CRC-H", "电流采集", "TRG,01 06 00 FF FF 00")]
        public void Trigger(string cmd = null)
        {
            Byte[] sendData = new Byte[8];
            string[] msg = cmd.Split(' ');
            sendData[0] = Convert.ToByte(msg[0], 16);
            sendData[1] = Convert.ToByte(msg[1], 16);
            sendData[2] = Convert.ToByte(msg[2], 16);
            sendData[3] = Convert.ToByte(msg[3], 16);
            sendData[4] = Convert.ToByte(msg[4], 16);
            sendData[5] = Convert.ToByte(msg[5], 16);
            sendData[6] = Convert.ToByte(msg[6], 16);
            short s = 0;
            for (int i = 0; i < sendData.Length; i++)
            {
                s += sendData[i];
            }
            sendData[7] = (byte)(~s);

            try
            {
                lock (obj)
                {
                    receiveString = null;
                    StopTrigger();
                    sp.DiscardInBuffer();
                    sp.DiscardOutBuffer();
                    sp.Write(sendData, 0, 8);
                }
            }
            catch (Exception ex)
            {
                log.Debug("电流采集写入异常" + ex.Message);
            }
        }
        public void Trigger2(string cmd = null)
        {
            Byte[] sendData = new Byte[8];
            string[] msg = cmd.Split(' ');
            sendData[0] = Convert.ToByte(msg[0], 16);
            sendData[1] = Convert.ToByte(msg[1], 16);
            sendData[2] = Convert.ToByte(msg[2], 16);
            sendData[3] = Convert.ToByte(msg[3], 16);
            sendData[4] = Convert.ToByte(msg[4], 16);
            sendData[5] = Convert.ToByte(msg[5], 16);
            sendData[6] = Convert.ToByte(msg[6], 16);
            sendData[7] = Convert.ToByte(msg[7], 16);
            //short s = 0;
            //for (int i = 0; i < sendData.Length; i++)
            //{
            //    s += sendData[i];
            //}
            //sendData[7] = (byte)(~s);
            try
            {
                lock (obj)
                {
                    receiveString = null;
                    StopTrigger();
                    sp.DiscardInBuffer();
                    sp.DiscardOutBuffer();
                    sp.Write(sendData, 0, 8);
                }
            }
            catch (Exception ex)
            {
                log.Debug("电流采集写入异常" + ex.Message);
            }
        }

        public void DeviceReceive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (obj)
                {
                    int count = sp.BytesToRead;
                    StringBuilder strB = new StringBuilder();
                    Byte[] ReceiveData = new Byte[count];
                    sp.Read(ReceiveData, 0, count);
                    if (ReceiveData.Length > 0)
                    {
                        if (receiveString != null) strB.Append("-");
                        strB.Append(ReceiveData[0].ToString("X2"));
                        for (int j = 1; j < ReceiveData.Length; j++)
                        {
                            strB.Append("-");
                            strB.Append(ReceiveData[j].ToString("X2"));
                        }
                    }
                    receiveString += strB.ToString();
                    startWatch.Restart();
                    while (sp.BytesToRead <= 0 )
                    {
                        Thread.Sleep(10);
                        if (100 < startWatch.ElapsedMilliseconds) break;
                    }
                    if (sp.BytesToRead > 0 || receiveString.Length < 7) return;

                    log.Debug("Receive Data: " + receiveString);
                }
            }
            catch (Exception ex)
            {
                log.Debug("电流采集异常" + ex.Message);
                receiveString = UniversalFlags.errorStr + ex.Message;
                receiveFinish = true;
            }

            if (DeviceDataReceiveCompelete != null)
            {
                DeviceDataReceiveCompelete(this, receiveString);
                receiveFinish = true;
            }
        }

        public static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;
                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置
                return new byte[] { lo, hi };
            }
            return new byte[] { 0, 0 };
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
                        + "继电器闭合: " + exe.Example + Environment.NewLine
                        + "继电器断开: TRG,01 06 00 FF 00 00" + Environment.NewLine + "读电流开始: TRG,01 06 00 FA 00 FF" + Environment.NewLine
                        + "读电流: TRG,01 03 00 00 00 18" + Environment.NewLine + "报警下限: TRG,01 06 00 FC 00 28" + Environment.NewLine
                        + "报警上限: TRG,01 06 00 FB 00 46" + Environment.NewLine + "报警功能: TRG,01 06 00 FD 00 01" + Environment.NewLine;
                }
                result += Environment.NewLine + "-----------------------------------" + Environment.NewLine;
            }
            else if (Regex.IsMatch(cmd, "^TRG"))
            {
                string args = cmd.Replace("TRG,", "");
                receiveFinish = false;
                BeginTrigger(args);
                while(!receiveFinish)
                {
                    Thread.Sleep(50);
                }
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