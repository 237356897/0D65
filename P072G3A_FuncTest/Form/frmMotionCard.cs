using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;
using System.IO;
using Motion.AdlinkAps;
using System.Threading;

namespace P072G3A_FuncTest
{
    public partial class frmMotionCard : Form
    {
        public static Common.void_StringDelegate AppendLog;
        public Dictionary<string, DataTable> DTable = new Dictionary<string, DataTable>();
        public Axis Y1 = new Axis(IoPoints.Card204c_0, 0);
        public Axis Y2 = new Axis(IoPoints.Card204c_0,1);
        public Axis Z = new Axis(IoPoints.Card204c_0,2);
        public Unit AxisY1;
        public Unit AxisY2;
        public Unit AxisZ;

        private DataSet ds = new DataSet();
        private string paramPath;
        private string currentProduct;
        private int? motionPositionY1;
        private int? motionPositionY2;
        private int? motionPositionZ;

        #region Axis Parameter

        public double EquivalentPulseY1 { get { return double.Parse(DTable["OtherParam"].Rows[0][6].ToString()); } set { DTable["OtherParam"].Rows[0][6] = value.ToString(); } }
        public double PulseEquivalentY1 { get { return double.Parse(DTable["OtherParam"].Rows[0][7].ToString()); } set { DTable["OtherParam"].Rows[0][7] = value.ToString(); } }
        public double EquivalentPulseY2 { get { return double.Parse(DTable["OtherParam"].Rows[1][6].ToString()); } set { DTable["OtherParam"].Rows[1][6] = value.ToString(); } }
        public double PulseEquivalentY2 { get { return double.Parse(DTable["OtherParam"].Rows[1][7].ToString()); } set { DTable["OtherParam"].Rows[1][7] = value.ToString(); } }
        public double EquivalentPulseZ { get { return double.Parse(DTable["OtherParam"].Rows[2][6].ToString()); } set { DTable["OtherParam"].Rows[2][6] = value.ToString(); } }
        public double PulseEquivalentZ { get { return double.Parse(DTable["OtherParam"].Rows[2][7].ToString()); } set { DTable["OtherParam"].Rows[2][7] = value.ToString(); } }

        #endregion

        #region Position Parameter
        public int LeftStandbyPos { get { return (int)(double.Parse(DTable["Y1"].Rows[0][2].ToString()) / PulseEquivalentY1); } }
        public int LeftBlackPos { get { return (int)(double.Parse(DTable["Y1"].Rows[1][2].ToString()) / PulseEquivalentY1); } }
        public int LeftWhitePos { get { return (int)(double.Parse(DTable["Y1"].Rows[2][2].ToString()) / PulseEquivalentY1); } }
        public int LeftGraphicPos { get { return (int)(double.Parse(DTable["Y1"].Rows[3][2].ToString()) / PulseEquivalentY1); } }
        public int RightStandbyPos { get { return (int)(double.Parse(DTable["Y2"].Rows[0][2].ToString()) / PulseEquivalentY2); } }
        public int RightBlackPos { get { return (int)(double.Parse(DTable["Y2"].Rows[1][2].ToString()) / PulseEquivalentY2); } }
        public int RightWhitePos { get { return (int)(double.Parse(DTable["Y2"].Rows[2][2].ToString()) /PulseEquivalentY2); } }
        public int RightGraphicPos { get { return (int)(double.Parse(DTable["Y2"].Rows[3][2].ToString()) / PulseEquivalentY2); } }
        public int ZStandbyPos { get { return (int)(double.Parse(DTable["Z"].Rows[0][2].ToString()) / PulseEquivalentZ); } }
        #endregion

        #region Velocity Parameter

