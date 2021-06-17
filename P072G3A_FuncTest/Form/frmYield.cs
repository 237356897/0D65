using desay.ProductData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace P072G3A_FuncTest
{
    public partial class frmYield : Form
    {
        Position.ItemYield itemYield;
        bool isLeftStation;

        public frmYield(bool isleft)
        {
            InitializeComponent();
            isLeftStation = isleft;
            itemYield = isleft ? Position.Instance.ItemCount : Position.Instance.ItemCount1;
        }

        private void frmYield_Load(object sender, EventArgs e)
        {
            InitDataGridView();
        }

        public void InitDataGridView()
        {
            //清空
            dataGridView1.Rows.Clear();
            //添加内容
            double Total = Config.Instance.LeftOKCount + Config.Instance.LeftNGCount;
            if (!isLeftStation)
            {
                Total = Config.Instance.RightOKCount + Config.Instance.RightNGCount;
            }
            double OK = isLeftStation ? Config.Instance.LeftOKCount : Config.Instance.RightOKCount;
            double OKYield = 0;
            if (Total != 0)
            {
                OKYield = OK / Total;
            }
            dataGridView1.Rows.Add(new object[]
            {
                "总数",
                Total.ToString(),
                "100.00%",
            });
            dataGridView1.Rows.Add(new object[]
            {
                "OK",
                OK.ToString(),
                OKYield.ToString("P2"),
            });
            for (int i = 0; i < Position.Instance.ItemName.Length; i++)
            {
                bool isTest = GetStructChecked(Position.Instance.ItemName[i].Split(',')[0]);
                if (isTest && Total != 0)
                {
                    dataGridView1.Rows.Add(new object[]
                {
                    Position.Instance.ItemName[i].Split(',')[1],
                    GetStructCount(Position.Instance.ItemName[i].Split(',')[0], itemYield),
                    (GetStructCount(Position.Instance.ItemName[i].Split(',')[0], itemYield)/Total).ToString("P2"),
                });
                }
            }
        }

        public double GetStructCount(string sName, Position.ItemYield itemYield)
        {
            double Value = 0;
            Type type = typeof(Position.ItemYield);
            FieldInfo[] infos = type.GetFields();
            foreach (var item in infos)
            {
                if (item.Name == sName)
                {
                    Value = (double)item.GetValue(itemYield);
                    return Value;
                }
            }
            return Value;
        }

        public bool GetStructChecked(string sName)
        {
            bool Res = false;
            Type type = typeof(Position.TestItemConfigParam);
            FieldInfo[] infos = type.GetFields();
            foreach (var item in infos)
            {
                if (item.Name == sName)
                {
                    Res = (bool)item.GetValue(Position.Instance.testItem);
                    return Res;
                }
            }
            return Res;
        }

        public void ClearAllData()
        {
            //数据清零
            Type type = typeof(Position.ItemYield);
            FieldInfo[] infos = type.GetFields();
            foreach (var item in infos)
            {
                object BoxedItemCount = Position.Instance.ItemCount;
                item.SetValue(BoxedItemCount, 0);
                Position.Instance.ItemCount = (Position.ItemYield)BoxedItemCount;

                object BoxedItemCount1 = Position.Instance.ItemCount1;
                item.SetValue(BoxedItemCount1, 0);
                Position.Instance.ItemCount1 = (Position.ItemYield)BoxedItemCount1;
            }
        }
    }
}
