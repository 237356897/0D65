using System;
using System.Drawing;
using System.Windows.Forms;

namespace Motion.Enginee
{
    public partial class CylinderOperate : UserControl
    {
        private readonly Func<bool> _manualOperate;
        public CylinderOperate()
        {
            InitializeComponent();
        }
        public CylinderOperate(Func<bool> ManualOperate) : this()
        {
            _manualOperate = ManualOperate;
        }
        public string CylinderName
        {
            set
            {
                btn.Text = value;
            }
        }
        public bool InOrigin
        {
            set
            {
                if (value)
                    picOrigin.BackColor = Color.Green;
                else
                    picOrigin.BackColor = Color.Red;
            }
        }
        public bool InMove
        {
            set
            {
                if (value)
                    picMove.BackColor = Color.Green;
                else
                    picMove.BackColor = Color.Red;
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            _manualOperate();
        }
    }
}