        #region Y1
        private int maximumVelocityY1 { get { return int.Parse(DTable["SpeedParam"].Rows[0][1].ToString()); } }
        private double accelerationTimeY1 { get { return double.Parse(DTable["SpeedParam"].Rows[0][2].ToString()); } }
        private double decelerationTimeY1 { get { return double.Parse(DTable["SpeedParam"].Rows[0][3].ToString()); } }
        private bool isSoftLimitY1 { get { return bool.Parse(DTable["OtherParam"].Rows[0][1].ToString()); } }
        private double softPositiveLimitY1 { get { return double.Parse(DTable["OtherParam"].Rows[0][2].ToString()) / PulseEquivalentY1; } }
        private double softNegativeLimitY1 { get { return double.Parse(DTable["OtherParam"].Rows[0][3].ToString()) / PulseEquivalentY1; } }
        private double leadY1 { get { return double.Parse(DTable["OtherParam"].Rows[0][4].ToString()); } }
        private double subdivisionNumY1 { get { return double.Parse(DTable["OtherParam"].Rows[0][5].ToString()); } }
        #endregion

        #region Y2
        private int maximumVelocityY2 { get { return int.Parse(DTable["SpeedParam"].Rows[1][1].ToString()); } }
        private double accelerationTimeY2 { get { return double.Parse(DTable["SpeedParam"].Rows[1][2].ToString()); } }
        private double decelerationTimeY2 { get { return double.Parse(DTable["SpeedParam"].Rows[1][3].ToString()); } }
        private bool isSoftLimitY2 { get { return bool.Parse(DTable["OtherParam"].Rows[1][1].ToString()); } }
        private double softPositiveLimitY2 { get { return double.Parse(DTable["OtherParam"].Rows[1][2].ToString()) / PulseEquivalentY2; } }
        private double softNegativeLimitY2 { get { return double.Parse(DTable["OtherParam"].Rows[1][3].ToString()) / PulseEquivalentY2; } }
        private double leadY2 { get { return double.Parse(DTable["OtherParam"].Rows[1][4].ToString()); } }
        private double subdivisionNumY2 { get { return double.Parse(DTable["OtherParam"].Rows[1][5].ToString()); } }

        #endregion

        #region Z
        private int maximumVelocityZ { get { return int.Parse(DTable["SpeedParam"].Rows[2][1].ToString()); } }
        private double accelerationTimeZ { get { return double.Parse(DTable["SpeedParam"].Rows[2][2].ToString()); } }
        private double decelerationTimeZ { get { return double.Parse(DTable["SpeedParam"].Rows[2][3].ToString()); } }
        private bool isSoftLimitZ { get { return bool.Parse(DTable["OtherParam"].Rows[2][1].ToString()); } }
        private double softPositiveLimitZ { get { return double.Parse(DTable["OtherParam"].Rows[2][2].ToString()) / PulseEquivalentZ; } }
        private double softNegativeLimitZ { get { return double.Parse(DTable["OtherParam"].Rows[2][3].ToString()) / PulseEquivalentZ; } }
        private double leadZ { get { return double.Parse(DTable["OtherParam"].Rows[2][4].ToString()); } }
        private double subdivisionNumZ { get { return double.Parse(DTable["OtherParam"].Rows[2][5].ToString()); } }

        #endregion

        #endregion

        public frmMotionCard()
        {
            InitializeComponent();
        }
        private void frmMotionCard_Load(object sender, EventArgs e)
        {
            comboBoxSelectAxis.SelectedIndex = 0;
            labJogSpeedDis.Text = string.Format("手动速度：{0}mm/s", (double)trackBarJogSpeed.Value / 100);
            IoPoints.Card204c_0.Initialize();
        }

        private void frmMotionCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            IoPoints.Card204c_0.Dispose();
        }

