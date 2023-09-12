namespace TopPartsElectronics_PS
{
    partial class FormPatern3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        public string OwnerName = string.Empty;
        public string ProcessName = string.Empty;
        public string SelectedHiddenLotNo= string.Empty;
        public string SelectedManfDate = string.Empty;
        public string SelectedManfDate_use_insert = string.Empty;
        public string ProcessId = string.Empty;
        public string SelectedQuantity = string.Empty;
        public string SelectedManfTime = string.Empty;
        public string itemcode = string.Empty;
        public string itemname = string.Empty;
        public string Sender_button = string.Empty;
        public string Customer_code = string.Empty;
        ///
        public string Get_process_dt_p3 = string.Empty;    
        public string Get_Qty_p3 = string.Empty;
        //30
        public string Material_code_selected = string.Empty;
        public string Search_lotNo = string.Empty;
        public string Current_button_color = string.Empty;
        public string Bproduct_p3 = string.Empty;
        public string Onhold_p3 = string.Empty;
        public string Scrap_p3 = string.Empty;
        public string reason_hs_p3 = string.Empty;
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_patern3_qty = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(133, 6);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(152, 32);
            this.dateTimePicker1.TabIndex = 131;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 24);
            this.label2.TabIndex = 130;
            this.label2.Text = "Process Date :";
            // 
            // txt_patern3_qty
            // 
            this.txt_patern3_qty.Location = new System.Drawing.Point(133, 64);
            this.txt_patern3_qty.MaxLength = 50;
            this.txt_patern3_qty.Name = "txt_patern3_qty";
            this.txt_patern3_qty.Size = new System.Drawing.Size(152, 32);
            this.txt_patern3_qty.TabIndex = 129;
            this.txt_patern3_qty.Text = "0000";
            this.txt_patern3_qty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_patern3_qty_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 24);
            this.label5.TabIndex = 128;
            this.label5.Text = "Quantity :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(571, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 127;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(443, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 83);
            this.btnSave.TabIndex = 126;
            this.btnSave.Text = "Save\r\n\r\n[F3]";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormPatern3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 130);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_patern3_qty);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPatern3";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPatern3";
            this.Load += new System.EventHandler(this.FormPatern3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_patern3_qty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
    }
}