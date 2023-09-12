namespace TopPartsElectronics_PS
{
    partial class FormLotInformationStatus
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
            this.gBoxLotinfo = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.gBFilters = new System.Windows.Forms.GroupBox();
            this.btn_shipping_dwn = new System.Windows.Forms.Button();
            this.btn_nextPg = new System.Windows.Forms.Button();
            this.chk_selectall = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.txt_selected_lotno = new System.Windows.Forms.TextBox();
            this.checkedListBox_lotno = new System.Windows.Forms.CheckedListBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSearchall = new System.Windows.Forms.Label();
            this.chk_item = new System.Windows.Forms.CheckBox();
            this.chk_customer = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.date_manf_to = new System.Windows.Forms.DateTimePicker();
            this.date_manf_frm = new System.Windows.Forms.DateTimePicker();
            this.chk_manf_dt_frm_to = new System.Windows.Forms.CheckBox();
            this.txt_itemname = new System.Windows.Forms.TextBox();
            this.btnSearchItem = new System.Windows.Forms.Button();
            this.textItemCode = new System.Windows.Forms.TextBox();
            this.btnSearchCustomer = new System.Windows.Forms.Button();
            this.txtCustomerCode = new System.Windows.Forms.TextBox();
            this.txtCustomerNameF = new System.Windows.Forms.TextBox();
            this.gBoxLotinfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gBFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBoxLotinfo
            // 
            this.gBoxLotinfo.Controls.Add(this.dataGridView1);
            this.gBoxLotinfo.Controls.Add(this.gBFilters);
            this.gBoxLotinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBoxLotinfo.Location = new System.Drawing.Point(0, 0);
            this.gBoxLotinfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gBoxLotinfo.Name = "gBoxLotinfo";
            this.gBoxLotinfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gBoxLotinfo.Size = new System.Drawing.Size(1465, 1030);
            this.gBoxLotinfo.TabIndex = 0;
            this.gBoxLotinfo.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 266);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1459, 760);
            this.dataGridView1.TabIndex = 107;
            this.dataGridView1.TabStop = false;
            // 
            // gBFilters
            // 
            this.gBFilters.Controls.Add(this.btn_shipping_dwn);
            this.gBFilters.Controls.Add(this.btn_nextPg);
            this.gBFilters.Controls.Add(this.chk_selectall);
            this.gBFilters.Controls.Add(this.btnClose);
            this.gBFilters.Controls.Add(this.txt_selected_lotno);
            this.gBFilters.Controls.Add(this.checkedListBox_lotno);
            this.gBFilters.Controls.Add(this.btnSearch);
            this.gBFilters.Controls.Add(this.lblSearchall);
            this.gBFilters.Controls.Add(this.chk_item);
            this.gBFilters.Controls.Add(this.chk_customer);
            this.gBFilters.Controls.Add(this.label1);
            this.gBFilters.Controls.Add(this.date_manf_to);
            this.gBFilters.Controls.Add(this.date_manf_frm);
            this.gBFilters.Controls.Add(this.chk_manf_dt_frm_to);
            this.gBFilters.Controls.Add(this.txt_itemname);
            this.gBFilters.Controls.Add(this.btnSearchItem);
            this.gBFilters.Controls.Add(this.textItemCode);
            this.gBFilters.Controls.Add(this.btnSearchCustomer);
            this.gBFilters.Controls.Add(this.txtCustomerCode);
            this.gBFilters.Controls.Add(this.txtCustomerNameF);
            this.gBFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.gBFilters.Location = new System.Drawing.Point(3, 29);
            this.gBFilters.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gBFilters.Name = "gBFilters";
            this.gBFilters.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gBFilters.Size = new System.Drawing.Size(1459, 237);
            this.gBFilters.TabIndex = 0;
            this.gBFilters.TabStop = false;
            // 
            // btn_shipping_dwn
            // 
            this.btn_shipping_dwn.Location = new System.Drawing.Point(1229, 130);
            this.btn_shipping_dwn.Name = "btn_shipping_dwn";
            this.btn_shipping_dwn.Size = new System.Drawing.Size(100, 83);
            this.btn_shipping_dwn.TabIndex = 153;
            this.btn_shipping_dwn.Text = "Download\r\n\r\n[F8]";
            this.btn_shipping_dwn.UseVisualStyleBackColor = true;
            this.btn_shipping_dwn.Click += new System.EventHandler(this.btn_shipping_dwn_Click);
            // 
            // btn_nextPg
            // 
            this.btn_nextPg.Enabled = false;
            this.btn_nextPg.Location = new System.Drawing.Point(1102, 130);
            this.btn_nextPg.Name = "btn_nextPg";
            this.btn_nextPg.Size = new System.Drawing.Size(100, 83);
            this.btn_nextPg.TabIndex = 152;
            this.btn_nextPg.Text = "Load Next Set of Data  >>";
            this.btn_nextPg.UseVisualStyleBackColor = true;
            this.btn_nextPg.Click += new System.EventHandler(this.btn_nextPg_Click);
            // 
            // chk_selectall
            // 
            this.chk_selectall.AutoSize = true;
            this.chk_selectall.Location = new System.Drawing.Point(750, 63);
            this.chk_selectall.Name = "chk_selectall";
            this.chk_selectall.Size = new System.Drawing.Size(98, 28);
            this.chk_selectall.TabIndex = 145;
            this.chk_selectall.Text = "Select All";
            this.chk_selectall.UseVisualStyleBackColor = true;
            this.chk_selectall.CheckedChanged += new System.EventHandler(this.chk_selectall_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1229, 23);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 144;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txt_selected_lotno
            // 
            this.txt_selected_lotno.Location = new System.Drawing.Point(864, 24);
            this.txt_selected_lotno.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_selected_lotno.MaxLength = 50;
            this.txt_selected_lotno.Name = "txt_selected_lotno";
            this.txt_selected_lotno.Size = new System.Drawing.Size(177, 32);
            this.txt_selected_lotno.TabIndex = 143;
            this.txt_selected_lotno.Text = "000000";
            this.txt_selected_lotno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_selected_lotno.Leave += new System.EventHandler(this.txt_selected_lotno_Leave);
            // 
            // checkedListBox_lotno
            // 
            this.checkedListBox_lotno.FormattingEnabled = true;
            this.checkedListBox_lotno.HorizontalScrollbar = true;
            this.checkedListBox_lotno.Location = new System.Drawing.Point(864, 53);
            this.checkedListBox_lotno.Name = "checkedListBox_lotno";
            this.checkedListBox_lotno.Size = new System.Drawing.Size(177, 166);
            this.checkedListBox_lotno.TabIndex = 142;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1100, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(102, 82);
            this.btnSearch.TabIndex = 141;
            this.btnSearch.Text = "Search \n\r\n[F2]";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSearchall
            // 
            this.lblSearchall.AutoSize = true;
            this.lblSearchall.Location = new System.Drawing.Point(746, 26);
            this.lblSearchall.Name = "lblSearchall";
            this.lblSearchall.Size = new System.Drawing.Size(93, 24);
            this.lblSearchall.TabIndex = 140;
            this.lblSearchall.Text = "Lot Number :";
            // 
            // chk_item
            // 
            this.chk_item.AutoSize = true;
            this.chk_item.Location = new System.Drawing.Point(6, 105);
            this.chk_item.Name = "chk_item";
            this.chk_item.Size = new System.Drawing.Size(18, 17);
            this.chk_item.TabIndex = 139;
            this.chk_item.UseVisualStyleBackColor = true;
            this.chk_item.CheckedChanged += new System.EventHandler(this.chk_item_CheckedChanged);
            // 
            // chk_customer
            // 
            this.chk_customer.AutoSize = true;
            this.chk_customer.Location = new System.Drawing.Point(6, 31);
            this.chk_customer.Name = "chk_customer";
            this.chk_customer.Size = new System.Drawing.Size(18, 17);
            this.chk_customer.TabIndex = 138;
            this.chk_customer.UseVisualStyleBackColor = true;
            this.chk_customer.CheckedChanged += new System.EventHandler(this.chk_customer_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(348, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 24);
            this.label1.TabIndex = 137;
            this.label1.Text = "To";
            // 
            // date_manf_to
            // 
            this.date_manf_to.CustomFormat = "dd-MM-yyyy";
            this.date_manf_to.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_manf_to.Location = new System.Drawing.Point(379, 156);
            this.date_manf_to.Name = "date_manf_to";
            this.date_manf_to.Size = new System.Drawing.Size(152, 32);
            this.date_manf_to.TabIndex = 136;
            // 
            // date_manf_frm
            // 
            this.date_manf_frm.CustomFormat = "dd-MM-yyyy";
            this.date_manf_frm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_manf_frm.Location = new System.Drawing.Point(178, 156);
            this.date_manf_frm.Name = "date_manf_frm";
            this.date_manf_frm.Size = new System.Drawing.Size(166, 32);
            this.date_manf_frm.TabIndex = 135;
            // 
            // chk_manf_dt_frm_to
            // 
            this.chk_manf_dt_frm_to.AutoSize = true;
            this.chk_manf_dt_frm_to.Location = new System.Drawing.Point(9, 158);
            this.chk_manf_dt_frm_to.Name = "chk_manf_dt_frm_to";
            this.chk_manf_dt_frm_to.Size = new System.Drawing.Size(161, 28);
            this.chk_manf_dt_frm_to.TabIndex = 134;
            this.chk_manf_dt_frm_to.Text = "Manufacturing Date";
            this.chk_manf_dt_frm_to.UseVisualStyleBackColor = true;
            this.chk_manf_dt_frm_to.CheckedChanged += new System.EventHandler(this.chk_manf_dt_frm_to_CheckedChanged);
            // 
            // txt_itemname
            // 
            this.txt_itemname.Location = new System.Drawing.Point(344, 97);
            this.txt_itemname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_itemname.MaxLength = 50;
            this.txt_itemname.Name = "txt_itemname";
            this.txt_itemname.ReadOnly = true;
            this.txt_itemname.Size = new System.Drawing.Size(349, 32);
            this.txt_itemname.TabIndex = 133;
            this.txt_itemname.TabStop = false;
            // 
            // btnSearchItem
            // 
            this.btnSearchItem.Location = new System.Drawing.Point(33, 91);
            this.btnSearchItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearchItem.Name = "btnSearchItem";
            this.btnSearchItem.Size = new System.Drawing.Size(127, 42);
            this.btnSearchItem.TabIndex = 131;
            this.btnSearchItem.Text = "Item Code :";
            this.btnSearchItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchItem.UseVisualStyleBackColor = true;
            this.btnSearchItem.Click += new System.EventHandler(this.btnSearchItem_Click);
            // 
            // textItemCode
            // 
            this.textItemCode.Location = new System.Drawing.Point(177, 97);
            this.textItemCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textItemCode.MaxLength = 50;
            this.textItemCode.Name = "textItemCode";
            this.textItemCode.Size = new System.Drawing.Size(166, 32);
            this.textItemCode.TabIndex = 130;
            this.textItemCode.Text = "000000";
            this.textItemCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearchCustomer
            // 
            this.btnSearchCustomer.Location = new System.Drawing.Point(33, 17);
            this.btnSearchCustomer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearchCustomer.Name = "btnSearchCustomer";
            this.btnSearchCustomer.Size = new System.Drawing.Size(127, 42);
            this.btnSearchCustomer.TabIndex = 128;
            this.btnSearchCustomer.Text = "Customer Code :";
            this.btnSearchCustomer.UseVisualStyleBackColor = true;
            this.btnSearchCustomer.Click += new System.EventHandler(this.btnSearchCustomer_Click);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Location = new System.Drawing.Point(177, 23);
            this.txtCustomerCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCustomerCode.MaxLength = 6;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Size = new System.Drawing.Size(166, 32);
            this.txtCustomerCode.TabIndex = 129;
            this.txtCustomerCode.Text = "000000";
            this.txtCustomerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCustomerNameF
            // 
            this.txtCustomerNameF.Location = new System.Drawing.Point(344, 23);
            this.txtCustomerNameF.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCustomerNameF.MaxLength = 50;
            this.txtCustomerNameF.Name = "txtCustomerNameF";
            this.txtCustomerNameF.ReadOnly = true;
            this.txtCustomerNameF.Size = new System.Drawing.Size(349, 32);
            this.txtCustomerNameF.TabIndex = 126;
            this.txtCustomerNameF.TabStop = false;
            // 
            // FormLotInformationStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1465, 1030);
            this.Controls.Add(this.gBoxLotinfo);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormLotInformationStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLotInformationStatus";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLotInformationStatus_FormClosing);
            this.Load += new System.EventHandler(this.FormLotInformationStatus_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLotInformationStatus_KeyDown);
            this.gBoxLotinfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gBFilters.ResumeLayout(false);
            this.gBFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gBoxLotinfo;
        private System.Windows.Forms.GroupBox gBFilters;
        private System.Windows.Forms.TextBox txt_itemname;
        private System.Windows.Forms.Button btnSearchItem;
        private System.Windows.Forms.TextBox textItemCode;
        private System.Windows.Forms.Button btnSearchCustomer;
        private System.Windows.Forms.TextBox txtCustomerCode;
        private System.Windows.Forms.TextBox txtCustomerNameF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker date_manf_to;
        private System.Windows.Forms.DateTimePicker date_manf_frm;
        private System.Windows.Forms.CheckBox chk_manf_dt_frm_to;
        private System.Windows.Forms.CheckBox chk_item;
        private System.Windows.Forms.CheckBox chk_customer;
        private System.Windows.Forms.Label lblSearchall;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckedListBox checkedListBox_lotno;
        private System.Windows.Forms.TextBox txt_selected_lotno;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chk_selectall;
        private System.Windows.Forms.Button btn_nextPg;
        private System.Windows.Forms.Button btn_shipping_dwn;
    }
}