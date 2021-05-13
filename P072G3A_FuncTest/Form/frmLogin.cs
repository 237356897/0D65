using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ToolKit;
using P072G3A_FuncTest;
using P072G3A_FuncTest.Data;

namespace CommonLibrary
{
    public partial class frmLogin : Form
    {
        public static Common.void_StringDelegate AppendLog;
        public static Common.void_StringDelegate UpdateOperator;


        private static string currentOperator = "";
        private static string currentAuthority = "";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            comboBoxRegisterAuthority.SelectedIndex = 0;
            UpdateOperator += updateOperator;
            loadUsersList();
            enabled(true);
            UpdateOperator(",");
            txtLoginPassword.Text = "";
        }

        #region Private Method

        private void appendText(string str)
        {
            AppendLog.Invoke(string.Format("<<用户界面>>:: {0}", str));
        }

        private void loadUsersList()
        {
            string[] items = Global.Instance.PassWord.Keys.ToArray<string>();
            if (items.Length > 0)
            {
                comboBLoginUser.Items.Clear();
                comboBLoginUser.Items.AddRange(items);
                comboBLoginUser.Items.Remove("desay");
                comboBLoginUser.SelectedItem = currentOperator;

                comboBModifyUser.Items.Clear();
                comboBModifyUser.Items.AddRange(items);
                comboBModifyUser.Items.Remove("desay");
                comboBModifyUser.SelectedItem = currentOperator;
            }

        }

        private void updateOperator(string operato)
        {
            string[] strTemps = operato.Split(',');
            currentOperator = strTemps[0];
            currentAuthority = strTemps[1];
        }

        private void enabled(bool enable)
        {
            btnLogin.Enabled=
            txtLoginPassword.Enabled=
            comboBLoginUser.Enabled = enable;

            btnLogout.Enabled = !enable;
        }

        private string getAuthority(string str)
        {
            string strReturn = "";
            switch (str)
            {
                case "操作员":
                    strReturn = "0";
                    break;

                case "技术员":
                    strReturn = "1";
                    break;

                case "管理员":
                    strReturn = "2";
                    break;

                case "开发员":
                    strReturn = "3";
                    break;
            }

            return strReturn;
        }
        #endregion

        #region 用户登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string[] user=new string[2];
            string password;
            if (comboBLoginUser.Text != "")
            {
                user[0] = comboBLoginUser.Text;
                user[1] = labelLoginAuthorityDis.Text;             
            }
            else
            {
                user[0] = "desay";
                user[1] = "开发员";              
            }

            password = Global.Instance.PassWord[user[0]].Split(',')[0];
            if (MD5.TextToMd5(txtLoginPassword.Text) == password)
            {
                MessageBox.Show("登陆成功：" + user[0]);
                appendText("登陆成功：" + user[0]);
                UpdateOperator(string.Join(",", user));
                enabled(false);
                frmMain.main.HometoolStripButton1_Click();
            }
            else
            {
                MessageBox.Show("登陆失败：密码不正确,请重新输入!");
                appendText("登陆失败：密码不正确,请重新输入!");
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            txtLoginPassword.Text = "";
            MessageBox.Show("注销登陆：" + currentOperator);
            appendText("注销登陆：" + currentOperator);

            UpdateOperator(",");
            enabled(true);
        }

        private void comboBLoginUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = Global.Instance.PassWord[comboBLoginUser.Text].Split(',')[1];

