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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopPartsElectronics_PS.Helper;
using YourApp.Data;
using static TopPartsElectronics_PS.Helper.GeneralModelClass;
using Excel = Microsoft.Office.Interop.Excel;

namespace TopPartsElectronics_PS
{
    public partial class FormShipping : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        List<Lotinfo_gridbind_common_pattern> lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
        private int[] daysInMonths;
        private string[] GroupLabel;
        private string[,] LabelString;
        private int[,] LabelSize;
        private bool search_gridheight_already_enable = false;    
        List<string> already_exits_shipment_lotnochild = new List<string>();
        List<string> already_exits_shipment_lotnochild_selectall = new List<string>();
        List<string> already_exits_shipment_process_id_header = new List<string>();    
        List<string> already_exits_row_header = new List<string>();
        List<string> already_exits_row_columns = new List<string>();
        int NoOfQty = 0;
        int checkall_NoOfQty = 0;

        int shipped_qty = 0;
        string selected_delete_lotno = string.Empty;
        string selected_date_checking_three_month_b4 = string.Empty;

        int PageNumber = 1;
        int PageSize = 10;

        public FormShipping()
        {
            InitializeComponent();
        }

        private void FormShipping_Load(object sender, EventArgs e)
        {
            dataGridView2.AutoGenerateColumns = false;
            date_shipment_date.Value = DateTime.Today.AddDays(1);
            date_manf_frm.Value = DateTime.Today.AddDays(-1);
            date_manf_to.Value = DateTime.Today.AddDays(-1);

        }