        #region Public Method
        public void LoadParameter(string str)
        {
            bool bTemp = false;
            currentProduct = str;
            paramPath = frmProductList.CreateFilePath(str + ".xml", "Motion");
            ds = new DataSet();
            ds.ReadXml(paramPath);

            DTable = new Dictionary<string, DataTable>();
            dgvSetSpeedParam.DataSource = ds;
            dgvSetSpeedParam.DataMember = "SpeedParam";
            dgvSetSpeedParam.Columns[0].Width = 100;
            
            DTable.Add("SpeedParam", ds.Tables["SpeedParam"]);
            setAxisParam();         

            dgvSetOtherParam.DataSource = ds;
            dgvSetOtherParam.DataMember = "OtherParam";
            DTable.Add("OtherParam", ds.Tables["OtherParam"]);

            dgvCoordinatesY1.DataSource = ds;
            dgvCoordinatesY1.DataMember = "CoordinatesY1";
            DTable.Add("Y1", ds.Tables["CoordinatesY1"]);
            foreach (DataGridViewRow row in dgvCoordinatesY1.Rows)
            {
                if (bool.TryParse(row.Cells[0].Value.ToString(),out bTemp))
                {
                    if (bTemp)
                    {
                        bTemp = false;
                        motionPositionY1 = (int)(double.Parse(row.Cells[2].Value.ToString()) / PulseEquivalentY1);
                        break;
                    }
                }
            }

            dgvCoordinatesY2.DataSource = ds;
            dgvCoordinatesY2.DataMember = "CoordinatesY2";
            DTable.Add("Y2", ds.Tables["CoordinatesY2"]);
            foreach (DataGridViewRow row in dgvCoordinatesY2.Rows)
            {
                if (bool.TryParse(row.Cells[0].Value.ToString(), out bTemp))
                {
                    if (bTemp)
                    {
                        bTemp = false;
                        motionPositionY2 = (int)(double.Parse(row.Cells[2].Value.ToString()) / PulseEquivalentY2);
                        break;
                    }
                }
            }

            dgvCoordinatesZ.DataSource = ds;
            dgvCoordinatesZ.DataMember = "CoordinatesZ";
            DTable.Add("Z", ds.Tables["CoordinatesZ"]);
            foreach (DataGridViewRow row in dgvCoordinatesZ.Rows)
            {
                if (bool.TryParse(row.Cells[0].Value.ToString(), out bTemp))
                {
                    if (bTemp)
                    {
                        bTemp = false;
                        motionPositionZ = (int)(double.Parse(row.Cells[2].Value.ToString()) / PulseEquivalentZ);
                        break;
                    }
                }
            } 
        }

        public void LoadSoftwareLimit()
        {
            if (isSoftLimitY1)
            { Y1.SetSoftConfig(softPositiveLimitY1, softNegativeLimitY1); }
            else
            { Y1.ClearSoftConfig(); }

            if (isSoftLimitY2)
            { Y2.SetSoftConfig(softPositiveLimitY2, softNegativeLimitY2); }
            else
            { Y2.ClearSoftConfig(); }

            if (isSoftLimitZ)
            { Z.SetSoftConfig(softPositiveLimitZ, softNegativeLimitZ); }
            else
            { Z.ClearSoftConfig(); }
        }
        #endregion

        #region Private Method
        private void appendText(string str)
        {
            AppendLog.Invoke(string.Format("<<运动卡界面>>:: {0}", str));
        }

        private void setPulseEquivalent()
        {
            PulseEquivalentY1 = leadY1 / subdivisionNumY1;
            PulseEquivalentY2 = leadY2 / subdivisionNumY2;
            PulseEquivalentZ = leadZ / subdivisionNumZ;
        }

        private void setEquivalentPulse()
        {
            EquivalentPulseY1 = subdivisionNumY1 / leadY1;
            EquivalentPulseY2 = subdivisionNumY2 / leadY2;
            EquivalentPulseZ = subdivisionNumZ / leadZ;
        }

        private void setAxisParam()
        {
            Y1.SetVelParam(accelerationTimeY1, decelerationTimeY1, maximumVelocityY1);
            Y2.SetVelParam(accelerationTimeY2, decelerationTimeY2, maximumVelocityY2);
            Z.SetVelParam(accelerationTimeZ, decelerationTimeZ, maximumVelocityZ);

            Y1.ClearSoftConfig();
            Y2.ClearSoftConfig();
            Z.ClearSoftConfig();
        }

