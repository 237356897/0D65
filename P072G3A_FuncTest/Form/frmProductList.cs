using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ToolKit;
using System.IO;
using desay.ProductData;
using System.Toolkit.Helpers;

namespace CommonLibrary
{
    public partial class frmProductList : Form
    {
        public static Common.void_StringDelegate AppendLog;
        public static Common.void_StringDelegate UpdateProduct;
        public static Common.void_StringDelegate NewAddProduct;
        public static Common.void_StringDelegate DeleteProduct;

        private string currentProduct { get { return labCurrentProduct.Text; } set { UpdateProduct(labCurrentProduct.Text = value); } }

        public frmProductList()
        {
            InitializeComponent();
        }

        private void frmProductList_Load(object sender, EventArgs e)
        {
            string[] strs = Config.Instance.ProductList;
            if (strs[0] != "")
            {
                listBProductList.Items.AddRange(strs);
            }
            currentProduct = Config.Instance.CurrentProduct;
        }

        #region Public Method
        public static string CreateFilePath(string fileName, string Directory)
        {
            string paramDirectory = "";
            switch (Directory)
            {
                case "Test":
                    paramDirectory = Config.Instance.TestDirectory;
                    break;
                case "Motion":
                    paramDirectory = Config.Instance.MotionDirectory;
                    break;
            }
            return string.Format("{0}{1}\\{2}", AppDomain.CurrentDomain.BaseDirectory, paramDirectory, fileName);
        }
        #endregion

        #region Private Method
        private void appendText(string str)
        {
            AppendLog.Invoke(string.Format("<<产品列表界面>>:: {0}", str));
        }

        private void SaveProduct(string product)
        {
            Config.Instance.CurrentProduct = product;
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);
        }

        #endregion

        #region FormOperation
        private void btnNewAdd_Click(object sender, EventArgs e)
        {
            if (txtNewProduct.Text != "")
            {
                if (listBProductList.Items.Contains(txtNewProduct.Text))
                {
                    MessageBox.Show("型号已存在，无需再新增！");
                    appendText("型号已存在，无需再新增！");
                }
                else
                {
                    listBProductList.Items.Add(txtNewProduct.Text);
                    string[] items = new string[listBProductList.Items.Count];
                    listBProductList.Items.CopyTo(items, 0);
                    Config.Instance.ProductList = items;
                    SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);

                    NewAddProduct(txtNewProduct.Text);
                    MessageBox.Show(string.Format("型号:{0}新增成功！", txtNewProduct.Text));
                    appendText(string.Format("型号:{0}新增成功！", txtNewProduct.Text));
                    txtNewProduct.Text = "";
                }
            }
            else
            {
                MessageBox.Show("型号不能为空！");
                appendText("型号不能为空！");
            }
        }

        private void btnDelegate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)listBProductList.SelectedItem))
            {
                MessageBox.Show("请选择需要删除的一个型号！");
                appendText("请选择需要删除的一个型号！");
            }
            else if ((string)listBProductList.SelectedItem == currentProduct)
            {
                MessageBox.Show("型号正在使用，无法删除！");
                appendText("型号正在使用，无法删除！");
            }
            else
            {
                string str = (string)listBProductList.SelectedItem;
                listBProductList.Items.Remove(str);
                string[] items = new string[listBProductList.Items.Count];
                listBProductList.Items.CopyTo(items, 0);
                Config.Instance.ProductList = items;
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath, Config.Instance);

                DeleteProduct(str);
                MessageBox.Show(string.Format("型号:{0}删除成功！", str));
                appendText(string.Format("型号:{0}删除成功！", str));
            }
        }

        private void btnChangeProduct_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)listBProductList.SelectedItem))
            {
                currentProduct = (string)listBProductList.SelectedItem;
                //算法窗体切换
                if (currentProduct == "FreTech")
                {
                    IniFile.WriteValue("ProjectConfig", "ModelName", "FVC2", AppConfig.ProjectCongigPath_A);
                    IniFile.WriteValue("ProjectConfig", "ModelName", "FVC2", AppConfig.ProjectCongigPath_B);
                }
                else
                {
                    IniFile.WriteValue("ProjectConfig", "ModelName", currentProduct, AppConfig.ProjectCongigPath_A);
                    IniFile.WriteValue("ProjectConfig", "ModelName", currentProduct, AppConfig.ProjectCongigPath_B);
                }
                SaveProduct(currentProduct);
                MessageBox.Show(string.Format("型号：{0}切换成功！", currentProduct));
                appendText(string.Format("型号：{0}切换成功！", currentProduct));
            }
            else
            {
                MessageBox.Show("请选择需要切换的一个型号！");
                appendText("请选择需要切换的一个型号！");
            }
        }
        #endregion

    }
}
