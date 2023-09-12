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
using TopPartsElectronics_PS.Helper;

namespace TopPartsElectronics_PS
{
    public partial class FormProduct : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        string itemname_old =string.Empty;
        string Selected_Product_tbl_id = string.Empty;
        string edit_allow = string.Empty;
        public FormProduct()
        {
            InitializeComponent();
        }
        private void FormProduct_Load(object sender, EventArgs e)
        {
            labeltype();
            currency();
        }
        public void labeltype()
        {
            ActionType = "GetData";
            string[] str = { "@ActionType" };
            string[] obj = {  ActionType };
            DataTable sdr = helper.GetDatasetByCommandString_dt("labletype_get", str, obj);
            comboLType.Items.Clear();
            comboLType.DisplayMember = "labeltype";
            comboLType.ValueMember = "labelkey";
            comboLType.DataSource = sdr;
            helper.CloseConnection();
        }
        public void currency()
        {
            ActionType = "GetData";
            string[] str = { "@ActionType" };
            string[] obj = { ActionType };
            DataTable sdr = helper.GetDatasetByCommandString_dt("currency_get", str, obj);
            comboCurrency.Items.Clear();
            comboCurrency.DisplayMember = "cshortname";
            comboCurrency.ValueMember = "currencykey";
            comboCurrency.DataSource = sdr;
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
        private void textBox_KeyPressFloat(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void FormProduct_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).productToolStripMenuItem.Enabled = true;
        }
        private void FormProduct_KeyDown(object sender, KeyEventArgs e)
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
                btnproduct_down.PerformClick();
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
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void textPrice_TextChanged(object sender, EventArgs e)
        {
            string buf = textPrice.Text;

            float val;

            if (float.TryParse(textPrice.Text, out val))
            {

            }
            else
            {

                if (buf.Length > 1)
                {
                    buf = buf.Substring(0, buf.Length - 1);
                }
                else
                {
                    buf = "";
                }
                textPrice.Text = buf;
            }
        }
        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            MysqlHelper.call_from_search_client = false;
            FormSearchClient frm = new FormSearchClient();
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ShowDialog();
        }
        public void SetSearchId(string code, string shortname, string fullname)
        {
            ResetInput();        
            if (code != string.Empty)
            {
                int formate_type = Convert.ToInt32(code);
                txtCustomerCode.Text = formate_type.ToString("D6");
            }
            txtCustomerNameS.Text = shortname;
            txtCustomerNameF.Text = fullname;
            FetchProductDetails(txtCustomerCode.Text, string.Empty);
        }
        public bool CheckInput()
        {
            bool result = true;
            if (txtCustomerCode.Text.Trim() == "" || txtCustomerCode.Text == "000000")
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
            else if (textItemCode.Text.Trim() == "")
            {
                MessageBox.Show("Item code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textItemCode.Focus();
                result = false;
            }
            else if (textItemName.Text.Trim() == "")
            {
                MessageBox.Show("Item name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textItemName.Focus();
                result = false;
            }
            else if (comboCurrency.SelectedIndex==-1)
            {
                MessageBox.Show("Unit price is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboCurrency.Focus();
                result = false;
            }
            else if (textPrice.Text.Trim() == "")
            {
                MessageBox.Show("Price is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textPrice.Focus();
                result = false;
            }
            else if (textQuantity.Text.Trim() == "")
            {
                MessageBox.Show("Box Qty is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textQuantity.Focus();
                result = false;
            }
            else if (comboLType.SelectedIndex == -1)
            {
                MessageBox.Show("Lable Type is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboLType.Focus();
                result = false;
            }
            else if (textMark1.Text.Trim() == "")
            {
                MessageBox.Show("Mark 1 is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textMark1.Focus();
                result = false;
            }
            else if (textMark2.Text.Trim() == "")
            {
                MessageBox.Show("Mark 2 is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textMark2.Focus();
                result = false;
            }
            else if (textMark3.Text.Trim() == "")
            {
                MessageBox.Show("Mark 3 name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textMark3.Focus();
                result = false;
            }
            else if (textMark4.Text.Trim() == "")
            {
                MessageBox.Show("Mark 4 name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textMark4.Focus();
                result = false;
            }
            return result;
        }
        public bool CheckInput_customercode()
        {
            bool result = true;
            if (txtCustomerCode.Text == "000000" || txtCustomerNameS.Text == "")
            {
                if (txtCustomerCode.Text.Trim() == "" && txtCustomerNameS.Text == "")
                {
                    MessageBox.Show("Atleast Fill anyone of this Maker Code Or Short Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerCode.Focus();
                    txtCustomerCode.Text = string.Empty;
                    txtCustomerNameF.Text=string.Empty;
                    txtCustomerNameS.Text = string.Empty;
                    result = false;
                }
            }
            return result;
        }
        public void ResetInput()
        {               
            textItemCode.Text = string.Empty;
            textItemName.Text = string.Empty;
            textAdditionalCode.Text = string.Empty;
            comboCurrency.SelectedIndex = -1;
            textPrice.Text = string.Empty;
            textQuantity.Text = string.Empty;
            comboLType.SelectedIndex = -1;
            textMark1.Text = string.Empty;
            textMark2.Text = string.Empty;
            textMark3.Text = string.Empty;
            textMark4.Text = string.Empty;
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            itemname_old = string.Empty;
        }
        public void FetchProductDetails(string custcd,string shortname)
        {
            dGProcess.Refresh();
            ActionType = "GetData";
            string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
            string[] obj = { custcd, shortname,"", ActionType };
            ds = helper.GetDatasetByCommandString("product_view", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                txtCustomerCode.Text=dt.Rows[0]["customercode"].ToString();
                txtCustomerNameF.Text = dt.Rows[0]["fullname"].ToString();
                txtCustomerNameS.Text = dt.Rows[0]["shortname"].ToString();

                dGProcess.DataSource = null;
                dGProcess.AutoGenerateColumns = false;

                //Set Columns Count
                dGProcess.ColumnCount = 18;

                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";

                dGProcess.Columns[1].Name = "customercode";
                dGProcess.Columns[1].DataPropertyName = "customercode";

                dGProcess.Columns[2].Name = "itemcode";
                dGProcess.Columns[2].DataPropertyName = "itemcode";

                dGProcess.Columns[3].Name = "itemname";
                dGProcess.Columns[3].DataPropertyName = "itemname";

                dGProcess.Columns[4].Name = "unitprice";
                dGProcess.Columns[4].DataPropertyName = "unitprice";

                dGProcess.Columns[5].Name = "boxqty";
                dGProcess.Columns[5].DataPropertyName = "boxqty";

                dGProcess.Columns[6].Name = "additional_code";
                dGProcess.Columns[6].DataPropertyName = "additional_code";

                dGProcess.Columns[7].Name = "mark_1";
                dGProcess.Columns[7].DataPropertyName = "mark_1";

                dGProcess.Columns[8].Name = "mark_2";
                dGProcess.Columns[8].DataPropertyName = "mark_2";

                dGProcess.Columns[9].Name = "mark_3";
                dGProcess.Columns[9].DataPropertyName = "mark_3";      

                dGProcess.Columns[10].Name = "mark_4";
                dGProcess.Columns[10].DataPropertyName = "mark_4";

                dGProcess.Columns[11].Name = "labletype";
                dGProcess.Columns[11].DataPropertyName = "labletype";

                dGProcess.Columns[12].Name = "idproduct";
                dGProcess.Columns[12].DataPropertyName = "idproduct";
                dGProcess.Columns[12].Visible = false;

                dGProcess.Columns[13].Name = "Unit Price";
                dGProcess.Columns[13].DataPropertyName = "unitprice_drp";
                dGProcess.Columns[13].Visible = false;

                dGProcess.Columns[14].Name = "Customer Name (Full)";
                dGProcess.Columns[14].DataPropertyName = "fullname";
                dGProcess.Columns[14].Visible = false;

                dGProcess.Columns[15].Name = "Customer Name (Short)";
                dGProcess.Columns[15].DataPropertyName = "shortname";
                dGProcess.Columns[15].Visible = false;

                dGProcess.Columns[16].Name = "edit_allow_flag";
                dGProcess.Columns[16].DataPropertyName = "edit_allow_flag";
                dGProcess.Columns[16].Visible = false;

                dGProcess.Columns[17].Name = "Label Type";
                dGProcess.Columns[17].DataPropertyName = "labeltype_text";
                dGProcess.Columns[17].Visible = true;
                dGProcess.DataSource = dt;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = dt;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {                
                if (CheckInput())
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to create new Procuct ?", "CREATE PRODUCT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ActionType = "SaveData";
                        string[] str_cexist = { "@custcd" };
                        string[] obj_cexist = { txtCustomerCode.Text };
                        MySqlDataReader already_cexist = helper.GetReaderByCmd("customer_code_exits", str_cexist, obj_cexist);
                        if (already_cexist.Read())
                        {
                            already_cexist.Close();
                            helper.CloseConnection();
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "SaveData";
                            string[] str_exist = { "@itmcd", "@itmnam", "@ActionType" };
                            string[] obj_exist = { textItemCode.Text,textItemName.Text, "itmcd" };
                            MySqlDataReader already_exist = helper.GetReaderByCmd("product_code_already_exist", str_exist, obj_exist);
                            if (already_exist.Read())
                            {
                                MessageBox.Show("Item Code is already exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                textItemCode.Focus();
                                already_exist.Close();
                                helper.CloseConnection();
                            }
                            else
                            {
                                already_exist.Close();
                                helper.CloseConnection();
                                string[] str = { "@idpro", "@customercd", "@fname", "@sname", "@itcd", "@itnam", "@unitdrp", "@unitp", "@bxqty", "@addcd", "@labletype", "@m1", "@m2", "@m3", "@m4", "@created_at", "@updated_at", "@ActionType" };
                                string[] obj = { "0", txtCustomerCode.Text,
                                txtCustomerNameF.Text,
                                txtCustomerNameS.Text,
                                textItemCode.Text,
                                textItemName.Text,
                                comboCurrency.GetItemText(this.comboCurrency.SelectedItem),
                                textPrice.Text,
                                textQuantity.Text,
                                textAdditionalCode.Text,
                                comboLType.GetItemText(this.comboLType.SelectedValue),
                                textMark1.Text,
                                textMark2.Text,
                                textMark3.Text,
                                textMark4.Text,
                                nowdate.ToString(),
                                string.Empty,
                                ActionType };
                                MySqlDataReader sdr = helper.GetReaderByCmd("product_ins", str, obj);
                                if (sdr.Read())
                                {
                                    sdr.Close();
                                    helper.CloseConnection();

                                    MessageBox.Show("Product Created Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ResetInput();
                                    FetchProductDetails(txtCustomerCode.Text, "");
                                }
                                else
                                {
                                    sdr.Close();
                                    helper.CloseConnection();
                                }
                            }
                        }
                        else
                        {
                            already_cexist.Close();
                            helper.CloseConnection();
                            MessageBox.Show("Customer Code its not register", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCustomerCode.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            txtCustomerCode.Text = row.Cells[1].Value.ToString();
            txtCustomerNameS.Text = row.Cells[14].Value.ToString();
            txtCustomerNameF.Text = row.Cells[15].Value.ToString();
            textItemCode.Text = row.Cells[2].Value.ToString();
            textItemName.Text = row.Cells[3].Value.ToString();
            itemname_old= row.Cells[3].Value.ToString();
            comboCurrency.Text = row.Cells[13].Value.ToString();
            textPrice.Text = row.Cells[4].Value.ToString();
            textQuantity.Text = row.Cells[5].Value.ToString();
            textAdditionalCode.Text = row.Cells[6].Value.ToString();
            textMark1.Text = row.Cells[7].Value.ToString();
            textMark2.Text = row.Cells[8].Value.ToString();
            textMark3.Text = row.Cells[9].Value.ToString();
            textMark4.Text = row.Cells[10].Value.ToString();
            comboLType.Text = row.Cells[17].Value.ToString();
            Selected_Product_tbl_id = row.Cells[12].Value.ToString();
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckInput() && !check_product_alreadyexist())
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to Update the Product ?", "UPDATE PRODUCT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if(check_itemname_alreadyexist())
                        {
                            ActionType = "UpdateData";
                        }
                        else if(!check_itemname_alreadyexist())
                        {
                            ActionType = "UpdItmname";
                        }                       
                        string[] str = { "@idpro", "@customercd", "@fname", "@sname", "@itcd", "@itnam", "@unitdrp", "@unitp", "@bxqty", "@addcd", "@lbltyp", "@m1", "@m2", "@m3", "@m4", "@crt_at", "@upd_at", "@ActionType","@uid","@madrs" , "@itnamold" };
                        string[] obj = { Selected_Product_tbl_id, txtCustomerCode.Text,
                                txtCustomerNameF.Text,
                                txtCustomerNameS.Text,
                                textItemCode.Text,
                                textItemName.Text,
                                comboCurrency.GetItemText(this.comboCurrency.SelectedItem),
                                textPrice.Text,
                                textQuantity.Text,
                                textAdditionalCode.Text,
                                comboLType.SelectedValue.ToString(),
                                textMark1.Text,
                                textMark2.Text,
                                textMark3.Text,
                                textMark4.Text,
                                 string.Empty,
                                 nowdate.ToString(),
                                ActionType,
                                CommonClass.logged_Id,
                                CommonClass.MacAddress,
                                itemname_old
                        };
                        MySqlDataReader sdr = helper.GetReaderByCmd("product_update", str, obj);
                        if (sdr.Read())
                        {
                            sdr.Close();
                            helper.CloseConnection();
                            ResetInput();
                            ds = new DataSet();                        
                            btnSave.Enabled = true;
                            btnAdd.Enabled = true;
                            itemname_old = string.Empty;
                            MessageBox.Show("Product Updated Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);              
                            FetchProductDetails(txtCustomerCode.Text, "");
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
                if (CheckInput())
                {
                    if(!check_delete_allow())
                    {
                        DialogResult dialogResult = MessageBox.Show("Do you want to Delete the Product ?", "DELETE PROCESS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "DeleteData";
                            string[] str = { "@idpro", "@ActionType" };
                            string[] obj = { Selected_Product_tbl_id, ActionType };

                            MySqlDataReader sdr = helper.GetReaderByCmd("product_delete", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                ds = new DataSet();
                                btnSave.Enabled = false;
                                btnAdd.Enabled = true;
                                FetchProductDetails(txtCustomerCode.Text, string.Empty);
                                MessageBox.Show("Product Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {
                                sdr.Close();
                                helper.CloseConnection();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not allow to delete, Already mapped into BOM", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private void txtCustomerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_customercode())
                {
                    FetchProductDetails(txtCustomerCode.Text, "");
                }
            }
        }
        private void txtCustomerNameS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_customercode())
                {                  
                    FetchProductDetails("",txtCustomerNameS.Text);                    
                }
            }
        }
        public bool check_delete_allow()
        {
            bool result = false;
            string[] str_exist = { "@custcd", "@itemcd", "@itemnam","@ActionType" };
            string[] obj_exist = { txtCustomerCode.Text, textItemCode.Text, textItemName.Text,"GetData" };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_delete_allow_product", str_exist, obj_exist);
            if (already_exist.Read())
            {
                // bom table check already exits 
                string pk_bom_id = already_exist["idbom"].ToString();
                result = true;
            }
            already_exist.Close();
            helper.CloseConnection();
            return result;
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ResetInput();
        }
        public bool check_product_alreadyexist()
        {
            bool result = false;
            string[] str_exist = { "@itmcd", "@itmnam", "@ActionType" };
            string[] obj_exist = { textItemCode.Text,textItemName.Text, "itmcd" };
            MySqlDataReader already_exist = helper.GetReaderByCmd("product_code_already_exist", str_exist, obj_exist);
            if (already_exist.Read())
            {
                result = false;
                already_exist.Close();
                helper.CloseConnection();
            }
            else
            {
                MessageBox.Show("Item Code is Not exist..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textItemCode.Focus();
                already_exist.Close();
                helper.CloseConnection();
                result = true;
            }
            return result;
        }
        private void btnproduct_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Product List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                        for (int i = 1; i < dGProcess.Columns.Count+1; i++)
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
                        Excel.Range copyRange_L = XcelApp.Range["L:L"];
                        Excel.Range copyRange_M = XcelApp.Range["M:M"];
                        Excel.Range copyRange_N = XcelApp.Range["N:N"];
                        Excel.Range copyRange_O = XcelApp.Range["O:O"];
                        Excel.Range copyRange_P = XcelApp.Range["P:P"];
                        Excel.Range copyRange_Q = XcelApp.Range["Q:Q"];
                        Excel.Range copyRange_R = XcelApp.Range["R:R"];
                        Excel.Range insertRange_C = XcelApp.Range["C:C"];
                        Excel.Range insertRange_F = XcelApp.Range["F:F"];
                        Excel.Range insertRange_L = XcelApp.Range["L:L"];
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_O.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_P.Cut());
                        insertRange_F.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_N.Cut());
                        insertRange_L.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_R.Cut());                        
                        Excel.Range DeleteRange_P = XcelApp.Range["P:P"];
                        Excel.Range DeleteRange_Q = XcelApp.Range["Q:Q"];
                        Excel.Range DeleteRange_R = XcelApp.Range["R:R"];   
                        DeleteRange_P.Delete();
                        DeleteRange_Q.Delete();
                        DeleteRange_R.Delete();
                        //Auto fit automatically adjust the width of columns of Excel  in givien range 
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess.Rows.Count, dGProcess.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess.Columns.Count]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGProcess.Columns.Count]].Font.Size = 13;
                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                   
                        string customerid = txtCustomerCode.Text;
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Product List -" + datetime;
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
        public bool check_itemname_alreadyexist()
        {
            bool result = false;
            try
            {
                string[] str_exist = { "@itmcd", "@itmnam", "@ActionType" };
                string[] obj_exist = { textItemCode.Text,textItemName.Text, "itmcdnam" };
                MySqlDataReader already_exist = helper.GetReaderByCmd("product_code_already_exist", str_exist, obj_exist);
                if (already_exist.Read())
                {
                    result = true;
                    already_exist.Close();
                    helper.CloseConnection();
                }
                else
                {                    
                    already_exist.Close();
                    helper.CloseConnection();
                }
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
