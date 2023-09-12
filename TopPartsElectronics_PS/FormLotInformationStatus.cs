using MySql.Data.MySqlClient;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopPartsElectronics_PS.Helper;
using YourApp.Data;
using static TopPartsElectronics_PS.Helper.GeneralModelClass;
using Excel = Microsoft.Office.Interop.Excel;
namespace TopPartsElectronics_PS
{
    public partial class FormLotInformationStatus : Form
    {
        MysqlHelper helper = new MysqlHelper();
        List<KeyValuePair<int, string>> kvpList_Latest = new List<KeyValuePair<int, string>>();
        List<list_of_lotnumbers> store_selected_lotno = new List<list_of_lotnumbers>();
        List<list_of_lotnumbers> store_selected_lotno_few = new List<list_of_lotnumbers>();
        int PageNumber = 1;
        int PageSize = 8;
        List<Lotinfo_gridbind_common_pattern> lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private int[] daysInMonths;
        private string[] GroupLabel;
        private string[,] LabelString;
        private int[,] LabelSize;
        List<string> already_exits_row_header = new List<string>();
        List<string> already_exits_row_columns = new List<string>();
        int NoOfQty = 0;
        int checkall_NoOfQty = 0;   
        public FormLotInformationStatus()
        {
            InitializeComponent();
        }

