using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    public partial class FormBOM : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        string Selected_BOM_tbl_id = string.Empty;
        string edit_allow = string.Empty;
        int Grid_sno = 0;
        int squence_sno = 0;
        int auto_generation_max_bomcode = 0;
        int idbom_view = 0;
        int bomcode_view = 0;
        int bom_tbl_pk = 0;
        string selected_processid = "0";
        DataGridViewRow newRow = new DataGridViewRow();
        DataTable dts = new DataTable();
        bool BOM_view_process_started = false;
        bool apply_change = false;
        DataTable dt = new DataTable();
        DataTable add_dt = new DataTable();
        public FormBOM()
        {
            InitializeComponent();
        }

        private void FormBOM_Load(object sender, EventArgs e)
        {
            dts_table_columns();
            max_user_id();
            textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();
        }
        public void dts_table_columns()
        {
            dts.Columns.Add("SNo.");
            dts.Columns.Add("sno");
            dts.Columns.Add("customercode");
            dts.Columns.Add("itemcode");
            dts.Columns.Add("itemname");
            dts.Columns.Add("process_order");
            dts.Columns.Add("process");
            dts.Columns.Add("material_code");
            dts.Columns.Add("material_name");
            dts.Columns.Add("customer_fullnam");
            dts.Columns.Add("customer_shortname");
            dts.Columns.Add("edit_allow_flag");
            dts.Columns.Add("idbom");
            dts.Columns.Add("bomcode");
            dts.Columns.Add("inputscreentyp");
            dts.Columns.Add("inputscreentyp_id");
            dts.Columns.Add("processcode");
            dGProcess_new.DataSource = dts;
            // Get Process drp
            DataTable dt = helper.ProcessList();
            cmbProcess.Items.Clear();
            cmbProcess.DisplayMember = "fullname";
            //cmbProcess.ValueMember = "idprocess"; 
            cmbProcess.ValueMember = "processcode";
            cmbProcess.DataSource = dt;
        }
        public void max_user_id()
        {
            ActionType = "bom_max";
            string[] str = { "@ActionType","@ActionRole", "@searchLotno","@input2" };
            string[] obj = { ActionType ,string.Empty,string.Empty, string.Empty };
            ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string max_bomcode = dt.Rows[0]["bomcode"].ToString(); ;
                if (max_bomcode == "")
                {
                    auto_generation_max_bomcode = 1;
                }
                else
                {
                    auto_generation_max_bomcode = Convert.ToInt32(dt.Rows[0]["bomcode"]);
                }
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
        private void FormBOM_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).partsCompositionToolStripMenuItem.Enabled = true;
        }
        private void FormBOM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnAdd.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btnDelete.PerformClick();
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
                btnbom_down.PerformClick();
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
            if (apply_change)
            {
                DialogResult dialogResult = MessageBox.Show("BOM Details Didn't Save, Do you want to Close This Form Means Lost the Data?", "CLOSE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ((Form1)MdiParent).partsCompositionToolStripMenuItem.Enabled = true;
                    apply_change = false;
                    this.Close();
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                ((Form1)MdiParent).partsCompositionToolStripMenuItem.Enabled = true;
                this.Close();                
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            FormSearchClient frm = new FormSearchClient();
            MysqlHelper.call_from_search_client = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ShowDialog();
        }

        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            FormSearchItem frm = new FormSearchItem();
            MysqlHelper.call_from_search_bom = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.CustomerCode = txtCustomerCode.Text;
            frm.CustomerNames = txtCustomerNameS.Text;
            frm.CustomerNameF = txtCustomerNameF.Text;
            frm.ShowDialog();
        }

        private void btnSearchMaterial_Click(object sender, EventArgs e)
        {
            FormSearchMaterial frm = new FormSearchMaterial();
            MysqlHelper.call_from_search_bom = true;
            MysqlHelper.call_from_search_client = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ShowDialog();
        }
        public void SetSearchId(string code, string shortname, string fullname)
        {
            if (code != string.Empty)
            {
                int formate_type = Convert.ToInt32(code);
                txtCustomerCode.Text = formate_type.ToString("D6");
            }       
            txtCustomerNameS.Text = shortname;
            txtCustomerNameF.Text = fullname;
            FetchBOMDetails(txtCustomerCode.Text, "");
            if (dGProcess_new.Rows.Count > 0)
            {
                BOM_view_process_started = true;
            }
            else
            {
                BOM_view_process_started = false;
            }
            textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();
            cmbProcess.SelectedIndex = -1;

        }
        public void SetSearchId_material(string code, string fullname)
        {
            textMaterialCode.Text = code;
            textMaterialName.Text = fullname;
        }
        public void SetSearchId_Item(string code, string fullname)
        {
            textItemCode.Text = code;
            textItemName.Text = fullname;
           // FetchBOMDetails_item(txtCustomerCode.Text, textItemCode.Text);
        }
        public void FetchBOMDetails(string customercode, string shortname)
        {
            dGProcess_new.Refresh();
            ds = helper.GetDatasetByBOMView(customercode, shortname);
            if (ds.Tables[0].Rows.Count > 0)
            {              
                dt = ds.Tables[0];
                DataTable dtIncremented = new DataTable(dt.TableName);
                DataColumn dc = new DataColumn("SNo.");
                dc.AutoIncrement = true;
                dc.AutoIncrementSeed = 1;
                dc.AutoIncrementStep = 1;
                dc.DataType = typeof(Int32);
                dtIncremented.Columns.Add(dc);
                dtIncremented.BeginLoadData();
                DataTableReader dtReader = new DataTableReader(dt);
                dtIncremented.Load(dtReader);
                dtIncremented.EndLoadData();
                dt = new DataTable();
                dt = dtIncremented;
                dGProcess_new.DataSource = null;
                dGProcess_new.AutoGenerateColumns = false;  
                idbom_view = Convert.ToInt16(dt.Rows[0]["idbom"]);
                bomcode_view = Convert.ToInt16(dt.Rows[0]["bomcode"]);
                txtCustomerNameF.Text = dt.Rows[0]["customer_fullnam"].ToString();
                txtCustomerNameS.Text = dt.Rows[0]["customer_shortname"].ToString();
                txtCustomerCode.Text = dt.Rows[0]["customercode"].ToString();
                //Set Columns Count
                //dGProcess_new.ColumnCount = 16; SNo.
                dGProcess_new.ColumnCount = 17;
                //Add Columns
                dGProcess_new.Columns[0].Name = "Sno";
                dGProcess_new.Columns[0].DataPropertyName = "SNo.";
                dGProcess_new.Columns[0].Width = 50;

                dGProcess_new.Columns[1].Name = "Sno";
                dGProcess_new.Columns[1].DataPropertyName = "sno";
                dGProcess_new.Columns[1].Width = 50;
                dGProcess_new.Columns[1].Visible = false;

                dGProcess_new.Columns[2].Name = "Customer Code";
                dGProcess_new.Columns[2].DataPropertyName = "customercode";
                dGProcess_new.Columns[2].Width = 150;

                dGProcess_new.Columns[3].Name = "Item Code";
                dGProcess_new.Columns[3].DataPropertyName = "itemcode";
                dGProcess_new.Columns[3].Width = 150;

                dGProcess_new.Columns[4].Name = "Item Name";
                dGProcess_new.Columns[4].DataPropertyName = "itemname";
                dGProcess_new.Columns[4].Width = 150;

                dGProcess_new.Columns[5].Name = "Process Order";
                dGProcess_new.Columns[5].DataPropertyName = "process_order";
                dGProcess_new.Columns[5].Width = 150;

                dGProcess_new.Columns[6].Name = "Process";
                dGProcess_new.Columns[6].DataPropertyName = "process";
                dGProcess_new.Columns[6].Width = 150;

                dGProcess_new.Columns[7].Name = "Material Code";
                dGProcess_new.Columns[7].DataPropertyName = "material_code";
                dGProcess_new.Columns[7].Width = 150;

                dGProcess_new.Columns[8].Name = "Material Name";
                dGProcess_new.Columns[8].DataPropertyName = "material_name";
                dGProcess_new.Columns[8].Width = 150;

                dGProcess_new.Columns[9].Name = "Customer Name (Full)";
                dGProcess_new.Columns[9].DataPropertyName = "customer_fullnam";
                dGProcess_new.Columns[9].Visible = false;

                dGProcess_new.Columns[10].Name = "Customer Name (Short)";
                dGProcess_new.Columns[10].DataPropertyName = "customer_shortname";
                dGProcess_new.Columns[10].Visible = false;

                dGProcess_new.Columns[11].Name = "edit_allow_flag";
                dGProcess_new.Columns[11].DataPropertyName = "edit_allow_flag";
                dGProcess_new.Columns[11].Visible = false;

                dGProcess_new.Columns[12].Name = "idbom";
                dGProcess_new.Columns[12].DataPropertyName = "idbom";
                dGProcess_new.Columns[12].Visible = false;

                dGProcess_new.Columns[13].Name = "bomcode";
                dGProcess_new.Columns[13].DataPropertyName = "bomcode";
                dGProcess_new.Columns[13].Visible = false; 

                dGProcess_new.Columns[14].Name = "inputscreentyp";
                dGProcess_new.Columns[14].DataPropertyName = "inputscreentyp";
                dGProcess_new.Columns[14].Visible = false;

                dGProcess_new.Columns[15].Name = "inputscreentyp_id";
                dGProcess_new.Columns[15].DataPropertyName = "inputscreentyp_id";
                dGProcess_new.Columns[15].Visible = false;

                dGProcess_new.Columns[16].Name = "processcode";
                dGProcess_new.Columns[16].DataPropertyName = "processcode";
                dGProcess_new.Columns[16].Visible = false;
                dGProcess_new.DataSource = dt;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess_new.DataSource = dt;
                dGProcess_new.DataSource = null;           
            }
        }
        public void FetchBOMDetails_item(string customercode, string shortname)
        {
            dGProcess_new.Refresh();
            ds = helper.GetDatasetByBOMView_Item(customercode, shortname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataTable dtIncremented = new DataTable(dt.TableName);
                DataColumn dc = new DataColumn("SNo.");
                dc.AutoIncrement = true;
                dc.AutoIncrementSeed = 1;
                dc.AutoIncrementStep = 1;
                dc.DataType = typeof(Int32);
                dtIncremented.Columns.Add(dc);
                dtIncremented.BeginLoadData();
                DataTableReader dtReader = new DataTableReader(dt);
                dtIncremented.Load(dtReader);
                dtIncremented.EndLoadData();
                dt = new DataTable();
                dt = dtIncremented;
                dGProcess_new.DataSource = null;
                dGProcess_new.AutoGenerateColumns = false;
                textItemName.Text = dt.Rows[0]["itemname"].ToString();
                idbom_view = Convert.ToInt16(dt.Rows[0]["idbom"]);
                bomcode_view = Convert.ToInt16(dt.Rows[0]["bomcode"]);
                txtCustomerNameF.Text = dt.Rows[0]["customer_fullnam"].ToString();
                txtCustomerNameS.Text = dt.Rows[0]["customer_shortname"].ToString();
                txtCustomerCode.Text = dt.Rows[0]["customercode"].ToString();
                //Set Columns Count
                //dGProcess_new.ColumnCount = 16;
                dGProcess_new.ColumnCount = 17;
                //Add Columns
                dGProcess_new.Columns[0].Name = "Sno";
                dGProcess_new.Columns[0].DataPropertyName = "SNo.";
                dGProcess_new.Columns[0].Width = 50;

                dGProcess_new.Columns[1].Name = "Sno";
                dGProcess_new.Columns[1].DataPropertyName = "sno";
                dGProcess_new.Columns[1].Visible = false;

                dGProcess_new.Columns[2].Name = "Customer Code";
                dGProcess_new.Columns[2].DataPropertyName = "customercode";
                dGProcess_new.Columns[2].Width = 150;

                dGProcess_new.Columns[3].Name = "Item Code";
                dGProcess_new.Columns[3].DataPropertyName = "itemcode";
                dGProcess_new.Columns[3].Width = 150;

                dGProcess_new.Columns[4].Name = "Item Name";
                dGProcess_new.Columns[4].DataPropertyName = "itemname";
                dGProcess_new.Columns[4].Width = 150;

                dGProcess_new.Columns[5].Name = "Process Order";
                dGProcess_new.Columns[5].DataPropertyName = "process_order";
                dGProcess_new.Columns[5].Width = 150;

                dGProcess_new.Columns[6].Name = "Process";
                dGProcess_new.Columns[6].DataPropertyName = "process";
                dGProcess_new.Columns[6].Width = 150;

                dGProcess_new.Columns[7].Name = "Material Code";
                dGProcess_new.Columns[7].DataPropertyName = "material_code";
                dGProcess_new.Columns[7].Width = 150;

                dGProcess_new.Columns[8].Name = "Material Name";
                dGProcess_new.Columns[8].DataPropertyName = "material_name";
                dGProcess_new.Columns[8].Width = 150;

                dGProcess_new.Columns[9].Name = "Customer Name (Full)";
                dGProcess_new.Columns[9].DataPropertyName = "customer_fullnam";
                dGProcess_new.Columns[9].Visible = false;

                dGProcess_new.Columns[10].Name = "Customer Name (Short)";
                dGProcess_new.Columns[10].DataPropertyName = "customer_shortname";
                dGProcess_new.Columns[10].Visible = false;

                dGProcess_new.Columns[11].Name = "edit_allow_flag";
                dGProcess_new.Columns[11].DataPropertyName = "edit_allow_flag";
                dGProcess_new.Columns[11].Visible = false;

                dGProcess_new.Columns[12].Name = "idbom";
                dGProcess_new.Columns[12].DataPropertyName = "idbom";
                dGProcess_new.Columns[12].Visible = false;

                dGProcess_new.Columns[13].Name = "bomcode";
                dGProcess_new.Columns[13].DataPropertyName = "bomcode";
                dGProcess_new.Columns[13].Visible = false;

                dGProcess_new.Columns[14].Name = "inputscreentyp";
                dGProcess_new.Columns[14].DataPropertyName = "inputscreentyp";
                dGProcess_new.Columns[14].Visible = true;

                dGProcess_new.Columns[15].Name = "inputscreentyp_id";
                dGProcess_new.Columns[15].DataPropertyName = "inputscreentyp_id";
                dGProcess_new.Columns[15].Visible = true;

                dGProcess_new.Columns[16].Name = "processcode";
                dGProcess_new.Columns[16].DataPropertyName = "processcode";
                dGProcess_new.Columns[16].Visible = true;

                dGProcess_new.DataSource = dt;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess_new.DataSource = dt;
                dGProcess_new.DataSource = null;           
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to ADD BOM ?", "CREATE BOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (CheckInput())
            {
                if (dialogResult == DialogResult.Yes)
                {
                    if (!already_exist())
                    {
                        DataRow dr;
                        if (BOM_view_process_started)
                        {
                            dr = dt.NewRow();
                            dr[0] = dGProcess_new.Rows.Count + 1;
                            dr[1] = dGProcess_new.Rows.Count + 1;
                            dr[2] = txtCustomerCode.Text;
                            dr[3] = textItemCode.Text;
                            dr[4] = textItemName.Text;
                            dr[5] = textOrder.Text;
                            dr[6] = cmbProcess.GetItemText(this.cmbProcess.SelectedItem);
                            dr[7] = textMaterialCode.Text;
                            dr[8] = textMaterialName.Text;
                            dr[9] = txtCustomerNameF.Text;
                            dr[10] = txtCustomerNameS.Text;
                            dr[11] = "edit_flag";
                            dr[12] = 0;
                            dr[13] = 0;
                            dr[14] = 0;
                            dr[15] = 0;
                            dr[16] = this.cmbProcess.SelectedValue;
                            dt.Rows.Add(dr);
                        }
                        else if (!BOM_view_process_started)
                        {
                            dr = dts.NewRow();
                            dr[0] = dGProcess_new.Rows.Count + 1;
                            dr[1] = dGProcess_new.Rows.Count + 1;
                            dr[2] = txtCustomerCode.Text;
                            dr[3] = textItemCode.Text;
                            dr[4] = textItemName.Text;
                            dr[5] = textOrder.Text;
                            dr[6] = cmbProcess.GetItemText(this.cmbProcess.SelectedItem);
                            dr[7] = textMaterialCode.Text;
                            dr[8] = textMaterialName.Text;
                            dr[9] = txtCustomerNameF.Text;
                            dr[10] = txtCustomerNameS.Text;
                            dr[11] = "edit_flag";
                            dr[12] = 0;
                            dr[13] = 0;
                            dr[14] = 0;
                            dr[15] = 0;
                            dr[16] = this.cmbProcess.SelectedValue;
                            dts.Rows.Add(dr);
                        }
                        squence_sno++;
                        Grid_sno++;
                        //  dGProcess_new.AutoGenerateColumns = false;
                        dGProcess_new.DataSource = null;
                        dGProcess_new.AutoGenerateColumns = false;
                        //Set Columns Count                 
                        dGProcess_new.ColumnCount = 17;
                        //Add Columns
                        dGProcess_new.Columns[0].Name = "Sno";
                        dGProcess_new.Columns[0].DataPropertyName = "sno";
                        dGProcess_new.Columns[0].Width = 50;

                        dGProcess_new.Columns[1].Name = "Sno";
                        dGProcess_new.Columns[1].DataPropertyName = "sno";
                        dGProcess_new.Columns[1].Visible = false;

                        dGProcess_new.Columns[2].Name = "Customer Code";
                        dGProcess_new.Columns[2].DataPropertyName = "customercode";
                        dGProcess_new.Columns[2].Width = 150;

                        dGProcess_new.Columns[3].Name = "Item Code";
                        dGProcess_new.Columns[3].DataPropertyName = "itemcode";
                        dGProcess_new.Columns[3].Width = 150;

                        dGProcess_new.Columns[4].Name = "Item Name";
                        dGProcess_new.Columns[4].DataPropertyName = "itemname";
                        dGProcess_new.Columns[4].Width = 150;

                        dGProcess_new.Columns[5].Name = "Process Order";
                        dGProcess_new.Columns[5].DataPropertyName = "process_order";
                        dGProcess_new.Columns[5].Width = 80;

                        dGProcess_new.Columns[6].Name = "Process";
                        dGProcess_new.Columns[6].DataPropertyName = "process";
                        dGProcess_new.Columns[6].Width = 150;

                        dGProcess_new.Columns[7].Name = "Material Code";
                        dGProcess_new.Columns[7].DataPropertyName = "material_code";
                        dGProcess_new.Columns[7].Width = 150;

                        dGProcess_new.Columns[8].Name = "Material Name";
                        dGProcess_new.Columns[8].DataPropertyName = "material_name";
                        dGProcess_new.Columns[8].Width = 150;

                        dGProcess_new.Columns[9].Name = "Customer Name (Full)";
                        dGProcess_new.Columns[9].DataPropertyName = "customer_fullnam";
                        dGProcess_new.Columns[9].Visible = false;

                        dGProcess_new.Columns[10].Name = "Customer Name (Short)";
                        dGProcess_new.Columns[10].DataPropertyName = "customer_shortname";
                        dGProcess_new.Columns[10].Visible = false;

                        dGProcess_new.Columns[11].Name = "edit_allow_flag";
                        dGProcess_new.Columns[11].DataPropertyName = "edit_allow_flag";
                        dGProcess_new.Columns[11].Visible = false;

                        dGProcess_new.Columns[12].Name = "idbom";
                        dGProcess_new.Columns[12].DataPropertyName = "idbom";
                        dGProcess_new.Columns[12].Visible = false;

                        dGProcess_new.Columns[13].Name = "bomcode";
                        dGProcess_new.Columns[13].DataPropertyName = "bomcode";
                        dGProcess_new.Columns[13].Visible = false;

                        dGProcess_new.Columns[14].Name = "inputscreentyp";
                        dGProcess_new.Columns[14].DataPropertyName = "inputscreentyp";
                        dGProcess_new.Columns[14].Visible = true;

                        dGProcess_new.Columns[15].Name = "inputscreentyp_id";
                        dGProcess_new.Columns[15].DataPropertyName = "inputscreentyp_id";
                        dGProcess_new.Columns[15].Visible = true;

                        dGProcess_new.Columns[16].Name = "processcode";
                        dGProcess_new.Columns[16].DataPropertyName = "processcode";
                        dGProcess_new.Columns[16].Visible = true;

                        if (BOM_view_process_started)
                        {
                            dGProcess_new.DataSource = dt;
                        }
                        else if (!BOM_view_process_started)
                        {
                            dGProcess_new.DataSource = dts;
                        }        
                        textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();
                        cmbProcess.SelectedIndex = -1;
                        textMaterialCode.Text = string.Empty;
                        textMaterialName.Text = string.Empty;
                        apply_change = true;
                    }
                }

            }

        }
        public bool already_exist()
        {
            bool result = true;
            //Selected item show in  list view
            int rowcount = dGProcess_new.RowCount;
            if (rowcount == 0)
            {
                result = false;
            }
            if (rowcount >= 1)
            {
                if (dGProcess_new != null)
                {
                    for (int i = 0; i < dGProcess_new.Rows.Count; i++)
                    {
                        string customercode = dGProcess_new.Rows[i].Cells[2].Value.ToString();
                        string itemcode = dGProcess_new.Rows[i].Cells[3].Value.ToString();
                        string process = dGProcess_new.Rows[i].Cells[6].Value.ToString();
                        string materialcode = dGProcess_new.Rows[i].Cells[7].Value.ToString();
                        string get_process = cmbProcess.GetItemText(this.cmbProcess.SelectedItem);
                        if (customercode == txtCustomerCode.Text && itemcode == textItemCode.Text && process == get_process && materialcode == textMaterialCode.Text)
                        {
                            MessageBox.Show("Customer Code & Item code & Process & Material code already Mapped..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cmbProcess.Focus();
                            result = true;
                            return result;
                        }
                        else
                        {
                            result = false;
                        }
                        newRow = (DataGridViewRow)dGProcess_new.Rows[i].Clone();
                    }
                }
            }
            return result;
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
            else if (textOrder.Text.Trim() == "")
            {
                MessageBox.Show("Process Order is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textOrder.Focus();
                result = false;
            }
            else if (cmbProcess.SelectedIndex == -1)
            {
                MessageBox.Show("Process is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProcess.Focus();
                result = false;
            }

            else if (textMaterialCode.Text.Trim() == "")
            {
                MessageBox.Show("Material code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textMaterialCode.Focus();
                result = false;
            }
            else if (textMaterialName.Text.Trim() == "")
            {
                MessageBox.Show("Material Name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textMaterialName.Focus();
                result = false;
            }
            return result;
        }
        public void ResetInput()
        {
            Grid_sno = 0;
            squence_sno = 0;
            dGProcess_new.DataSource = null;
            newRow = new DataGridViewRow();        
            txtCustomerNameF.Text = string.Empty;
            txtCustomerNameS.Text = string.Empty;
            textItemName.Text = string.Empty;
            textOrder.Text = string.Empty;
            cmbProcess.SelectedIndex = -1;
            textMaterialCode.Text = string.Empty;
            textMaterialName.Text = string.Empty;
            apply_change=false;
        }

        private void txtCustomerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_customercode())
                {
                    FetchBOMDetails(txtCustomerCode.Text, "");
                    if (dGProcess_new.Rows.Count > 0)
                    {
                        BOM_view_process_started = true;
                    }
                    else
                    {
                        BOM_view_process_started = false;
                    }
                    textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();

                }
            }
        }
        public bool CheckInput_customercode()
        {
            bool result = true;
            if (txtCustomerCode.Text == "000000" || txtCustomerNameS.Text == "")
            {
                MessageBox.Show("Customer Code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerCode.Focus();
                txtCustomerCode.Text = string.Empty;
                txtCustomerNameF.Text = string.Empty;
                txtCustomerNameS.Text = string.Empty;
                result = false;
            }
            else if (textItemCode.Text == "000000" || textItemCode.Text == "")
            {
                MessageBox.Show("Item Code is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textItemCode.Focus();             
                result = false;
            }
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(apply_change)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to Update BOM ?", "CREATE BOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        int grid_count = dGProcess_new.Rows.Count;
                        if (grid_count > 0)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            ActionType = "SaveData";
                            string[] str_exist = { "@cusmcd", "@itmcd", "@proces", "@matcd" };
                            string[] obj_exist = { txtCustomerCode.Text, textItemCode.Text, cmbProcess.GetItemText(this.cmbProcess.SelectedItem), textMaterialCode.Text };


                            MySqlDataReader already_exist = helper.GetReaderByCmd("bom_code_already_exist", str_exist, obj_exist);
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
                                string bomcode_gen = string.Empty;
                                for (int i = 0; i < dGProcess_new.Rows.Count; i++)
                                {
                                    // check bom id 
                                    string check_bomcode = dGProcess_new.Rows[i].Cells[13].Value.ToString();
                                    // other are already in table 
                                    if (check_bomcode == "0")
                                    {
                                        if (bomcode_view > 0)
                                        {
                                            check_bomcode = bomcode_view.ToString();
                                        }
                                        else if (bomcode_view == 0)
                                        {
                                            check_bomcode = auto_generation_max_bomcode.ToString();
                                        }


                                        string[] str = {
                                "@idbom",
                                "@bomcode",
                                "@customercd",
                                "@itcd",
                                "@itnam",
                                "@proc_ord",
                                "@proces",
                                "@materialcd",
                                "@materialname",
                                "@customerfname",
                                "@customersname",
                                "@process_id",
                                "@created_at",
                                "@updated_at",
                                "@ActionType"
                            };
                                        string[] obj = { "0",check_bomcode,
                                   dGProcess_new.Rows[i].Cells[2].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[3].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[4].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[5].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[6].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[7].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[8].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[9].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[10].Value.ToString(),
                                   dGProcess_new.Rows[i].Cells[16].Value.ToString(),
                                   nowdate.ToString(),
                                   string.Empty,
                                   ActionType
                                  };
                                        MySqlDataReader sdrs = helper.GetReaderByCmd("bom_ins", str, obj);
                                        if (sdrs.Read())
                                        {
                                            sdrs.Close();
                                            helper.CloseConnection();
                                        }
                                        else
                                        {
                                            ResetInput();
                                            sdrs.Close();
                                            helper.CloseConnection();

                                        }
                                    }
                                }
                                if (dGProcess_new.Rows.Count > 0)
                                {
                                    ResetInput();
                                    bomcode_view = 0;
                                    max_user_id();
                                    dt = new DataTable();
                                    dts = new DataTable();
                                    // dts_table_columns();
                                    MessageBox.Show("BOM Insert Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    apply_change = false;
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("BOM Grid is null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No changes detected.Add any new BOM Details.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                DialogResult dialogResult = MessageBox.Show("Do you want to Delete the BOM ?", "DELETE BOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(dialogResult == DialogResult.Yes)
                {
                    if (bomcode_view != 0 && bom_tbl_pk !=0 && selected_processid !="0")
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if(!check_delete_allow())
                        {
                            ActionType = "DeleteData";
                            string[] str = { "@bomcd", "@bompk", "@ActionType" };
                            string[] obj = { bomcode_view.ToString(), bom_tbl_pk.ToString(), ActionType };

                            MySqlDataReader sdr = helper.GetReaderByCmd("bom_delete", str, obj);
                            if (sdr.Read())
                            {
                                sdr.Close();
                                helper.CloseConnection();
                                ResetInput();
                                FetchBOMDetails(txtCustomerCode.Text, string.Empty);
                                textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();
                                bomcode_view = 0;
                                btnAdd.Enabled = true;
                                btnSave.Enabled = true;
                                MessageBox.Show("BOM Deleted Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {
                                sdr.Close();
                                helper.CloseConnection();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not allow to delete, Already mapped into lotinformation", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Select atleast anyone in List", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool check_delete_allow()
        {
            bool result = false;
            string[] str_exist = { "@custcd", "@itemcd", "@mat_cd", "@pro_id", "@ActionType" };
            string[] obj_exist = { txtCustomerCode.Text,textItemCode.Text,textMaterialCode.Text, selected_processid, "GetData" };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_delete_allow_bom", str_exist, obj_exist);
            if (already_exist.Read())
            {
                // bom table check already exits 
                string pk_bom_id = already_exist["idproduction_input_master"].ToString();
                result = true;
            }
            already_exist.Close();
            helper.CloseConnection();
            return result;
        }
        private void dGProcess_new_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGProcess_new.Rows[rowIndex];
            txtCustomerCode.Text = row.Cells[2].Value.ToString();
            txtCustomerNameS.Text = row.Cells[10].Value.ToString();
            txtCustomerNameF.Text = row.Cells[9].Value.ToString();
            textItemCode.Text = row.Cells[3].Value.ToString();
            textItemName.Text = row.Cells[4].Value.ToString();
            textOrder.Text = row.Cells[5].Value.ToString();
            //cmbProcess.SelectedIndex = cmbProcess.Items.IndexOf(row.Cells[5].Value.ToString());
            cmbProcess.Text = row.Cells[6].Value.ToString();
            textMaterialCode.Text = row.Cells[7].Value.ToString();
            textMaterialName.Text = row.Cells[8].Value.ToString();
            bomcode_view = Convert.ToInt16(row.Cells[13].Value);
            bom_tbl_pk = Convert.ToInt16(row.Cells[12].Value);
            selected_processid = row.Cells[16].Value.ToString();
            btnSave.Enabled = false;
            btnAdd.Enabled = false;
        }
        private void txtCustomerNameS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_customercode())
                {
                    FetchBOMDetails(string.Empty, txtCustomerNameS.Text);
                    if (dGProcess_new.Rows.Count > 0)
                    {
                        BOM_view_process_started = true;
                    }
                    else
                    {
                        BOM_view_process_started = false;
                    }
                    textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();

                }
            }
        }

        private void textOrder_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
            if (textOrder.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(textOrder.Text);
                textOrder.Text = formate_type.ToString("D2");
            }
        }

        private void textItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_customercode())
                {
                    FetchBOMDetails_item(txtCustomerCode.Text, textItemCode.Text);
                    if (dGProcess_new.Rows.Count > 0)
                    {
                        BOM_view_process_started = true;
                    }
                    else
                    {
                        BOM_view_process_started = false;
                    }
                    textOrder.Text = (dGProcess_new.Rows.Count + 1).ToString();
                }
                else
                {
                    MessageBox.Show("Customer Code or Item code is null", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerCode.Focus();
                }
            }
        }
        public void Customer_itemcode_details(string ActionType, string customercode)
        {
            try
            {
                string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
                string[] obj = { customercode, string.Empty, textItemCode.Text, ActionType };
                ds = helper.GetDatasetByCommandString("product_view", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    DataTable dtIncremented = new DataTable(dt.TableName);
                    DataColumn dc = new DataColumn("SNo.");
                    dc.AutoIncrement = true;
                    dc.AutoIncrementSeed = 1;
                    dc.AutoIncrementStep = 1;
                    dc.DataType = typeof(Int32);
                    dtIncremented.Columns.Add(dc);
                    dtIncremented.BeginLoadData();
                    DataTableReader dtReader = new DataTableReader(dt);
                    dtIncremented.Load(dtReader);
                    dtIncremented.EndLoadData();
                    dt = new DataTable();
                    dt = dtIncremented;
                    dGProcess_new.DataSource = null;
                    dGProcess_new.AutoGenerateColumns = false;
                    idbom_view = Convert.ToInt16(dt.Rows[0]["idbom"]);
                    bomcode_view = Convert.ToInt16(dt.Rows[0]["bomcode"]);
                    txtCustomerNameF.Text = dt.Rows[0]["customer_fullnam"].ToString();
                    txtCustomerNameS.Text = dt.Rows[0]["customer_shortname"].ToString();
                    txtCustomerCode.Text = dt.Rows[0]["customercode"].ToString();
                    //Set Columns Count                 
                    dGProcess_new.ColumnCount = 14;
                    //Add Columns
                    dGProcess_new.Columns[0].Name = "Sno";
                    dGProcess_new.Columns[0].DataPropertyName = "SNo.";
                    dGProcess_new.Columns[0].Width = 50;

                    dGProcess_new.Columns[1].Name = "Sno";
                    dGProcess_new.Columns[1].DataPropertyName = "sno";
                    dGProcess_new.Columns[1].Width = 50;

                    dGProcess_new.Columns[2].Name = "Customer Code";
                    dGProcess_new.Columns[2].DataPropertyName = "customercode";
                    dGProcess_new.Columns[2].Width = 150;

                    dGProcess_new.Columns[3].Name = "Item Code";
                    dGProcess_new.Columns[3].DataPropertyName = "itemcode";
                    dGProcess_new.Columns[3].Width = 150;

                    dGProcess_new.Columns[4].Name = "Item Name";
                    dGProcess_new.Columns[4].DataPropertyName = "itemname";
                    dGProcess_new.Columns[4].Width = 150;

                    dGProcess_new.Columns[5].Name = "Process Order";
                    dGProcess_new.Columns[5].DataPropertyName = "process_order";
                    dGProcess_new.Columns[5].Width = 150;

                    dGProcess_new.Columns[6].Name = "Process";
                    dGProcess_new.Columns[6].DataPropertyName = "process";
                    dGProcess_new.Columns[6].Width = 150;

                    dGProcess_new.Columns[7].Name = "Material Code";
                    dGProcess_new.Columns[7].DataPropertyName = "material_code";
                    dGProcess_new.Columns[7].Width = 150;

                    dGProcess_new.Columns[8].Name = "Material Name";
                    dGProcess_new.Columns[8].DataPropertyName = "material_name";
                    dGProcess_new.Columns[8].Width = 150;

                    dGProcess_new.Columns[9].Name = "Customer Name (Full)";
                    dGProcess_new.Columns[9].DataPropertyName = "customer_fullnam";
                    dGProcess_new.Columns[9].Visible = false;

                    dGProcess_new.Columns[10].Name = "Customer Name (Short)";
                    dGProcess_new.Columns[10].DataPropertyName = "customer_shortname";
                    dGProcess_new.Columns[10].Visible = false;

                    dGProcess_new.Columns[11].Name = "edit_allow_flag";
                    dGProcess_new.Columns[11].DataPropertyName = "edit_allow_flag";
                    dGProcess_new.Columns[11].Visible = false;

                    dGProcess_new.Columns[12].Name = "idbom";
                    dGProcess_new.Columns[12].DataPropertyName = "idbom";
                    dGProcess_new.Columns[12].Visible = false;

                    dGProcess_new.Columns[13].Name = "bomcode";
                    dGProcess_new.Columns[13].DataPropertyName = "bomcode";
                    dGProcess_new.Columns[13].Visible = false;

                    dGProcess_new.DataSource = dt;

                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess_new.DataSource = dt;             
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            ResetInput();
        }

        private void btnbom_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download BOM List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (dGProcess_new.Rows.Count > 0)
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
                        for (int i = 1; i < dGProcess_new.Columns.Count - 1; i++)
                        {
                            if (Date_column_names.Contains(dGProcess_new.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[1, i] = dGProcess_new.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dGProcess_new.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[1, i] = dGProcess_new.Columns[i - 1].HeaderText;
                                Date_column_index.Add(get_date_column);
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dGProcess_new.Rows.Count; i++)
                        {
                            for (int j = 0; j < dGProcess_new.Columns.Count - 2; j++)
                            {
                                if (Convert.ToString(dGProcess_new.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check customer code column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGProcess_new.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        int formate_type = Convert.ToInt32(dGProcess_new.Rows[i].Cells[j].Value.ToString());
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
                        Excel.Range copyRange_I = XcelApp.Range["J:J"];
                        Excel.Range copyRange_J = XcelApp.Range["K:K"]; 
                        Excel.Range insertRange_C = XcelApp.Range["D:D"];                   

                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_I.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_J.Cut());

                        Excel.Range DeleteRange_B = XcelApp.Range["B:B"];
                        Excel.Range DeleteRange_K = XcelApp.Range["K:K"];
                        Excel.Range DeleteRange_L = XcelApp.Range["L:L"];
                        Excel.Range DeleteRange_M = XcelApp.Range["M:M"];
                        Excel.Range DeleteRange_N = XcelApp.Range["N:N"];                     
                        DeleteRange_K.Delete();
                        DeleteRange_L.Delete();
                        DeleteRange_M.Delete();
                        DeleteRange_N.Delete();
                        DeleteRange_B.Delete();
                        //Auto fit automatically adjust the width of columns of Excel  in givien range .  

                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess_new.Rows.Count, dGProcess_new.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess_new.Columns.Count]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGProcess_new.Columns.Count]].Font.Size = 13;

                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                  
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\BOM List -" + datetime;
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
