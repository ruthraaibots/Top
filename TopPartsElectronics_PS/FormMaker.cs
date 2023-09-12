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
    public partial class FormMaker : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        string Selcted_Maker_tbl_id = string.Empty;
        public FormMaker()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMaker_Load(object sender, EventArgs e)
        {
            FetchMakerDetails();
            Max_id();
        }

        private void textBox_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FormMaker_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).makerToolStripMenuItem.Enabled = true;
        }
        private void FormMaker_KeyDown(object sender, KeyEventArgs e)
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
                btnmaker_down.PerformClick();
            }
        }
        private void text_enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.AliceBlue;
        }
        private void text_leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
            if (txtMakerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtMakerCode.Text);
                txtMakerCode.Text = formate_type.ToString("D6");
            }
        }
        public void FetchMakerDetails()
        {
            dGMaker.Refresh();
            ActionType = "GetData";
            string[] str = { "@idmak", "@makercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", "", "", "", "", "", ActionType };
            ds = helper.GetDatasetByCommandString("maker_crud", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGMaker.DataSource = null;
                dGMaker.AutoGenerateColumns = false;
                //Set Columns Count
                dGMaker.ColumnCount = 6;
                //Add Columns
                dGMaker.Columns[0].Name = "sno";
                dGMaker.Columns[0].DataPropertyName = "sno";

                dGMaker.Columns[1].Name = "makercode";
                dGMaker.Columns[1].DataPropertyName = "makercode";
                dGMaker.Columns[1].DefaultCellStyle.Format = "D6";

                dGMaker.Columns[2].Name = "shortname";
                dGMaker.Columns[2].DataPropertyName = "shortname";

                dGMaker.Columns[3].Name = "fullname";
                dGMaker.Columns[3].DataPropertyName = "fullname";

                dGMaker.Columns[4].Name = "edit_allow_flag";
                dGMaker.Columns[4].DataPropertyName = "edit_allow_flag";
                dGMaker.Columns[4].Visible = false;

                dGMaker.Columns[5].Name = "idmaker";
                dGMaker.Columns[5].DataPropertyName = "idmaker";
                dGMaker.Columns[5].Visible = false;
                dGMaker.DataSource = dt;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGMaker.DataSource = dt;   
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to create new Maker ?", "CREATE MAKER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "SaveData";
                        string[] str_exist = { "@makercd" };
                        string[] obj_exist = { txtMakerCode.Text };
                        MySqlDataReader already_exist = helper.GetReaderByCmd("maker_code_already_exist", str_exist, obj_exist);
                        if (already_exist.Read())
                        {
                            MessageBox.Show("Maker Code is already exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtMakerCode.Text = string.Empty;
                            txtMakerCode.Focus();
                            already_exist.Close();
                            helper.CloseConnection();
                        }
                        else
                        {
                            already_exist.Close();
                            helper.CloseConnection();
                            string[] str = { "@idmak", "@makercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                            string[] obj = {"0", txtMakerCode.Text, txtMakerNameF.Text, txtMakerNameS.Text, nowdate.ToString(), "", ActionType };
                            MySqlDataReader sdr = helper.GetReaderByCmd("maker_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                FetchMakerDetails();
                                MessageBox.Show("Maker Created Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public bool CheckInput()
        {
            bool result = true;
            if (txtMakerCode.Text.Trim() == "" || txtMakerCode.Text == "000000")
            {
                MessageBox.Show("Maker Code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMakerCode.Focus();
                result = false;
            }
            else if (txtMakerNameS.Text.Trim() == "")
            {
                MessageBox.Show("Maker Short name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMakerNameS.Focus();
                result = false;
            }
            else if (txtMakerNameF.Text.Trim() == "")
            {
                MessageBox.Show("Maker Full name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMakerNameF.Focus();
                result = false;
            }

            return result;
        }
        public void ResetInput()
        {
            txtMakerCode.Text = "000000";
            txtMakerNameF.Text = string.Empty;
            txtMakerNameS.Text = string.Empty;
            txtMakerCode.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = true;
            Max_id();
        }

        private void dGMaker_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            string edit_allow = string.Empty;
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGMaker.Rows[rowIndex];
            if (txtMakerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(row.Cells[1].Value.ToString());
                txtMakerCode.Text = formate_type.ToString("D6");
            }
            txtMakerCode.Text = row.Cells[1].Value.ToString();
            txtMakerNameS.Text = row.Cells[2].Value.ToString();
            txtMakerNameF.Text = row.Cells[3].Value.ToString();
            edit_allow = row.Cells[4].Value.ToString();
            Selcted_Maker_tbl_id= row.Cells[5].Value.ToString();
            if (!checkboth_edit_delete_allow())
            {                
                txtMakerCode.Enabled = false;
            }
            else if(checkboth_edit_delete_allow())
            {
                txtMakerCode.Enabled = true;
            }
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            txtMakerCode.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Update the Maker ?", "UPDATE MAKER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput() && !check_makercd_alreadyexist())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "UpdateData";
                        string[] str = { "@idmak", "@makercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                        string[] obj = { Selcted_Maker_tbl_id, txtMakerCode.Text, txtMakerNameF.Text, txtMakerNameS.Text, nowdate.ToString(), "", ActionType };
                        MySqlDataReader sdr = helper.GetReaderByCmd("maker_crud", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                            ResetInput();
                            ds = new DataSet();
                            FetchMakerDetails();
                            btnSave.Enabled = false;
                            btnAdd.Enabled = true;
                            MessageBox.Show("Maker Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                DialogResult dialogResult = MessageBox.Show("Do you want to Delete the Maker ?", "DELETE MAKER", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        if(!checkboth_edit_delete_allow())
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "DeleteData";
                            string[] str = { "@idmak", "@makercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                            string[] obj = { "0", txtMakerCode.Text, txtMakerNameF.Text, txtMakerNameS.Text, nowdate.ToString(), "", ActionType };

                            MySqlDataReader sdr = helper.GetReaderByCmd("maker_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                ds = new DataSet();
                                FetchMakerDetails();
                                btnSave.Enabled = false;
                                btnAdd.Enabled = true;
                                MessageBox.Show("Maker Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                sdr.Close();
                                helper.CloseConnection();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not allow to delete, Already mapped into Material", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string ActionType = "GetDataMaker";
            string[] str_alreadyexist = { "@uk_value1", "@uk_value2", "@uk_value3", "@ActionType" };
            string[] obj_alreadyexist = { txtMakerCode.Text, string.Empty, string.Empty, ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_delete_allow_master", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {
                // bom table check already exits 
                string pk_maker_cd = already_exist["makercode"].ToString();
                if (pk_maker_cd == txtMakerCode.Text)
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
            string ActionType = "GetMakerMax";
            string[] str_alreadyexist = { "@uk_value1", "@uk_value2", "@ActionType" };
            string[] obj_alreadyexist = { string.Empty, string.Empty, ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("max_id_master", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {
                // max id
                txtMakerCode.Text = already_exist["makercode"].ToString();
                if (txtMakerCode.Text != string.Empty)
                {
                    int formate_type = Convert.ToInt32(txtMakerCode.Text);
                    txtMakerCode.Text = formate_type.ToString("D6");
                } 
            }
            already_exist.Close();
            helper.CloseConnection();
            return result;
        }
        public bool check_makercd_alreadyexist()
        {
            bool result = false;
            string[] str_exist = { "@makercd" };
            string[] obj_exist = { txtMakerCode.Text };
            MySqlDataReader already_exist = helper.GetReaderByCmd("maker_code_already_exist", str_exist, obj_exist);
            if (already_exist.Read())
            {                
                already_exist.Close();
                helper.CloseConnection();
                result = false;
            }
            else
            {
                MessageBox.Show("Maker Code is Not exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Maker List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {                    
                    if (dGMaker.Rows.Count > 0)
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
                        for (int i = 1; i < dGMaker.Columns.Count-1; i++)
                        {
                            if (Date_column_names.Contains(dGMaker.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[1, i] = dGMaker.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dGMaker.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[1, i] = dGMaker.Columns[i - 1].HeaderText;
                                Date_column_index.Add(get_date_column);
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dGMaker.Rows.Count; i++)
                        {
                            for (int j = 0; j < dGMaker.Columns.Count-2; j++)
                            {
                                if (Convert.ToString(dGMaker.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check customer code column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGMaker.Rows[i].Cells[j].Value.ToString();
                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        int formate_type = Convert.ToInt32(dGMaker.Rows[i].Cells[j].Value.ToString());
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
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGMaker.Rows.Count, dGMaker.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGMaker.Columns.Count]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGMaker.Columns.Count]].Font.Size = 13;
                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                      
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Maker List -" + datetime;
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