        private void FormLotInformationStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).lotinfostatusStripMenuItem.Enabled = true;
        }

        private void FormLotInformationStatus_Load(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 4;

            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("FormLotInformationStatus_Load", ex);
            }
        }
        public void checked_filter()
        {
            try
            {
                if (checkInput_chkbx_Customer_item_manfdt())
                {
                    lot_number_get("lotinfo_status_wit_manfdt", "CustItmMfdt", txtCustomerCode.Text, textItemCode.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                }
                else if (checkInput_chkbx_check_Customer_only())
                {
                    lot_number_get("lotinfo_status", "CustOnly", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                else if (checkInput_chkbx_check_Customer_item())
                {
                    lot_number_get("lotinfo_status", "CustItem", txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, string.Empty);
                }
                else if (checkInput_chkbx_manfdt_only())
                {
                    lot_number_get("lotinfo_status_wit_manfdt", "MfdtOnly", string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                }
                else if (checkInput_chkbx_manfdt_cust())
                {
                    lot_number_get("lotinfo_status_wit_manfdt", "CustMfdt", txtCustomerCode.Text, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),string.Empty);
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("checked_filter", ex);
            }
        }
        public void lot_number_get(string SpName,string Actiontyp,string Custcd,string Itemcd,string ManfdtFrm,string ManfdtTo,string lotnumber)
        {
            try
            {
                store_selected_lotno = new List<list_of_lotnumbers>();
                string[] str = { "@ActionType", "@CustomerCd", "@ItemCd", "@ManfDtFrm", "@ManfDtTo", "@lotnumber" };
                string[] obj = { Actiontyp, Custcd, Itemcd,ManfdtFrm,ManfdtTo,lotnumber };
                MySqlDataReader sdr = helper.GetReaderByCmd(SpName, str, obj);
                int row_index = 0;
                kvpList_Latest = new List<KeyValuePair<int, string>>();           
                while (sdr.Read())// using read() method to read all rows one-by-one
                {
                    list_of_lotnumbers model = new list_of_lotnumbers(); 
                    // temp lot numbers store
                
                    model.lotno = sdr["lotno"].ToString(); 
                    model.customercode = sdr["customercode"].ToString();
                    model.item_code = sdr["item_code"].ToString();
                    string manf_dt_str = sdr["manuf_date"].ToString();
                    string manf_dt_lotonly_str = sdr["manfdt_only_lot"].ToString();
                    if (!string.IsNullOrWhiteSpace(manf_dt_str))
                    {
                        model.manf_dt = Convert.ToDateTime(sdr["manuf_date"]);
                    }
                    if (!string.IsNullOrWhiteSpace(manf_dt_lotonly_str))
                    {
                        model.manf_dt_lotonly = Convert.ToDateTime(sdr["manfdt_only_lot"]);
                    }
        
                    string current_value = model.customercode + "," + model.item_code;                  
                    kvpList_Latest.Add(new KeyValuePair<int, string>(Convert.ToInt32(sdr["lotno"]), current_value));
                    store_selected_lotno.Add(model);
                    row_index++;
                }
                sdr.Close();
                helper.CloseConnection();
                checkedListBox_lotno.DataSource = null;
                checkedListBox_lotno.DataSource = new BindingSource(kvpList_Latest, null);
                checkedListBox_lotno.DisplayMember = "Key";
                checkedListBox_lotno.ValueMember = "Value";
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("lot_number_get", ex);
            }
        }
        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormSearchClient frm = new FormSearchClient();
            chk_customer.Checked = false;
            chk_item.Checked = false;
            chk_manf_dt_frm_to.Checked = false;
            txtCustomerCode.Text = string.Empty;
            txtCustomerNameF.Text = string.Empty;
            textItemCode.Text = string.Empty;
            txt_itemname.Text = string.Empty;
            checkedListBox_lotno.DataSource = null;
            MysqlHelper.call_from_lotinfomation_status_to_client = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
        public void SetSearchId_customer(string code, string shortname, string fullname)
        {
            txtCustomerCode.Text = code;         
            txtCustomerNameF.Text = fullname;
            textItemCode.Text = string.Empty;
            txt_itemname.Text = string.Empty;
            chk_customer.Checked = true;
            chk_item.Checked = false;
        }
        public void SetSearchId_Item_lotinfo_sts(string customercode, string itemcode, string fullname)
        {
            textItemCode.Text = itemcode;
            txt_itemname.Text = fullname;
            chk_item.Checked = true;
        }
        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormSearchItem frm = new FormSearchItem();
            chk_item.Checked = false;
            MysqlHelper.call_from_lotinfomation_status_to_item = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.CustomerCode = txtCustomerCode.Text;    
            frm.CustomerNameF = txtCustomerNameF.Text;
            frm.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
        // check all
        // Customer & item & manf dt
        public bool checkInput_chkbx_Customer_item_manfdt()
        {
            bool result = false;
            if (chk_customer.Checked && chk_item.Checked && chk_manf_dt_frm_to.Checked)
            {
                result = true;
            }
            return result;
        }
        // Customer only
        public bool checkInput_chkbx_check_Customer_only()
        {
            bool result = false;
            if (chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked)
            {
                result = true;
            }
            return result;
        }
        // Customer & item
        public bool checkInput_chkbx_check_Customer_item()
        {
            bool result = false;
            if (chk_customer.Checked && chk_item.Checked && !chk_manf_dt_frm_to.Checked)
            {
                result = true;
            }
            return result;
        }
        
        // manf dt only
        public bool checkInput_chkbx_manfdt_only()
        {
            bool result = false;
            if (!chk_customer.Checked && !chk_item.Checked && chk_manf_dt_frm_to.Checked)
            {
                result = true;
            }
            return result;
        }
        // manf dt & cust
        public bool checkInput_chkbx_manfdt_cust()
        {
            bool result = false;
            if (chk_customer.Checked && !chk_item.Checked && chk_manf_dt_frm_to.Checked)
            {
                result = true;
            }
            return result;
        }

        private void chk_customer_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_customer.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (txtCustomerCode.Text == "000000" || txtCustomerCode.Text == string.Empty)
                {
                    chk_customer.Checked = false;
                    MessageBox.Show("Must Choose Customer Code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSearchCustomer.Focus();
                    return;
                }
                else if (txtCustomerNameF.Text == string.Empty)
                {
                    chk_customer.Checked = false;
                    MessageBox.Show("Must Choose Customer Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSearchCustomer.Focus();
                    return;
                }        
                checkedListBox_lotno.DataSource = null;
                checked_filter();
                Cursor.Current = Cursors.Default;
            }
            else if(!chk_customer.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                chk_item.Checked = false;
                chk_manf_dt_frm_to.Checked = false;
                txtCustomerCode.Text = string.Empty;
                txtCustomerNameF.Text = string.Empty;
                textItemCode.Text = string.Empty;
                txt_itemname.Text = string.Empty;
                checkedListBox_lotno.DataSource = null;             
                Cursor.Current = Cursors.Default;
            }
            
        }

        private void chk_item_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_customer.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chk_item.Checked)
                {
                    if (textItemCode.Text == "000000" || textItemCode.Text == string.Empty)
                    {
                        chk_item.Checked = false;
                        MessageBox.Show("Must Choose Item Code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnSearchItem.Focus();
                        return;
                    }
                    else if (txt_itemname.Text == string.Empty)
                    {
                        chk_item.Checked = false;
                        MessageBox.Show("Must Choose Item Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnSearchItem.Focus();
                        return;
                    }              
                    checkedListBox_lotno.DataSource = null;
                    checked_filter();
                    Cursor.Current = Cursors.Default;
                }
                else if (!chk_item.Checked)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    textItemCode.Text = string.Empty;
                    txt_itemname.Text = string.Empty;
                    checkedListBox_lotno.DataSource = null;
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("Must Select Customer....", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chk_customer.Focus();
            }
         
        }

        private void chk_manf_dt_frm_to_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_manf_dt_frm_to.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                checkedListBox_lotno.DataSource = null;
                checked_filter();
                Cursor.Current = Cursors.Default;
            }
        }
       
        private void ChangeAllCheckBoxValues(bool value)
        {
            for (int i = 0; i < checkedListBox_lotno.Items.Count; i++)
            {
                checkedListBox_lotno.SetItemChecked(i, value);
            }
        }

        private void txt_selected_lotno_Leave(object sender, EventArgs e)
        {
            try
            {               
             
                if (checkInput_chkbx_Customer_item_manfdt())
                {
                    lot_number_get("lotinfo_status_wit_manfdt", "CustItmMfdtLotno", txtCustomerCode.Text, textItemCode.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),txt_selected_lotno.Text);
                }
                else if (checkInput_chkbx_check_Customer_only())
                {
                    lot_number_get("lotinfo_status", "CustOnlyLotno", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, txt_selected_lotno.Text);
                }
                else if (checkInput_chkbx_check_Customer_item())
                {
                    lot_number_get("lotinfo_status", "CustItemLotno", txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, txt_selected_lotno.Text);
                }
                else if (checkInput_chkbx_manfdt_only())
                {
                    lot_number_get("lotinfo_status_wit_manfdt", "MfdtOnlyLotno", string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txt_selected_lotno.Text + "%");
                }
                else if (checkInput_chkbx_manfdt_cust())
                {
                    lot_number_get("lotinfo_status_wit_manfdt", "CustMfdtLotno", txtCustomerCode.Text, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txt_selected_lotno.Text + "%");
                }
                
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("txt_selected_lotno_KeyPress", ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if(CheckInput())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    CommonClass.Process_name_gridbind_columns_lotinfostatus = new List<PI_Process>();
                    // Grid header load and use process names static                   
                    dataGridView1.Columns.Clear();
                    terminal_addlist_loadgrid_call("shipment_others");
                    store_selected_lotno_few = new List<list_of_lotnumbers>();
                    if (chk_selectall.Checked && chk_selectall.Text== "Select All")
                    {
                        Common_Selected_Lotno_all();
                    }
                    else
                    {
                        Common_Selected_Lotno_few();
                    }                    
                    dataGridView1.Sort(dataGridView1.Columns[13], ListSortDirection.Descending);
                    dataGridView1.RefreshEdit();
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[9].Visible = false;
                    dataGridView1.Columns[11].Visible = false;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                   
                        MessageBox.Show("Atleast one Checked of this check-box or Lot number....", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chk_customer.Focus();
              
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnSearch_Click",ex);
            }
        }
        public bool CheckInput()
        {
            bool result = true;
            if (!chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked)
            {                
                result = false;
            }
            else
            {
                result = false;
                foreach (var obj in checkedListBox_lotno.CheckedItems)
                {
                    result = true;                 
                    break;
                }
            }           
            return result;
        }
        
        private void chk_selectall_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_selectall.CheckState == CheckState.Checked)
            {
                ChangeAllCheckBoxValues(true);
                chk_selectall.Text = "Un-Select All";
            }
            else
            {
                ChangeAllCheckBoxValues(false);
                chk_selectall.Text = "Select All";
            }
        }
        public void insert_lotinfo_value_assign_gridbind(string ActionTypeTwo, string lotn, string lotn_frm, string lotn_to, string manf_dt_frm, string manf_dt_to, string customer_code, string item_code, string auctionrole, string sp_name, string common_cust_name, string common_item_name)
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();

                // lot information grid data's
                // p1     
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p1 = { ActionType_p1, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };
                MySqlDataReader ds_pattern1 = helper.GetReaderByCmd(sp_name, str_p1, obj_p1);
                List<Lotinfo_gridbind_common_pattern_new_ship> m_model_p1 = LocalReportExtensions.GetList<Lotinfo_gridbind_common_pattern_new_ship>(ds_pattern1);

                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.processName = "TERMINAL BOARD INFO";
                c_model.pattern_type = "5";
                list_cmodel.Add(c_model);
                if (m_model_p1.Count > 0)
                {

                    m_model_p1.ForEach(dr =>
                    {

                        string lotno_split = dr.lotnojoin_p1.ToString();
                        string[] lotnumbers = lotno_split.Split(',');

                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();

                            // lot no format change                        
                            string dG1joinlotno = lot;
                            string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                            string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                            int convert_lotno = Convert.ToInt32(lotno_spl);
                            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                            string lotno_format = convert_lotno.ToString("D7");
                            string lotnochld_format = convert_lotnochld.ToString("D2");

                            if (lotno_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotno_format);
                                lotno_format = formate_type.ToString("D7");
                            }
                            if (lotnochld_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotnochld_format);
                                lotnochld_format = formate_type.ToString("D2");
                            }
                            dG1joinlotno = lotno_format + "-" + lotnochld_format;
                            c_model.lotno = lotno_format;
                            c_model.lotnojoin = dG1joinlotno;

                            c_model.processId = dr.processId_p1.ToString();
                            c_model.processName = dr.processName_p1.ToString();
                            c_model.partno = dr.partno_p1.ToString();
                            c_model.qty = dr.quantity_p1.ToString();
                            c_model.plantingdate = dr.planting_p1.ToString();
                            c_model.pb_date = dr.pb_dt_p1.ToString();
                            c_model.tb_manuf_dt = dr.tb_manuf_dt_p1.ToString();
                            c_model.tb_expairy_dt = dr.tb_expairy_dt_p1.ToString();
                            c_model.tb_qty = dr.tb_qty_p1.ToString();
                            c_model.lotno_p1 = dr.lotno_p1.ToString();
                            c_model.material_code = dr.materialcd.ToString();

                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;

                            c_model.tb_bproduct = dr.bproduct_p1.ToString();
                            c_model.onhold = dr.onhold_p1.ToString();
                            c_model.scrap = dr.scrap_p1.ToString();

                            c_model.reason_hs = dr.reason_hs_p1.ToString();
                            list_cmodel.Add(c_model);
                        });

                    });

                }
                helper.CloseConnection();
                string ActionType_p2 = "p2view";
                string[] str_p2 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p2 = { ActionType_p2, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };

                MySqlDataReader ds_pattern2 = helper.GetReaderByCmd(sp_name, str_p2, obj_p2);
                List<Lotinfo_gridbind_p2_ship> m_model_p2 = LocalReportExtensions.GetList<Lotinfo_gridbind_p2_ship>(ds_pattern2);
                if (m_model_p2.Count > 0)
                {

                    m_model_p2.ForEach(dr =>
                    {

                        string lotno_split = dr.lotnojoin_p2.ToString();
                        string[] lotnumbers = lotno_split.Split(',');

                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();

                            c_model.pattern_type = dr.pattern_type.ToString();
                            // lot no format change                        
                            string dG1joinlotno = lot;
                            string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                            string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                            int convert_lotno = Convert.ToInt32(lotno_spl);
                            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                            string lotno_format = convert_lotno.ToString("D7");
                            string lotnochld_format = convert_lotnochld.ToString("D2");

                            if (lotno_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotno_format);
                                lotno_format = formate_type.ToString("D7");
                            }
                            if (lotnochld_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotnochld_format);
                                lotnochld_format = formate_type.ToString("D2");
                            }
                            dG1joinlotno = lotno_format + "-" + lotnochld_format;
                            c_model.lotno = lotno_format;
                            c_model.lotnojoin = dG1joinlotno;
                            c_model.processId = dr.processId_p2.ToString();
                            c_model.processName = dr.processName_p2.ToString();
                            c_model.plantingdate = dr.process_date_p2.ToString();
                            c_model.partno = dr.contorlno_p2.ToString();
                            c_model.lotno = dr.slot_no_p2.ToString();
                            c_model.qty = dr.quantity_p2.ToString();
                            c_model.tb_manuf_dt = dr.tb_manuf_dt_p2.ToString();
                            c_model.tb_expairy_dt = dr.tb_expairy_dt_p2.ToString();
                            c_model.tb_qty = dr.tb_qty_p2.ToString();
                            c_model.sheetlotno_p2 = dr.sheet_lotno_p2.ToString();
                            c_model.material_code = dr.materialcd.ToString();
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            c_model.tb_bproduct = dr.bproduct_p2.ToString();
                            c_model.onhold = dr.onhold_p2.ToString();
                            c_model.scrap = dr.scrap_p2.ToString();
                            c_model.reason_hs = dr.reason_hs_p2.ToString();
                            list_cmodel.Add(c_model);
                        });

                    });

                }
                helper.CloseConnection();
                string ActionType_p3 = "p3view";
                string[] str_p3 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p3 = { ActionType_p3, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };

                MySqlDataReader ds_pattern3 = helper.GetReaderByCmd(sp_name, str_p3, obj_p3);
                List<Lotinfo_gridbind_p3_ship> m_model_p3 = LocalReportExtensions.GetList<Lotinfo_gridbind_p3_ship>(ds_pattern3);
                if (m_model_p3.Count > 0)
                {
                    m_model_p3.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p3.ToString();
                        string[] lotnumbers = lotno_split.Split(',');

                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            // lot no format change                        
                            string dG1joinlotno = lot;
                            string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                            string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                            int convert_lotno = Convert.ToInt32(lotno_spl);
                            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                            string lotno_format = convert_lotno.ToString("D7");
                            string lotnochld_format = convert_lotnochld.ToString("D2");

                            if (lotno_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotno_format);
                                lotno_format = formate_type.ToString("D7");
                            }
                            if (lotnochld_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotnochld_format);
                                lotnochld_format = formate_type.ToString("D2");
                            }
                            dG1joinlotno = lotno_format + "-" + lotnochld_format;
                            c_model.lotno = lotno_format;
                            c_model.lotnojoin = dG1joinlotno;
                            c_model.processId = dr.processId_p3.ToString();
                            c_model.processName = dr.processName_p3.ToString();
                            c_model.plantingdate = dr.process_date_p3.ToString();
                            c_model.qty = dr.quantity_p3.ToString();
                            c_model.tb_manuf_dt = dr.tb_manuf_dt_p3.ToString();
                            c_model.tb_expairy_dt = dr.tb_expairy_dt_p3.ToString();
                            c_model.tb_qty = dr.tb_qty_p3.ToString();
                            c_model.material_code = dr.materialcd.ToString();
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            c_model.tb_bproduct = dr.bproduct_p3.ToString();
                            c_model.onhold = dr.onhold_p3.ToString();
                            c_model.scrap = dr.scrap_p3.ToString();
                            c_model.reason_hs = dr.reason_hs_p3.ToString();
                            list_cmodel.Add(c_model);
                        });


                    });

                }
                helper.CloseConnection();
                string ActionType_p4 = "p4view";
                string[] str_p4 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p4 = { ActionType_p4, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };


                MySqlDataReader ds_pattern4 = helper.GetReaderByCmd(sp_name, str_p4, obj_p4);
                List<Lotinfo_gridbind_p4_ship> m_model_p4 = LocalReportExtensions.GetList<Lotinfo_gridbind_p4_ship>(ds_pattern4);
                if (m_model_p4.Count > 0)
                {

                    m_model_p4.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p4.ToString();
                        string[] lotnumbers = lotno_split.Split(',');

                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();

                            // lot no format change                        
                            string dG1joinlotno = lot;
                            string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                            string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                            int convert_lotno = Convert.ToInt32(lotno_spl);
                            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                            string lotno_format = convert_lotno.ToString("D7");
                            string lotnochld_format = convert_lotnochld.ToString("D2");

                            if (lotno_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotno_format);
                                lotno_format = formate_type.ToString("D7");
                            }
                            if (lotnochld_format != string.Empty)
                            {
                                int formate_type = Convert.ToInt32(lotnochld_format);
                                lotnochld_format = formate_type.ToString("D2");
                            }
                            dG1joinlotno = lotno_format + "-" + lotnochld_format;
                            c_model.lotno = lotno_format;
                            c_model.lotnojoin = dG1joinlotno;


                            c_model.processId = dr.processId_p4.ToString();
                            c_model.processName = dr.processName_p4.ToString();
                            c_model.partno = dr.partno_p4.ToString();
                            c_model.qty = dr.quantity_p4.ToString();
                            c_model.tb_manuf_dt = dr.tb_manuf_dt_p4.ToString();
                            c_model.tb_expairy_dt = dr.tb_expairy_dt_p4.ToString();
                            c_model.tb_qty = dr.tb_qty_p4.ToString();
                            c_model.lotno_p4 = dr.lotno_p4.ToString();
                            c_model.material_code = dr.materialcd.ToString();
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            c_model.tb_bproduct = dr.bproduct_p4.ToString();
                            c_model.onhold = dr.onhold_p4.ToString();
                            c_model.scrap = dr.scrap_p4.ToString();
                            c_model.reason_hs = dr.reason_hs_p4.ToString();
                            list_cmodel.Add(c_model);
                        });

                    });

                }
                helper.CloseConnection();
                list_cmodel = list_cmodel.OrderBy(o => o.lotnojoin).ToList();
                // shipment date check after 2month means not show

                if (dataGridView1.Rows.Count >= 0 && list_cmodel.Count > 1)
                {                    
                        int header_lot_index = 0;


                        var get_lotnumber = (from num in list_cmodel
                                             select num.lotnojoin).Distinct();
                        string gets = get_lotnumber.Count().ToString();

                        ///9022022
                        ///grid row header bind                                             

                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                        //foreach (var rowheader in row_header_lotno_all_combined)
                        foreach (var rowheader in get_lotnumber)
                        {
                            if (header_lot_index > 0 && !already_exits_row_header.Contains(rowheader))
                            {
                                DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                //// add  lot number  
                                First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                First_row_dynamic_colm.HeaderCell.Value = rowheader;
                                this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                already_exits_row_header.Add(rowheader);
                            }
                            header_lot_index++;                           
                        }
                        this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dataGridView1.EnableHeadersVisualStyles = false;
                        dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;
                        dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    
                   

                }
                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);
                if (list_cmodel.Count > 1)
                {

                    foreach (var item in list_cmodel)
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = item.processName.Split(',');

                            int chk_index = 0;

                            if (!already_exits_row_columns.Contains(item.lotnojoin))
                            {
                                foreach (var chk in split_process_name)
                                {
                                    string patern_type = item.pattern_type;

                                    foreach (var itm in CommonClass.Process_name_gridbind_columns_lotinfostatus_runtime)
                                    {
                                        string patern_type_list = itm.PaternType;
                                        string processId = itm.process_id;                                      
                                        if (itm.ProcessNames == chk && itm.materialcode == item.material_code.Split(',')[chk_index])
                                        {
                                            Console.WriteLine("Last lot no : " + item.lotnojoin);
                                            if (patern_type_list == "4" && processId == "108")
                                            {
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if (patern_type_list == "4" && processId == "109")
                                            {
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if (patern_type_list == "4" && processId == "110")
                                            {
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if (patern_type_list == "2" && processId == "107")
                                            {
                                                columun_count_v = columun_count_v + 10;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (patern_type_list == "1")
                                            {
                                                columun_count_v = columun_count_v + 5;
                                            }
                                            else if (patern_type_list == "2")
                                            {
                                                columun_count_v = columun_count_v + 10;
                                            }
                                            else if (patern_type_list == "3")
                                            {
                                                columun_count_v = columun_count_v + 2;
                                            }
                                            else if (patern_type_list == "4")
                                            {
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if (patern_type_list == "5")
                                            {
                                                columun_count_v = columun_count_v + 15;
                                            }
                                        }

                                    }

                                    int dataGridview1_row_index = 1;
                                    string shipment_date = string.Empty;
                                    this.dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    this.dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    this.dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        int row_index = row.Index;

                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();

                                            if (Compare_lotNo == item.lotnojoin)
                                            {
                                                row.Cells[0].Value = row_index + 1;
                                                row.Cells[1].Value = item.lotnojoin.Split(',')[0];
                                                row.Cells[2].Value = item.tb_bproduct.Split(',')[0];

                                                row.Cells[3].Value = item.onhold.Split(',')[0];
                                                row.Cells[4].Value = item.scrap.Split(',')[0];
                                                row.Cells[5].Value = item.reason_hs.Split(',')[0];
                                                if (!string.IsNullOrEmpty(item.shipment_date))
                                                {
                                                    shipment_date = item.shipment_date.Split(',')[0];
                                                    row.Cells[6].Value = shipment_date;                                                   
                                                    dataGridView1.Rows[row_index].Cells[7].Value = false;
                                                    dataGridView1.Rows[row_index].Cells[7].ReadOnly = true;
                                                }
                                                else
                                                {
                                                    shipment_date = "-";
                                                    row.Cells[6].Value = shipment_date;
                                                    dataGridView1.Rows[row_index].Cells[7].ReadOnly = true;
                                                }

                                                row.Cells[8].Value = item.customer_code;
                                                row.Cells[9].Value = item.customer_name;
                                                row.Cells[10].Value = item.item_code;
                                                row.Cells[11].Value = item.item_name;
                                                row.Cells[12].Value = item.tb_qty.Split(',')[chk_index];
                                                string sumOfQty = item.tb_qty.Split(',')[chk_index];
                                                if (sumOfQty != string.Empty)
                                                {
                                                    int add_sumoff = Convert.ToInt32(sumOfQty);
                                                    checkall_NoOfQty = add_sumoff + checkall_NoOfQty;
                                                }

                                                // calculate the total qty 
                                                if (Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                                {
                                                    string Selected_qty = row.Cells[12].Value.ToString();
                                                    int add_qty = NoOfQty + Convert.ToInt16(Selected_qty);
                                                    NoOfQty = add_qty;
                                                }
                                                //010923 row.Cells[13].Value = item.tb_manuf_dt.Split(',')[chk_index];
                                                DateTime manuf_dt = Convert.ToDateTime(item.tb_manuf_dt.Split(',')[chk_index],
                                               System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                row.Cells[13].Value = manuf_dt;
                                                // compare to current date
                                                DateTime from_dt = Convert.ToDateTime(item.tb_expairy_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                DateTime to_dt = DateTime.Now;
                                                int result = DateTime.Compare(from_dt, to_dt);
                                                if (result >= 1)
                                                {
                                                    row.Cells[14].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                }
                                                else
                                                {
                                                    row.Cells[14].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                }
                                                if (patern_type == "1")
                                                {
                                                    row.Cells[columun_count_v].Value = item.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.lotno_p1.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.plantingdate.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.pb_date.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }
                                                else if (patern_type == "2")
                                                {
                                                    row.Cells[columun_count_v].Value = item.plantingdate.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.sheetlotno_p2.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }
                                                else if (patern_type == "3")
                                                {
                                                    row.Cells[columun_count_v].Value = item.plantingdate.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }
                                                else if (patern_type == "4")
                                                {
                                                    row.Cells[columun_count_v].Value = item.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;

                                                    row.Cells[columun_count_v].Value = item.lotno_p4.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }



                                            }
                                            dataGridview1_row_index++;
                                        }

                                    }
                                    chk_index++;

                                }

                            }
                        }
                        list_index++;
                    }
                    // 2nd time loop. skip existing
                    already_exits_row_columns.AddRange(already_exits_row_header);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("insert_lotinfo_value_assign_gridbind", ex);
            }
        }
        public void lot_number_only_row_common(string ActionType_only_lot, string LotNum,string Custcd,string Itemcd)
        {
            try
            {
                string Compare_lotNo;              
                string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                string[] obj_only_lot = { ActionType_only_lot, Custcd, Itemcd, LotNum };
                DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);       
                if (ds_only_lot.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_only_lot.Tables[0].Rows)
                    {
                        string lotno_join = dr["lotnoJoin"].ToString();
                        string dG1joinlotno = lotno_join;
                        string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                        string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                        int convert_lotno = Convert.ToInt32(lotno_spl);
                        int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                        string lotno_format = convert_lotno.ToString("D7");
                        string lotnochld_format = convert_lotnochld.ToString("D2");
                        if (lotno_format != string.Empty)
                        {
                            int formate_type = Convert.ToInt32(lotno_format);
                            lotno_format = formate_type.ToString("D7");
                        }
                        if (lotnochld_format != string.Empty)
                        {
                            int formate_type = Convert.ToInt32(lotnochld_format);
                            lotnochld_format = formate_type.ToString("D2");
                        }
                        dG1joinlotno = lotno_format + "-" + lotnochld_format;
                        string lotno = lotno_format;                  
                        string manf_dt = dr["manufacturing_date"].ToString();
                        string expairy_dt = dr["expairy_date"].ToString();              
                        string lotqty = dr["lotqty"].ToString();                   
                        string bproduct = dr["bproduct"].ToString();
                        string onHold = dr["onhold"].ToString();
                        string scrap = dr["scrap"].ToString();
                        string reason_hs = dr["reason_hs"].ToString();
                        // row values bind 
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                Compare_lotNo = row.HeaderCell.Value.ToString();
                                if (Compare_lotNo == dG1joinlotno)
                                {                         
                                    if (onHold == "H")
                                    {
                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                    }
                                    else
                                    {
                                        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                                        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                                        this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
                                    }
                                    if (!string.IsNullOrEmpty(scrap))
                                    {
                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    if (row.Cells[0].Value == null)
                                    {
                                        row.Cells[0].Value = bproduct;
                                    }
                                    if (row.Cells[1].Value == null)
                                    {
                                        row.Cells[1].Value = onHold;
                                    }
                                    if (row.Cells[2].Value == null)
                                    {
                                        row.Cells[2].Value = scrap;
                                    }
                                    if (row.Cells[3].Value == null)
                                    {
                                        row.Cells[3].Value = reason_hs;
                                    }

                                    row.Cells[4].Value = lotqty;
                                    DateTime manfaturing_dt = Convert.ToDateTime(manf_dt,
                                   System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

                                    row.Cells[5].Value = manfaturing_dt;
                                    // compare to current date
                                    DateTime from_dt = Convert.ToDateTime(expairy_dt,
                                    System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                    DateTime to_dt = DateTime.Now;
                                    int result = DateTime.Compare(from_dt, to_dt);
                                    if (result >= 1)
                                    {
                                        row.Cells[6].Value = expairy_dt;
                                    }
                                    else
                                    {
                                        row.Cells[6].Value = expairy_dt;
                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    string manf_dte = manfaturing_dt.ToString("yyyyMMdd");
                                    row.Cells[7].Value = manf_dte + lotno + lotnochld_format;
                                    row.Cells[8].Value = lotnochld_format;
                                }
                            }
                        }                   
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("lot_number_only_row_common", ex);
            }
        }
        public void terminal_addlist_loadgrid_call(string ActionType)
        {            
            CommonClass.Process_name_gridbind_lotinfostatus = new List<PI_Process>();            
            PI_Process models = new PI_Process();
            models.id = "XXX";
            models.ProcessNames = "TERMINAL BOARD INFO";
            models.PaternType = "5";
            models.process_id = "0";
            CommonClass.Process_name_gridbind_lotinfostatus.Add(models);
            DataSet dset = new DataSet();

            dset = helper.GetDatasetByBOMView_Pro_input_shipment(string.Empty, string.Empty, ActionType);
            if (dset.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = dset.Tables[0];
                int i = 1;
                foreach (DataRow val in dtbl.Rows)
                {
                    PI_Process model = new PI_Process();
                    model.id = i.ToString();
                    model.ProcessNames = val[5].ToString();
                    model.PaternType = val[14].ToString();
                    model.process_id = val[15].ToString();
                    model.materialcode = val[6].ToString();
                    model.itemcode = val[2].ToString();
                    CommonClass.Process_name_gridbind_lotinfostatus.Add(model);
                    i++;
                }         
                CommonClass.Process_name_gridbind_columns_lotinfostatus.AddRange(CommonClass.Process_name_gridbind_lotinfostatus);
            }
            LoadDataGrid();
        }
        public void terminal_addlist_loadgrid_call_loop(string ActionType, string custcd, string itemcd)
        {
            CommonClass.Process_name_gridbind_lotinfostatus_runtime = new List<PI_Process>();
            CommonClass.Process_name_gridbind_columns_lotinfostatus_runtime = new List<PI_Process>();
            PI_Process models = new PI_Process();
            models.id = "XXX";
            models.ProcessNames = "TERMINAL BOARD INFO";
            models.PaternType = "5";
            models.process_id = "0";
            CommonClass.Process_name_gridbind_lotinfostatus_runtime.Add(models);
            DataSet dset = new DataSet();

            dset = helper.GetDatasetByBOMView_Pro_input_shipment(custcd, itemcd, ActionType);
            if (dset.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = dset.Tables[0];
                int i = 1;
                foreach (DataRow val in dtbl.Rows)
                {
                    PI_Process model = new PI_Process();
                    model.id = i.ToString();
                    model.ProcessNames = val[5].ToString();
                    model.PaternType = val[14].ToString();
                    model.process_id = val[15].ToString();
                    model.materialcode = val[6].ToString();
                    model.itemcode = val[2].ToString();
                    CommonClass.Process_name_gridbind_lotinfostatus_runtime.Add(model);
                    i++;
                }
                CommonClass.Process_name_gridbind_columns_lotinfostatus_runtime.AddRange(CommonClass.Process_name_gridbind_lotinfostatus_runtime);
            }      
        }
        private void LoadDataGrid()
        {
            try
            {
                dataGridView1.DataSource = null;
                int total_process = CommonClass.Process_name_gridbind_lotinfostatus.Count;

                // grid bind start
                int totalgroup = total_process;

                daysInMonths = new int[totalgroup]; // check line 129
                GroupLabel = new string[totalgroup];
                LabelString = new string[totalgroup, 15];
                LabelSize = new int[totalgroup, 15];
                List<KeyValuePair<int, string>> kvpList = new List<KeyValuePair<int, string>>();

                int i = 0;
                this.dataGridView1.Columns.Clear();
                foreach (var itm in CommonClass.Process_name_gridbind_lotinfostatus)
                {

                    int key = Convert.ToInt16(itm.PaternType);
                    string values = itm.ProcessNames;
                    kvpList.Add(new KeyValuePair<int, string>(key, values));
                    if (i > 0)
                    {
                        GroupLabel[i] = itm.ProcessNames;
                        if (key == 1)
                        {
                            LabelString[i, 0] = "Part No.";
                            LabelString[i, 1] = "Lot No.";
                            LabelString[i, 2] = "Planting Date";
                            LabelString[i, 3] = "Quantity";
                            LabelString[i, 4] = "Pb";
                            LabelSize[i, 0] = 80;
                            LabelSize[i, 1] = 80;
                            LabelSize[i, 2] = 120;
                            LabelSize[i, 3] = 80;
                            LabelSize[i, 4] = 80;
                        }
                        else if (key == 2)
                        {
                            LabelString[i, 0] = "Process Date";
                            LabelString[i, 1] = "Control No.";
                            LabelString[i, 2] = "Sheet LotNo.";
                            LabelString[i, 3] = "Quantity";
                            LabelSize[i, 0] = 120;
                            LabelSize[i, 1] = 120;
                            LabelSize[i, 2] = 100;
                            LabelSize[i, 3] = 80;
                        }
                        else if (key == 3)
                        {
                            LabelString[i, 0] = "Process Date";
                            LabelString[i, 1] = "Quantity";
                            LabelSize[i, 0] = 120;
                            LabelSize[i, 1] = 60;
                        }
                        else if (key == 4)
                        {
                            LabelString[i, 0] = "Part No";
                            LabelString[i, 1] = "Lot No";
                            LabelString[i, 2] = "Quantity";
                            LabelSize[i, 0] = 80;
                            LabelSize[i, 1] = 80;
                            LabelSize[i, 2] = 60;
                        }
                    }
                    else if (i == 0)
                    {
                        GroupLabel[0] = "TERMINAL BOARD INFO";
                        LabelString[0, 0] = "S.no";
                        LabelString[0, 1] = "Lot number";
                        LabelString[0, 2] = "B Product";
                        LabelString[0, 3] = "On Hold";
                        LabelString[0, 4] = "Scrap";
                        LabelString[0, 5] = "Remarks";
                        LabelString[0, 6] = "Shipment Date";
                        LabelString[0, 7] = "";
                        LabelString[0, 8] = "Customer Code";
                        LabelString[0, 9] = "Customer Name";
                        LabelString[0, 10] = "Item Code";
                        LabelString[0, 11] = "Item Name";
                        LabelString[0, 12] = "Quantity";
                        LabelString[0, 13] = "Manufacturing Date";
                        LabelString[0, 14] = "Expiry Date";
                        LabelSize[0, 0] = 30;
                        LabelSize[0, 1] = 100;
                        LabelSize[0, 2] = 85;
                        LabelSize[0, 3] = 80;
                        LabelSize[0, 4] = 80;
                        LabelSize[0, 5] = 100;
                        LabelSize[0, 6] = 150;
                        LabelSize[0, 7] = 80;
                        LabelSize[0, 8] = 150;
                        LabelSize[0, 9] = 150;
                        LabelSize[0, 10] = 150;
                        LabelSize[0, 11] = 150;
                        LabelSize[0, 12] = 150;
                        LabelSize[0, 13] = 150;
                        LabelSize[0, 14] = 150;
                    }
                    i++;
                }
                daysInMonths = new int[GroupLabel.Count()];
                // Add a column for each day of the year; where
                // column name = the date (creates all unique column names)
                // column header text = the numeric day of the month
                for (int month = 1; month <= kvpList.Count; month++)
                {
                    var element = kvpList.ElementAt(month - 1);
                    var Key = element.Key;
                    if (Key == 1)
                    {
                        daysInMonths[month - 1] = 5;
                    }
                    else if (Key == 2)
                    {
                        daysInMonths[month - 1] = 4;
                    }
                    else if (Key == 3)
                    {
                        daysInMonths[month - 1] = 2;
                    }
                    else if (Key == 4)
                    {
                        daysInMonths[month - 1] = 3;
                    }
                    else if (Key == 5)
                    {
                        daysInMonths[month - 1] = 15;
                    }
                    for (int day = 1; day <= daysInMonths[month - 1]; day++)
                    {


                        string colname = "";
                        string colheadname = "";
                        int colsize = 120;

                        if (month <= totalgroup)
                        {
                            colname = LabelString[month - 1, day - 1];
                            colheadname = LabelString[month - 1, day - 1];
                            colsize = LabelSize[month - 1, day - 1];

                        }
                        else
                        {
                            colname = string.Empty;
                            colheadname = string.Empty;
                            colsize = 80;
                        }
                        if (colname != string.Empty)
                        {
                            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn()
                            {
                                Name = colname,
                                HeaderText = colheadname,
                                Width = colsize
                            };
                            this.dataGridView1.Columns.Add(col);
                        }
                        else if (colname == string.Empty)
                        {
                            DataGridViewCheckBoxColumn col_chk = new DataGridViewCheckBoxColumn()
                            {

                                Width = colsize

                            };

                            this.dataGridView1.Columns.Add(col_chk);
                        }

                    }
                }
                dataGridView1.Columns[13].ValueType = typeof(DateTime);

                dataGridView1.Columns[13].DefaultCellStyle.Format = "dd-MM-yyyy";
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                this.dataGridView1.Paint += DataGridView1_Paint;
                this.dataGridView1.Scroll += DataGridView1_Scroll;
                this.dataGridView1.ColumnWidthChanged += DataGridView1_ColumnWidthChanged;
                this.dataGridView1.Resize += DataGridView1_Resize;

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("LoadDataGrid", ex);
            }
        }
        private void InvalidateHeader()
        {
            System.Drawing.Rectangle rtHeader = this.dataGridView1.DisplayRectangle;
            rtHeader.Height = this.dataGridView1.ColumnHeadersHeight / 2;
            this.dataGridView1.Invalidate(rtHeader);
        }
        private void DataGridView1_Resize(object sender, EventArgs e)
        {
            this.InvalidateHeader();
        }
        private void DataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.InvalidateHeader();
        }
        private void DataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            this.InvalidateHeader();
        }
        private void DataGridView1_Paint(object sender, PaintEventArgs e)
        {
            int col = 0;
            int count = 0;
            // For each month, create the display rectangle for the main title and draw it.
            foreach (int daysInMonth in daysInMonths)
            {
                System.Drawing.Rectangle r1 = this.dataGridView1.GetCellDisplayRectangle(col, -1, true);
                // Start the rectangle from the first visible day of the month,
                // and add the width of the column for each following day.
                for (int day = 0; day < daysInMonth; day++)
                {
                    System.Drawing.Rectangle r2 = this.dataGridView1.GetCellDisplayRectangle(col + day, -1, true);

                    if (r1.Width == 0) // Cell is not displayed.
                    {
                        r1 = r2;
                    }
                    else
                    {
                        r1.Width += r2.Width;
                    }
                }
                r1.X += 1;
                r1.Y += 1;
                r1.Height = r1.Height / 2 - 2;
                r1.Width -= 2;
                using (Brush back = new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor))
                using (Brush fore = new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor))
                using (Pen p = new Pen(this.dataGridView1.GridColor))
                using (StringFormat format = new StringFormat())
                {
                    string month = GroupLabel[count];
                    count++;
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.FillRectangle(back, r1);
                    e.Graphics.DrawRectangle(p, r1);
                    e.Graphics.DrawString(month, this.dataGridView1.ColumnHeadersDefaultCellStyle.Font, fore, r1, format);
                }
                col += daysInMonth; // Move to the first column of the next month.
            }
        }

        private void btn_nextPg_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;          
                dataGridView1.Refresh();
                int cPageNo = CommonClass.lotInfo_status_curentPageNo_nxtPg + 1;
                var Get_records = CommonClass.Runtime_Store_LI_Infostatus.ToPagedList(cPageNo, PageSize);
                CommonClass.lotInfo_status_curentPageNo_nxtPg = Get_records.PageNumber;
                if (Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = true;
                }
                else if (!Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = false;
                }
                foreach (var get_details in Get_records)
                {
                    insert_lotinfo_value_assign_gridbind(string.Empty, get_details.lotno, string.Empty, string.Empty, string.Empty, string.Empty, get_details.customercode, get_details.item_code, CommonClass.lotInfo_status_actionTyp_nxtPg, CommonClass.lotInfo_status_spname_nxtPg, string.Empty, string.Empty);
                }
                dataGridView1.Refresh();
                dataGridView1.Sort(dataGridView1.Columns[13], ListSortDirection.Descending);
                dataGridView1.RefreshEdit();
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("btn_nextPg_Click",ex);
            }
        }
        public void Common_Selected_Lotno_all()
        {
            try
            {
                CommonClass.Runtime_Store_LI_Infostatus = store_selected_lotno.OrderByDescending(c => c.manf_dt).ToList();
                CommonClass.lotInfo_status_spname_nxtPg = "lotinfostatus_cust_itemcd_wtlot";
                CommonClass.lotInfo_status_actionTyp_nxtPg = "custcd_itemcd";
                var Get_records = store_selected_lotno.OrderByDescending(c => c.manf_dt).ToPagedList(PageNumber, PageSize);
                if (Get_records.IsLastPage)
                {
                    btn_nextPg.Enabled = false;
                }
                else if (Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = true;
                }
                int rows = 1;
                already_exits_row_header = new List<string>();
                already_exits_row_columns = new List<string>();
                CommonClass.lotInfo_status_curentPageNo_nxtPg = PageNumber;
                foreach (var get_details in Get_records)
                {
                    // Grid header load and use process names dynamic
                    terminal_addlist_loadgrid_call_loop("GetData", get_details.customercode, get_details.item_code);
                    string spname = "lotinfostatus_cust_itemcd_wtlot";
                    insert_lotinfo_value_assign_gridbind(string.Empty, get_details.lotno, string.Empty, string.Empty, string.Empty, string.Empty, get_details.customercode, get_details.item_code, "custcd_itemcd", spname, string.Empty, string.Empty);
                    rows++;
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("Common_Selected_Lotno_all", ex);
            }
        }
        public void Common_Selected_Lotno_few()
        {
            try
            {
                int row_index = 0;
                
                foreach (KeyValuePair<int, string> obj in checkedListBox_lotno.CheckedItems)
                {
                    list_of_lotnumbers model_selected_lotno = new list_of_lotnumbers();                    
                    model_selected_lotno.lotno = obj.Key.ToString();
                    string split_value_cust_itm = obj.Value.ToString();
                    model_selected_lotno.customercode = split_value_cust_itm.Split(',')[0];
                    model_selected_lotno.item_code = split_value_cust_itm.Split(',')[1];
                    store_selected_lotno_few.Add(model_selected_lotno);
                    row_index++;
                }
                CommonClass.Runtime_Store_LI_Infostatus = store_selected_lotno_few.OrderByDescending(c => c.manf_dt).ToList();
                CommonClass.lotInfo_status_spname_nxtPg = "lotinfostatus_cust_itemcd_wtlot";
                CommonClass.lotInfo_status_actionTyp_nxtPg = "custcd_itemcd";
                var Get_records = store_selected_lotno_few.OrderByDescending(c => c.manf_dt).ToPagedList(PageNumber, PageSize);
                if (Get_records.IsLastPage)
                {
                    btn_nextPg.Enabled = false;
                }
                else if (Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = true;
                }
                int rows = 1;
                already_exits_row_header = new List<string>();
                already_exits_row_columns = new List<string>();
                CommonClass.lotInfo_status_curentPageNo_nxtPg = PageNumber;
                foreach (var get_details in Get_records)
                {
                    // Grid header load and use process names dynamic
                    terminal_addlist_loadgrid_call_loop("GetData", get_details.customercode, get_details.item_code);
                    string spname = "lotinfostatus_cust_itemcd_wtlot";
                    insert_lotinfo_value_assign_gridbind(string.Empty, get_details.lotno, string.Empty, string.Empty, string.Empty, string.Empty, get_details.customercode, get_details.item_code, "custcd_itemcd", spname, string.Empty, string.Empty);
                    rows++;
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("Common_Selected_Lotno", ex);
            }
        }
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void btn_shipping_dwn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Lot Information status Details ?", "DOWNLOAD SHIPMENT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Console.WriteLine("Download start time : " + DateTime.Now.ToString("HH:mm:ss"));
                    copyAlltoClipboard();
                    Excel.Application XcelApp;
                    Excel.Workbook oWB;
                    Excel.Worksheet ws;
                    object misValue = System.Reflection.Missing.Value;
                    XcelApp = new Excel.Application();
                    oWB = XcelApp.Workbooks.Add(misValue);
                    ws = oWB.ActiveSheet;
                    ws = (Excel.Worksheet)oWB.Worksheets.get_Item(1);
                     // Accessing the first worksheet in the Excel file                   
                    Excel.Range oRng;
                    XcelApp.DisplayAlerts = false;
                    int top_i = 8;
                  
                    foreach (var topheader in CommonClass.Process_name_gridbind_columns_lotinfostatus)
                    {
                        if (topheader.ProcessNames != "TERMINAL BOARD INFO")
                        {
                            if (topheader.PaternType == "1")
                            {
                                Excel.Range d1 = ws.Cells[1, top_i];
                                top_i = top_i + 4;
                                Excel.Range d2 = ws.Cells[1, top_i];
                                oRng = ws.get_Range(d1, d2);
                                oRng.Value2 = topheader.ProcessNames;
                                oRng.Merge(Missing.Value);
                            }
                            else if (topheader.PaternType == "2")
                            {
                                Excel.Range d1 = ws.Cells[1, top_i];
                                top_i = top_i + 3;
                                Excel.Range d2 = ws.Cells[1, top_i];
                                oRng = ws.get_Range(d1, d2);
                                oRng.Value2 = topheader.ProcessNames;
                                oRng.Merge(Missing.Value);
                            }
                            else if (topheader.PaternType == "3")
                            {
                                Excel.Range d1 = ws.Cells[1, top_i];
                                top_i = top_i + 1;
                                Excel.Range d2 = ws.Cells[1, top_i];
                                oRng = ws.get_Range(d1, d2);
                                oRng.Value2 = topheader.ProcessNames;
                                oRng.Merge(Missing.Value);
                            }
                            else if (topheader.PaternType == "4")
                            {
                                Excel.Range d1 = ws.Cells[1, top_i];
                                top_i = top_i + 2;
                                Excel.Range d2 = ws.Cells[1, top_i];
                                oRng = ws.get_Range(d1, d2);
                                oRng.Value2 = topheader.ProcessNames;
                                oRng.Merge(Missing.Value);
                            }
                        }
                        else
                        {
                            Excel.Range c1 = ws.Cells[1, 6];             
                            top_i = top_i + 7;
                            Excel.Range c2 = ws.Cells[1, top_i];
                            oRng =ws.get_Range(c1, c2);
                            oRng.Value2 = topheader.ProcessNames;
                            oRng.Merge(Missing.Value);
                        }
                        top_i++;
                    }
                    int get_date_column = 1;                
                    for (int i = 1; i < dataGridView1.Columns.Count; i++)
                    {
                        XcelApp.Cells[2, i + 1] = dataGridView1.Columns[get_date_column].HeaderText;                       
                        get_date_column++;
                    }
                    Excel.Range DeleteRange_A = XcelApp.Range["A:A"];                
                    Excel.Range DeleteRange_G = XcelApp.Range["G:G"];
                    Excel.Range DeleteRange_H = XcelApp.Range["H:H"];
                    Excel.Range DeleteRange_J = XcelApp.Range["J:J"];
                    Excel.Range DeleteRange_L = XcelApp.Range["L:L"];

                    DeleteRange_G.Delete();
                     DeleteRange_H.Delete();
                    DeleteRange_J.Delete();
                    DeleteRange_L.Delete();
                    Excel.Range CR = (Excel.Range)ws.Cells[3, 1];
                    CR.Select();
                    ws.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                    DeleteRange_A.Delete();
                    // Column Header 1 
                    List<ObjColumns> array = new List<ObjColumns>();
                    array.Add(new ObjColumns("A1", "I1"));
                    oRng = ws.get_Range("A1", "I1");
                    oRng.Value2 = "TERMINAL BOARD INFO";
                    oRng.Merge(Missing.Value);
                    //  Auto fit automatically adjust the width of columns of Excel  in givien range .                   
                    XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView1.Rows.Count, dataGridView1.Columns.Count]].EntireColumn.AutoFit();
                    XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView1.Columns.Count + 5]].Font.Bold = true;
                    XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[dataGridView1.Columns.Count + 5]].Font.Bold = true;
                    XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dataGridView1.Columns.Count]].Font.Size = 13;
                    XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[1, dataGridView1.Columns.Count]].Font.Size = 12;
                    XcelApp.Columns.Borders.Color = Color.Black;
                    XcelApp.Columns.AutoFit();
                    DateTime current_date = DateTime.Now;
                    DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                    string compinepath = "\\Lot Information Status-" + datetime;
                    string newFileName = path + compinepath;
                    // Now save this file.
                    ws.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12);
                    XcelApp.Visible = true;
                    dataGridView1.ClearSelection();
                    Console.WriteLine("Download end time : " + DateTime.Now.ToString("HH:mm:ss"));
                    Cursor.Current = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_shipping_dwn_Click", ex);
            }
        }
        public static void CheckDirectory(string logFolder)
        {
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

        }
        public class ObjColumns
        {
            public ObjColumns(string s, string t)
            {
                start = s;
                ends = t;
            }
            public string start { get; set; }
            public string ends { get; set; }
        }

        private void FormLotInformationStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnSearch.PerformClick();
            }
            if (e.KeyCode == Keys.F9)
            {
                btnClose.PerformClick();
            }
            if (e.KeyCode == Keys.F8)
            {
                btn_shipping_dwn.PerformClick();
            }
        }
    }
}
