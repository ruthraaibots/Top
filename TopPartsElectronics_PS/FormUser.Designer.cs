namespace TopPartsElectronics_PS
{
    partial class FormUser
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnuser_down = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtLN = new System.Windows.Forms.TextBox();
            this.txtFN = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dGUser = new System.Windows.Forms.DataGridView();
            this.no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.first = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_sp_cfm_pwd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_sp_newpwd = new System.Windows.Forms.TextBox();
            this.txt_sp_uid = new System.Windows.Forms.TextBox();
            this.txt_sp_username = new System.Windows.Forms.TextBox();
            this.btn_sp_upt = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView_rights = new System.Windows.Forms.DataGridView();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnupdaterights = new System.Windows.Forms.Button();
            this.sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formcaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isactive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rights = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.formrightsid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGUser)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rights)).BeginInit();
            this.SuspendLayout();
            // 
            // btnuser_down
            // 
            this.btnuser_down.Location = new System.Drawing.Point(706, 100);
            this.btnuser_down.Name = "btnuser_down";
            this.btnuser_down.Size = new System.Drawing.Size(100, 83);
            this.btnuser_down.TabIndex = 113;
            this.btnuser_down.Text = "Download \n\r\n[F7]";
            this.btnuser_down.UseVisualStyleBackColor = true;
            this.btnuser_down.Click += new System.EventHandler(this.btnuser_down_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(487, 100);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(102, 82);
            this.btn_refresh.TabIndex = 80;
            this.btn_refresh.Text = "Refresh\r\n\r\n[F5]";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(597, 12);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(102, 82);
            this.btn_save.TabIndex = 15;
            this.btn_save.Text = "Save\n\r\n[F3]";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "Password * :";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(95, 54);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(382, 32);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Enter += new System.EventHandler(this.text_enter);
            this.txtPassword.Leave += new System.EventHandler(this.text_leave);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(597, 100);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 82);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close\r\n\r\n[F9]";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(706, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 83);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete\r\n\r\n[F4]";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(487, 11);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 82);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add New\r\n\r\n[F2]";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtLN
            // 
            this.txtLN.Location = new System.Drawing.Point(94, 136);
            this.txtLN.MaxLength = 50;
            this.txtLN.Name = "txtLN";
            this.txtLN.Size = new System.Drawing.Size(383, 32);
            this.txtLN.TabIndex = 5;
            this.txtLN.Enter += new System.EventHandler(this.text_enter);
            this.txtLN.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtFN
            // 
            this.txtFN.Location = new System.Drawing.Point(94, 95);
            this.txtFN.MaxLength = 50;
            this.txtFN.Name = "txtFN";
            this.txtFN.Size = new System.Drawing.Size(383, 32);
            this.txtFN.TabIndex = 4;
            this.txtFN.Enter += new System.EventHandler(this.text_enter);
            this.txtFN.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(160, 11);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(317, 32);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.Enter += new System.EventHandler(this.text_enter);
            this.txtUserName.Leave += new System.EventHandler(this.text_leave);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(94, 11);
            this.txtUserID.MaxLength = 4;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(66, 32);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.Text = "0000";
            this.txtUserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUserID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressDecimal);
            this.txtUserID.Leave += new System.EventHandler(this.txtUserID_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-1, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "Last Name :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "First Name * :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "User ID * :";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(847, 658);
            this.tabControl1.TabIndex = 114;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnuser_down);
            this.tabPage1.Controls.Add(this.dGUser);
            this.tabPage1.Controls.Add(this.btn_refresh);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btn_save);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.txtUserID);
            this.tabPage1.Controls.Add(this.btnClose);
            this.tabPage1.Controls.Add(this.txtUserName);
            this.tabPage1.Controls.Add(this.btnDelete);
            this.tabPage1.Controls.Add(this.txtFN);
            this.tabPage1.Controls.Add(this.btnAdd);
            this.tabPage1.Controls.Add(this.txtLN);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(839, 621);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Login User Creation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dGUser
            // 
            this.dGUser.AllowUserToAddRows = false;
            this.dGUser.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dGUser.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dGUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no,
            this.USER_ID,
            this.name,
            this.first,
            this.last,
            this.pwd});
            this.dGUser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dGUser.Location = new System.Drawing.Point(3, 189);
            this.dGUser.Name = "dGUser";
            this.dGUser.RowTemplate.Height = 24;
            this.dGUser.Size = new System.Drawing.Size(833, 429);
            this.dGUser.TabIndex = 4;
            this.dGUser.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGUser_CellClick);
            // 
            // no
            // 
            this.no.HeaderText = "No";
            this.no.Name = "no";
            this.no.ReadOnly = true;
            this.no.Width = 50;
            // 
            // USER_ID
            // 
            this.USER_ID.HeaderText = "USER_ID";
            this.USER_ID.Name = "USER_ID";
            this.USER_ID.ReadOnly = true;
            this.USER_ID.Visible = false;
            // 
            // name
            // 
            this.name.HeaderText = "USER Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 250;
            // 
            // first
            // 
            this.first.HeaderText = "FIRST Name";
            this.first.Name = "first";
            this.first.ReadOnly = true;
            this.first.Width = 250;
            // 
            // last
            // 
            this.last.HeaderText = "LAST Name";
            this.last.Name = "last";
            this.last.ReadOnly = true;
            this.last.Width = 150;
            // 
            // pwd
            // 
            this.pwd.HeaderText = "password";
            this.pwd.Name = "pwd";
            this.pwd.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txt_sp_cfm_pwd);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txt_sp_newpwd);
            this.tabPage2.Controls.Add(this.txt_sp_uid);
            this.tabPage2.Controls.Add(this.txt_sp_username);
            this.tabPage2.Controls.Add(this.btn_sp_upt);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(839, 621);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Print Change Password";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(77, 269);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 24);
            this.label8.TabIndex = 23;
            this.label8.Text = "Confirm Password * :";
            // 
            // txt_sp_cfm_pwd
            // 
            this.txt_sp_cfm_pwd.Location = new System.Drawing.Point(229, 269);
            this.txt_sp_cfm_pwd.MaxLength = 20;
            this.txt_sp_cfm_pwd.Name = "txt_sp_cfm_pwd";
            this.txt_sp_cfm_pwd.PasswordChar = '*';
            this.txt_sp_cfm_pwd.Size = new System.Drawing.Size(382, 32);
            this.txt_sp_cfm_pwd.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Bahnschrift Condensed", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(283, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(210, 40);
            this.label7.TabIndex = 21;
            this.label7.Text = "Change Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(151, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "User ID * :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 24);
            this.label6.TabIndex = 20;
            this.label6.Text = "New Password * :";
            // 
            // txt_sp_newpwd
            // 
            this.txt_sp_newpwd.Location = new System.Drawing.Point(229, 195);
            this.txt_sp_newpwd.MaxLength = 20;
            this.txt_sp_newpwd.Name = "txt_sp_newpwd";
            this.txt_sp_newpwd.PasswordChar = '*';
            this.txt_sp_newpwd.Size = new System.Drawing.Size(382, 32);
            this.txt_sp_newpwd.TabIndex = 18;
            // 
            // txt_sp_uid
            // 
            this.txt_sp_uid.Location = new System.Drawing.Point(224, 115);
            this.txt_sp_uid.MaxLength = 4;
            this.txt_sp_uid.Name = "txt_sp_uid";
            this.txt_sp_uid.ReadOnly = true;
            this.txt_sp_uid.Size = new System.Drawing.Size(66, 32);
            this.txt_sp_uid.TabIndex = 16;
            this.txt_sp_uid.Text = "0001";
            this.txt_sp_uid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_sp_username
            // 
            this.txt_sp_username.Enabled = false;
            this.txt_sp_username.Location = new System.Drawing.Point(290, 115);
            this.txt_sp_username.MaxLength = 50;
            this.txt_sp_username.Name = "txt_sp_username";
            this.txt_sp_username.Size = new System.Drawing.Size(321, 32);
            this.txt_sp_username.TabIndex = 17;
            this.txt_sp_username.Text = "super admin";
            // 
            // btn_sp_upt
            // 
            this.btn_sp_upt.Location = new System.Drawing.Point(509, 320);
            this.btn_sp_upt.Name = "btn_sp_upt";
            this.btn_sp_upt.Size = new System.Drawing.Size(102, 82);
            this.btn_sp_upt.TabIndex = 19;
            this.btn_sp_upt.Text = "Upated \r\n[F5]";
            this.btn_sp_upt.UseVisualStyleBackColor = true;
            this.btn_sp_upt.Click += new System.EventHandler(this.btn_sp_upt_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnupdaterights);
            this.tabPage3.Controls.Add(this.dataGridView_rights);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.cmbUsers);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(839, 621);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "User Rights";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView_rights
            // 
            this.dataGridView_rights.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_rights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_rights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sno,
            this.formcaption,
            this.fid,
            this.isactive,
            this.rights,
            this.formrightsid});
            this.dataGridView_rights.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView_rights.Location = new System.Drawing.Point(0, 156);
            this.dataGridView_rights.Name = "dataGridView_rights";
            this.dataGridView_rights.RowTemplate.Height = 24;
            this.dataGridView_rights.Size = new System.Drawing.Size(839, 465);
            this.dataGridView_rights.TabIndex = 23;
            this.dataGridView_rights.ColumnContextMenuStripChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_rights_ColumnContextMenuStripChanged);
            this.dataGridView_rights.SelectionChanged += new System.EventHandler(this.dataGridView_rights_SelectionChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Bahnschrift Condensed", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(275, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 40);
            this.label10.TabIndex = 22;
            this.label10.Text = "User Rights";
            // 
            // cmbUsers
            // 
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(162, 97);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(229, 32);
            this.cmbUsers.TabIndex = 11;
            this.cmbUsers.SelectionChangeCommitted += new System.EventHandler(this.cmbUsers_SelectionChangeCommitted);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 24);
            this.label9.TabIndex = 10;
            this.label9.Text = "List of users :";
            // 
            // btnupdaterights
            // 
            this.btnupdaterights.Location = new System.Drawing.Point(576, 62);
            this.btnupdaterights.Name = "btnupdaterights";
            this.btnupdaterights.Size = new System.Drawing.Size(102, 67);
            this.btnupdaterights.TabIndex = 24;
            this.btnupdaterights.Text = "Upated \r\n[F5]";
            this.btnupdaterights.UseVisualStyleBackColor = true;
            this.btnupdaterights.Click += new System.EventHandler(this.btnupdaterights_Click);
            // 
            // sno
            // 
            this.sno.HeaderText = "No";
            this.sno.Name = "sno";
            // 
            // formcaption
            // 
            this.formcaption.HeaderText = "Form Caption";
            this.formcaption.Name = "formcaption";
            // 
            // fid
            // 
            this.fid.HeaderText = "Form Id";
            this.fid.Name = "fid";
            this.fid.Visible = false;
            // 
            // isactive
            // 
            this.isactive.HeaderText = "IsActive";
            this.isactive.Name = "isactive";
            this.isactive.Visible = false;
            // 
            // rights
            // 
            this.rights.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.rights.HeaderText = "Rights";
            this.rights.Items.AddRange(new object[] {
            "No Rights",
            "Full Rights"});
            this.rights.Name = "rights";
            // 
            // formrightsid
            // 
            this.formrightsid.HeaderText = "Frm Rights Id";
            this.formrightsid.Name = "formrightsid";
            this.formrightsid.Visible = false;
            // 
            // FormUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 658);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormUser";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage User";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUser_Closing);
            this.Load += new System.EventHandler(this.FormUser_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGUser)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rights)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtLN;
        private System.Windows.Forms.TextBox txtFN;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btnuser_down;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_sp_newpwd;
        private System.Windows.Forms.TextBox txt_sp_uid;
        private System.Windows.Forms.TextBox txt_sp_username;
        private System.Windows.Forms.Button btn_sp_upt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_sp_cfm_pwd;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.DataGridView dataGridView_rights;
        private System.Windows.Forms.DataGridView dGUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn no;
        private System.Windows.Forms.DataGridViewTextBoxColumn USER_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn first;
        private System.Windows.Forms.DataGridViewTextBoxColumn last;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwd;
        private System.Windows.Forms.Button btnupdaterights;
        private System.Windows.Forms.DataGridViewTextBoxColumn sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn formcaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn fid;
        private System.Windows.Forms.DataGridViewTextBoxColumn isactive;
        private System.Windows.Forms.DataGridViewComboBoxColumn rights;
        private System.Windows.Forms.DataGridViewTextBoxColumn formrightsid;
    }
}