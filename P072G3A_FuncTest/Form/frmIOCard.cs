using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;
using System.ToolKit;
using System.Threading;
using Motion.Interfaces;
using desay.ProductData;
using System.Toolkit.Helpers;

namespace P072G3A_FuncTest
{
    public partial class frmIOCard : Form
    {
        public static Common.void_StringDelegate AppendLog;
        public Unit CyLeftBlackUpDown;
        public Unit CyRightBlackUpDown;
        public Unit CyWhiteUpDown;
        public Unit CyLeftSidesway;
        public Unit CyRightSidesway;
        public bool IsManualMode = false;

        #region Parameter

        public int LeftBlackCYUpDelay { get { return int.Parse(txtLeftBlackCYUpDelay.Text); } }
        public int LeftBlackCYDownDelay { get { return int.Parse(txtLeftBlackCYDownDelay.Text); } }
        public int RightBlackCYUpDelay { get { return int.Parse(txtRightBlackCYUpDelay.Text); } }
        public int RightBlackCYDownDelay { get { return int.Parse(txtRightBlackCYDownDelay.Text); } }
        public int WhiteCYUpDelay { get { return int.Parse(txtWhiteCYUpDelay.Text); } }
        public int WhiteCYDownDelay { get { return int.Parse(txtWhiteCYDownDelay.Text); } }
        public int LeftSideswayCYReturnDelay { get { return int.Parse(txtLeftSideswayCYReturnDelay.Text); } }
        public int LeftSideswayCYReachDelay { get { return int.Parse(txtLeftSideswayCYReachDelay.Text); } }
        public int RightSideswayCYReturnDelay { get { return int.Parse(txtRightSideswayCYReturnDelay.Text); } }
        public int RightSideswayCYReachDelay { get { return int.Parse(txtRightSideswayCYReachDelay.Text); } }

        #endregion

        #region Public Call

        public IoPoint[] TDI = new IoPoint[]
            {
                IoPoints.TDI0,
                IoPoints.TDI1,
                IoPoints.TDI2,
                IoPoints.TDI3,
                IoPoints.TDI4,
                IoPoints.TDI5,
                IoPoints.TDI6,
                IoPoints.TDI7,
                IoPoints.TDI8,
                IoPoints.TDI9,
                IoPoints.TDI10,
                IoPoints.TDI11,
                IoPoints.TDI12,
                IoPoints.TDI13,
                IoPoints.TDI14,
                IoPoints.TDI15,
            };
        public IoPoint[] IDI = new IoPoint[]
            {
                IoPoints.IDI0,
                IoPoints.IDI1,
                IoPoints.IDI2,
                IoPoints.IDI3,
                IoPoints.IDI4,
                IoPoints.IDI5,
                IoPoints.IDI6,
                IoPoints.IDI7,
                IoPoints.IDI8,
                IoPoints.IDI9,
                IoPoints.IDI10,
                IoPoints.IDI11,
                IoPoints.IDI12,
                IoPoints.IDI13,
                IoPoints.IDI14,
                IoPoints.IDI15,
            };

        private Dictionary<string, Button> tDI = new Dictionary<string, Button>();
        private Dictionary<string, Button> iDI = new Dictionary<string, Button>();

        public List<Common.bool_bool_Delegate> TDO = new List<Common.bool_bool_Delegate>();      
        public List<Common.bool_bool_Delegate> IDO = new List<Common.bool_bool_Delegate>(); 
               
        private bool isButtonClick = false;

        #endregion

        public frmIOCard()
        {
            InitializeComponent();
        }

        private void frmIOCard_Load(object sender, EventArgs e)
        {
            //IO卡初始化
            IoPoints.DaskController.Initialize();

            LoadParameter();

            foreach (Control ctr in groupBTDI.Controls)
            {
                if (ctr is Button)
                { tDI.Add(ctr.Text, (Button)ctr); }
            }

            foreach (Control ctr in groupBIDI.Controls)
            {
                if (ctr is Button)
                { iDI.Add(ctr.Text, (Button)ctr); }
            }

            IoPoints.TDO7.Value = true;
        }

        private void frmIOCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            //释放IO卡
            IoPoints.DaskController.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            groupBTDO.Enabled = IsManualMode;
            groupBIDO.Enabled = IsManualMode;

