namespace TopPartsElectronics_PS
{
    partial class FormBOM
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnbom_down = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.textMaterialName = new System.Windows.Forms.TextBox();
            this.btnSearchMaterial = new System.Windows.Forms.Button();
            this.cmbProcess = new System.Windows.Forms.ComboBox();
            this.btnSearchItem = new System.Windows.Forms.Button();
            this.textMaterialCode = new System.Windows.Forms.TextBox();
            this.btnSearchCustomer = new System.Windows.Forms.Button();
            this.textOrder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textItemName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textItemCode = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtCustomerNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtCustomerNameF = new System.Windows.Forms.TextBox();
            this.txtCustomerCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dGProcess_new = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess_new)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnbom_down);
            this.groupBox1.Controls.Add(this.btn_refresh);
            this.groupBox1.Controls.Add(this.textMaterialName);
            this.groupBox1.Controls.Add(this.btnSearchMaterial);
            this.groupBox1.Controls.Add(this.cmbProcess);
            this.groupBox1.Controls.Add(this.btnSearchItem);
            this.groupBox1.Controls.Add(this.textMaterialCode);
            this.groupBox1.Controls.Add(this.btnSearchCustomer);
            this.groupBox1.Controls.Add(this.textOrder);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textItemName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textItemCode);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtCustomerNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtCustomerNameF);
            this.groupBox1.Controls.Add(this.txtCustomerCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(1075, 295);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnbom_down
            // 
            this.btnbom_down.Location = new System.Drawing.Point(840, 205);
            this.btnbom_down.Name = "btnbom_down";
            this.btnbom_down.Size = new System.Drawing.Size(100, 83);
            this.btnbom_down.TabIndex = 112;
            this.btnbom_down.Text = "Download \n\r\n[F7]";
            this.btnbom_down.UseVisualStyleBackColor = true;
            this.btnbom_down.Click += new System.EventHandler(this.btnbom_down_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(715, 204);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(102, 82);
            this.btn_refresh.TabIndex = 105;
            this.btn_refresh.Text = "Refresh\r\n\r\n[F5]";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // textMaterialName
            // 
            this.textMaterialName.Location = new System.Drawing.Point(333, 248);
            this.textMaterialName.MaxLength = 50;
            this.textMaterialName.Name = "textMaterialName";
            this.textMaterialName.ReadOnly = true;
            this.textMaterialName.Size = new System.Drawing.Size(357, 32);
            this.textMaterialName.TabIndex = 12;
            this.textMaterialName.TabStop = false;
            // 
            // btnSearchMaterial
            // 
            this.btnSearchMaterial.Location = new System.Drawing.Point(76, 244);
            this.btnSearchMaterial.Name = "btnSearchMaterial";
            this.btnSearchMaterial.Size = new System.Drawing.Size(127, 42);
            this.btnSearchMaterial.TabIndex = 10;
            this.btnSearchMaterial.Text = "Material Code :";
            this.btnSearchMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchMaterial.UseVisualStyleBackColor = true;
            this.btnSearchMaterial.Click += new System.EventHandler(this.btnSearchMaterial_Click);
            // 
            // cmbProcess
            // 
            this.cmbProcess.FormattingEnabled = true;
            this.cmbProcess.Location = new System.Drawing.Point(493, 208);
            this.cmbProcess.Name = "cmbProcess";
            this.cmbProcess.Size = new System.Drawing.Size(197, 32);
            this.cmbProcess.TabIndex = 9;
            // 
            // btnSearchItem
            // 
            this.btnSearchItem.Location = new System.Drawing.Point(73, 121);
            this.btnSearchItem.Name = "btnSearchItem";
            this.btnSearchItem.Size = new System.Drawing.Size(127, 42);
            this.btnSearchItem.TabIndex = 5;
            this.btnSearchItem.Text = "Item Code :";
            this.btnSearchItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchItem.UseVisualStyleBackColor = true;
            this.btnSearchItem.Click += new System.EventHandler(this.btnSearchItem_Click);
            // 
            // textMaterialCode
            // 
            this.textMaterialCode.Location = new System.Drawing.Point(206, 248);
            this.textMaterialCode.MaxLength = 6;
            this.textMaterialCode.Name = "textMaterialCode";
            this.textMaterialCode.Size = new System.Drawing.Size(126, 32);
            this.textMaterialCode.TabIndex = 11;
            this.textMaterialCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textMaterialCode.Enter += new System.EventHandler(this.text_enter);
            this.textMaterialCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.textMaterialCode.Leave += new System.EventHandler(this.text_leave);
            // 
            // btnSearchCustomer
            // 
            this.btnSearchCustomer.Location = new System.Drawing.Point(74, 31);
            this.btnSearchCustomer.Name = "btnSearchCustomer";
            this.btnSearchCustomer.Size = new System.Drawing.Size(127, 42);
            this.btnSearchCustomer.TabIndex = 1;
            this.btnSearchCustomer.Text = "Customer Code :";
            this.btnSearchCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchCustomer.UseVisualStyleBackColor = true;
            this.btnSearchCustomer.Click += new System.EventHandler(this.btnSearchCustomer_Click);
            // 
            // textOrder
            // 
            this.textOrder.Location = new System.Drawing.Point(205, 208);
            this.textOrder.MaxLength = 6;
            this.textOrder.Name = "textOrder";
            this.textOrder.Size = new System.Drawing.Size(84, 32);
            this.textOrder.TabIndex = 8;
            this.textOrder.Text = "0";
            this.textOrder.Enter += new System.EventHandler(this.text_enter);
            this.textOrder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.textOrder.Leave += new System.EventHandler(this.textOrder_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(88, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 24);
            this.label7.TabIndex = 83;
            this.label7.Text = "Process Order :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(420, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 24);
            this.label6.TabIndex = 81;
            this.label6.Text = "Process :";
            // 
            // textItemName
            // 
            this.textItemName.Location = new System.Drawing.Point(207, 162);
            this.textItemName.MaxLength = 5;
            this.textItemName.Name = "textItemName";
            this.textItemName.ReadOnly = true;
            this.textItemName.Size = new System.Drawing.Size(483, 32);
            this.textItemName.TabIndex = 7;
            this.textItemName.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(110, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 24);
            this.label5.TabIndex = 79;
            this.label5.Text = "Item Name :";
            // 
            // textItemCode
            // 
            this.textItemCode.Location = new System.Drawing.Point(207, 123);
            this.textItemCode.MaxLength = 50;
            this.textItemCode.Name = "textItemCode";
            this.textItemCode.Size = new System.Drawing.Size(166, 32);
            this.textItemCode.TabIndex = 6;
            this.textItemCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textItemCode.Enter += new System.EventHandler(this.text_enter);
            this.textItemCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textItemCode_KeyDown);
            this.textItemCode.Leave += new System.EventHandler(this.text_leave);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(838, 24);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCustomerNameS
            // 
            this.txtCustomerNameS.Location = new System.Drawing.Point(554, 37);
            this.txtCustomerNameS.MaxLength = 30;
            this.txtCustomerNameS.Name = "txtCustomerNameS";
            this.txtCustomerNameS.Size = new System.Drawing.Size(136, 32);
            this.txtCustomerNameS.TabIndex = 3;
            this.txtCustomerNameS.Enter += new System.EventHandler(this.text_enter);
            this.txtCustomerNameS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomerNameS_KeyDown);
            this.txtCustomerNameS.Leave += new System.EventHandler(this.text_leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(384, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 24);
            this.label2.TabIndex = 76;
            this.label2.Text = "Customer Name (Short):";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(838, 113);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(715, 112);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 83);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete\r\n\r\n[F4]";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(715, 24);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Add New\r\n\r\n[F2]";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtCustomerNameF
            // 
            this.txtCustomerNameF.Location = new System.Drawing.Point(207, 81);
            this.txtCustomerNameF.MaxLength = 50;
            this.txtCustomerNameF.Name = "txtCustomerNameF";
            this.txtCustomerNameF.ReadOnly = true;
            this.txtCustomerNameF.Size = new System.Drawing.Size(483, 32);
            this.txtCustomerNameF.TabIndex = 4;
            this.txtCustomerNameF.TabStop = false;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Location = new System.Drawing.Point(207, 38);
            this.txtCustomerCode.MaxLength = 6;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Size = new System.Drawing.Size(134, 32);
            this.txtCustomerCode.TabIndex = 2;
            this.txtCustomerCode.Text = "000000";
            this.txtCustomerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCustomerCode.Enter += new System.EventHandler(this.text_enter);
            this.txtCustomerCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomerCode_KeyDown);
            this.txtCustomerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtCustomerCode.Leave += new System.EventHandler(this.text_leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Customer Name (Full) :";
            // 
            // dGProcess_new
            // 
            this.dGProcess_new.AllowUserToAddRows = false;
            this.dGProcess_new.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGProcess_new.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGProcess_new.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGProcess_new.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGProcess_new.Location = new System.Drawing.Point(0, 295);
            this.dGProcess_new.Name = "dGProcess_new";
            this.dGProcess_new.RowTemplate.Height = 24;
            this.dGProcess_new.Size = new System.Drawing.Size(1075, 380);
            this.dGProcess_new.TabIndex = 104;
            this.dGProcess_new.TabStop = false;
            this.dGProcess_new.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_new_CellContentClick);
            this.dGProcess_new.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_new_CellContentClick);
            // 
            // FormBOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 675);
            this.Controls.Add(this.dGProcess_new);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormBOM";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage BOM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBOM_Closing);
            this.Load += new System.EventHandler(this.FormBOM_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormBOM_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess_new)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textMaterialCode;
        private System.Windows.Forms.Button btnSearchCustomer;
        private System.Windows.Forms.TextBox textOrder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textItemName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textItemCode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCustomerNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtCustomerNameF;
        private System.Windows.Forms.TextBox txtCustomerCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProcess;
        private System.Windows.Forms.Button btnSearchItem;
        private System.Windows.Forms.Button btnSearchMaterial;
        private System.Windows.Forms.TextBox textMaterialName;
        private System.Windows.Forms.DataGridView dGProcess_new;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btnbom_down;
    }
}