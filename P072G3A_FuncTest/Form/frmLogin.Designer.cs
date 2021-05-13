namespace CommonLibrary
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelLoginAuthorityDis = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.comboBLoginUser = new System.Windows.Forms.ComboBox();
            this.txtLoginPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labelModifyAuthorityDis = new System.Windows.Forms.Label();
            this.btnUserRemove = new System.Windows.Forms.Button();
            this.btnModifyClear = new System.Windows.Forms.Button();
            this.btnModifyConfim = new System.Windows.Forms.Button();
            this.txtModifyNewPasswordConfim = new System.Windows.Forms.TextBox();
            this.txtModifyNewPassword = new System.Windows.Forms.TextBox();
            this.txtModifyOldPassword = new System.Windows.Forms.TextBox();
            this.comboBModifyUser = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxRegisterAuthority = new System.Windows.Forms.ComboBox();
            this.btnRegisterClear = new System.Windows.Forms.Button();
            this.txtRegisterPasswordConfim = new System.Windows.Forms.TextBox();
            this.txtRegisterPassword = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.txtRegisterUser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(404, 257);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Silver;
            this.tabPage1.Controls.Add(this.labelLoginAuthorityDis);
            this.tabPage1.Controls.Add(this.btnLogout);
            this.tabPage1.Controls.Add(this.comboBLoginUser);
            this.tabPage1.Controls.Add(this.txtLoginPassword);
            this.tabPage1.Controls.Add(this.btnLogin);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(396, 231);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "用户登录";
            // 
            // labelLoginAuthorityDis
            // 
            this.labelLoginAuthorityDis.AutoSize = true;
            this.labelLoginAuthorityDis.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLoginAuthorityDis.Location = new System.Drawing.Point(281, 62);
            this.labelLoginAuthorityDis.Name = "labelLoginAuthorityDis";
            this.labelLoginAuthorityDis.Size = new System.Drawing.Size(57, 12);
            this.labelLoginAuthorityDis.TabIndex = 9;
            this.labelLoginAuthorityDis.Text = "权限显示";
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(233, 172);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 8;
            this.btnLogout.Text = "注销";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // comboBLoginUser
            // 
            this.comboBLoginUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBLoginUser.FormattingEnabled = true;
            this.comboBLoginUser.Location = new System.Drawing.Point(154, 59);
            this.comboBLoginUser.Name = "comboBLoginUser";
            this.comboBLoginUser.Size = new System.Drawing.Size(121, 20);
            this.comboBLoginUser.TabIndex = 7;
            this.comboBLoginUser.SelectedIndexChanged += new System.EventHandler(this.comboBLoginUser_SelectedIndexChanged);
            // 
            // txtLoginPassword
            // 
            this.txtLoginPassword.Location = new System.Drawing.Point(154, 114);
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '*';
            this.txtLoginPassword.Size = new System.Drawing.Size(121, 21);
            this.txtLoginPassword.TabIndex = 6;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(106, 172);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "登陆";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(104, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "密码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(78, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "用户选择：";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Silver;
            this.tabPage2.Controls.Add(this.labelModifyAuthorityDis);
            this.tabPage2.Controls.Add(this.btnUserRemove);
            this.tabPage2.Controls.Add(this.btnModifyClear);
            this.tabPage2.Controls.Add(this.btnModifyConfim);
            this.tabPage2.Controls.Add(this.txtModifyNewPasswordConfim);
            this.tabPage2.Controls.Add(this.txtModifyNewPassword);
            this.tabPage2.Controls.Add(this.txtModifyOldPassword);
            this.tabPage2.Controls.Add(this.comboBModifyUser);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(396, 231);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "用户修改";
            // 
            // labelModifyAuthorityDis
            // 
            this.labelModifyAuthorityDis.AutoSize = true;
            this.labelModifyAuthorityDis.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelModifyAuthorityDis.Location = new System.Drawing.Point(155, 9);
            this.labelModifyAuthorityDis.Name = "labelModifyAuthorityDis";
            this.labelModifyAuthorityDis.Size = new System.Drawing.Size(57, 12);
            this.labelModifyAuthorityDis.TabIndex = 11;
            this.labelModifyAuthorityDis.Text = "权限显示";
            // 
            // btnUserRemove
            // 
            this.btnUserRemove.Location = new System.Drawing.Point(302, 22);
            this.btnUserRemove.Name = "btnUserRemove";
            this.btnUserRemove.Size = new System.Drawing.Size(75, 23);
            this.btnUserRemove.TabIndex = 10;
            this.btnUserRemove.Text = "用户移除";
            this.btnUserRemove.UseVisualStyleBackColor = true;
            this.btnUserRemove.Click += new System.EventHandler(this.btnUserRemove_Click);
            // 
            // btnModifyClear
            // 
            this.btnModifyClear.Location = new System.Drawing.Point(236, 181);
            this.btnModifyClear.Name = "btnModifyClear";
            this.btnModifyClear.Size = new System.Drawing.Size(75, 23);
            this.btnModifyClear.TabIndex = 9;
            this.btnModifyClear.Text = "清空";
            this.btnModifyClear.UseVisualStyleBackColor = true;
            this.btnModifyClear.Click += new System.EventHandler(this.btnModifyClear_Click);
            // 
            // btnModifyConfim
            // 
            this.btnModifyConfim.Location = new System.Drawing.Point(112, 181);
            this.btnModifyConfim.Name = "btnModifyConfim";
            this.btnModifyConfim.Size = new System.Drawing.Size(75, 23);
            this.btnModifyConfim.TabIndex = 8;
            this.btnModifyConfim.Text = "确认";
            this.btnModifyConfim.UseVisualStyleBackColor = true;
            this.btnModifyConfim.Click += new System.EventHandler(this.btnModifyConfim_Click);
            // 
            // txtModifyNewPasswordConfim
            // 
            this.txtModifyNewPasswordConfim.Location = new System.Drawing.Point(157, 146);
            this.txtModifyNewPasswordConfim.Name = "txtModifyNewPasswordConfim";
            this.txtModifyNewPasswordConfim.PasswordChar = '*';
            this.txtModifyNewPasswordConfim.Size = new System.Drawing.Size(121, 21);
            this.txtModifyNewPasswordConfim.TabIndex = 7;
            // 
            // txtModifyNewPassword
            // 
            this.txtModifyNewPassword.Location = new System.Drawing.Point(157, 105);
            this.txtModifyNewPassword.Name = "txtModifyNewPassword";
            this.txtModifyNewPassword.PasswordChar = '*';
            this.txtModifyNewPassword.Size = new System.Drawing.Size(121, 21);
            this.txtModifyNewPassword.TabIndex = 6;
            // 
            // txtModifyOldPassword
            // 
            this.txtModifyOldPassword.Location = new System.Drawing.Point(157, 64);
            this.txtModifyOldPassword.Name = "txtModifyOldPassword";
            this.txtModifyOldPassword.PasswordChar = '*';
            this.txtModifyOldPassword.Size = new System.Drawing.Size(121, 21);
            this.txtModifyOldPassword.TabIndex = 5;
            // 
            // comboBModifyUser
            // 
            this.comboBModifyUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBModifyUser.FormattingEnabled = true;
            this.comboBModifyUser.Location = new System.Drawing.Point(157, 24);
            this.comboBModifyUser.Name = "comboBModifyUser";
            this.comboBModifyUser.Size = new System.Drawing.Size(121, 20);
            this.comboBModifyUser.TabIndex = 4;
            this.comboBModifyUser.SelectedIndexChanged += new System.EventHandler(this.comboBModifyUser_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(74, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "新密码确认：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(98, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "新密码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(98, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "旧密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(110, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户：";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Silver;
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.comboBoxRegisterAuthority);
            this.tabPage3.Controls.Add(this.btnRegisterClear);
            this.tabPage3.Controls.Add(this.txtRegisterPasswordConfim);
            this.tabPage3.Controls.Add(this.txtRegisterPassword);
            this.tabPage3.Controls.Add(this.btnRegister);
            this.tabPage3.Controls.Add(this.txtRegisterUser);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(396, 231);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "用户注册";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(114, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 12);
            this.label10.TabIndex = 9;
            this.label10.Text = "权限：";
            // 
            // comboBoxRegisterAuthority
            // 
            this.comboBoxRegisterAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegisterAuthority.FormattingEnabled = true;
            this.comboBoxRegisterAuthority.Items.AddRange(new object[] {
            "操作员",
            "技术员",
            "管理员"});
            this.comboBoxRegisterAuthority.Location = new System.Drawing.Point(164, 15);
            this.comboBoxRegisterAuthority.Name = "comboBoxRegisterAuthority";
            this.comboBoxRegisterAuthority.Size = new System.Drawing.Size(100, 20);
            this.comboBoxRegisterAuthority.TabIndex = 8;
            // 
            // btnRegisterClear
            // 
            this.btnRegisterClear.Location = new System.Drawing.Point(237, 166);
            this.btnRegisterClear.Name = "btnRegisterClear";
            this.btnRegisterClear.Size = new System.Drawing.Size(75, 23);
            this.btnRegisterClear.TabIndex = 7;
            this.btnRegisterClear.Text = "清空";
            this.btnRegisterClear.UseVisualStyleBackColor = true;
            this.btnRegisterClear.Click += new System.EventHandler(this.btnRegisterClear_Click);
            // 
            // txtRegisterPasswordConfim
            // 
            this.txtRegisterPasswordConfim.Location = new System.Drawing.Point(164, 123);
            this.txtRegisterPasswordConfim.Name = "txtRegisterPasswordConfim";
            this.txtRegisterPasswordConfim.PasswordChar = '*';
            this.txtRegisterPasswordConfim.Size = new System.Drawing.Size(100, 21);
            this.txtRegisterPasswordConfim.TabIndex = 6;
            // 
            // txtRegisterPassword
            // 
            this.txtRegisterPassword.Location = new System.Drawing.Point(164, 82);
            this.txtRegisterPassword.Name = "txtRegisterPassword";
            this.txtRegisterPassword.PasswordChar = '*';
            this.txtRegisterPassword.Size = new System.Drawing.Size(100, 21);
            this.txtRegisterPassword.TabIndex = 5;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(116, 166);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(75, 23);
            this.btnRegister.TabIndex = 4;
            this.btnRegister.Text = "注册";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // txtRegisterUser
            // 
            this.txtRegisterUser.Location = new System.Drawing.Point(164, 41);
            this.txtRegisterUser.Name = "txtRegisterUser";
            this.txtRegisterUser.Size = new System.Drawing.Size(100, 21);
            this.txtRegisterUser.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(88, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "密码确认：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(114, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "密码：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(101, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "新用户：";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(404, 257);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.Text = "用户";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox comboBLoginUser;
        private System.Windows.Forms.TextBox txtLoginPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnModifyClear;
        private System.Windows.Forms.Button btnModifyConfim;
        private System.Windows.Forms.TextBox txtModifyNewPasswordConfim;
        private System.Windows.Forms.TextBox txtModifyNewPassword;
        private System.Windows.Forms.TextBox txtModifyOldPassword;
        private System.Windows.Forms.ComboBox comboBModifyUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnRegisterClear;
        private System.Windows.Forms.TextBox txtRegisterPasswordConfim;
        private System.Windows.Forms.TextBox txtRegisterPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TextBox txtRegisterUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnUserRemove;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxRegisterAuthority;
        private System.Windows.Forms.Label labelLoginAuthorityDis;
        private System.Windows.Forms.Label labelModifyAuthorityDis;
    }
}