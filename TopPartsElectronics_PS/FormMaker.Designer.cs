namespace TopPartsElectronics_PS
{
    partial class FormMaker
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
            this.btnmaker_down = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtMakerNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtMakerNameF = new System.Windows.Forms.TextBox();
            this.txtMakerCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dGMaker = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makercode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shortname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGMaker)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnmaker_down);
            this.groupBox1.Controls.Add(this.btn_refresh);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtMakerNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtMakerNameF);
            this.groupBox1.Controls.Add(this.txtMakerCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(997, 201);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btnmaker_down
            // 
            this.btnmaker_down.Location = new System.Drawing.Point(838, 15);
            this.btnmaker_down.Name = "btnmaker_down";
            this.btnmaker_down.Size = new System.Drawing.Size(100, 83);
            this.btnmaker_down.TabIndex = 109;
            this.btnmaker_down.Text = "Download \n\r\n[F7]";
            this.btnmaker_down.UseVisualStyleBackColor = true;
            this.btnmaker_down.Click += new System.EventHandler(this.btnclient_down_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(715, 113);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(102, 82);
            this.btn_refresh.TabIndex = 78;
            this.btn_refresh.Text = "Refresh\r\n\r\n[F5]";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(592, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtMakerNameS
            // 
            this.txtMakerNameS.Location = new System.Drawing.Point(207, 83);
            this.txtMakerNameS.MaxLength = 30;
            this.txtMakerNameS.Name = "txtMakerNameS";
            this.txtMakerNameS.Size = new System.Drawing.Size(136, 32);
            this.txtMakerNameS.TabIndex = 2;
            this.txtMakerNameS.Enter += new System.EventHandler(this.text_enter);
            this.txtMakerNameS.Leave += new System.EventHandler(this.text_leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 24);
            this.label2.TabIndex = 76;
            this.label2.Text = "Maker Name (Short):";
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
            this.btnDelete.Location = new System.Drawing.Point(715, 14);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 83);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete\r\n\r\n[F4]";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(469, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add New\r\n\r\n[F2]";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtMakerNameF
            // 
            this.txtMakerNameF.Location = new System.Drawing.Point(207, 124);
            this.txtMakerNameF.MaxLength = 50;
            this.txtMakerNameF.Name = "txtMakerNameF";
            this.txtMakerNameF.Size = new System.Drawing.Size(468, 32);
            this.txtMakerNameF.TabIndex = 3;
            this.txtMakerNameF.Enter += new System.EventHandler(this.text_enter);
            this.txtMakerNameF.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtMakerCode
            // 
            this.txtMakerCode.Location = new System.Drawing.Point(207, 37);
            this.txtMakerCode.MaxLength = 6;
            this.txtMakerCode.Name = "txtMakerCode";
            this.txtMakerCode.Size = new System.Drawing.Size(134, 32);
            this.txtMakerCode.TabIndex = 1;
            this.txtMakerCode.Text = "000000";
            this.txtMakerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMakerCode.Enter += new System.EventHandler(this.text_enter);
            this.txtMakerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtMakerCode.Leave += new System.EventHandler(this.text_leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Maker Name (Full):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Maker Code :";
            // 
            // dGMaker
            // 
            this.dGMaker.AllowUserToAddRows = false;
            this.dGMaker.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGMaker.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGMaker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGMaker.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.makercode,
            this.shortname,
            this.fullname});
            this.dGMaker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGMaker.Location = new System.Drawing.Point(0, 201);
            this.dGMaker.Name = "dGMaker";
            this.dGMaker.RowTemplate.Height = 24;
            this.dGMaker.Size = new System.Drawing.Size(997, 474);
            this.dGMaker.TabIndex = 101;
            this.dGMaker.TabStop = false;
            this.dGMaker.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGMaker_CellClick);
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 50;
            // 
            // makercode
            // 
            this.makercode.HeaderText = "Maker Code";
            this.makercode.Name = "makercode";
            this.makercode.ReadOnly = true;
            this.makercode.Width = 150;
            // 
            // shortname
            // 
            this.shortname.HeaderText = "Maker Name (Short)";
            this.shortname.Name = "shortname";
            this.shortname.ReadOnly = true;
            this.shortname.Width = 200;
            // 
            // fullname
            // 
            this.fullname.HeaderText = "Maker Name (Full)";
            this.fullname.Name = "fullname";
            this.fullname.ReadOnly = true;
            this.fullname.Width = 450;
            // 
            // FormMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 675);
            this.Controls.Add(this.dGMaker);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMaker";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMaker_Closing);
            this.Load += new System.EventHandler(this.FormMaker_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMaker_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGMaker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtMakerNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtMakerNameF;
        private System.Windows.Forms.TextBox txtMakerCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGMaker;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn makercode;
        private System.Windows.Forms.DataGridViewTextBoxColumn shortname;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullname;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btnmaker_down;
    }
}