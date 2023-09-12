namespace TopPartsElectronics_PS
{
    partial class FormSearchMaker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public string OwnerName = "";

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMakerNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtMakerCode = new System.Windows.Forms.TextBox();
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
            this.groupBox1.Controls.Add(this.txtMakerNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtMakerCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(900, 134);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // txtMakerNameS
            // 
            this.txtMakerNameS.Location = new System.Drawing.Point(207, 83);
            this.txtMakerNameS.MaxLength = 30;
            this.txtMakerNameS.Name = "txtMakerNameS";
            this.txtMakerNameS.Size = new System.Drawing.Size(136, 32);
            this.txtMakerNameS.TabIndex = 2;
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
            this.btnClose.Location = new System.Drawing.Point(767, 24);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(643, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(102, 82);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search\r\n\r\n[F2]";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
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
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGMaker.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dGMaker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGMaker.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.makercode,
            this.shortname,
            this.fullname});
            this.dGMaker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGMaker.Location = new System.Drawing.Point(0, 134);
            this.dGMaker.Name = "dGMaker";
            this.dGMaker.RowTemplate.Height = 24;
            this.dGMaker.Size = new System.Drawing.Size(900, 456);
            this.dGMaker.TabIndex = 102;
            this.dGMaker.TabStop = false;
            this.dGMaker.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGMaker_CellContentClick);
            this.dGMaker.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGMaker_CellContentClick);
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
            // FormSearchMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 590);
            this.Controls.Add(this.dGMaker);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormSearchMaker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Maker";
            this.Load += new System.EventHandler(this.FormSearchMaker_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormSearchMaker_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGMaker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMakerNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtMakerCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGMaker;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn makercode;
        private System.Windows.Forms.DataGridViewTextBoxColumn shortname;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullname;
    }
}