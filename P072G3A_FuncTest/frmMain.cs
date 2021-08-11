
using CommonLibrary;
using desay.ProductData;
using Image_Sitenamespace;
using log4net;
using Motion.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Toolkit.Helpers;
using System.ToolKit;
using System.Windows.Forms;

namespace P072G3A_FuncTest
{

    public partial class frmMain : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);


        private ILog log = LogManager.GetLogger(typeof(frmMain));
        public static frmMain main;
        public static frmHome frmhome = new frmHome();
        public frmSystemSetting frmSystem = new frmSystemSetting();
        public frmSerialPort frmScan = new frmSerialPort("扫码器");
        public frmIOCard frmIO = new frmIOCard();
        public frmLogin frmlogin = new frmLogin();
        public frmMotionCard frmMotion = new frmMotionCard();
        public frmMotionDebug frmDebug = new frmMotionDebug();
        public frmProductList frmProduct = new frmProductList();
        public FrmMES frmMES = new FrmMES();
        Stopwatch sw = new Stopwatch();
        ComPort ComPort1, ComPort2, ComPort3, ComPort4, ComPort5, ComPort6, ComPort7;
        List<DateTime> periodStartTime = new List<DateTime>(2);

        #region 机构个体对象
        Unit axisZ;
        Unit axisY1;
        Unit axisY2;
        Unit cyLeftBlackUpDown;
        Unit cyRightBlackUpDown;
        Unit cyWhiteUpDown;
        Unit cyLeftSidesway;
        Unit cyRightSidesway;
        Unit pressureTotal;
        Unit safetyDoor;
        #endregion

        #region 信号标志位

        /// <summary>
        /// 白场工作标志位
        /// </summary>
        private bool IsWhiteWorking = false;
        /// <summary>
        /// 图卡工作标志位
        /// </summary>
        private bool IsMTFWorking = false;
        /// <summary>
        /// 设备报警标志位
        /// </summary>
        private bool isAlarm = false;
        /// <summary>
        /// 急停键急停标志位
        /// </summary>
        private bool isEmgStoping = false;
        /// <summary>
        /// 光栅急停标志位
        /// </summary>
        private bool isRasterStoping = false;
        /// <summary>
        /// 设备已经复位标志位
        /// </summary>
        private bool HaveReseted = false;
        /// <summary>
        /// 设备暂停中
        /// </summary>
        private bool isPausing = false;
        /// <summary>
        /// 轮到左工位
        /// </summary>
        private bool isLeftRun = true;
        /// <summary>
        /// 已丢弃NG品
        /// </summary>
        private bool HaveDropNG = true;
        /// <summary>
        /// 已切换机种
        /// </summary>
        private bool HaveChangeProduct = false;
        /// <summary>
        /// 左工位工作中
        /// </summary>
        private bool leftWorkingFlag = false;
        /// <summary>
        /// 右工位工作中
        /// </summary>
        private bool rightWorkingFlag = false;

        private bool online = false;
        /// <summary>
        /// 左工位扫码标志位
        /// </summary>
        private bool isLeftScan = false;
        /// <summary>
        /// 右工位扫码标志位
        /// </summary>
        private bool isRightScan = false;
        private int leftTestProcess = -1;
        private int rightTestProcess = -1;
        private int resetingProcess = 0;
        /// <summary>
        /// 左工位条码
        /// </summary>
        private string leftBarcode;
        /// <summary>
        /// 右工位条码
        /// </summary>
        private string rightBarcode;

        private MachineStatus.Status currentMachineStatus = MachineStatus.Status.停止;
        private IAsyncResult scanIAR = null;
        private string scanBarDis { get { return frmhome.ProductBarcode; } set { frmhome.ProductBarcode = value; } }
        /// <summary>
        /// 关闭可见光
        /// </summary>
        public bool isCloseVisibleLight;
        /// <summary>
        /// 红外和可见光
        /// </summary>
        public bool isChangeLightSuccess;
        /// <summary>
        /// 电流
        /// </summary>
        public bool isReadCurrentSuccess;
        /// <summary>
        /// 白场光源
        /// </summary>
        public bool isChangeWhiteControlSuccess;

        private bool bLeftResult = true;
        private bool bRightResult = true;
        private bool bLeftTotalResult = true;
        private bool bRightTotalResult = true;
        private string leftSZErrorCode = "PASS";
        private string rightSZErrorCode = "PASS";

        private double bLeftCurentValue = 0;
        private double bRightCurentValue = 0;
        private double bLeftVoltageValue = 0;
        private double bRightVoltageValue = 0;
        #endregion 

        #region 窗体加载

        public frmMain()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            main = this;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            leftTestStatusLabel.BackColor = Color.Gray;
            rightTestStatusLabel.BackColor = Color.Gray;
            leftBarCodeLabel.Text = "";
            rightBarCodeLabel.Text = "";
            isCloseVisibleLight = false;


            tbtnStart.Enabled = false; //初始时禁用启动按键
            tbtnStop.Enabled = false;  //初始时禁用停止按键
            tbtnPause.Enabled = false; //初始时禁用暂停按键

            #region 加载子窗体
            appendText("加载程序子窗体...");
            #region 窗体对象
            frmHome.AppendLog += AppendText;

            frmLogin.AppendLog += AppendText;
            frmLogin.UpdateOperator += updateOperator;

            frmProductList.AppendLog += AppendText;
            frmProductList.UpdateProduct += updateProduct;
            frmProductList.NewAddProduct += NewAddProduct;
            frmProductList.DeleteProduct += DeleteProduct;

            frmSerialPort.AppendLog += AppendText;
            frmScan.DataReceived += frmScanBarcode;

            frmIOCard.AppendLog += AppendText;

            frmMotionCard.AppendLog += AppendText;
            #endregion

            #region 关联选型卡
            loadForm();
            tabMain.SelectedTab = tabPUsers;
            #endregion

            #endregion

            #region 加载三色灯线程
            appendText("加载信号灯资源...");
            MachineStatus.LoadConfig(new IoPoint[]
            { IoPoints.TDO0, IoPoints.TDO1, IoPoints.TDO2, IoPoints.TDO3 });
            #endregion

            #region 加载模组
            appendText("加载模组资源...");
            #region Z轴
            frmMotion.AxisZ = axisZ = new Unit(
                () => { return true; },
                () =>
                {
                    if (frmMotion.Z.IsAlarm)
                    { IoPoints.TDO7.Value = false; }

                    return frmMotion.Z.IsAlarm;
                }, () => { return 0; }, () => { return "Z轴异常报警！"; });
            #endregion

            #region Y1轴
            frmMotion.AxisY1 = axisY1 = new Unit(
                () =>
                {
                    bool b = false;

                    bool b1 = IoPoints.TDI8.Value;  //左黑场气缸上感应OK
                    bool b2 = IoPoints.TDI10.Value; //右黑场气缸上感应OK

                    bool b3 = IoPoints.TDI14.Value; //左通道平移气缸缩回感应OK                    
                    bool b4 = IoPoints.IDI0.Value;  //右通道平移气缸缩回感应OK  

                    bool b5 = IoPoints.TDI12.Value; //白场气缸上感应OK
                    bool b6 = IoPoints.IDI6.Value;  //左盖板检测

                    if (b1 && b3 && b5 && b6)
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }
                    return b;
                },
                () => { return Config.Instance.IsLeftWork && frmMotion.Y1.IsAlarm; },
                () => { return 0; },
                () => { return "Y1轴异常报警！"; });
            #endregion

            #region Y2轴
            frmMotion.AxisY2 = axisY2 = new Unit(
                () =>
                {
                    bool b = false;

                    bool b1 = IoPoints.TDI8.Value;//左黑场气缸上感应OK
                    bool b2 = IoPoints.TDI10.Value;//右黑场气缸上感应OK

                    bool b3 = IoPoints.TDI14.Value;//左通道平移气缸缩回感应OK                  
                    bool b4 = IoPoints.IDI0.Value; //右通道平移气缸缩回感应OK

                    bool b5 = IoPoints.TDI12.Value;//白场气缸上感应OK
                    bool b6 = IoPoints.IDI8.Value;//右盖板检测

                    if (b2 && b4 && b5 && b6)
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }
                    return b;
                },
                () => { return Config.Instance.IsRightWork && frmMotion.Y2.IsAlarm; },
                () => { return 0; },
                () => { return "Y2轴异常报警！"; });
            #endregion

            #region 左黑场升降气缸
            int cyLeftBlackUpDown_alarmDelay = 0;
            string cyLeftBlackUpDown_alarmMsg = "";
            frmIO.CyLeftBlackUpDown = cyLeftBlackUpDown = new Unit(
                () => { return frmMotion.Y1.IsMotionless; },//Y1轴是否已停止
                () =>
                {
                    bool b = false;
                    if (Config.Instance.IsLeftWork)
                    {
                        bool b1 = IoPoints.TDO9.Value;
                        bool b2 = IoPoints.TDI9.Value;//左黑场气缸下感应OK
                        bool b3 = IoPoints.TDI8.Value;//左黑场气缸上感应OK
                        bool b4 = IoPoints.TDO8.Value;
                        if (b4 && !b3)
                        {
                            b = true;
                            cyLeftBlackUpDown_alarmDelay = frmIO.LeftBlackCYUpDelay;
                            cyLeftBlackUpDown_alarmMsg = "左黑场升降气缸上动作延时后未到位报警！";
                        }
                        if (b1 && !b2)
                        {
                            b = true;
                            cyLeftBlackUpDown_alarmDelay = frmIO.LeftBlackCYDownDelay;
                            cyLeftBlackUpDown_alarmMsg = "左黑场升降气缸下动作延时后未到位报警！";
                        }
                    }
                    return b;
                }, () => { return cyLeftBlackUpDown_alarmDelay; }, () => { return cyLeftBlackUpDown_alarmMsg; });
            #endregion

            #region 右黑场升降气缸
            int cyRightBlackUpDown_alarmDelay = 0;
            string cyRightBlackUpDown_alarmMsg = "";
            frmIO.CyRightBlackUpDown = cyRightBlackUpDown = new Unit(
                () => { return frmMotion.Y2.IsMotionless; },//Y2轴是否已停止
                () =>
                {
                    bool b = false;
                    if (Config.Instance.IsRightWork)
                    {
                        bool b4 = IoPoints.TDO10.Value;//右通道黑场升降气缸下
                        bool b3 = IoPoints.TDI11.Value;//右黑场气缸下感应OK                                            
                        bool b2 = IoPoints.TDI10.Value;//右黑场气缸上感应OK
                        bool b1 = IoPoints.TDO11.Value;//右通道黑场升降气缸上

                        if (b4 && !b3)
                        {
                            b = true;
                            cyRightBlackUpDown_alarmDelay = frmIO.RightBlackCYDownDelay;
                            cyRightBlackUpDown_alarmMsg = "右黑场升降气缸下动作延时后未到位报警！";
                        }
                        else if (b1 && !b2)
                        {
                            b = true;
                            cyRightBlackUpDown_alarmDelay = frmIO.RightBlackCYUpDelay;
                            cyRightBlackUpDown_alarmMsg = "右黑场升降气缸上动作延时后未到位报警！";
                        }
                    }
                    return b;
                }, () => { return cyRightBlackUpDown_alarmDelay; }, () => { return cyRightBlackUpDown_alarmMsg; });
            #endregion

            #region 白场升降气缸
            int cyWhiteUpDown_alarmDelay = 0;
            string cyWhiteUpDown_alarmMsg = "";
            frmIO.CyWhiteUpDown = cyWhiteUpDown = new Unit(
                () =>
                {
                    bool b = true;
                    if (!frmMotion.Y1.IsMotionless || !frmMotion.Y2.IsMotionless)//Y1 Y2轴正在运动中
                    {
                        b = false;
                    }
                    return b;
                },
                () =>
                {
                    bool b = false;

                    bool b1 = IoPoints.TDO12.Value;//下
                    bool b2 = IoPoints.TDI13.Value;//白场升降气缸下感应
                    bool b3 = IoPoints.TDO13.Value;//上
                    bool b4 = IoPoints.TDI12.Value;//白场升降气缸上感应
                    if (b3 && !b4)
                    {
                        b = true;
                        cyWhiteUpDown_alarmDelay = frmIO.WhiteCYDownDelay;
                        cyWhiteUpDown_alarmMsg = "白场升降气缸下动作延时后未到位报警！";
                    }
                    else if (b1 && !b2)
                    {
                        b = true;
                        cyWhiteUpDown_alarmDelay = frmIO.WhiteCYUpDelay;
                        cyWhiteUpDown_alarmMsg = "白场升降气缸上动作延时后未到位报警！";
                    }
                    return b;
                }, () => { return cyWhiteUpDown_alarmDelay; }, () => { return cyWhiteUpDown_alarmMsg; });
            #endregion

            #region 左平移气缸
            int cyLeftSidesway_alarmDelay = 0;
            string cyLeftSidesway_alarmMsg = "";
            frmIO.CyLeftSidesway = cyLeftSidesway = new Unit(
                () =>
                {
                    bool b = true;
                    bool temp = IoPoints.IDI2.Value || !IoPoints.IDI0.Value;//气缸伸出感应
                    bool temp2 = frmMotion.Y1.IsPosComplete(frmMotion.LeftGraphicPos) && frmMotion.Y2.IsPosComplete(frmMotion.RightGraphicPos);
                    bool temp3 = frmMotion.Y1.IsPosComplete(frmMotion.LeftWhitePos) && frmMotion.Y2.IsPosComplete(frmMotion.RightWhitePos);
                    if (temp && (temp2 || temp3))
                    {
                        b = false;
                    }
                    return b;
                },
                () =>
                {
                    bool b = false;
                    if (Config.Instance.IsLeftWork)
                    {
                        bool b1 = IoPoints.TDI14.Value;//左通道平移气缸缩回感应
                        bool b2 = IoPoints.TDI15.Value;//左通道平移气缸伸出感应                       
                        bool b3 = IoPoints.IDO0.Value;//缩回
                        bool b4 = IoPoints.IDO1.Value;//伸出

                        if (b4 && !b2)
                        {
                            b = true;
                            cyLeftSidesway_alarmDelay = frmIO.LeftSideswayCYReachDelay;
                            cyLeftSidesway_alarmMsg = "左平移气缸伸出动作延时后未到位报警！";
                        }
                        else if (b3 && !b1)
                        {
                            b = true;
                            cyLeftSidesway_alarmDelay = frmIO.LeftSideswayCYReturnDelay;
                            cyLeftSidesway_alarmMsg = "左平移气缸缩回动作延时后未到位报警！";
                        }

                    }
                    return b;
                }, () => { return cyLeftSidesway_alarmDelay; }, () => { return cyLeftSidesway_alarmMsg; });
            #endregion

            #region 右平移气缸
            int cyRightSidesway_alarmDelay = 0;
            string cyRightSidesway_alarmMsg = "";
            frmIO.CyRightSidesway = cyRightSidesway = new Unit(
                () =>
                {
                    bool b = true;
                    bool temp = IoPoints.TDI15.Value || !IoPoints.TDI14.Value;//左通道平移气缸伸出感应
                    bool temp2 = frmMotion.Y1.IsPosComplete(frmMotion.LeftGraphicPos) && frmMotion.Y2.IsPosComplete(frmMotion.RightGraphicPos);
                    bool temp3 = frmMotion.Y1.IsPosComplete(frmMotion.LeftWhitePos) && frmMotion.Y2.IsPosComplete(frmMotion.RightWhitePos);
                    if (temp && (temp2 || temp3))
                    {
                        b = false;
                    }
                    return b;
                },
                () =>
                {
                    bool b = false;
                    if (Config.Instance.IsRightWork)
                    {
                        bool b1 = IoPoints.IDO3.Value;//缩回
                        bool b2 = IoPoints.IDO2.Value;//伸出
                        bool b3 = IoPoints.IDI0.Value;//气缸缩回感应
                        bool b4 = IoPoints.IDI2.Value;//气缸伸出感应

                        if (b2 && !b4)
                        {
                            b = true;
                            cyRightSidesway_alarmDelay = frmIO.RightSideswayCYReachDelay;
                            cyRightSidesway_alarmMsg = "右平移气缸伸出动作延时后未到位报警！";
                        }
                        else if (b1 && !b3)
                        {
                            b = true;
                            cyRightSidesway_alarmDelay = frmIO.RightSideswayCYReturnDelay;
                            cyRightSidesway_alarmMsg = "右平移气缸缩回动作延时后未到位报警！";
                        }
                    }
                    return b;
                }, () => { return cyRightSidesway_alarmDelay; }, () => { return cyRightSidesway_alarmMsg; });
            #endregion

            #region 总气压表
            pressureTotal = new Unit(
                () => { return true; },
                () => { return IoPoints.IDI4.Value; },
                () => { return 0; },
                () => { return "设备总气压异常报警！"; });
            #endregion

            #region 安全门
            safetyDoor = new Unit(
                () => { return true; },
                () => { return Config.Instance.IsSafetyDoor && online && !IoPoints.TDI4.Value; }, //安全门
                () => { return 100; },
                () => { return "危险--设备安全门已打开！"; });
            #endregion

            #endregion

            #region 加载报警线程
            appendText("加载报警线程...");
            Unit.Alarm = alarm;
            Unit.Run();
            #endregion

            #region 加载主线程
            appendText("加载主线程...");
            Thread mainWorkThread = new Thread(new ThreadStart(mainWork));
            mainWorkThread.IsBackground = true;
            mainWorkThread.Start();
            #endregion

            #region 通讯初始化
            appendText("通讯初始化...");
            #region 光源控制器

            //ComPort1 = new ComPort()
            //{
            //    Name = "光源控制1"
            //};
            //try
            //{

            //    ComPort1.SetConnectionParam(Global.Instance.ComPort1ComParam);
            //    ComPort1.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithComPort1ReceiveData);
            //    ComPort1.DeviceOpen();
            //}
            //catch (Exception ex)
            //{
            //    AppendText(string.Format("{0}连接失败：{1}", ComPort1.Name, ex.Message));
            //}

            //ComPort2 = new ComPort()
            //{
            //    Name = "光源控制2"
            //};
            //try
            //{
            //    ComPort2.SetConnectionParam(Global.Instance.ComPort2ComParam);
            //    ComPort2.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithComPort2ReceiveData);
            //    ComPort2.DeviceOpen();
            //}
            //catch (Exception ex)
            //{
            //    AppendText(string.Format("{0}连接失败：{1}", ComPort2.Name, ex.Message));
            //}

            //ComPort3 = new ComPort()
            //{
            //    Name = "光源控制3"
            //};
            //try
            //{
            //    ComPort3.SetConnectionParam(Global.Instance.ComPort3ComParam);
            //    ComPort3.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithComPort3ReceiveData);
            //    ComPort3.DeviceOpen();
            //}
            //catch (Exception ex)
            //{
            //    AppendText(string.Format("{0}连接失败：{1}", ComPort3.Name, ex.Message));
            //}

            //ComPort4 = new ComPort()
            //{
            //    Name = "光源控制4"
            //};
            //try
            //{
            //    ComPort4.SetConnectionParam(Global.Instance.ComPort4ComParam);
            //    ComPort4.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithComPort4ReceiveData);
            //    ComPort4.DeviceOpen();
            //}
            //catch (Exception ex)
            //{
            //    AppendText(string.Format("{0}连接失败：{1}", ComPort4.Name, ex.Message));
            //}

            //ComPort5 = new ComPort()
            //{
            //    Name = "光源控制5"
            //};
            //try
            //{
            //    ComPort5.SetConnectionParam(Global.Instance.ComPort5ComParam);
            //    ComPort5.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithComPort5ReceiveData);
            //    ComPort5.DeviceOpen();
            //}
            //catch (Exception ex)
            //{
            //    AppendText(string.Format("{0}连接失败：{1}", ComPort5.Name, ex.Message));
            //}

            //ComPort6 = new ComPort()
            //{
            //    Name = "白场光源控制6"
            //};
            //try
            //{
            //    ComPort6.SetConnectionParam(Global.Instance.ComPort6ComParam);
            //    ComPort6.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithComPort6ReceiveData);
            //    ComPort6.DeviceOpen();
            //}
            //catch (Exception ex)
            //{
            //    AppendText(string.Format("{0}连接失败：{1}", ComPort6.Name, ex.Message));
            //}

            #endregion

            #region 电压电流

            ComPort7 = new ComPort()
            {
                Name = "电压电流读取"
            };
            try
            {
                ComPort7.SetConnectionParam(Global.Instance.ComPort7ComParam);
                ComPort7.DeviceDataReceiveCompelete += new CommonLibrary.DataReceiveCompleteEventHandler(DealWithComPort7ReceiveData);
                ComPort7.DeviceOpen();
                appendText("电流电压采集串口打开成功");
            }
            catch (Exception ex)
            {
                AppendText(string.Format("{0}连接失败：{1}", ComPort7.Name, ex.Message));
            }

            #endregion
            appendText("数据加载完成！");
            #endregion

        }

        #region 通讯事件

        private void DealWithComPort1ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.LightSource1Data = result.Split('-');

                //AppendText("光源1获取数据成功：" + result);
            }
            catch (Exception ex)
            {
                AppendText("光源1获取数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isLightSource1Completed = true;
            }
        }

        private void DealWithComPort2ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.LightSource2Data = result.Split('-');

                //AppendText("光源2获取数据成功：" + result);
            }
            catch (Exception ex)
            {
                AppendText("光源2获取数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isLightSource2Completed = true;
            }
        }

        private void DealWithComPort3ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.LightSource3Data = result.Split('-');

                //AppendText("光源3获取数据成功：" + result);
            }
            catch (Exception ex)
            {
                AppendText("光源3获取数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isLightSource3Completed = true;
            }
        }

        private void DealWithComPort4ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.LightSource4Data = result.Split('-');

                //AppendText("光源4获取数据成功：" + result);
            }
            catch (Exception ex)
            {
                AppendText("光源4获取数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isLightSource4Completed = true;
            }
        }

        private void DealWithComPort5ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.LightSource5Data = result.Split('-');

                // AppendText("光源5获取数据成功：" + result);
            }
            catch (Exception ex)
            {
                AppendText("光源5获取数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isLightSource5Completed = true;
            }
        }

        private void DealWithComPort6ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.LightSource6Data = result.Split('-');

                //AppendText("光源6获取数据成功：" + result);
            }
            catch (Exception ex)
            {
                AppendText("光源6数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isLightSource6Completed = true;
            }
        }

        private void DealWithComPort7ReceiveData(object sender, string result)
        {
            try
            {
                if (result.Contains(UniversalFlags.errorStr))
                    throw new Exception(result);

                Marking.currentSource7Data = result.Split('-');
                if (Marking.currentSource7Data.Count() == 13)
                {
                    byte[] receivedData = new byte[13];
                    for (int i = 0; i < 13; i++)
                    {
                        receivedData[i] = Convert.ToByte(Marking.currentSource7Data[i], 16);
                    }
                    if (receivedData[0] == 1)
                    {
                        appendText("电压读取成功");
                        byte[] EndData1 = new byte[2];
                        Array.Copy(receivedData, 7, EndData1, 0, 2);
                        string str1 = ByteArrayToHexString(EndData1);
                        if (leftTestProcess > 0)
                        {
                            bLeftVoltageValue = getVoltageValueWithStr(str1);
                            appendText("左边电压为：" + bLeftVoltageValue.ToString());
                        }


                        byte[] EndData2 = new byte[2];
                        Array.Copy(receivedData, 9, EndData2, 0, 2);
                        string str2 = ByteArrayToHexString(EndData2);
                        if (rightTestProcess > 0)
                        {
                            bRightVoltageValue = getVoltageValueWithStr(str2);
                            appendText("右边电压为：" + bRightVoltageValue.ToString());
                        }


                    }
                    if (receivedData[0] == 2)
                    {
                        appendText("电流读取成功");
                        byte[] EndData1 = new byte[2];
                        Array.Copy(receivedData, 5, EndData1, 0, 2);
                        string str1 = ByteArrayToHexString(EndData1);
                        if (leftTestProcess > 0)
                        {
                            bLeftCurentValue = getCurrentAValueWithStr(str1);
                            appendText("左边电流为：" + bLeftCurentValue.ToString());
                        }
                        //bLeftCurentValue = getCurrentAValueWithStr(str1);
                        //appendText("左边电流为：" + bLeftCurentValue.ToString());
                        byte[] EndData2 = new byte[2];
                        Array.Copy(receivedData, 9, EndData2, 0, 2);
                        string str2 = ByteArrayToHexString(EndData2);
                        if (rightTestProcess > 0)
                        {
                            bRightCurentValue = getCurrentAValueWithStr(str2);
                            appendText("右边电流为：" + bRightCurentValue.ToString());
                        }
                        //bRightCurentValue = getCurrentAValueWithStr(str2);
                        //appendText("右边电流为：" + bRightCurentValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                AppendText("电流7数据失败：" + ex.Message);
            }
            finally
            {
                Marking.isCurrentSource7Completed = true;

            }
        }

        #endregion

        private void loadForm()
        {
            loadTabPForm(frmhome, tabPMain);
            loadTabPForm(frmMotion, tabPMotionCard);
            loadTabPForm(frmIO, tabPIOCard);
            loadTabPForm(frmlogin, tabPUsers);
            loadTabPForm(frmProduct, tabPProductList);
            loadTabPForm(frmScan, tabPScanBarcode);
            loadTabPForm(frmSystem, tabPSystemSetting);
            loadTabPForm(frmMES, tabPMES);
        }

        private void loadTabPForm(Form frm, TabPage tabP)
        {
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Parent = tabP;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            tabMain.SelectedTab = tabP;
        }

        private void alarm(string msg)
        {
            if (!isAlarm)
            {
                isAlarm = true;
                MachineStatus.PreStatus = currentMachineStatus;
            }
            currentMachineStatus = MachineStatus.Status.故障报警;
            appendText(string.Format("程序异常暂停,故障报警:{0}", msg));
        }

        #endregion

        #region UI操作

        #region 选项卡切换

        private void 主界面toolStripButton1_Click(object sender, EventArgs e)
        {
            HometoolStripButton1_Click();
        }

        public void HometoolStripButton1_Click()
        {
            if (frmhome.Authority != "")
            {
                frmhome.RePaintUI();
                tabMain.SelectedTab = tabPMain;
                if (HaveChangeProduct)
                {
                    HaveChangeProduct = false;
                    frmhome.DisposeDesayTestDlg();
                }
                frmhome.ShowDesayTestDlg();
                if (frmhome.Authority == "管理员")
                {
                    if (frmDebug == null || frmDebug.IsDisposed)
                    {
                        frmDebug = new frmMotionDebug();
                    }
                    frmDebug.Show();
                }
                appendText("打开主界面。。。");
            }
            else
            {
                MessageBox.Show("请先登录用户！");
                appendText("请先登录用户！");
            }
        }

        private void 用户toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Marking.ManualMode)
            {
                frmhome.HideDesayTestDlg();
                tabMain.SelectedTab = tabPUsers;
                appendText("打开用户界面。。。");
            }
            else
            { appendText("请选择手动模式，再执行此操作！"); }
        }

        private void 产品列表toolStripButton1_Click(object sender, EventArgs e)
        {
            if (frmhome.Authority != "操作员" && frmhome.Authority != "")
            {
                if (Marking.ManualMode)
                {
                    frmhome.HideDesayTestDlg();
                    tabMain.SelectedTab = tabPProductList;
                    appendText("打开产品列表界面。。。");

                }
                else
                { appendText("请选择手动模式，再执行此操作！"); }
            }
            else
            {
                MessageBox.Show("此用户不能操作！请联系技术员。");
                appendText("此用户不能操作！请联系技术员。");
            }
        }

        private void 扫码器toolStripButton1_Click(object sender, EventArgs e)
        {
            if (frmhome.Authority != "操作员" && frmhome.Authority != "")
            {
                if (Marking.ManualMode)
                {
                    frmhome.HideDesayTestDlg();
                    tabMain.SelectedTab = tabPScanBarcode;
                    appendText("打开扫码器列表界面。。。");
                }
                else
                { appendText("请选择手动模式，再执行此操作！"); }
            }
            else
            {
                MessageBox.Show("此用户不能操作！请联系技术员。");
                appendText("此用户不能操作！请联系技术员。");
            }
        }

        private void 运动控制卡toolStripButton4_Click(object sender, EventArgs e)
        {
            if (frmhome.Authority != "操作员" && frmhome.Authority != "")
            {
                if (Marking.ManualMode)
                {
                    frmhome.HideDesayTestDlg();
                    tabMain.SelectedTab = tabPMotionCard;
                    appendText(string.Format("打开运动控制卡界面。。。"));
                }
                else
                {
                    appendText("请选择手动模式，再执行此操作！");
                }
            }
            else
            {
                MessageBox.Show("此用户不能操作！请联系技术员。");
                appendText("此用户不能操作！请联系技术员。");
            }
        }

        private void IO卡toolStripButton5_Click(object sender, EventArgs e)
        {
            if (frmhome.Authority != "操作员" && frmhome.Authority != "")
            {
                frmIO.IsManualMode = Marking.ManualMode;
                frmhome.HideDesayTestDlg();
                tabMain.SelectedTab = tabPIOCard;
                appendText("打开IO卡界面。。。");
                // HiddenDesayTestDlg();
            }
            else
            {
                MessageBox.Show("此用户不能操作！请联系技术员。");
                appendText("此用户不能操作！请联系技术员。");
            }
        }

        private void MEStoolStripButton6_Click(object sender, EventArgs e)
        {
            if (frmhome.Authority != "操作员" && frmhome.Authority != "")
            {
                frmhome.HideDesayTestDlg();
                tabMain.SelectedTab = tabPMES;
                appendText("打开MES配置界面。。。");
                // HiddenDesayTestDlg();
            }
            else
            {
                MessageBox.Show("此用户不能操作！请联系技术员。");
                appendText("此用户不能操作！请联系技术员。");
            }
        }

        private void 系统设置toolStripButton1_Click(object sender, EventArgs e)
        {
            if (frmhome.Authority != "操作员" && frmhome.Authority != "")
            {
                if (Marking.ManualMode)
                {
                    frmhome.HideDesayTestDlg();
                    tabMain.SelectedTab = tabPSystemSetting;
                    appendText("打开系统设置界面。。。");
                }
                else
                { appendText("请选择手动模式，再执行此操作！"); }
            }
            else
            {
                MessageBox.Show("此用户不能操作！请联系技术员。");
                appendText("此用户不能操作！请联系技术员。");
            }
        }

        private void 退出toolStripButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否确定关闭软件?", "关闭提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            appendText(string.Format("关闭软件：已选择（{0}）", result == DialogResult.Cancel ? "取消" : "确定"));

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);
                SerializerManager<Position>.Instance.Save(Marking.PositionPath, Position.Instance);
                frmhome.DisposeDesayTestDlg();
                //平移气缸
                IoPoints.IDO0.Value = false;
                IoPoints.IDO2.Value = false;
                //指示灯
                IoPoints.TDO14.Value = false;
                IoPoints.TDO15.Value = false;
                //三色灯
                MachineStatus.ExitSystem = true;
                IoPoints.TDO0.Value = false;
                IoPoints.TDO1.Value = false;
                IoPoints.TDO2.Value = false;
                IoPoints.TDO3.Value = false;
            }
        }

        #endregion

        #region 设备运行操作

        //手动模式 自动模式切换
        private void tbtnMode_Click(object sender, EventArgs e)
        {
            if (HaveReseted || !Marking.ManualMode)//设备已复位,可以切换自动模式
            {
                Marking.ManualMode = !Marking.ManualMode;
                if (Marking.ManualMode)
                {
                    tbtnMode.Text = "  手动模式  ";
                    tbtnMode.Image = Properties.Resources.Manual;
                }
                else
                {
                    tbtnMode.Text = "  自动模式  ";
                    tbtnMode.Image = Properties.Resources.Auto;
                }
                appendText(string.Format("模式选择：{0}", tbtnMode.Text));

                if (Marking.ManualMode)
                {
                    tbtnStart.Enabled = false; //手动模式禁用启动按键
                    tbtnStop.Enabled = false;  //手动模式禁用停止按键
                    tbtnPause.Enabled = false; //手动模式禁用暂停按键
                }
                else
                {
                    if (!isAlarm)
                    {
                        tbtnStart.Enabled = true; //自动模式+无报警 启用启动按键
                        tbtnStop.Enabled = true;  //手动模式+无报警 启用停止按键
                        tbtnPause.Enabled = true; //手动模式+无报警 启用暂停按键
                    }
                    else
                    {
                        appendText("设备报警中，无法切换到自动模式！");
                        Marking.ManualMode = true;
                        tbtnMode.Text = "  手动模式  ";
                        tbtnMode.Image = Properties.Resources.Manual;
                    }
                }
            }
            else
            {
                appendText("设备未复位，无法切换到自动模式！");
            }
        }

        private void tbtnStart_Click(object sender, EventArgs e)
        {
            if (!Marking.ManualMode)
            {
                frmMotion.Z.MoveAbs(frmMotion.ZStandbyPos);
                isPausing = false; //启动
                tbtnStart.Enabled = false;   //启动后 禁用启动按键
                tbtnStop.Enabled = true;     //启动后 启用停止按键
                tbtnPause.Enabled = true;    //启动后 启用暂停按键
                tbtnReset.Enabled = false;   //启动后 禁用复位按键                
                IoPoints.TDO5.Value = false;
                currentMachineStatus = MachineStatus.Status.运行ing;

                if (!online) //未启动，启动3个线程
                {
                    online = true; //启动
                    if (Config.Instance.IsLeftWork)
                    {
                        Thread leftWorkThread = new Thread(new ThreadStart(TestLeft));
                        leftWorkThread.IsBackground = true;
                        leftWorkThread.Start();
                    }

                    if (Config.Instance.IsRightWork)
                    {
                        Thread rightWorkThread = new Thread(new ThreadStart(TestRight));
                        rightWorkThread.IsBackground = true;
                        rightWorkThread.Start();
                    }

                    if (Config.Instance.IsScan)
                    {
                        Thread scanBarWorkThread = new Thread(new ThreadStart(scanBarWork));
                        scanBarWorkThread.IsBackground = true;
                        scanBarWorkThread.Start();
                    }
                }
                appendText("启动。。！");
            }
        }

        private void tbtnStop_Click(object sender, EventArgs e)
        {
            sw.Restart();
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs>(tbtnStop_Click), sender, e);
            }
            else
            {
                if (!Marking.ManualMode)
                {
                    online = false;//停止
                    tbtnStart.Enabled = false; //停止后禁用启动按键
                    tbtnStop.Enabled = false;  //停止后禁用停止按键
                    tbtnPause.Enabled = false; //停止后禁用暂停按键                    
                    tbtnReset.Enabled = true;  //停止后启用复位按键
                    HaveReseted = false;//停止
                    IoPoints.TDO4.Value = true;
                    IoPoints.TDO5.Value = false;
                    currentMachineStatus = MachineStatus.Status.停止;
                    appendText(string.Format("{0}。。！", currentMachineStatus));
                }
            }
        }

        private void tbtnPause_Click(object sender, EventArgs e)
        {
            sw.Restart();
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs>(tbtnPause_Click), sender, e);
            }
            else
            {
                if (!Marking.ManualMode)
                {
                    tbtnStart.Enabled = true;   //暂停后启用启动按键
                    tbtnPause.Enabled = false;  //暂停后禁用暂停按键
                    tbtnStop.Enabled = true;    //暂停后启用停止按键
                    IoPoints.TDO5.Value = true;
                    isPausing = true;//暂停
                    currentMachineStatus = MachineStatus.Status.暂停;
                    appendText(string.Format("{0}。。！", currentMachineStatus));
                }
            }
        }

        /// <summary>
        /// 复位中标志位
        /// </summary>
        private bool isReseting = false;
        private void tbtnReset_Click(object sender, EventArgs e)
        {

            if (frmhome.Authority != "")
            {
                if (!isReseting && Marking.ManualMode)//手动模式+未复位
                {
                    isReseting = true;
                    new Action(() =>
                    {
                        reset();
                    }).BeginInvoke(null, null);
                }
                else if (!isReseting)//自动模式+未复位
                {
                    if (isAlarm) //自动模式+有报警
                    {
                        appendText("设备清除报警后将继续运行，请注意安全！");
                        isAlarm = false;
                        currentMachineStatus = MachineStatus.PreStatus;
                    }
                    else //自动模式+无报警
                    {
                        appendText("设备在手动模式下，才能复位！");
                    }
                }
                else  //自动模式+复位中
                {
                    appendText("设备正在复位中。。。");
                }
            }
            else
            {
                MessageBox.Show("请先登录用户！");
                appendText("请先登录用户！");
            }
        }

        private void tbtnSpeaker_Click(object sender, EventArgs e)
        {
            MachineStatus.VoiceClosed = !MachineStatus.VoiceClosed;
            if (MachineStatus.VoiceClosed)
            {
                tbtnSpeaker.Text = "静音";
                tbtnSpeaker.Image = Properties.Resources.silent;
            }
            else
            {
                tbtnSpeaker.Text = "蜂鸣";
                tbtnSpeaker.Image = Properties.Resources.sound;
            }
        }
        #endregion

        #region UI界面更新
        private void tbtnClear_Click(object sender, EventArgs e)
        {
            //提示
            frmhome.ProductionReset();
            //总数,OK,NG清零
            productCount(null);
            //不良率清零
            frmhome.ClearAllData();
        }

        private void productCount(bool? isOK)
        {
            frmhome.productCount(isOK);
        }

        private void updateOperator(string operato)
        {
            string[] strTemps = operato.Split(',');
            frmhome.Operator = strTemps[0];
            frmhome.Authority = strTemps[1];
            frmhome.UpdateUIData();
        }

        private void periodStart(int num)
        {
            if (!Config.Instance.IsLeftWork)
            {
                periodStartTime.Add(DateTime.Now);
            }
            if (periodStartTime.Count == num)
            {
                periodStartTime.Add(DateTime.Now);
            }
            periodStartTime[num] = DateTime.Now;
        }

        private void periodEnd(int num)
        {
            TimeSpan ts = DateTime.Now.Subtract(periodStartTime[num]);
            节拍toolStripStatusLabel2.Text = string.Format("{0}:{1}ms", ts.Seconds, ts.Milliseconds);
        }

        private void resetLeftPicBoxControlColor()
        {
            frmhome.ResetLeftPicBoxColor();
        }

        private void resetRightPicBoxControlColor()
        {
            frmhome.ResetRightPicBoxColor();
        }

        private void changePicBoxImg(string ControlName, bool res)
        {
            frmhome.ChangePicBoxImg(ControlName, res);
        }

        private void AppendText(string txt)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendText), txt);
            }
            else
            {
                txtLog.Text = txtLog.Text.Insert(0, string.Format("{0}---{1}{2}{2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), txt, Environment.NewLine));
                log.Debug(txt);
            }
        }

        private void appendText(string str)
        {
            AppendText(string.Format("<<主界面>>:: {0}", str));
        }

        #endregion

        #endregion

        #region PrivateMethod 

        #region 扫码
        private void scanBarWork()
        {
            while (online) //scanBarWork 扫码
            {
                if (!isAlarm && (scanIAR == null || scanIAR.IsCompleted) && (!isPausing) &&
                   ((Config.Instance.IsLeftWork && !leftWorkingFlag && !IoPoints.IDI6.Value) || (Config.Instance.IsRightWork && !rightWorkingFlag && !IoPoints.IDI8.Value)))
                {
                    scanIAR = frmScan.BeginTrigger();
                }

                Thread.Sleep(200);
            }
        }

        private void frmScanBarcode(string received, TriggerStartingEventArgs trg)
        {
            try
            {
                if (!(string.IsNullOrEmpty(received) || received.Contains("ERROR") || received == "TimeOut"))
                {
                    scanBarDis = received;
                    if (HaveDropNG)
                    {
                        appendText(string.Format("扫码枪条码:{0}", received));
                        WriteSNInToFile(scanBarDis);
                    }
                    else
                    {
                        appendText("请将NG品丢弃后，再进行扫码！");
                    }
                }
            }
            catch (Exception ex)
            {
                tbtnPause_Click(null, null);
                appendText(string.Format("程序异常暂停,扫条码流程异常！:{0}", ex.ToString()));
            }
        }

        public void WriteSNInToFile(string SNStr)
        {
            try
            {
                string path = "";
                if (!IoPoints.IDI6.Value && isLeftRun)
                {
                    path = @"D:\SN\SN.ini";
                    isLeftScan = true;
                    leftBarCodeLabel.Text = SNStr;
                    IoPoints.IDO4.Value = false;
                    IoPoints.IDO5.Value = false;
                    frmhome.LeftRes = "";
                }

                if (!IoPoints.IDI8.Value && !isLeftRun)
                {
                    path = @"D:\SN\SN02.ini";
                    isRightScan = true;
                    rightBarCodeLabel.Text = SNStr;
                    IoPoints.IDO6.Value = false;
                    IoPoints.IDO7.Value = false;
                    frmhome.RightRes = "";
                }

                //if (!File.Exists(path))
                //{
                //    FileStream stream = System.IO.File.Create(path);
                //    stream.Close();
                //    stream.Dispose();
                //}
                ////写入文件
                //IniFile.WriteValue("BarCode", "Sn", SNStr, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #endregion

        #region 复位、测试、急停流程
        private void mainWork()
        {
            while (true)
            {
                if (online && !isAlarm) // mainwork
                {
                    try
                    {
                        #region-----左工作-----
                        if (Config.Instance.IsLeftWork && ((!IoPoints.TDI5.Value && !IoPoints.TDI6.Value && isLeftScan) || Config.Instance.IsDryRunMode) && !leftWorkingFlag && isLeftRun)
                        {
                            isLeftScan = false;
                            isLeftRun = false;
                            leftTestProcess = 0;
                            leftWorkingFlag = true;
                            leftBarcode = scanBarDis;
                            //结果
                            resetLeftPicBoxControlColor();
                            bLeftResult = true;
                            bLeftTotalResult = true;
                            leftSZErrorCode = "PASS";

                            bLeftCurentValue = 0;
                            bLeftVoltageValue = 0;

                            IoPoints.IDO4.Value = false;
                            IoPoints.IDO5.Value = false;

                            ImageSitA.SetBarcode(leftBarcode, "JIG_0100");
                            periodStart(0);
                            //左边新增IR上电
                            IoPoints.IDO8.Value = true;
                        }
                        else if (!Config.Instance.IsLeftWork)
                        {
                            isLeftRun = false;
                        }
                        #endregion

                        #region-----右工作-----
                        if (Config.Instance.IsRightWork && ((!IoPoints.TDI5.Value && !IoPoints.TDI6.Value && isRightScan) || Config.Instance.IsDryRunMode) && !rightWorkingFlag && !isLeftRun)
                        {
                            isRightScan = false;
                            isLeftRun = true;
                            rightTestProcess = 0;
                            rightWorkingFlag = true;
                            rightBarcode = scanBarDis;

                            resetRightPicBoxControlColor();
                            bRightResult = true;
                            bRightTotalResult = true;
                            rightSZErrorCode = "PASS";

                            bRightCurentValue = 0;
                            bRightVoltageValue = 0;

                            IoPoints.IDO6.Value = false;
                            IoPoints.IDO7.Value = false;

                            ImageSitB.SetBarcode(rightBarcode, "JIG_0100");
                            periodStart(1);

                            //右边新增IR上电
                            IoPoints.IDO9.Value = true;
                        }
                        else if (!Config.Instance.IsRightWork)
                        {
                            isLeftRun = true;
                        }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        appendText(string.Format("(检查原因解决后重启软件)程序异常暂停,主工作流程异常！:{0}", e.ToString()));
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void TestLeft()
        {
            while (online) //TestLeft
            {
                if (!isAlarm && !isPausing)
                {
                    switch (leftTestProcess)
                    {
                        case 0:
                            if (!IoPoints.IDI6.Value)
                            {
                                appendText("左顶盖检测：是否未压紧？");
                                MessageBox.Show("左顶盖检测：是否未压紧？");
                            }
                            else
                            {
                                leftTestProcess = 6;
                            }
                            break;
                        case 6:
                            if (ImageSitA.PlayCamera())
                            {
                                appendText("左工位：点亮成功");
                                leftTestProcess = 10;
                            }
                            else
                            {
                                appendText("左工位：点亮失败");
                                leftSZErrorCode = "OpenCamera NG";
                                bLeftTotalResult = false;
                                leftTestProcess = 340;
                            }
                            break;
                        case 10:
                            if (axisY1.IsAction())
                            {
                                frmMotion.Y1.IsServoON = true;
                                if (Position.Instance.testItem.bDark_Test)
                                {
                                    frmMotion.Y1.MoveAbs(frmMotion.LeftBlackPos);
                                    leftTestProcess = 20;
                                    appendText("左工位：准备移动到黑场测试");
                                }
                                else
                                {
                                    leftTestProcess = 90;
                                }
                            }
                            break;
                        case 20:
                            if (frmMotion.Y1.IsPosComplete(frmMotion.LeftBlackPos))
                            {
                                leftTestProcess = 30;
                                appendText("左工位：到达黑场测试位");
                            }
                            break;

                        case 30:
                            if (cyLeftBlackUpDown.IsAction())
                            {
                                IoPoints.TDO8.Value = false;
                                IoPoints.TDO9.Value = true;
                                leftTestProcess = 40;
                            }
                            break;

                        case 40://黑场下感应
                            if (IoPoints.TDI9.Value)
                            {
                                Thread.Sleep(100);
                                leftTestProcess = 50;
                            }
                            break;
                        case 50:
                            if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bBlackEN)
                            {
                                appendText("左工位：开始测试黑场");
                                StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                bLeftResult = ImageSitA.DesayTestDark(buf);
                                Thread.Sleep(500);
                                string strout = buf.ToString();
                                appendText(strout);
                                if (!bLeftResult)
                                {
                                    leftSZErrorCode = ("Dark error");
                                    if (bLeftTotalResult)
                                    {
                                        Position.Instance.ItemCount.bBlackEN++;
                                    }                                    
                                } 
                                appendText("左工位：黑场测试结束");
                                bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                changePicBoxImg("BlackPicBoxA", bLeftResult);
                            }
                            if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bInfraredDarkEN)
                            {
                                appendText("左工位：打开红外灯");
                                ImageSitA.IR_LED_ON();
                                Thread.Sleep(500);
                                StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                bLeftResult = ImageSitA.DesayTestDark_LedLight(buf);
                                Thread.Sleep(500);
                                string strout = buf.ToString();
                                appendText(strout);
                                appendText("左工位：关闭红外灯");
                                ImageSitA.IR_LED_OFF();
                                if (!bLeftResult)
                                {
                                    leftSZErrorCode = ("Infrared_Dark error");
                                    if (bLeftTotalResult)
                                    {
                                        Position.Instance.ItemCount.bInfraredDarkEN++;
                                    }
                                }
                                appendText("左工位：黑场红外亮度测试结束");
                                bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                changePicBoxImg("InfraredDarkPicBoxA", bLeftResult);                                
                            }
                            appendText("左工位：黑场测试结束");
                            leftTestProcess = 60;
                            break;

                        case 60:
                            leftTestProcess = 70;
                            break;

                        case 70:
                            if (cyLeftBlackUpDown.IsAction())
                            {
                                IoPoints.TDO9.Value = false;
                                IoPoints.TDO8.Value = true;
                                leftTestProcess = 80;
                            }
                            break;

                        case 80://黑场上感应
                            if (IoPoints.TDI8.Value)
                            {
                                Thread.Sleep(100);
                                leftTestProcess = 90;
                            }
                            break;
                        case 90://等待
                            if (axisY1.IsAction())
                            {
                                if (Position.Instance.testItem.bLight_Test)
                                {
                                    if (!IsWhiteWorking)
                                    {
                                        IsWhiteWorking = true;
                                        frmMotion.Y1.MoveAbs(frmMotion.LeftWhitePos);
                                        leftTestProcess = 100;
                                        appendText("左工位：准备移动到白场测试");
                                    }
                                }
                                else
                                {
                                    leftTestProcess = 210;
                                }
                            }
                            break;
                        case 100:
                            if (frmMotion.Y1.IsPosComplete(frmMotion.LeftWhitePos))
                            {
                                appendText("左工位：到达白场测试位");
                                ComPort7.BeginTrigger2("01 03 00 00 00 04 44 09");//发送读取电压的命令   
                                leftTestProcess = 105;
                            }
                            break;
                        case 105://白场上感应
                            if (IoPoints.TDI12.Value)
                            {
                                leftTestProcess = 110;
                            }
                            break;
                        case 110://左通道平移气缸伸出
                            if (cyLeftSidesway.IsAction())
                            {
                                IoPoints.IDO0.Value = false;
                                IoPoints.IDO1.Value = true;
                                leftTestProcess = 120;
                                Thread.Sleep(1000);
                            }
                            break;
                        case 120://左通道平移气缸伸出感应
                            if (IoPoints.TDI15.Value)
                            {
                                leftTestProcess = 130;
                            }
                            break;
                        case 130:
                            if (cyWhiteUpDown.IsAction())
                            {
                                IoPoints.TDO13.Value = false;
                                IoPoints.TDO12.Value = true;
                                leftTestProcess = 140;
                                Thread.Sleep(1000);
                            }
                            break;
                        case 140://白场升降气缸下感应
                            if (IoPoints.TDI13.Value)
                            {
                                Thread.Sleep(100);
                                appendText("左工位：开始测试白场");
                                if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bWBEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bLeftResult = ImageSitA.DesayTestWhite_WB(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bLeftResult)
                                    {
                                        leftSZErrorCode = ("WB Error");
                                        if (bLeftTotalResult)
                                        {
                                            Position.Instance.ItemCount.bWBEN++;
                                        }
                                    }
                                    appendText("左工位：WB测试结束");
                                    bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                    changePicBoxImg("WBPicBoxA", bLeftResult);
                                }
                                if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bShadingEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bLeftResult = ImageSitA.DesayTestWhite_Shading(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bLeftResult)
                                    {
                                        leftSZErrorCode = ("Shading Error");
                                        if (bLeftTotalResult)
                                        {
                                            Position.Instance.ItemCount.bShadingEN++;
                                        }
                                    }
                                    appendText("左工位：Shading测试结束");
                                    bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                    changePicBoxImg("ShadingPicBoxA", bLeftResult);
                                }
                                if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bBlemishEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bLeftResult = ImageSitA.DesayTestWhite_Blemish(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bLeftResult)
                                    {
                                        leftSZErrorCode = ("Blemish Error");
                                        if (bLeftTotalResult)
                                        {
                                            Position.Instance.ItemCount.bBlemishEN++;
                                        }
                                    }
                                    appendText("左工位：Blemish测试结束");
                                    bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                    changePicBoxImg("BlemishPicBoxA", bLeftResult);
                                }
                                if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bBadPixelEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bLeftResult = ImageSitA.DesayTestWhite_DefectPixel(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bLeftResult)
                                    {
                                        leftSZErrorCode = ("BadPixel Error");
                                        if (bLeftTotalResult)
                                        {
                                            Position.Instance.ItemCount.bBadPixelEN++;
                                        }
                                    }
                                    appendText("左工位：BadPixel测试结束");
                                    bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                    changePicBoxImg("BadPixelPicBoxA", bLeftResult);
                                }
                                if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bHotPixelEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bLeftResult = ImageSitA.DesayTestWhite_DefectPixel(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bLeftResult)
                                    {
                                        leftSZErrorCode = ("HotPixel Error");
                                        if (bLeftTotalResult)
                                        {
                                            Position.Instance.ItemCount.bHotPixelEN++;
                                        }
                                    }
                                    appendText("左工位：HotPixel测试结束");
                                    bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                    changePicBoxImg("HotPixelPicBoxA", bLeftResult);
                                }
                                appendText("左工位：白场测试结束");
                                leftTestProcess = 150;
                            }
                            break;
                        case 150:
                            if (cyWhiteUpDown.IsAction())
                            {
                                IoPoints.TDO12.Value = false;
                                IoPoints.TDO13.Value = true;
                                leftTestProcess = 180;
                                Thread.Sleep(1000);
                            }
                            break;
                        case 180://白场升降气缸上感应
                            if (IoPoints.TDI12.Value)
                            {
                                Thread.Sleep(100);
                                leftTestProcess = 190;
                            }
                            break;
                        case 190://左通道平移气缸缩回
                            if (cyLeftSidesway.IsAction())
                            {
                                IoPoints.IDO1.Value = false;
                                IoPoints.IDO0.Value = true;
                                leftTestProcess = 200;
                            }
                            break;
                        case 200://左通道平移气缸缩回感应
                            if (IoPoints.TDI14.Value)
                            {
                                Thread.Sleep(100);
                                leftTestProcess = 210;
                                //ComPort7.BeginTrigger2("02 03 00 08 00 04 C5 F8");//发送读取电流的命令
                            }
                            break;

                        case 210:
                            leftTestProcess = 220;
                            break;
                        case 220://等待
                            if (axisY1.IsAction())
                            {
                                if (Position.Instance.testItem.bMTF_Test)
                                {
                                    frmMotion.Y1.MoveAbs(frmMotion.LeftGraphicPos);
                                    IsWhiteWorking = false;
                                    leftTestProcess = 230;
                                    appendText("左工位：准备移动到图卡测试");
                                }
                                else
                                {
                                    leftTestProcess = 320;
                                }
                            }
                            break;
                        case 230://到达图卡测试位
                            if (frmMotion.Y1.IsPosComplete(frmMotion.LeftGraphicPos))
                            {
                                leftTestProcess = 250;
                                appendText("左工位：到达图卡测试位");
                            }
                            break;
                        case 250://左通道平移气缸伸出
                            if (cyLeftSidesway.IsAction() && !IsMTFWorking)
                            {
                                IsMTFWorking = true;
                                IoPoints.IDO0.Value = false;
                                IoPoints.IDO1.Value = true;
                                leftTestProcess = 260;
                            }
                            break;
                        case 260://左通道平移气缸伸出感应
                            if (IoPoints.TDI15.Value)
                            {
                                if (Position.Instance.testItem.bInfraredDarkEN)
                                {
                                    ImageSitA.IR_LED_ON();
                                    Thread.Sleep(500);
                                    ComPort7.BeginTrigger2("02 03 00 08 00 04 C5 F8");//发送读取电流的命令
                                    Thread.Sleep(500);
                                    ImageSitA.IR_LED_OFF();
                                }

                                Thread.Sleep(Config.Instance.bDelayTimeValue);
                                if (IoPoints.TDI15.Value)
                                {
                                    appendText("左工位：开始测试图卡");                                    
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bIRMTFEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bLeftResult = ImageSitA.DesayTestIRChart(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("IRMTF Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bIRMTFEN++;
                                            }
                                        }
                                        appendText("左工位：IRMTF测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("IRMTFPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && (Position.Instance.testItem.bColorEN || Position.Instance.testItem.bGrayEN))
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        int result = ImageSitA.DesayTestChart_ColorGray(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (result == 0)
                                        {
                                            bLeftResult = true;
                                            changePicBoxImg("ColorPicBoxA", bLeftResult);
                                            changePicBoxImg("GrayPicBoxA", bLeftResult);
                                        }
                                        else
                                        {
                                            bLeftResult = false;
                                            if (result == 1)
                                            {
                                                leftSZErrorCode = ("Color Error");
                                                changePicBoxImg("ColorPicBoxA", false);
                                                changePicBoxImg("GrayPicBoxA", true);
                                                if (bLeftTotalResult)
                                                {
                                                    Position.Instance.ItemCount.bColorEN++;
                                                }
                                            }
                                            else if (result == 2)
                                            {
                                                leftSZErrorCode = ("Gray Error");
                                                changePicBoxImg("GrayPicBoxA", false);
                                                changePicBoxImg("ColorPicBoxA", true);
                                                if (bLeftTotalResult)
                                                {
                                                    Position.Instance.ItemCount.bGrayEN++;
                                                }
                                            }
                                            else
                                            {
                                                changePicBoxImg("ColorPicBoxA", false);
                                                changePicBoxImg("GrayPicBoxA", false);
                                                if (bLeftTotalResult)
                                                {
                                                    Position.Instance.ItemCount.bColorEN++;
                                                    Position.Instance.ItemCount.bGrayEN++;
                                                }
                                            }
                                        }
                                        appendText("左工位：Color/Gray测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                    }

                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bFOVEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bLeftResult = ImageSitA.DesayTestChart_FOV(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("FOV Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bFOVEN++;
                                            }
                                        }
                                        appendText("左工位：FOV测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("FOVPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bAlignmentEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bLeftResult = ImageSitA.DesayTestChart_Alignment(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("Aligment Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bAlignmentEN++;
                                            }
                                        }
                                        appendText("左工位：Aligment测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("AlignmentPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bDistortionEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bLeftResult = ImageSitA.DesayTestChart_Distortion(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("Distortion Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bDistortionEN++;
                                            }
                                        }
                                        appendText("左工位：Distortion测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("DistortionPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bSNREN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bLeftResult = ImageSitA.DesayTestChart_SNR(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("SNR Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bSNREN++;
                                            }
                                        }
                                        appendText("左工位：SNR测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("SNRPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bRotationEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bLeftResult = ImageSitA.DesayTestChart_TiltRotation(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("Rotation Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bRotationEN++;
                                            }
                                        }
                                        appendText("左工位：Rotation测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("RotationPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bFPSEN)
                                    {
                                        float fps = ImageSitA.DesayTest_GetFPS();
                                        Thread.Sleep(500);
                                        appendText("左工位测试帧率为：" + fps.ToString());
                                        if (fps > Position.Instance.testItem.bFPSMinValue && fps < Position.Instance.testItem.bFPSMaxValue)
                                        {
                                            bLeftResult = true;
                                        }
                                        else
                                        {
                                            bLeftResult = false;
                                            leftSZErrorCode = ("FPS Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bFPSEN++;
                                            }
                                        }
                                        appendText("左工位：FPS测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("FPSPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bVoltageEN)
                                    {
                                        appendText("左工位电压为:" + bLeftVoltageValue.ToString());
                                        if (bLeftVoltageValue > Position.Instance.testItem.bVoltageMinValue && bLeftVoltageValue < Position.Instance.testItem.bVoltageMaxValue)
                                        {
                                            bLeftResult = true;
                                        }
                                        else
                                        {
                                            bLeftResult = false;
                                            leftSZErrorCode = ("Voltage Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bVoltageEN++;
                                            }
                                        }
                                        appendText("左工位：电压测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("VoltagePicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bCurrentEN)
                                    {
                                        appendText("左工位电流为:" + bLeftCurentValue.ToString());
                                        if (bLeftCurentValue > Position.Instance.testItem.bCurrentMinValue && bLeftCurentValue < Position.Instance.testItem.bCurrentMaxValue)
                                        {
                                            bLeftResult = true;
                                        }
                                        else
                                        {
                                            bLeftResult = false;
                                            leftSZErrorCode = ("Current Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bCurrentEN++;
                                            }
                                        }
                                        appendText("左工位：电流测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("CurrentPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bPowerEN)
                                    {
                                        double power = bLeftCurentValue * bLeftVoltageValue;
                                        appendText("左工位产品功率为：" + power.ToString());
                                        if (power > Position.Instance.testItem.bPowerMinValue && power < Position.Instance.testItem.bPowerMaxValue)
                                        {
                                            bLeftResult = true;
                                        }
                                        else
                                        {
                                            bLeftResult = false;
                                            leftSZErrorCode = ("Power Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bPowerEN++;
                                            }
                                        }
                                        appendText("左工位：功率测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("PowPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bChangeViewEN)
                                    {
                                        bLeftResult = sideAChangeViewTest();
                                        Thread.Sleep(500);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("ChangeView Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bChangeViewEN++;
                                            }
                                        }
                                        appendText("左工位：切换测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("ChangeViewPicBoxA", bLeftResult);
                                    }
                                    if ((bLeftTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bMTFEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);
                                        bLeftResult = ImageSitA.DesayTestChart_SFR(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bLeftResult)
                                        {
                                            leftSZErrorCode = ("MTF Error");
                                            if (bLeftTotalResult)
                                            {
                                                Position.Instance.ItemCount.bMTFEN++;
                                            }
                                        }
                                        ImageSitA.StopCamera();
                                        appendText("左工位：MTF测试结束");
                                        bLeftTotalResult = bLeftTotalResult & bLeftResult;
                                        changePicBoxImg("MTFPicBoxA", bLeftResult);
                                    }
                                    appendText("左工位：图卡测试结束");
                                    leftTestProcess = 300;
                                }
                            }
                            break;
                        case 300://左通道平移气缸缩回
                            if (cyLeftSidesway.IsAction())
                            {
                                IoPoints.IDO1.Value = false;
                                IoPoints.IDO0.Value = true;
                                leftTestProcess = 310;
                            }
                            break;
                        case 310://左通道平移气缸缩回感应
                            if (IoPoints.TDI14.Value)
                            {
                                IsMTFWorking = false;
                                leftTestProcess = 320;
                            }
                            break;
                        case 320://Y1回到待命位
                            if (axisY1.IsAction() && !IsWhiteWorking)
                            {
                                frmMotion.Y1.MoveAbs(frmMotion.LeftStandbyPos);
                                leftTestProcess = 330;
                            }
                            break;
                        case 330://回待命位
                            if (frmMotion.Y1.IsPosComplete(frmMotion.LeftStandbyPos))
                            {
                                leftTestProcess = 340;
                            }
                            break;
                        case 340:
                            if (leftSZErrorCode == "PASS")
                            {
                                IoPoints.IDO4.Value = true;
                                IoPoints.IDO5.Value = false;
                                Config.Instance.LeftOKCount++;
                                productCount(true);
                            }
                            else
                            {
                                IoPoints.IDO4.Value = false;
                                IoPoints.IDO5.Value = true;
                                Config.Instance.LeftNGCount++;
                                productCount(false);
                            }

                            periodEnd(0);
                            ImageSitA.StopCamera();
                            appendText(string.Format("产品条码：{0},测试结果：{1}", leftBarcode, bLeftTotalResult));
                            frmhome.LeftRes = bLeftTotalResult ? "OK" : "NG";
                            HaveDropNG = bLeftTotalResult ? HaveDropNG : false;
                            leftWorkingFlag = false;
                            leftTestProcess = -1;
                            leftBarCodeLabel.Text = "";
                            break;
                    }
                }
            }
        }

        private void TestRight()
        {
            while (online) //TestRight
            {
                if (!isAlarm && !isPausing)
                {
                    switch (rightTestProcess)
                    {
                        case 0:
                            if (!IoPoints.IDI8.Value)
                            {
                                appendText("右顶盖检测：是否未压紧？");
                                MessageBox.Show("右顶盖检测：是否未压紧？");
                            }
                            else
                            {
                                rightTestProcess = 6;
                            }
                            break;
                        case 6:
                            if (ImageSitB.PlayCamera())
                            {
                                appendText("右工位：点亮成功");
                                rightTestProcess = 10;
                            }
                            else
                            {
                                appendText("右工位：点亮失败");
                                rightSZErrorCode = "OpenCamera NG";
                                bRightTotalResult = false;
                                rightTestProcess = 340;
                            }
                            break;
                        case 10:
                            if (axisY2.IsAction())
                            {
                                frmMotion.Y2.IsServoON = true;
                                if (Position.Instance.testItem.bDark_Test)
                                {
                                    frmMotion.Y2.MoveAbs(frmMotion.RightBlackPos);
                                    rightTestProcess = 20;
                                    appendText("右工位：准备移动到黑场测试");
                                }
                                else
                                {
                                    rightTestProcess = 90;
                                }
                            }
                            break;
                        case 20:
                            if (frmMotion.Y2.IsPosComplete(frmMotion.RightBlackPos))
                            {
                                rightTestProcess = 30;
                                appendText("右工位：到达黑场测试位");
                            }
                            break;

                        case 30:
                            if (cyRightBlackUpDown.IsAction())
                            {
                                IoPoints.TDO11.Value = false;
                                IoPoints.TDO10.Value = true;
                                rightTestProcess = 40;
                            }
                            break;

                        case 40://右通道黑场升降气缸下感应
                            if (IoPoints.TDI11.Value)
                            {
                                Thread.Sleep(100);
                                rightTestProcess = 50;
                            }
                            break;
                        case 50:
                            if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bBlackEN)
                            {
                                appendText("右工位：开始测试黑场");
                                StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                bRightResult = ImageSitB.DesayTestDark(buf);
                                Thread.Sleep(500);
                                string strout = buf.ToString();
                                appendText(strout);
                                if (!bRightResult)
                                {
                                    rightSZErrorCode = ("Dark error");
                                    if (bRightTotalResult)
                                    {
                                        Position.Instance.ItemCount1.bBlackEN++;
                                    }
                                }
                                appendText("右工位：黑场测试结束");
                                bRightTotalResult = bRightTotalResult & bRightResult;
                                changePicBoxImg("BlackPicBoxB", bRightResult);
                            }
                            if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bInfraredDarkEN)
                            {
                                appendText("右工位：打开红外灯");
                                ImageSitB.IR_LED_ON();
                                Thread.Sleep(500);
                                StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                bRightResult = ImageSitB.DesayTestDark_LedLight(buf);
                                Thread.Sleep(500);
                                string strout = buf.ToString();
                                appendText(strout);
                                appendText("右工位：关闭红外灯");
                                ImageSitB.IR_LED_OFF();
                                if (!bRightResult)
                                {
                                    rightSZErrorCode = ("Infrared_Dark error");
                                    if (bRightTotalResult)
                                    {
                                        Position.Instance.ItemCount1.bInfraredDarkEN++;
                                    }
                                }
                                appendText("左工位：黑场红外亮度测试结束");
                                bRightTotalResult = bRightTotalResult & bRightResult;
                                changePicBoxImg("InfraredDarkPicBoxB", bRightResult);
                            }
                            appendText("右工位：黑场测试结束");
                            rightTestProcess = 60;
                            break;

                        case 60:
                            rightTestProcess = 70;
                            break;

                        case 70:
                            if (cyRightBlackUpDown.IsAction())
                            {
                                IoPoints.TDO10.Value = false;
                                IoPoints.TDO11.Value = true;
                                rightTestProcess = 80;
                            }
                            break;

                        case 80://右通道黑场升降气缸上感应
                            if (IoPoints.TDI10.Value)
                            {
                                Thread.Sleep(100);
                                rightTestProcess = 90;
                            }
                            break;
                        case 90:
                            if (axisY2.IsAction())
                            {
                                if (Position.Instance.testItem.bLight_Test)
                                {
                                    if (!IsWhiteWorking)
                                    {
                                        IsWhiteWorking = true;
                                        frmMotion.Y2.MoveAbs(frmMotion.RightWhitePos);
                                        rightTestProcess = 100;
                                        appendText("右工位：准备移动到白场测试");
                                    }
                                }
                                else
                                {
                                    rightTestProcess = 210;
                                }
                            }
                            
                            break;
                        case 100:
                            if (frmMotion.Y2.IsPosComplete(frmMotion.RightWhitePos))
                            {
                                appendText("右工位：到达白场测试位");
                                rightTestProcess = 105;
                                ComPort7.BeginTrigger2("01 03 00 00 00 04 44 09");//发送读取电压的命令
                            }
                            break;
                        case 105://白场上感应
                            if (IoPoints.TDI12.Value)
                            {
                                rightTestProcess = 110;
                            }
                            break;
                        case 110://右通道平移气缸伸出
                            if (cyRightSidesway.IsAction())
                            {
                                IoPoints.IDO2.Value = true;
                                IoPoints.IDO3.Value = false;
                                rightTestProcess = 120;
                                Thread.Sleep(1000);
                            }
                            break;
                        case 120://右边通道平移气缸伸出感应
                            if (IoPoints.IDI2.Value)
                            {
                                rightTestProcess = 130;
                            }
                            break;
                        case 130:
                            if (cyWhiteUpDown.IsAction())
                            {
                                IoPoints.TDO13.Value = false;
                                IoPoints.TDO12.Value = true;
                                rightTestProcess = 140;
                                Thread.Sleep(1000);
                            }
                            break;
                        case 140://白场升降气缸下感应
                            if (IoPoints.TDI13.Value)
                            {
                                Thread.Sleep(100);
                                appendText("右工位：开始测试白场");
                                if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bWBEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bRightResult = ImageSitB.DesayTestWhite_WB(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bRightResult)
                                    {
                                        rightSZErrorCode = ("WB Error");
                                        if (bRightTotalResult)
                                        {
                                            Position.Instance.ItemCount1.bWBEN++;
                                        }
                                    }
                                    appendText("右工位：WB测试结束");
                                    bRightTotalResult = bRightTotalResult & bRightResult;
                                    changePicBoxImg("WBPicBoxB", bRightResult);
                                }
                                if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bShadingEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bRightResult = ImageSitB.DesayTestWhite_Shading(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bRightResult)
                                    {
                                        rightSZErrorCode = ("Shading Error");
                                        if (bRightTotalResult)
                                        {
                                            Position.Instance.ItemCount1.bShadingEN++;
                                        }
                                    }
                                    appendText("右工位：Shading测试结束");
                                    bRightTotalResult = bRightTotalResult & bRightResult;
                                    changePicBoxImg("ShadingPicBoxB", bRightResult);
                                }
                                if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bBlemishEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bRightResult = ImageSitB.DesayTestWhite_Blemish(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bRightResult)
                                    {
                                        rightSZErrorCode = ("Blemish Error");
                                        if (bRightTotalResult)
                                        {
                                            Position.Instance.ItemCount1.bBlemishEN++;
                                        }
                                    }
                                    appendText("右工位：Blemish测试结束");
                                    bRightTotalResult = bRightTotalResult & bRightResult;
                                    changePicBoxImg("BlemishPicBoxB", bRightResult);
                                }
                                if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bBadPixelEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bRightResult = ImageSitB.DesayTestWhite_DefectPixel(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bRightResult)
                                    {
                                        rightSZErrorCode = ("BadPixel Error");
                                        if (bRightTotalResult)
                                        {
                                            Position.Instance.ItemCount1.bBadPixelEN++;
                                        }
                                    }
                                    appendText("右工位：BadPixel测试结束");
                                    bRightTotalResult = bRightTotalResult & bRightResult;
                                    changePicBoxImg("BadPixelPicBoxB", bRightResult);
                                }
                                if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bHotPixelEN)
                                {
                                    StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                    bRightResult = ImageSitB.DesayTestWhite_DefectPixel(buf);
                                    Thread.Sleep(500);
                                    string strout = buf.ToString();
                                    appendText(strout);
                                    if (!bRightResult)
                                    {
                                        rightSZErrorCode = ("HotPixel Error");
                                        if (bRightTotalResult)
                                        {
                                            Position.Instance.ItemCount1.bHotPixelEN++;
                                        }
                                    }
                                    appendText("右工位：HotPixel测试结束");
                                    bRightTotalResult = bRightTotalResult & bRightResult;
                                    changePicBoxImg("HotPixelPicBoxB", bRightResult);
                                }
                                appendText("右工位：白场测试结束");
                                rightTestProcess = 150;
                            }
                            break;
                        case 150:
                            if (cyWhiteUpDown.IsAction())
                            {
                                IoPoints.TDO12.Value = false;
                                IoPoints.TDO13.Value = true;
                                rightTestProcess = 180;
                                Thread.Sleep(1000);
                            }
                            break;
                        case 180://白场升降气缸上感应
                            if (IoPoints.TDI12.Value)
                            {
                                Thread.Sleep(100);
                                rightTestProcess = 190;
                            }
                            break;
                        case 190://右通道平移气缸缩回
                            if (cyRightSidesway.IsAction())
                            {
                                IoPoints.IDO2.Value = false;
                                IoPoints.IDO3.Value = true;
                                rightTestProcess = 200;
                            }
                            break;
                        case 200://右通道平移气缸缩回感应
                            if (IoPoints.IDI0.Value)
                            {
                                Thread.Sleep(100);
                                rightTestProcess = 210;
                                //ComPort7.BeginTrigger2("02 03 00 08 00 04 C5 F8");//发送读取电流的命令
                            }
                            break;

                        case 210:
                            rightTestProcess = 220;
                            break;
                        case 220:
                            if (axisY2.IsAction())
                            {
                                if (Position.Instance.testItem.bMTF_Test)
                                {
                                    frmMotion.Y2.MoveAbs(frmMotion.RightGraphicPos);
                                    IsWhiteWorking = false;
                                    rightTestProcess = 230;
                                    appendText("右工位：准备移动到图卡测试");
                                }
                                else
                                {
                                    rightTestProcess = 320;
                                }
                            }
                            break;
                        case 230://到达图卡测试位
                            if (frmMotion.Y2.IsPosComplete(frmMotion.RightGraphicPos))
                            {
                                rightTestProcess = 250;
                                appendText("右工位：到达图卡测试位");
                            }
                            break;
                        case 250://右通道平移气缸伸出
                            if (cyRightSidesway.IsAction())
                            {
                                IsMTFWorking = true;
                                IoPoints.IDO2.Value = true;
                                IoPoints.IDO3.Value = false;
                                rightTestProcess = 260;
                            }
                            break;
                        case 260://右通道平移气缸伸出感应
                            if (IoPoints.IDI2.Value)
                            {
                                if (Position.Instance.testItem.bInfraredDarkEN)
                                {
                                    ImageSitB.IR_LED_ON();
                                    Thread.Sleep(500);
                                    ComPort7.BeginTrigger2("02 03 00 08 00 04 C5 F8");//发送读取电流的命令
                                    Thread.Sleep(500);
                                    ImageSitB.IR_LED_OFF();
                                }

                                Thread.Sleep(Config.Instance.bDelayTimeValue);
                                if (IoPoints.IDI2.Value)
                                {
                                    appendText("右工位：开始测试图卡");                                    
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bIRMTFEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bRightResult = ImageSitB.DesayTestIRChart(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("IRMTF Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bIRMTFEN++;
                                            }
                                        }
                                        appendText("右工位：IRMTF测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("IRMTFPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && (Position.Instance.testItem.bColorEN || Position.Instance.testItem.bGrayEN))
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        int result = ImageSitB.DesayTestChart_ColorGray(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (result == 0)
                                        {
                                            bRightResult = true;
                                            changePicBoxImg("ColorPicBoxB", bRightResult);
                                            changePicBoxImg("GrayPicBoxB", bRightResult);
                                        }
                                        else
                                        {
                                            bRightResult = false;
                                            if (result == 1)
                                            {
                                                rightSZErrorCode = ("Color Error");
                                                changePicBoxImg("ColorPicBoxB", false);
                                                changePicBoxImg("GrayPicBoxB", true);
                                                if (bRightTotalResult)
                                                {
                                                    Position.Instance.ItemCount1.bColorEN++;
                                                }
                                            }
                                            else if (result == 2)
                                            {
                                                rightSZErrorCode = ("Gray Error");
                                                changePicBoxImg("ColorPicBoxB", true);
                                                changePicBoxImg("GrayPicBoxB", false);
                                                if (bRightTotalResult)
                                                {
                                                    Position.Instance.ItemCount1.bGrayEN++;
                                                }
                                            }
                                            else
                                            {
                                                changePicBoxImg("ColorPicBoxB", false);
                                                changePicBoxImg("GrayPicBoxB", false);
                                                if (bRightTotalResult)
                                                {
                                                    Position.Instance.ItemCount1.bColorEN++;
                                                    Position.Instance.ItemCount1.bGrayEN++;
                                                }
                                            }
                                        }
                                        appendText("右工位：Color/Gray测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                    }

                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bFOVEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bRightResult = ImageSitB.DesayTestChart_FOV(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("FOV Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bFOVEN++;
                                            }
                                        }
                                        appendText("右工位：FOV测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("FOVPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bAlignmentEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bRightResult = ImageSitB.DesayTestChart_Alignment(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("Aligment Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bAlignmentEN++;
                                            }
                                        }
                                        appendText("右工位：Aligment测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("AlignmentPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bDistortionEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bRightResult = ImageSitB.DesayTestChart_Distortion(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("Distortion Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bDistortionEN++;
                                            }
                                        }
                                        appendText("右工位：Distortion测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("DistortionPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bSNREN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bRightResult = ImageSitB.DesayTestChart_SNR(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("SNR Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bSNREN++;
                                            }
                                        }
                                        appendText("右工位：SNR测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("SNRPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bRotationEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                                        bRightResult = ImageSitB.DesayTestChart_TiltRotation(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("Rotation Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bRotationEN++;
                                            }
                                        }
                                        appendText("右工位：Rotation测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("RotationPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bFPSEN)
                                    {
                                        float fps = ImageSitB.DesayTest_GetFPS();
                                        Thread.Sleep(500);
                                        appendText("右工位测试帧率为：" + fps.ToString());
                                        if (fps > Position.Instance.testItem.bFPSMinValue && fps < Position.Instance.testItem.bFPSMaxValue)
                                        {
                                            bRightResult = true;
                                        }
                                        else
                                        {
                                            bRightResult = false;
                                            rightSZErrorCode = ("FPS Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bFPSEN++;
                                            }
                                        }
                                        appendText("右工位：FPS测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("FPSPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bVoltageEN)
                                    {
                                        appendText("右工位电压为：" + bRightVoltageValue.ToString());
                                        if (bRightVoltageValue > Position.Instance.testItem.bVoltageMinValue && bRightVoltageValue < Position.Instance.testItem.bVoltageMaxValue)
                                        {
                                            bRightResult = true;
                                        }
                                        else
                                        {
                                            bRightResult = false;
                                            rightSZErrorCode = ("Voltage Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bVoltageEN++;
                                            }
                                        }
                                        appendText("右工位：电压测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("VoltagePicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bCurrentEN)
                                    {
                                        appendText("右工位电流为:" + bRightCurentValue.ToString());
                                        if (bRightCurentValue > Position.Instance.testItem.bCurrentMinValue && bRightCurentValue < Position.Instance.testItem.bCurrentMaxValue)
                                        {
                                            bRightResult = true;
                                        }
                                        else
                                        {
                                            bRightResult = false;
                                            rightSZErrorCode = ("Current Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bCurrentEN++;
                                            }
                                        }
                                        appendText("右工位：电流测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("CurrentPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bPowerEN)
                                    {
                                        double power = bRightCurentValue * bRightVoltageValue;
                                        appendText("右工位产品功率为：" + power.ToString());
                                        if (power > Position.Instance.testItem.bPowerMinValue && power < Position.Instance.testItem.bPowerMaxValue)
                                        {
                                            bRightResult = true;
                                        }
                                        else
                                        {
                                            bRightResult = false;
                                            rightSZErrorCode = ("Power Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bPowerEN++;
                                            }
                                        }
                                        appendText("右工位：功率测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("PowPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bChangeViewEN)
                                    {
                                        bRightResult = sideBChangeViewTest();
                                        Thread.Sleep(500);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("ChangeView Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bChangeViewEN++;
                                            }
                                        }
                                        appendText("右工位：切换测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("ChangeViewPicBoxB", bRightResult);
                                    }
                                    if ((bRightTotalResult || Position.Instance.testItem.bAllTestEN) && Position.Instance.testItem.bMTFEN)
                                    {
                                        StringBuilder buf = new StringBuilder(6072);
                                        bRightResult = ImageSitB.DesayTestChart_SFR(buf);
                                        Thread.Sleep(500);
                                        string strout = buf.ToString();
                                        appendText(strout);
                                        if (!bRightResult)
                                        {
                                            rightSZErrorCode = ("MTF Error");
                                            if (bRightTotalResult)
                                            {
                                                Position.Instance.ItemCount1.bMTFEN++;
                                            }
                                        }
                                        ImageSitB.StopCamera();
                                        appendText("右工位：MTF测试结束");
                                        bRightTotalResult = bRightTotalResult & bRightResult;
                                        changePicBoxImg("MTFPicBoxB", bRightResult);
                                    }
                                    appendText("右工位：图卡测试结束");
                                    rightTestProcess = 300;
                                }
                            }
                            break;
                        case 300://右通道平移气缸缩回
                            if (cyRightSidesway.IsAction())
                            {
                                IoPoints.IDO2.Value = false;
                                IoPoints.IDO3.Value = true;
                                rightTestProcess = 310;
                            }
                            break;
                        case 310://右通道平移气缸缩回感应
                            if (IoPoints.IDI0.Value)
                            {
                                IsMTFWorking = false;
                                rightTestProcess = 320;
                            }
                            break;
                        case 320://Y2回待命位
                            if (axisY2.IsAction() && !IsWhiteWorking)
                            {
                                frmMotion.Y2.MoveAbs(frmMotion.RightStandbyPos);
                                rightTestProcess = 330;
                            }
                            break;
                        case 330://回待命位
                            if (frmMotion.Y2.IsPosComplete(frmMotion.RightStandbyPos))
                            {
                                rightTestProcess = 340;
                            }
                            break;
                        case 340:
                            if (rightSZErrorCode == "PASS")
                            {
                                IoPoints.IDO6.Value = true;
                                IoPoints.IDO7.Value = false;
                                Config.Instance.RightOKCount++;
                                productCount(true);
                            }
                            else
                            {
                                IoPoints.IDO6.Value = false;
                                IoPoints.IDO7.Value = true;
                                Config.Instance.RightNGCount++;
                                productCount(false);
                            }

                            periodEnd(1);
                            ImageSitB.StopCamera();
                            appendText(string.Format("产品条码：{0},测试结果：{1}", rightBarcode, bRightTotalResult));
                            frmhome.RightRes = bRightTotalResult ? "OK" : "NG";
                            HaveDropNG = bRightTotalResult ? HaveDropNG : false;
                            rightWorkingFlag = false;
                            rightTestProcess = -1;
                            rightBarCodeLabel.Text = "";
                            break;
                    }
                }
            }
        }

        private bool sideAChangeViewTest()
        {
            int step = 0;
            bool testResult = true;
            while (step != 40)
            {
                switch (step)
                {
                    case 0:
                        //wide View 测试
                        IoPoints.IDO12.Value = true;
                        IoPoints.IDO13.Value = true;
                        Thread.Sleep(500);
                        StringBuilder buf1 = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                        int result0 = ImageSitA.DesayTestChart_ChageView(0, buf1);
                        if (result0 != 0)
                        {
                            testResult = false;
                        }
                        step = 10;
                        break;
                    case 10:
                        //topDown view 测试
                        IoPoints.IDO12.Value = true;
                        IoPoints.IDO13.Value = false;
                        Thread.Sleep(500);
                        StringBuilder buf2 = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                        int result1 = ImageSitA.DesayTestChart_ChageView(1, buf2);
                        if (result1 != 1)
                        {
                            testResult = false;
                        }
                        step = 20;
                        break;
                    case 20:
                        //Normal view 测试
                        IoPoints.IDO12.Value = false;
                        IoPoints.IDO13.Value = true;
                        Thread.Sleep(500);
                        StringBuilder buf3 = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                        int result2 = ImageSitA.DesayTestChart_ChageView(2, buf3);
                        if (result2 != 2)
                        {
                            testResult = false;
                        }
                        step = 30;
                        break;
                    case 30:
                        IoPoints.IDO12.Value = false;
                        IoPoints.IDO13.Value = false;
                        step = 40;
                        break;
                }
            }
            return testResult;
        }

        private bool sideBChangeViewTest()
        {
            int step = 0;
            bool testResult = true;
            while (step != 40)
            {
                switch (step)
                {
                    case 0:
                        //wide View 测试
                        IoPoints.IDO14.Value = true;
                        IoPoints.IDO15.Value = true;
                        Thread.Sleep(500);
                        StringBuilder buf1 = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                        int result0 = ImageSitB.DesayTestChart_ChageView(0, buf1);
                        if (result0 != 0)
                        {
                            testResult = false;
                        }
                        step = 10;
                        break;
                    case 10:
                        //topDown view 测试
                        IoPoints.IDO14.Value = true;
                        IoPoints.IDO15.Value = false;
                        Thread.Sleep(500);
                        StringBuilder buf2 = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                        int result1 = ImageSitB.DesayTestChart_ChageView(1, buf2);
                        if (result1 != 1)
                        {
                            testResult = false;
                        }
                        step = 20;
                        break;
                    case 20:
                        //Normal view 测试
                        IoPoints.IDO14.Value = false;
                        IoPoints.IDO15.Value = true;
                        Thread.Sleep(500);
                        StringBuilder buf3 = new StringBuilder(6072);//指定的buf大小必须大于传入的字符长度
                        int result2 = ImageSitB.DesayTestChart_ChageView(2, buf3);
                        if (result2 != 2)
                        {
                            testResult = false;
                        }
                        step = 30;
                        break;
                    case 30:
                        IoPoints.IDO14.Value = false;
                        IoPoints.IDO15.Value = false;
                        step = 40;
                        break;
                }
            }
            return testResult;
        }

        private void reset()
        {
            if (Marking.ManualMode)
            {
                if (Config.Instance.IsLeftWork || Config.Instance.IsRightWork)
                {
                    currentMachineStatus = MachineStatus.Status.复位ing;
                    appendText(string.Format("{0}。。！", currentMachineStatus));
                    resetingProcess = 0;
                    Thread resetingThread = new Thread(new ThreadStart(reseting));
                    resetingThread.IsBackground = true;
                    resetingThread.Start();

                    while (currentMachineStatus == MachineStatus.Status.复位ing)
                    {
                        //复位按钮灯闪烁
                        IoPoints.TDO6.Value = !IoPoints.TDO6.Value;
                        Thread.Sleep(500);
                    }

                    if (currentMachineStatus == MachineStatus.Status.复位完成)
                    {
                        IoPoints.TDO6.Value = false;
                        HaveReseted = true;//复位
                        isReseting = false;
                    }
                    else
                    {
                        resetStart = false;
                        isReseting = false;
                        appendText("设备复位流程中断，请检查！");
                    }
                }
                else
                {
                    isReseting = false;
                    appendText("请启用工位后，重新复位！");
                }
            }
            else
            {
                isReseting = false;
                appendText("设备在手动模式下，才能复位！");
            }
        }
        /// <summary>
        /// 复位循环标志位
        /// </summary>
        private bool resetStart = false;
        private void reseting()
        {
            resetStart = true;
            while (!isEmgStoping && resetStart)
            {
                try
                {
                    switch (resetingProcess)
                    {
                        #region---Clear Flag---
                        case 0:
                            if (!IoPoints.IDI6.Value || !IoPoints.IDI8.Value)
                            {
                                appendText(string.Format("顶盖检测：左顶盖{0}，右顶盖{1}。。！", IoPoints.IDI6.Value ? "正常" : "异常", IoPoints.IDI8.Value ? "正常" : "异常"));
                                resetingProcess = 500;
                            }
                            else
                            {
                                appendText("复位数据与标志。。。");
                                isLeftScan = false;
                                isRightScan = false;
                                isLeftRun = true;
                                leftWorkingFlag = false;
                                rightWorkingFlag = false;
                                frmhome.LeftRes = "";
                                frmhome.RightRes = "";
                                scanBarDis = "";
                                leftBarcode = "";
                                rightBarcode = "";
                                resetingProcess = 10;
                                frmhome.ResetAndStopCamera();
                            }
                            break;
                        #endregion

                        #region---Reset Device---
                        case 10:
                            appendText("复位电机ON状态。。。");
                            frmMotion.Y1.IsServoON = true;
                            frmMotion.Y2.IsServoON = true;
                            frmMotion.Z.IsServoON = true;
                            frmMotion.Y1.DecelStop();
                            frmMotion.Y2.DecelStop();
                            frmMotion.Z.DecelStop();
                            if (!IoPoints.TDO7.Value)
                            {
                                IoPoints.TDO7.Value = true;//Z轴放开刹车
                            }
                            resetingProcess = 20;
                            break;

                        case 20:
                            appendText("复位输出状态。。。");
                            IoPoints.TDO7.Value = true;//Z轴放开刹车
                            IoPoints.TDO13.Value = true;//复位白场升降气缸上
                            IoPoints.TDO12.Value = false;
                            IoPoints.TDO8.Value = true; //复位左通道黑场气缸上
                            IoPoints.TDO9.Value = false;
                            IoPoints.TDO11.Value = true;//复位右通道黑场气缸上
                            IoPoints.TDO12.Value = false;
                            //新增信号指示灯关闭
                            Thread.Sleep(2000);
                            IoPoints.IDO4.Value = false;
                            IoPoints.IDO5.Value = false;
                            IoPoints.IDO6.Value = false;
                            IoPoints.IDO7.Value = false;
                            resetingProcess = 21;
                            break;
                        case 21:
                            if (IoPoints.TDI12.Value)
                            {
                                IoPoints.IDO0.Value = true;//复位左通道平移气缸缩回
                                IoPoints.IDO1.Value = false;
                                IoPoints.IDO2.Value = false;
                                IoPoints.IDO3.Value = true;//复位右通道平移气缸缩回

                                resetingProcess = 22;
                            }
                            else
                            {
                                appendText("复位失败,请检查白场气缸上感应!");
                                resetingProcess = 500;
                            }

                            break;
                        case 22:
                            if (IoPoints.TDI8.Value)
                            {
                                resetingProcess = 23;
                            }
                            else
                            {
                                appendText("复位失败,请检查左黑场气缸上感应!");
                                resetingProcess = 500;
                            }
                            break;
                        case 23:
                            if (IoPoints.TDI10.Value)
                            {
                                resetingProcess = 25;
                            }
                            else
                            {
                                appendText("复位失败,请检查右黑场气缸上感应!");
                                resetingProcess = 500;
                            }
                            break;
                        case 25:
                            if (IoPoints.TDI14.Value && IoPoints.IDI0.Value && !IoPoints.TDI15.Value && !IoPoints.IDI2.Value)
                            {
                                resetingProcess = 30;
                            }
                            break;
                        case 30:
                            if (axisZ.IsAction())
                            {
                                appendText("复位Z轴回原点。。。");

                                frmMotion.Z.BackHome();
                                Thread.Sleep(500);
                                resetingProcess = 40;
                            }
                            break;
                        case 40:
                            if (frmMotion.Z.IsOrg)
                            {
                                resetingProcess = 50;
                            }
                            break;
                        case 50:
                            //if (axisZ.IsAction())
                            //{
                            //    appendText("复位Z轴到待命工作位置。。。");
                            //    frmMotion.Z.MoveAbs(frmMotion.ZStandbyPos);
                            //    resetingProcess = 60;
                            //}
                            resetingProcess = 60;
                            break;
                        case 60:
                            if (axisY1.IsAction())
                            {
                                resetingProcess = 70;
                            }
                            else
                            {
                                appendText("左工位Y轴状态异常！");
                                resetingProcess = 500;
                            }
                            break;
                        case 70:
                            if (axisY2.IsAction())
                            {
                                resetingProcess = 80;
                            }
                            else
                            {
                                appendText("右工位Y轴状态异常！");
                                resetingProcess = 500;
                            }
                            break;
                        case 80:
                            if (Config.Instance.IsLeftWork)
                            {
                                appendText("复位Y1轴回原点。。。");
                                frmMotion.Y1.BackHome();
                            }
                            if (Config.Instance.IsRightWork)
                            {
                                appendText("复位Y2轴回原点。。。");
                                frmMotion.Y2.BackHome();
                            }
                            Thread.Sleep(500);
                            resetingProcess = 90;
                            break;
                        case 90:
                            if ((!Config.Instance.IsLeftWork || frmMotion.Y1.IsMotionless) && (!Config.Instance.IsRightWork || frmMotion.Y2.IsMotionless))
                            {
                                resetingProcess = 100;
                            }
                            break;
                        case 100:
                            if (axisY1.IsAction())
                            {
                                resetingProcess = 110;
                            }
                            else
                            {
                                appendText("左工位Y轴状态异常！");
                                resetingProcess = 500;
                            }
                            break;
                        case 110:
                            if (axisY2.IsAction())
                            {
                                resetingProcess = 120;
                            }
                            else
                            {
                                appendText("右工位Y轴状态异常！");
                                resetingProcess = 500;
                            }
                            break;
                        case 120:
                            if (Config.Instance.IsLeftWork)
                            {
                                appendText("复位Y1轴到待命工作位置。。。");
                                frmMotion.Y1.MoveAbs(frmMotion.LeftStandbyPos);
                            }
                            if (Config.Instance.IsRightWork)
                            {
                                appendText("复位Y2轴到待命工作位置。。。");
                                frmMotion.Y2.MoveAbs(frmMotion.RightStandbyPos);
                            }
                            Thread.Sleep(500);
                            resetingProcess = 130;
                            break;
                        case 130:
                            if ((!Config.Instance.IsLeftWork || frmMotion.Y1.IsPosComplete(frmMotion.LeftStandbyPos))
                                &&
                                (!Config.Instance.IsRightWork || frmMotion.Y2.IsPosComplete(frmMotion.RightStandbyPos)))
                            {
                                frmMotion.LoadSoftwareLimit();
                                BeginInvoke(new Action(() =>
                                {
                                    tbtnStart.Enabled = false;  //复位后禁用启动按键
                                    currentMachineStatus = MachineStatus.Status.复位完成;
                                    appendText(string.Format("{0}。。！", currentMachineStatus));
                                }));
                                resetStart = false;
                                resetingProcess = 0;
                                IsWhiteWorking = false;
                                IsMTFWorking = false;
                            }
                            break;
                        case 500:
                            resetStart = false;
                            resetingProcess = 0;
                            currentMachineStatus = MachineStatus.Status.停止;
                            MessageBox.Show("设备复位流程中断，请检查！", "提示");
                            break;
                            #endregion
                    }
                }
                catch (Exception e)
                {
                    resetStart = false;
                    resetingProcess = 0;
                    appendText(string.Format("(请检查后确定而继续复位)设备复位流程异常！:{0}", e.ToString()));
                }
                Thread.Sleep(50);
            }
        }

        private void emgStop()
        {
            #region 主要用硬件输入急停（因timer1计时器出错时也需要急停处理，则加入此三行代码急停控制）
            frmMotion.Y1.EmergencyStop();
            frmMotion.Y2.EmergencyStop();
            frmMotion.Z.EmergencyStop();
            #endregion
            online = false;//急停
            tbtnStart.Enabled = false; //急停中禁用启动按键
            tbtnStop.Enabled = false;  //急停中禁用停止按键
            tbtnPause.Enabled = false; //急停中禁用暂停按键
            tbtnReset.Enabled = false; //急停时禁用复位按键
            HaveReseted = false;//急停
            IoPoints.TDO5.Value = false;
            currentMachineStatus = MachineStatus.Status.急停;
        }

        #endregion

        #region 机种操作
        private void NewAddProduct(string str)
        {
            string MotionPath = frmProductList.CreateFilePath(str + ".xml", "Motion");
            string CurrentMotionPath = frmProductList.CreateFilePath(Config.Instance.CurrentProduct + ".xml", "Motion");
            Common.FileCreate(MotionPath);
            DataSet ds = new DataSet();
            ds.ReadXml(CurrentMotionPath);
            ds.WriteXml(MotionPath);
            string TestItemPath = frmProductList.CreateFilePath(str + ".xml", "Test");
            SerializerManager<Position>.Instance.Save(TestItemPath, Position.Instance);
        }

        private void DeleteProduct(string str)
        {
            Common.FileDelete(frmProductList.CreateFilePath(str + ".xml", "Motion"));
            Common.FileDelete(frmProductList.CreateFilePath(str + ".xml", "Test"));
        }

        private void updateProduct(string product)
        {
            Invoke(new Action(() =>
            {
                if (product != "")
                {
                    try
                    {
                        HaveChangeProduct = true;
                        frmhome.ProductModel = product;
                        string TestItemPath = frmProductList.CreateFilePath(product + ".xml", "Test");
                        Position.Instance = SerializerManager<Position>.Instance.Load(TestItemPath);
                        frmSystem.loadTestItemConfigParam();
                        frmMotion.LoadParameter(product);
                        frmhome.UpdateUIData();
                    }
                    catch
                    {
                        appendText("机种切换失败，请重启软件");
                    }
                    
                }
            }));
        }

        #endregion

        #region 光源、电压操作
        /// <summary>
        /// 白场光源控制
        /// </summary>
        /// <param name="isOn">打开/关闭</param>
        private void setWhiteLightControlOnOrOff(bool isOn)
        {
            int Step = 0;
            while (!isChangeWhiteControlSuccess)
            {
                switch (Step)
                {
                    case 0:
                        if (isOn)
                        {
                            ComPort6.BeginTrigger2("55 AA 01 D0 07 00 00 28");//开启的是亮度为2000
                        }
                        else
                        {
                            ComPort6.BeginTrigger2("55 AA 01 00 00 00 00 FF");//关闭
                        }
                        Step = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource6Completed)
                        {
                            Marking.isLightSource6Completed = false;
                            if (Marking.LightSource6Data.Count() == 8)
                            {
                                byte[] receivedData = new byte[2];
                                receivedData[0] = Convert.ToByte(Marking.LightSource6Data[0], 16);
                                receivedData[1] = Convert.ToByte(Marking.LightSource6Data[1], 16);
                                if (receivedData[0] == 0xAA && receivedData[1] == 0x55)
                                {
                                    isChangeWhiteControlSuccess = true;
                                    Step = 20;
                                }
                            }
                            else
                            {
                                appendText("白场光源长度校核数据错误");
                                Step = 0;
                            }
                        }
                        break;
                    default: break;
                }
            }

        }

        /// <summary>
        /// 切换为红外光源
        /// </summary>
        private void changeLightTO940nm()
        {

            int Step1 = 0;
            int Step2 = 0;
            int Step3 = 0;
            int Step4 = 0;
            int Step5 = 0;

            while (!isChangeLightSuccess)
            {
                Thread.Sleep(100);
                switch (Step1)
                {
                    case 0:
                        ComPort1.BeginTrigger("55 AA 03 04 00 00 00");
                        Step1 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource1Completed)
                        {
                            Marking.isLightSource1Completed = false;
                            if (Marking.LightSource1Data.Count() == 32)
                            {
                                Step1 = 20;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step1 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort1.BeginTrigger("55 AA 02 02 00 00 00");
                        Step1 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource1Completed)
                        {
                            Marking.isLightSource1Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource1Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource1Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step1 = 50;

                            }
                            else
                            {
                                appendText("光源通道校核数据错误");
                                Step1 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step2)
                {
                    case 0:
                        ComPort2.BeginTrigger("55 AA 03 04 00 00 00");
                        Step2 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource2Completed)
                        {
                            Marking.isLightSource2Completed = false;
                            if (Marking.LightSource2Data.Count() == 32)
                            {
                                Step2 = 20;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step2 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort2.BeginTrigger("55 AA 02 02 00 00 00");
                        Step2 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource2Completed)
                        {
                            Marking.isLightSource2Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource2Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource2Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step2 = 50;

                            }
                            else
                            {
                                appendText("光源通道校核数据错误");
                                Step2 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step3)
                {
                    case 0:
                        ComPort3.BeginTrigger("55 AA 03 04 00 00 00");
                        Step3 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource3Completed)
                        {
                            Marking.isLightSource3Completed = false;
                            if (Marking.LightSource3Data.Count() == 32)
                            {
                                Step3 = 20;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step3 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort3.BeginTrigger("55 AA 02 02 00 00 00");
                        Step3 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource3Completed)
                        {
                            Marking.isLightSource3Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource3Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource3Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step3 = 50;

                            }
                            else
                            {
                                appendText("光源通道校核数据错误");
                                Step3 = 0;
                            }
                        }
                        break;
                    default: break;
                }
                switch (Step4)
                {
                    case 0:
                        ComPort4.BeginTrigger("55 AA 03 04 00 00 00");
                        Step4 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource4Completed)
                        {
                            Marking.isLightSource4Completed = false;
                            if (Marking.LightSource4Data.Count() == 32)
                            {
                                Step4 = 20;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step4 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort4.BeginTrigger("55 AA 02 02 00 00 00");
                        Step4 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource4Completed)
                        {
                            Marking.isLightSource4Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource4Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource4Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step4 = 50;

                            }
                            else
                            {
                                appendText("光源通道校核数据错误");
                                Step4 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step5)
                {
                    case 0:
                        ComPort5.BeginTrigger("55 AA 03 04 00 00 00");
                        Step5 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource5Completed)
                        {
                            Marking.isLightSource5Completed = false;
                            if (Marking.LightSource5Data.Count() == 32)
                            {
                                Step5 = 20;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step5 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort5.BeginTrigger("55 AA 02 02 00 00 00");
                        Step5 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource5Completed)
                        {
                            Marking.isLightSource5Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource5Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource5Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step5 = 50;

                            }
                            else
                            {
                                appendText("光源通道校核数据错误");
                                Step5 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                if (Step1 == 50 && Step2 == 50 && Step3 == 50 && Step4 == 50 && Step5 == 50)
                {
                    isChangeLightSuccess = true;

                }
                else
                {
                    // appendText("光源切换失败");
                }
            }

        }

        /// <summary>
        /// 切换为可见光源
        /// </summary>
        private void changeLightTO3100and6500()
        {
            //if (InvokeRequired)
            //{
            //    Invoke(new Action<string>(AppendText));
            //}
            //else
            //{
            int Step1 = 0;
            int Step2 = 0;
            int Step3 = 0;
            int Step4 = 0;
            int Step5 = 0;

            while (!isChangeLightSuccess)
            {
                Thread.Sleep(100);
                switch (Step1)
                {
                    case 0:
                        ComPort1.BeginTrigger("55 AA 03 04 00 00 00");
                        Step1 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource1Completed)
                        {
                            Marking.isLightSource1Completed = false;
                            if (Marking.LightSource1Data.Count() == 32)
                            {
                                Step1 = 20;
                            }
                            else
                            {
                                appendText("ComPort1-32长度校核数据错误");
                                Step1 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort1.BeginTrigger("55 AA 02 03 00 00 00");
                        Step1 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource1Completed)
                        {
                            Marking.isLightSource1Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource1Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource1Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step1 = 50;
                            }
                            else
                            {
                                appendText("ComPort1光源通道校核数据错误");
                                Step1 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step2)
                {
                    case 0:
                        ComPort2.BeginTrigger("55 AA 03 04 00 00 00");
                        Step2 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource2Completed)
                        {
                            Marking.isLightSource2Completed = false;
                            if (Marking.LightSource2Data.Count() == 32)
                            {
                                Step2 = 20;
                            }
                            else
                            {
                                appendText("ComPort2-32长度校核数据错误");
                                Step2 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort2.BeginTrigger("55 AA 02 03 00 00 00");
                        Step2 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource2Completed)
                        {
                            Marking.isLightSource2Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource2Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource2Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step2 = 50;
                            }
                            else
                            {
                                appendText("ComPort2光源通道校核数据错误");
                                Step2 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step3)
                {
                    case 0:
                        ComPort3.BeginTrigger("55 AA 03 04 00 00 00");
                        Step3 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource3Completed)
                        {
                            Marking.isLightSource3Completed = false;
                            if (Marking.LightSource3Data.Count() == 32)
                            {
                                Step3 = 20;
                            }
                            else
                            {
                                appendText("ComPort3-32长度校核数据错误");
                                Step3 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort3.BeginTrigger("55 AA 02 03 00 00 00");
                        Step3 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource3Completed)
                        {
                            Marking.isLightSource3Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource3Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource3Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step3 = 50;
                            }
                            else
                            {
                                appendText("ComPort3光源通道校核数据错误");
                                Step3 = 0;
                            }
                        }
                        break;
                    default: break;
                }
                switch (Step4)
                {
                    case 0:
                        ComPort4.BeginTrigger("55 AA 03 04 00 00 00");
                        Step4 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource4Completed)
                        {
                            Marking.isLightSource4Completed = false;
                            if (Marking.LightSource4Data.Count() == 32)
                            {
                                Step4 = 20;
                            }
                            else
                            {
                                appendText("ComPort4-32长度校核数据错误");
                                Step4 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort4.BeginTrigger("55 AA 02 03 00 00 00");
                        Step4 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource4Completed)
                        {
                            Marking.isLightSource4Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource4Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource4Data[6], 16);

                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step4 = 50;
                            }
                            else
                            {
                                appendText("ComPort4-光源通道校核数据错误");
                                Step4 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step5)
                {
                    case 0:
                        ComPort5.BeginTrigger("55 AA 03 04 00 00 00");
                        Step5 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource5Completed)
                        {
                            Marking.isLightSource5Completed = false;
                            if (Marking.LightSource5Data.Count() == 32)
                            {
                                Step5 = 20;
                            }
                            else
                            {
                                appendText("ComPort5-32长度校核数据错误");
                                Step5 = 0;
                            }
                        }
                        break;
                    case 20:
                        ComPort5.BeginTrigger("55 AA 02 03 00 00 00");
                        Step5 = 30;
                        break;
                    case 30:
                        if (Marking.isLightSource5Completed)
                        {
                            Marking.isLightSource5Completed = false;
                            byte[] receivedData = new byte[2];
                            receivedData[0] = Convert.ToByte(Marking.LightSource5Data[0], 16);
                            receivedData[1] = Convert.ToByte(Marking.LightSource5Data[6], 16);
                            if (receivedData[0] == 0xAA && receivedData[1] == 0x01)
                            {
                                Step5 = 50;
                            }
                            else
                            {
                                appendText("ComPort5光源通道校核数据错误");
                                Step5 = 0;
                            }
                        }
                        break;
                    default: break;
                }
                if (Step1 == 50 && Step2 == 50 && Step3 == 50 && Step4 == 50 && Step5 == 50)
                {
                    isChangeLightSuccess = true;
                }
            }

        }

        /// <summary>
        /// 关闭光源
        /// </summary>
        private void closeVisibleOrIRLight()
        {
            int Step1 = 0;
            int Step2 = 0;
            int Step3 = 0;
            int Step4 = 0;
            int Step5 = 0;
            while (!isCloseVisibleLight)
            {
                Thread.Sleep(100);
                switch (Step1)
                {
                    case 0:
                        ComPort1.BeginTrigger("55 AA 03 04 00 00 00");
                        Step1 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource1Completed)
                        {
                            Marking.isLightSource1Completed = false;
                            if (Marking.LightSource1Data.Count() == 32)
                            {
                                Step1 = 50;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step1 = 0;
                            }
                        }
                        break;
                    default: break;
                }

                switch (Step2)
                {
                    case 0:
                        ComPort2.BeginTrigger("55 AA 03 04 00 00 00");
                        Step2 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource2Completed)
                        {
                            Marking.isLightSource2Completed = false;
                            if (Marking.LightSource2Data.Count() == 32)
                            {
                                Step2 = 50;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step2 = 0;
                            }
                        }
                        break;

                    default: break;
                }

                switch (Step3)
                {
                    case 0:
                        ComPort3.BeginTrigger("55 AA 03 04 00 00 00");
                        Step3 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource3Completed)
                        {
                            Marking.isLightSource3Completed = false;
                            if (Marking.LightSource3Data.Count() == 32)
                            {
                                Step3 = 50;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step3 = 0;
                            }
                        }
                        break;
                    default: break;
                }
                switch (Step4)
                {
                    case 0:
                        ComPort4.BeginTrigger("55 AA 03 04 00 00 00");
                        Step4 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource4Completed)
                        {
                            Marking.isLightSource4Completed = false;
                            if (Marking.LightSource4Data.Count() == 32)
                            {
                                Step4 = 50;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step4 = 0;
                            }
                        }
                        break;

                    default: break;
                }

                switch (Step5)
                {
                    case 0:
                        ComPort5.BeginTrigger("55 AA 03 04 00 00 00");
                        Step5 = 10;
                        break;
                    case 10:
                        if (Marking.isLightSource5Completed)
                        {
                            Marking.isLightSource5Completed = false;
                            if (Marking.LightSource5Data.Count() == 32)
                            {
                                Step5 = 50;
                            }
                            else
                            {
                                appendText("32长度校核数据错误");
                                Step5 = 0;
                            }
                        }
                        break;

                    default: break;
                }

                if (Step1 == 50 && Step2 == 50 && Step3 == 50 && Step4 == 50 && Step5 == 50)
                {
                    isCloseVisibleLight = true;
                    Thread.Sleep(3000);
                }
                else
                {
                    // appendText("光源切换失败");
                }
            }

        }

        /// <summary>
        /// Byte转16进制字符串
        /// </summary>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }

        public double getCurrentAValueWithStr(string str)
        {
            int dataNum = System.Convert.ToInt32(str, 16);
            float temp = (float)dataNum / 10000;
            Double currentA = (Double)temp * 5 * 1000;
            currentA = Math.Round(currentA, 2);
            return currentA;
        }

        public double getVoltageValueWithStr(string str)
        {
            int dataNum = System.Convert.ToInt32(str, 16);
            float temp = (float)dataNum / 10000;
            Double voltageA = (Double)temp * 60;
            voltageA = Math.Round(voltageA, 2);
            return voltageA;
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            try
            {
                #region-----Machine Status Display-----   
                label1.Text = "左流程：" + leftTestProcess.ToString();
                label3.Text = "右流程：" + rightTestProcess.ToString();
                Color color = Color.Red;
                switch (currentMachineStatus)
                {
                    case MachineStatus.Status.运行ing:
                        color = Color.Green;
                        break;
                    case MachineStatus.Status.复位ing:
                        color = Color.Purple;
                        break;
                    case MachineStatus.Status.复位完成:
                    case MachineStatus.Status.暂停:
                    case MachineStatus.Status.停止:
                        color = Color.Blue;
                        break;
                    case MachineStatus.Status.故障报警:
                        color = Color.Red;
                        break;
                    case MachineStatus.Status.急停:
                        color = Color.Red;
                        break;
                }
                设备状态toolStripStatusLabel1.Text = currentMachineStatus.ToString();
                设备状态toolStripStatusLabel1.ForeColor = color;

                tbtnReset.Text = Marking.ManualMode ? ("复位") : (isAlarm ? "报警清除" : "复位");
                MachineStatus.NewStatus = currentMachineStatus;

                if (Config.Instance.IsLeftWork && isLeftRun && (currentMachineStatus == MachineStatus.Status.运行ing))
                {
                    leftTestStatusLabel.BackColor = Color.Green;
                    rightTestStatusLabel.BackColor = Color.Gray;
                }
                if (Config.Instance.IsRightWork && !isLeftRun && (currentMachineStatus == MachineStatus.Status.运行ing))
                {
                    leftTestStatusLabel.BackColor = Color.Gray;
                    rightTestStatusLabel.BackColor = Color.Green;
                }

                #endregion

                #region-----Monitor ExternalButton-----              
                //停止键
                if (tbtnStop.Enabled && IoPoints.TDI0.Value)
                {
                    tbtnStop_Click(null, null);
                }
                //暂停键
                if (tbtnPause.Enabled && IoPoints.TDI1.Value)
                {
                    tbtnPause_Click(null, null);
                }
                //复位键
                if (tbtnReset.Enabled && IoPoints.TDI2.Value && !isReseting)
                {
                    new Action(() =>
                    {
                        int count = 0;
                        bool Timing = true; //计时
                        while (tbtnReset.Enabled && Timing)
                        {
                            if (!isAlarm)
                            {
                                Thread.Sleep(100);
                                ++count;
                                if (count >= 10 || !IoPoints.TDI2.Value)
                                { Timing = false; }
                            }
                            else
                            {
                                Timing = false;
                            }
                        }
                        if (count >= 10 && !isReseting)
                        {
                            isReseting = true;
                            reset();
                        }
                    }).BeginInvoke(null, null);
                }
                //急停键
                if (!isEmgStoping && !IoPoints.TDI3.Value)
                {
                    Thread.Sleep(200);
                    if (!IoPoints.TDI3.Value)
                    {
                        isEmgStoping = true;//急停键按下
                        emgStop();
                    }
                }
                else if (isEmgStoping && IoPoints.TDI3.Value) //急停按键弹起
                {
                    设备状态toolStripStatusLabel1.Text = "停止";
                    currentMachineStatus = MachineStatus.Status.停止;
                    isEmgStoping = false;//急停键弹起
                    tbtnReset.Enabled = true;//急停复位启用复位按键
                }

                if (设备状态toolStripStatusLabel1.Text == "运行ing")
                {
                    IoPoints.TDO14.Value = false;//双色灯亮绿灯
                    IoPoints.TDO15.Value = true;
                }
                else
                {
                    IoPoints.TDO14.Value = false;//双色灯关闭
                    IoPoints.TDO15.Value = false;
                }
                //光栅
                if (Config.Instance.IsGuanShan && online && !isEmgStoping && !isRasterStoping) //光栅
                {
                    bool isLeftOperate = frmMotion.Y1.IsPosComplete(frmMotion.LeftStandbyPos);
                    bool isRightOperate = frmMotion.Y2.IsPosComplete(frmMotion.RightStandbyPos);
                    if (!IoPoints.IDI10.Value && !isLeftOperate && !isRightOperate)
                    {
                        isRasterStoping = true; //光栅遮挡
                        emgStop();
                    }
                }
                else if (isRasterStoping && IoPoints.IDI10.Value)
                {
                    设备状态toolStripStatusLabel1.Text = "停止";
                    currentMachineStatus = MachineStatus.Status.停止;
                    isRasterStoping = false; //光栅正常
                    tbtnReset.Enabled = true;//光栅急停-复位启用复位按键
                }

                //NG桶
                if (IoPoints.IDI7.Value || !Config.Instance.IsNGBox)
                {
                    HaveDropNG = true;
                }
                #endregion

                #region---- Light Control------
                if (frmSystem.isChangeLightTo940nm && !frmSystem.isChangeLightTo3100nm)
                {
                    isChangeLightSuccess = false;
                    changeLightTO940nm();

                    frmSystem.isChangeLightTo940nm = false;
                }
                if (!frmSystem.isChangeLightTo940nm && frmSystem.isChangeLightTo3100nm)
                {
                    isChangeLightSuccess = false;
                    changeLightTO3100and6500();

                    frmSystem.isChangeLightTo3100nm = false;
                }

                if (frmSystem.isOpenWhiteLightControl && !frmSystem.isCloseWhiteLightControl)
                {
                    isChangeWhiteControlSuccess = false;
                    setWhiteLightControlOnOrOff(true);//打开白场光源
                }

                if (!frmSystem.isOpenWhiteLightControl && frmSystem.isCloseWhiteLightControl)
                {
                    isChangeWhiteControlSuccess = false;
                    setWhiteLightControlOnOrOff(false);//关闭白场光源
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (!isEmgStoping)
                {
                    emgStop();
                }
                appendText(string.Format("监视机制出现异常导致急停：{0}", ex.ToString()));
            }
            finally
            {
                timer1.Enabled = true;
            }
        }

        #endregion

    }
}
