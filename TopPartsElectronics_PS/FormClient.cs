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
    public partial class FormClient : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        public FormClient()
        {
            InitializeComponent();
        }
        private void FormClient_Load(object sender, EventArgs e)
        {
            Max_id();
            FetchCustomerDetails();
        }        
        private void FormClient_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).clientToolStripMenuItem.Enabled = true;
        }
        private void FormClient_KeyDown(object sender, KeyEventArgs e)
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
            if (e.KeyCode == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            if (e.KeyCode == Keys.F9)
            {
                btnClose.PerformClick();
            }
            if (e.KeyCode == Keys.F7)
            {
                btnclient_down.PerformClick();
            }
        }
        private void text_enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.AliceBlue;
        }
        private void text_leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
            if (txtCustomerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtCustomerCode.Text);
                txtCustomerCode.Text = formate_type.ToString("D6");
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }         
        }
        public void FetchCustomerDetails()
        {
            dGClient.Refresh();
            ActionType = "GetData";
            string[] str = { "@idcust", "@customercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", "", "", "", "", "", ActionType };

            ds = helper.GetDatasetByCommandString("customer_crud", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGClient.DataSource = null;
                dGClient.AutoGenerateColumns = false;
                //Set Columns Count
                dGClient.ColumnCount = 4;
                //Add Columns
                dGClient.Columns[0].Name = "sno";
                dGClient.Columns[0].DataPropertyName = "sno";
                dGClient.Columns[1].Name = "customercode";
                dGClient.Columns[1].DataPropertyName = "customercode";
                dGClient.Columns[1].DefaultCellStyle.Format = "D6";
                dGClient.Sort(dGClient.Columns[1], ListSortDirection.Descending);

                dGClient.Columns[2].Name = "shortname";
                dGClient.Columns[2].DataPropertyName = "shortname";

                dGClient.Columns[3].Name = "fullname";
                dGClient.Columns[3].DataPropertyName = "fullname"; 

                dGClient.DataSource = dt;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGClient.DataSource = dt;         
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to create new Customer ?", "CREATE CUSTOMER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "SaveData";
                        string[] str_exist = { "@customercd" };
                        string[] obj_exist = { txtCustomerCode.Text };
                        MySqlDataReader already_exist = helper.GetReaderByCmd("customer_code_already_exist", str_exist, obj_exist);
                        if (already_exist.Read())
                        {
                            MessageBox.Show("Customer Code is already exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCustomerCode.Text=string.Empty;
                            txtCustomerCode.Focus();
                            already_exist.Close();
                            helper.CloseConnection();
                        }                        
                        else
                        {
                            already_exist.Close();
                            helper.CloseConnection();
                            string[] str_snam_exist = { "@sname" };
                            string[] obj_snam_exist = {  txtCustomerNameS.Text };
                            MySqlDataReader already_snam_exist = helper.GetReaderByCmd("customer_shortname_exits", str_snam_exist, obj_snam_exist);

                            // customer short name
                            if (already_snam_exist.Read())
                            {                               
                                MessageBox.Show("Customer Short Name is already exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtCustomerNameS.Text = string.Empty;
                                txtCustomerNameS.Focus();
                                already_snam_exist.Close();
                                helper.CloseConnection();
                            }
                            else
                            {
                                already_snam_exist.Close();
                                helper.CloseConnection();
                                string[] str = { "@idcust", "@customercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                                string[] obj = { "0", txtCustomerCode.Text, txtCustomerNameF.Text, txtCustomerNameS.Text, nowdate.ToString(), "", ActionType };

                                MySqlDataReader sdr = helper.GetReaderByCmd("customer_crud", str, obj);
                                if (sdr.Read())
                                {
                                    sdr.Close();
                                    helper.CloseConnection();
                                    FetchCustomerDetails();
                                    MessageBox.Show("Customer Created Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }    
        public bool CheckInput()
        {
            bool result = true;
            if (txtCustomerCode.Text.Trim() == "" || txtCustomerCode.Text== "000000")
            {
                MessageBox.Show("Customer Code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerCode.Focus();
                result = false;
            }
            else if (txtCustomerNameS.Text.Trim() == "")
            {
                MessageBox.Show("Customer Short name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerNameS.Focus();
                result = false;
            }
            else if (txtCustomerNameF.Text.Trim() == "")
            {
                MessageBox.Show("Customer Full name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerNameF.Focus();
                result = false;
            }
          
            return result;
        }
        public void ResetInput()
        {
            txtCustomerCode.Text = "000000"; 
            txtCustomerNameF.Text = string.Empty;
            txtCustomerNameS.Text = string.Empty;
            txtCustomerCode.Enabled = true;
            btnSave.Enabled = true;
            Max_id();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Update the Customer ?", "UPDATE CUSROMER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput() && !check_customer_code_alredyexist())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "UpdateData";
                        string[] str = { "@idcust", "@customercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                        string[] obj = { "0", txtCustomerCode.Text, txtCustomerNameF.Text, txtCustomerNameS.Text, "", nowdate.ToString(), ActionType };
                        MySqlDataReader sdr = helper.GetReaderByCmd("customer_crud", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                            ResetInput();
                            ds = new DataSet();
                            FetchCustomerDetails();
                            btnSave.Enabled = false;
                            btnAdd.Enabled = true;
                            MessageBox.Show("Customer Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dGClient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGClient.Rows[rowIndex];
            if (txtCustomerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(row.Cells[1].Value.ToString());
                txtCustomerCode.Text = formate_type.ToString("D6");
            }           
            txtCustomerNameS.Text = row.Cells[2].Value.ToString();
            txtCustomerNameF.Text = row.Cells[3].Value.ToString();
            if(!checkboth_edit_delete_allow())
            {
                txtCustomerCode.Enabled = true;
            }
            else if(checkboth_edit_delete_allow())
            {
                txtCustomerCode.Enabled = false;
            }
         
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Delete the Customer ?", "DELETE CUSTOMER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        if(!checkboth_edit_delete_allow())
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "DeleteData";
                            string[] str = { "@idcust", "@customercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                            string[] obj = { "0", txtCustomerCode.Text, txtCustomerNameF.Text, txtCustomerNameS.Text, "", nowdate.ToString(), ActionType };

                            MySqlDataReader sdr = helper.GetReaderByCmd("customer_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                ds = new DataSet();
                                FetchCustomerDetails();
                                btnSave.Enabled = false;
                                btnAdd.Enabled = true;
                                MessageBox.Show("Customer Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {
                                sdr.Close();
                                helper.CloseConnection();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not allow to delete, Already mapped into Product", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string ActionType = "GetDataClient";
            string[] str_alreadyexist = { "@uk_value1", "@uk_value2", "@uk_value3", "@ActionType" };
            string[] obj_alreadyexist = { txtCustomerCode.Text, string.Empty, string.Empty, ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_delete_allow_master", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {
                // bom table check already exits 
                string pk_customer_id = already_exist["customercode"].ToString();
                if(pk_customer_id==txtCustomerCode.Text)
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
            btnAdd.Enabled = true;
            ResetInput();
        }
        public bool Max_id()
        {
            bool result = false;
            string ActionType = "GetCustomerMax";
            string[] str_alreadyexist = { "@uk_value1", "@uk_value2",  "@ActionType" };
            string[] obj_alreadyexist = { string.Empty, string.Empty, ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("max_id_master", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {
                // max id
                txtCustomerCode.Text = already_exist["customercode"].ToString();                

            }
            already_exist.Close();
            helper.CloseConnection();
            return result;
        }
        public bool check_customer_code_alredyexist()
        {
            bool result = false;
            string[] str_exist = { "@customercd" };
            string[] obj_exist = { txtCustomerCode.Text };
            MySqlDataReader already_exist = helper.GetReaderByCmd("customer_code_already_exist", str_exist, obj_exist);
            if (already_exist.Read())
            {               
                already_exist.Close();
                helper.CloseConnection();
                result = false;
            }
            else
            {
                MessageBox.Show("Customer Code is not exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCustomerCode.Focus();
                already_exist.Close();
                helper.CloseConnection();
                result = true;
            }
            return result;
        }

        private void btnclient_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Client List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //MergeCells();
                    if (dGClient.Rows.Count > 0)
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
                        for (int i = 1; i < dGClient.Columns.Count+1; i++)
                        {
                            if (Date_column_names.Contains(dGClient.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[1, i] = dGClient.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dGClient.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[1, i] = dGClient.Columns[i - 1].HeaderText;
                                Date_column_index.Add(get_date_column);
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dGClient.Rows.Count; i++)
                        {
                            for (int j = 0; j < dGClient.Columns.Count; j++)
                            {
                                if (Convert.ToString(dGClient.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check customer code column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGClient.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        int formate_type = Convert.ToInt32(dGClient.Rows[i].Cells[j].Value.ToString());
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
                        // Auto fit automatically adjust the width of columns of Excel  in givien range .  
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGClient.Rows.Count, dGClient.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGClient.Columns.Count]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGClient.Columns.Count]].Font.Size = 13;
                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                        
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Customer List -" + datetime;
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