            #region 输入控件显示
            for (int i = 0; i < TDI.Length; ++i)
            {
                tDI["I" + (i + 1)].Enabled = TDI[i].Value;
            }

            for (int i = 0; i < IDI.Length; ++i)
            {
                iDI["I" + i].Enabled = IDI[i].Value;
            }
            #endregion

            #region 输出控件显示(暂时不需要-备用)
            //for (int i = 0; i < TDO.Count; ++i)
            //{
            //    tDO["O" + (i + 1)].BackColor = TDO[i](null) ? Color.DarkGray : Color.Transparent;
            //}

            //for (int i = 0; i < IDO.Count; ++i)
            //{
            //    iDO["O" + i].BackColor = IDO[i](null) ? Color.DarkGray : Color.Transparent;
            //}
            #endregion

            timer1.Enabled = true;
        }

        #region 参数操作

        public void LoadParameter()
        {
            txtLeftBlackCYUpDelay.Text = Config.Instance.LeftBlackCYUpDelay;
            txtLeftBlackCYDownDelay.Text = Config.Instance.LeftBlackCYDownDelay;
            txtRightBlackCYUpDelay.Text = Config.Instance.RightBlackCYUpDelay;
            txtRightBlackCYDownDelay.Text = Config.Instance.RightBlackCYDownDelay;
            txtWhiteCYUpDelay.Text = Config.Instance.WhiteCYUpDelay;
            txtWhiteCYDownDelay.Text = Config.Instance.WhiteCYDownDelay;
            txtLeftSideswayCYReturnDelay.Text = Config.Instance.LeftSideswayCYReturnDelay;
            txtLeftSideswayCYReachDelay.Text = Config.Instance.LeftSideswayCYReachDelay;
            txtRightSideswayCYReturnDelay.Text = Config.Instance.RightSideswayCYReturnDelay;
            txtRightSideswayCYReachDelay.Text = Config.Instance.RightSideswayCYReachDelay;
        }

