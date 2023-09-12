namespace TopPartsElectronics_PS
{
    partial class FormPatern2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public string OwnerName = string.Empty;
        public string ProcessName = string.Empty;
        public string SelectedLotNumber = string.Empty;
        public string ProcessId = string.Empty;
        public string SelectedManfDate = string.Empty;
        public string SelectedManfDate_use_insert = string.Empty;
        public string SelectedQuantity = string.Empty;
        public string SelectedManfTime = string.Empty;
        public string itemcode = string.Empty;
        public string itemname = string.Empty;
        public string Sender_button = string.Empty;
        public string Customer_code = string.Empty;
        ///
        public string Get_process_dt = string.Empty;
        public string Get_CtrlNo = string.Empty;
        public string Get_sheet_lotno = string.Empty;
        public string Get_Qty = string.Empty;
        //30
        public string Material_code_selected = string.Empty;
        public string Search_lotNo = string.Empty;
        public string Current_button_color = string.Empty;
        public string Bproduct_p2 = string.Empty;
        public string Onhold_p2 = string.Empty;
        public string Scrap_p2 = string.Empty;
        public string reason_hs_p2 = string.Empty;
        /// 
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtLotNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCtrlNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_patern2_qty = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(601, 27);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 117;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(473, 27);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 116;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtLotNo
            // 
            this.txtLotNo.Location = new System.Drawing.Point(151, 126);
            this.txtLotNo.MaxLength = 50;
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Size = new System.Drawing.Size(256, 32);
            this.txtLotNo.TabIndex = 111;
            this.txtLotNo.Text = "0000000";
            this.txtLotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLotNo_KeyPress);
            this.txtLotNo.Leave += new System.EventHandler(this.txtLotNo_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 24);
            this.label1.TabIndex = 110;
            this.label1.Text = "Sheet Lot No :";
            // 
            // txtCtrlNo
            // 
            this.txtCtrlNo.Location = new System.Drawing.Point(149, 74);
            this.txtCtrlNo.MaxLength = 50;
            this.txtCtrlNo.Name = "txtCtrlNo";
            this.txtCtrlNo.Size = new System.Drawing.Size(256, 32);
            this.txtCtrlNo.TabIndex = 109;
            this.txtCtrlNo.Text = "000";
            this.txtCtrlNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCtrlNo_KeyPress);
            this.txtCtrlNo.Leave += new System.EventHandler(this.txtCtrlNo_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 24);
            this.label3.TabIndex = 108;
            this.label3.Text = "Control No :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 24);
            this.label5.TabIndex = 118;
            this.label5.Text = "Quantity :";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(163, 24);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(152, 32);
            this.dateTimePicker1.TabIndex = 121;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 24);
            this.label2.TabIndex = 120;
            this.label2.Text = "Process Date :";
            // 
            // txt_patern2_qty
            // 
            this.txt_patern2_qty.Location = new System.Drawing.Point(149, 180);
            this.txt_patern2_qty.MaxLength = 50;
            this.txt_patern2_qty.Name = "txt_patern2_qty";
            this.txt_patern2_qty.Size = new System.Drawing.Size(258, 32);
            this.txt_patern2_qty.TabIndex = 119;
            this.txt_patern2_qty.Text = "0000";
            this.txt_patern2_qty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_patern2_qty_KeyPress);
            // 
            // FormPatern2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 230);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_patern2_qty);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtLotNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCtrlNo);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPatern2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "a";
            this.Load += new System.EventHandler(this.FormPatern2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtLotNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCtrlNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_patern2_qty;
    }
}