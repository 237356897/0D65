using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace CommonLibrary
{
    public abstract class Serialport
    {
        public event EventHandler<TriggerStartingEventArgs> TriggerStarting;
        public SerialPort Port;

        /// <summary>
        /// 获取或设置读取失败的重试次数
        /// </summary>
        public int Retries { get; set; }
        public string Cmd { get { return cmd; } set { cmd = value; } }

        /// <summary>
        /// 约束继承"触发一次读取数据",实现函数体
        /// </summary>
        /// <returns>读取接收的数据</returns>
        protected abstract string triggerReadOnce();
        protected abstract void initDevice();
        protected abstract string cmd { get; set; }

        /// <summary>
        /// 向系统串口提供串口对象
        /// </summary>
        /// <param name="serialPort">串口对象</param>
        public Serialport(SerialPort serialPort)
        {
            this.Port = serialPort;
        }

        public void InitDevice()
        {
            initDevice();
        }

        /// <summary>
        /// 触发读取实例对象的数据
        /// </summary>
        /// <returns>返回读取到的数据</returns>
        public string Trigger(string cmd=null)
        {
            string readData = "TimeOut";
            for (int i = 0; i <= Retries; i++)
            {
                eachReadTriggerCount(i + 1);
                try
                {
                    if (cmd != null)
                    { Cmd = cmd; }
                    readData = triggerReadOnce().Trim();
                    if (readData != "TimeOut" && readData != "" && !readData.Contains("ERROR"))
                    {
                        break;
                    }
                }
                catch
                { Thread.Sleep(10); }
            }
            return readData;

        }

        /// <summary>
        /// 每次的触发读取计数
        /// </summary>
        /// <param name="count">第count次触发读取</param>
        private void eachReadTriggerCount(int count)
        {
            if (TriggerStarting != null)
            {
                TriggerStarting(this, new TriggerStartingEventArgs { TryCount = count });
            }
        }

    }
}