        private void FormShipping_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).shippingToolStripMenuItem.Enabled = true;
        }
      

        private void FormShipping_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_execute.PerformClick();
            }
            if (e.KeyCode == Keys.F3)
            {
                btnsearch.PerformClick();
            }
            if (e.KeyCode == Keys.F4)
            {
                btn_selectall.PerformClick();
            }
            if (e.KeyCode == Keys.F9)
            {
                btn_close.PerformClick();
            }
            if(e.KeyCode== Keys.F6)
            {
                btn_delete_lotno.PerformClick();
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            FormSearchClient frm = new FormSearchClient();
            MysqlHelper.call_from_shipping_to_client = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ShowDialog();
        }

        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            FormSearchItem frm = new FormSearchItem();
            MysqlHelper.call_from_shipping_to_item = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.CustomerCode = txtCustomerCode.Text;
            frm.CustomerNameF = txtCustomerNameF.Text;
            frm.ShowDialog();
        }
        public void SetSearchId(string code, string shortname, string fullname)
        {
            txtCustomerCode.Text = code;
            txtCustomerNameF.Text = fullname;
            textItemCode.Text = string.Empty;
            txt_itemname.Text = string.Empty;
            chk_customer.Checked = true;

        }

        public void SetSearchId_Item(string customercode, string itemcode, string fullname)
        {
            textItemCode.Text = itemcode;
            txt_itemname.Text = fullname;
            chk_customer.Checked = true;
            chk_item.Checked = true;
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {                
                if (checkInput_checkbox_check_anyone_must())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // pagination details refresh
                    btn_nextPg.Enabled = true;
                    CommonClass.shipping_spname_nxtPg = string.Empty;
                    CommonClass.shipping_actionTyp2_nxtPg = string.Empty;
                    CommonClass.shipping_manfdt_frm_nxtPg = string.Empty;
                    CommonClass.shipping_manfdt_to_nxtPg = string.Empty;
                    CommonClass.shipping_lotno_child_frm_nxtPg = string.Empty;
                    CommonClass.shipping_lotno_child_to_nxtPg = string.Empty;
                    CommonClass.shipping_actionTyp1_nxtPg = string.Empty;
                    CommonClass.shipping_curentPageNo_nxtPg = 0;
                    CommonClass.shipping_curentPageSize_nxtPg = 0;
                    // pg end
                
                    CommonClass.Process_name_gridbind_columns_shipping = new List<PI_Process>();
                    CommonClass.Process_name_gridbind_columns_shipping_runtime = new List<PI_Process>();
                    already_exits_shipment_lotnochild_selectall = new List<string>();
                    already_exits_shipment_lotnochild = new List<string>();
                    already_exits_shipment_process_id_header = new List<string>();
                    already_exits_row_header = new List<string>();
                    shipped_qty = 0;
                    NoOfQty = 0;
                    checkall_NoOfQty = 0;
                    lbl_totalqty.Text = "0";
                    lbl_noboxsp.Text = "0";
                    cmb_box_ship.SelectedIndex = -1;
                    cmb_box_ship.Text = "Choose";           

                    DataTable dt = new DataTable();
                    dataGridView2.DataSource = dt;
                    dataGridView2.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.DataSource = null;
                    CommonClass.shipping_update_lotno = new List<shippingUpdate>();
                    cmb_box_ship.Enabled = true;
                   
                    if (!search_gridheight_already_enable)
                    {
                        this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 3;
                        search_gridheight_already_enable = true;
                    }
                    selected_checkbox_method();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Atleast one Checked of this check-box ....", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chk_lotno.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnsearch_Click", ex);
            }
        }
        public void selected_checkbox_method()
        {
            // pagination data below
            CommonClass.shipping_manfdt_frm_nxtPg = date_manf_frm.Value.ToString("yyyy-MM-dd");
            CommonClass.shipping_manfdt_to_nxtPg = date_manf_to.Value.ToString("yyyy-MM-dd");
            CommonClass.shipping_lotno_child_frm_nxtPg = txt_lotno_frm.Text;
            CommonClass.shipping_lotno_child_to_nxtPg = txt_lotno_to.Text;
            CommonClass.shipping_curentPageNo_nxtPg = PageNumber;
            CommonClass.shipping_curentPageSize_nxtPg = PageSize;
            // pg end
            if (checkInput_checkbox_check_all())
            {
                if (only_chkecked_lotno_and_manfdt_customer_itemcd())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }                 
                    terminal_addlist_loadgrid_call("shipment_others");
         
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();    
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, string.Empty, "lotno_manfdt_cust" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }               
                    already_exits_row_header = new List<string>();                 
                    already_exits_row_columns = new List<string>();
                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_all";
                    CommonClass.shipping_actionTyp2_nxtPg = "allchecked";                    
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);
                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", txtCustomerCode.Text, textItemCode.Text);
                        string sp_name = "allpattern_view_itemcode_shipment_all";
                        insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "allchecked", sp_name,get_cd.customer_name,get_cd.item_code);
                    }
                }
            }
            else if (!checkInput_checkbox_check_all())
            {
                // all 
                if (chkecked_all())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked 
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
                   
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, string.Empty, "lotno_manfdt_cust" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }
                    already_exits_row_header = new List<string>();      
                    already_exits_row_columns = new List<string>();
                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_all";
                    CommonClass.shipping_actionTyp2_nxtPg = "allchecked";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", txtCustomerCode.Text, textItemCode.Text);
                        string sp_name = "allpattern_view_itemcode_shipment_all";
                        insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "allchecked", sp_name, get_cd.customer_name, get_cd.item_code);
                    }
                }
                // only lotno
                if (only_chkecked_lotno())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked // 
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
                 
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
                   
               
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text,txt_lotno_frm.Text,txt_lotno_to.Text,string.Empty, string.Empty, string.Empty, string.Empty, "lotno" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
                      
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }                    
                    already_exits_row_header = new List<string>();             
                    already_exits_row_columns = new List<string>();
                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_only_lotno";
                    CommonClass.shipping_actionTyp2_nxtPg = "lotno";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count > 0)
                    {
                        foreach (var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            string sp_name = "allpattern_view_itemcode_shipment_only_lotno";
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, get_cd.customer_code, get_cd.item_code, "lotno", sp_name,get_cd.customer_name,get_cd.item_name);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    }
                }
                // only manf date
                if (only_chkecked_manfdt())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");                
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();          
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to","@custcd","@itemcd", "@ActionType" };
                    string[] obj = { string.Empty, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),string.Empty, string.Empty, "manfdt" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }                 
                    already_exits_row_header = new List<string>();           
                    already_exits_row_columns = new List<string>();
                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_only_manfdt";
                    CommonClass.shipping_actionTyp2_nxtPg = "manfdt";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);
                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count > 0)
                    {
                        foreach (var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            string sp_name = "allpattern_view_itemcode_shipment_only_manfdt";
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "manfdt", sp_name,get_cd.customer_name,get_cd.item_name);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    }
                    

                }
                // only manf date and customer
                if (only_chkecked_manfdt_and_custmr())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
                 
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
                                       
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { string.Empty, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, string.Empty, "manfdt_cust" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {                    
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }
              
                    already_exits_row_header = new List<string>();                
                    already_exits_row_columns = new List<string>();

                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_only_manfdt_cust_witlot";
                    CommonClass.shipping_actionTyp2_nxtPg = "manfdt_cust";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count > 0)
                    {
                        foreach (var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            string sp_name = "allpattern_view_itemcode_shipment_only_manfdt_cust_witlot";
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "manfdt_cust", sp_name,get_cd.customer_name,get_cd.item_name);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    }
                    

                }
                // only customer
                if (only_chkecked_cust())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked 
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
              
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

                 
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { string.Empty, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, string.Empty, "cust" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {                  
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();

                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }
                    CommonClass.Process_name_gridbind_shipping_runtime_filter = new List<PI_Process>();
                    CommonClass.Process_name_gridbind_columns_shipping_runtime_filter = new List<PI_Process>();

                   
                    already_exits_row_header = new List<string>();
             
                    already_exits_row_columns = new List<string>();

                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_cust";
                    CommonClass.shipping_actionTyp2_nxtPg = "cust";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count > 0)
                    {
                        foreach(var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            string sp_name = "allpattern_view_itemcode_shipment_cust";
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, string.Empty, string.Empty, string.Empty, string.Empty, get_cd.customer_code, get_cd.item_code, "cust", sp_name,get_cd.customer_name,get_cd.item_name);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    }
                    
                }                
                // lot no and manufact date 
                else if (only_chkecked_lotno_and_manfdt())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
           
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

   
                    string[] str = { "@lotno", "@lotno_frm","@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text,txt_lotno_frm.Text,txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, string.Empty, "lotno_manfdt" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
               
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();

                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }                 
                    already_exits_row_header = new List<string>();           
                    already_exits_row_columns = new List<string>();

                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_lot_manfdt";
                    CommonClass.shipping_actionTyp2_nxtPg = "lotno_mdt";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                        string sp_name = "allpattern_view_itemcode_shipment_lot_manfdt";
                        insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "lotno_mdt", sp_name,get_cd.customer_name,get_cd.item_name);

                    }
                }
                // lot no,manufact date and customer
                else if (only_chkecked_lotno_and_manfdt_customer())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
                   
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

        
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, string.Empty, "lotno_manfdt_cust" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {                 
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }          
                    already_exits_row_header = new List<string>();       
                    already_exits_row_columns = new List<string>();

                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_all";
                    CommonClass.shipping_actionTyp2_nxtPg = "allchecked";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                        string sp_name = "allpattern_view_itemcode_shipment_all";
                        insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "allchecked", sp_name,get_cd.customer_name,get_cd.item_name);

                    }
                }
                // lot no ,customer 
                else if (only_chkecked_lotno_and_customer())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    } 
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
               
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

         
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, "lotno_cust" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {                  
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }             
                    already_exits_row_header = new List<string>();           
                    already_exits_row_columns = new List<string>();

                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_lotno_cust";
                    CommonClass.shipping_actionTyp2_nxtPg = "lot_custcd";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                  
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                        string sp_name = "allpattern_view_itemcode_shipment_lotno_cust";
                        insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "lot_custcd", sp_name,get_cd.customer_name,get_cd.item_name);

                    }
                }
                // customer and item 
                else if (only_chkecked_customer_and_itemcode())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }        
                    terminal_addlist_loadgrid_call("shipment_others");
             
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

             
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, txtCustomerCode.Text,textItemCode.Text, "cust_item" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {              
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }
                 
                    already_exits_row_header = new List<string>();              
                    already_exits_row_columns = new List<string>();

                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_cust_itemcd_wtlot";
                    CommonClass.shipping_actionTyp2_nxtPg = "custcd_itemcd";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End                 
                  
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count>0)
                    {
                        foreach (var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);                           
                            string sp_name = "allpattern_view_itemcode_shipment_cust_itemcd_wtlot";                          
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "custcd_itemcd", sp_name, get_cd.customer_name, get_cd.item_name);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    } 
                }
                // manf , cust , itemcd
                else if (only_chkecked_manf_customer_and_itemcode())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
                   
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

                   
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "manf_cust_item" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }
                 
                    already_exits_row_header = new List<string>();
                    already_exits_row_columns = new List<string>();
                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_manfdt_cust_item_witlot";
                    CommonClass.shipping_actionTyp2_nxtPg = "manfdt_custcd_itemcd";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count>0)
                    {
                        foreach (var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            string sp_name = "allpattern_view_itemcode_shipment_manfdt_cust_item_witlot";
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "manfdt_custcd_itemcd", sp_name,get_cd.customer_name,get_cd.item_name);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    }
                }
                // lotno , cust , itemcd
                else if (only_chkecked_lotno_and_customer_itemcd())
                {
                    string ActionType = string.Empty;
                    if (chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_onhold_scrap";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold_scrap";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_shipdate_scrap";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked && chk_scrap_dt.Checked)
                    {
                        ActionType = "shipment_only_lotno_scrap";
                    }
                    // start at 2 check only checked //
                    else if (chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude";
                    }
                    else if (!chkExclude.Checked && chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_onhold";
                    }
                    else if (!chkExclude.Checked && !chk_ex_onhold.Checked)
                    {
                        ActionType = "shipment_only_lotno_Exclude_onhold";
                    }
                    terminal_addlist_loadgrid_call("shipment_others");
                    
                    List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

                 
                    string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                    string[] obj = { txt_lotno.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, txtCustomerCode.Text, textItemCode.Text, "lot_cust_item" };
                    DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                    if (dtable_spm.Rows.Count > 0)
                    {
                      
                        foreach (DataRow drow in dtable_spm.Rows)
                        {
                            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                            model.customer_code = drow["customer_code"].ToString();
                            model.item_code = drow["item_code"].ToString();
                            model.lotno = drow["lot_no"].ToString();
                            model.customer_name = drow["customername"].ToString();
                            model.item_name = drow["item_name"].ToString();
                            get_cust_itemcd.Add(model);
                        }
                    }
                    
                    already_exits_row_header = new List<string>();
                    already_exits_row_columns = new List<string>();
                    // Pagination data below 
                    CommonClass.Runtime_Store_Shipping_details = get_cust_itemcd.ToList();
                    CommonClass.shipping_spname_nxtPg = "allpattern_view_itemcode_shipment_lot_cust_item";
                    CommonClass.shipping_actionTyp2_nxtPg = "manfdt_custcd_itemcd";
                    CommonClass.shipping_actionTyp1_nxtPg = ActionType;
                    //Pagination End
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    if (Get_records.Count > 0)
                    {
                        foreach (var get_cd in Get_records)
                        {
                            terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            string sp_name = "allpattern_view_itemcode_shipment_lot_cust_item";
                            insert_lotinfo_value_assign_gridbind(ActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, "manfdt_custcd_itemcd", sp_name,get_cd.customer_name,get_cd.item_name);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerCode.Focus();
                    }
                    
                }
            }
        }
        public void grid_bind(string ActionType, string lotn, string lotn_frm, string lotn_to, string manf_dt_frm, string manf_dt_to, string customer_code, string item_code, string auctionrole)
        {
            try
            {
                dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.AllowUserToAddRows = true;
                DataTable dtable = new DataTable();
                string[] str_view = { "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@cust_cd", "@itm_cd", "@ActionType", "@Actionrole" };
                string[] obj_view = { lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, item_code, ActionType, auctionrole };
                dtable = helper.GetDatasetByCommandString_dt("shipment_details_fetch", str_view, obj_view);

                var allDuplicates = dtable.AsEnumerable()
                          .GroupBy(dr => dr.Field<string>("lotnoandchild"))
                          .Where(g => g.Count() > 1)
                          .SelectMany(g => g)
                          .ToList();
                var cleaning_only_or_inspection_only = dtable.AsEnumerable()
                    .GroupBy(dr => dr.Field<string>("lotnoandchild"))
                    .Where(g => g.Count() <= 1)
                    .SelectMany(g => g)
                    .ToList();
             

                int index = 0;
                if (dtable.Rows.Count > 0)
                {
                    List<string> already_exits_row = new List<string>();
                    foreach (DataRow drow in dtable.Rows)
                    {
                        if (!already_exits_row.Contains(drow["lotnoandchild"].ToString()))
                        {
                            dataGridView2.Rows.Add();
                            // lot no format change 
                            string lotnumber = drow["lotnoandchild"].ToString();
                            string lotno_spl = lotnumber.Split('-')[0].ToString();
                            string lotno_spl_chld = lotnumber.Split('-')[1].ToString();
                            int convert_lotno = Convert.ToInt32(lotno_spl);
                            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                            string lotno_format = convert_lotno.ToString("D7");
                            string lotnochld_format = convert_lotnochld.ToString("D2");
                          
                            dataGridView2.Rows[index].Cells[0].Value = lotno_format + "-" + lotnochld_format;
                            dataGridView2.Rows[index].Cells[2].Value = drow["customerfull_name"];
                            dataGridView2.Rows[index].Cells[3].Value = drow["item_code"];
                            dataGridView2.Rows[index].Cells[4].Value = drow["item_name"];
                            dataGridView2.Rows[index].Cells[5].Value = drow["manufacturing_date"];
                            dataGridView2.Rows[index].Cells[6].Value = drow["expairy_dt"];
                            dataGridView2.Rows[index].Cells[7].Value = drow["lotqty"];
                            string process_id = drow["process_id"].ToString();

                            if (process_id == "101")
                            {
                                dataGridView2.Rows[index].Cells[9].Value = drow["process_date"];

                                var duplicates = dtable.AsEnumerable()
                               .GroupBy(dr => dr.Field<string>("lotnoandchild"))
                               .Where(g => g.Count() > 1)
                               .Select(g => new
                               {
                                   g
                                   //   key = g["lotnoandchild"],
                                   //  val = g[""]
                               })
                                .ToList();
                                string current_lotnoandchild = drow["lotnoandchild"].ToString();
                                foreach (var item2 in allDuplicates)
                                {
                                    string lotno_child = item2.ItemArray[3].ToString();
                                    string current_processid = item2.ItemArray[5].ToString();
                                    if (current_lotnoandchild == lotno_child)
                                    {
                                        if (current_processid == "102")
                                        {
                                            dataGridView2.Rows[index].Cells[8].Value = item2.ItemArray[4].ToString();
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item2.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item2.ItemArray[15].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item2.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item2.ItemArray[15].ToString();
                                            }
                                        }
                                    }
                                }
                                foreach (var item3 in cleaning_only_or_inspection_only)
                                {
                                    string lotno_child = item3.ItemArray[3].ToString();
                                    string current_processid = item3.ItemArray[5].ToString();
                                    if (current_lotnoandchild == lotno_child)
                                    {
                                        if (current_processid == "102")
                                        {
                                            dataGridView2.Rows[index].Cells[8].Value = item3.ItemArray[4].ToString();
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item3.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item3.ItemArray[15].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item3.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item3.ItemArray[15].ToString();
                                            }
                                        }
                                    }
                                }

                            }
                            if (process_id == "102")
                            {
                                dataGridView2.Rows[index].Cells[9].Value = drow["process_date"];
                                
                                string current_lotnoandchild = drow["lotnoandchild"].ToString();
                                foreach (var item2 in allDuplicates)
                                {
                                    string lotno_child = item2.ItemArray[3].ToString();
                                    string current_processid = item2.ItemArray[5].ToString();
                                    if (current_lotnoandchild == lotno_child)
                                    {
                                        if (current_processid == "101")
                                        {
                                            dataGridView2.Rows[index].Cells[9].Value = item2.ItemArray[4].ToString();
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item2.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item2.ItemArray[15].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item2.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item2.ItemArray[15].ToString();
                                            }
                                        }
                                    }
                                }
                                foreach (var item3 in cleaning_only_or_inspection_only)
                                {
                                    string lotno_child = item3.ItemArray[3].ToString();
                                    string current_processid = item3.ItemArray[5].ToString();
                                    if (current_lotnoandchild == lotno_child)
                                    {
                                        if (current_processid == "102")
                                        {
                                            dataGridView2.Rows[index].Cells[8].Value = item3.ItemArray[4].ToString();
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item3.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item3.ItemArray[15].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (dataGridView2.Rows[index].Cells[11].Value != null)
                                            {
                                                string join_pk_patternthree = dataGridView2.Rows[index].Cells[11].Value.ToString();
                                                dataGridView2.Rows[index].Cells[11].Value = join_pk_patternthree + "," + item3.ItemArray[15].ToString();
                                            }
                                            else if (dataGridView2.Rows[index].Cells[11].Value == null)
                                            {
                                                dataGridView2.Rows[index].Cells[11].Value = item3.ItemArray[15].ToString();
                                            }
                                        }
                                    }
                                }

                            }                           
                            string shipment_date = drow["lotinfoms_shipmentdt"].ToString();
                            dataGridView2.Rows[index].Cells[10].Value = shipment_date;
                            dataGridView2.Columns[10].DefaultCellStyle.Format = "dd/MM/yyyy";
                            dataGridView2.Rows[index].Cells[12].Value = drow["pk_lotinfo_master"];
                            dataGridView2.Rows[index].Cells[13].Value = drow["customercode"];
                            dataGridView2.Rows[index].Cells[14].Value = drow["shortname"];

                            if (shipment_date == "-")
                            {
                                dataGridView2.Rows[index].Cells[4].Value = CheckState.Checked;
                                dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                // checked the row 
                                bool flag = false;                    
                                DataGridViewRow row = dataGridView2.Rows[index];
                               
                                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
                                if (chk.Value == chk.TrueValue)
                                {
                                    chk.Value = chk.FalseValue;
                                }
                                else
                                {
                                    chk.Value = chk.TrueValue;
                                }
                                chk.Value = !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
                                if (Convert.ToBoolean(chk.Value))
                                {                                    
                                    if (Convert.ToString(row.Cells[0].Value) == string.Empty)
                                    {
                                        flag = true;
                                    }
                                    if (!flag)
                                    {
                                        shippingUpdate model = new shippingUpdate();
                                        string lotnoandchild = row.Cells[0].Value.ToString();
                                        model.lotno = lotnoandchild.Split('-')[0];
                                        model.lotno_from = lotnoandchild.Split('-')[1];
                                        if (Convert.ToString(row.Cells[11].Value) != string.Empty)
                                        {
                                            model.pk_p3 = row.Cells[11].Value.ToString();
                                        }
                                        else
                                        {
                                            model.pk_p3 = "-";
                                        }
                                        if (Convert.ToString(row.Cells[12].Value) != string.Empty)
                                        {
                                            model.pk_lotinfo_ms = row.Cells[12].Value.ToString();
                                        }
                                        else
                                        {
                                            model.pk_lotinfo_ms = "-";
                                        }
                                        CommonClass.shipping_update_lotno.Add(model);
                                    }
                                    else
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = false;                                        

                                    }

                                }
                                else if (!Convert.ToBoolean(chk.Value))
                                {
                                    string lotnoandchild = row.Cells[0].Value.ToString();
                                    string lotnu = lotnoandchild.Split('-')[0];
                                    string lotno_from = lotnoandchild.Split('-')[1];
                                    CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotnu && x.lotno_from == lotno_from);
                                    CommonClass.shipping_update_lotno.Distinct().ToList();
                                }
                         
                            }
                            else
                            {
                                dataGridView2.Rows[index].Cells[1].Value = false;
                                dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                            }

                            index++;

                            already_exits_row.Add(drow["lotnoandchild"].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Records Found..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.dataGridView2.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("grid_bind", ex);
            }
        }
        public bool only_chkecked_lotno()
        {
            bool result = false;
            if (chk_lotno.Checked && !chk_manf_dt_frm_to.Checked && !chk_item.Checked && !chk_customer.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_manfdt()
        {
            bool result = false;
            if (chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && !chk_item.Checked && !chk_customer.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool chkecked_all()
        {
            bool result = false;
            if (chk_manf_dt_frm_to.Checked && chk_lotno.Checked && chk_item.Checked && chk_customer.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_cust()
        {
            bool result = false;
            if (chk_customer.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && !chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_item()
        {
            bool result = false;
            if (!chk_customer.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && chk_item.Checked)
            {
                result = true;
            }
            return result;
        }        
        public bool only_chkecked_lotno_and_manfdt()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_manf_dt_frm_to.Checked && !chk_item.Checked && !chk_customer.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_manfdt_and_custmr()
        {
            bool result = false;
            if (!chk_lotno.Checked && chk_manf_dt_frm_to.Checked && !chk_item.Checked && chk_customer.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_lotno_and_manfdt_customer()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_manf_dt_frm_to.Checked && chk_customer.Checked && !chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_lotno_and_customer()
        {
            bool result = false;
            if (chk_lotno.Checked && !chk_manf_dt_frm_to.Checked && chk_customer.Checked && !chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_customer_and_itemcode()
        {
            bool result = false;
            if (!chk_lotno.Checked && !chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_manf_customer_and_itemcode()
        {
            bool result = false;
            if (!chk_lotno.Checked && chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_lotno_and_manfdt_customer_itemcd()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool only_chkecked_lotno_and_customer_itemcd()
        {
            bool result = false;
            if (chk_lotno.Checked && !chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_item.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool checkInput_checkbox_check_anyone_must()
        {
            bool result = true;
            if (!chk_lotno.Checked && !chk_manf_dt_frm_to.Checked && !chk_customer.Checked && !chk_item.Checked)
            {
                result = false;
            }
            return result;
        }

        public bool checkInput_checkbox_check_all()
        {
            bool result = true;
            if (!chkExclude.Checked)
            {
                result = false;
            }
            else if (!chk_lotno.Checked)
            {
                result = false;
            }
            else if (!chk_manf_dt_frm_to.Checked)
            {
                result = false;
            }
            else if (!chk_customer.Checked)
            {
                result = false;
            }
            else if (!chk_item.Checked)
            {
                result = false;
            }
            return result;
        }

        private void chk_lotno_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_lotno.Checked)
            {
                if (txt_lotno.Text == "0000000" || txt_lotno.Text == string.Empty)
                {
                    chk_lotno.Checked = false;
                    MessageBox.Show("Must Enter Lot Number..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_lotno.Focus();
                    return;
                }
                else if (txt_lotno_frm.Text == string.Empty)
                {
                    chk_lotno.Checked = false;
                    MessageBox.Show("Must Enter Lot Number From..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_lotno_frm.Focus();
                    return;
                }
                else if (txt_lotno_to.Text == string.Empty)
                {
                    chk_lotno.Checked = false;
                    MessageBox.Show("Must Enter Lot Number To..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_lotno_to.Focus();
                    return;
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to close this form ?", "CLOSE SHIPPING", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void chk_customer_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_customer.Checked)
            {
                if (txtCustomerCode.Text == "000000" || txtCustomerCode.Text == string.Empty)
                {
                    chk_customer.Checked = false;
                    MessageBox.Show("Must Choose Customer Code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerCode.Focus();
                    return;
                }
                else if (txtCustomerNameF.Text == string.Empty)
                {
                    chk_customer.Checked = false;
                    MessageBox.Show("Must Choose Customer Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerNameF.Focus();
                    return;
                }

            }
        }

        private void chk_item_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_item.Checked)
            {
                if (textItemCode.Text == "000000" || textItemCode.Text == string.Empty)
                {
                    chk_item.Checked = false;
                    MessageBox.Show("Must Choose Item Code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textItemCode.Focus();
                    return;
                }
                else if (txt_itemname.Text == string.Empty)
                {
                    chk_item.Checked = false;
                    MessageBox.Show("Must Choose Item Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_itemname.Focus();
                    return;
                }
            }
        }

        private void btn_execute_Click(object sender, EventArgs e)
        {
            try
            {
                if (CommonClass.shipping_update_lotno.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to Execute ?", "EXECUTE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                   
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.Default;
                        string ActionType = "shipment_date_upt";
                        bool shipment_result = false;
                        foreach (var item in CommonClass.shipping_update_lotno)
                        {

                            if (item.pk_p3 != null && item.pk_p3 != "-")
                            {
                                string[] pk_p3id = item.pk_p3.Split(',');
                                foreach (var split in pk_p3id)
                                {
                                    string[] str_view = { "@lotnum", "@lotno_child_frm", "@pk_p3", "@shipmentdate", "@shipmt_lotinfoms_only_flg", "@ActionType" };
                                    string[] obj_view = { item.lotno, item.lotno_from, split, date_shipment_date.Value.ToString("yyyy-MM-dd"), "No", ActionType };
                                    MySqlDataReader shipment_updated = helper.GetReaderByCmd("shipment_update", str_view, obj_view);
                                    if (shipment_updated.Read())
                                    {
                                        shipment_result = true;
                                    }
                                    else
                                    {
                                        shipment_result = false;
                                    }
                                    shipment_updated.Close();
                                    helper.CloseConnection();
                                }
                            }
                            else
                            {
                                string[] str_view = { "@lotnum", "@lotno_child_frm", "@pk_p3", "@shipmentdate", "@shipmt_lotinfoms_only_flg", "@ActionType" };
                                string[] obj_view = { item.lotno, item.lotno_from, string.Empty, date_shipment_date.Value.ToString("yyyy-MM-dd"), "Yes", ActionType };
                                MySqlDataReader shipment_updated = helper.GetReaderByCmd("shipment_update", str_view, obj_view);
                                if (shipment_updated.Read())
                                {
                                    shipment_result = true;
                                }
                                else
                                {
                                    shipment_result = false;
                                }
                                shipment_updated.Close();
                                helper.CloseConnection();
                            }


                        }
                        if (shipment_result)
                        {
                            MessageBox.Show("Shipment Date Updated successfully....", "UPDATE SHIPMENT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbl_noboxsp.Text = "0";
                            lbl_totalqty.Text = "0";
                            cmb_box_ship.SelectedIndex = -1;
                            cmb_box_ship.Text = "Choose";
                        }
                        DataTable dt = new DataTable();
                        dataGridView2.DataSource = dt;
                        dataGridView2.DataSource = null;
                        CommonClass.shipping_update_lotno = new List<shippingUpdate>();
                        selected_checkbox_method();                       
                        Cursor.Current = Cursors.Default;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Atleast one Checked the Lot Number....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridView2.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_execute_Click", ex);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.RefreshEdit();
        }

        private void btn_selectall_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Select All ?", "SELECT DATA'S", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool flag = false;          
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
                        if (chk.Value == chk.TrueValue)
                        {
                            chk.Value = chk.FalseValue;
                        }
                        else
                        {
                            chk.Value = chk.TrueValue;
                        }
                        chk.Value = !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
                        if (Convert.ToBoolean(chk.Value))
                        {
                            if (Convert.ToString(row.Cells["Lotno"].Value) == string.Empty || Convert.ToString(row.Cells[11].Value) == string.Empty || Convert.ToString(row.Cells[12].Value) == string.Empty)
                            {
                                flag = true;
                            }
                            if (!flag)
                            {
                                shippingUpdate model = new shippingUpdate();
                                string lotnoandchild = row.Cells[0].Value.ToString();
                                model.lotno = lotnoandchild.Split('-')[0];
                                model.lotno_from = lotnoandchild.Split('-')[1];
                                model.pk_p3 = row.Cells[11].Value.ToString();
                                model.pk_lotinfo_ms = row.Cells[12].Value.ToString();
                                CommonClass.shipping_update_lotno.Add(model);
                            }
                            else
                            {
                                chk.Value = chk.FalseValue;
                            }

                        }
                        else if (!Convert.ToBoolean(chk.Value))
                        {
                            string lotnoandchild = row.Cells[0].Value.ToString();
                            string lotnu = lotnoandchild.Split('-')[0];
                            string lotno_from = lotnoandchild.Split('-')[1];
                            CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotnu && x.lotno_from == lotno_from);
                            CommonClass.shipping_update_lotno.Distinct().ToList();
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_selectall_Click", ex);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool flag = false;
                if (dataGridView2.CurrentCell.ColumnIndex.Equals(1) && e.RowIndex != -1)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (e.RowIndex < 0)
                    {
                        return;
                    }
                    int rowIndex = e.RowIndex;
                    DataGridViewRow row = dataGridView2.Rows[rowIndex];
                    DataGridViewCheckBoxCell cell = this.dataGridView2.CurrentCell as DataGridViewCheckBoxCell;
                    
                    if (Convert.ToString(row.Cells[0].Value) == string.Empty)
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        string lotnoandchild_null_check = row.Cells[0].Value.ToString();
                        if (lotnoandchild_null_check != null)
                        {
                            if (cell != null && !cell.ReadOnly)
                            {
                                cell.Value = cell.Value == null || !((bool)cell.Value);
                                this.dataGridView2.RefreshEdit();
                                this.dataGridView2.NotifyCurrentCellDirty(true);
                            }
                            if (Convert.ToBoolean(cell.Value))
                            {
                                shippingUpdate model = new shippingUpdate();
                                string lotnoandchild = row.Cells[0].Value.ToString();
                                model.lotno = lotnoandchild.Split('-')[0];
                                model.lotno_from = lotnoandchild.Split('-')[1];
                                if (Convert.ToString(row.Cells[11].Value) != string.Empty)
                                {
                                    model.pk_p3 = row.Cells[11].Value.ToString();
                                }

                                model.pk_lotinfo_ms = row.Cells[12].Value.ToString();
                                CommonClass.shipping_update_lotno.Add(model);
                            }
                            else if (!Convert.ToBoolean(cell.Value))
                            {
                                string lotnoandchild = row.Cells[0].Value.ToString();
                                string lotnu = lotnoandchild.Split('-')[0];
                                string lotno_from = lotnoandchild.Split('-')[1];
                                CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotnu && x.lotno_from == lotno_from);
                                CommonClass.shipping_update_lotno.Distinct().ToList();
                            }
                        }
                    }
                    else
                    {
                        row.Cells[1].Value = CheckState.Unchecked;                
                    }


                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("dataGridView2_CellContentClick", ex);
            }
        }

        private void txt_lotno_Leave(object sender, EventArgs e)
        {
            if (txt_lotno.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_lotno.Text);
                txt_lotno.Text = formate_type.ToString("D7");
            }
        }

        private void txt_lotno_frm_Leave(object sender, EventArgs e)
        {
            if (txt_lotno_frm.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_lotno_frm.Text);
                txt_lotno_frm.Text = formate_type.ToString("D2");
            }
        }

        private void txt_lotno_to_Leave(object sender, EventArgs e)
        {
            if (txt_lotno_to.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_lotno_to.Text);
                txt_lotno_to.Text = formate_type.ToString("D2");
            }
        }

        private void btn_shipping_dwn_Click(object sender, EventArgs e)
        {
            try
            {               
               
                    Cursor.Current = Cursors.WaitCursor; 
                   DialogResult dialogResult = MessageBox.Show("Do you want to Download Shipment Details ?", "DOWNLOAD SHIPMENT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        if (dataGridView1.Rows.Count > 0)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            List<string> Date_column_names = new List<string>();
                            List<int> Date_column_index = new List<int>();
                            Date_column_names.Add("Shipment Date");
                            Date_column_names.Add("Manufacturing Date");
                            Date_column_names.Add("Expiry Date");
                            Date_column_names.Add("Process Date");
                            Date_column_names.Add("Planting Date");                    
                         
                            Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                        
                            Excel.Range oRng;
                            Excel._Workbook oWB;
                            Excel._Worksheet ws;
                            XcelApp.DisplayAlerts = false;
                            oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                            ws = (Excel._Worksheet)oWB.ActiveSheet;
                            int top_i = 11;
                            // Column Header 1 
                            List<ObjColumns> array = new List<ObjColumns>();
                            array.Add(new ObjColumns("A1", "I1"));                       
                            oRng = ws.get_Range("A1", "I1");
                            oRng.Value2 = "TERMINAL BOARD INFO";
                            oRng.Merge(Missing.Value);
                            foreach (var topheader in CommonClass.Process_name_gridbind_columns_shipping)
                            {                               
                                if (topheader.ProcessNames != "TERMINAL BOARD INFO")
                                {
                                    if (topheader.PaternType == "1")
                                    {
                                        Excel.Range d1 = ws.Cells[1, top_i];
                                        top_i = top_i + 4;
                                        Excel.Range d2 = ws.Cells[1, top_i];
                                        oRng = (Excel.Range)ws.get_Range(d1, d2);
                                        oRng.Value2 = topheader.ProcessNames;
                                        oRng.Merge(Missing.Value);
                                    }
                                    else if (topheader.PaternType == "2")
                                    {
                                        Excel.Range d1 = ws.Cells[1, top_i];
                                        top_i = top_i + 3;
                                        Excel.Range d2 = ws.Cells[1, top_i];
                                        oRng = (Excel.Range)ws.get_Range(d1, d2);
                                        oRng.Value2 = topheader.ProcessNames;
                                        oRng.Merge(Missing.Value);
                                    }
                                    else if (topheader.PaternType == "3")
                                    {
                                        Excel.Range d1 = ws.Cells[1, top_i];
                                        top_i = top_i + 1;
                                        Excel.Range d2 = ws.Cells[1, top_i];
                                        oRng = (Excel.Range)ws.get_Range(d1, d2);
                                        oRng.Value2 = topheader.ProcessNames;
                                        oRng.Merge(Missing.Value);
                                    }
                                    else if (topheader.PaternType == "4")
                                    {
                                        Excel.Range d1 = ws.Cells[1, top_i];
                                        top_i = top_i + 2;
                                        Excel.Range d2 = ws.Cells[1, top_i];
                                        oRng = (Excel.Range)ws.get_Range(d1, d2);
                                        oRng.Value2 = topheader.ProcessNames;
                                        oRng.Merge(Missing.Value);
                                    }
                                }
                                else
                                {
                                    Excel.Range c1 = ws.Cells[1,6];
                                    top_i = top_i + 8;
                                    Excel.Range c2 = ws.Cells[1, top_i];
                                    oRng = (Excel.Range)ws.get_Range(c1, c2);
                                    oRng.Value2 = topheader.ProcessNames;
                                    oRng.Merge(Missing.Value);
                                }
                                top_i++;
                            }
                            // Column Header 2
                            int get_date_column = 0;
                            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                            {
                                XcelApp.Cells[2, 1] = "LotNo.";
                                XcelApp.Cells[2, 2] = "Customer Code";
                                if (!Date_column_names.Contains(dataGridView1.Columns[i - 1].HeaderText))
                                {
                                    XcelApp.Cells[2, i + 2] = dataGridView1.Columns[i - 1].HeaderText;
                                }
                                else if (Date_column_names.Contains(dataGridView1.Columns[i - 1].HeaderText))
                                {
                                    XcelApp.Cells[2, i + 2] = dataGridView1.Columns[i - 1].HeaderText;
                                    Date_column_index.Add(get_date_column);
                                }
                                get_date_column++;
                            }
                            // Row header 
                            for (int i = 1; i < dataGridView1.Rows.Count + 1; i++)
                            {
                                XcelApp.Cells[i + 2, 1] = dataGridView1.Rows[i - 1].HeaderCell.Value.ToString();
                            }
                            // Row general details 
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                                {
                                    if (Convert.ToString(dataGridView1.Rows[i].Cells[j].Value) != string.Empty)
                                    {
                                        if (!Date_column_index.Contains(j))
                                        {
                                            XcelApp.Cells[i + 3, j + 3] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                        }
                                        else if (Date_column_index.Contains(j))
                                        {
                                        Excel.Range d1 = ws.Cells[i + 3, j + 3];
                                        Excel.Range d2 = ws.Cells[i + 3, j + 3];
                                        
                                        XcelApp.Range[d1, d2].EntireColumn.NumberFormat = "dd-MM-yyyy";

                                        if (Convert.ToString(dataGridView1.Rows[i].Cells[j].Value) != string.Empty && Convert.ToString(dataGridView1.Rows[i].Cells[j].Value) != "-")
                                            {
                                                string date_val = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                                DateTimePicker dt = new DateTimePicker();
                                           
                                            dt.Value = Convert.ToDateTime(date_val,
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                            DateTime convertformateDate = Convert.ToDateTime(date_val.Replace("\"", ""), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                         
                                    
                                            XcelApp.Cells[i + 3, j + 3] = convertformateDate;
                                            }
                                            else
                                            {
                                                XcelApp.Cells[i + 3, j + 3] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                            }
                                       }
                                    }
                                    else
                                    {
                                        XcelApp.Cells[i + 3, j + 3] = string.Empty;
                                    }
                                }
                            }
                        Excel.Range copyRange_B = XcelApp.Range["B:B"];             
                        Excel.Range DeleteRange_D = XcelApp.Range["A:A"];
                        Excel.Range DeleteRange_F = XcelApp.Range["H:H"];

                        DeleteRange_D.Delete();
          
                        copyRange_B.Delete();
                        DeleteRange_F.Delete();

                        // Auto fit automatically adjust the width of columns of Excel  in givien range .  
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView1.Rows.Count, dataGridView1.Columns.Count]].EntireColumn.AutoFit();
                            XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView1.Columns.Count + 5]].Font.Bold = true;
                            XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[dataGridView1.Columns.Count + 5]].Font.Bold = true;
                            XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dataGridView1.Columns.Count]].Font.Size = 13;
                            XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[1, dataGridView1.Columns.Count]].Font.Size = 12;
                            XcelApp.Columns.Borders.Color = Color.Black;
                            XcelApp.Columns.AutoFit();
                            XcelApp.Visible = true;
                            DateTime current_date = DateTime.Now;
                            DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                           
                        string CreateFolder = "C:\\TMPS";
                        
                        CheckDirectory(CreateFolder);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Shipping Details -" + datetime;
                        string newFileName = CreateFolder + compinepath;
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
        private void btnunselect_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView1.RowCount>=1)
                {
                    string AlertMessage = string.Empty;
                    if (cmb_box_ship.SelectedIndex != -1)
                    {
                        if (cmb_box_ship.SelectedIndex == 0)
                        {
                            AlertMessage = "Do you want to Select All ?";
                        }
                        else if (cmb_box_ship.SelectedIndex == 1)
                        {
                            AlertMessage = "Do you want to Select Top "+txtNoboxcount.Text+" rows ?";
                        }
                        DialogResult dialogResult = MessageBox.Show(AlertMessage, "SELECT DATA'S", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            bool flag = false;               
                            shipped_qty = 0;
                            NoOfQty = 0;
                            checkall_NoOfQty = 0;
                            lbl_noboxsp.Text = "0";
                            lbl_totalqty.Text = "0";
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[7];

                                if (btnunselect.Text == "Select Box [F4]")
                                {
                                    // Select ALL
                                    if (cmb_box_ship.SelectedIndex == 0)
                                    {
                                        if (!Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                        {
                                            dataGridView1.Rows[row.Index].Cells[7].Value = true;
                                        }
                                        else if (Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                        {
                                            dataGridView1.Rows[row.Index].Cells[7].Value = true;
                                        }                                       
                                    }
                                    // No. of box only selected 
                                    else if (cmb_box_ship.SelectedIndex == 1)
                                    {
                                        if (row.Index <= Convert.ToInt32(txtNoboxcount.Text) - 1)
                                        {
                                            if (!Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                            {
                                                dataGridView1.Rows[row.Index].Cells[7].Value = true;
                                                checkall_NoOfQty += Convert.ToInt32(dataGridView1.Rows[row.Index].Cells[12].Value);
                                            }
                                            else if (Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                            {
                                                dataGridView1.Rows[row.Index].Cells[7].Value = true;
                                                checkall_NoOfQty += Convert.ToInt32(dataGridView1.Rows[row.Index].Cells[12].Value);
                                            }
                                            if (dataGridView1.RowCount >= 1)
                                            {
                                                shipped_qty = row.Index + 1;
                                                lbl_noboxsp.Text = shipped_qty.ToString();
                                                lbl_totalqty.Text = checkall_NoOfQty.ToString();                                           
                                                btnunselect.Text = "Select Box [F4]";
                                            }
                                        }
                                        else
                                        {
                                         
                                            if (!Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                            {
                                                dataGridView1.Rows[row.Index].Cells[7].Value = false;
                                            }
                                            else if (Convert.ToBoolean(row.Cells[7].EditedFormattedValue))
                                            {
                                                dataGridView1.Rows[row.Index].Cells[7].Value = false;
                                            }                                            
                                        }
                                    }

                                }
                                this.dataGridView1.RefreshEdit();
                                this.dataGridView1.NotifyCurrentCellDirty(true);

                                if (Convert.ToBoolean(chk.Value))
                                {
                                    shippingUpdate model = new shippingUpdate();
                                    string lotnoandchild = row.HeaderCell.Value.ToString();
                                    if (!already_exits_shipment_lotnochild_selectall.Contains(lotnoandchild))
                                    {
                                        model.lotno_child = lotnoandchild;
                                        model.lotno = lotnoandchild.Split('-')[0];
                                        model.lotno_from = lotnoandchild.Split('-')[1];
                                        CommonClass.shipping_update_lotno.Add(model);
                                        already_exits_shipment_lotnochild_selectall.Add(model.lotno_child);
                                     
                                    }
                           
                                }
                                else if (!Convert.ToBoolean(chk.Value))
                                {
                                    string lotnoandchild = row.HeaderCell.Value.ToString(); 
                                    string lotnu = lotnoandchild.Split('-')[0];
                                    string lotno_from = lotnoandchild.Split('-')[1];
                                    CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotnu && x.lotno_from == lotno_from);
                                    CommonClass.shipping_update_lotno.Distinct().ToList();                           
                                }
                            }
                            if (cmb_box_ship.SelectedIndex == 0)
                            {
                                lbl_totalqty.Text = (from DataGridViewRow rows in dataGridView1.Rows
                                                     select Convert.ToInt32(rows.Cells[12].FormattedValue)).Sum().ToString();
                                lbl_noboxsp.Text = (from DataGridViewRow rows in dataGridView1.Rows
                                                    select Convert.ToInt32(rows.Cells[0].FormattedValue)).Count().ToString();
                            }
                            else if (cmb_box_ship.SelectedIndex == 0)
                            {
                                // selected count
                                lbl_noboxsp.Text = checkall_NoOfQty.ToString();
                                // total qty 
                            }
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Must choose any of one No. of Box to select field.. !!!", "Info"); txtCustomerCode.Focus();
                        cmb_box_ship.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("No Records found.. !!!", "Info"); txtCustomerCode.Focus();
                    cmb_box_ship.Focus();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnunselect_Click", ex);
            }
        }

        private void txtCustomerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtCustomerCode.Text != "" && txtCustomerCode.Text != "0000000")
            {                
                    DataSet ds = helper.GetDatasetByClientcodeNames(txtCustomerCode.Text, string.Empty);
                    DataTable dtbl = new DataTable();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dtbl = ds.Tables[0];
                        txtCustomerNameF.Text = dtbl.Rows[0]["fullname"].ToString();
                        chk_customer.Checked = true;
                        helper.CloseConnection();
                    }
            }
        }

        private void textItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtCustomerCode.Text != "" && txtCustomerCode.Text != "000000")
                {
                    if (textItemCode.Text != "" && textItemCode.Text != "000000")
                    {
                        DataSet dset = helper.GetDatasetByClientcodeNames(txtCustomerCode.Text, string.Empty);
                        DataTable dtbl = new DataTable();

                        string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
                        string[] obj = { txtCustomerCode.Text, string.Empty, textItemCode.Text, "GetDataCustomerItem" };
                        dset = helper.GetDatasetByCommandString("product_view", str, obj);
                        if (dset.Tables[0].Rows.Count > 0)
                        {
                            dtbl = dset.Tables[0];
                            txt_itemname.Text = dtbl.Rows[0]["itemname"].ToString();
                            chk_item.Checked = true;
                            helper.CloseConnection();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Customer Code is Null");
                    txtCustomerCode.Focus();
                }

            }
        }
        public void insert_lotinfo_value_assign_gridbind(string ActionTypeTwo, string lotn, string lotn_frm, string lotn_to, string manf_dt_frm, string manf_dt_to, string customer_code, string item_code, string auctionrole,string sp_name,string common_cust_name,string common_item_name)
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();

                // lot information grid data's
                // p1     
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to","@Customercd", "@proc_id", "@itmcd" , "@Actionrole", "@Actionroletwo" };
                string[] obj_p1 = { ActionType_p1, lotn,lotn_frm, lotn_to, manf_dt_frm, manf_dt_to,customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };

               
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
              
                if (dataGridView1.Rows.Count >= 0)
                {
                    if (list_cmodel.Count > 1)
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
                        foreach(var rowheader in get_lotnumber)
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
                            Console.WriteLine(rowheader);
                        }
                        this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dataGridView1.EnableHeadersVisualStyles = false;
                        dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;
                        dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    }
                    //090823row_header_lotno_all_combined = new List<string>();
                    
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
                                                                      
                                    foreach (var itm in CommonClass.Process_name_gridbind_columns_shipping_runtime)
                                    {
                                        string patern_type_list = itm.PaternType;
                                        string processId = itm.process_id;                                      
                                        
                                        if (itm.ProcessNames == chk && itm.materialcode == item.material_code.Split(',')[chk_index])
                                        {
                                            if(patern_type_list == "4" && processId == "108")
                                            {
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if (patern_type_list == "4" && processId == "109")
                                            {
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if(patern_type_list == "4" && processId == "110")
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
                                                row.Cells[0].Value = row_index+1;
                                                row.Cells[1].Value = item.lotnojoin.Split(',')[0];
                                                row.Cells[2].Value = item.tb_bproduct.Split(',')[0];
                                            
                                                row.Cells[3].Value = item.onhold.Split(',')[0];
                                                row.Cells[4].Value = item.scrap.Split(',')[0];
                                                row.Cells[5].Value = item.reason_hs.Split(',')[0];
                                                if (!string.IsNullOrEmpty(item.shipment_date))
                                                {
                                                    shipment_date = item.shipment_date.Split(',')[0];
                                                    row.Cells[6].Value = shipment_date;
                                                    dataGridView1.Rows[row_index].DefaultCellStyle.BackColor = Color.LightGray;
                                                    dataGridView1.Rows[row_index].Cells[7].Value = false;
                                                    dataGridView1.Rows[row_index].Cells[7].ReadOnly = true;
                                                
                                                    
                                                }
                                                else
                                                {
                                                    shipment_date = "-";
                                                    row.Cells[6].Value = shipment_date;
                                                    dataGridView1.Rows[row_index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                                   
                                                    dataGridView1.Rows[row_index].Cells[7].ReadOnly = true;
                                           
                                                }
                                             
                                                row.Cells[8].Value = item.customer_code;
                                                row.Cells[9].Value = item.customer_name;
                                                row.Cells[10].Value = item.item_code;
                                                row.Cells[11].Value = item.item_name;
                                                row.Cells[12].Value = item.tb_qty.Split(',')[chk_index];
                                                string sumOfQty = item.tb_qty.Split(',')[chk_index];
                                                if(sumOfQty!=string.Empty)
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
                                                row.Cells[13].Value = item.tb_manuf_dt.Split(',')[chk_index];
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
        private void LoadDataGrid()
        {
            try
            {
                dataGridView1.DataSource = null;
                int total_process = CommonClass.Process_name_gridbind_shipping.Count;
               
                // grid bind start
                int totalgroup = total_process;
               
                daysInMonths = new int[totalgroup]; // check line 129
                GroupLabel = new string[totalgroup];
                LabelString = new string[totalgroup, 15];
                LabelSize = new int[totalgroup, 15];
                List<KeyValuePair<int, string>> kvpList = new List<KeyValuePair<int, string>>();

                int i = 0;
                this.dataGridView1.Columns.Clear();
                foreach (var itm in CommonClass.Process_name_gridbind_shipping)              
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
                    
                    Console.WriteLine(col);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.RefreshEdit();
        }

       
        public void terminal_addlist_loadgrid_call(string ActionType)
        {            
            CommonClass.Process_name_gridbind_shipping = new List<PI_Process>();
            PI_Process models = new PI_Process();
            models.id = "XXX";
            models.ProcessNames = "TERMINAL BOARD INFO";
            models.PaternType = "5";
            models.process_id = "0";
            CommonClass.Process_name_gridbind_shipping.Add(models);
            DataSet dset = new DataSet();

            dset = helper.GetDatasetByBOMView_Pro_input_shipment(txtCustomerCode.Text, textItemCode.Text, ActionType);
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
                    CommonClass.Process_name_gridbind_shipping.Add(model);
                    i++;
                }
                CommonClass.Process_name_gridbind_columns_shipping.AddRange(CommonClass.Process_name_gridbind_shipping);
            }
            LoadDataGrid();
        }
        public void terminal_addlist_loadgrid_call_loop(string ActionType, string custcd, string itemcd)
        {
            CommonClass.Process_name_gridbind_shipping_runtime = new List<PI_Process>();
            CommonClass.Process_name_gridbind_columns_shipping_runtime = new List<PI_Process>();
            PI_Process models = new PI_Process();
            models.id = "XXX";
            models.ProcessNames = "TERMINAL BOARD INFO";
            models.PaternType = "5";
            models.process_id = "0";
            CommonClass.Process_name_gridbind_shipping_runtime.Add(models);
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
                    CommonClass.Process_name_gridbind_shipping_runtime.Add(model);
                    i++;
                }
                CommonClass.Process_name_gridbind_columns_shipping_runtime.AddRange(CommonClass.Process_name_gridbind_shipping_runtime);
            }
        }
        public void terminal_addlist_loadgrid_call_loop_header(string ActionType,string custcd,string itemcd)
        {
            CommonClass.Process_name_gridbind_shipping_runtime = new List<PI_Process>();
            CommonClass.Process_name_gridbind_columns_shipping_runtime = new List<PI_Process>();

            CommonClass.Process_name_gridbind_shipping_runtime_filter = new List<PI_Process>();


            PI_Process models = new PI_Process();
            models.id = "XXX";
            models.ProcessNames = "TERMINAL BOARD INFO";
            models.PaternType = "5";
            models.process_id = "0";
            if (already_exits_shipment_process_id_header.Contains(models.process_id) == false)
            {
                CommonClass.Process_name_gridbind_shipping_runtime_filter.Add(models);
                already_exits_shipment_process_id_header.Add(models.process_id);
            }
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
                    if(already_exits_shipment_process_id_header.Contains(model.process_id) ==false)
                    {
                        CommonClass.Process_name_gridbind_shipping_runtime_filter.Add(model);
                        already_exits_shipment_process_id_header.Add(model.process_id);
                    }
                    
                    i++;
                }
                CommonClass.Process_name_gridbind_columns_shipping_runtime_filter.AddRange(CommonClass.Process_name_gridbind_shipping_runtime_filter);
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
       

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex < 0)
                {
                    return;
                }
                int rowIndex = e.RowIndex;
                // use pattern popup open
                selected_delete_lotno = dataGridView1.CurrentRow.HeaderCell.Value.ToString();
                selected_date_checking_three_month_b4 = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString();
                DataTable dt = new DataTable();
                dataGridView2.DataSource = dt;              
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("dataGridView1_RowHeaderMouseClick", ex);
            }
        }

        private void btn_delete_lotno_Click(object sender, EventArgs e)
        {
            try
            {                
                if (!string.IsNullOrEmpty(selected_delete_lotno) && !string.IsNullOrEmpty(selected_date_checking_three_month_b4))
                {
                    if(selected_date_checking_three_month_b4 != "-")
                    {
                        Cursor.Current = Cursors.Default;
                        DateTime compare_date = DateTime.Parse(selected_date_checking_three_month_b4);
                        DateTime Result = compare_date.AddMonths(+3);
                        int grater_than = DateTime.Compare(Result, nowdate);
                        if (grater_than >= 0)
                        {
                            string ActionType = "shipment_date_upt";
                            DialogResult dialogResult = MessageBox.Show("Do you want to Delete the Lot No. " + selected_delete_lotno + " ?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                string[] str_view = { "@lotnum", "@lotno_child_frm", "@ActionType" };
                                string[] obj_view = { selected_delete_lotno.Split('-')[0], selected_delete_lotno.Split('-')[1], ActionType };
                                MySqlDataReader shipment_updated = helper.GetReaderByCmd("shipment_update_delete_lotno", str_view, obj_view);
                                if (shipment_updated.Read())
                                {
                                    selected_delete_lotno = string.Empty;
                                    selected_date_checking_three_month_b4 = string.Empty;
                                    MessageBox.Show("Shipping Details Deleted Successfully..", "DELETE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dataGridView2.Focus();
                                }
                                else
                                {
                                    selected_delete_lotno = string.Empty;
                                    selected_date_checking_three_month_b4 = string.Empty;
                                }
                                shipment_updated.Close();
                                helper.CloseConnection();                     
                                DataTable dt = new DataTable();
                                dataGridView2.DataSource = dt;
                                dataGridView2.DataSource = null;
                                CommonClass.shipping_update_lotno = new List<shippingUpdate>();
                                selected_checkbox_method();
                            }
                            
                        }
                        else
                        {

                            MessageBox.Show("Shipped date is more than three months so not allow to delete....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView2.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Shipped date is null so not allow to delete....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);                 
                        dataGridView2.Focus();

                    }
                }
                else
                {
                    MessageBox.Show("Atleast one Checked the Lot Number Row Header....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridView2.Focus();
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("btn_delete_lotno_Click", ex);
            }
        }

        private void cmb_box_ship_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_box_ship.SelectedIndex==1)
            {
                txtNoboxcount.Enabled = true;
            }
            else
            {
                txtNoboxcount.Enabled = false;
                txtNoboxcount.Text = "99";
            }
        }

        private void txtNoboxcount_Leave(object sender, EventArgs e)
        {
            if (txtNoboxcount.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtNoboxcount.Text);
                txtNoboxcount.Text = formate_type.ToString("D2");
            }
        }

        private void btn_nextPg_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
              //090823  dataGridView1.ClearSelection();
                dataGridView1.Refresh();
                int cPageNo = CommonClass.shipping_curentPageNo_nxtPg + 1;
                var Get_records = CommonClass.Runtime_Store_Shipping_details.ToPagedList(cPageNo, PageSize);
                CommonClass.shipping_curentPageNo_nxtPg = Get_records.PageNumber;
                
                if (Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = true;
                }
                else if (!Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = false;
                }               
                foreach (var get_cd in Get_records)
                {
                    terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
 
                    insert_lotinfo_value_assign_gridbind(CommonClass.shipping_actionTyp1_nxtPg, get_cd.lotno, CommonClass.shipping_lotno_child_frm_nxtPg, CommonClass.shipping_lotno_child_to_nxtPg, CommonClass.shipping_manfdt_frm_nxtPg, CommonClass.shipping_manfdt_to_nxtPg, get_cd.customer_code, get_cd.item_code, CommonClass.shipping_actionTyp2_nxtPg, CommonClass.shipping_spname_nxtPg, get_cd.customer_name, get_cd.item_name);

                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_nextPg_Click", ex);
            }
        }
    }
}
