using CommonLibrary;
using desay.ProductData;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Toolkit.Helpers;
using System.ToolKit;
using System.Windows.Forms;


namespace P072G3A_FuncTest
{
    public partial class frmSystemSetting : Form
    {
        private string serialName;
        SerialPort _serialPort = new SerialPort();
        private string[] portArray;

        public bool isChangeLightTo940nm = false;
        public bool isChangeLightTo3100nm = false;
        public bool isOpenWhiteLightControl = false;
        public bool isCloseWhiteLightControl = false;

        public delegate void ReadVisibleCurrentAHandler();
        public ReadVisibleCurrentAHandler readVisibleCurrentAHandle = null;
        public delegate void ReadIRCurrentAHandler();
        public ReadIRCurrentAHandler readIRCurrentAHandle = null;

      
        public frmSystemSetting()
        {
            InitializeComponent();
            loadConfigParamForCurrentATest();          
        }

        private void frmSystemSetting_Load(object sender, EventArgs e)
        {
            loadTestItemConfigParam();
        }

        public void loadTestItemConfigParam()
        {
            #region 测试项
            BlackItemCheckBox.Checked = Position.Instance.testItem.bBlackEN;
            InfraredDarkCheckBox.Checked = Position.Instance.testItem.bInfraredDarkEN;
            BlemishCheckBox.Checked = Position.Instance.testItem.bBlemishEN;
            BadPixelCheckBox.Checked = Position.Instance.testItem.bBadPixelEN;
            WhiteBlaceCheckBox.Checked = Position.Instance.testItem.bWBEN;
            MTFCheckBox.Checked = Position.Instance.testItem.bMTFEN;
            IRMTFcheckBox.Checked = Position.Instance.testItem.bIRMTFEN;
            GrayCheckBox.Checked = Position.Instance.testItem.bGrayEN;
            ColorCheckBox.Checked = Position.Instance.testItem.bColorEN;
            FovCheckBox.Checked = Position.Instance.testItem.bFOVEN;
            FPSCheckBox.Checked = Position.Instance.testItem.bFPSEN;
            AlignmentCheckBox.Checked = Position.Instance.testItem.bAlignmentEN;
            DistortionCheckBox.Checked = Position.Instance.testItem.bDistortionEN;
            VoltageCheckBox.Checked = Position.Instance.testItem.bVoltageEN;
            CurrentCheckBox.Checked = Position.Instance.testItem.bCurrentEN;
            PowCheckBox.Checked = Position.Instance.testItem.bPowerEN;
            FWCheckBox.Checked = Position.Instance.testItem.bFWEN;
            AllTestCheckBox.Checked = Position.Instance.testItem.bAllTestEN;
            ShadingCheckBox.Checked = Position.Instance.testItem.bShadingEN;
            SNRCheckBox.Checked = Position.Instance.testItem.bSNREN;
            RotationCheckBox.Checked = Position.Instance.testItem.bRotationEN;
            HotPixelCheckBox.Checked = Position.Instance.testItem.bHotPixelEN;
            ChangeViewCheckBox.Checked = Position.Instance.testItem.bChangeViewEN;
           
            #endregion

            #region 参数设置
            VolotageMinTextBox.Text = Position.Instance.testItem.bVoltageMinValue.ToString();
            VolotageMaxTextBox.Text = Position.Instance.testItem.bVoltageMaxValue.ToString();
            CurrentMinTextBox.Text = Position.Instance.testItem.bCurrentMinValue.ToString();
            CurrentMaxTextBox.Text = Position.Instance.testItem.bCurrentMaxValue.ToString();
            PowerMinTextBox.Text = Position.Instance.testItem.bPowerMinValue.ToString();
            PowerMaxTextBox.Text = Position.Instance.testItem.bPowerMaxValue.ToString();
            FPSMinTextBox.Text = Position.Instance.testItem.bFPSMinValue.ToString();
            FPSMaxTextBox.Text = Position.Instance.testItem.bFPSMaxValue.ToString();
            FWTextBox.Text = Position.Instance.testItem.bFWValue.ToString();
            DelayTimeTextBox.Text = Config.Instance.bDelayTimeValue.ToString();
            #endregion

            #region 屏蔽项
            checkBoxNoSafetyDoor.Checked = !Config.Instance.IsSafetyDoor;
            checkBoxNoLeftWork.Checked = !Config.Instance.IsLeftWork;
            checkBoxNoRightWork.Checked = !Config.Instance.IsRightWork;
            checkBoxRunMode.Checked = Config.Instance.IsDryRunMode;
            checkBox_GuanShan.Checked = !Config.Instance.IsGuanShan;
            checkBox_ScanBarCode.Checked = !Config.Instance.IsScan;
            chkNGbox.Checked = !Config.Instance.IsNGBox;
            chkDarkTest.Checked = !Position.Instance.testItem.bDark_Test;
            chkLightTest.Checked = !Position.Instance.testItem.bLight_Test;
            chkMTFTest.Checked = !Position.Instance.testItem.bMTF_Test;
            #endregion

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 测试项
            Position.Instance.testItem.bBlackEN = BlackItemCheckBox.Checked;
            Position.Instance.testItem.bInfraredDarkEN = InfraredDarkCheckBox.Checked;
            Position.Instance.testItem.bBlemishEN = BlemishCheckBox.Checked;
            Position.Instance.testItem.bBadPixelEN = BadPixelCheckBox.Checked;
            Position.Instance.testItem.bWBEN = WhiteBlaceCheckBox.Checked;
            Position.Instance.testItem.bMTFEN = MTFCheckBox.Checked;
            Position.Instance.testItem.bIRMTFEN = IRMTFcheckBox.Checked;
            Position.Instance.testItem.bGrayEN = GrayCheckBox.Checked;
            Position.Instance.testItem.bColorEN = ColorCheckBox.Checked;
            Position.Instance.testItem.bFOVEN = FovCheckBox.Checked;
            Position.Instance.testItem.bFPSEN = FPSCheckBox.Checked;
            Position.Instance.testItem.bAlignmentEN = AlignmentCheckBox.Checked;
            Position.Instance.testItem.bDistortionEN = DistortionCheckBox.Checked;
            Position.Instance.testItem.bVoltageEN = VoltageCheckBox.Checked;
            Position.Instance.testItem.bCurrentEN = CurrentCheckBox.Checked;
            Position.Instance.testItem.bPowerEN = PowCheckBox.Checked;
            Position.Instance.testItem.bFWEN = FWCheckBox.Checked;
            Position.Instance.testItem.bAllTestEN = AllTestCheckBox.Checked;
            Position.Instance.testItem.bShadingEN = ShadingCheckBox.Checked;
            Position.Instance.testItem.bSNREN = SNRCheckBox.Checked;
            Position.Instance.testItem.bRotationEN = RotationCheckBox.Checked;
            Position.Instance.testItem.bHotPixelEN = HotPixelCheckBox.Checked;
            Position.Instance.testItem.bChangeViewEN = ChangeViewCheckBox.Checked;
            #endregion

            #region 参数设置
            Position.Instance.testItem.bVoltageMinValue = Convert.ToSingle(VolotageMinTextBox.Text);
            Position.Instance.testItem.bVoltageMaxValue = Convert.ToSingle(VolotageMaxTextBox.Text);
            Position.Instance.testItem.bCurrentMinValue = Convert.ToSingle(CurrentMinTextBox.Text);
            Position.Instance.testItem.bCurrentMaxValue = Convert.ToSingle(CurrentMaxTextBox.Text);
            Position.Instance.testItem.bPowerMinValue = Convert.ToSingle(PowerMinTextBox.Text);
            Position.Instance.testItem.bPowerMaxValue = Convert.ToSingle(PowerMaxTextBox.Text);
            Position.Instance.testItem.bFPSMinValue = Convert.ToSingle(FPSMinTextBox.Text);
            Position.Instance.testItem.bFPSMaxValue = Convert.ToSingle(FPSMaxTextBox.Text);
            Position.Instance.testItem.bFWValue = Convert.ToSingle(FWTextBox.Text);
            Config.Instance.bDelayTimeValue = (int)Convert.ToSingle(DelayTimeTextBox.Text);
            #endregion

            #region 屏蔽项
            Config.Instance.IsSafetyDoor = !checkBoxNoSafetyDoor.Checked;
            Config.Instance.IsLeftWork = !checkBoxNoLeftWork.Checked;
            Config.Instance.IsRightWork = !checkBoxNoRightWork.Checked;
            Config.Instance.IsDryRunMode = checkBoxRunMode.Checked;
            Config.Instance.IsGuanShan = !checkBox_GuanShan.Checked;
            Config.Instance.IsScan = !checkBox_ScanBarCode.Checked;
            Config.Instance.IsNGBox = !chkNGbox.Checked;
            Position.Instance.testItem.bDark_Test = !chkDarkTest.Checked;
            Position.Instance.testItem.bLight_Test = !chkLightTest.Checked;
            Position.Instance.testItem.bMTF_Test = !chkMTFTest.Checked;
            #endregion

            SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath,Config.Instance);
            SerializerManager<Position>.Instance.Save(Marking.PositionPath,Position.Instance);
            MessageBox.Show("数据保存成功");
        }


