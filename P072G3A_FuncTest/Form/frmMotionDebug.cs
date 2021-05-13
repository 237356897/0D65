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
    public partial class frmMotionDebug : Form
    {
        public double EquivalentPulseY1;
        public double PulseEquivalentY1;
        public double EquivalentPulseY2;
        public double PulseEquivalentY2;
        public double EquivalentPulseZ;
        public double PulseEquivalentZ;

        public frmMotionDebug()
        {
            InitializeComponent();
        }
        private void frmMotionDebug_Load(object sender, EventArgs e)
        {
            EquivalentPulseY1 = frmMain.main.frmMotion.EquivalentPulseY1;
            PulseEquivalentY1 = frmMain.main.frmMotion.PulseEquivalentY1;
            EquivalentPulseY2 = frmMain.main.frmMotion.EquivalentPulseY2;
            PulseEquivalentY2 = frmMain.main.frmMotion.PulseEquivalentY2;
            EquivalentPulseZ = frmMain.main.frmMotion.EquivalentPulseZ;
            PulseEquivalentZ = frmMain.main.frmMotion.PulseEquivalentZ;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            labCurrentPositionY1.Text = (frmMain.main.frmMotion.Y1.GetCurrentCommandPosition * PulseEquivalentY1).ToString("0.00");
            labCurrentPositionY2.Text = (frmMain.main.frmMotion.Y2.GetCurrentCommandPosition * PulseEquivalentY2).ToString("0.00");
            labCurrentPositionZ.Text = (frmMain.main.frmMotion.Z.GetCurrentCommandPosition * PulseEquivalentZ).ToString("0.00");
            labCurrentSpeedY1.Text = (frmMain.main.frmMotion.Y1.GetCurrentCommandSpeed * PulseEquivalentY1).ToString("0.00");
            labCurrentSpeedY2.Text = (frmMain.main.frmMotion.Y2.GetCurrentCommandSpeed * PulseEquivalentY2).ToString("0.00");
            labCurrentSpeedZ.Text = (frmMain.main.frmMotion.Z.GetCurrentCommandSpeed * PulseEquivalentZ).ToString("0.00");

            checkBoxIsAlarmZ.Checked = frmMain.main.frmMotion.Z.IsAlarm;
            checkBoxIsAlarmY1.Checked = frmMain.main.frmMotion.Y1.IsAlarm;
            checkBoxIsAlarmY2.Checked = frmMain.main.frmMotion.Y2.IsAlarm;
            checkBoxIsServerONZ.Checked = frmMain.main.frmMotion.Z.IsServoON;
            checkBoxIsServerONY1.Checked = frmMain.main.frmMotion.Y1.IsServoON;
            checkBoxIsServerONY2.Checked = frmMain.main.frmMotion.Y2.IsServoON;
            checkBoxPositiveLimitZ.Checked = frmMain.main.frmMotion.Z.IsPel;
            checkBoxPositiveLimitY1.Checked = frmMain.main.frmMotion.Y1.IsPel;
            checkBoxPositiveLimitY2.Checked = frmMain.main.frmMotion.Y2.IsPel;
            checkBoxOriginZ.Checked = frmMain.main.frmMotion.Z.IsOrg;
            checkBoxOriginY1.Checked = frmMain.main.frmMotion.Y1.IsOrg;
            checkBoxOriginY2.Checked = frmMain.main.frmMotion.Y2.IsOrg;
            checkBoxNegativeLimitZ.Checked = frmMain.main.frmMotion.Z.IsMel;
            checkBoxNegativeLimitY1.Checked = frmMain.main.frmMotion.Y1.IsMel;
            checkBoxNegativeLimitY2.Checked = frmMain.main.frmMotion.Y2.IsMel;


            timer1.Enabled = true;
        }

        private void btnAddY1_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Y1.IsMotionless && radioBtnSetDistance.Checked && frmMain.main.frmMotion.AxisY1.IsAction())
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

                frmMain.main.frmMotion.Y1.MoveRel(pulseNum, frmMain.main.frmMotion.getManualSpeed("Y1"));
            }
            else if (!frmMain.main.frmMotion.AxisY1.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnDecY1_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Y1.IsMotionless && radioBtnSetDistance.Checked && frmMain.main.frmMotion.AxisY1.IsAction())
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

                frmMain.main.frmMotion.Y1.MoveRel(pulseNum * -1, frmMain.main.frmMotion.getManualSpeed("Y1"));
            }
            else if (!frmMain.main.frmMotion.AxisY1.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnDecY2_Click(object sender, EventArgs e)
        {

        }

        private void btnAddY2_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Y2.IsMotionless && radioBtnSetDistance.Checked && frmMain.main.frmMotion.AxisY2.IsAction())
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

                frmMain.main.frmMotion.Y2.MoveRel(pulseNum, frmMain.main.frmMotion.getManualSpeed("Y2"));
            }
            else if (!frmMain.main.frmMotion.AxisY2.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }

        private void btnAddZ_Click(object sender, EventArgs e)
        {

        }

        private void btnDecZ_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Z.IsMotionless && radioBtnSetDistance.Checked && frmMain.main.frmMotion.AxisZ.IsAction())
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

                frmMain.main.frmMotion.Z.MoveRel(pulseNum * -1, frmMain.main.frmMotion.getManualSpeed("Z"));
            }
            else if (!frmMain.main.frmMotion.AxisZ.IsAction())
            { MessageBox.Show("此机构状态存在干涉，不可动作！"); }
        }
    }
}
