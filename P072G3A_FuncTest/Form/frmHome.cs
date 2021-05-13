using CommonLibrary;
using desay.ProductData;
using Image_Sitenamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.ToolKit;
using System.Windows.Forms;

namespace P072G3A_FuncTest
{
    public partial class frmHome : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        IntPtr DlgHandleA;
        IntPtr DlgHandleB;

        public struct ResultPos
        {
            public Point FirstPoint_A;
            public Point FirstPoint_B;
            public Point LabFirstPoint_A;
            public Point LabFirstPoint_B;
            public int PicWidth;
            public int PicHeight;
            public int LabWidth;
            public int LabHeight;

        };

        public ResultPos RPos;
        public static Common.void_StringDelegate AppendLog;
        public string Operator;
        public string Authority;
        public string ProductModel;
        public string ProductBarcode;
        public string LeftRes { get { return labLeftTotalRes.Text; } set { labLeftTotalRes.Text = value; labLeftTotalRes.ForeColor = ChangeRes(value); } }
        public string RightRes { get { return labRightTotalRes.Text; } set { labRightTotalRes.Text = value; labRightTotalRes.ForeColor = ChangeRes(value); } }


        public frmHome()
        {
            InitializeComponent();
            RPos = new ResultPos();
            RPos.FirstPoint_A = new Point(17, 21);
            RPos.FirstPoint_B = new Point(17, 21);
            RPos.LabFirstPoint_A = new Point(48, 27);
            RPos.LabFirstPoint_B = new Point(48, 27);
            RPos.PicWidth = 94;
            RPos.PicHeight = 30;
            RPos.LabWidth = 94;
            RPos.LabHeight = 30;
        }

        public void frmHome_Load(object sender, EventArgs e)
        {
            labLeftTotalRes.Text = "";
            labLeftTotalRes.ForeColor = Color.Black;
            labRightTotalRes.Text = "";
            labRightTotalRes.ForeColor = Color.Black;
            RePaintUI();
            UpdateUIData();
        }

        public void AppendText(string str)
        {
            AppendLog.Invoke(string.Format("<<主界面>>:: {0}", str));
        }