        #region 注释代码
        private void loadConfigParamForCurrentATest()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"。ini\Config.ini";
            serialName = IniFile.ReadValue("CurrentA", "Com", filePath, "COM9");
            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            portArray = str;
            if (str == null)
            {
                // MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                //System.Diagnostics.Debug.WriteLine(s);
                //serialPortComBox.Items.Add(s);
            }
            int start = 0;
            for (int i = 0; i < str.Count(); i++)
            {
                if (str[i] == serialName)
                {
                    start = i;
                }
            }
            //  serialPortComBox.SelectedIndex = start;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen) return;
            if (readVisibleCurrentAHandle != null)
            {
                readCurrentA_Click(null, null);
            }
            if (readIRCurrentAHandle != null)
            {
                readIRCurrent();
            }
        }

        private void readCurrentA_Click(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                MessageBox.Show("连接失败，请检查");
                return;
            }

            Byte[] TxData = { 0x02, 0x03, 0x00, 0x08, 0x00, 0x04 }; //定义通讯指令 ,0x01设备地址为4，0x03功能码为3， 0x00, 0x03是通讯地址吗，0x00, 0x01是寄存器数量，0x74, 0x5f是CRC校验
            byte[] tempCRC = CRC16Test(TxData);


            Byte[] endData = { TxData[0], TxData[1], TxData[2], TxData[3], TxData[4], TxData[5], tempCRC[0], tempCRC[1] };


            _serialPort.Write(endData, 0, 8);//发送指令
            Thread.Sleep(100);

            byte[] recData = new byte[13];
            _serialPort.Read(recData, 0, 13);//接收数据

            appendWriteOrReceiveData(endData);
            appendWriteOrReceiveData(recData);

            string path = "D:\\CurrentList.ini";

            byte[] EndData1 = new byte[2];
            Array.Copy(recData, 3, EndData1, 0, 2);
            string str1 = ByteArrayToHexString(EndData1);


            IniFile.WriteValue("Current", "dynamic_Chanel_1", getCurrentAValueWithStr(str1), path);

            byte[] EndData2 = new byte[2];
            Array.Copy(recData, 5, EndData2, 0, 2);
            string str2 = ByteArrayToHexString(EndData2);
            IniFile.WriteValue("Current", "dynamic_Chanel_2", getCurrentAValueWithStr(str2), path);

            byte[] EndData3 = new byte[2];
            Array.Copy(recData, 7, EndData3, 0, 2);
            string str3 = ByteArrayToHexString(EndData3);
            IniFile.WriteValue("Current", "dynamic_Chanel_3", getCurrentAValueWithStr(str3), path);

            byte[] EndData4 = new byte[2];
            Array.Copy(recData, 9, EndData4, 0, 2);
            string str4 = ByteArrayToHexString(EndData4);
            IniFile.WriteValue("Current", "dynamic_Chanel_4", getCurrentAValueWithStr(str4), path);

            string endStr = getCurrentAValueWithStr(str1) + "|||" + getCurrentAValueWithStr(str2) + "|||" + getCurrentAValueWithStr(str3) + "|||" + getCurrentAValueWithStr(str4) + "|||";

        }

        private void readIRCurrent()
        {
            if (!_serialPort.IsOpen)
            {
                MessageBox.Show("连接失败，请检查");
                return;
            }

            Byte[] TxData = { 0x02, 0x03, 0x00, 0x08, 0x00, 0x04 }; //定义通讯指令 ,0x01设备地址为4，0x03功能码为3， 0x00, 0x03是通讯地址吗，0x00, 0x01是寄存器数量，0x74, 0x5f是CRC校验
            byte[] tempCRC = CRC16Test(TxData);


            Byte[] endData = { TxData[0], TxData[1], TxData[2], TxData[3], TxData[4], TxData[5], tempCRC[0], tempCRC[1] };


            _serialPort.Write(endData, 0, 8);//发送指令
            Thread.Sleep(100);

            byte[] recData = new byte[13];
            _serialPort.Read(recData, 0, 13);//接收数据

            appendWriteOrReceiveData(endData);
            appendWriteOrReceiveData(recData);

            string path = "D:\\CurrentListIR.ini";

            byte[] EndData1 = new byte[2];
            Array.Copy(recData, 3, EndData1, 0, 2);
            string str1 = ByteArrayToHexString(EndData1);


            IniFile.WriteValue("Current", "dynamic_Chanel_1", getCurrentAValueWithStr(str1), path);

            byte[] EndData2 = new byte[2];
            Array.Copy(recData, 5, EndData2, 0, 2);
            string str2 = ByteArrayToHexString(EndData2);
            IniFile.WriteValue("Current", "dynamic_Chanel_2", getCurrentAValueWithStr(str2), path);

            byte[] EndData3 = new byte[2];
            Array.Copy(recData, 7, EndData3, 0, 2);
            string str3 = ByteArrayToHexString(EndData3);
            IniFile.WriteValue("Current", "dynamic_Chanel_3", getCurrentAValueWithStr(str3), path);

            byte[] EndData4 = new byte[2];
            Array.Copy(recData, 9, EndData4, 0, 2);
            string str4 = ByteArrayToHexString(EndData4);
            IniFile.WriteValue("Current", "dynamic_Chanel_4", getCurrentAValueWithStr(str4), path);

            string endStr = getCurrentAValueWithStr(str1) + "|||" + getCurrentAValueWithStr(str2) + "|||" + getCurrentAValueWithStr(str3) + "|||" + getCurrentAValueWithStr(str4) + "|||";
        }

        public static byte[] CRC16Test(byte[] data)
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

        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }

        public string getCurrentAValueWithStr(string str)
        {
            int dataNum = System.Convert.ToInt32(str, 16);
            float temp = (float)dataNum / 50000;
            Double currentA = (Double)temp * 0.2 * 1000;
            currentA = Math.Round(currentA, 2);
            //MessageBox.Show(currentA.ToString());
            return currentA.ToString();
        }

        public void appendWriteOrReceiveData(byte[] data)
        {
            //receivedBox.ForeColor = Color.Red;
            //string hexString = string.Empty;

            //if (data != null)
            //{
            //    StringBuilder strB = new StringBuilder();
            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        strB.Append(data[i].ToString("X2"));
            //        strB.Append("-");
            //    }
            //    hexString = strB.ToString();
            //}
            //receivedBox.AppendText(DateTime.Now.ToString("HH:mm:ss ") + hexString + "\r\n");
            //// AddLgoToTXT(DateTime.Now.ToString("HH:mm:ss ") + hexString + "\r\n");
            //receivedBox.ForeColor = Color.Black;
        }

        #endregion

        #region 光源切换

        private void setToVisibleLight_Click(object sender, EventArgs e)
        {

            Button but = (Button)sender;
            but.Enabled = false;
            isChangeLightTo3100nm = true;
            isChangeLightTo940nm = false;
            Thread.Sleep(2000);
            //isChangeLightTo3100nm = false;
            but.Enabled = true;
        }

        private void setTo940nm_Click(object sender, EventArgs e)
        {

            Button but = (Button)sender;
            but.Enabled = false;
            isChangeLightTo3100nm = false;
            isChangeLightTo940nm = true;
            Thread.Sleep(2000);
            //isChangeLightTo940nm = false;
            but.Enabled = true;
        }

        private void openWhiteLightControl_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            but.Enabled = false;
            isOpenWhiteLightControl = true;
            isCloseWhiteLightControl = false;
            Thread.Sleep(1000);
            but.Enabled = true;
        }

        private void closeWhiteLightControl_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            but.Enabled = false;
            isOpenWhiteLightControl = false;
            isCloseWhiteLightControl = true;
            Thread.Sleep(1000);
            but.Enabled = true;
        }

        #endregion

    }
}
