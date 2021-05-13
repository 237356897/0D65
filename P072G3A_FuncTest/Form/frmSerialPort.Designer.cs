namespace CommonLibrary
{
    partial class frmSerialPort
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSerialPort));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripCmbxComPort = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnSave = new System.Windows.Forms.ToolStripButton();
            this.txtRetries = new System.Windows.Forms.TextBox();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReadDT = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBBaudRate = new System.Windows.Forms.ComboBox();
            this.comboBDataBit = new System.Windows.Forms.ComboBox();
            this.comboBStopBit = new System.Windows.Forms.ComboBox();
            this.comboBParityBit = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.labDeviceModel = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnOpen,
            this.toolStripBtnClose,
            this.toolStripCmbxComPort,
            this.toolStripBtnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(352, 43);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripBtnOpen
            // 
            this.toolStripBtnOpen.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripBtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnOpen.Image = global::P072G3A_FuncTest.Properties.Resources.打开串口;
            this.toolStripBtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnOpen.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.toolStripBtnOpen.Name = "toolStripBtnOpen";
            this.toolStripBtnOpen.Size = new System.Drawing.Size(40, 40);
            this.toolStripBtnOpen.Text = "打开";
            this.toolStripBtnOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnOpen.ToolTipText = "打开";
            this.toolStripBtnOpen.Click += new System.EventHandler(this.toolStripBtnOpen_Click);
            // 
            // toolStripBtnClose
            // 
            this.toolStripBtnClose.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnClose.Image = global::P072G3A_FuncTest.Properties.Resources.关闭串口;
            this.toolStripBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnClose.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.toolStripBtnClose.Name = "toolStripBtnClose";
            this.toolStripBtnClose.Size = new System.Drawing.Size(40, 40);
            this.toolStripBtnClose.Text = "关闭";
            this.toolStripBtnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnClose.ToolTipText = "关闭";
            this.toolStripBtnClose.Click += new System.EventHandler(this.toolStripBtnClose_Click);
            // 
            // toolStripCmbxComPort
            // 
            this.toolStripCmbxComPort.AutoSize = false;
            this.toolStripCmbxComPort.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.toolStripCmbxComPort.Margin = new System.Windows.Forms.Padding(21, 0, 1, 0);
            this.toolStripCmbxComPort.Name = "toolStripCmbxComPort";
            this.toolStripCmbxComPort.Size = new System.Drawing.Size(75, 25);
            this.toolStripCmbxComPort.ToolTipText = "串口列表";
            // 
            // toolStripBtnSave
            // 
            this.toolStripBtnSave.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripBtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnSave.Image = global::P072G3A_FuncTest.Properties.Resources.Save;
            this.toolStripBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnSave.Margin = new System.Windows.Forms.Padding(60, 1, 0, 2);
            this.toolStripBtnSave.Name = "toolStripBtnSave";
            this.toolStripBtnSave.Size = new System.Drawing.Size(40, 40);
            this.toolStripBtnSave.Text = "保存";
            this.toolStripBtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnSave.Click += new System.EventHandler(this.toolStripBtnSave_Click);
            // 
            // txtRetries
            // 
            this.txtRetries.Location = new System.Drawing.Point(236, 88);
            this.txtRetries.Name = "txtRetries";
            this.txtRetries.Size = new System.Drawing.Size(104, 21);
            this.txtRetries.TabIndex = 17;
            this.txtRetries.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRetries_KeyPress);
            // 
            // txtCmd
            // 
            this.txtCmd.Location = new System.Drawing.Point(236, 115);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(104, 21);
            this.txtCmd.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(160, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "重试次数：";
            // 
            // btnReadDT
            // 
            this.btnReadDT.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnReadDT.Image = global::P072G3A_FuncTest.Properties.Resources.接收;
            this.btnReadDT.Location = new System.Drawing.Point(288, 205);
            this.btnReadDT.Name = "btnReadDT";
            this.btnReadDT.Size = new System.Drawing.Size(52, 50);
            this.btnReadDT.TabIndex = 12;
            this.toolTip1.SetToolTip(this.btnReadDT, "读取数据");
            this.btnReadDT.UseVisualStyleBackColor = false;
            this.btnReadDT.Click += new System.EventHandler(this.btnReadDT_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(160, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "触发命令：";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(236, 61);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(79, 21);
            this.txtTimeout.TabIndex = 22;
            this.txtTimeout.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimeout_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(186, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "超时：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(321, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "ms";
            // 
            // comboBBaudRate
            // 
            this.comboBBaudRate.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBBaudRate.FormattingEnabled = true;
            this.comboBBaudRate.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "56000",
            "57600",
            "115200"});
            this.comboBBaudRate.Location = new System.Drawing.Point(12, 62);
            this.comboBBaudRate.Name = "comboBBaudRate";
            this.comboBBaudRate.Size = new System.Drawing.Size(98, 20);
            this.comboBBaudRate.TabIndex = 24;
            this.toolTip1.SetToolTip(this.comboBBaudRate, "波特率");
            // 
            // comboBDataBit
            // 
            this.comboBDataBit.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBDataBit.FormattingEnabled = true;
            this.comboBDataBit.Items.AddRange(new object[] {
            "8bit",
            "7bit",
            "6bit",
            "5bit",
            "4bit"});
            this.comboBDataBit.Location = new System.Drawing.Point(12, 105);
            this.comboBDataBit.Name = "comboBDataBit";
            this.comboBDataBit.Size = new System.Drawing.Size(98, 20);
            this.comboBDataBit.TabIndex = 25;
            this.toolTip1.SetToolTip(this.comboBDataBit, "数据位");
            // 
            // comboBStopBit
            // 
            this.comboBStopBit.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBStopBit.FormattingEnabled = true;
            this.comboBStopBit.Items.AddRange(new object[] {
            "1bit",
            "2bit"});
            this.comboBStopBit.Location = new System.Drawing.Point(12, 148);
            this.comboBStopBit.Name = "comboBStopBit";
            this.comboBStopBit.Size = new System.Drawing.Size(98, 20);
            this.comboBStopBit.TabIndex = 26;
            this.toolTip1.SetToolTip(this.comboBStopBit, "停止位");
            // 
            // comboBParityBit
            // 
            this.comboBParityBit.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBParityBit.FormattingEnabled = true;
            this.comboBParityBit.Items.AddRange(new object[] {
            "None无",
            "Odd奇",
            "Even偶"});
            this.comboBParityBit.Location = new System.Drawing.Point(12, 191);
            this.comboBParityBit.Name = "comboBParityBit";
            this.comboBParityBit.Size = new System.Drawing.Size(98, 20);
            this.comboBParityBit.TabIndex = 27;
            this.toolTip1.SetToolTip(this.comboBParityBit, "校验位");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "设备-型号：";
            // 
            // labDeviceModel
            // 
            this.labDeviceModel.AutoSize = true;
            this.labDeviceModel.BackColor = System.Drawing.Color.Transparent;
            this.labDeviceModel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labDeviceModel.ForeColor = System.Drawing.Color.Green;
            this.labDeviceModel.Location = new System.Drawing.Point(95, 246);
            this.labDeviceModel.Name = "labDeviceModel";
            this.labDeviceModel.Size = new System.Drawing.Size(47, 12);
            this.labDeviceModel.TabIndex = 29;
            this.labDeviceModel.Text = "XXXX-XX";
            // 
            // frmSerialPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(352, 267);
            this.Controls.Add(this.labDeviceModel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBParityBit);
            this.Controls.Add(this.comboBStopBit);
            this.Controls.Add(this.comboBDataBit);
            this.Controls.Add(this.comboBBaudRate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTimeout);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.txtRetries);
            this.Controls.Add(this.txtCmd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnReadDT);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSerialPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSerialPort";
            this.Load += new System.EventHandler(this.frmSerialPort_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripBtnOpen;
        private System.Windows.Forms.ToolStripButton toolStripBtnClose;
        private System.Windows.Forms.ToolStripComboBox toolStripCmbxComPort;
        private System.Windows.Forms.ToolStripButton toolStripBtnSave;
        private System.Windows.Forms.TextBox txtRetries;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReadDT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBBaudRate;
        private System.Windows.Forms.ComboBox comboBDataBit;
        private System.Windows.Forms.ComboBox comboBStopBit;
        private System.Windows.Forms.ComboBox comboBParityBit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labDeviceModel;
    }
}