        private void btnParamSave_Click(object sender, EventArgs e)
        {
            Config.Instance.LeftBlackCYUpDelay = txtLeftBlackCYUpDelay.Text;
            Config.Instance.LeftBlackCYDownDelay = txtLeftBlackCYDownDelay.Text;
            Config.Instance.RightBlackCYUpDelay = txtRightBlackCYUpDelay.Text;
            Config.Instance.RightBlackCYDownDelay = txtRightBlackCYDownDelay.Text;
            Config.Instance.WhiteCYUpDelay = txtWhiteCYUpDelay.Text;
            Config.Instance.WhiteCYDownDelay = txtWhiteCYDownDelay.Text;
            Config.Instance.LeftSideswayCYReturnDelay = txtLeftSideswayCYReturnDelay.Text;
            Config.Instance.LeftSideswayCYReachDelay = txtLeftSideswayCYReachDelay.Text;
            Config.Instance.RightSideswayCYReturnDelay = txtRightSideswayCYReturnDelay.Text;
            Config.Instance.RightSideswayCYReachDelay = txtRightSideswayCYReachDelay.Text;
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);
            appendText("参数保存成功！");
        }

        private void appendText(string str)
        {
            AppendLog.Invoke(string.Format("<<IO卡界面>>:: {0}", str));
        }

        #endregion

        #region Private Method

        #region TDO1
        private bool tDO1(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    threeColorRedLight((bool)value);
                }).BeginInvoke(null,null);               
            }
            return IoPoints.TDO0.Value;
        }
        private void threeColorRedLight(bool b)
        {
            btn3ColorRedLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO0.Value = b;
        }
        #endregion
        #region TDO2
        private bool tDO2(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    threeColorYellowLight((bool)value);
                }).BeginInvoke(null,null);               
            }
            return IoPoints.TDO1.Value;
        }
        private void threeColorYellowLight(bool b)
        {
            btn3ColorYellowLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO1.Value = b;
        }
        #endregion
        #region TDO3
        private bool tDO3(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    threeColorGreenLight((bool)value);
                }).BeginInvoke(null,null);               
            }
            return IoPoints.TDO2.Value;
        }
        private void threeColorGreenLight(bool b)
        {
            btn3ColorGreenLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO2.Value = b;
        }
        #endregion
        #region TDO4
        private bool tDO4(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    buzzer((bool)value);
                }).BeginInvoke(null,null);               
            }
            return IoPoints.TDO3.Value;
        }
        private void buzzer(bool b)
        {
            if (!Config.Instance.IsSpeak)
            {
                btnBuzzer.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO3.Value = b;
            }
        }
        #endregion
        #region TDO5
        private bool tDO5(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    stopLight((bool)value);
                }).BeginInvoke(null,null);                
            }
            return IoPoints.TDO4.Value;
        }
        private void stopLight(bool b)
        {
            btnPauseLight.BackColor =
            btnResetLight.BackColor = Color.Transparent;
            btnStopLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO5.Value =
            IoPoints.TDO6.Value = false;
            IoPoints.TDO4.Value = b;
        }
        #endregion
        #region TDO6
        private bool tDO6(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    pauseLight((bool)value);
                }).BeginInvoke(null,null);                
            }
            return IoPoints.TDO5.Value;
        }
        private void pauseLight(bool b)
        {
            btnStopLight.BackColor =
            btnResetLight.BackColor = Color.Transparent;
            btnPauseLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO4.Value =
            IoPoints.TDO6.Value = false;
            IoPoints.TDO5.Value = b;
        }
        #endregion
        #region TDO7
        private bool tDO7(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    resetLight((bool)value);
                }).BeginInvoke(null,null);               
            }
            return IoPoints.TDO6.Value;
        }
        private void resetLight(bool b)
        {
            btnStopLight.BackColor =
            btnPauseLight.BackColor = Color.Transparent;
            btnResetLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO4.Value =
            IoPoints.TDO5.Value = false;
            IoPoints.TDO6.Value = b;
        }
        #endregion
        #region TDO8
        private bool tDO8(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    axisNotBrakeZ((bool)value);
                }).BeginInvoke(null,null);                
            }
            return IoPoints.TDO7.Value;
        }
        private void axisNotBrakeZ(bool b)
        {
            btnAxisNotBrakeZ.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO7.Value = b;
        }
        #endregion
        #region TDO9
        private bool tDO9(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    leftBlackUpDownCYUp((bool)value);
                }).BeginInvoke(null,null);                
            }
            return IoPoints.TDO8.Value;
        }
        private void leftBlackUpDownCYUp(bool b)
        {
            if (CyLeftBlackUpDown.IsAction())
            {
                btnLeftSideswayCYReach.BackColor = Color.Transparent;
                IoPoints.TDO9.Value = false;
                btnLeftSideswayCYReturn.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO8.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region TDO10
        private bool tDO10(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    leftBlackUpDownCYDown((bool)value);
                }).BeginInvoke(null,null);                
            }
            return IoPoints.TDO9.Value;
        }
        private void leftBlackUpDownCYDown(bool b)
        {
            if (CyLeftBlackUpDown.IsAction())
            {
                btnLeftSideswayCYReturn.BackColor = Color.Transparent;
                IoPoints.TDO8.Value = false;
                btnLeftSideswayCYReach.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO9.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region TDO11
        private bool tDO11(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    rightBlackUpDownCYUp((bool)value);
                }).BeginInvoke(null,null);
            }
            return IoPoints.TDO10.Value;
        }
        private void rightBlackUpDownCYUp(bool b)
        {
            if (CyRightBlackUpDown.IsAction())
            {
                btnRightSideswayCYReach.BackColor = Color.Transparent;
                IoPoints.TDO11.Value = false;
                btnRightSideswayCYReturn.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO10.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region TDO12
        private bool tDO12(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    rightBlackUpDownCYDown((bool)value);
                }).BeginInvoke(null,null);                
            }
            return IoPoints.TDO11.Value;
        }
        private void rightBlackUpDownCYDown(bool b)
        {
            if (CyRightBlackUpDown.IsAction())
            {
                btnRightSideswayCYReturn.BackColor = Color.Transparent;
                IoPoints.TDO10.Value = false;
                btnRightSideswayCYReach.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO11.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region TDO13
        private bool tDO13(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    whiteUpDownCYUp((bool)value);
                }).BeginInvoke(null,null);                
            }

            return IoPoints.TDO12.Value;
        }
        private void whiteUpDownCYUp(bool b)
        {
            if (CyWhiteUpDown.IsAction())
            {
                btnWhiteUpDownCYDown.BackColor = Color.Transparent;
                IoPoints.TDO13.Value = false;
                btnWhiteUpDownCYUp.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO12.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region TDO14
        private bool tDO14(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    whiteUpDownCYDown((bool)value);
                }).BeginInvoke(null,null);                
            }

            return IoPoints.TDO13.Value;
        }
        private void whiteUpDownCYDown(bool b)
        {
            if (CyWhiteUpDown.IsAction())
            {
                btnWhiteUpDownCYUp.BackColor = Color.Transparent;
                IoPoints.TDO12.Value = false;
                btnWhiteUpDownCYDown.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.TDO13.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region TDO15
        private bool tDO15(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    twoColorGreenLight((bool)value);
                }).BeginInvoke(null, null);
            }

            return IoPoints.TDO14.Value;
        }
        private void twoColorGreenLight(bool b)
        {
            btn2ColorRedLight.BackColor = Color.Transparent;
            btn2ColorGreenLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO15.Value = false;
            IoPoints.TDO14.Value = b;
            Thread.Sleep(250);
            IoPoints.TDO14.Value = !b;
            Thread.Sleep(250);
            IoPoints.TDO14.Value = b;
            Thread.Sleep(250);
            IoPoints.TDO14.Value = !b;
            Thread.Sleep(250);
            IoPoints.TDO14.Value = b;
        }
        #endregion
        #region TDO16
        private bool tDO16(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    twoColorRedLight((bool)value);
                }).BeginInvoke(null, null);
            }

            return IoPoints.TDO15.Value;
        }
        private void twoColorRedLight(bool b)
        {
            btn2ColorGreenLight.BackColor = Color.Transparent;
            btn2ColorRedLight.BackColor = b ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO14.Value = false;
            IoPoints.TDO15.Value = b;
            Thread.Sleep(250);
            IoPoints.TDO15.Value = !b;
            Thread.Sleep(250);
            IoPoints.TDO15.Value = b;
            Thread.Sleep(250);
            IoPoints.TDO15.Value = !b;
            Thread.Sleep(250);
            IoPoints.TDO15.Value = b;
        }
        #endregion

        #region IDO0
        private bool iDO0(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    leftSideswayCY((bool)value);
                }).BeginInvoke(null, null);
            }
            return IoPoints.IDO0.Value;
        }
        private void leftSideswayCY(bool b)
        {
            if (CyLeftSidesway.IsAction())
            {
                btnLeftBlackUpDownCY.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.IDO0.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region IDO2
        private bool iDO2(bool? value = null)
        {
            if (value != null)
            {
                new Action(() =>
                {
                    rightSideswayCY((bool)value);
                }).BeginInvoke(null, null);
            }
            return IoPoints.IDO2.Value;
        }
        private void rightSideswayCY(bool b)
        {
            if (CyRightSidesway.IsAction())
            {
                btnRightBlackUpDownCY.BackColor = b ? Color.DarkGray : Color.Transparent;
                IoPoints.IDO2.Value = b;
            }
            else if (isButtonClick)
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
            isButtonClick = false;
        }
        #endregion
        #region IDO1
        private bool iDO1(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO1.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO1.Value = b;
        //    }
        //}
        #endregion        
        #region IDO3
        private bool iDO3(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO3.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO3.Value = b;
        //    }
        //}
        #endregion
        #region IDO4
        private bool iDO4(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO4.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO4.Value = b;
        //    }
        //}
        #endregion
        #region IDO5
        private bool iDO5(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO5.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO5.Value = b;
        //    }
        //}
        #endregion
        #region IDO6
        private bool iDO6(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO6.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO6.Value = b;
        //    }
        //}
        #endregion
        #region IDO7
        private bool iDO7(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO7.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO7.Value = b;
        //    }
        //}
        #endregion
        #region IDO8
        private bool iDO8(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO8.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO8.Value = b;
        //    }
        //}
        #endregion
        #region IDO9
        private bool iDO9(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO9.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO9.Value = b;
        //    }
        //}
        #endregion
        #region IDO10
        private bool iDO10(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO10.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO10.Value = b;
        //    }
        //}
        #endregion
        #region IDO11
        private bool iDO11(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO11.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO11.Value = b;
        //    }
        //}
        #endregion
        #region IDO12
        private bool iDO12(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO12.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO12.Value = b;
        //    }
        //}
        #endregion
        #region IDO13
        private bool iDO13(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO13.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO13.Value = b;
        //    }
        //}
        #endregion
        #region IDO14
        private bool iDO14(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO14.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO14.Value = b;
        //    }
        //}
        #endregion
        #region IDO15
        private bool iDO15(bool? value = null)
        {
            if (value != null)
            { /*((bool)value)*/; }
            return IoPoints.IDO15.Value;
        }
        //private void /**/(bool b)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<bool>(/**/), b);
        //    }
        //    else
        //    {
        //        /**/.BackColor = b ? Color.DarkGray : Color.Transparent;
        //        IoPoints.IDO15.Value = b;
        //    }
        //}
        #endregion

        #endregion

        #region IO输出操作
        private void btn3ColorRedLight_Click(object sender, EventArgs e)
        {
            btn3ColorRedLight.BackColor = !IoPoints.TDO0.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO0.Value = !IoPoints.TDO0.Value;
        }

        private void btn3ColorYellowLight_Click(object sender, EventArgs e)
        {
            btn3ColorYellowLight.BackColor = !IoPoints.TDO1.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO1.Value = !IoPoints.TDO1.Value;
        }

        private void btn3ColorGreenLight_Click(object sender, EventArgs e)
        {
            btn3ColorGreenLight.BackColor = !IoPoints.TDO2.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO2.Value = !IoPoints.TDO2.Value;
        }

        private void btnBuzzer_Click(object sender, EventArgs e)
        {
            btnBuzzer.BackColor = !IoPoints.TDO3.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO3.Value = !IoPoints.TDO3.Value;
        }

        private void btnStopLight_Click(object sender, EventArgs e)
        {
            btnPauseLight.BackColor =
            btnResetLight.BackColor = Color.Transparent;
            btnStopLight.BackColor = !IoPoints.TDO4.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO5.Value =
            IoPoints.TDO6.Value = false;
            IoPoints.TDO4.Value = !IoPoints.TDO4.Value;
        }

        private void btnPauseLight_Click(object sender, EventArgs e)
        {
            btnStopLight.BackColor =
            btnResetLight.BackColor = Color.Transparent;
            btnPauseLight.BackColor = !IoPoints.TDO5.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO4.Value =
            IoPoints.TDO6.Value = false;
            IoPoints.TDO5.Value = !IoPoints.TDO5.Value;
        }

        private void btnResetLight_Click(object sender, EventArgs e)
        {
            btnStopLight.BackColor =
            btnPauseLight.BackColor = Color.Transparent;
            btnResetLight.BackColor = !IoPoints.TDO6.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO4.Value =
            IoPoints.TDO5.Value = false;
            IoPoints.TDO6.Value = !IoPoints.TDO6.Value;
        }

        private void btnAxisNotBrakeZ_Click(object sender, EventArgs e)
        {
            btnAxisNotBrakeZ.BackColor = !IoPoints.TDO7.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO7.Value = !IoPoints.TDO7.Value;
        }

        private void btnLeftSideswayCYReturn_Click(object sender, EventArgs e)
        {
            if (CyLeftBlackUpDown.IsAction())
            {
                btnLeftSideswayCYReach.BackColor = Color.Transparent;
                IoPoints.TDO9.Value = false;
                btnLeftSideswayCYReturn.BackColor = Color.DarkGray;
                IoPoints.TDO8.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void btnLeftSideswayCYReach_Click(object sender, EventArgs e)
        {
            if (CyLeftBlackUpDown.IsAction())
            {
                btnLeftSideswayCYReturn.BackColor = Color.Transparent;
                IoPoints.TDO8.Value = false;
                btnLeftSideswayCYReach.BackColor = Color.DarkGray;
                IoPoints.TDO9.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void btnRightSideswayCYReturn_Click(object sender, EventArgs e)
        {
            if (CyRightBlackUpDown.IsAction())
            {
                btnRightSideswayCYReach.BackColor = Color.Transparent;
                IoPoints.TDO11.Value = false;
                btnRightSideswayCYReturn.BackColor = Color.DarkGray;
                IoPoints.TDO10.Value = true;
            }
            else 
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void btnRightSideswayCYReach_Click(object sender, EventArgs e)
        {
            if (CyRightBlackUpDown.IsAction())
            {
                btnRightSideswayCYReturn.BackColor = Color.Transparent;
                IoPoints.TDO10.Value = false;
                btnRightSideswayCYReach.BackColor = Color.DarkGray;
                IoPoints.TDO11.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void btnWhiteUpDownCYUp_Click(object sender, EventArgs e)
        {
            if (CyWhiteUpDown.IsAction())
            {
                btnWhiteUpDownCYDown.BackColor = Color.Transparent;
                IoPoints.TDO13.Value = false;
                btnWhiteUpDownCYUp.BackColor = Color.DarkGray;
                IoPoints.TDO12.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void btnWhiteUpDownCYDown_Click(object sender, EventArgs e)
        {
            if (CyWhiteUpDown.IsAction())
            {
                btnWhiteUpDownCYUp.BackColor = Color.Transparent;
                IoPoints.TDO12.Value = false;
                btnWhiteUpDownCYDown.BackColor = Color.DarkGray;
                IoPoints.TDO13.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }       

        private void btn2ColorGreenLight_Click(object sender, EventArgs e)
        {
            btn2ColorGreenLight.BackColor = !IoPoints.TDO14.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO15.Value = false;
            IoPoints.TDO14.Value = !IoPoints.TDO14.Value;
        }

        private void btn2ColorRedLight_Click(object sender, EventArgs e)
        {            
            btn2ColorRedLight.BackColor = !IoPoints.TDO15.Value ? Color.DarkGray : Color.Transparent;
            IoPoints.TDO14.Value = false;
            IoPoints.TDO15.Value = !IoPoints.TDO15.Value;
        }

        private void btnLeftBlackUpDownCY_Click(object sender, EventArgs e)
        {
            if (CyLeftSidesway.IsAction())
            {
                IoPoints.IDO1.Value = false;
                IoPoints.IDO0.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void btnRightBlackUpDownCY_Click(object sender, EventArgs e)
        {
            if (CyRightSidesway.IsAction())
            {
                IoPoints.IDO3.Value = false;
                IoPoints.IDO2.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }        

        private void btnLeftBlackDownUpCY_Click(object sender, EventArgs e)
        {
            if (CyRightSidesway.IsAction())
            {
                IoPoints.IDO2.Value = false;
                IoPoints.IDO3.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        private void button52_Click(object sender, EventArgs e)
        {
            if (CyLeftSidesway.IsAction())
            {
                IoPoints.IDO0.Value = false;
                IoPoints.IDO1.Value = true;
            }
            else
            {
                MessageBox.Show("此机构状态存在干涉，不可动作！");
            }
        }

        #endregion

        private void btnThroughView_Click(object sender, EventArgs e)
        {
            IoPoints.IDO14.Value = false;
            IoPoints.IDO15.Value = false;
        }

        private void btnWideView_Click(object sender, EventArgs e)
        {
            IoPoints.IDO14.Value = true;
            IoPoints.IDO15.Value = true;
        }

        private void btnTopDownView_Click(object sender, EventArgs e)
        {
            IoPoints.IDO14.Value = true;
            IoPoints.IDO15.Value = false;
        }

        private void btnNormalView_Click(object sender, EventArgs e)
        {
            IoPoints.IDO14.Value = false;
            IoPoints.IDO15.Value = true;
        }

        private void button57_Click(object sender, EventArgs e)
        {
            IoPoints.IDO12.Value = false;
            IoPoints.IDO13.Value = false;
        }

        private void button55_Click(object sender, EventArgs e)
        {
            IoPoints.IDO12.Value = true;
            IoPoints.IDO13.Value = true;
        }

        private void button59_Click(object sender, EventArgs e)
        {
            IoPoints.IDO12.Value = true;
            IoPoints.IDO13.Value = false;
        }

        private void button58_Click(object sender, EventArgs e)
        {
            IoPoints.IDO12.Value = false;
            IoPoints.IDO13.Value = true;
        }
    }
}