        public Color ChangeRes(string res)
        {
            switch (res)
            {
                case "OK":
                    return Color.Green;
                case "NG":
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        #region 算法窗体
        public void DisposeDesayTestDlg()
        {
            if (DlgHandleA != IntPtr.Zero)
            {
                ImageSitA.ExitDesayImageDlg();
                DlgHandleA = IntPtr.Zero;
            }
            if (DlgHandleB != IntPtr.Zero)
            {
                ImageSitB.ExitDesayImageDlg();
                DlgHandleB = IntPtr.Zero;
            }  
        }

        public void ShowDesayTestDlg()
        {
            //打开采集图像画面
            if (DlgHandleA == IntPtr.Zero)
            {
                ImageSitA.ShowDesayImageDlg(true);
                DlgHandleA = ImageSitA.GetDesayImageDlgHwnd();
            }
            SetWindowPos(DlgHandleA, 0, 10, 150, 460, 470, 0);
            ShowWindow(DlgHandleA, 1);//1为显示，0为隐藏

            //打开采集图像画面
            if (DlgHandleB == IntPtr.Zero)
            {
                ImageSitB.ShowDesayImageDlg(false);
                DlgHandleB = ImageSitB.GetDesayImageDlgHwnd();
            }
            SetWindowPos(DlgHandleB, 0, 720, 150, 460, 470, 0);
            ShowWindow(DlgHandleB, 1);//1为显示，0为隐藏
        }

        public void HideDesayTestDlg()
        {
            if (DlgHandleA != IntPtr.Zero)
            {
                SetWindowPos(DlgHandleA, 0, 10, 150, 460, 470, 0);
                ShowWindow(DlgHandleA, 0);//1为显示，0为隐藏
            }            
            if (DlgHandleB != IntPtr.Zero)
            {
                SetWindowPos(DlgHandleB, 0, 720, 150, 460, 470, 0);
                ShowWindow(DlgHandleB, 0);//1为显示，0为隐藏 
            }            
        }

        #endregion

        #region UI操作
        private void AbtnRun_Click(object sender, EventArgs e)
        {
            AppendText("HHHHHHHHHHHHHHH");
            ImageSitA.PlayCamera();
        }

        private void AbtnStop_Click(object sender, EventArgs e)
        {
            AppendText("HHHHHHHHHHHHHHH");
            ImageSitA.StopCamera();
        }

        private void Asetting_Click(object sender, EventArgs e)
        {
            ImageSitA.ShowSettingDlg("参数设置SitA");
        }

        private void Adevice_Click(object sender, EventArgs e)
        {
            ImageSitA.ShowSetDeviceDlg("管脚定义SitA");
        }

        private void Apin_Click(object sender, EventArgs e)
        {
            ImageSitA.ShowSetPinDlg("管脚定义SitA");
        }

        private void BbtnRun_Click(object sender, EventArgs e)
        {
            AppendText("HHHHHHHHHHHHHHH");
            ImageSitB.PlayCamera();
        }

        private void BbtnStop_Click(object sender, EventArgs e)
        {
            AppendText("HHHHHHHHHHHHHHH");
            ImageSitB.StopCamera();
        }

        private void Bsetting_Click(object sender, EventArgs e)
        {
            ImageSitB.ShowSettingDlg("参数设置SitB");
        }

        private void Bdevice_Click(object sender, EventArgs e)
        {
            ImageSitB.ShowSetDeviceDlg("管脚定义SitB");
        }

        private void Bpin_Click(object sender, EventArgs e)
        {
            ImageSitB.ShowSetPinDlg("管脚定义SitB");
        }

        private void ASave_Click(object sender, EventArgs e)
        {

        }

        private void BSave_Click(object sender, EventArgs e)
        {

        }

        public void ResetAndStopCamera()
        {
            ImageSitA.StopCamera();
            ImageSitB.StopCamera();
        }
        #endregion

        #region UI界面更新
        public void UpdateUIData()
        {
            labOperator.Text = Operator;
            labAuthority.Text = Authority;
            labProductModel.Text = ProductModel;
            labProductBarcode.Text = ProductBarcode;

            labProductCountDis.Text = Config.Instance.ProductCountDis;
            labOKCountDis.Text = Config.Instance.OKCountDis;
            labNGCountDis.Text = Config.Instance.NGCountDis;
        }

        public void ProductionReset()
        {
            AppendText(string.Format("总产量：{0}，OK品：{1}，NG品：{2}。全部清零！", labProductCountDis.Text, labOKCountDis.Text, labNGCountDis.Text));
        }

        public void productCount(bool? isOK)
        {
            if (isOK != null)
            {
                if ((bool)isOK)
                {
                    setCtrTxt(labOKCountDis, (int.Parse(labOKCountDis.Text) + 1).ToString());
                    Config.Instance.OKCountDis = labOKCountDis.Text;
                    //IoPoints.TDO14.Value = true; //双色灯绿（OK信号灯）
                }
                else
                {
                    setCtrTxt(labNGCountDis, (int.Parse(labNGCountDis.Text) + 1).ToString());
                    Config.Instance.NGCountDis = labNGCountDis.Text;
                    //IoPoints.TDO15.Value = true; //双色灯红（NG信号灯）
                }
                setCtrTxt(labProductCountDis, (int.Parse(labProductCountDis.Text) + 1).ToString());
            }
            else
            {
                setCtrTxt(labOKCountDis, "0");
                setCtrTxt(labNGCountDis, "0");
                setCtrTxt(labProductCountDis, "0");
                Config.Instance.OKCountDis = "0";
                Config.Instance.NGCountDis = "0";
                
            }
            Config.Instance.ProductCountDis = labProductCountDis.Text;
        }

        private void setCtrTxt(Control ctr, string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Control, string>(setCtrTxt), ctr, str);
            }
            else
            {
                ctr.Text = str;
            }
        }

        public void ResetLeftPicBoxColor()
        {
            BlackPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            BlemishPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            BadPixelPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            WBPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            MTFPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            IRMTFPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            GrayPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            ColorPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            FOVPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            FPSPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            AlignmentPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            CurrentPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            VoltagePicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            PowerPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            FWPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            HotPixelPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            ShadingPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            DistortionPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            SNRPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            RotationPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            ChangeViewPicBoxA.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
        }

        public void ResetRightPicBoxColor()
        {
            BlackPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            BlemishPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            BadPixelPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            BlackPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            WBPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            MTFPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            IRMTFPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            GrayPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            ColorPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            FOVPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            FPSPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            AlignmentPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            CurrentPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            VoltagePicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            PowerPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            FWPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            HotPixelPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            ShadingPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            DistortionPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            SNRPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            RotationPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
            ChangeViewPicBoxB.Image = Image.FromFile(@".\Resources\Circle - Gray.png");
        }

