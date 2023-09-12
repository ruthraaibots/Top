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
    public partial class FormMaterial : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        string Selected_Material_tbl_id = string.Empty;
        public FormMaterial()
        {
            InitializeComponent();
        }

        private void FormMaterial_Load(object sender, EventArgs e)
        {
            FetchMaterialDetails();
        }
        public void FetchMaterialDetails()
        {
            try
            {
                dGProcess.Refresh();
                ActionType = "GetData";
                string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@price", "@fname", "@created_at", "@updated_at", "@ActionType" };
                string[] obj = { "0", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ActionType };

                ds = helper.GetDatasetByCommandString("material_crud", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = null;
                    dGProcess.AutoGenerateColumns = false;
                    //Set Columns Count
                    dGProcess.ColumnCount = 9;
                    //Add Columns
                    dGProcess.Columns[0].Name = "Sno";
                    dGProcess.Columns[0].DataPropertyName = "sno";

                    dGProcess.Columns[1].Name = "Maker Code";
                    dGProcess.Columns[1].DataPropertyName = "makercode";

                    dGProcess.Columns[2].Name = "Material Code";
                    dGProcess.Columns[2].DataPropertyName = "materialcode";

                    dGProcess.Columns[3].Name = "Material Name (Full)";
                    dGProcess.Columns[3].DataPropertyName = "material_fullname";

                    dGProcess.Columns[4].Name = "Material Code";
                    dGProcess.Columns[4].DataPropertyName = "idmaterial";
                    dGProcess.Columns[4].Visible = false;

                    dGProcess.Columns[5].Name = "Maker Name (Full)";
                    dGProcess.Columns[5].DataPropertyName = "maker_fullname";
                    dGProcess.Columns[5].Visible = false;

                    dGProcess.Columns[6].Name = "Classification";
                    dGProcess.Columns[6].DataPropertyName = "classification";
                    dGProcess.Columns[6].Visible = true;

                    dGProcess.Columns[7].Name = "Price";
                    dGProcess.Columns[7].DataPropertyName = "price";
                    dGProcess.Columns[7].Visible = true;

                    dGProcess.Columns[8].Name = "Material Name (Short)";
                    dGProcess.Columns[8].DataPropertyName = "shortname";
                    dGProcess.Columns[8].Visible = false;

                    dGProcess.DataSource = dt;
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = dt;       
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void FetchMaterialDetails_makercode(string makercode)
        {
            try
            {
                dGProcess.Refresh();
                ActionType = "GetDataSingle";
                string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@price", "@fname", "@created_at", "@updated_at", "@ActionType" };
                string[] obj = { "0", makercode, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ActionType };
                ds = helper.GetDatasetByCommandString("material_crud", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = null;
                    dGProcess.AutoGenerateColumns = false;

                    //Set Columns Count
                    dGProcess.ColumnCount = 9;
                    //Add Columns
                    dGProcess.Columns[0].Name = "Sno";
                    dGProcess.Columns[0].DataPropertyName = "sno";

                    dGProcess.Columns[1].Name = "Maker Code";
                    dGProcess.Columns[1].DataPropertyName = "makercode";
                    dGProcess.Columns[1].DefaultCellStyle.Format = "D6";
                    dGProcess.Sort(dGProcess.Columns[1], ListSortDirection.Descending);

                    dGProcess.Columns[2].Name = "Material Code";
                    dGProcess.Columns[2].DataPropertyName = "materialcode";

                    dGProcess.Columns[3].Name = "Material Name (Full)";
                    dGProcess.Columns[3].DataPropertyName = "material_fullname";

                    dGProcess.Columns[4].Name = "idmaterial";
                    dGProcess.Columns[4].DataPropertyName = "idmaterial";
                    dGProcess.Columns[4].Visible = false;

                    dGProcess.Columns[5].Name = "Maker Name (Full)";
                    dGProcess.Columns[5].DataPropertyName = "maker_fullname";
                    dGProcess.Columns[5].Visible = false;

                    dGProcess.Columns[6].Name = "Classification";
                    dGProcess.Columns[6].DataPropertyName = "classification";
                    dGProcess.Columns[6].Visible = true;

                    dGProcess.Columns[7].Name = "Price";
                    dGProcess.Columns[7].DataPropertyName = "price";
                    dGProcess.Columns[7].Visible = true;

                    dGProcess.Columns[8].Name = "Material Name (Short)";
                    dGProcess.Columns[8].DataPropertyName = "shortname";
                    dGProcess.Columns[8].Visible = false;
                    dGProcess.DataSource = dt;
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = dt;
                    //MessageBox.Show("No Records Found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FormMaterial_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).materialToolStripMenuItem.Enabled = true;
        }
        private void textBox_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FormMaterial_KeyDown(object sender, KeyEventArgs e)
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
                btnmaterial_down.PerformClick();
            }
        }
        private void text_enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.AliceBlue;
        }
        private void text_leave(object sender, EventArgs e)
        {
            if (txtMakerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtMakerCode.Text);
                txtMakerCode.Text = formate_type.ToString("D6");
            }
            ((TextBox)sender).BackColor = Color.White;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            else if (txtMakerName.Text.Trim() == "")
            {
                MessageBox.Show("Maker Name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMakerName.Focus();
                result = false;
            }
            else if (txtMaterialCode.Text.Trim() == "" || txtMaterialCode.Text == "000000")
            {
                MessageBox.Show("Material Code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialCode.Focus();
                result = false;
            }
            else if (txtMaterialNameF.Text.Trim() == "")
            {
                MessageBox.Show("Material name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialNameF.Focus();
                result = false;
            }
            return result;
        }
        public void ResetInput()
        {            
            txtMaterialCode.Text = string.Empty;
            txtMaterialNameF.Text = string.Empty;
            txtClassification.Text = string.Empty;
            txtprice.Text = "0";
            btnSave.Enabled = true;
            btnAdd.Enabled = true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to create new Material ?", "CREATE MATERIAL", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "SaveData";
                        string[] str_exist = { "@materialcd" };
                        string[] obj_exist = { txtMaterialCode.Text };
                        MySqlDataReader already_exist = helper.GetReaderByCmd("material_code_already_exist", str_exist, obj_exist);
                        if (already_exist.Read())
                        {
                            MessageBox.Show("Material Code is already exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtMaterialCode.Text = string.Empty;
                            txtMaterialCode.Focus();
                            already_exist.Close();
                            helper.CloseConnection();
                        }
                        else
                        {
                            already_exist.Close();
                            helper.CloseConnection();
                            string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@price", "@fname",  "@created_at", "@updated_at", "@ActionType" };
                            string[] obj = { "0", txtMakerCode.Text,txtMaterialCode.Text,txtClassification.Text, txtprice.Text, txtMaterialNameF.Text, nowdate.ToString(), "", ActionType };
                            MySqlDataReader sdr = helper.GetReaderByCmd("material_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                FetchMaterialDetails_makercode(txtMakerCode.Text);
                                MessageBox.Show("Material Created Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnSearchMaker_Click(object sender, EventArgs e)
        {
            FormSearchMaker frm = new FormSearchMaker();
            frm.Owner = this;
            frm.OwnerName = "MT";
            frm.ShowDialog();           
        }
        public void SetSearchId(string code,string makername)
        {    
            if (code != string.Empty)
            {
                int formate_type = Convert.ToInt32(code);
                txtMakerCode.Text = formate_type.ToString("D6");
            }
            txtMakerName.Text = makername;
            FetchMaterialDetails_makercode(txtMakerCode.Text);
        }

        private void dGProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            Selected_Material_tbl_id = string.Empty;
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGProcess.Rows[rowIndex];
            txtMakerCode.Text = row.Cells[1].Value.ToString();
            txtMaterialCode.Text = row.Cells[2].Value.ToString();
            txtMaterialNameF.Text = row.Cells[3].Value.ToString();
            Selected_Material_tbl_id = row.Cells[4].Value.ToString();
            txtMakerName.Text = row.Cells[5].Value.ToString();
            txtClassification.Text = row.Cells[6].Value.ToString();
            txtprice.Text = row.Cells[7].Value.ToString();
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            if(!checkboth_editdelete_allow())
            {
                txtMakerCode.Enabled = true;
            }
            else if(checkboth_editdelete_allow())
            {
                txtMakerCode.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Update the Material ?", "UPDATE MATERIAL", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput() && !check_material_already_exist())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ActionType = "UpdateData";
                        string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@price", "@fname", "@created_at", "@updated_at", "@ActionType" };
                        string[] obj = { Selected_Material_tbl_id, txtMakerCode.Text, txtMaterialCode.Text, txtClassification.Text, txtprice.Text, txtMaterialNameF.Text, "", nowdate.ToString(), ActionType };
                        MySqlDataReader sdr = helper.GetReaderByCmd("material_crud", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                            ResetInput();
                            ds = new DataSet();                            
                            btnSave.Enabled = false;
                            btnAdd.Enabled = true;
                            FetchMaterialDetails_makercode(txtMakerCode.Text);
                            MessageBox.Show("Material Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DialogResult dialogResult = MessageBox.Show("Do you want to Delete the Material ?", "DELETE MATERIAL", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CheckInput())
                    {
                        if(!checkboth_editdelete_allow())
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "DeleteData";
                            string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@price", "@fname", "@created_at", "@updated_at", "@ActionType" };
                            string[] obj = { Selected_Material_tbl_id, "", "", "", "", "", "", "", ActionType };
                            MySqlDataReader sdr = helper.GetReaderByCmd("material_crud", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                ds = new DataSet();
                                btnSave.Enabled = false;
                                btnAdd.Enabled = true;
                                FetchMaterialDetails_makercode(txtMakerCode.Text);
                                MessageBox.Show("Material Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void text_leave_materialcd(object sender, EventArgs e)
        {
            if (txtMaterialCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtMaterialCode.Text);
                txtMaterialCode.Text = formate_type.ToString("D6");
            }
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public bool checkboth_editdelete_allow()
        {
            bool result = false;
            string ActionType = "GetDataMaterial";
            string[] str_alreadyexist = { "@uk_value1", "@uk_value2", "@uk_value3", "@ActionType" };
            string[] obj_alreadyexist = { txtMaterialCode.Text, string.Empty, string.Empty, ActionType };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_delete_allow_master", str_alreadyexist, obj_alreadyexist);
            if (already_exist.Read())
            {
                // bom table check already exits 
                string pk_material_cd = already_exist["material_code"].ToString();
                if (pk_material_cd == txtMaterialCode.Text)
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
        public bool check_material_already_exist()
        {
            bool result = false;
            string[] str_exist = { "@materialcd" };
            string[] obj_exist = { txtMaterialCode.Text };
            MySqlDataReader already_exist = helper.GetReaderByCmd("material_code_already_exist", str_exist, obj_exist);
            if (already_exist.Read())
            {
                already_exist.Close();
                helper.CloseConnection();
                result = false;
            }
            else
            {
                MessageBox.Show("Material Code is Not exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);    
                txtMaterialCode.Focus();
                already_exist.Close();
                helper.CloseConnection();
                result = true;
            }
            return result;
        }

        private void btnmaterial_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Material List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                        for (int i = 1; i < dGProcess.Columns.Count +1; i++)
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
                            for (int j = 0; j < dGProcess.Columns.Count; j++)
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
                        Excel.Range copyRange_F = XcelApp.Range["F:F"];
                        Excel.Range copyRange_I = XcelApp.Range["I:I"];
                        Excel.Range insertRange_C = XcelApp.Range["C:C"];
                        Excel.Range insertRange_E = XcelApp.Range["E:E"];
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_F.Cut());
                        insertRange_E.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_I.Cut());
                        Excel.Range DeleteRange_G = XcelApp.Range["G:G"];
                        DeleteRange_G.Delete();
                        // Auto fit automatically adjust the width of columns of Excel  in givien range .  
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
                        string compinepath = "\\Material List -" + datetime;
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
