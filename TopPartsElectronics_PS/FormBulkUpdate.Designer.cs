namespace TopPartsElectronics_PS
{
    partial class FormBulkUpdate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public string customerCode = string.Empty;
        public string processId = string.Empty;
        public string itemCode = string.Empty;
        public string itemName = string.Empty;
        public string lotQty = string.Empty;
        public string manufacturingTime = string.Empty;
        public int grid_selected_row = 0;
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
            this.textLotNoChild_frm = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.textLotNoAdd = new System.Windows.Forms.TextBox();
            this.txtLotnoChild_to = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_customercode_bulk = new System.Windows.Forms.Label();
            this.lblItemcd_bulk = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblitemname_bulk = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textLotNoChild_frm
            // 
            this.textLotNoChild_frm.Location = new System.Drawing.Point(426, 114);
            this.textLotNoChild_frm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textLotNoChild_frm.MaxLength = 3;
            this.textLotNoChild_frm.Name = "textLotNoChild_frm";
            this.textLotNoChild_frm.Size = new System.Drawing.Size(49, 36);
            this.textLotNoChild_frm.TabIndex = 105;
            this.textLotNoChild_frm.Text = "01";
            this.textLotNoChild_frm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textLotNoChild_frm.Leave += new System.EventHandler(this.textLotNoChild_frm_Leave);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(345, 117);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(60, 25);
            this.label25.TabIndex = 104;
            this.label25.Text = "From -";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(27, 114);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(63, 25);
            this.label24.TabIndex = 103;
            this.label24.Text = "Lot No :";
            // 
            // textLotNoAdd
            // 
            this.textLotNoAdd.Location = new System.Drawing.Point(151, 111);
            this.textLotNoAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textLotNoAdd.MaxLength = 7;
            this.textLotNoAdd.Name = "textLotNoAdd";
            this.textLotNoAdd.Size = new System.Drawing.Size(98, 36);
            this.textLotNoAdd.TabIndex = 102;
            this.textLotNoAdd.Text = "0000000";
            this.textLotNoAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textLotNoAdd.Leave += new System.EventHandler(this.textLotNoAdd_Leave);
            // 
            // txtLotnoChild_to
            // 
            this.txtLotnoChild_to.Location = new System.Drawing.Point(525, 114);
            this.txtLotnoChild_to.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLotnoChild_to.MaxLength = 3;
            this.txtLotnoChild_to.Name = "txtLotnoChild_to";
            this.txtLotnoChild_to.Size = new System.Drawing.Size(49, 36);
            this.txtLotnoChild_to.TabIndex = 107;
            this.txtLotnoChild_to.Text = "99";
            this.txtLotnoChild_to.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLotnoChild_to.Leave += new System.EventHandler(this.txtLotnoChild_to_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(484, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 27);
            this.label1.TabIndex = 106;
            this.label1.Text = "To -";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(649, 104);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(112, 47);
            this.btnSearch.TabIndex = 108;
            this.btnSearch.Text = "Search [F5]";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(28, 159);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(920, 99);
            this.panel1.TabIndex = 119;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 25);
            this.label2.TabIndex = 120;
            this.label2.Text = "Customer Code :";
            // 
            // lbl_customercode_bulk
            // 
            this.lbl_customercode_bulk.AutoSize = true;
            this.lbl_customercode_bulk.ForeColor = System.Drawing.Color.Indigo;
            this.lbl_customercode_bulk.Location = new System.Drawing.Point(147, 69);
            this.lbl_customercode_bulk.Name = "lbl_customercode_bulk";
            this.lbl_customercode_bulk.Size = new System.Drawing.Size(22, 29);
            this.lbl_customercode_bulk.TabIndex = 121;
            this.lbl_customercode_bulk.Text = "-";
            // 
            // lblItemcd_bulk
            // 
            this.lblItemcd_bulk.AutoSize = true;
            this.lblItemcd_bulk.ForeColor = System.Drawing.Color.Indigo;
            this.lblItemcd_bulk.Location = new System.Drawing.Point(422, 69);
            this.lblItemcd_bulk.Name = "lblItemcd_bulk";
            this.lblItemcd_bulk.Size = new System.Drawing.Size(22, 29);
            this.lblItemcd_bulk.TabIndex = 123;
            this.lblItemcd_bulk.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(345, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 25);
            this.label4.TabIndex = 122;
            this.label4.Text = "Item Code :";
            // 
            // lblitemname_bulk
            // 
            this.lblitemname_bulk.AutoSize = true;
            this.lblitemname_bulk.ForeColor = System.Drawing.Color.Indigo;
            this.lblitemname_bulk.Location = new System.Drawing.Point(728, 69);
            this.lblitemname_bulk.Name = "lblitemname_bulk";
            this.lblitemname_bulk.Size = new System.Drawing.Size(22, 29);
            this.lblitemname_bulk.TabIndex = 125;
            this.lblitemname_bulk.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(645, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 25);
            this.label5.TabIndex = 124;
            this.label5.Text = "Item Name :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift Condensed", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(312, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(338, 48);
            this.label3.TabIndex = 140;
            this.label3.Text = "Bulk Lot Number Update";
            // 
            // FormBulkUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 258);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblitemname_bulk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblItemcd_bulk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_customercode_bulk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtLotnoChild_to);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textLotNoChild_frm);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.textLotNoAdd);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBulkUpdate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bulk Update";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBulkUpdate_FormClosed);
            this.Load += new System.EventHandler(this.FormBulkUpdate_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormBulkUpdate_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textLotNoChild_frm;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textLotNoAdd;
        private System.Windows.Forms.TextBox txtLotnoChild_to;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_customercode_bulk;
        private System.Windows.Forms.Label lblItemcd_bulk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblitemname_bulk;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}