        public int getManualSpeed(string axis)
        {
            double pulseEquivalent = 0.0;
            switch (axis.ToUpper())
            {
                case "Y1":
                    pulseEquivalent = PulseEquivalentY1;
                    break;

                case "Y2":
                    pulseEquivalent = PulseEquivalentY2;
                    break;

                case "Z":
                    pulseEquivalent = PulseEquivalentZ;
                    break;

                default:
                    MessageBox.Show("调用获取手动速度函数，参数赋值错误！");
                    break;
            }

            return (int)(((double)trackBarJogSpeed.Value / 100) / pulseEquivalent);//10000/100=100mm
        }
        #endregion

        private void btnMotion_Click(object sender, EventArgs e)
        {
            btnMotion.Enabled = false;
            Axis axis = null;
            Unit unit = null;
            int? motionPulse = 0;
            switch (comboBoxSelectAxis.Text)
            {
                case "Y1":
                    axis = Y1;
                    unit = AxisY1;
                    motionPulse = motionPositionY1;
                    break;

                case "Y2":
                    axis = Y2;
                    unit = AxisY2;
                    motionPulse = motionPositionY2;
                    break;

                case "Z":
                    axis = Z;
                    unit = AxisZ;
                    motionPulse = motionPositionZ;
                    break;
            }
            new Action(() =>
            {
                if (unit.IsAction() && motionPulse != null)
                { axis.MoveAbs((int)motionPulse); }
                else if (!unit.IsAction())
                { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
                Thread.Sleep(50);

                while (!axis.IsMotionless)
                { Thread.Sleep(20); }

                Invoke(new Action(() => 
                { btnMotion.Enabled = true; }));

            }).BeginInvoke(null,null);
        }

        private void btnBackHome_Click(object sender, EventArgs e)
        {
            btnBackHome.Enabled = false;
            Axis axis = null;
            Unit unit = null;
            switch (comboBoxSelectAxis.Text)
            {
                case "Y1":
                    axis = Y1;
                    unit = AxisY1;
                    break;

                case "Y2":
                    axis = Y2;
                    unit = AxisY2;
                    break;

                case "Z":
                    axis = Z;
                    unit = AxisZ;
                    break;
            }
            new Action(() =>
            {
                if (unit.IsAction())
                {
                    axis.BackHome();
                }
                else
                { MessageBox.Show("此机构状态存在干涉，不可动作！"); }

                Thread.Sleep(500);

                while (!axis.IsMotionless)
                { Thread.Sleep(20); }

                Invoke(new Action(() =>
                { btnBackHome.Enabled = true; }));

            }).BeginInvoke(null,null);
        }

        private void btnCoordRead_Click(object sender, EventArgs e)
        {
            switch (comboBoxSelectAxis.Text)
            {
                case "Y1":
                    if (motionPositionY1 != null)
                    {
                        motionPositionY1 = Y1.GetCurrentCommandPosition;
                        foreach (DataGridViewRow row in dgvCoordinatesY1.Rows)
                        {
                            if (bool.Parse(row.Cells[0].Value.ToString()))
                            {
                                row.Cells[2].Value = ((int)motionPositionY1 * PulseEquivalentY1).ToString("0.00");
                                break;
                            }
                        }
                    }
                    break;

                case "Y2":
                    if (motionPositionY2 != null)
                    {
                        motionPositionY2 = Y2.GetCurrentCommandPosition;
                        foreach (DataGridViewRow row in dgvCoordinatesY2.Rows)
                        {
                            if (bool.Parse(row.Cells[0].Value.ToString()))
                            {
                                row.Cells[2].Value = ((int)motionPositionY2 * PulseEquivalentY2).ToString("0.00");
                                break;
                            }
                        }
                    }
                    break;

                case "Z":
                    if (motionPositionZ != null)
                    {
                        motionPositionZ = Z.GetCurrentCommandPosition;
                        foreach (DataGridViewRow row in dgvCoordinatesZ.Rows)
                        {
                            if (bool.Parse(row.Cells[0].Value.ToString()))
                            {
                                row.Cells[2].Value = ((int)motionPositionZ * PulseEquivalentZ).ToString("0.00");
                                break;
                            }
                        }
                    }
                    break;
            }
        }
        private void btnPosSave_Click(object sender, EventArgs e)
        {
            if (DTable["Y1"].Rows[DTable["Y1"].Rows.Count - 1][2].ToString() != "")
            {
                DTable["Y1"].Rows.Add();
            }

            if (DTable["Y2"].Rows[DTable["Y2"].Rows.Count - 1][2].ToString() != "")
            {
                DTable["Y2"].Rows.Add();
            }

            if (DTable["Z"].Rows[DTable["Z"].Rows.Count - 1][2].ToString() != "")
            {
                DTable["Z"].Rows.Add();
            }

            ds.WriteXml(paramPath);
            appendText(string.Format("产品{0}：轴坐标保存成功！", currentProduct));
        }

        private void btnParamSave_Click(object sender, EventArgs e)
        {
            if (DTable["SpeedParam"].Rows[DTable["SpeedParam"].Rows.Count - 1][0].ToString() != "")
            {
                DTable["SpeedParam"].Rows.Add();
            }

            if (DTable["OtherParam"].Rows[DTable["OtherParam"].Rows.Count - 1][0].ToString() != "")
            {
                DTable["OtherParam"].Rows.Add();
            }

            setEquivalentPulse();
            setPulseEquivalent();
            setAxisParam();

            ds.WriteXml(paramPath);
            appendText(string.Format("产品{0}：参数保存成功！", currentProduct));
        }

        #region Y1轴(dgv)
        private void dgvCoordinatesY1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && dgvCoordinatesY1[2, e.RowIndex].Value.ToString() != "")
            {
                if (bool.Parse(dgvCoordinatesY1[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString()))
                {
                    foreach (DataGridViewRow row in dgvCoordinatesY1.Rows)
                    {
                        if (row != dgvCoordinatesY1.Rows[e.RowIndex])
                        { row.Cells[0].Value = false; }
                        else
                        { motionPositionY1 = (int)(double.Parse(row.Cells[2].Value.ToString()) / PulseEquivalentY1); }
                    }
                }
                else
                { motionPositionY1 = null; }
            }
        }

