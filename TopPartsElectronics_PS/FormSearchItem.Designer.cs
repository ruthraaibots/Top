namespace TopPartsElectronics_PS
{
    partial class FormSearchItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public string OwnerName = "";
        public string CustomerCode = "";
        public string CustomerNames = "";
        public string CustomerNameF = "";

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
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textItemCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCustomerNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtCustomerNameF = new System.Windows.Forms.TextBox();
            this.txtCustomerCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dGProcess = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textItemCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtCustomerNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtCustomerNameF);
            this.groupBox1.Controls.Add(this.txtCustomerCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(963, 166);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(838, 23);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(83, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 24);
            this.label1.TabIndex = 80;
            this.label1.Text = "Customer Code :";
            // 
            // textItemCode
            // 
            this.textItemCode.Location = new System.Drawing.Point(207, 120);
            this.textItemCode.MaxLength = 50;
            this.textItemCode.Name = "textItemCode";
            this.textItemCode.Size = new System.Drawing.Size(166, 32);
            this.textItemCode.TabIndex = 5;
            this.textItemCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textItemCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textItemCode_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 24);
            this.label4.TabIndex = 77;
            this.label4.Text = "Item Code :";
            // 
            // txtCustomerNameS
            // 
            this.txtCustomerNameS.Location = new System.Drawing.Point(554, 37);
            this.txtCustomerNameS.MaxLength = 30;
            this.txtCustomerNameS.Name = "txtCustomerNameS";
            this.txtCustomerNameS.ReadOnly = true;
            this.txtCustomerNameS.Size = new System.Drawing.Size(136, 32);
            this.txtCustomerNameS.TabIndex = 3;
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
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(715, 24);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Search \n\r\n[F2]";
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
            this.txtCustomerCode.ReadOnly = true;
            this.txtCustomerCode.Size = new System.Drawing.Size(134, 32);
            this.txtCustomerCode.TabIndex = 2;
            this.txtCustomerCode.Text = "000000";
            this.txtCustomerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // dGProcess
            // 
            this.dGProcess.AllowUserToAddRows = false;
            this.dGProcess.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGProcess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGProcess.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dGProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.CODE,
            this.nameS,
            this.nameF});
            this.dGProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGProcess.Location = new System.Drawing.Point(0, 166);
            this.dGProcess.Name = "dGProcess";
            this.dGProcess.RowTemplate.Height = 24;
            this.dGProcess.Size = new System.Drawing.Size(963, 509);
            this.dGProcess.TabIndex = 104;
            this.dGProcess.TabStop = false;
            this.dGProcess.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_CellContentClick);
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 56;
            // 
            // CODE
            // 
            this.CODE.HeaderText = "Customer Code";
            this.CODE.Name = "CODE";
            this.CODE.ReadOnly = true;
            this.CODE.Width = 138;
            // 
            // nameS
            // 
            this.nameS.HeaderText = "Item Code";
            this.nameS.Name = "nameS";
            this.nameS.ReadOnly = true;
            this.nameS.Width = 105;
            // 
            // nameF
            // 
            this.nameF.HeaderText = "Item Name";
            this.nameF.Name = "nameF";
            this.nameF.ReadOnly = true;
            this.nameF.Width = 111;
            // 
            // FormSearchItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 675);
            this.Controls.Add(this.dGProcess);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormSearchItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Item";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSearchItem_FormClosed);
            this.Load += new System.EventHandler(this.FormSearchItem_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textItemCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCustomerNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtCustomerNameF;
        private System.Windows.Forms.TextBox txtCustomerCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameS;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameF;
    }
}