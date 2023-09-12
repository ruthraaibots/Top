using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YourApp.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace TopPartsElectronics_PS
{
    public partial class FormProcess : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        string Selected_Process_tbl_id = string.Empty;
        string edit_allow = string.Empty;
        public FormProcess()
        {
            InitializeComponent();
        }
        private void FormProcess_Load(object sender, EventArgs e)
        {
            FetchProcessDetails();
            InputscreenType_drp();
            Max_id();
        }
        public void InputscreenType_drp()
        {
            ActionType = "GetData";
            string[] str = { "@ActionType" };
            string[] obj = { ActionType };
            DataTable sdr = helper.GetDatasetByCommandString_dt("inputscreentyp_get", str, obj);
            comboLType.Items.Clear();
            comboLType.DisplayMember = "inputtype";
            comboLType.ValueMember = "idinputscreen";
            comboLType.DataSource = sdr;
            helper.CloseConnection();
        }
        private void textBox_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void FormProcess_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).processToolStripMenuItem.Enabled = true;
        }
        private void FormProcess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnAdd.PerformClick();
            }
            if (e.KeyCode == Keys.F3)
            {
                btnSave.PerformClick();
            }
            if (e.KeyCode == Keys.F4)
            {
                btnDelete.PerformClick();
            }
            if (e.KeyCode == Keys.F9)
            {
                btnClose.PerformClick();
            }
            if (e.KeyCode == Keys.F7)
            {
                btnprocess_down.PerformClick();
            }
        }
        private void text_enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.AliceBlue;
        }
        private void text_leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
            if (txtProcessCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtProcessCode.Text);
                txtProcessCode.Text = formate_type.ToString("D3");
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public bool CheckInput()
        {
            bool result = true;
            if (txtProcessCode.Text.Trim() == "" || txtProcessCode.Text == "000000")
            {
                MessageBox.Show("Process Code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProcessCode.Focus();
                result = false;
            }
            else if (txtProcessNameS.Text.Trim() == "")
            {
                MessageBox.Show("Process Short name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProcessNameS.Focus();
                result = false;
            }
            else if (txtProcessNameF.Text.Trim() == "")
            {
                MessageBox.Show("Process Full name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProcessNameF.Focus();
                result = false;
            }
            else if (textShowOrder.Text.Trim() == "")
            {
                MessageBox.Show("Show Order is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textShowOrder.Focus();
                result = false;
            }
            return result;
        }
        public void ResetInput()
        {
            txtProcessCode.Text = "000";
            txtProcessNameF.Text = string.Empty;
            txtProcessNameS.Text = string.Empty;
            textShowOrder.Text = string.Empty;
            txtProcessNameS.Text = string.Empty;
            comboLType.SelectedIndex = -1;
            txtProcessCode.Enabled = true;
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            Max_id();
        }
        public void FetchProcessDetails()
        {
            dGProcess.Refresh();
            ActionType = "GetData";
            string[] str = { "@idproc", "@processcd", "@fname", "@sname", "@showord", "@inpscrtyp", "@created_at", "@updated_at", "@ActionType", "@inpscrtyp_id" };
            string[] obj = { "0", "", "", "", "", "","","", ActionType,"" };

            ds = helper.GetDatasetByCommandString("process_crud", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = null;
                dGProcess.AutoGenerateColumns = false;

                //Set Columns Count
                dGProcess.ColumnCount = 8;

                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";

                dGProcess.Columns[1].Name = "processcode";
                dGProcess.Columns[1].DataPropertyName = "processcode";

                dGProcess.Columns[2].Name = "shortname";
                dGProcess.Columns[2].DataPropertyName = "shortname";

                dGProcess.Columns[3].Name = "fullname";
                dGProcess.Columns[3].DataPropertyName = "fullname";

                dGProcess.Columns[4].Name = "showorder";
                dGProcess.Columns[4].DataPropertyName = "showorder";


                dGProcess.Columns[5].Name = "Input Screen Type";
                dGProcess.Columns[5].DataPropertyName = "inputscreentyp";
                dGProcess.Columns[5].Visible = false;

                dGProcess.Columns[6].Name = "idprocess";
                dGProcess.Columns[6].DataPropertyName = "idprocess";
                dGProcess.Columns[6].Visible = false;

                dGProcess.Columns[7].Name = "edit_allow_flag";
                dGProcess.Columns[7].DataPropertyName = "edit_allow_flag";
                dGProcess.Columns[7].Visible = false;
                dGProcess.DataSource = dt;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = dt;
            }
        }
        private void dGProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGProcess.Rows[rowIndex];
            txtProcessCode.Text = row.Cells[1].Value.ToString();
            txtProcessNameS.Text = row.Cells[2].Value.ToString();
            txtProcessNameF.Text = row.Cells[3].Value.ToString();
            textShowOrder.Text = row.Cells[4].Value.ToString();
  
            comboLType.Text = row.Cells[5].Value.ToString();
            Selected_Process_tbl_id = row.Cells[6].Value.ToString();
            edit_allow = row.Cells[7].Value.ToString();
            if (!checkboth_edit_delete_allow())
            {
                txtProcessCode.Enabled = true;
            }
            else if (checkboth_edit_delete_allow())
            {
                txtProcessCode.Enabled = false;
            }
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to create new Process ?", "CREATE PROCESS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "SaveData";
                        string[] str_exist = { "@processcd" };
                        string[] obj_exist = { txtProcessCode.Text };
                        MySqlDataReader already_exist = helper.GetReaderByCmd("process_code_already_exist", str_exist, obj_exist);
                        if (already_exist.Read())
                        {
                            MessageBox.Show("Process Code is already exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtProcessCode.Text = string.Empty;
                            txtProcessCode.Focus();
                            already_exist.Close();
                            helper.CloseConnection();
                        }
                        else
                        {
                            already_exist.Close();
                            helper.CloseConnection();
                            string[] str = { "@idproc", "@processcd", "@fname", "@sname", "@showord", "@inpscrtyp", "@created_at", "@updated_at", "@ActionType", "@inpscrtyp_id" };
                            string[] obj = { "0", txtProcessCode.Text, txtProcessNameF.Text, txtProcessNameS.Text, textShowOrder.Text, this.comboLType.GetItemText(this.comboLType.SelectedItem),nowdate.ToString(),"", ActionType, this.comboLType.GetItemText(this.comboLType.SelectedValue) };
                            MySqlDataReader sdr = helper.GetReaderByCmd("process_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                FetchProcessDetails();
                                MessageBox.Show("Process Created Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                throw ex;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Update the Process ?", "UPDATE PROCESS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput() && !check_process_alreadyexist())
                    {                        
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "UpdateData";
                            string[] str = { "@idproc", "@processcd", "@fname", "@sname", "@showord", "@inpscrtyp", "@created_at", "@updated_at", "@ActionType", "@inpscrtyp_id" };
                            string[] obj = { Selected_Process_tbl_id, txtProcessCode.Text, txtProcessNameF.Text, txtProcessNameS.Text, textShowOrder.Text, this.comboLType.GetItemText(this.comboLType.SelectedItem), "", nowdate.ToString(), ActionType, this.comboLType.GetItemText(this.comboLType.SelectedValue) };

                            MySqlDataReader sdr = helper.GetReaderByCmd("process_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                ds = new DataSet();
                                FetchProcessDetails();
                                btnSave.Enabled = false;
                                btnAdd.Enabled = true;
                                MessageBox.Show("Process Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DialogResult dialogResult = MessageBox.Show("Do you want to Delete the Process ?", "DELETE PROCESS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {                    
                    if (CheckInput())
                    {
                        if(!checkboth_edit_delete_allow())
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "DeleteData";
                            string[] str = { "@idproc", "@processcd", "@fname", "@sname", "@showord", "@inpscrtyp", "@created_at", "@updated_at", "@ActionType", "@inpscrtyp_id" };
                            string[] obj = { Selected_Process_tbl_id, "", "", "", "", "", "", "", ActionType, "" };
                            MySqlDataReader sdr = helper.GetReaderByCmd("process_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                ds = new DataSet();
                                FetchProcessDetails();
                                btnSave.Enabled = false;
                                btnAdd.Enabled = true;
                                MessageBox.Show("Process Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                sdr.Close();
                                helper.CloseConnection();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not allow to delete, Already mapped into BOM", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool checkboth_edit_delete_allow()
        {
            bool result = false;
            string ActionType = "GetData";
            string[] str_alreadyexist = { "@processcd", "@pnam",   "@ActionType" };
            string[] obj_alreadyexist = { txtProcessCode.Text,txtProcessNameF.Text,ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_delete_allow_process", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {               
                // bom table check already exits 
                string pk_bom_id = already_exist["processid"].ToString();
                if (pk_bom_id == txtProcessCode.Text)
                {
                    result = true;
                }
            }
            already_exist.Close();
            helper.CloseConnection();
            return result;
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ResetInput();
        }
        public bool Max_id()
        {
            bool result = false;
            string ActionType = "GetProcessMax";
            string[] str_alreadyexist = { "@uk_value1", "@uk_value2", "@ActionType" };
            string[] obj_alreadyexist = { string.Empty, string.Empty, ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("max_id_master", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {
                // max id
                txtProcessCode.Text = already_exist["processcode"].ToString();
            }
            already_exist.Close();
            helper.CloseConnection();
            return result;
        }
        public bool check_process_alreadyexist()
        {
            bool result = false;
            string[] str_exist = { "@processcd" };
            string[] obj_exist = { txtProcessCode.Text };
            MySqlDataReader already_exist = helper.GetReaderByCmd("process_code_already_exist", str_exist, obj_exist);
            if (already_exist.Read())
            {                
                txtProcessCode.Focus();
                already_exist.Close();
                helper.CloseConnection();
                result = false;
            }
            else
            {
                MessageBox.Show("Process Code is Not exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);          
                txtProcessCode.Focus();
                already_exist.Close();
                helper.CloseConnection();
                result =true;
            }
            return result;
        }

        private void btnprocess_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Process List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (dGProcess.Rows.Count > 0)
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
                        for (int i = 1; i < dGProcess.Columns.Count - 1; i++)
                        {
                            if (Date_column_names.Contains(dGProcess.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[1, i] = dGProcess.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dGProcess.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[1, i] = dGProcess.Columns[i - 1].HeaderText;
                                Date_column_index.Add(get_date_column);
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dGProcess.Rows.Count; i++)
                        {
                            for (int j = 0; j < dGProcess.Columns.Count-2; j++)
                            {
                                if (Convert.ToString(dGProcess.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check customer code column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGProcess.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        int formate_type = Convert.ToInt32(dGProcess.Rows[i].Cells[j].Value.ToString());
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
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess.Rows.Count, dGProcess.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess.Columns.Count]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGProcess.Columns.Count]].Font.Size = 13;
                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                   
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Process List -" + datetime;
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
    }
}
