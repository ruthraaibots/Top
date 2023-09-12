using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using YourApp.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using TopPartsElectronics_PS.Helper;

namespace TopPartsElectronics_PS
{
    public partial class FormUser : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        // 
        string iduser = "0";        
        string ActionType = string.Empty;
        public FormUser()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView_rights.AllowUserToAddRows = false;
                max_user_id();
                FetchUserDetails();
                user_right_usernames();
                user_right_formsname();
                btnAdd.Enabled = true;
                btn_save.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void max_user_id()
        {
            ActionType = "user_max";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType,string.Empty,string.Empty,string.Empty };

            ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                txtUserID.Text = dt.Rows[0]["userid"].ToString();
            }
        }
        private void FetchUserDetails()
        {
           dGUser.Refresh();
           ActionType = "GetData";          
           string[] str = { "@iduser", "@usname", "@pwd", "@fname", "@lname", "@created_at", "@updated_at", "@ActionType","@rlid","@rlnam" };
           string[] obj = { "0", "","", "","", "","", ActionType,"2","user" };

            ds = helper.GetDatasetByCommandString("users_crud", str, obj);           
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGUser.DataSource = null;
                dGUser.AutoGenerateColumns = false;
           
             
                //Set Columns Count
                dGUser.ColumnCount = 6;

                //Add Columns
                dGUser.Columns[0].Name = "sno";       
                dGUser.Columns[0].DataPropertyName = "sno";
      
                dGUser.Columns[1].Name = "User_ID";    
                dGUser.Columns[1].DataPropertyName = "idusers";

   
                dGUser.Columns[2].Name = "username";
                dGUser.Columns[2].DataPropertyName = "username";

                dGUser.Columns[3].Name = "first_name";         
                dGUser.Columns[3].DataPropertyName = "first_name";

                dGUser.Columns[4].Name = "last_name";
                dGUser.Columns[4].DataPropertyName = "last_name";


                dGUser.Columns[5].Name = "password";
                dGUser.Columns[5].DataPropertyName = "pwd";
                dGUser.DataSource = dt;             

            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGUser.DataSource = dt;
                MessageBox.Show("No Records Found");
            }
        }
        private void textBox_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }


        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnAdd.PerformClick(); 
            }
            if (e.KeyCode == Keys.F3)
            {
                btn_save.PerformClick();
            }
            if (e.KeyCode == Keys.F5)
            {
                btnDelete.PerformClick();
            }
            if (e.KeyCode == Keys.F9)
            {
                btnClose.PerformClick();
            }
            if (e.KeyCode == Keys.F7)
            {
                btnuser_down.PerformClick();
            }
        }

        private void FormUser_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).userToolStripMenuItem.Enabled = true;

        }

        private void text_enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.AliceBlue;
        }

        private void text_leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {              
               user_register(txtUserName.Text, txtPassword.Text);
                        
            }
            catch(Exception ex)
            {
               
                helper.CloseConnection();
                throw ex;
            }
        }
        public void user_register(string usname,string passwd)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to create new User ?", "CREATE USER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        string[] str_us_ext = { "@usname" };
                        string[] obj_us_ext = { txtUserName.Text };

                        MySqlDataReader sdr_us = helper.GetReaderByCmd("user_already_exist", str_us_ext, obj_us_ext);
                        if (sdr_us.Read())
                        {
                            sdr_us.Close();
                            helper.CloseConnection();
                            MessageBox.Show("User Name Already exist", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                        else
                        {
                            sdr_us.Close();
                            helper.CloseConnection();
                        byte[] salt;
                        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                        var pbkdf2 = new Rfc2898DeriveBytes(txtPassword.Text, salt, 10000);

                        byte[] hash = pbkdf2.GetBytes(20);

                        byte[] hashBytes = new byte[36];

                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hash, 0, hashBytes, 16, 20);

                        string savePasswordHash = Convert.ToBase64String(hashBytes);
                        ActionType = "SaveData";
                        string[] str = { "@iduser", "@usname", "@pwd","@fname", "@lname", "@created_at", "@updated_at", "@ActionType", "@rlid", "@rlnam" };
                        string[] obj = { "", txtUserName.Text, savePasswordHash,  txtFN.Text, txtLN.Text, nowdate.ToString(), "", ActionType, "2", "user" };
                        MySqlDataReader sdr = helper.GetReaderByCmd("users_crud", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                                new_user_rights_insert();
                            FetchUserDetails();
                            MessageBox.Show("User Created Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetInput();
                        }
                        else
                        {
                            sdr.Close();
                            helper.CloseConnection();
                        }
                    }
                }
                }
            }
            catch (Exception ex)
            {
                
                helper.CloseConnection();
                throw ex;
            }
          
        }
        private bool CheckInput()
        {
            bool result = true;
            if (txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("Username is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                result = false;
            }
            else if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Password is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                result = false;
            }
            else if (txtFN.Text.Trim() == "")
            {
                MessageBox.Show("First Name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFN.Focus();
                result = false;
            }           
            return result;
        }

        private void ResetInput()
        {
            max_user_id();
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtLN.Text = "";
            txtFN.Text = "";
        }

        private void dGUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGUser.Rows[rowIndex];
            txtUserID.Text = row.Cells[1].Value.ToString();
            txtUserName.Text = row.Cells[2].Value.ToString();
            txtFN.Text = row.Cells[3].Value.ToString();
            txtLN.Text = row.Cells[4].Value.ToString();
            txtPassword.Text = row.Cells[5].Value.ToString();
            btn_save.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Update the User ?", "UPDATE USER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        byte[] salt;
                        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                        var pbkdf2 = new Rfc2898DeriveBytes(txtPassword.Text, salt, 10000);

                        byte[] hash = pbkdf2.GetBytes(20);

                        byte[] hashBytes = new byte[36];

                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hash, 0, hashBytes, 16, 20);

                        string savePasswordHash = Convert.ToBase64String(hashBytes);
                        ActionType = "UpdateData";
                        string[] str = { "@iduser", "@usname", "@pwd", "@fname", "@lname", "@created_at", "@updated_at", "@ActionType", "@rlid", "@rlnam" };
                        string[] obj = { txtUserID.Text, txtUserName.Text, savePasswordHash, txtFN.Text, txtLN.Text, "", nowdate.ToString(), ActionType, "2", "user" };

                        MySqlDataReader sdr = helper.GetReaderByCmd("users_crud", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                            ResetInput();                           
                            ds = new DataSet();
                            FetchUserDetails();
                            btn_save.Enabled = false;
                            btnAdd.Enabled = true;
                            MessageBox.Show("User Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        }
                        else
                        {
                            sdr.Close();
                            helper.CloseConnection();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Delete the User ?", "DELETE USER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "DeleteData";
                        string[] str = { "@iduser", "@usname", "@pwd", "@fname", "@lname", "@created_at", "@updated_at", "@ActionType", "@rlid", "@rlnam" };
                        string[] obj = { txtUserID.Text, txtUserName.Text, txtPassword.Text, txtFN.Text, txtLN.Text, "", nowdate.ToString(), ActionType, "2", "user" };

                        MySqlDataReader sdr = helper.GetReaderByCmd("users_crud", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                            ResetInput();
                            ds = new DataSet();
                            FetchUserDetails();
                            btn_save.Enabled = false;
                            btnAdd.Enabled = true;
                            MessageBox.Show("User Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            sdr.Close();
                            helper.CloseConnection();
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void txtUserID_Leave(object sender, EventArgs e)
        {
            if (txtUserID.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtUserID.Text);
                txtUserID.Text = formate_type.ToString("D3");
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void btnuser_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download User List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (dGUser.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        List<string> Date_column_names = new List<string>();
                        List<int> Date_column_index = new List<int>();
                        Date_column_names.Add("Customer Code");
                        Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();

                        Excel._Workbook oWB;
                        Excel._Worksheet ws;
                        XcelApp.DisplayAlerts = false;
                        oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                        ws = (Excel._Worksheet)oWB.ActiveSheet;
                        int get_date_column = 0;
                        for (int i = 1; i < dGUser.Columns.Count; i++)
                        {
                            if (Date_column_names.Contains(dGUser.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[1, i] = dGUser.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dGUser.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[1, i] = dGUser.Columns[i - 1].HeaderText;
                                Date_column_index.Add(get_date_column);
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dGUser.Rows.Count; i++)
                        {
                            for (int j = 0; j < dGUser.Columns.Count-1; j++)
                            {
                                if (Convert.ToString(dGUser.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check customer code column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGUser.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        int formate_type = Convert.ToInt32(dGUser.Rows[i].Cells[j].Value.ToString());
                                        string lotnoD6 = formate_type.ToString("D6");
                                        Excel.Range d1 = ws.Cells[i + 2, j + 1];
                                        Excel.Range d2 = ws.Cells[i + 2, j + 1];
                                        XcelApp.Range[d1, d2].EntireColumn.NumberFormat = "@";
                                        XcelApp.Cells[i + 2, j + 1] = lotnoD6;
                                    }

                                }
                                else
                                {
                                    XcelApp.Cells[i + 2, j + 1] = string.Empty;
                                }
                            }
                        }                       
                        //Auto fit automatically adjust the width of columns of Excel  in givien range .  

                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGUser.Rows.Count, dGUser.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGUser.Columns.Count]].Font.Bold = true;

                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGUser.Columns.Count]].Font.Size = 13;

                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                       
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\User List -" + datetime;
                        string newFileName = path + compinepath;
                        // Now save this file.
                        ws.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12);

                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        MessageBox.Show("No Record To Export !!!", "Info");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_sp_upt_Click(object sender, EventArgs e)
        {
            try
            {
                if(CheckInput_chgpwd())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ActionType = "Update_chgpwd";
                    byte[] salt;
                    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                    var pbkdf2 = new Rfc2898DeriveBytes(txt_sp_cfm_pwd.Text, salt, 10000);

                    byte[] hash = pbkdf2.GetBytes(20);

                    byte[] hashBytes = new byte[36];

                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);
                    DateTime current_date_time = DateTime.Now;
                    string savePasswordHash = Convert.ToBase64String(hashBytes);
                    string[] str = { "@iduser", "@usname", "@pwd", "@fname", "@lname", "@created_at", "@updated_at", "@ActionType", "@rlid", "@rlnam" };
                    string[] obj = { txt_sp_uid.Text, txt_sp_username.Text, savePasswordHash, string.Empty, string.Empty, string.Empty, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType,"2","user" };

                    MySqlDataReader sdr = helper.GetReaderByCmd("users_crud", str, obj);
                    if (sdr.Read())
                    {
                        sdr.Close();
                        helper.CloseConnection();
                        ResetInput_chgpwd();                       
                        MessageBox.Show("User Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        sdr.Close();
                        helper.CloseConnection();
                    }                    
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckInput_chgpwd()
        {
            bool result = true;
            if (txt_sp_username.Text.Trim() == "")
            {
                MessageBox.Show("Username is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_sp_username.Focus();
                result = false;
            }
            else if (txt_sp_newpwd.Text.Trim() == "")
            {
                MessageBox.Show("New Password is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_sp_newpwd.Focus();
                result = false;
            }
            else if (txt_sp_cfm_pwd.Text.Trim() == "")
            {
                MessageBox.Show("Confirm Password is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_sp_cfm_pwd.Focus();
                result = false;
            }
            else if (txt_sp_cfm_pwd.Text.Trim() != txt_sp_newpwd.Text.Trim())
            {
                MessageBox.Show("Confirm Password Not Match..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_sp_cfm_pwd.Focus();
                result = false;
            }
            return result;
        }

        private void ResetInput_chgpwd()
        {            
            txt_sp_newpwd.Text = "";
            txt_sp_cfm_pwd.Text = "";            
        }
        public void user_right_usernames()
        {          
            ActionType = "GetDataUsers";
            string[] str = { "@iduser", "@usname", "@pwd", "@fname", "@lname", "@created_at", "@updated_at", "@ActionType", "@rlid", "@rlnam" };
            string[] obj = { "0", "", "", "", "", "", "", ActionType, "2", "user" };

            ds = helper.GetDatasetByCommandString("users_crud", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable sdr = ds.Tables[0];
                cmbUsers.Items.Clear();
                cmbUsers.DisplayMember = "username";
                cmbUsers.ValueMember = "idusers";
                cmbUsers.DataSource = sdr;
            }
        }
        public void user_right_formsname()
        {
            string Roll_id = string.Empty;
            // user tbl roll id get 
            Roll_id = Get_rollId();
            ActionType = "GetData";
            string[] str = { "@ActionType" };
            string[] obj = { ActionType };

            DataSet dset = helper.GetDatasetByCommandString("formsname_get", str, obj);
            if (dset.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = dset.Tables[0];                
                int index = 0;               
                string user_id = cmbUsers.SelectedValue.ToString();             
                foreach (DataRow drow in dtbl.Rows)
                {
                    //string formid = dtbl.Rows[index][1].ToString();
                    string user_form_find = "FormUser";
                    string get_form = drow[2].ToString();
                    if (user_form_find != get_form)
                    {
                        string formid = drow[1].ToString();
                        dataGridView_rights.Rows.Add();
                        dataGridView_rights.Rows[index].Cells[0].Value = drow[0];
                        dataGridView_rights.Rows[index].Cells[1].Value = drow[2];

                        dataGridView_rights.Rows[index].Cells[2].Value = drow[1];
                        string[] str_rights = { "@usid", "@rlid", "@fmid", "@ActionType" };
                        string[] obj_rights = { user_id, Roll_id, formid, "GetRights" };
                        MySqlDataReader sdr_rights = helper.GetReaderByCmd("formsrights_get", str_rights, obj_rights);
                        if (sdr_rights.Read())
                        {
                            string Is_active = sdr_rights["Isactive"].ToString();
                            string Id_formrights = sdr_rights["idforms_rights"].ToString();
                            if (Is_active == "1")
                            {
                                dataGridView_rights.Rows[index].Cells[3].Value = Is_active;
                                DataGridViewComboBoxCell stateCell = (DataGridViewComboBoxCell)(dataGridView_rights.Rows[index].Cells[4]);
                                stateCell.Value = "Full Rights";
                            }
                            else if (Is_active == "0")
                            {
                                dataGridView_rights.Rows[index].Cells[3].Value = Is_active;
                                DataGridViewComboBoxCell stateCell = (DataGridViewComboBoxCell)(dataGridView_rights.Rows[index].Cells[4]);
                                stateCell.Value = "No Rights";
                            }
                            dataGridView_rights.Rows[index].Cells[5].Value = Id_formrights;
                        }
                        else
                        {
                            dataGridView_rights.Rows[index].Cells[3].Value = "0";
                            DataGridViewComboBoxCell stateCell = (DataGridViewComboBoxCell)(dataGridView_rights.Rows[index].Cells[4]);
                            stateCell.Value = "No Rights";
                        }
                        index++;
                        sdr_rights.Close();
                        helper.CloseConnection();
                    }
                    
                }
            
            }
            else
            {
                DataTable dtbl = dset.Tables[0];
                dataGridView_rights.DataSource = dtbl;
                MessageBox.Show("No Records Found");
            }
        }

        private void dataGridView_rights_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView_rights_ColumnContextMenuStripChanged(object sender, DataGridViewColumnEventArgs e)
        {
            
            
        }

        private void cmbUsers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                dataGridView_rights.DataSource = null;
                dataGridView_rights.Refresh();
                dataGridView_rights.Rows.Clear();
                dataGridView_rights.AutoGenerateColumns = false;
             
                user_right_formsname();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public string Get_rollId()
        {            
            string Roll_id = string.Empty;
            // user tbl roll id get 
            string[] str_usnames = { "@usname", "@ActionType" };
            string[] obj_usnames = { cmbUsers.Text, "GetUser" };
            MySqlDataReader sdr_usnames = helper.GetReaderByCmd("get_user_name", str_usnames, obj_usnames);
            if (sdr_usnames.Read())
            {
                Roll_id = sdr_usnames["roll_id"].ToString();
            }
            sdr_usnames.Close();
            helper.CloseConnection();
            return Roll_id;
        }
        private void btnupdaterights_Click(object sender, EventArgs e)
        {
            try
            {
                bool update = false;
                if(dataGridView_rights.Rows.Count>0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string rollId = string.Empty;
                    rollId = Get_rollId();
                    DateTime current_datetime = DateTime.Now;
                    foreach (DataGridViewRow row in dataGridView_rights.Rows)
                    {
                        string userId = string.Empty;                       
                        string formId = string.Empty;
                        string Rights = string.Empty;
                        string getRights = string.Empty;
                        string pk_formRightId = string.Empty;
                        userId = cmbUsers.SelectedValue.ToString();
                        formId = row.Cells[2].Value.ToString();                    
                        getRights = row.Cells[4].Value.ToString();
                        pk_formRightId = row.Cells[5].Value.ToString();
                        if (getRights == "Full Rights")
                        {
                            Rights = "1";
                        }
                        else if(getRights == "No Rights")
                        {
                            Rights = "0";
                        }
                        ActionType = "UpdateData";
                        string[] str = { "@idfrmrights", "@isact", "@updt_at", "@ActionType", "@uid" };
                        string[] obj = { pk_formRightId, Rights, current_datetime.ToString("dd-MM-yyyy hh:mm:ss"), ActionType, CommonClass.logged_Id };
                        MySqlDataReader sdr = helper.GetReaderByCmd("formsrights_update", str, obj);
                        if (sdr.Read())
                        {                           
                            update = true;
                        }
                        sdr.Close();
                        helper.CloseConnection();
                    }
                    if(update)
                    {
                        MessageBox.Show("User Rights Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void new_user_rights_insert()
        {
            try
            {
                ActionType = "GetData";
                string[] str = { "@ActionType" };
                string[] obj = { ActionType };
                DataSet dset = helper.GetDatasetByCommandString("formsname_get", str, obj);
                if (dset.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = dset.Tables[0];
                    int index = 0;
                    string user_id = txtUserID.Text;
                    DateTime current_date_time = DateTime.Now;
                    foreach (DataRow drow in dtbl.Rows)
                    {
                        string formid = drow[1].ToString();
                        ActionType = "SaveData";
                        string[] str_ins = { "@uid","@rid","@fid", "@isact", "@creat_at", "@ActionType", "@cuserid" };
                        string[] obj_ins = {  user_id,"2",formid,"0", current_date_time.ToString("yyyy-MM-dd HH:mm:ss"),ActionType,CommonClass.logged_Id };
                        MySqlDataReader sdr = helper.GetReaderByCmd("formsrights_insert", str_ins, obj_ins);
                        if (sdr.Read())
                        {
                          
                        }
                        sdr.Close();
                        helper.CloseConnection();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
