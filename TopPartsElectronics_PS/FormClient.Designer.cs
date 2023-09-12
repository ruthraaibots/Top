namespace TopPartsElectronics_PS
{
    partial class FormClient
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
            this.btnclient_down = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtCustomerNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtCustomerNameF = new System.Windows.Forms.TextBox();
            this.txtCustomerCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dGClient = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customercode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGClient)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnclient_down);
            this.groupBox1.Controls.Add(this.btn_refresh);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtCustomerNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtCustomerNameF);
            this.groupBox1.Controls.Add(this.txtCustomerCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(997, 201);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // btnclient_down
            // 
            this.btnclient_down.Location = new System.Drawing.Point(838, 22);
            this.btnclient_down.Name = "btnclient_down";
            this.btnclient_down.Size = new System.Drawing.Size(100, 83);
            this.btnclient_down.TabIndex = 108;
            this.btnclient_down.Text = "Download \n\r\n[F7]";
            this.btnclient_down.UseVisualStyleBackColor = true;
            this.btnclient_down.Click += new System.EventHandler(this.btnclient_down_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(715, 113);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(102, 82);
            this.btn_refresh.TabIndex = 77;
            this.btn_refresh.Text = "Refresh\r\n\r\n[F5]";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(591, 23);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCustomerNameS
            // 
            this.txtCustomerNameS.Location = new System.Drawing.Point(207, 83);
            this.txtCustomerNameS.MaxLength = 30;
            this.txtCustomerNameS.Name = "txtCustomerNameS";
            this.txtCustomerNameS.Size = new System.Drawing.Size(136, 32);
            this.txtCustomerNameS.TabIndex = 2;
            this.txtCustomerNameS.Enter += new System.EventHandler(this.text_enter);
            this.txtCustomerNameS.Leave += new System.EventHandler(this.text_leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 86);
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
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(716, 22);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 83);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete\r\n\r\n[F4]";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(468, 23);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add New\r\n\r\n[F2]";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtCustomerNameF
            // 
            this.txtCustomerNameF.Location = new System.Drawing.Point(207, 124);
            this.txtCustomerNameF.MaxLength = 50;
            this.txtCustomerNameF.Name = "txtCustomerNameF";
            this.txtCustomerNameF.Size = new System.Drawing.Size(468, 32);
            this.txtCustomerNameF.TabIndex = 3;
            this.txtCustomerNameF.Enter += new System.EventHandler(this.text_enter);
            this.txtCustomerNameF.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Location = new System.Drawing.Point(207, 37);
            this.txtCustomerCode.MaxLength = 6;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Size = new System.Drawing.Size(134, 32);
            this.txtCustomerCode.TabIndex = 1;
            this.txtCustomerCode.Text = "000000";
            this.txtCustomerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCustomerCode.TextChanged += new System.EventHandler(this.txtCustomerCode_TextChanged);
            this.txtCustomerCode.Enter += new System.EventHandler(this.text_enter);
            this.txtCustomerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtCustomerCode.Leave += new System.EventHandler(this.text_leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Customer Name (Full):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customer Code :";
            // 
            // dGClient
            // 
            this.dGClient.AllowUserToAddRows = false;
            this.dGClient.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGClient.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGClient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.customercode,
            this.nameS,
            this.fullname});
            this.dGClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGClient.Location = new System.Drawing.Point(0, 201);
            this.dGClient.Name = "dGClient";
            this.dGClient.RowTemplate.Height = 24;
            this.dGClient.Size = new System.Drawing.Size(997, 474);
            this.dGClient.TabIndex = 100;
            this.dGClient.TabStop = false;
            this.dGClient.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGClient_CellClick);
            this.dGClient.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGClient_CellClick);
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 50;
            // 
            // customercode
            // 
            this.customercode.HeaderText = "Customer Code";
            this.customercode.Name = "customercode";
            this.customercode.ReadOnly = true;
            this.customercode.Width = 150;
            // 
            // nameS
            // 
            this.nameS.HeaderText = "Customer Name (Short)";
            this.nameS.Name = "nameS";
            this.nameS.ReadOnly = true;
            this.nameS.Width = 200;
            // 
            // fullname
            // 
            this.fullname.HeaderText = "Customer Name (Full)";
            this.fullname.Name = "fullname";
            this.fullname.ReadOnly = true;
            this.fullname.Width = 450;
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 675);
            this.Controls.Add(this.dGClient);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormClient";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Customer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClient_Closing);
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormClient_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGClient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCustomerNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtCustomerNameF;
        private System.Windows.Forms.TextBox txtCustomerCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn customercode;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameS;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullname;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btnclient_down;
    }
}