        private void dgvCoordinatesY1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                double tempDouble;
                bool tempBool;
                if (!double.TryParse(e.FormattedValue.ToString(), out tempDouble))
                {
                    e.Cancel = true;
                    appendText(string.Format("Y1轴单元格的值为：{0}，包含了非法字符，请设置成数字！", e.FormattedValue));
                }
                else if (bool.TryParse(dgvCoordinatesY1[0, e.RowIndex].Value.ToString(), out tempBool) && tempBool)
                {
                    motionPositionY1 = (int)(tempDouble / PulseEquivalentY1);
                }
            }
        }
        #endregion

        #region Y2轴(dgv)
        private void dgvCoordinatesY2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && dgvCoordinatesY2[2, e.RowIndex].Value.ToString() != "")
            {
                if (bool.Parse(dgvCoordinatesY2[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString()))
                {
                    foreach (DataGridViewRow row in dgvCoordinatesY2.Rows)
                    {
                        if (row != dgvCoordinatesY2.Rows[e.RowIndex])
                        { row.Cells[0].Value = false; }
                        else
                        { motionPositionY2 = (int)(double.Parse(row.Cells[2].Value.ToString()) / PulseEquivalentY2); }
                    }
                }
                else
                { motionPositionY2 = null; }
            }
        }

        private void dgvCoordinatesY2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                double tempDouble;
                bool tempBool;
                if (!double.TryParse(e.FormattedValue.ToString(), out tempDouble))
                {
                    e.Cancel = true;
                    appendText(string.Format("Y2轴单元格的值为：{0}，包含了非法字符，请设置成数字！", e.FormattedValue));
                }
                else if (bool.TryParse(dgvCoordinatesY2[0, e.RowIndex].Value.ToString(), out tempBool) && tempBool)
                {
                    motionPositionY2 = (int)(tempDouble / PulseEquivalentY2);
                }
            }
        }
        #endregion

        #region Z轴(dgv)
        private void dgvCoordinatesZ_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && dgvCoordinatesZ[2, e.RowIndex].Value.ToString() != "")
            {
                if (bool.Parse(dgvCoordinatesZ[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString()))
                {
                    foreach (DataGridViewRow row in dgvCoordinatesZ.Rows)
                    {
                        if (row != dgvCoordinatesZ.Rows[e.RowIndex])
                        { row.Cells[0].Value = false; }
                        else
                        { motionPositionZ = (int)(double.Parse(row.Cells[2].Value.ToString()) / PulseEquivalentZ); }
                    }
                }
            }
            else
            { motionPositionZ = null; }
        }

        private void dgvCoordinatesZ_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                double tempDouble;
                bool tempBool;
                if (!double.TryParse(e.FormattedValue.ToString(), out tempDouble))
                {
                    e.Cancel = true;
                    appendText(string.Format("Z轴单元格的值为：{0}，包含了非法字符，请设置成数字！", e.FormattedValue));
                }
                else if (bool.TryParse(dgvCoordinatesZ[0, e.RowIndex].Value.ToString(), out tempBool) && tempBool)
                {
                    motionPositionZ = (int)(tempDouble / PulseEquivalentZ);
                }
            }
        }
        #endregion

        #region SpeedParam(dgv)
        private void dgvSetSpeedParam_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                double tempDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out tempDouble))
                {
                    e.Cancel = true;
                    appendText(string.Format("设置速度参数单元格的值为：{0}，包含了非法字符，请设置成数字！", e.FormattedValue));
                }
            }
        }
        #endregion

        #region OtherParam(dgv)
        private void dgvSetOtherParam_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.ColumnIndex != 1)
            {
                double tempDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out tempDouble))
                {
                    e.Cancel = true;
                    appendText(string.Format("设置其他参数单元格的值为：{0}，包含了非法字符，请设置成数字！", e.FormattedValue));
                }
            }
        }
        #endregion

        #region ManualOperation
        private void trackBarJogSpeed_Scroll(object sender, EventArgs e)
        {
            labJogSpeedDis.Text = string.Format("手动速度：{0}mm/s", (double)trackBarJogSpeed.Value / 100);
        }

        private void btnAddY1_Click(object sender, EventArgs e)//Y1前进
        {

            if (Y1.IsMotionless && radioBtnSetDistance.Checked && AxisY1.IsAction())
            {
                int pulseNum = 0;
                if (radioBtnDistance10um.Checked)
                { pulseNum = (int)(0.01 / PulseEquivalentY1); }
                else if (radioBtnDistance100um.Checked)
                { pulseNum = (int)(0.1 / PulseEquivalentY1); }
                else if (radioBtnDistance1000um.Checked)
                { pulseNum = (int)(1 / PulseEquivalentY1); }
                else
                { pulseNum = (int)((double)numericUpDOtherum.Value / PulseEquivalentY1); }

                Y1.MoveRel(pulseNum, getManualSpeed("Y1"));
            }
            else if (!AxisY1.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnDecY1_Click(object sender, EventArgs e)//Y1后退
        {
            if (Y1.IsMotionless && radioBtnSetDistance.Checked && AxisY1.IsAction())
            {
                int pulseNum = 0;
                if (radioBtnDistance10um.Checked)
                { pulseNum = (int)(0.01 / PulseEquivalentY1); }
                else if (radioBtnDistance100um.Checked)
                { pulseNum = (int)(0.1 / PulseEquivalentY1); }
                else if (radioBtnDistance1000um.Checked)
                { pulseNum = (int)(1 / PulseEquivalentY1); }
                else
                { pulseNum = (int)((double)numericUpDOtherum.Value / PulseEquivalentY1); }

                Y1.MoveRel(pulseNum * -1, getManualSpeed("Y1"));
            }
            else if (!AxisY1.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnAddY2_Click(object sender, EventArgs e)//Y2后退
        {
            if (Y2.IsMotionless && radioBtnSetDistance.Checked && AxisY2.IsAction())
            {
                int pulseNum = 0;
                if (radioBtnDistance10um.Checked)
                { pulseNum = (int)(0.01 / PulseEquivalentY2); }
                else if (radioBtnDistance100um.Checked)
                { pulseNum = (int)(0.1 / PulseEquivalentY2); }
                else if (radioBtnDistance1000um.Checked)
                { pulseNum = (int)(1 / PulseEquivalentY2); }
                else
                { pulseNum = (int)((double)numericUpDOtherum.Value / PulseEquivalentY2); }

                Y2.MoveRel(pulseNum, getManualSpeed("Y2"));
            }
            else if (!AxisY2.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnDecY2_Click(object sender, EventArgs e)//Y2前进
        {

        }

        private void btnAddZ_Click(object sender, EventArgs e)//Z上移
        {

        }

        private void btnDecZ_Click(object sender, EventArgs e)//Z下移
        {
            if (Z.IsMotionless && radioBtnSetDistance.Checked && AxisZ.IsAction())
            {
                int pulseNum = 0;
                if (radioBtnDistance10um.Checked)
                { pulseNum = (int)(0.01 / PulseEquivalentZ); }
                else if (radioBtnDistance100um.Checked)
                { pulseNum = (int)(0.1 / PulseEquivalentZ); }
                else if (radioBtnDistance1000um.Checked)
                { pulseNum = (int)(1 / PulseEquivalentZ); }
                else
                { pulseNum = (int)((double)numericUpDOtherum.Value / PulseEquivalentZ); }

                Z.MoveRel(pulseNum * -1, getManualSpeed("Z"));
            }
            else if (!AxisZ.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnAddY1_MouseDown(object sender, MouseEventArgs e)//Y1前进
        {
            if (Y1.IsMotionless && radioBtnContinue.Checked && AxisY1.IsAction())
            {
                Y1.MoveContinu(MoveDirection.Postive, getManualSpeed("Y1"));
            }
            else if (!AxisY1.IsAction())
            { //MessageBox.Show("此机构状态存在干涉，不可动作！"); 
            }
        }

        private void btnAddY1_MouseUp(object sender, MouseEventArgs e)//Y1前进
        {
            if (radioBtnContinue.Checked)
            {
                Y1.DecelStop();
            }
        }

        private void btnDecY1_MouseDown(object sender, MouseEventArgs e)//Y1后退
        {
            if (Y1.IsMotionless && radioBtnContinue.Checked && AxisY1.IsAction())
            {
                Y1.MoveContinu(MoveDirection.Negative, getManualSpeed("Y1"));
            }
            else if (!AxisY1.IsAction())
            { //MessageBox.Show("此机构状态存在干涉，不可动作！"); 
            }
        }

        private void btnDecY1_MouseUp(object sender, MouseEventArgs e)//Y1后退
        {
            if (radioBtnContinue.Checked)
            {
                Y1.DecelStop();
            }
        }

        private void btnAddY2_MouseDown(object sender, MouseEventArgs e)//Y2后退
        {
            if (Y2.IsMotionless && radioBtnContinue.Checked && AxisY2.IsAction())
            {
                Y2.MoveContinu(MoveDirection.Postive, getManualSpeed("Y2"));
            }
            else if (!AxisY2.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnAddY2_MouseUp(object sender, MouseEventArgs e)//Y2后退
        {
            if (radioBtnContinue.Checked)
            {
                Y2.DecelStop();
            }
        }

        private void btnDecY2_MouseDown(object sender, MouseEventArgs e)//Y2前进
        {
            if (Y2.IsMotionless && radioBtnContinue.Checked && AxisY2.IsAction())
            {
                Y2.MoveContinu(MoveDirection.Negative, getManualSpeed("Y2"));
            }
            else if (!AxisY2.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnDecY2_MouseUp(object sender, MouseEventArgs e)//Y2前进
        {
            if (radioBtnContinue.Checked)
            {
                Y2.DecelStop();
            }
        }

        private void btnAddZ_MouseDown(object sender, MouseEventArgs e)//Z上移
        {
            if (Z.IsMotionless && radioBtnContinue.Checked && AxisZ.IsAction())
            {
                Z.MoveContinu(MoveDirection.Postive, getManualSpeed("Z"));
            }
            else if (!AxisZ.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnAddZ_MouseUp(object sender, MouseEventArgs e)//Z上移
        {
            if (radioBtnContinue.Checked)
            {
                Z.DecelStop();
            }
        }

        private void btnDecZ_MouseDown(object sender, MouseEventArgs e)//Z下移
        {
            if (Z.IsMotionless && radioBtnContinue.Checked && AxisZ.IsAction())
            {
                Z.MoveContinu(MoveDirection.Negative, getManualSpeed("Z"));
            }
            else if (!AxisZ.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnDecZ_MouseUp(object sender, MouseEventArgs e)//Z下移
        {
            if (radioBtnContinue.Checked)
            {
                Z.DecelStop();
            }
        }

        private void checkBoxIsServerONZ_CheckedChanged(object sender, EventArgs e)
        {
            if (Z.IsMotionless)
            { Z.IsServoON = checkBoxIsServerONZ.Checked; }
        }

        private void checkBoxIsServerONY1_CheckedChanged(object sender, EventArgs e)
        {
            if (Y1.IsMotionless)
            { Y1.IsServoON = checkBoxIsServerONY1.Checked; }
        }

        private void checkBoxIsServerONY2_CheckedChanged(object sender, EventArgs e)
        {
            if (Y2.IsMotionless)
            { Y2.IsServoON = checkBoxIsServerONY2.Checked; }
        }
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            labCurrentPositionY1.Text = (Y1.GetCurrentCommandPosition * PulseEquivalentY1).ToString("0.00");
            labCurrentPositionY2.Text = (Y2.GetCurrentCommandPosition * PulseEquivalentY2).ToString("0.00");
            labCurrentPositionZ.Text = (Z.GetCurrentCommandPosition * PulseEquivalentZ).ToString("0.00");
            labCurrentSpeedY1.Text = (Y1.GetCurrentCommandSpeed * PulseEquivalentY1).ToString("0.00");
            labCurrentSpeedY2.Text = (Y2.GetCurrentCommandSpeed * PulseEquivalentY2).ToString("0.00");
            labCurrentSpeedZ.Text = (Z.GetCurrentCommandSpeed * PulseEquivalentZ).ToString("0.00");

            checkBoxIsAlarmZ.Checked = Z.IsAlarm;
            checkBoxIsAlarmY1.Checked = Y1.IsAlarm;
            checkBoxIsAlarmY2.Checked = Y2.IsAlarm;
            checkBoxIsServerONZ.Checked = Z.IsServoON;
            checkBoxIsServerONY1.Checked = Y1.IsServoON;
            checkBoxIsServerONY2.Checked = Y2.IsServoON;
            checkBoxPositiveLimitZ.Checked = Z.IsPel;
            checkBoxPositiveLimitY1.Checked = Y1.IsPel;
            checkBoxPositiveLimitY2.Checked = Y2.IsPel;
            checkBoxOriginZ.Checked = Z.IsOrg;
            checkBoxOriginY1.Checked = Y1.IsOrg;
            checkBoxOriginY2.Checked = Y2.IsOrg;
            checkBoxNegativeLimitZ.Checked = Z.IsMel;
            checkBoxNegativeLimitY1.Checked = Y1.IsMel;
            checkBoxNegativeLimitY2.Checked = Y2.IsMel;


            timer1.Enabled = true;
        }
    }
}
