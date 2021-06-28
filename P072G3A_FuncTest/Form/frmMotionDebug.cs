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
using Image_Sitenamespace;
using desay.ProductData;

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
            combTestItem.SelectedIndex = 0;
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
            if (frmMain.main.frmMotion.Y1.IsMotionless && radioBtnSetDistance.Checked)
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
        }

        private void btnDecY1_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Y1.IsMotionless && radioBtnSetDistance.Checked)
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
        }

        private void btnDecY2_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Y2.IsMotionless && radioBtnSetDistance.Checked)
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

                frmMain.main.frmMotion.Y2.MoveRel(pulseNum * -1, frmMain.main.frmMotion.getManualSpeed("Y2"));
            }
        }

        private void btnAddY2_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Y2.IsMotionless && radioBtnSetDistance.Checked)
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
        }

        private void btnAddZ_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Z.IsMotionless && radioBtnSetDistance.Checked)
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

                frmMain.main.frmMotion.Z.MoveRel(pulseNum, frmMain.main.frmMotion.getManualSpeed("Z"));
            }
        }

        private void btnDecZ_Click(object sender, EventArgs e)
        {
            if (frmMain.main.frmMotion.Z.IsMotionless && radioBtnSetDistance.Checked)
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
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //指定的buf大小必须大于传入的字符长度
            StringBuilder buf = new StringBuilder(6072);
            bool bResult = false;           
            string strout = string.Empty;
            if (radbLeft.Checked)
            {
                switch (combTestItem.SelectedIndex)
                {
                    case 0://暗板
                        bResult = ImageSitA.DesayTestDark(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 1://黑场红外
                        ImageSitA.IR_LED_ON();
                        Thread.Sleep(500);
                        bResult = ImageSitA.DesayTestDark_LedLight(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        ImageSitA.IR_LED_OFF();
                        break;
                    case 2://白平衡
                        bResult = ImageSitA.DesayTestWhite_WB(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 3://Shading
                        bResult = ImageSitA.DesayTestWhite_Shading(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 4://脏污
                        bResult = ImageSitA.DesayTestWhite_Blemish(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 5://坏点
                        bResult = ImageSitA.DesayTestWhite_DefectPixel(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 6://亮点
                        bResult = ImageSitA.DesayTestWhite_DefectPixel(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 7://清晰度
                        bResult = ImageSitA.DesayTestChart_SFR(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 8://IR清晰度
                        bResult = ImageSitA.DesayTestIRChart(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 9://灰阶
                        int result = ImageSitA.DesayTestChart_ColorGray(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        bResult = result == 0 || result == 1;
                        break;
                    case 10://色彩
                        int result1 = ImageSitA.DesayTestChart_ColorGray(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        bResult = result1 == 0 || result1 == 2;
                        break;
                    case 11://视场角
                        bResult = ImageSitA.DesayTestChart_FOV(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 12://光学中心
                        bResult = ImageSitA.DesayTestChart_Alignment(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 13://畸变
                        bResult = ImageSitA.DesayTestChart_Distortion(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 14://SNR
                        bResult = ImageSitA.DesayTestChart_SNR(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 15://旋转倾斜
                        bResult = ImageSitA.DesayTestChart_TiltRotation(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 16://帧率
                        float fps = ImageSitA.DesayTest_GetFPS();
                        Thread.Sleep(500);
                        if (fps > Position.Instance.testItem.bFPSMinValue && fps < Position.Instance.testItem.bFPSMaxValue)
                        {
                            bResult = true;
                        }
                        else
                        {
                            bResult = false;
                        }
                        strout = fps.ToString();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (combTestItem.SelectedIndex)
                {
                    case 0://暗板
                        bResult = ImageSitB.DesayTestDark(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 1://黑场红外
                        ImageSitA.IR_LED_ON();
                        Thread.Sleep(500);
                        bResult = ImageSitB.DesayTestDark_LedLight(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        ImageSitA.IR_LED_OFF();
                        break;
                    case 2://白平衡
                        bResult = ImageSitB.DesayTestWhite_WB(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 3://Shading
                        bResult = ImageSitB.DesayTestWhite_Shading(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 4://脏污
                        bResult = ImageSitB.DesayTestWhite_Blemish(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 5://坏点
                        bResult = ImageSitB.DesayTestWhite_DefectPixel(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 6://亮点
                        bResult = ImageSitB.DesayTestWhite_DefectPixel(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 7://清晰度
                        bResult = ImageSitB.DesayTestChart_SFR(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 8://IR清晰度
                        bResult = ImageSitB.DesayTestIRChart(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 9://灰阶
                        int result = ImageSitB.DesayTestChart_ColorGray(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        bResult = result == 0 || result == 1;
                        break;
                    case 10://色彩
                        int result1 = ImageSitB.DesayTestChart_ColorGray(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        bResult = result1 == 0 || result1 == 2;
                        break;
                    case 11://视场角
                        bResult = ImageSitB.DesayTestChart_FOV(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 12://光学中心
                        bResult = ImageSitB.DesayTestChart_Alignment(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 13://畸变
                        bResult = ImageSitB.DesayTestChart_Distortion(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 14://SNR
                        bResult = ImageSitB.DesayTestChart_SNR(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 15://旋转倾斜
                        bResult = ImageSitB.DesayTestChart_TiltRotation(buf);
                        Thread.Sleep(500);
                        strout = buf.ToString();
                        break;
                    case 16://帧率
                        float fps = ImageSitB.DesayTest_GetFPS();
                        Thread.Sleep(500);
                        if (fps > Position.Instance.testItem.bFPSMinValue && fps < Position.Instance.testItem.bFPSMaxValue)
                        {
                            bResult = true;
                        }
                        else
                        {
                            bResult = false;
                        }
                        strout = fps.ToString();
                        break;
                    default:
                        break;
                }                
            }
            tbResult.Text = strout;
            if (bResult)
            {
                pictureBox1.Image = Image.FromFile(@".\Resources\GreenBall.png");
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@".\Resources\RedBall.png");
            }
        }

        private void combTestItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
        }

        private void radbLeft_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
        }

        private void radbRight_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
        }
    }
}
