namespace TopPartsElectronics_PS
{
    partial class FormMaterial
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
            this.btnmaterial_down = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.txtprice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClassification = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMakerName = new System.Windows.Forms.TextBox();
            this.txtMakerCode = new System.Windows.Forms.TextBox();
            this.btnSearchMaker = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtMaterialNameF = new System.Windows.Forms.TextBox();
            this.txtMaterialCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dGProcess = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makercode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.materialcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.material_fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idmaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker_fullname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnmaterial_down);
            this.groupBox1.Controls.Add(this.btn_refresh);
            this.groupBox1.Controls.Add(this.txtprice);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtClassification);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMakerName);
            this.groupBox1.Controls.Add(this.txtMakerCode);
            this.groupBox1.Controls.Add(this.btnSearchMaker);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtMaterialNameF);
            this.groupBox1.Controls.Add(this.txtMaterialCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(966, 296);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnmaterial_down
            // 
            this.btnmaterial_down.Location = new System.Drawing.Point(840, 203);
            this.btnmaterial_down.Name = "btnmaterial_down";
            this.btnmaterial_down.Size = new System.Drawing.Size(100, 83);
            this.btnmaterial_down.TabIndex = 113;
            this.btnmaterial_down.Text = "Download \n\r\n[F7]";
            this.btnmaterial_down.UseVisualStyleBackColor = true;
            this.btnmaterial_down.Click += new System.EventHandler(this.btnmaterial_down_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(715, 203);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(102, 82);
            this.btn_refresh.TabIndex = 105;
            this.btn_refresh.Text = "Refresh\r\n\r\n[F5]";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // txtprice
            // 
            this.txtprice.Location = new System.Drawing.Point(205, 205);
            this.txtprice.MaxLength = 6;
            this.txtprice.Name = "txtprice";
            this.txtprice.Size = new System.Drawing.Size(98, 32);
            this.txtprice.TabIndex = 81;
            this.txtprice.Text = "0";
            this.txtprice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtprice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtprice_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 24);
            this.label4.TabIndex = 80;
            this.label4.Text = "Price :";
            // 
            // txtClassification
            // 
            this.txtClassification.Location = new System.Drawing.Point(207, 165);
            this.txtClassification.MaxLength = 50;
            this.txtClassification.Name = "txtClassification";
            this.txtClassification.Size = new System.Drawing.Size(468, 32);
            this.txtClassification.TabIndex = 79;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 78;
            this.label2.Text = "Classification :";
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
            this.txtMakerCode.Enter += new System.EventHandler(this.text_enter);
            this.txtMakerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtMakerCode.Leave += new System.EventHandler(this.text_leave);
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
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(838, 24);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.btnDelete.Location = new System.Drawing.Point(715, 112);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 83);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete\r\n\r\n[F4]";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(715, 24);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add New\r\n\r\n[F2]";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtMaterialNameF
            // 
            this.txtMaterialNameF.Location = new System.Drawing.Point(207, 117);
            this.txtMaterialNameF.MaxLength = 50;
            this.txtMaterialNameF.Name = "txtMaterialNameF";
            this.txtMaterialNameF.Size = new System.Drawing.Size(468, 32);
            this.txtMaterialNameF.TabIndex = 5;
            this.txtMaterialNameF.Enter += new System.EventHandler(this.text_enter);
            this.txtMaterialNameF.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Location = new System.Drawing.Point(207, 71);
            this.txtMaterialCode.MaxLength = 6;
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(98, 32);
            this.txtMaterialCode.TabIndex = 3;
            this.txtMaterialCode.Text = "000000";
            this.txtMaterialCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaterialCode.Enter += new System.EventHandler(this.text_enter);
            this.txtMaterialCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtMaterialCode.Leave += new System.EventHandler(this.text_leave_materialcd);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Material Name :";
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
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGProcess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.makercode,
            this.materialcode,
            this.material_fullname,
            this.idmaterial,
            this.maker_fullname});
            this.dGProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGProcess.Location = new System.Drawing.Point(0, 296);
            this.dGProcess.Name = "dGProcess";
            this.dGProcess.RowTemplate.Height = 24;
            this.dGProcess.Size = new System.Drawing.Size(966, 379);
            this.dGProcess.TabIndex = 102;
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
            // makercode
            // 
            this.makercode.HeaderText = "Maker Code";
            this.makercode.Name = "makercode";
            // 
            // materialcode
            // 
            this.materialcode.HeaderText = "Material Code";
            this.materialcode.Name = "materialcode";
            this.materialcode.ReadOnly = true;
            this.materialcode.Width = 150;
            // 
            // material_fullname
            // 
            this.material_fullname.HeaderText = "Material Name (Full)";
            this.material_fullname.Name = "material_fullname";
            this.material_fullname.ReadOnly = true;
            this.material_fullname.Width = 450;
            // 
            // idmaterial
            // 
            this.idmaterial.HeaderText = "idmaterial";
            this.idmaterial.Name = "idmaterial";
            this.idmaterial.Visible = false;
            // 
            // maker_fullname
            // 
            this.maker_fullname.HeaderText = "Maker Name";
            this.maker_fullname.Name = "maker_fullname";
            this.maker_fullname.Visible = false;
            // 
            // FormMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 675);
            this.Controls.Add(this.dGProcess);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMaterial";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Material";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMaterial_Closing);
            this.Load += new System.EventHandler(this.FormMaterial_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMaterial_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtMaterialNameF;
        private System.Windows.Forms.TextBox txtMaterialCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGProcess;
        private System.Windows.Forms.Button btnSearchMaker;
        private System.Windows.Forms.TextBox txtMakerCode;
        private System.Windows.Forms.TextBox txtMakerName;
        private System.Windows.Forms.TextBox txtClassification;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtprice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btnmaterial_down;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn makercode;
        private System.Windows.Forms.DataGridViewTextBoxColumn materialcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn material_fullname;
        private System.Windows.Forms.DataGridViewTextBoxColumn idmaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker_fullname;
    }
}