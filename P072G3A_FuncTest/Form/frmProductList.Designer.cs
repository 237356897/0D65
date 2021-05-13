namespace CommonLibrary
{
    partial class frmProductList
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
            this.btnNewAdd = new System.Windows.Forms.Button();
            this.listBProductList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labCurrentProduct = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewProduct = new System.Windows.Forms.TextBox();
            this.btnDelegate = new System.Windows.Forms.Button();
            this.btnChangeProduct = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnNewAdd
            // 
            this.btnNewAdd.Location = new System.Drawing.Point(202, 169);
            this.btnNewAdd.Name = "btnNewAdd";
            this.btnNewAdd.Size = new System.Drawing.Size(129, 40);
            this.btnNewAdd.TabIndex = 0;
            this.btnNewAdd.Text = "新增";
            this.btnNewAdd.UseVisualStyleBackColor = true;
            this.btnNewAdd.Click += new System.EventHandler(this.btnNewAdd_Click);
            // 
            // listBProductList
            // 
            this.listBProductList.FormattingEnabled = true;
            this.listBProductList.ItemHeight = 12;
            this.listBProductList.Location = new System.Drawing.Point(14, 34);
            this.listBProductList.Name = "listBProductList";
            this.listBProductList.Size = new System.Drawing.Size(157, 304);
            this.listBProductList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(199, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前产品型号：";
            // 
            // labCurrentProduct
            // 
            this.labCurrentProduct.AutoSize = true;
            this.labCurrentProduct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labCurrentProduct.Location = new System.Drawing.Point(199, 51);
            this.labCurrentProduct.Name = "labCurrentProduct";
            this.labCurrentProduct.Size = new System.Drawing.Size(44, 16);
            this.labCurrentProduct.TabIndex = 3;
            this.labCurrentProduct.Text = "mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "产品型号列表：";
            // 
            // txtNewProduct
            // 
            this.txtNewProduct.Location = new System.Drawing.Point(202, 124);
            this.txtNewProduct.Name = "txtNewProduct";
            this.txtNewProduct.Size = new System.Drawing.Size(129, 21);
            this.txtNewProduct.TabIndex = 5;
            // 
            // btnDelegate
            // 
            this.btnDelegate.Location = new System.Drawing.Point(202, 225);
            this.btnDelegate.Name = "btnDelegate";
            this.btnDelegate.Size = new System.Drawing.Size(129, 40);
            this.btnDelegate.TabIndex = 6;
            this.btnDelegate.Text = "删除";
            this.btnDelegate.UseVisualStyleBackColor = true;
            this.btnDelegate.Click += new System.EventHandler(this.btnDelegate_Click);
            // 
            // btnChangeProduct
            // 
            this.btnChangeProduct.Location = new System.Drawing.Point(202, 282);
            this.btnChangeProduct.Name = "btnChangeProduct";
            this.btnChangeProduct.Size = new System.Drawing.Size(129, 40);
            this.btnChangeProduct.TabIndex = 7;
            this.btnChangeProduct.Text = "切换机种";
            this.btnChangeProduct.UseVisualStyleBackColor = true;
            this.btnChangeProduct.Click += new System.EventHandler(this.btnChangeProduct_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(199, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "新增产品型号：";
            // 
            // frmProductList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 356);
            this.Controls.Add(this.btnChangeProduct);
            this.Controls.Add(this.btnDelegate);
            this.Controls.Add(this.txtNewProduct);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labCurrentProduct);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBProductList);
            this.Controls.Add(this.btnNewAdd);
            this.Name = "frmProductList";
            this.Text = "frmProductList";
            this.Load += new System.EventHandler(this.frmProductList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewAdd;
        private System.Windows.Forms.ListBox listBProductList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labCurrentProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewProduct;
        private System.Windows.Forms.Button btnDelegate;
        private System.Windows.Forms.Button btnChangeProduct;
        private System.Windows.Forms.Label label3;
    }
}