namespace TopPartsElectronics_PS
{
    partial class FormSearchMaterial
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMakerName = new System.Windows.Forms.TextBox();
            this.txtMakerCode = new System.Windows.Forms.TextBox();
            this.btnSearchMaker = new System.Windows.Forms.Button();
            this.txtMaterialNameS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtMaterialCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dGProcess = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.material_fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker_fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMakerName);
            this.groupBox1.Controls.Add(this.txtMakerCode);
            this.groupBox1.Controls.Add(this.btnSearchMaker);
            this.groupBox1.Controls.Add(this.txtMaterialNameS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtMaterialCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(1033, 225);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // txtMakerName
            // 
            this.txtMakerName.Location = new System.Drawing.Point(306, 30);
            this.txtMakerName.MaxLength = 50;
            this.txtMakerName.Name = "txtMakerName";
            this.txtMakerName.ReadOnly = true;
            this.txtMakerName.Size = new System.Drawing.Size(374, 32);
            this.txtMakerName.TabIndex = 2;
            this.txtMakerName.TabStop = false;
            // 
            // txtMakerCode
            // 
            this.txtMakerCode.Location = new System.Drawing.Point(206, 30);
            this.txtMakerCode.MaxLength = 6;
            this.txtMakerCode.Name = "txtMakerCode";
            this.txtMakerCode.Size = new System.Drawing.Size(99, 32);
            this.txtMakerCode.TabIndex = 1;
            this.txtMakerCode.Text = "000000";
            this.txtMakerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMakerCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMakerCode_KeyDown);
            // 
            // btnSearchMaker
            // 
            this.btnSearchMaker.Location = new System.Drawing.Point(91, 24);
            this.btnSearchMaker.Name = "btnSearchMaker";
            this.btnSearchMaker.Size = new System.Drawing.Size(109, 42);
            this.btnSearchMaker.TabIndex = 77;
            this.btnSearchMaker.Text = "Maker Code :";
            this.btnSearchMaker.UseVisualStyleBackColor = true;
            this.btnSearchMaker.Click += new System.EventHandler(this.btnSearchMaker_Click);
            // 
            // txtMaterialNameS
            // 
            this.txtMaterialNameS.Location = new System.Drawing.Point(207, 117);
            this.txtMaterialNameS.MaxLength = 30;
            this.txtMaterialNameS.Name = "txtMaterialNameS";
            this.txtMaterialNameS.Size = new System.Drawing.Size(98, 32);
            this.txtMaterialNameS.TabIndex = 4;
            this.txtMaterialNameS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMaterialNameS_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 24);
            this.label2.TabIndex = 76;
            this.label2.Text = "Material Name (Short):";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(838, 24);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(715, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(102, 82);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search\r\n\r\n[F2]";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Location = new System.Drawing.Point(207, 71);
            this.txtMaterialCode.MaxLength = 20;
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(98, 32);
            this.txtMaterialCode.TabIndex = 3;
            this.txtMaterialCode.Text = "000000";
            this.txtMaterialCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaterialCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMaterialCode_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Material Code :";
            // 
            // dGProcess
            // 
            this.dGProcess.AllowUserToAddRows = false;
            this.dGProcess.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGProcess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dGProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.maker,
            this.CODE,
            this.material_fullname,
            this.maker_fullname,
            this.classification,
            this.price});
            this.dGProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGProcess.Location = new System.Drawing.Point(0, 225);
            this.dGProcess.Name = "dGProcess";
            this.dGProcess.RowTemplate.Height = 24;
            this.dGProcess.Size = new System.Drawing.Size(1033, 450);
            this.dGProcess.TabIndex = 103;
            this.dGProcess.TabStop = false;
            this.dGProcess.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_CellContentClick);
            this.dGProcess.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGProcess_CellContentClick);
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 50;
            // 
            // maker
            // 
            this.maker.HeaderText = "Maker Code";
            this.maker.Name = "maker";
            // 
            // CODE
            // 
            this.CODE.HeaderText = "Material Code";
            this.CODE.Name = "CODE";
            this.CODE.ReadOnly = true;
            this.CODE.Width = 150;
            // 
            // material_fullname
            // 
            this.material_fullname.HeaderText = "Material Name (Short)";
            this.material_fullname.Name = "material_fullname";
            this.material_fullname.ReadOnly = true;
            this.material_fullname.Width = 200;
            // 
            // maker_fullname
            // 
            this.maker_fullname.HeaderText = "Maker Name (Full)";
            this.maker_fullname.Name = "maker_fullname";
            this.maker_fullname.ReadOnly = true;
            this.maker_fullname.Width = 250;
            // 
            // classification
            // 
            this.classification.HeaderText = "classification";
            this.classification.Name = "classification";
            this.classification.Width = 125;
            // 
            // price
            // 
            this.price.HeaderText = "price";
            this.price.Name = "price";
            this.price.Width = 125;
            // 
            // FormSearchMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 675);
            this.Controls.Add(this.dGProcess);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormSearchMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Material";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSearchMaterial_FormClosed);
            this.Load += new System.EventHandler(this.FormSearchMaterial_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMakerName;
        private System.Windows.Forms.TextBox txtMakerCode;
        private System.Windows.Forms.Button btnSearchMaker;
        private System.Windows.Forms.TextBox txtMaterialNameS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtMaterialCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn material_fullname;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker_fullname;
        private System.Windows.Forms.DataGridViewTextBoxColumn classification;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
    }
}