            if (MD5.TextToMd5("0") == strTemp)
            { labelLoginAuthorityDis.Text = "操作员"; }
            else if (MD5.TextToMd5("1") == strTemp)
            { labelLoginAuthorityDis.Text = "技术员"; }
            else if (MD5.TextToMd5("2") == strTemp)
            { labelLoginAuthorityDis.Text = "管理员"; }
            else
            { labelLoginAuthorityDis.Text = "开发员"; }
            //txtLoginPassword.Text = "";
        }

        #endregion

        #region 用户修改
        private void btnUserRemove_Click(object sender, EventArgs e)
        {
            if (currentOperator == "admin" || currentOperator == "desay")
            {
                if (comboBModifyUser.Text != "admin" && !string.IsNullOrEmpty(comboBModifyUser.Text))
                {
                    Global.Instance.PassWord.Remove(comboBModifyUser.Text);
                    MessageBox.Show("移除用户成功：" + comboBModifyUser.Text);
                    appendText("移除用户成功：" + comboBModifyUser.Text);
                    SerializableDictionary<string, string>.Instance.SaveDicXml(Global.Instance.PassWord);
                    loadUsersList();
                }
                else
                {
                    MessageBox.Show(string.Format("移除用户失败：({0})此用户无法移除！", comboBModifyUser.Text));
                    appendText(string.Format("移除用户失败：({0})此用户无法移除！", comboBModifyUser.Text));
                }
            }
            else
            {
                MessageBox.Show(string.Format("移除用户失败：({0})无权限使用移除功能！", currentOperator));
                appendText(string.Format("移除用户失败：({0})无权限使用移除功能！", currentOperator));
            }
        }

        private void btnModifyConfim_Click(object sender, EventArgs e)
        {
            string modifyUser = "";
            if (txtModifyNewPassword.Text == txtModifyNewPasswordConfim.Text)
            {
                if (currentOperator == "admin" || currentOperator == "desay")
                {
                    if (string.IsNullOrEmpty(comboBModifyUser.Text) && currentOperator != "desay")
                    {
                        MessageBox.Show("修改用户失败：用户名不能为空！！！");
                        appendText("修改用户失败：用户名不能为空！！！");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(comboBModifyUser.Text))
                        {
                            modifyUser = comboBModifyUser.Text;
                            string MD5Password = MD5.TextToMd5(txtModifyNewPassword.Text);
                            string MD5Authority = MD5.TextToMd5(getAuthority(labelModifyAuthorityDis.Text));
                            Global.Instance.PassWord[modifyUser] = MD5Password + "," + MD5Authority;
                        }
                        SerializableDictionary<string, string>.Instance.SaveDicXml(Global.Instance.PassWord);
                        MessageBox.Show("修改用户密码成功：" + modifyUser);
                        appendText("修改用户密码成功：" + modifyUser);
                    }
                }
                else if (string.IsNullOrEmpty(comboBModifyUser.Text))
                {
                    MessageBox.Show("修改用户失败：用户名不能为空！！！");
                    appendText("修改用户失败：用户名不能为空！！！");
                }
                else
                {
                    string oldPassword = Global.Instance.PassWord[comboBModifyUser.Text].Split(',')[0];
                    if (MD5.TextToMd5(txtModifyOldPassword.Text) == oldPassword)
                    {
                        Global.Instance.PassWord[comboBModifyUser.Text] = "MD5.TextToMd5(txtModifyNewPassword.Text), MD5.TextToMd5(getAuthority(labelModifyAuthorityDis.Text))";
                        SerializableDictionary<string, string>.Instance.SaveDicXml(Global.Instance.PassWord);
                        MessageBox.Show("修改用户密码成功：" + comboBModifyUser.Text);
                        appendText("修改用户密码成功：" + comboBModifyUser.Text);
                    }
                    else
                    {
                        MessageBox.Show("修改用户失败：旧密码输入错误！！！");
                        appendText("修改用户失败：旧密码输入错误！！！");
                    }
                }
            }
            else
            {
                MessageBox.Show("修改用户失败：新密码与新密码确认不一致！！！");
                appendText("修改用户失败：新密码与新密码确认不一致！！！");
            }
        }

        private void btnModifyClear_Click(object sender, EventArgs e)
        {
            txtModifyOldPassword.Text = "";
            txtModifyNewPassword.Text = "";
            txtModifyNewPasswordConfim.Text = "";
        }

        private void comboBModifyUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = Global.Instance.PassWord[comboBModifyUser.Text].Split(',')[1];

            if (MD5.TextToMd5("0") == strTemp)
            { labelModifyAuthorityDis.Text = "操作员"; }
            else if (MD5.TextToMd5("1") == strTemp)
            { labelModifyAuthorityDis.Text = "技术员"; }
            else if (MD5.TextToMd5("2") == strTemp)
            { labelModifyAuthorityDis.Text = "管理员"; }
            else
            { labelModifyAuthorityDis.Text = "开发员"; }
        }

        #endregion

        #region 用户注册
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (currentOperator == "admin" || currentOperator == "desay")
            {
                if (txtRegisterUser.Text.ToLower() != "desay"/* && txtRegisterUser.Text.ToLower() != "admin"*/ && !string.IsNullOrEmpty(txtRegisterUser.Text))
                {                
                    if (txtRegisterPassword.Text == txtRegisterPasswordConfim.Text)
                    {
                        if ((comboBoxRegisterAuthority.Text == "管理员" && txtRegisterUser.Text != "admin") || (comboBoxRegisterAuthority.Text != "管理员" && txtRegisterUser.Text == "admin"))
                        {
                            MessageBox.Show("注册用户失败：管理员用户不能随便注册，必须指定admin为管理员！！！");
                            appendText("注册用户失败：管理员用户不能随便注册，必须指定admin为管理员！！！");
                        }
                        else
                        {
                            Global.Instance.PassWord[txtRegisterUser.Text] = "MD5.TextToMd5(txtRegisterPassword.Text), MD5.TextToMd5(getAuthority(comboBoxRegisterAuthority.Text))";
                            SerializableDictionary<string, string>.Instance.SaveDicXml(Global.Instance.PassWord);
                            MessageBox.Show(string.Format("注册用户成功：{0},权限：{1}", txtRegisterUser.Text, comboBoxRegisterAuthority.Text));
                            appendText(string.Format("注册用户成功：{0},权限：{1}", txtRegisterUser.Text, comboBoxRegisterAuthority.Text));

                            loadUsersList();
                        }
                    }
                    else
                    {
                        MessageBox.Show("注册用户失败：密码与密码确认不一致！！！");
                        appendText("注册用户失败：密码与密码确认不一致！！！");
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("注册用户失败：({0})此用户无法注册！！！", txtRegisterUser.Text));
                    appendText(string.Format("注册用户失败：({0})此用户无法注册！！！", txtRegisterUser.Text));
                }
            }
            else
            {
                MessageBox.Show(string.Format("注册用户失败：({0})无权限使用注册功能！", currentOperator));
                appendText(string.Format("注册用户失败：({0})无权限使用注册功能！",currentOperator));
            }
        }

        private void btnRegisterClear_Click(object sender, EventArgs e)
        {
            txtRegisterUser.Text = "";
            txtRegisterPassword.Text = "";
            txtRegisterPasswordConfim.Text = "";
        }
        #endregion
               
    }
}
