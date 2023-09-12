namespace TopPartsElectronics_PS
{
    partial class FormProcess
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
            this.btnprocess_down = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.textShowOrder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboLType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtProcessNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtProcessNameF = new System.Windows.Forms.TextBox();
            this.txtProcessCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dGProcess = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shortname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.showorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnprocess_down);
            this.groupBox1.Controls.Add(this.btn_refresh);
            this.groupBox1.Controls.Add(this.textShowOrder);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboLType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtProcessNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtProcessNameF);
            this.groupBox1.Controls.Add(this.txtProcessCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(960, 201);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btnprocess_down
            // 
            this.btnprocess_down.Location = new System.Drawing.Point(601, 111);
            this.btnprocess_down.Name = "btnprocess_down";
            this.btnprocess_down.Size = new System.Drawing.Size(100, 83);
            this.btnprocess_down.TabIndex = 111;
            this.btnprocess_down.Text = "Download \n\r\n[F7]";
            this.btnprocess_down.UseVisualStyleBackColor = true;
            this.btnprocess_down.Click += new System.EventHandler(this.btnprocess_down_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(724, 111);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(102, 82);
            this.btn_refresh.TabIndex = 79;
            this.btn_refresh.Text = "Refresh\r\n\r\n[F5]";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // textShowOrder
            // 
            this.textShowOrder.Location = new System.Drawing.Point(495, 83);
            this.textShowOrder.MaxLength = 30;
            this.textShowOrder.Name = "textShowOrder";
            this.textShowOrder.Size = new System.Drawing.Size(98, 32);
            this.textShowOrder.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(341, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 24);
            this.label5.TabIndex = 78;
            this.label5.Text = "Process Show Order :";
            // 
            // comboLType
            // 
            this.comboLType.FormattingEnabled = true;
            this.comboLType.ItemHeight = 24;
            this.comboLType.Location = new System.Drawing.Point(206, 166);
            this.comboLType.Name = "comboLType";
            this.comboLType.Size = new System.Drawing.Size(215, 32);
            this.comboLType.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 24);
            this.label4.TabIndex = 77;
            this.label4.Text = "Input Screen Type :";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(724, 23);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtProcessNameS
            // 
            this.txtProcessNameS.Location = new System.Drawing.Point(207, 83);
            this.txtProcessNameS.MaxLength = 30;
            this.txtProcessNameS.Name = "txtProcessNameS";
            this.txtProcessNameS.Size = new System.Drawing.Size(98, 32);
            this.txtProcessNameS.TabIndex = 2;
            this.txtProcessNameS.Enter += new System.EventHandler(this.text_enter);
            this.txtProcessNameS.Leave += new System.EventHandler(this.text_leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 24);
            this.label2.TabIndex = 76;
            this.label2.Text = "Process Name (Short) :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(846, 111);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(848, 22);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 83);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete\r\n\r\n[F4]";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(601, 23);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add New\r\n\r\n[F2]";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtProcessNameF
            // 
            this.txtProcessNameF.Location = new System.Drawing.Point(207, 124);
            this.txtProcessNameF.MaxLength = 50;
            this.txtProcessNameF.Name = "txtProcessNameF";
            this.txtProcessNameF.Size = new System.Drawing.Size(386, 32);
            this.txtProcessNameF.TabIndex = 4;
            this.txtProcessNameF.Enter += new System.EventHandler(this.text_enter);
            this.txtProcessNameF.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtProcessCode
            // 
            this.txtProcessCode.Location = new System.Drawing.Point(207, 37);
            this.txtProcessCode.MaxLength = 4;
            this.txtProcessCode.Name = "txtProcessCode";
            this.txtProcessCode.Size = new System.Drawing.Size(98, 32);
            this.txtProcessCode.TabIndex = 1;
            this.txtProcessCode.Text = "000";
            this.txtProcessCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtProcessCode.Enter += new System.EventHandler(this.text_enter);
            this.txtProcessCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtProcessCode.Leave += new System.EventHandler(this.text_leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Process Name (Full) :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Process Code :";
            // 
            // dGProcess
            // 
            this.dGProcess.AllowUserToAddRows = false;
            this.dGProcess.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGProcess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.processcode,
            this.shortname,
            this.fullname,
            this.showorder});
            this.dGProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGProcess.Location = new System.Drawing.Point(0, 201);
            this.dGProcess.Name = "dGProcess";
            this.dGProcess.RowTemplate.Height = 24;
            this.dGProcess.Size = new System.Drawing.Size(960, 474);
            this.dGProcess.TabIndex = 101;
            this.dGProcess.TabStop = false;
            this.dGProcess.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_CellContentClick);
            this.dGProcess.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_CellContentClick);
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 70;
            // 
            // processcode
            // 
            this.processcode.HeaderText = "Process Code";
            this.processcode.Name = "processcode";
            this.processcode.ReadOnly = true;
            this.processcode.Width = 150;
            // 
            // shortname
            // 
            this.shortname.HeaderText = "Process Name (Short)";
            this.shortname.Name = "shortname";
            this.shortname.ReadOnly = true;
            this.shortname.Width = 200;
            // 
            // fullname
            // 
            this.fullname.HeaderText = "Process Name (Full)";
            this.fullname.Name = "fullname";
            this.fullname.ReadOnly = true;
            this.fullname.Width = 450;
            // 
            // showorder
            // 
            this.showorder.HeaderText = "Show Order";
            this.showorder.Name = "showorder";
            this.showorder.Width = 50;
            // 
            // FormProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 675);
            this.Controls.Add(this.dGProcess);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormProcess";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Process";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProcess_Closing);
            this.Load += new System.EventHandler(this.FormProcess_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormProcess_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtProcessNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtProcessNameF;
        private System.Windows.Forms.TextBox txtProcessCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGProcess;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboLType;
        private System.Windows.Forms.TextBox textShowOrder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btnprocess_down;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn processcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn shortname;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullname;
        private System.Windows.Forms.DataGridViewTextBoxColumn showorder;
    }
}