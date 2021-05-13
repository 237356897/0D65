using desay.ProductData;
using log4net;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Toolkit.Helpers;
using System.ToolKit;
using System.Windows.Forms;

namespace CommonLibrary
{
    public partial class frmSerialPort : Form
    {
        private static ILog log = LogManager.GetLogger(typeof(frmSerialPort));

        public static Common.void_StringDelegate AppendLog;
        public Common.void_StringTriggerDelegate DataReceived;
        /// <summary>
        /// 是需要初始化的串口设备,True为可以初始化，否则不可以初始化
        /// </summary>
        public Common.void_BoolDelegate IsInitComDevice;
        public string RECData = "";

        private SerialPort serialPort1 = new SerialPort();
        private Dictionary<string, int> stopBits = new Dictionary<string, int>();
        private Dictionary<string, int> parityBits = new Dictionary<string, int>();
        private Serialport portDevice;
        private IAsyncResult triggerRes;
        private string comDeviceName = "SerialPort";
        private string deviceType;
        private bool isBtnRead = false;
        public frmSerialPort()
        {
            InitializeComponent();
        }

        public frmSerialPort(string deviceName)
            : this()
        {
            comDeviceName = deviceName;
        }

        private void frmSerialPort_Load(object sender, EventArgs e)
        {
            toolStripCmbxComPort.Items.AddRange(SerialPort.GetPortNames());

            for (int i = 0; i < comboBStopBit.Items.Count; ++i)
            { stopBits.Add((string)comboBStopBit.Items[i], i + 1); }

            for (int i = 0; i < comboBParityBit.Items.Count; ++i)
            { parityBits.Add((string)comboBParityBit.Items[i], i); }

            bool isExecute = true;
            while (isExecute)
            {
                isExecute = false;
                toolStripCmbxComPort.Text = Config.Instance.ScanCom;
                comboBBaudRate.Text = Config.Instance.BaudRate;
                comboBDataBit.Text = Config.Instance.DataBit;
                comboBStopBit.Text = Config.Instance.StopBit;
                comboBParityBit.Text = Config.Instance.ParityBit;
                txtTimeout.Text = Config.Instance.Timeout;
                txtRetries.Text = Config.Instance.Retries;
                txtCmd.Text = Config.Instance.Command;
                deviceType = Config.Instance.Type;
                switch (deviceType)
                {                    
                    case "DatalogicScan":
                        portDevice = new DatalogicBarScan(serialPort1);
                        //serialPort1.ReadTimeout = 500;
                        break;

                    default:
                        MessageBox.Show(string.Format("配置文件没有设置{0}里“Type”的正确参数,请前往设置后再点击确定按钮重新加载！", comDeviceName));
                        isExecute = true;
                        break;
                }
            }
            labDeviceModel.Text = string.Format("{0}-{1}", comDeviceName, deviceType);
            portDevice.TriggerStarting += new EventHandler<TriggerStartingEventArgs>(logDeviceTriggerReads);

            toolStripBtnOpen_Click(sender, e);
        }


        #region Public Method
        public bool IsSending = false;
        public IAsyncResult BeginTrigger(string cmd=null)
        {
            if (triggerRes == null || triggerRes.IsCompleted)
            {
                triggerRes = new Action(() => { trigger(cmd); }).BeginInvoke(null, null);
            }

            return triggerRes;
        }

        public void btnOpen_Click(object sender, EventArgs e)
        { toolStripBtnOpen_Click(sender, e); }

        #endregion

        #region Private Method
        private void appendText(string str)
        {
            AppendLog.Invoke(string.Format("<<{1}界面>>:: {0}", str, comDeviceName));
        }