        public void ChangePicBoxImg(string ControlName, bool res)
        {

            PictureBox picBox = (PictureBox)this.GetType().GetField(ControlName, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
            if (res)
            {
                picBox.Image = Image.FromFile(@".\Resources\GreenBall.png");
            }
            else
            {
                picBox.Image = Image.FromFile(@".\Resources\RedBall.png");
            }
        }

        public void RePaintUI()
        {
            Type type = typeof(Position.TestItemConfigParam);
            FieldInfo[] infos = type.GetFields();
            int index = 0;
            foreach (var item in infos)
            {
                string FirstChar = item.Name.Substring(0, 1);
                bool EndChar = item.Name.Contains("EN");
                if (EndChar && FirstChar == "b" && item.Name!="bAllTestEN")
                {
                    bool b = (bool)item.GetValue(Position.Instance.testItem);
                    if (b)
                    {
                        int length = item.Name.Length;
                        string PicNameA = item.Name.Substring(1).Substring(0, length - 3) + "PicBoxA";
                        string PicNameB = item.Name.Substring(1).Substring(0, length - 3) + "PicBoxB";
                        string LabNameA = "lab" + item.Name.Substring(1).Substring(0, length - 3) + "A";
                        string LabNameB = "lab" + item.Name.Substring(1).Substring(0, length - 3) + "B";
                        PictureBox picBoxA = (PictureBox)this.GetType().GetField(PicNameA, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        PictureBox picBoxB = (PictureBox)this.GetType().GetField(PicNameB, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        Label LabelA = (Label)this.GetType().GetField(LabNameA, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        Label LabelB = (Label)this.GetType().GetField(LabNameB, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);

                        int col = (index % 6);
                        int row = index / 6;
                        //Point A = new Point(RPos.FirstPoint_A.X + RPos.PicWidth * col, RPos.FirstPoint_A.Y + RPos.PicHeight * row);
                        //Point B = new Point(RPos.FirstPoint_B.X + RPos.PicWidth * col, RPos.FirstPoint_B.Y + RPos.PicHeight * row);
                        //Point AA = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);
                        //Point BB = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);

                        picBoxA.Location = new Point(RPos.FirstPoint_A.X + RPos.PicWidth * col, RPos.FirstPoint_A.Y + RPos.PicHeight * row);
                        picBoxB.Location = new Point(RPos.FirstPoint_B.X + RPos.PicWidth * col, RPos.FirstPoint_B.Y + RPos.PicHeight * row);
                        LabelA.Location = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);
                        LabelB.Location = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);

                        picBoxA.Visible = true;
                        picBoxB.Visible = true;
                        LabelA.Visible = true;
                        LabelB.Visible = true;
                        index++;
                    }
                }
            }
            foreach (var item in infos)
            {
                string FirstChar = item.Name.Substring(0, 1);
                bool EndChar = item.Name.Contains("EN");
                if (EndChar && FirstChar == "b" && item.Name!="bAllTestEN")
                {
                    bool b = (bool)item.GetValue(Position.Instance.testItem);
                    if (!b)
                    {
                        int length = item.Name.Length;
                        string PicNameA = item.Name.Substring(1).Substring(0, length - 3) + "PicBoxA";
                        string PicNameB = item.Name.Substring(1).Substring(0, length - 3) + "PicBoxB";
                        string LabNameA = "lab" + item.Name.Substring(1).Substring(0, length - 3) + "A";
                        string LabNameB = "lab" + item.Name.Substring(1).Substring(0, length - 3) + "B";
                        PictureBox picBoxA = (PictureBox)this.GetType().GetField(PicNameA, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        PictureBox picBoxB = (PictureBox)this.GetType().GetField(PicNameB, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        Label LabelA = (Label)this.GetType().GetField(LabNameA, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        Label LabelB = (Label)this.GetType().GetField(LabNameB, System.Reflection.BindingFlags.Instance |
                                                                                 System.Reflection.BindingFlags.NonPublic |
                                                                                 System.Reflection.BindingFlags.IgnoreCase).GetValue(this);

                        int col = (index % 6);
                        int row = index / 6;
                        //Point A1 = new Point(RPos.FirstPoint_A.X + RPos.PicWidth * col, RPos.FirstPoint_A.Y + RPos.PicHeight * row);
                        //Point B1= new Point(RPos.FirstPoint_B.X + RPos.PicWidth * col, RPos.FirstPoint_B.Y + RPos.PicHeight * row);
                        //Point AA1 = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);
                        //Point BB1 = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);

                        picBoxA.Location = new Point(RPos.FirstPoint_A.X + RPos.PicWidth * col, RPos.FirstPoint_A.Y + RPos.PicHeight * row);
                        picBoxB.Location = new Point(RPos.FirstPoint_B.X + RPos.PicWidth * col, RPos.FirstPoint_B.Y + RPos.PicHeight * row);
                        LabelA.Location = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);
                        LabelB.Location = new Point(RPos.LabFirstPoint_A.X + RPos.LabWidth * col, RPos.LabFirstPoint_A.Y + RPos.LabHeight * row);

                        picBoxA.Visible = false;
                        picBoxB.Visible = false;
                        LabelA.Visible = false;
                        LabelB.Visible = false;
                        index++;
                    }
                }
            }
        }

        #endregion
    }
}
