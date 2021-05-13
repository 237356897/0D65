namespace CommonLibrary
{
    partial class frmSocket
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSocket));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtSendData = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.comboBClients = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnConn = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnDisconn = new System.Windows.Forms.ToolStripButton();
            this.toolStripTxtIP = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTxtPort = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripCmbxSocketType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnSave = new System.Windows.Forms.ToolStripButton();
            this.label6 = new System.Windows.Forms.Label();
            this.labDeviceModel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReadData = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSendData
            // 
            this.txtSendData.Location = new System.Drawing.Point(12, 133);
            this.txtSendData.Multiline = true;
            this.txtSendData.Name = "txtSendData";
            this.txtSendData.Size = new System.Drawing.Size(528, 55);
            this.txtSendData.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtSendData, "输入需要发送的数据");
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnSend.Location = new System.Drawing.Point(12, 96);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(88, 31);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "发送";
            this.toolTip1.SetToolTip(this.btnSend, "发送");
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // comboBClients
            // 
            this.comboBClients.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBClients.FormattingEnabled = true;
            this.comboBClients.Location = new System.Drawing.Point(463, 49);
            this.comboBClients.Name = "comboBClients";
            this.comboBClients.Size = new System.Drawing.Size(77, 20);
            this.comboBClients.TabIndex = 19;
            this.toolTip1.SetToolTip(this.comboBClients, "已连接的客户端列表");
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnConn,
            this.toolStripBtnDisconn,
            this.toolStripTxtIP,
            this.toolStripSeparator1,
            this.toolStripTxtPort,
            this.toolStripCmbxSocketType,
            this.toolStripBtnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(552, 40);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripBtnConn
            // 
            this.toolStripBtnConn.AutoSize = false;
            this.toolStripBtnConn.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.toolStripBtnConn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnConn.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.toolStripBtnConn.Name = "toolStripBtnConn";
            this.toolStripBtnConn.Size = new System.Drawing.Size(60, 25);
            this.toolStripBtnConn.Text = "连接";
            this.toolStripBtnConn.Click += new System.EventHandler(this.toolStripBtnConn_Click);
            // 
            // toolStripBtnDisconn
            // 
            this.toolStripBtnDisconn.AutoSize = false;
            this.toolStripBtnDisconn.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.toolStripBtnDisconn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnDisconn.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.toolStripBtnDisconn.Name = "toolStripBtnDisconn";
            this.toolStripBtnDisconn.Size = new System.Drawing.Size(60, 25);
            this.toolStripBtnDisconn.Text = "断开连接";
            this.toolStripBtnDisconn.ToolTipText = "断开连接";
            this.toolStripBtnDisconn.Click += new System.EventHandler(this.toolStripBtnDisconn_Click);
            // 
            // toolStripTxtIP
            // 
            this.toolStripTxtIP.AutoSize = false;
            this.toolStripTxtIP.Margin = new System.Windows.Forms.Padding(20, 0, 1, 0);
            this.toolStripTxtIP.Name = "toolStripTxtIP";
            this.toolStripTxtIP.Size = new System.Drawing.Size(100, 25);
            this.toolStripTxtIP.ToolTipText = "IP地址";
            this.toolStripTxtIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTxtIP_KeyPress);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripTxtPort
            // 
            this.toolStripTxtPort.AutoSize = false;
            this.toolStripTxtPort.Margin = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.toolStripTxtPort.Name = "toolStripTxtPort";
            this.toolStripTxtPort.Size = new System.Drawing.Size(45, 25);
            this.toolStripTxtPort.ToolTipText = "端口";
            this.toolStripTxtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTxtPort_KeyPress);
            // 
            // toolStripCmbxSocketType
            // 
            this.toolStripCmbxSocketType.AutoSize = false;
            this.toolStripCmbxSocketType.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.toolStripCmbxSocketType.DropDownWidth = 75;
            this.toolStripCmbxSocketType.Items.AddRange(new object[] {
            "Client",
            "Server"});
            this.toolStripCmbxSocketType.Margin = new System.Windows.Forms.Padding(30, 0, 1, 0);
            this.toolStripCmbxSocketType.Name = "toolStripCmbxSocketType";
            this.toolStripCmbxSocketType.Size = new System.Drawing.Size(75, 25);
            this.toolStripCmbxSocketType.ToolTipText = "Socket类型";
            // 
            // toolStripBtnSave
            // 
            this.toolStripBtnSave.AutoSize = false;
            this.toolStripBtnSave.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.toolStripBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnSave.Margin = new System.Windows.Forms.Padding(50, 1, 0, 2);
            this.toolStripBtnSave.Name = "toolStripBtnSave";
            this.toolStripBtnSave.Size = new System.Drawing.Size(40, 30);
            this.toolStripBtnSave.Text = "保存";
            this.toolStripBtnSave.ToolTipText = "保存";
            this.toolStripBtnSave.Click += new System.EventHandler(this.toolStripBtnSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(12, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "设备-型号：";
            // 
            // labDeviceModel
            // 
            this.labDeviceModel.AutoSize = true;
            this.labDeviceModel.ForeColor = System.Drawing.Color.Green;
            this.labDeviceModel.Location = new System.Drawing.Point(89, 49);
            this.labDeviceModel.Name = "labDeviceModel";
            this.labDeviceModel.Size = new System.Drawing.Size(47, 12);
            this.labDeviceModel.TabIndex = 16;
            this.labDeviceModel.Text = "XXXX-XX";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReadData);
            this.groupBox1.Location = new System.Drawing.Point(12, 194);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 108);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "显示接收信息";
            // 
            // txtReadData
            // 
            this.txtReadData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReadData.Location = new System.Drawing.Point(3, 17);
            this.txtReadData.Multiline = true;
            this.txtReadData.Name = "txtReadData";
            this.txtReadData.ReadOnly = true;
            this.txtReadData.Size = new System.Drawing.Size(522, 88);
            this.txtReadData.TabIndex = 19;
            this.toolTip1.SetToolTip(this.txtReadData, "显示所读取的数据");
            // 
            // frmSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(552, 314);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBClients);
            this.Controls.Add(this.labDeviceModel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtSendData);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSocket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmSocket";
            this.TransparencyKey = System.Drawing.SystemColors.ButtonShadow;
            this.Load += new System.EventHandler(this.frmSocket_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripBtnConn;
        private System.Windows.Forms.ToolStripButton toolStripBtnDisconn;
        private System.Windows.Forms.ToolStripTextBox toolStripTxtIP;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTxtPort;
        private System.Windows.Forms.ToolStripButton toolStripBtnSave;
        private System.Windows.Forms.TextBox txtSendData;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ToolStripComboBox toolStripCmbxSocketType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labDeviceModel;
        private System.Windows.Forms.ComboBox comboBClients;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtReadData;
    }
}