        private void enabled(bool enable)
        {
            //toolStripBtnClose.Enabled =
            //btnReadDT.Enabled = !enable;

            //toolStripBtnOpen.Enabled =
            //toolStripCmbxComPort.Enabled =
            //toolStripBtnSave.Enabled =
            //comboBBaudRate.Enabled =
            //comboBDataBit.Enabled =
            //comboBStopBit.Enabled =
            //comboBParityBit.Enabled =
            //txtTimeout.Enabled =
            //txtRetries.Enabled =
            //txtCmd.Enabled = enable;
        }

        private void logDeviceTriggerReads(object sender, TriggerStartingEventArgs e)
        {
            log.DebugFormat("{0}第｛1｝次读取...", comDeviceName, e.TryCount);
        }

        private void setComParameter()
        {
            try
            {
                serialPort1.PortName = toolStripCmbxComPort.Text;
                serialPort1.BaudRate = int.Parse(comboBBaudRate.Text);
                serialPort1.DataBits = int.Parse(comboBDataBit.Text.Replace("bit", ""));
                serialPort1.StopBits = (StopBits)stopBits[comboBStopBit.Text];
                serialPort1.Parity = (Parity)parityBits[comboBParityBit.Text];
                serialPort1.ReadTimeout = int.Parse(txtTimeout.Text);

                portDevice.Retries = int.Parse(txtRetries.Text);
                portDevice.Cmd = txtCmd.Text;
                txtCmd.Text = portDevice.Cmd;
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("配置串口数据有误\r\n{0}",e.ToString()));
                appendText(string.Format("配置串口数据有误\r\n{0}", e.ToString()));
            }
        }

        private void trigger(string cmd=null)
        {
            log.DebugFormat("{0}读取数据。。。", comDeviceName);
            RECData = portDevice.Trigger(cmd).Trim();

            if (isBtnRead)
            {
                isBtnRead = false;
                appendText(string.Format("接收数据：{0}", RECData));
            }
            else
            {
                TriggerStartingEventArgs trg = new TriggerStartingEventArgs();
                trg.Command = cmd;
                DataReceived.Invoke(RECData, trg);
            }

            log.DebugFormat("{0}读取数据结束。。。", comDeviceName);
        }
        #endregion

        #region Form Operation
        private void toolStripBtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                { serialPort1.Close(); }

                setComParameter();
                serialPort1.Open();

                switch (deviceType)
                {
                    case "BT3561":
                    case "ST5520":
                        portDevice.InitDevice();
                        IsInitComDevice.BeginInvoke(false, null, null);
                        break;
                }
                appendText("串口打开成功");
                enabled(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}串口打开失败：{1}", comDeviceName, ex.ToString()));
                appendText(string.Format("{0}串口打开失败：{1}", comDeviceName, ex.ToString()));
                IsInitComDevice.BeginInvoke(true, null, null);
                enabled(true);
            }
        }

        private void toolStripBtnClose_Click(object sender, EventArgs e)
        {
            IsInitComDevice.BeginInvoke(true, null, null);
            serialPort1.Close();

            enabled(true);
        }

        private void toolStripBtnSave_Click(object sender, EventArgs e)
        {
            Config.Instance.CurrentCom = toolStripCmbxComPort.Text;
            Config.Instance.BaudRate = comboBBaudRate.Text;
            Config.Instance.DataBit = comboBDataBit.Text;
            Config.Instance.StopBit = comboBStopBit.Text;
            Config.Instance.ParityBit = comboBParityBit.Text;
            Config.Instance.Timeout = txtTimeout.Text;
            Config.Instance.Retries = txtRetries.Text;
            Config.Instance.Command = txtCmd.Text;
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);
            appendText("COM数据保存成功");
            // setComParameter();
        }

        private void btnReadDT_Click(object sender, EventArgs e)
        {
            isBtnRead = true;
            BeginTrigger(txtCmd.Text);
        }

        #endregion

        #region Key Press

        private void txtTimeout_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.IsDigitalInput(sender, e);
        }

        private void txtRetries_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.IsDigitalInput(sender, e);
        }

        #endregion
    }
}
