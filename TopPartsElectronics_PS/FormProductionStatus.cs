using MySql.Data.MySqlClient;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopPartsElectronics_PS.Helper;
using YourApp.Data;
using static TopPartsElectronics_PS.Helper.GeneralModelClass;
using Excel = Microsoft.Office.Interop.Excel;

namespace TopPartsElectronics_PS
{
    public partial class FormProductionStatus : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        public bool shipment_date_already_get = false;
        public string selected_customer_code = string.Empty;
        List<Lotinfo_gridbind_common_pattern> lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
        List<string> already_exits_row_header = new List<string>();
        List<string> already_exits_row_header_lotno_only = new List<string>();
        List<string> row_header_lotno_all_combined = new List<string>();
        List<string> only_expiry_datas = new List<string>();
        List<string> only_expiry_datas_grid_1 = new List<string>();
        List<string> only_expiry_datas_row_lotnojoin = new List<string>();
        List<string> only_expiry_datas_row_lotnojoin_gridview_1 = new List<string>();
        List<int> only_expiry_datas_row_index = new List<int>();
        List<int> only_expiry_datas_row_index_grid_1 = new List<int>();
        List<string> already_exits_row_columns = new List<string>();
        List<string> already_exits_shipment_lotnochild = new List<string>();
        private int[] daysInMonths;
        private string[] GroupLabel;
        private string[,] LabelString;
        private int[,] LabelSize;
        //
        private int[] daysInMonths_d3;
        private string[] GroupLabel_d3;
        private string[,] LabelString_d3;
        private int[,] LabelSize_d3;
        int PageNumber = 1;
        int PageSize = 15;
        IPagedList<productlist> list;
        public bool shipment_gridbind_with_shpfilter_dataLoad = false;
        public bool shipment_gridbind_dataLoad = false;
        public FormProductionStatus()
        {
            InitializeComponent();
        }
        private void FormProductionStatus_Load(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 2;
                this.dataGridView3.ColumnHeadersHeight = this.dataGridView3.ColumnHeadersHeight * 2;
                dateTimePicker_ship_to.Value = DateTime.Today.AddDays(1);
                dateTimePicker_ship_frm.Value = DateTime.Today.AddDays(1);
                date_manf_frm.Value = DateTime.Today.AddDays(-1);
                date_manf_to.Value = DateTime.Today.AddDays(-1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FormProductionStatus_Closing(object sender, FormClosingEventArgs e)
        {
            ((Form1)MdiParent).productionStatusToolStripMenuItem.Enabled = true;
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                FormSearchClient frm = new FormSearchClient();
                MysqlHelper.call_from_ProductionStatus_to_client = true;
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetSearchClientId(string code, string shortname, string fullname)
        {
            txtCustomerCode.Text = code;
            txtCustomerNameS.Text = fullname;
            chk_customer.Checked = true;
        }

        public void SetSearchId_Item(string customercode, string itemcode, string fullname)
        {
            textItemCode.Text = itemcode;
            textItemName.Text = fullname;
            chk_item.Checked = true;
        }

        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormSearchItem frm = new FormSearchItem();
                MysqlHelper.call_from_ProductionStatus_to_item = true;
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.CustomerCode = txtCustomerCode.Text;
                frm.CustomerNames = txtCustomerNameS.Text;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chkExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExclude.Checked == false)
            {
                dateTimePicker_ship_frm.Enabled = true;
                lbl_ship_dt.Enabled = true;
                dateTimePicker_ship_to.Enabled = true;
                chk_shiped_frm.Checked = true;
                chk_shiped_frm.Enabled = true;
            }
            else if (chkExclude.Checked == true)
            {
                dateTimePicker_ship_frm.Enabled = false;
                lbl_ship_dt.Enabled = false;
                dateTimePicker_ship_to.Enabled = false;
                chk_shiped_frm.Checked = false;
                chk_shiped_frm.Enabled = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    //DialogResult dialogResult = MessageBox.Show("Do you want to Search ?", "SEARCH PRODCUTION_STATUS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (dialogResult == DialogResult.Yes)
                    //{
                    Cursor.Current = Cursors.WaitCursor;
                    // shipment load list 
                    CommonClass.Runtime_Store_Print_details = new List<shipping_custcd_itemcd>();
                    shipment_gridbind_with_shpfilter_dataLoad = false;
                    shipment_gridbind_dataLoad = false;
                    btn_nextPg.Enabled = true;
                    //
                    only_expiry_datas = new List<string>();
                    only_expiry_datas_grid_1 = new List<string>();
                    only_expiry_datas_row_lotnojoin = new List<string>();
                    only_expiry_datas_row_lotnojoin_gridview_1 = new List<string>();
                    only_expiry_datas_row_index = new List<int>();
                    only_expiry_datas_row_index_grid_1 = new List<int>();
                    dataGridView1.DataSource = null;
                    dataGridView1.Refresh();
                    dataGridView3.DataSource = null;
                    this.tabControl1.SelectedTab = tabPage1;
                    string ActionType = string.Empty;
                    string ActionType_ship_tab = string.Empty;
                    CommonClass.Process_name_gridbind_columns_shipping = new List<PI_Process>();
                    List<string> already_exits_row_header = new List<string>();
                    DataTable dt = new DataTable();
                    dGProductInfoList.DataSource = null;
                    textLotNoAdd.Text = string.Empty;
                    textLotNoChild.Text = string.Empty;
                    dateTimePicker_Manf.Text = nowdate.ToShortDateString();
                    txt_lotinfo_quantity.Text = string.Empty;
                  
                    string chk_2st_digit = txt_machine_no.Text;
                    string chk_1st_digit = txt_machine_no.Text.Substring(0,1); 
                    //// this is for handling negative numbers, we are only insterested in postitve number
                    //int number = Convert.ToInt16(chk_1st_digit);
                    //number = Math.Abs(number);
                    // special case for 0 as Log of 0 would be infinity
                    if(txt_machine_no.Text!="00")
                    {
                        if (chk_1st_digit == "0")
                        {
                            txt_machine_no.Text = chk_2st_digit.Substring(1, 1);
                        }
                    }
                   
                    string machine_no = "^" + txt_machine_no.Text;                
                    string round_machine_no = "^" + chk_2st_digit;
                    if (!chkExclude.Checked)
                    {
                        ActionType = "shipment_only_lotno";
                        ActionType_ship_tab = "shipment_only_lotno";
                    }
                    else if (chkExclude.Checked)
                    {
                        ActionType = "shipment_lotno_Exclude";
                        ActionType_ship_tab= "shipment_only_lotno_Exclude";
                    }
                    // only lot no..
                    if (checkInput_checkbox_check_lotno_only())
                    {
                         pstatus_gridbind(ActionType, "lotno", string.Empty, string.Empty, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_only_lotno";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "lotno", ActionType_ship_tab, sp_name, "lotno",dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"),string.Empty);
                        //call_shipment_main_filter("lotno", textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    // only machine no..
                    else if (checkInput_checkbox_check_machine_only())
                    {                                                
                        pstatus_gridbind(ActionType, "machno", string.Empty, string.Empty, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, string.Empty, string.Empty,round_machine_no);
                        // shipment list

                        dGProductInfoList.Refresh();                        
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        string sp_name = "allpattern_view_itemcode_shipment_only_machine";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_mach", ActionType_ship_tab, sp_name, "machno", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                        
                    }
                    // lotno , customer , item
                    else if (checkInput_checkbox_check_lotno_customer_item())
                    {
                        pstatus_gridbind(ActionType, "lotno_cust_item", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_lot_cust_item";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "lot_cust_item", ActionType_ship_tab, sp_name, "manfdt_custcd_itemcd");

                       // call_shipment_main_filter("lotno_cust_item", textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    //lotno , customer 
                    else if (checkInput_checkbox_check_lotno_customer())
                    {
                        pstatus_gridbind(ActionType, "lotno_cust", txtCustomerCode.Text, string.Empty, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_lotno_cust";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "lotno_cust", ActionType_ship_tab, sp_name, "lot_custcd");

                        //call_shipment_main_filter("lotno_cust", textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    // customer , item
                    else if (checkInput_checkbox_check_customer_item())
                    {
                        pstatus_gridbind(ActionType, "cust_item", txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        // shipment list
                        dGProductInfoList.Refresh();
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        string sp_name = "allpattern_view_itemcode_shipment_cust_itemcd";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "cust_item", ActionType_ship_tab, sp_name, "custcd_itemcd");

                        //call_shipment_main_filter("cust_item", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    // customer
                    else if (checkInput_checkbox_check_customer_only())
                    {
                        pstatus_gridbind(ActionType, "cust", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        // shipment list
                        dGProductInfoList.Refresh();
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        string sp_name = "allpattern_view_itemcode_shipment_cust";
                        shipment_gridbind(textLotNo.Text,txt_lotno_frm.Text,txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),txtCustomerCode.Text,textItemCode.Text,"cust", ActionType_ship_tab, sp_name, "cust");
                        //call_shipment_main_filter("cust", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    // newly
                    // shipment 
                    else if (checkInput_checkbox_check_shipmentfrm_to_only())
                    {
                        pstatus_gridbind(ActionType, "shiponly", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty, string.Empty, string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shipmentonly";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "onlyshipdt", ActionType_ship_tab, sp_name, "allchecked",dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"),string.Empty);

                        //call_shipment_main_filter("shiponly", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));

                    }
                    // all
                    else if (checkInput_checkbox_check_all())
                    {
                        pstatus_gridbind(ActionType, "all", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shipall";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_cust_item_lot_manfdt", ActionType_ship_tab, sp_name, "allchecked", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("allchecked", textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, txtCustomerCode.Text, textItemCode.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    }
                    // customer , item , lot , manf 
                    else if (checkInput_checkbox_check_customer_item_lot_manf())
                    {

                        already_exits_row_header = new List<string>();
                        already_exits_row_header_lotno_only = new List<string>();
                        row_header_lotno_all_combined = new List<string>();
                        already_exits_row_columns = new List<string>();
                        pstatus_gridbind(ActionType, "wout_ship", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_all";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "lotno_manfdt_cust", ActionType_ship_tab, sp_name, "allchecked");

                        //terminal_addlist_loadgrid_call("shipment_others");
                        //terminal_addlist_loadgrid_call_loop("GetData", txtCustomerCode.Text, textItemCode.Text);
                        //string sp_name = "allpattern_view_itemcode_shipment_all";
                        //insert_lotinfo_value_assign_gridbind(ActionType_ship_tab, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "allchecked", sp_name, txtCustomerNameS.Text, textItemName.Text);

                        //call_shipment_main_filter("lotno_cust_item", textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, txtCustomerCode.Text, textItemCode.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty, string.Empty);

                    }
                    //  ship , customer , item , lot 
                    //else if (checkInput_checkbox_check_ship_customer_item_lot())
                    //{
                    //    pstatus_gridbind(ActionType, "wout_manf", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty, string.Empty);
                    //    // shipment list
                    //    DataTable dt_ship = new DataTable();
                    //    dataGridView2.DataSource = dt_ship;
                    //    dataGridView2.DataSource = null;
                    //    call_shipment_main_filter("shiponly", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));

                    //}
                    //// ship , customer , item 
                    //else if (checkInput_checkbox_check_ship_customer_item())
                    //{
                    //    pstatus_gridbind(ActionType, "wout_lot_manf", txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty, string.Empty);
                    //    // shipment list
                    //    DataTable dt_ship = new DataTable();
                    //    dataGridView2.DataSource = dt_ship;
                    //    dataGridView2.DataSource = null;
                    //    call_shipment_main_filter("shiponly", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));

                    //}
                    // ship , customer 
                    else if (checkInput_checkbox_check_ship_customer())
                    {
                        pstatus_gridbind(ActionType, "wout_itm_lot_manf", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty, string.Empty, string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shipcust";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "shipcust", ActionType_ship_tab, sp_name, "shipcust", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);


                        //call_shipment_main_filter("shiponly_cust", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    }
                    // ship , lotno
                    else if (checkInput_checkbox_check_ship_lotno())
                    {
                        pstatus_gridbind(ActionType, "with_ship_lotno", txtCustomerCode.Text, string.Empty, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty, string.Empty, string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shiplot";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_lot", ActionType_ship_tab, sp_name, "ship_lot", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);


                        //call_shipment_main_filter("shiponly_cust", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    }
                    // lot , manf date
                    else if (checkInput_checkbox_check_lot_manf())
                    {
                        pstatus_gridbind(ActionType, "with_lot_manf", string.Empty, string.Empty, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_lot_manfdt";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "lotno_manfdt", ActionType_ship_tab, sp_name, "lotno_mdt");

                       // call_shipment_main_filter("shiponly", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    }
                    // manf date
                    else if (checkInput_checkbox_check_manf())
                    {
                        pstatus_gridbind(ActionType, "only_manf", string.Empty, string.Empty, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_lot_manfdt";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "manfdt", ActionType_ship_tab, sp_name, "lotno_mdt");

                        //call_shipment_main_filter("shiponly", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    }
                    // item
                    else if (checkInput_checkbox_check_item())
                    {
                        pstatus_gridbind(ActionType, "item", string.Empty, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView2.DataSource = dt_ship;
                        dataGridView2.DataSource = null;
                        call_shipment_main_filter("only_item", textLotNo.Text, string.Empty, string.Empty, txtCustomerCode.Text, textItemCode.Text, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));

                    }
                    // ship , manf date
                    else if (checkInput_checkbox_check_ship_manf())
                    {
                        //pstatus_gridbind(ActionType, "ship_manf", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"));
                        pstatus_gridbind(ActionType, "with_ship_manfdt", txtCustomerCode.Text, string.Empty, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"),date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shipmanfdt";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "shipmanfdt", ActionType_ship_tab, sp_name, "shipmanfdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                     //   call_shipment_main_filter("ship_manf", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    }
                    // customer , manf dt
                    else if (checkInput_checkbox_check_cust_manf())
                    {
                        pstatus_gridbind(ActionType, "cust_manf", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_only_manfdt_cust";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "manfdt_cust", ActionType_ship_tab, sp_name, "manfdt_cust");

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // ship, customer , manf dt
                    else if (checkInput_checkbox_check_ship_cust_manf())
                    {
                        pstatus_gridbind(ActionType, "ship_cust_manf", txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        // existing sp use. already we filter the data so only.
                        string sp_name = "allpattern_view_itemcode_shipment_shipmanfdt";
                        //string sp_name = "allpattern_view_itemcode_shipment_cust_manfdt";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_cust_manf", ActionType_ship_tab, sp_name, "shipmanfdt",dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"),dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // ship, customer , lotno, manf dt
                    else if (checkInput_checkbox_check_ship_cust_lot_manf())
                    {
                        pstatus_gridbind(ActionType, "ship_cust_lot_manf", txtCustomerCode.Text, textItemCode.Text,textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text,dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        // existing sp use. already we filter the data so only.
                        string sp_name = "allpattern_view_itemcode_shipment_shipmanfdt";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_cust_lot_manf", ActionType_ship_tab, sp_name, "shipmanfdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // ship, customer , lotno
                    else if (checkInput_checkbox_check_ship_cust_lot())
                    {
                        pstatus_gridbind(ActionType, "ship_cust_lot", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shipcustlot";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_cust_lot", ActionType_ship_tab, sp_name, "shipmanfdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // ship, lotno, manf dt
                    else if (checkInput_checkbox_check_ship_lot_manf())
                    {
                        pstatus_gridbind(ActionType, "ship_lot_manf", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        // existing sp use. already we filter the data so only.
                        string sp_name = "allpattern_view_itemcode_shipment_shipmanfdt";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_lot_manf", ActionType_ship_tab, sp_name, "shipmanfdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // ship, customer , item
                    else if (checkInput_checkbox_check_ship_cust_item())
                    {
                        pstatus_gridbind(ActionType, "ship_cust_item", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_shipcustlot";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_cust_item", ActionType_ship_tab, sp_name, "shipmanfdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // ship, customer , item ,lot
                    else if (checkInput_checkbox_check_ship_cust_item_lot())
                    {
                        pstatus_gridbind(ActionType, "ship_cust_item_lot", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        // existing sp use. already we filter the data so only.
                        string sp_name = "allpattern_view_itemcode_shipment_shipcustlot";
                        shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "ship_cust_item_lot", ActionType_ship_tab, sp_name, "shipmanfdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), string.Empty);

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // customer ,lot , manf dt
                    else if (checkInput_checkbox_check_customer_lot_manfdt())
                    {
                        pstatus_gridbind(ActionType, "cust_lot_manfdt", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_cust_lot_manfdt";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "cust_lot_manfdt", ActionType_ship_tab, sp_name, "cust_lot_manfdt");

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // customer ,item , manf dt
                    else if (checkInput_checkbox_check_customer_item_manfdt())
                    {
                        pstatus_gridbind(ActionType, "cust_item_manfdt", txtCustomerCode.Text, textItemCode.Text, textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), string.Empty);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_itemcode_shipment_cust_item_manfdt";
                        shipment_gridbind(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "cust_item_manfdt", ActionType_ship_tab, sp_name, "cust_lot_manfdt");

                        //call_shipment_main_filter("cust_manf", string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                    }
                    // machine sq start 
                    
                    // all with out lot no
                    else if(checkInput_checkbox_check_all_with_mno())
                    {                       
                        pstatus_gridbind(ActionType, "all_mno", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_with_machine_number";
                        //shipment_gridbind(txt_machine_no.Text, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno", ActionType_ship_tab, sp_name, "all_mno");
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno", ActionType_ship_tab, sp_name, "all_mno", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , ship dt , customer, item 
                    else if(checkInput_checkbox_check_mno_shp_cus_itm())
                    {                        
                        pstatus_gridbind(ActionType, "all_mno_shipdt_custitm", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_shipdt_custitm";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_shipdt_custitm", ActionType_ship_tab, sp_name, "all_mno_shipdt_custitm", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , ship dt , customer
                    else if (checkInput_checkbox_check_mno_shp_cus())
                    {                        
                        pstatus_gridbind(ActionType, "all_mno_shipdt_cust", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_shipdt_cust";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_shipdt_cust", ActionType_ship_tab, sp_name, "all_mno_shipdt_cust", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , ship dt 
                    else if (checkInput_checkbox_check_mno_shp())
                    {                     
                        pstatus_gridbind(ActionType, "all_mno_shipdt", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_shipdt";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_shipdt", ActionType_ship_tab, sp_name, "all_mno_shipdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , cust ,item ,manf dt 
                    else if (checkInput_checkbox_check_mno_cust_itm_manf())
                    {
                       
                        pstatus_gridbind(ActionType, "all_mno_custitm_mdt", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_custitm_mdt";                       
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_custitm_mdt", ActionType_ship_tab, sp_name, "all_mno_custitm_mdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , cust ,item  
                    else if (checkInput_checkbox_check_mno_cust_itm())
                    {
                        
                        pstatus_gridbind(ActionType, "all_mno_custitm", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_custitm";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_custitm", ActionType_ship_tab, sp_name, "all_mno_custitm", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);

                    }
                    // machine , cust
                    else if (checkInput_checkbox_check_mno_cust())
                    {                       
                        pstatus_gridbind(ActionType, "all_mno_cus", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_cust";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_cust", ActionType_ship_tab, sp_name, "all_mno_cust", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , cust , manf dt
                    else if (checkInput_checkbox_check_mno_cust_manfdt())
                    {
                        
                        pstatus_gridbind(ActionType, "all_mno_cust_mdt", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"),round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_cust_manfdt";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_cust", ActionType_ship_tab, sp_name, "all_mno_cust", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    // machine , manfdt
                    else if (checkInput_checkbox_check_mno_manfdt())
                    {
                     
                        pstatus_gridbind(ActionType, "all_mno_mdt", txtCustomerCode.Text, textItemCode.Text, machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                        // shipment list
                        DataTable dt_ship = new DataTable();
                        dataGridView3.DataSource = dt_ship;
                        dataGridView3.DataSource = null;
                        dGProductInfoList.Refresh();
                        string sp_name = "allpattern_view_machine_mdt";
                        shipment_gridbind_with_shpfilter(machine_no, txt_lotno_frm.Text, txt_lotno_to.Text, date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), txtCustomerCode.Text, textItemCode.Text, "all_mno_mdt", ActionType_ship_tab, sp_name, "all_mno_mdt", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), round_machine_no);
                    }
                    //foreach(DataGridViewRow row in dataGridView3.Rows)
                    //{
                    //    int rowIndex = row.Index;
                    //    string row_header_lotno = row.HeaderCell.Value.ToString();
                    //    if(only_expiry_datas_row_index.Contains(dataGridView3.CurrentRow.Index) ==true)
                    //    {                     
                    //        dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
                    //    }
                    //    //rowIndex++;
                    //}
                    if (chk_expirydt.Checked)
                    {
                        // start with the last row, and work towards the first
                        for (int n = dataGridView3.Rows.Count - 1; n >= 0; n--)
                        {
                            if (dataGridView3.Rows[n].HeaderCell.Value != null)
                            {
                                //if (dataGridView3.Rows[n].HeaderCell.Value.Equals(dataGridView3.Rows[m].Cells[2].Value)                          {
                                //    dataGridView3.Rows.RemoveAt(n);
                                //    //break;
                                //}
                                string row_header_lotno = dataGridView3.Rows[n].HeaderCell.Value.ToString();
                                if (only_expiry_datas_row_lotnojoin.Contains(row_header_lotno) == true)
                                {
                                    dataGridView3.Rows.RemoveAt(n);
                                }
                            }
                        }
                        for (int n = dataGridView1.Rows.Count - 1; n >= 0; n--)
                        {
                            if (dataGridView1.Rows[n].HeaderCell.Value != null)
                            {
                                //if (dataGridView3.Rows[n].HeaderCell.Value.Equals(dataGridView3.Rows[m].Cells[2].Value)                          {
                                //    dataGridView3.Rows.RemoveAt(n);
                                //    //break;
                                //}
                                string row_header_lotno = dataGridView1.Rows[n].HeaderCell.Value.ToString();
                                if (only_expiry_datas_row_lotnojoin_gridview_1.Contains(row_header_lotno) == true)
                                {
                                    dataGridView1.Rows.RemoveAt(n);
                                }
                            }
                        }
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            int rowIndex = row.Index;
                            string row_header_lotno = row.HeaderCell.Value.ToString();
                            if (only_expiry_datas_row_index_grid_1.Contains(rowIndex) == true)
                            {
                                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                            }
                            rowIndex++;
                        }

                    }
                    if (txt_machine_no.Text != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(txt_machine_no.Text);
                        txt_machine_no.Text = formate_type.ToString("D2");
                    }
                    Cursor.Current = Cursors.Default;
                    // }
                }
                else
                {
                    MessageBox.Show("Atleast any one select these field Lotno ,Customer Code and Item Code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chk_lotno.Focus();
                }
            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                throw ex;
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool checkInput()
        {
            bool result = false;
            if (chk_customer.Checked || chk_item.Checked || chk_lotno.Checked || chk_shiped_frm.Checked || chk_manf_dt_frm_to.Checked || chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked all
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_all()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_customer.Checked && chk_item.Checked && chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked customer , item , lot and manf dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_customer_item_lot_manf()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_customer.Checked && chk_item.Checked && chk_manf_dt_frm_to.Checked && !chk_shiped_frm.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked  lot and manf dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_lot_manf()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_manf_dt_frm_to.Checked && !chk_shiped_frm.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  manf dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_manf()
        {
            bool result = false;
            if (chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && !chk_shiped_frm.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  item
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_item()
        {
            bool result = false;
            if (chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && !chk_shiped_frm.Checked && !chk_customer.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        ///  checked lotno , customer code and item code 
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_lotno_customer_item()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_customer.Checked && chk_item.Checked && !chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        ///  checked lotno , customer code
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_lotno_customer()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_customer.Checked && !chk_item.Checked && !chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked &&!chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked  , customer ,lotno,manfdt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_customer_lot_manfdt()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_customer.Checked && !chk_item.Checked && !chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked  , customer ,lotno,manfdt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_customer_item_manfdt()
        {
            bool result = false;
            if (!chk_lotno.Checked && chk_customer.Checked && chk_item.Checked && !chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        ///  checked lotno 
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_lotno_only()
        {
            bool result = false;
            if (chk_lotno.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        ///  checked machine 
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_machine_only()
        {
            bool result = false;
            if (chk_machine_no.Checked && !chk_lotno.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        ///  customer code and item code
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_customer_item()
        {
            bool result = false;
            if (chk_customer.Checked && chk_item.Checked && !chk_lotno.Checked && !chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        ///  customer code
        /// </summary>
        /// <returns></returns>
        ///  /// <summary>
        ///  checked shipment date from ,to
        /// </summary>
        /// <returns></returns>
        /// shipped,  checked customer , item , lot 
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_customer_item_lot()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_customer.Checked && chk_item.Checked && chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// shipped,  checked customer , item  
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_customer_item()
        {
            bool result = false;
            if (chk_customer.Checked && chk_item.Checked && chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// shipped,  manf
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_manf()
        {
            bool result = false;
            if (chk_manf_dt_frm_to.Checked && chk_shiped_frm.Checked && !chk_customer.Checked && !chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// ship , customer,  manf
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_cust_manf()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && chk_customer.Checked && !chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// customer, lotno, manf
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_cust_lot_manf()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// lotno, manf
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_lot_manf()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && !chk_customer.Checked && chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// customer,  manf
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_cust_lot()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        ///ship, customer,  item
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_cust_item()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && chk_customer.Checked && !chk_lotno.Checked && chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// shipm customer,  item .lot
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_ship_cust_item_lot()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && chk_customer.Checked && chk_lotno.Checked && chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// customer,  manf
        /// /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_cust_manf()
        {
            bool result = false;
            if (!chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && chk_customer.Checked && !chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// shipped,  checked customer  
        public bool checkInput_checkbox_check_ship_customer()
        {
            bool result = false;
            if (chk_customer.Checked && chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        /// shipped,  checked lotno  
        public bool checkInput_checkbox_check_ship_lotno()
        {
            bool result = false;
            if (chk_lotno.Checked && chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool checkInput_checkbox_check_shipmentfrm_to_only()
        {
            bool result = false;
            if (chk_shiped_frm.Checked && !chk_lotno.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
        public bool checkInput_checkbox_check_customer_only()
        {
            bool result = false;
            if (chk_customer.Checked && !chk_item.Checked && !chk_lotno.Checked && !chk_shiped_frm.Checked && !chk_manf_dt_frm_to.Checked && !chk_machine_no.Checked)
            {
                result = true;
            }
            return result;
        }
                
        ///  checked all with machine number
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_all_with_mno()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_customer.Checked && chk_item.Checked && chk_shiped_frm.Checked && chk_manf_dt_frm_to.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, ship dt, cust , itm
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_shp_cus_itm()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_shiped_frm.Checked && chk_customer.Checked && chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked )
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, ship dt, cust 
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_shp_cus()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_shiped_frm.Checked && chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, ship dt 
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_shp()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_shiped_frm.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, cust,itm,manf dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_cust_itm_manf()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_customer.Checked && chk_item.Checked && chk_manf_dt_frm_to.Checked && !chk_shiped_frm.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, cust,itm
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_cust_itm()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_customer.Checked && chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_shiped_frm.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, cust
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_cust()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked && !chk_shiped_frm.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, cust , manf dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_cust_manfdt()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_customer.Checked && chk_manf_dt_frm_to.Checked && !chk_item.Checked &&  !chk_shiped_frm.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, ship dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_ship()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_shiped_frm.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_manf_dt_frm_to.Checked  && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        ///  checked machine number, cust,itm,manf dt
        /// </summary>
        /// <returns></returns>
        public bool checkInput_checkbox_check_mno_manfdt()
        {
            bool result = false;
            if (chk_machine_no.Checked && chk_manf_dt_frm_to.Checked && !chk_customer.Checked && !chk_item.Checked && !chk_shiped_frm.Checked && !chk_lotno.Checked)
            {
                result = true;
            }
            return result;
        }
        public void pstatus_gridbind(string AcutionType, string Actionrole, string customerCode, string itemCode, string lotno, string lotno_frm, string lotno_to, string ship_frm, string ship_to, string manf_dt_frm, string manf_df_to,string round_lotno)
        {
            try
            {
                dGProductInfoList.DataSource = null;
                DataTable ps_grid = new DataTable();
                string[] str_view = { "@ActionType", "@Actionrole", "@cust_cd", "@itm_cd", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@ship_frm", "@ship_to", "@manf_frm", "@manf_to","@rlotno" };
                string[] obj_view = { AcutionType, Actionrole, customerCode, itemCode, lotno, lotno_frm, lotno_to, ship_frm, ship_to, manf_dt_frm, manf_df_to,round_lotno };
                DataSet ds = helper.GetDatasetByCommandString("product_status_details_fetch", str_view, obj_view);
                helper.CloseConnection();
                //MySqlDataReader dss = helper.GetReaderByCmd("product_status_details_fetch", str_view, obj_view);
                //List<productlist> m_model_p1 = LocalReportExtensions.GetList<productlist>(dss);
                if (ds.Tables[0].Rows.Count > 0)                
                {                    
                    ps_grid = ds.Tables[0];
                    for (int i = ps_grid.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow drow = ps_grid.Rows[i];
                        //130623 string clotno = ps_grid.Rows[i]["lotno"].ToString();
                        string clotno = ps_grid.Rows[i]["lotnumber"].ToString();
                        if (clotno == "")
                        {
                            drow.Delete();
                        }                       
                    }
                    ps_grid.AcceptChanges();
                    for (int count = 0; count < ps_grid.Rows.Count; count++)
                    {
                        ps_grid.Rows[count]["sno"] = count + 1;
                    }
                    txtCustomerCode.Text = ps_grid.Rows[0]["customercode"].ToString();
                    txtCustomerNameS.Text = ps_grid.Rows[0]["customerfull_name"].ToString();
                    if (chk_item.Checked)
                    {
                        textItemName.Text = ps_grid.Rows[0]["item_name"].ToString();
                    }
                    //ps_grid.DefaultView.Sort = "lotno ASC";
                    //List<productlist> GridList = new List<productlist>();
                    //GridList = (from DataRow dr in ps_grid.Rows
                    //               select new productlist()
                    //               {
                    //                   sno = dr["sno"].ToString(),
                    //                   lotno = dr["lotno"].ToString(),
                    //                   customercode = dr["customercode"].ToString(),
                    //                   customershort_name = dr["customershort_name"].ToString(),
                    //                   customerfull_name = dr["customerfull_name"].ToString(),
                    //                   item_code = dr["item_code"].ToString(),
                    //                   item_name = dr["item_name"].ToString(),
                    //                   unit_price_country_shortcd = dr["unit_price_country_shortcd"].ToString(),
                    //                   unit_price = dr["unit_price"].ToString(),
                    //                   box_qty = dr["box_qty"].ToString(),
                    //                   lable_typ = dr["lable_typ"].ToString(),
                    //                   m1 = dr["m1"].ToString(),
                    //                   m2 = dr["m2"].ToString(),
                    //                   m3 = dr["m3"].ToString(),
                    //                   m4 = dr["m4"].ToString(),
                    //                   additional_code = dr["additional_code"].ToString(),
                    //                   idpi_product_information = dr["idpi_product_information"].ToString()                                   
                    //               }).ToList();
                    //Set Columns Count
                    dGProductInfoList.ColumnCount = 18;

                    dGProductInfoList.AutoGenerateColumns = false;
                    dGProductInfoList.AllowUserToAddRows = false;
                    this.dGProductInfoList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                    
                    dGProductInfoList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    dGProductInfoList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    dGProductInfoList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                    dGProductInfoList.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dGProductInfoList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
                    //Add Columns
                    dGProductInfoList.Columns[0].Name = "S.no";
                    dGProductInfoList.Columns[0].DataPropertyName = "sno";
                    dGProductInfoList.Columns[0].Width = 50;

                    dGProductInfoList.Columns[1].Name = "Lot no";
                    dGProductInfoList.Columns[1].DataPropertyName = "lotnumber";
                    dGProductInfoList.Columns[1].DefaultCellStyle.Format = "D7";
                    dGProductInfoList.Columns[1].Width = 150;

                    dGProductInfoList.Columns[2].Name = "Customer Code";
                    dGProductInfoList.Columns[2].DataPropertyName = "customercode";
                    dGProductInfoList.Columns[2].Visible = false;


                    dGProductInfoList.Columns[3].Name = "Customer Name (Short)";
                    dGProductInfoList.Columns[3].DataPropertyName = "customershort_name";
                    dGProductInfoList.Columns[3].Width = 150;

                    dGProductInfoList.Columns[4].Name = "Customer Name (Full)";
                    dGProductInfoList.Columns[4].DataPropertyName = "customerfull_name";
                    dGProductInfoList.Columns[4].Visible = false;

                    dGProductInfoList.Columns[5].Name = "Item Code";
                    dGProductInfoList.Columns[5].DataPropertyName = "item_code";
                    dGProductInfoList.Columns[5].Visible = false;

                    dGProductInfoList.Columns[6].DataPropertyName = "item_name";
                    dGProductInfoList.Columns[6].Name = "Item Name";
                    dGProductInfoList.Columns[6].Width = 150;

                    dGProductInfoList.Columns[7].DataPropertyName = "unit_price_country_shortcd";
                    dGProductInfoList.Columns[7].Name = "Currency Types";
                    dGProductInfoList.Columns[7].Width = 150;

                    dGProductInfoList.Columns[8].DataPropertyName = "unit_price";
                    dGProductInfoList.Columns[8].Name = "Unit Price";
                    dGProductInfoList.Columns[8].Width = 150;

                    dGProductInfoList.Columns[9].DataPropertyName = "box_qty";
                    dGProductInfoList.Columns[9].Name = "Box Qty";
                    dGProductInfoList.Columns[9].Width = 150;

                    dGProductInfoList.Columns[10].DataPropertyName = "lable_typ";
                    dGProductInfoList.Columns[10].Name = "Lable Type";
                    dGProductInfoList.Columns[10].Width = 150;

                    dGProductInfoList.Columns[11].DataPropertyName = "m1";
                    dGProductInfoList.Columns[11].Name = "M1";
                    dGProductInfoList.Columns[11].Width = 100;

                    dGProductInfoList.Columns[12].DataPropertyName = "m2";
                    dGProductInfoList.Columns[12].Name = "M2";
                    dGProductInfoList.Columns[12].Width = 100;

                    dGProductInfoList.Columns[13].DataPropertyName = "m3";
                    dGProductInfoList.Columns[13].Name = "M3";
                    dGProductInfoList.Columns[13].Width = 100;


                    dGProductInfoList.Columns[14].DataPropertyName = "m4";
                    dGProductInfoList.Columns[14].Name = "M4";
                    dGProductInfoList.Columns[14].Width = 90;

                    dGProductInfoList.Columns[15].DataPropertyName = "additional_code";
                    dGProductInfoList.Columns[15].Name = "additional_code";
                    dGProductInfoList.Columns[15].Visible = false;

                    dGProductInfoList.Columns[16].DataPropertyName = "idpi_product_information";
                    dGProductInfoList.Columns[16].Name = "idpi_product_information";
                    dGProductInfoList.Columns[16].Visible = false;

                    dGProductInfoList.Columns[17].DataPropertyName = "item_code";
                    dGProductInfoList.Columns[17].Name = "item_code";
                    dGProductInfoList.Columns[17].Visible = false;


                    dGProductInfoList.DataSource = ps_grid;                  
                    //this.dGProductInfoList.Sort(this.dGProductInfoList.Columns[6], ListSortDirection.Ascending);

                    //List<productlist> page_list = new List<productlist>();
                    //var orderBy = GridList.OrderBy(r => r.customerfull_name).ToList();
                    //page_list.AddRange(orderBy.ToPagedList(PageNumber, PageSize));
                    //dGProductInfoList.DataSource = page_list;

                    this.dGProductInfoList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                    this.dGProductInfoList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    helper.CloseConnection();
                }
                else
                {
                   // MessageBox.Show("No Records Found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSearchCustomer.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    btnSearchCustomer.Focus();
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
                    btnSearchItem.Focus();
                    return;
                }
            }
        }

        private void chk_lotno_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_lotno.Checked)
            {
                if (textLotNo.Text == "0000000" || textLotNo.Text == string.Empty)
                {
                    chk_lotno.Checked = false;
                    MessageBox.Show("Enter the lot no..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textLotNo.Focus();
                    return;
                }
                else if(chk_machine_no.Checked)
                {                    
                    MessageBox.Show("Already Machine Number Choose, uncheck..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chk_machine_no.Focus();
                    return;
                }
            }
        }

        private void dGProductInfoList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex < 0)
                {
                    return;
                }
                int rowIndex = e.RowIndex;
                DataGridViewRow row = dGProductInfoList.Rows[rowIndex];

                string Lotno = row.Cells[1].Value.ToString();
                if (Lotno != string.Empty)
                {
                    int formate_type = Convert.ToInt32(Lotno);
                    Lotno = formate_type.ToString("D7");
                }
                string customer_code = row.Cells[2].Value.ToString();
                selected_customer_code = customer_code;
                string item_code = row.Cells[5].Value.ToString();
                string item_name = row.Cells[6].Value.ToString();
                FetchBOMDetails(customer_code, item_code);
                // lot information list
                textLotNoAdd.Text = Lotno;
                txt_lotinfo_itm_nam.Text = item_name;
                txt_lotinfo_itemcode.Text = item_code;
                // shipment list
                txt_pl_lotno.Text = Lotno;
                txt_pi_itemname.Text = item_name;
                txt_pl_itemcode.Text = item_code;
                // Lotinformation list 
                dynamic_button();
                lotinfo_value_assign_gridbind();
                if(chk_expirydt.Checked)
                {
                    //foreach (DataGridViewRow d1 in dataGridView1.Rows)
                    //{
                    //    int rowIndex_d1 = d1.Index;
                    //    string row_header_lotno = d1.HeaderCell.Value.ToString();
                    //    if (only_expiry_datas_row_index_grid_1.Contains(rowIndex_d1) == true)
                    //    {
                    //        dataGridView1.Rows.RemoveAt(rowIndex_d1);
                    //    }
                    //    rowIndex_d1++;
                    //}
                    only_expiry_datas_row_lotnojoin.Distinct().ToList();
                    for (int n = dataGridView1.Rows.Count - 1; n >= 0; n--)
                    {
                        if (dataGridView1.Rows[n].HeaderCell.Value != null)
                        {
                            //if (dataGridView3.Rows[n].HeaderCell.Value.Equals(dataGridView3.Rows[m].Cells[2].Value)                          {
                            //    dataGridView3.Rows.RemoveAt(n);
                            //    //break;
                            //}
                            string row_header_lotno = dataGridView1.Rows[n].HeaderCell.Value.ToString();
                            if (only_expiry_datas_row_lotnojoin.Contains(row_header_lotno) == true)
                            {
                                dataGridView1.Rows.RemoveAt(n);
                            }
                        }
                    }

                    only_expiry_datas_row_lotnojoin_gridview_1.Distinct().ToList();
                    for (int n = dataGridView3.Rows.Count - 1; n >= 0; n--)
                    {
                        if (dataGridView3.Rows[n].HeaderCell.Value != null)
                        {
                            //if (dataGridView3.Rows[n].HeaderCell.Value.Equals(dataGridView3.Rows[m].Cells[2].Value)                          {
                            //    dataGridView3.Rows.RemoveAt(n);
                            //    //break;
                            //}
                            string row_header_lotno = dataGridView3.Rows[n].HeaderCell.Value.ToString();
                            if (only_expiry_datas_row_lotnojoin_gridview_1.Contains(row_header_lotno) == true)
                            {
                                dataGridView3.Rows.RemoveAt(n);
                            }
                        }
                    }
                }
                // shipment list
                DataTable dt_ship = new DataTable();
                dataGridView2.DataSource = dt_ship;
                dataGridView2.DataSource = null;
                call_shipment("onlylotno", txt_pl_lotno.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FetchBOMDetails(string customercode, string itemcode)
        {
            try
            {
                dGProductInfoList.Refresh();
                DataSet ds = new DataSet();
                DataTable dt_BOMDetails = new DataTable();
                ds = helper.GetDatasetByBOMView_Pro_input(customercode, itemcode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt_BOMDetails = ds.Tables[0];
                    CommonClass.Process_name_Status = new List<PI_Process>();
                    CommonClass.Process_name_gridbind_Status = new List<PI_Process>();
                    PI_Process models = new PI_Process();
                    models.id = "XXX";
                    models.ProcessNames = "TERMINAL BOARD INFO";
                    models.PaternType = "5";
                    models.process_id = "0";
                    CommonClass.Process_name_Status.Add(models);
                    int i = 1;
                    foreach (DataRow dt_bom in dt_BOMDetails.Rows)
                    {
                        PI_Process model = new PI_Process();
                        model.id = i.ToString();
                        model.ProcessNames = dt_bom[5].ToString();
                        model.PaternType = dt_bom[14].ToString();
                        model.process_id = dt_bom[15].ToString();
                        model.materialcode = dt_bom[6].ToString();
                        model.itemcode = dt_bom[2].ToString();
                        CommonClass.Process_name_Status.Add(model);
                        i++;
                    }
                    CommonClass.Process_name_gridbind_Status.AddRange(CommonClass.Process_name_Status);
                    helper.CloseConnection();
                }
                else
                {
                    helper.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void dynamic_button()
        {
            try
            {
                int i = 10;
                int x = -1;

                panel1.Controls.Clear();
                int total_process = CommonClass.Process_name_Status.Count;
                foreach (var itm in CommonClass.Process_name_Status)
                {
                    Color back_clr = System.Drawing.Color.Red;
                    Color fore_clr = System.Drawing.Color.White;
                    string getid = itm.id;

                    if (getid != "XXX")
                    {
                        //This block dynamically creates a Button and adds it to the form
                        Button btn = new Button();

                        btn.BackColor = System.Drawing.Color.Red;
                        btn.ForeColor = System.Drawing.Color.White;
                        btn.Location = new System.Drawing.Point(19, 29);
                        btn.Name = itm.id + "#" + itm.PaternType + "#" + itm.ProcessNames + "#" + itm.process_id;
                        btn.Size = new System.Drawing.Size(80, 60);
                        btn.TabIndex = 103;
                        btn.Text = itm.ProcessNames;
                        btn.UseVisualStyleBackColor = false;
                        //btn.Click += new System.EventHandler(this.Patern_Click);
                        btn.Location = new Point(i, x);
                        panel1.AutoScroll = true;
                        panel1.Controls.Add(btn);
                        i += 100;
                    }

                }
                // dynamic header  create datagridview1 
                LoadDataGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void Patern_Click(object sender, EventArgs e)
        {
            //if (!check_lotno_lotnoChild_itemCode())
            //{
            string patern_type = ((Button)sender).Name.Split('#')[1];
            string patern_Name = ((Button)sender).Name.Split('#')[2];
            string process_id = ((Button)sender).Name.Split('#')[3];
            // Button btn = (Button)sender;
            if (patern_type == "1")
            {
                FormPatern1 frm = new FormPatern1();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.Part_No = string.Empty;
                frm.ProcessName = patern_Name;
                frm.ProcessId = process_id;
                frm.Sender_button = ((Button)sender).Name;
                frm.Name = ((Button)sender).Name;
                frm.SelectedPartNumber = "selected_dgProduct_partnumber";
                frm.SelectedLotNumber = textLotNoAdd.Text + "-" + textLotNoChild.Text;
                frm.SelectedLotNumber_spl = string.Empty;
                frm.SelectedManfDate = dateTimePicker_Manf.Value.ToShortDateString();
                frm.SelectedManfDate_use_insert = dateTimePicker_Manf.Value.ToString("yyyy-MM-dd");
                frm.SelectedManfTime = txt_manf_time.Text;
                frm.SelectedQuantity = txt_lotinfo_quantity.Text;
                frm.itemcode = txt_lotinfo_itemcode.Text;
                frm.itemname = txt_lotinfo_itm_nam.Text;
                frm.Customer_code = txtCustomerCode.Text;              
                frm.ShowDialog();

            }
            else if (patern_type == "2")
            {
                FormPatern2 frm = new FormPatern2();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.ProcessName = patern_Name;
                frm.Sender_button = ((Button)sender).Name;
                frm.ProcessId = process_id;
                frm.SelectedLotNumber = textLotNoAdd.Text + "-" + textLotNoChild.Text;
                frm.SelectedManfDate = dateTimePicker_Manf.Value.ToShortDateString();
                frm.SelectedManfDate_use_insert = dateTimePicker_Manf.Value.ToString("yyyy-MM-dd");
                frm.SelectedQuantity = txt_lotinfo_quantity.Text;
                frm.SelectedManfTime = txt_manf_time.Text;
                frm.itemcode = txt_lotinfo_itemcode.Text;
                frm.itemname = txt_lotinfo_itm_nam.Text;
                frm.Customer_code = txtCustomerCode.Text;
                frm.ShowDialog();
            }
            else if (patern_type == "3")
            {
                FormPatern3 frm = new FormPatern3();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.ProcessName = patern_Name;
                frm.Sender_button = ((Button)sender).Name;
                frm.SelectedManfDate = dateTimePicker_Manf.Value.ToShortDateString();
                frm.SelectedManfDate_use_insert = dateTimePicker_Manf.Value.ToString("yyyy-MM-dd");
                frm.ProcessId = process_id;
                frm.SelectedQuantity = txt_lotinfo_quantity.Text;
                frm.SelectedManfTime = txt_manf_time.Text;
                frm.SelectedHiddenLotNo = textLotNoAdd.Text + "-" + textLotNoChild.Text;
                frm.itemcode = txt_lotinfo_itemcode.Text;
                frm.itemname = txt_lotinfo_itm_nam.Text;
                frm.Customer_code = txtCustomerCode.Text;
                frm.ShowDialog();
            }
            else if (patern_type == "4")
            {
                FormPatern4 frm = new FormPatern4();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.ProcessName = patern_Name;
                frm.Sender_button = ((Button)sender).Name;
                frm.SelectedManfDate = dateTimePicker_Manf.Value.ToShortDateString();
                frm.SelectedManfDate_use_insert = dateTimePicker_Manf.Value.ToString("yyyy-MM-dd");
                frm.ProcessId = process_id;
                frm.SelectedQuantity = txt_lotinfo_quantity.Text;
                frm.SelectedManfTime = txt_manf_time.Text;
                frm.SelectedLotNumber = textLotNoAdd.Text + "-" + textLotNoChild.Text;
                frm.itemcode = txt_lotinfo_itemcode.Text;
                frm.itemname = txt_lotinfo_itm_nam.Text;
                frm.Customer_code = txtCustomerCode.Text;
                frm.ShowDialog();
            }
            //}
            //else
            //{
            //    MessageBox.Show("Lot No. and Lot No Child already mapped in some other item code..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void LoadDataGrid()
        {
            try
            {
                dataGridView1.DataSource = null;
                int total_process = CommonClass.Process_name_gridbind_Status.Count;
                // grid bind start
                int totalgroup = total_process;
                int year = DateTime.Now.Year;
                daysInMonths = new int[totalgroup]; // check line 129
                GroupLabel = new string[totalgroup];
                LabelString = new string[totalgroup, 10];
                LabelSize = new int[totalgroup, 10];
                List<KeyValuePair<int, string>> kvpList = new List<KeyValuePair<int, string>>();
                List<PI_Process> module = new List<PI_Process>();
                int i = 0;
                this.dataGridView1.Columns.Clear();
                foreach (var itm in CommonClass.Process_name_gridbind_Status)
                {
                    string getid = itm.id;
                    //if (selected_dgProduct_partnumber == getid || getid == "XXX")                    
                    //{
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
                            LabelString[i, 2] = "Plating Date";
                            LabelString[i, 3] = "Quantity";
                            LabelString[i, 4] = "Pb Date";
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
                            LabelSize[i, 2] = 80;
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
                        LabelString[0, 0] = "B";
                        LabelString[0, 1] = "H";
                        LabelString[0, 2] = "S";
                        LabelString[0, 3] = "Remarks";
                        LabelString[0, 4] = "Quantity";
                        LabelString[0, 5] = "Manufacturing Date";
                        LabelString[0, 6] = "Expiry Date";
                        LabelSize[0, 0] = 40;
                        LabelSize[0, 1] = 40;
                        LabelSize[0, 2] = 40;
                        LabelSize[0, 3] = 80;
                        LabelSize[0, 4] = 80;
                        LabelSize[0, 5] = 150;
                        LabelSize[0, 6] = 150;
                    }

                    //}

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
                    var Value = element.Value;
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
                        daysInMonths[month - 1] = 7;
                    }
                    for (int day = 1; day <= daysInMonths[month - 1]; day++)
                    {
                        DateTime date = new DateTime(year, month, day);

                        string colname = "";
                        string colheadname = "";
                        int colsize = 80;

                        if (month <= totalgroup)
                        {
                            colname = LabelString[month - 1, day - 1];
                            colheadname = LabelString[month - 1, day - 1];
                            colsize = LabelSize[month - 1, day - 1];

                        }
                        else
                        {
                            colname = date.ToString();
                            colheadname = day.ToString();
                            colsize = 80;
                        }
                        DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn()
                        {
                            Name = colname,
                            HeaderText = colheadname,
                            Width = colsize
                        };
                        this.dataGridView1.Columns.Add(col);
                    }
                }
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                this.dataGridView1.Paint += dataGridView1_Paint;
                this.dataGridView1.Scroll += dataGridView1_Scroll;
                this.dataGridView1.ColumnWidthChanged += dataGridView1_ColumnWidthChanged;
                this.dataGridView1.Resize += dataGridView1_Resize;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InvalidateHeader()
        {
            System.Drawing.Rectangle rtHeader = this.dataGridView1.DisplayRectangle;
            rtHeader.Height = this.dataGridView1.ColumnHeadersHeight / 2;
            this.dataGridView1.Invalidate(rtHeader);
        }
        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            int col = 0;
            int count = 0;
            if (daysInMonths != null)
            {
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
                        //  string month = DateTime.Parse(this.dataGridView1.Columns[col].Name).ToString("MMMM");
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

        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            InvalidateHeader();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            InvalidateHeader();
        }

        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            InvalidateHeader();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TabPage selectedTab = tabControl1.SelectedTab;
                tabControl1.SelectedTab = selectedTab;
                if (tabControl1.SelectedTab.Text == "Product Information List")
                {

                }
                else if (tabControl1.SelectedTab.Text == "Lot Information List")
                {
                    string boxQty = "1,000";
                    boxQty = boxQty.Replace(",", "");
                    txt_lotinfo_quantity.Text = boxQty;
                }
                else if (tabControl1.SelectedTab.Text == "Shipped Details")
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void lotinfo_value_assign_gridbind()
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();

                List<Lotinfo_gridbind_common> list_lotinfo_Common = new List<Lotinfo_gridbind_common>();

                string ActionType = "pilotinfo";
                string[] str = { "@ActionType", "@lotno" };
                string[] obj = { ActionType, textLotNoAdd.Text };
                // lot information common data's 
                DataSet ds = helper.GetDatasetByCommandString("pi_lotinfo_fetch", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    string p_code = dt.Rows[0]["idproduction_input_master"].ToString();
                    txt_lotinfo_itm_nam.Text = dt.Rows[0]["item_name"].ToString();
                    //txt_lotinfo_quantity.Text = dt.Rows[0]["lotqty"].ToString();
                }
                helper.CloseConnection();
                // lot information grid data's
                // p1
                string index = string.Empty;
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p1 = { ActionType_p1, textLotNoAdd.Text, txtCustomerCode.Text, string.Empty, txt_lotinfo_itemcode.Text };

                DataSet ds_pattern1 = helper.GetDatasetByCommandString("allpattern_view_itemcode_lotno", str_p1, obj_p1);
                List<Lotinfo_gridbind_common_pattern> clist_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.processName = "TERMINAL BOARD INFO";
                c_model.pattern_type = "5";
                list_cmodel.Add(c_model);
                if (ds_pattern1.Tables[0].Rows.Count > 0)
                {
                    Lotinfo_gridbind_common model_p1 = new Lotinfo_gridbind_common();
                    foreach (DataRow dr in ds_pattern1.Tables[0].Rows)
                    {
                        string lotno_split = dr["lotnojoin_p1"].ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr["pattern_type"].ToString();
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
                            //
                            c_model.processId = dr["processId_p1"].ToString();
                            c_model.processName = dr["processName_p1"].ToString();
                            c_model.partno = dr["partno_p1"].ToString();
                            c_model.qty = dr["quantity_p1"].ToString();
                            c_model.plantingdate = dr["planting_p1"].ToString();
                            c_model.pb_date = dr["pb_dt_p1"].ToString();
                            c_model.tb_manuf_dt = dr["tb_manuf_dt_p1"].ToString();
                            c_model.tb_expairy_dt = dr["tb_expairy_dt_p1"].ToString();
                            c_model.tb_qty = dr["tb_qty_p1"].ToString();
                            c_model.material_code = dr["materialcd"].ToString();
                            c_model.tb_bproduct = dr["bproduct_p1"].ToString();
                            c_model.onhold = dr["onhold_p1"].ToString();
                            c_model.scrap = dr["scrap_p1"].ToString();
                            c_model.reason_hs = dr["reason_hs_p1"].ToString();
                            list_cmodel.Add(c_model);
                        }
                    }

                }
                helper.CloseConnection();
                string ActionType_p2 = "p2view";
                string[] str_p2 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p2 = { ActionType_p2, textLotNoAdd.Text, txtCustomerCode.Text, string.Empty, txt_lotinfo_itemcode.Text };
                DataSet ds_pattern2 = helper.GetDatasetByCommandString("allpattern_view_itemcode_lotno", str_p2, obj_p2);
                if (ds_pattern2.Tables[0].Rows.Count > 0)
                {
                    Lotinfo_gridbind_common model_p2 = new Lotinfo_gridbind_common();
                    foreach (DataRow dr in ds_pattern2.Tables[0].Rows)
                    {
                        string lotno_split = dr["lotnojoin_p2"].ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr["pattern_type"].ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p2"].ToString();
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
                            //
                            c_model.processId = dr["processId_p2"].ToString();
                            c_model.processName = dr["processName_p2"].ToString();
                            c_model.plantingdate = dr["process_date_p2"].ToString();
                            c_model.partno = dr["contorlno_p2"].ToString();
                            //080822 c_model.lotno = dr["slot_no_p2"].ToString();
                            c_model.lotno = lotno_format;
                            c_model.qty = dr["quantity_p2"].ToString();
                            c_model.tb_manuf_dt = dr["tb_manuf_dt_p2"].ToString();
                            c_model.tb_expairy_dt = dr["tb_expairy_dt_p2"].ToString();
                            c_model.tb_qty = dr["tb_qty_p2"].ToString();
                            c_model.material_code = dr["materialcd"].ToString();
                            c_model.tb_bproduct = dr["bproduct_p2"].ToString();
                            c_model.onhold = dr["onhold_p2"].ToString();
                            c_model.scrap = dr["scrap_p2"].ToString();
                            c_model.reason_hs = dr["reason_hs_p2"].ToString();
                            list_cmodel.Add(c_model);
                        }
                    }

                }
                helper.CloseConnection();
                string ActionType_p3 = "p3view";
                string[] str_p3 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p3 = { ActionType_p3, textLotNoAdd.Text, txtCustomerCode.Text, string.Empty, txt_lotinfo_itemcode.Text };
                //string[] obj_p3 = { ActionType_p3, textLotNoAdd.Text, cmbProcess.SelectedValue.ToString(), string.Empty };
                DataSet ds_pattern3 = helper.GetDatasetByCommandString("allpattern_view_itemcode_lotno", str_p3, obj_p3);
                if (ds_pattern3.Tables[0].Rows.Count > 0)
                {
                    //  Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                    Lotinfo_gridbind_common model_p3 = new Lotinfo_gridbind_common();
                    foreach (DataRow dr in ds_pattern3.Tables[0].Rows)
                    {
                        string lotno_split = dr["lotnojoin_p3"].ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr["pattern_type"].ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p3"].ToString();
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
                            //
                            c_model.processId = dr["processId_p3"].ToString();
                            c_model.processName = dr["processName_p3"].ToString();
                            c_model.plantingdate = dr["process_date_p3"].ToString();
                            c_model.qty = dr["quantity_p3"].ToString();
                            c_model.tb_manuf_dt = dr["tb_manuf_dt_p3"].ToString();
                            c_model.tb_expairy_dt = dr["tb_expairy_dt_p3"].ToString();
                            c_model.tb_qty = dr["tb_qty_p3"].ToString();
                            c_model.material_code = dr["materialcd"].ToString();
                            c_model.tb_bproduct = dr["bproduct_p3"].ToString();
                            c_model.onhold = dr["onhold_p3"].ToString();
                            c_model.scrap = dr["scrap_p3"].ToString();
                            c_model.reason_hs = dr["reason_hs_p3"].ToString();
                            list_cmodel.Add(c_model);
                        }
                    }

                }
                helper.CloseConnection();
                string ActionType_p4 = "p4view";
                string[] str_p4 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p4 = { ActionType_p4, textLotNoAdd.Text, txtCustomerCode.Text, string.Empty, txt_lotinfo_itemcode.Text };
                //string[] obj_p4 = { ActionType_p4, textLotNoAdd.Text, cmbProcess.SelectedValue.ToString(), string.Empty };
                DataSet ds_pattern4 = helper.GetDatasetByCommandString("allpattern_view_itemcode_lotno", str_p4, obj_p4);
                if (ds_pattern4.Tables[0].Rows.Count > 0)
                {
                    Lotinfo_gridbind_common model_p4 = new Lotinfo_gridbind_common();
                    foreach (DataRow dr in ds_pattern4.Tables[0].Rows)
                    {
                        string lotno_split = dr["lotnojoin_p4"].ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr["pattern_type"].ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p4"].ToString();
                            // lot no format change                        
                            string dG1joinlotno = dr["lotnojoin_p4"].ToString();
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
                            //
                            c_model.processId = dr["processId_p4"].ToString();
                            c_model.processName = dr["processName_p4"].ToString();
                            c_model.partno = dr["partno_p4"].ToString();
                            c_model.qty = dr["quantity_p4"].ToString();
                            c_model.tb_manuf_dt = dr["tb_manuf_dt_p4"].ToString();
                            c_model.tb_expairy_dt = dr["tb_expairy_dt_p4"].ToString();
                            c_model.tb_qty = dr["tb_qty_p4"].ToString();
                            //060623 c_model.lotno_p4 = dr["lotno_p4_p4"].ToString();
                            c_model.lotno_p4 = dr["lotno_p4"].ToString();
                            c_model.material_code = dr["materialcd"].ToString();

                            c_model.tb_bproduct = dr["bproduct_p4"].ToString();
                            c_model.onhold = dr["onhold_p4"].ToString();
                            c_model.scrap = dr["scrap_p4"].ToString();
                            c_model.reason_hs = dr["reason_hs_p4"].ToString();
                            list_cmodel.Add(c_model);
                        }
                    }

                }
                helper.CloseConnection();
                list_cmodel = list_cmodel.OrderBy(o => o.lotnojoin).ToList();
                List<string> already_exits_row_header = new List<string>();
                List<string> already_exits_row_header_lotno_only = new List<string>();
                List<string> row_header_lotno_all_combined = new List<string>();
                List<string> already_exits_row_columns = new List<string>();
                if (dataGridView1.Rows.Count == 0)
                {
                    if (list_cmodel.Count > 1)
                    {
                        int header_lot_index = 0;
                        List<string> already_exits_row = new List<string>();
                        foreach (var lotno in list_cmodel)
                        {
                            if (header_lot_index > 0)
                            {
                                //if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                //{
                                //    // DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                //    ////// add  lot number  
                                //    //First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                //    //First_row_dynamic_colm.HeaderCell.Value = lotno.lotnojoin;
                                //    //this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                //    row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                //    //already_exits_row.Add(lotno.lotnojoin);
                                //    already_exits_row_header.Add(lotno.lotnojoin);
                                //}

                                if (!chk_expirydt.Checked)
                                {

                                    if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                    {
                                        row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        already_exits_row_header.Add(lotno.lotnojoin);
                                    }

                                }
                                else if (chk_expirydt.Checked)
                                {
                                    DateTime compare_date = DateTime.Parse(lotno.tb_expairy_dt.Split(',')[0]);

                                    int grater_than = DateTime.Compare(nowdate, compare_date);
                                    if (grater_than >= 0)
                                    {
                                        only_expiry_datas_grid_1.Add(lotno.lotnojoin);
                                        //row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        //already_exits_row_header.Add(lotno.lotnojoin);
                                    }

                                    if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                    {
                                        row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        already_exits_row_header.Add(lotno.lotnojoin);
                                    }
                                }
                            }
                            header_lot_index++;
                        }
                        /////////////////////////////
                        // only lot number table refer 
                        //string ActionType_only_lot = "onlylotview";
                        string ActionType_only_lot = "onlylotview_lotno";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textLotNoAdd.Text };

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
                                string lotno_chld = lotnochld_format;
                                string item_cd = dr["item_code"].ToString();
                                string manf_dt = dr["manufacturing_date"].ToString();
                                string expairy_dt = dr["expairy_date"].ToString();
                                string manf_time = dr["manufacturing_time"].ToString();
                                string lotqty = dr["lotqty"].ToString();
                                string flag_onlylotno = dr["flag_only_lotno"].ToString();
                                string print_lablestatus = dr["print_lable_status"].ToString();
                                string print_labledate = dr["print_lable_date"].ToString();
                                string print_shipmentflg = dr["shipment_flag"].ToString();
                                string print_shipmentdate = dr["shipment_date"].ToString();
                                // shipment expiry date check
                                if (!string.IsNullOrEmpty(print_shipmentdate))
                                {
                                    DateTime compare_date = DateTime.Parse(print_shipmentdate);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        // already_exits_row_header.Add(dG1joinlotno);
                                        already_exits_row_header_lotno_only.Add(dG1joinlotno);
                                        continue;
                                    }
                                }
                                //already_exits_row_columns.AddRange(already_exits_row_header);
                                already_exits_row_columns.AddRange(already_exits_row_header_lotno_only);
                                // header bind 
                                if (already_exits_row_header.Contains(dG1joinlotno) == false)
                                {
                                    row_header_lotno_all_combined.Add(dG1joinlotno);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }

                        }
                        ///9022022
                        ///grid row header bind
                        ///list_cmodel.OrderBy(o => o.lotnojoin).ToList();                        
                        row_header_lotno_all_combined = row_header_lotno_all_combined.OrderBy(i => i).ToList();
                        foreach (var rowheader in row_header_lotno_all_combined)
                        {
                            DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                            //// add  lot number  
                            First_row_dynamic_colm.CreateCells(this.dataGridView1);
                            First_row_dynamic_colm.HeaderCell.Value = rowheader;

                            this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                            if (only_expiry_datas_grid_1.Contains(rowheader) == false)
                            {
                                only_expiry_datas_row_index_grid_1.Add(dataGridView1.RowCount - 1);
                                only_expiry_datas_row_lotnojoin.Add(rowheader);
                            }
                            else
                            {

                            }

                        }
                    }
                    else if (list_cmodel.Count == 1)
                    {
                        string ActionType_only_lot = "onlylotview";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textLotNoAdd.Text };

                        DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);
                        if (ds_only_lot.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds_only_lot.Tables[0].Rows)
                            {
                                //string lotno = dr["lotno"].ToString();
                                //string lotno_chld = dr["lot_no_child"].ToString();
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
                                string lotno_chld = lotnochld_format;

                                string item_cd = dr["item_code"].ToString();
                                string manf_dt = dr["manufacturing_date"].ToString();
                                string expairy_dt = dr["expairy_date"].ToString();
                                string manf_time = dr["manufacturing_time"].ToString();
                                string lotqty = dr["lotqty"].ToString();
                                string flag_onlylotno = dr["flag_only_lotno"].ToString();
                                string print_lablestatus = dr["print_lable_status"].ToString();
                                string print_labledate = dr["print_lable_date"].ToString();
                                string print_shipmentflg = dr["shipment_flag"].ToString();
                                string print_shipmentdate = dr["shipment_date"].ToString();
                                // shipment expiry date check
                                if (!string.IsNullOrEmpty(print_shipmentdate))
                                {
                                    DateTime compare_date = DateTime.Parse(print_shipmentdate);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        already_exits_row_header.Add(dG1joinlotno);
                                        continue;
                                    }
                                }
                                already_exits_row_columns.AddRange(already_exits_row_header);
                                // header bind 
                                if (already_exits_row_header.Contains(dG1joinlotno) == false)
                                {
                                    DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                    //// add  lot number  
                                    First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                    First_row_dynamic_colm.HeaderCell.Value = dG1joinlotno;

                                    this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }

                        }
                    }
                }
                int columun_count_v = 0;
                string mapped_processname = string.Empty;
                if (list_cmodel.Count > 1)
                {
                    foreach (var item in list_cmodel)
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = item.processName.Split(',');
                            int chk_index = 0;
                            if (already_exits_row_columns.Contains(item.lotnojoin) == false)
                            {
                                foreach (var chk in split_process_name)
                                {
                                    string patern_type = item.pattern_type;
                                    foreach (var itm in CommonClass.Process_name_Status)
                                    {
                                        string patern_type_list = itm.PaternType;
                                        //if (itm.ProcessNames == chk)
                                        if (itm.ProcessNames == chk && itm.materialcode == item.material_code.Split(',')[chk_index])
                                        {
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
                                                columun_count_v = columun_count_v + 4;
                                            }
                                            else if (patern_type_list == "3")
                                            {
                                                columun_count_v = columun_count_v + 2;
                                            }
                                            else if (patern_type_list == "4")
                                            {
                                                columun_count_v = columun_count_v + 3;
                                            }
                                            else if (patern_type_list == "5")
                                            {
                                                 columun_count_v = columun_count_v + 7;
                                                //columun_count_v = columun_count_v + 9;
                                            }
                                        }

                                    }
                                    // List compare submited button name wise
                                    string current_procesname = item.processName.Split(',')[0];
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();
                                            if (Compare_lotNo == item.lotnojoin)
                                            {
                                                // int index_column = list_index;
                                                int index_column = columun_count_v;
                                                //row.Cells[0].Value = item.tb_qty.Split(',')[chk_index];

                                                //row.Cells[1].Value = item.tb_manuf_dt.Split(',')[chk_index];

                                                //row.Cells[2].Value = item.tb_expairy_dt.Split(',')[chk_index];

                                                if (!string.IsNullOrEmpty(item.tb_bproduct.Split(',')[chk_index]))
                                                {
                                                    //dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                                }
                                                row.Cells[0].Value = item.tb_bproduct.Split(',')[chk_index];
                                                if (!string.IsNullOrEmpty(item.onhold.Split(',')[chk_index]))
                                                {
                                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                                }
                                                if (!string.IsNullOrEmpty(item.scrap.Split(',')[chk_index]))
                                                {
                                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                }
                                                row.Cells[1].Value = item.onhold.Split(',')[chk_index];
                                                row.Cells[2].Value = item.scrap.Split(',')[chk_index];
                                                row.Cells[3].Value = item.reason_hs.Split(',')[chk_index];
                                                row.Cells[4].Value = item.tb_qty.Split(',')[chk_index];

                                                row.Cells[5].Value = item.tb_manuf_dt.Split(',')[chk_index];
                                                // compare to current date
                                                DateTime from_dt = Convert.ToDateTime(item.tb_expairy_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                DateTime to_dt = DateTime.Now;
                                                int result = DateTime.Compare(from_dt, to_dt);
                                                if (result >= 1)
                                                {
                                                    row.Cells[6].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                }
                                                else
                                                {
                                                    row.Cells[6].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                }
                                                if (patern_type == "1")
                                                {
                                                    row.Cells[columun_count_v].Value = item.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.lotno;
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
                                                    row.Cells[columun_count_v].Value = item.lotno;
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
                                                    //row.Cells[columun_count_v].Value = item.lotno;
                                                    row.Cells[columun_count_v].Value = item.lotno_p4.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }

                                            }
                                        }



                                    }
                                    chk_index++;
                                }
                            }

                        }
                        list_index++;

                    }
                    //
                    lot_number_only_row_common("onlylotview");
                }
                else if (list_cmodel.Count == 1)
                {
                    string ActionType_only_lot = "onlylotview";
                    string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                    string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textLotNoAdd.Text };

                    DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);
                    if (ds_only_lot.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds_only_lot.Tables[0].Rows)
                        {
                            //string lotno = dr["lotno"].ToString();
                            //string lotno_chld = dr["lot_no_child"].ToString();
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
                            string lotno_chld = lotnochld_format;

                            string item_cd = dr["item_code"].ToString();
                            string manf_dt = dr["manufacturing_date"].ToString();
                            string expairy_dt = dr["expairy_date"].ToString();
                            string manf_time = dr["manufacturing_time"].ToString();
                            string lotqty = dr["lotqty"].ToString();
                            string flag_onlylotno = dr["flag_only_lotno"].ToString();
                            string print_lablestatus = dr["print_lable_status"].ToString();
                            string print_labledate = dr["print_lable_date"].ToString();
                            string print_shipmentflg = dr["shipment_flag"].ToString();
                            string print_shipmentdate = dr["shipment_date"].ToString();
                            // shipment expiry date check
                            if (!string.IsNullOrEmpty(print_shipmentdate))
                            {
                                DateTime compare_date = DateTime.Parse(print_shipmentdate);
                                DateTime Result = compare_date.AddMonths(+2);
                                int grater_than = DateTime.Compare(Result, nowdate);
                                if (grater_than <= 0)
                                {
                                    already_exits_row_header.Add(dG1joinlotno);
                                    continue;
                                }
                            }
                            already_exits_row_columns.AddRange(already_exits_row_header);
                            // header bind 
                            if (already_exits_row_header.Contains(dG1joinlotno) == false)
                            {
                                DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                //// add  lot number  
                                First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                First_row_dynamic_colm.HeaderCell.Value = dG1joinlotno;

                                this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                already_exits_row_header.Add(dG1joinlotno);
                                
                                if (only_expiry_datas_grid_1.Contains(dG1joinlotno) == false)
                                {
                                    only_expiry_datas_row_index.Add(dataGridView3.CurrentRow.Index);
                                    only_expiry_datas_row_lotnojoin.Add(dG1joinlotno);
                                }
                                else
                                {

                                }
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
        public void lot_number_only_row_common(string ActionType_only_lot)
        {
            try
            {
                string Compare_lotNo;
                //string ActionType_only_lot = "onlylotview";
                string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textLotNoAdd.Text };

                DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);
                if (ds_only_lot.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_only_lot.Tables[0].Rows)
                    {
                        //string lotno = dr["lotno"].ToString();
                        //string lotno_chld = dr["lot_no_child"].ToString();
                        string lotno_join = dr["lotnoJoin"].ToString();
                        string dG1joinlotno = lotno_join;
                        string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                        string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                        int convert_lotno = Convert.ToInt32(lotno_spl);
                        int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                        string lotno_format = convert_lotno.ToString("D7");
                        string lotnochld_format = convert_lotnochld.ToString("D2");
                        string bproduct = dr["bproduct"].ToString();
                        string onHold = dr["onhold"].ToString();
                        string scrap = dr["scrap"].ToString();
                        string reason_hs = dr["reason_hs"].ToString();

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
                        string lotno_chld = lotnochld_format;

                        string item_cd = dr["item_code"].ToString();
                        string manf_dt = dr["manufacturing_date"].ToString();
                        string expairy_dt = dr["expairy_date"].ToString();
                        string manf_time = dr["manufacturing_time"].ToString();
                        string lotqty = dr["lotqty"].ToString();
                        string flag_onlylotno = dr["flag_only_lotno"].ToString();
                        string print_lablestatus = dr["print_lable_status"].ToString();
                        string print_labledate = dr["print_lable_date"].ToString();
                        string print_shipmentflg = dr["shipment_flag"].ToString();
                        string print_shipmentdate = dr["shipment_date"].ToString();

                        // row values bind 

                        //if (already_exits_row_columns.Contains(lotno_join) == false)
                        //{
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
                                        //dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = DefaultBackColor;
                                        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                                        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                                        this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

                                    }                                    
                                    row.Cells[0].Value = bproduct;
                                    if (!string.IsNullOrEmpty(onHold))
                                    {
                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                    }
                                    if (!string.IsNullOrEmpty(scrap))
                                    {
                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    row.Cells[1].Value = onHold;
                                    row.Cells[2].Value = scrap;
                                    row.Cells[3].Value = reason_hs;
                                    row.Cells[4].Value = lotqty;

                                    row.Cells[5].Value = manf_dt;
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
                                }
                            }
                        }
                        // }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex < 0)
                {
                    return;
                }
                int rowIndex = e.RowIndex;
                CommonClass.Process_name_gridbind_columns_shipping = new List<PI_Process>();
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                string lotsplit = dataGridView1.CurrentRow.HeaderCell.Value.ToString();
                textLotNoAdd.Text = lotsplit.Split('-')[0];
                textLotNoChild.Text = lotsplit.Split('-')[1];

                dateTimePicker_Manf.Value = Convert.ToDateTime(row.Cells[5].Value.ToString(),
                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                txt_lotinfo_quantity.Text = row.Cells[4].Value.ToString();
                color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
                /// shipment tab
                txt_pl_lotno.Text = lotsplit.Split('-')[0];
                txt_pl_frm_lotc.Text = lotsplit.Split('-')[1];
                ///datagrid view 2 
                DataTable dt = new DataTable();
                dataGridView3.DataSource = dt;
                string ActionType_ship_tab = string.Empty;
                if (!chkExclude.Checked)
                {
                    
                    ActionType_ship_tab = "shipment_only_lotno";
                }
                else if (chkExclude.Checked)
                {
                 
                    ActionType_ship_tab = "shipment_only_lotno_Exclude";
                }
                if (chk_shiped_frm.Checked)
                {
                    string sp_name = "allpattern_view_itemcode_shipment_only_lotno";
                    //shipment_gridbind_with_shpfilter(textLotNo.Text, txt_lotno_frm.Text, txt_lotno_to.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), selected_customer_code, txt_lotinfo_itemcode.Text, "common_cust_item", "ActionType_ship_tab", sp_name, "lotno", dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"));
                    //shipment_gridbind(textLotNoAdd.Text, textLotNoChild.Text, textLotNoChild.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), selected_customer_code, txt_lotinfo_itemcode.Text, "lotno", ActionType_ship_tab, sp_name, "lotno");
                    shipment_gridbind_single_lot(textLotNoAdd.Text, textLotNoChild.Text, textLotNoChild.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), selected_customer_code, txt_lotinfo_itemcode.Text, "lotno", ActionType_ship_tab, sp_name, "lotno");
                }
                else if(!chk_shiped_frm.Checked)
                {
                    string sp_name = "allpattern_view_itemcode_shipment_only_lotno";
                    //shipment_gridbind(textLotNoAdd.Text, textLotNoChild.Text, textLotNoChild.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), selected_customer_code, txt_lotinfo_itemcode.Text, "lotno", ActionType_ship_tab, sp_name, "lotno");
                    shipment_gridbind_single_lot(textLotNoAdd.Text, textLotNoChild.Text, textLotNoChild.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), selected_customer_code, txt_lotinfo_itemcode.Text, "lotno", ActionType_ship_tab, sp_name, "lotno");
                }
                //call_shipment("onelotonechild", txt_pl_lotno.Text, txt_pl_frm_lotc.Text, txt_pl_frm_lotc.Text, string.Empty, string.Empty, string.Empty, string.Empty);


                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void call_shipment(string actionrole, string lotno, string lotno_frm, string lotno_to, string manf_dt_frm, string manf_dt_to, string ship_dt_frm, string ship_dt_to)
        {
            string ActionType = string.Empty;
            if (!chkExclude.Checked)
            {
                ActionType = "shipment_only_lotno";
            }
            else if (chkExclude.Checked)
            {
                ActionType = "shipment_only_lotno_Exclude";
            }
            /// get textbox customer code and item code pass the value
            grid_bind(ActionType, lotno, lotno_frm, lotno_to, txtCustomerCode.Text, txt_pl_itemcode.Text, manf_dt_frm, manf_dt_to, ship_dt_frm, ship_dt_to, actionrole);

        }
        public void call_shipment_newgrid(string actionrole, string lotno, string lotno_frm, string lotno_to, string manf_dt_frm, string manf_dt_to, string ship_dt_frm, string ship_dt_to)
        {
            string ActionType = string.Empty;
            if (!chkExclude.Checked)
            {
                ActionType = "shipment_only_lotno";
            }
            else if (chkExclude.Checked)
            {
                ActionType = "shipment_only_lotno_Exclude";
            }
            /// get textbox customer code and item code pass the value
            //grid_bind(ActionType, lotno, lotno_frm, lotno_to, txtCustomerCode.Text, txt_pl_itemcode.Text, manf_dt_frm, manf_dt_to, ship_dt_frm, ship_dt_to, actionrole);

        }
        public void call_shipment_main_filter(string actionrole, string lotno, string lotno_frm, string lotno_to, string custcd, string itemcd, string manf_dt_frm, string manf_dt_to, string ship_dt_frm, string ship_dt_to)
        {
            string ActionType = string.Empty;
            if (!chkExclude.Checked)
            {
                ActionType = "shipment_only_lotno";
            }
            else if (chkExclude.Checked)
            {
                ActionType = "shipment_only_lotno_Exclude";
            }
            /// get textbox customer code and item code pass the value
            grid_bind(ActionType, lotno, lotno_frm, lotno_to, custcd, itemcd, manf_dt_frm, manf_dt_to, ship_dt_frm, ship_dt_to, actionrole);

        }
        public void color_change_dynamic_button(string lotno, string lotno_child)
        {
            int i = 10;
            int x = -1;
            panel1.Controls.Clear();

            int total_process = CommonClass.Process_name_Status.Count;

            foreach (var itm in CommonClass.Process_name_Status)
            {
                Color back_clr = System.Drawing.Color.Red;
                Color fore_clr = System.Drawing.Color.White;
                string ActionType = "GetColor";
                string[] str_exist = { "@cust_cd", "@item_cd", "@pro_id", "@lotno", "@lotno_child", "@material_cd", "@ActionType" };
                string[] obj_exist = { txtCustomerCode.Text, txt_lotinfo_itemcode.Text, itm.process_id, lotno, lotno_child, itm.materialcode, ActionType };
                MySqlDataReader getColor = helper.GetReaderByCmd("get_button_color", str_exist, obj_exist);
                if (getColor.Read())
                {
                    string get_processid = getColor["lotinfo_mast"].ToString();
                    string get_processid_temp = getColor["lotinfo_mast_temp"].ToString();
                    if (get_processid != "0" || get_processid_temp != "0")
                    {
                        back_clr = Color.Green;
                    }

                }
                getColor.Close();
                helper.CloseConnection();
                string getid = itm.id;

                // Production information tab : selected partnumber only button create
                if (getid != "XXX")
                {
                    //This block dynamically creates a Button and adds it to the form
                    Button btn = new Button();

                    btn.BackColor = back_clr;
                    btn.ForeColor = fore_clr;
                    btn.Location = new System.Drawing.Point(19, 29);
                    btn.Name = itm.id + "#" + itm.PaternType + "#" + itm.ProcessNames + "#" + itm.process_id;
                    btn.Size = new System.Drawing.Size(80, 60);
                    btn.TabIndex = 103;
                    btn.Text = itm.ProcessNames;
                    btn.UseVisualStyleBackColor = false;
                    //btn.Click += new System.EventHandler(this.Patern_Click);
                    btn.Location = new Point(i, x);
                    panel1.AutoScroll = true;
                    panel1.Controls.Add(btn);

                    i += 100;
                }


            }
        }
        public void grid_bind(string ActionType, string lotn, string lotn_frm, string lotn_to, string customer_code, string item_code, string manf_dt_frm, string manf_dt_to, string ship_dt_frm, string ship_dt_to, string auctionrole)
        {
            try
            {
                DataSet ds_view = new DataSet();
                DataTable dtable_ps = new DataTable();
                dataGridView2.DataSource = dtable_ps;
                dataGridView2.DataSource = null;
                dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.AllowUserToAddRows = true;

                string[] str_view = { "@lotno", "@lotno_frm", "@lotno_to", "@cust_cd", "@itm_cd", "@shipdate_frm", "@shipdate_to", "@manf_frm", "@manf_to", "@ActionType", "@Actionrole" };
                string[] obj_view = { lotn, lotn_frm, lotn_to, customer_code, item_code, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), date_manf_frm.Value.ToString("yyyy-MM-dd"), date_manf_to.Value.ToString("yyyy-MM-dd"), ActionType, auctionrole };
                //dtable_ps = helper.GetDatasetByCommandString_dt("Production_status_shipment_details", str_view, obj_view);
                dtable_ps = helper.GetDatasetByCommandString_dt("Production_status_shipment_det", str_view, obj_view);

                var allDuplicates = dtable_ps.AsEnumerable()
                          .GroupBy(dr => dr.Field<string>("lotnoandchild"))
                          .Where(g => g.Count() > 1)
                          .SelectMany(g => g)
                          .ToList();
                var cleaning_only_or_inspection_only = dtable_ps.AsEnumerable()
                    .GroupBy(dr => dr.Field<string>("lotnoandchild"))
                    .Where(g => g.Count() <= 1)
                    .SelectMany(g => g)
                    .ToList();
                //var query = dtable.Select(x => x.Field<string>("lotno_child")).Distinct();

                int index = 0;
                if (dtable_ps.Rows.Count > 0)
                {
                    List<string> already_exits_row = new List<string>();
                    foreach (DataRow drow in dtable_ps.Rows)
                    {
                        string lotnoandchild_bind = drow["lotnoandchild"].ToString();
                        // lot no format change                        
                        string dG1joinlotno = drow["lotnoandchild"].ToString();
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
                        if (already_exits_row.Contains(drow["lotnoandchild"].ToString()) == false)
                        {
                            dataGridView2.Rows.Add();

                            dataGridView2.Rows[index].Cells[0].Value = dG1joinlotno;
                            dataGridView2.Rows[index].Cells[1].ReadOnly = true;
                            dataGridView2.Rows[index].Cells[2].Value = drow["customercode"];
                            dataGridView2.Rows[index].Cells[3].Value = drow["item_code"];
                            dataGridView2.Rows[index].Cells[4].Value = drow["item_name"];
                            dataGridView2.Rows[index].Cells[5].Value = drow["manufacturing_date"];
                            dataGridView2.Rows[index].Cells[6].Value = drow["expairy_dt"];
                            dataGridView2.Rows[index].Cells[7].Value = drow["lotqty"];
                            string process_id = drow["process_id"].ToString();

                            if (process_id == "101")
                            {
                                dataGridView2.Rows[index].Cells[9].Value = drow["process_date"];

                                var duplicates = dtable_ps.AsEnumerable()
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
                                //string current_lotnoandchild = dG1joinlotno;
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
                                var duplicates = dtable_ps.AsEnumerable()
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
                                //string current_lotnoandchild = dG1joinlotno;
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
                            dataGridView2.Columns[10].DefaultCellStyle.Format = "dd/MM/yyyy";
                            string shipment_date = drow["shipmentdate"].ToString();
                            dataGridView2.Rows[index].Cells[10].Value = shipment_date;
                            dataGridView2.Rows[index].Cells[12].Value = drow["shortname"];
                            dataGridView2.Rows[index].Cells[13].Value = drow["customerfull_name"];
                            if (shipment_date == "-")
                            {
                                dataGridView2.Rows[index].Cells[1].Value = CheckState.Checked;
                                dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                // checked the row 
                                bool flag = false;
                                List<shippingUpdate> list_cmodel = new List<shippingUpdate>();
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
                                        if (Convert.ToString(row.Cells["Lotno"].Value) == string.Empty)
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
                                            CommonClass.shipping_update_lotno.Add(model);
                                        }

                                    }
                                    else if (!Convert.ToBoolean(chk.Value))
                                    {
                                        string lotnoandchild = row.Cells[0].Value.ToString();
                                        string lotno = lotnoandchild.Split('-')[0];
                                        string lotno_from = lotnoandchild.Split('-')[1];
                                        CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotno && x.lotno_from == lotno_from);
                                        CommonClass.shipping_update_lotno.Distinct().ToList();
                                    }
                                }
                            }
                            else
                            {
                                dataGridView2.Rows[index].Cells[1].Value = CheckState.Unchecked;
                                dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                            index++;

                            already_exits_row.Add(drow["lotnoandchild"].ToString());
                            //already_exits_row.Add(dG1joinlotno);
                        }


                    }
                }
                else
                {
                    //  MessageBox.Show("No Records Found..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.dataGridView2.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void textLotNo_Leave(object sender, EventArgs e)
        {
            if (textLotNo.Text != string.Empty)
            {
                //int formate_type = Convert.ToInt32(textLotNo.Text);
                //textLotNo.Text = formate_type.ToString("D7");
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

        private void textLotNoAdd_Leave(object sender, EventArgs e)
        {
            if (textLotNoAdd.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(textLotNoAdd.Text);
                textLotNoAdd.Text = formate_type.ToString("D7");
            }
        }

        private void textLotNoChild_Leave(object sender, EventArgs e)
        {
            if (textLotNoChild.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(textLotNoChild.Text);
                textLotNoChild.Text = formate_type.ToString("D2");
            }
        }

        private void txt_pl_lotno_Leave(object sender, EventArgs e)
        {
            if (txt_pl_lotno.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_pl_lotno.Text);
                txt_pl_lotno.Text = formate_type.ToString("D7");
            }
        }

        private void txt_pl_frm_lotc_Leave(object sender, EventArgs e)
        {
            if (txt_pl_frm_lotc.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_pl_frm_lotc.Text);
                txt_pl_frm_lotc.Text = formate_type.ToString("D2");
            }
        }

        private void txt_pl_to_lotc_Leave(object sender, EventArgs e)
        {
            if (txt_pl_to_lotc.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_pl_to_lotc.Text);
                txt_pl_to_lotc.Text = formate_type.ToString("D2");
            }
        }

        private void btn_productinfolist_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Product Information List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //MergeCells();

                    if (dGProductInfoList.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        List<string> Date_column_names = new List<string>();
                        List<int> Date_column_index = new List<int>();
                        Date_column_names.Add("Lot no");


                        Microsoft.Office.Interop.Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                        //XcelApp.Application.Workbooks.Add(Type.Missing);

                        Excel._Workbook oWB;
                        Excel._Worksheet ws;
                        XcelApp.DisplayAlerts = false;
                        oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                        ws = (Excel._Worksheet)oWB.ActiveSheet;

                        int get_date_column = 0;
                        for (int i = 1; i < dGProductInfoList.Columns.Count - 2; i++)
                        {
                            if (Date_column_names.Contains(dGProductInfoList.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[1, i] = dGProductInfoList.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dGProductInfoList.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[1, i] = dGProductInfoList.Columns[i - 1].HeaderText;
                                Date_column_index.Add(get_date_column);
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dGProductInfoList.Rows.Count; i++)
                        {
                            for (int j = 0; j < dGProductInfoList.Columns.Count - 3; j++)
                            {
                                if (Convert.ToString(dGProductInfoList.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check Lotno column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGProductInfoList.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {

                                        int formate_type = Convert.ToInt32(dGProductInfoList.Rows[i].Cells[j].Value.ToString());
                                        string lotnoD6 = formate_type.ToString("D7");
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

                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProductInfoList.Rows.Count, dGProductInfoList.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProductInfoList.Columns.Count]].Font.Bold = true;

                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGProductInfoList.Columns.Count]].Font.Size = 13;

                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                    
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Product Information Status List -" + datetime;
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

        private void btn_lotinfo_download_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download LotInformation List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //MergeCells();
                    if (dataGridView1.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        List<string> Date_column_names = new List<string>();
                        List<int> Date_column_index = new List<int>();
                        Date_column_names.Add("Manufacturing Date");
                        Date_column_names.Add("Expiry Date");
                        Date_column_names.Add("Process Date");
                        Date_column_names.Add("Planting Date");
                        ///
                        Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                        //XcelApp.Application.Workbooks.Add(Type.Missing);
                        Excel.Range oRng;
                        Excel._Workbook oWB;
                        Excel._Worksheet ws;
                        XcelApp.DisplayAlerts = false;
                        oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                        ws = (Excel._Worksheet)oWB.ActiveSheet;
                        int top_i = 8;
                        // Column Header 1                       
                        oRng = ws.get_Range("A1", "F1");
                        oRng.Value2 = "";
                        oRng.Merge(Missing.Value);
                        foreach (var topheader in CommonClass.Process_name_gridbind_Status)
                        {
                            //XcelApp.Range()
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
                                Excel.Range c1 = ws.Cells[1, 6];
                                top_i = top_i + 4;
                                Excel.Range c2 = ws.Cells[1, top_i];
                                oRng = (Excel.Range)ws.get_Range(c1, c2);
                                oRng.Value2 = topheader.ProcessNames;
                                oRng.Merge(Missing.Value);
                            }

                            //oRng = ws.get_Range(array[1].start, array[top_i].ends);
                            //oRng.Value2 = topheader.ProcessNames;
                            //oRng.Merge(Missing.Value);
                            //XcelApp.Cells[1, top_i + 5] = topheader.ProcessNames;

                            top_i++;
                        }
                        // Column Header 2
                        int get_date_column = 0;
                        for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                        {
                            XcelApp.Cells[2, 1] = "LotNo.";
                            XcelApp.Cells[2, 2] = "Customer Code";
                            XcelApp.Cells[2, 3] = "Customer Name";
                            XcelApp.Cells[2, 4] = "Item Code";
                            XcelApp.Cells[2, 5] = "Item Name";
                            if (Date_column_names.Contains(dataGridView1.Columns[i - 1].HeaderText) == false)
                            {
                                XcelApp.Cells[2, i + 5] = dataGridView1.Columns[i - 1].HeaderText;
                            }
                            else if (Date_column_names.Contains(dataGridView1.Columns[i - 1].HeaderText) == true)
                            {
                                XcelApp.Cells[2, i + 5] = dataGridView1.Columns[i - 1].HeaderText;
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
                                    XcelApp.Cells[3 + i, 2] = txtCustomerCode.Text;
                                    XcelApp.Cells[3 + i, 3] = txtCustomerNameS.Text;
                                    XcelApp.Cells[3 + i, 4] = txt_lotinfo_itemcode.Text;
                                    XcelApp.Cells[3 + i, 5] = txt_lotinfo_itm_nam.Text;

                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 3, j + 6] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        if (Convert.ToString(dataGridView1.Rows[i].Cells[j].Value) != string.Empty)
                                        {
                                            string date_val = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                            DateTimePicker dt = new DateTimePicker();
                                            dt.Value = Convert.ToDateTime(date_val,
                                            System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                            DateTime convertformateDate = Convert.ToDateTime(date_val.Replace("\"", ""), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                            //XcelApp.Cells[i + 3, j + 6] = dt.Value.ToShortDateString();
                                            XcelApp.Cells[i + 3, j + 6] = convertformateDate;
                                        }
                                        else
                                        {
                                            XcelApp.Cells[i + 3, j + 6] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                        }
                                        Excel.Range d1 = ws.Cells[i + 3, j + 6];
                                        Excel.Range d2 = ws.Cells[i + 3, j + 6];
                                        XcelApp.Range[d1, d2].EntireColumn.NumberFormat = "dd-mm-yyyy";
                                    }
                                }
                                else
                                {
                                    XcelApp.Cells[i + 3, j + 6] = string.Empty;
                                }
                            }
                        }
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
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Lot Information status List -" + datetime;
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

        private void btn_shippeddetails_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Shippment List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Console.WriteLine("Download start time : " + DateTime.Now.ToString("HH:mm:ss"));
                        copyAlltoClipboard();
                        Microsoft.Office.Interop.Excel.Application XcelApp;
                        Microsoft.Office.Interop.Excel.Workbook oWB;
                        Microsoft.Office.Interop.Excel.Worksheet ws;
                        object misValue = System.Reflection.Missing.Value;
                        XcelApp = new Excel.Application();
                        oWB = XcelApp.Workbooks.Add(misValue);
                        ws = oWB.ActiveSheet;
                        ws = (Excel.Worksheet)oWB.Worksheets.get_Item(1);
                        Excel.Range CR = (Excel.Range)ws.Cells[3, 1];
                        CR.Select();
                        ws.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                        // Accessing the first worksheet in the Excel file
                        // Insert a new column C.
                        ws.Columns["B"].Insert();
                        ws.Columns["C"].Insert();
                        ws.Columns["D"].Insert();
                        ws.Columns["E"].Insert();
                        for (int i = 0; i < dataGridView3.Rows.Count; i++)
                        {
                            XcelApp.Cells[3 + i, 2] = txtCustomerCode.Text;
                            XcelApp.Cells[3 + i, 3] = txtCustomerNameS.Text;
                            XcelApp.Cells[3 + i, 4] = txt_lotinfo_itemcode.Text;
                            XcelApp.Cells[3 + i, 5] = txt_lotinfo_itm_nam.Text;
                        }
                        //Console.WriteLine("whole values and default column end time : " + DateTime.Now.ToString("HH:mm:ss"));
                        //List<string> Date_column_names = new List<string>();
                        //List<int> Date_column_index = new List<int>();
                        //Date_column_names.Add("Manufacturing Date");
                        //Date_column_names.Add("Expiry Date");
                        //Date_column_names.Add("Process Date");
                        //Date_column_names.Add("Planting Date");       
                        Excel.Range oRng;
                        //Excel._Workbook oWB;
                        //Excel._Worksheet ws;
                        XcelApp.DisplayAlerts = false;
                        // oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                        // ws = (Excel._Worksheet)oWB.ActiveSheet;
                        int top_i = 8;
                        // Column Header 1 
                        List<ObjColumns> array = new List<ObjColumns>();
                        array.Add(new ObjColumns("A1", "F1"));
                        oRng = ws.get_Range("A1", "F1");
                        oRng.Value2 = "";
                        oRng.Merge(Missing.Value);
                        foreach (var topheader in CommonClass.Process_name_gridbind_columns_shipping)
                        {
                            //XcelApp.Range()
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
                                Excel.Range c1 = ws.Cells[1, 6];
                                //top_i = top_i + 2;
                                top_i = top_i + 6;
                                Excel.Range c2 = ws.Cells[1, top_i];
                                oRng = (Excel.Range)ws.get_Range(c1, c2);
                                oRng.Value2 = topheader.ProcessNames;
                                oRng.Merge(Missing.Value);
                            }
                            top_i++;
                        }
                        //Console.WriteLine("header 1 end time : " + DateTime.Now.ToString("HH:mm:ss"));
                        // Column Header 2
                        int get_date_column = 0;
                        bool skip_columns_lotno = false;
                        bool skip_columns_after = false;
                        bool skip_columns_lotnochild = false;
                        int reduct_count_two = 0;
                        for (int i = 1; i < dataGridView3.Columns.Count + 1; i++)
                        {
                            int cell_count = i;
                            XcelApp.Cells[2, 1] = "LotNo.";
                            XcelApp.Cells[2, 2] = "Customer Code";
                            XcelApp.Cells[2, 3] = "Customer Name";
                            XcelApp.Cells[2, 4] = "Item Code";
                            XcelApp.Cells[2, 5] = "Item Name";
                            string skip_Lotno = dataGridView3.Columns[i - 1].HeaderText;

                            if (skip_Lotno == "Lotno" || skip_Lotno == "LotnoChild")
                            {
                                skip_columns_lotno = true;
                                if (skip_Lotno == "LotnoChild")
                                {
                                    skip_columns_after = true;
                                }
                            }
                            if (!skip_columns_after)
                            {
                                if (!skip_columns_lotno)
                                {
                                    XcelApp.Cells[2, i + 5] = dataGridView3.Columns[i - 1].HeaderText;
                                }
                                else if (skip_columns_lotno)
                                {
                                    reduct_count_two = cell_count + 1;
                                }
                            }
                            else if (skip_columns_after && !skip_columns_lotno)
                            {
                                reduct_count_two = cell_count + 1;
                                skip_columns_lotno = false;
                            }
                            else if (skip_columns_after && skip_columns_lotno)
                            {
                                if (skip_columns_lotnochild)
                                {
                                    XcelApp.Cells[2, i + 3] = dataGridView3.Columns[i - 1].HeaderText;
                                }
                                else
                                {
                                    skip_columns_lotnochild = true;
                                }

                            }
                            //if (Date_column_names.Contains(dataGridView1.Columns[i - 1].HeaderText) == false)
                            //{
                            //    XcelApp.Cells[2, i + 5] = dataGridView1.Columns[i - 1].HeaderText;
                            //}
                            //else if (Date_column_names.Contains(dataGridView1.Columns[i - 1].HeaderText) == true)
                            //{
                            //    XcelApp.Cells[2, i + 5] = dataGridView1.Columns[i - 1].HeaderText;
                            //    Date_column_index.Add(get_date_column);
                            //}
                            get_date_column++;
                        }

                        Excel.Range DeleteRange_G = XcelApp.Range["G:G"];
                        DeleteRange_G.Delete();
                        //Console.WriteLine("Header 2 end time : " + DateTime.Now.ToString("HH:mm:ss"));
                        //  Auto fit automatically adjust the width of columns of Excel  in givien range .                   
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView3.Rows.Count, dataGridView3.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView3.Columns.Count + 5]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[dataGridView3.Columns.Count + 5]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dataGridView3.Columns.Count]].Font.Size = 13;
                        XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[1, dataGridView3.Columns.Count]].Font.Size = 12;
                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();

                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Shipment Details -" + datetime;
                        string newFileName = path + compinepath;
                        // Now save this file.
                        ws.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12);
                        XcelApp.Visible = true;
                        dataGridView3.ClearSelection();
                        Console.WriteLine("Download end time : " + DateTime.Now.ToString("HH:mm:ss"));
                        Cursor.Current = Cursors.Default;
                    }

                    /////////////////////////
                    ///
                    //if (dataGridView3.Rows.Count > 0)
                    //{
                    //    Cursor.Current = Cursors.WaitCursor;
                    //    List<string> Date_column_names = new List<string>();
                    //    List<int> Date_column_index = new List<int>();
                    //    Date_column_names.Add("Shipment Date");
                    //    Date_column_names.Add("Manufacturing Date");
                    //    Date_column_names.Add("Expiry Date");
                    //    Date_column_names.Add("Process Date");
                    //    Date_column_names.Add("Planting Date"); 
                    //    ///
                    //    Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                    //    //XcelApp.Application.Workbooks.Add(Type.Missing);
                    //    Excel.Range oRng;
                    //    Excel._Workbook oWB;
                    //    Excel._Worksheet ws;
                    //    XcelApp.DisplayAlerts = false;
                    //    oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                    //    ws = (Excel._Worksheet)oWB.ActiveSheet;
                    //    int top_i = 7;
                    //    // Column Header 1 
                    //    List<ObjColumns> array = new List<ObjColumns>();
                    //    array.Add(new ObjColumns("A1", "J1"));
                    //    //array.Add(new ObjColumns("F1", "H1"));
                    //    //array.Add(new ObjColumns("I1", "K1"));
                    //    oRng = ws.get_Range("A1", "J1");
                    //    oRng.Value2 = "TERMINAL BOARD INFO";
                    //    oRng.Merge(Missing.Value);
                    //    Missing miss = Missing.Value;
                    //    foreach (var topheader in CommonClass.Process_name_gridbind_columns_shipping)
                    //    {
                    //        //XcelApp.Range()
                    //        if (topheader.ProcessNames != "TERMINAL BOARD INFO")
                    //        {
                    //            if (topheader.PaternType == "1")
                    //            {
                    //                Excel.Range d1 = ws.Cells[1, top_i];
                    //                top_i = top_i + 4;
                    //                Excel.Range d2 = ws.Cells[1, top_i];
                    //                oRng = (Excel.Range)ws.get_Range(d1, d2);
                    //                oRng.Value2 = topheader.ProcessNames;
                    //                oRng.Merge(Missing.Value);
                    //            }
                    //            else if (topheader.PaternType == "2")
                    //            {
                    //                Excel.Range d1 = ws.Cells[1, top_i];
                    //                top_i = top_i + 3;
                    //                Excel.Range d2 = ws.Cells[1, top_i];
                    //                oRng = (Excel.Range)ws.get_Range(d1, d2);
                    //                oRng.Value2 = topheader.ProcessNames;
                    //                oRng.Merge(Missing.Value);
                    //            }
                    //            else if (topheader.PaternType == "3")
                    //            {
                    //                Excel.Range d1 = ws.Cells[1, top_i];
                    //                top_i = top_i + 1;
                    //                Excel.Range d2 = ws.Cells[1, top_i];
                    //                oRng = (Excel.Range)ws.get_Range(d1, d2);
                    //                oRng.Value2 = topheader.ProcessNames;
                    //                oRng.Merge(Missing.Value);
                    //            }
                    //            else if (topheader.PaternType == "4")
                    //            {
                    //                Excel.Range d1 = ws.Cells[1, top_i];
                    //                top_i = top_i + 2;
                    //                Excel.Range d2 = ws.Cells[1, top_i];
                    //                oRng = (Excel.Range)ws.get_Range(d1, d2);
                    //                oRng.Value2 = topheader.ProcessNames;
                    //                oRng.Merge(Missing.Value);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Excel.Range c1 = ws.Cells[1, 6];
                    //            top_i = top_i + 4;
                    //            Excel.Range c2 = ws.Cells[1, top_i];
                    //            oRng = (Excel.Range)ws.get_Range(c1, c2);
                    //            oRng.Value2 = topheader.ProcessNames;
                    //            oRng.Merge(Missing.Value);
                    //        }

                    //        //oRng = ws.get_Range(array[1].start, array[top_i].ends);
                    //        //oRng.Value2 = topheader.ProcessNames;
                    //        //oRng.Merge(Missing.Value);
                    //        //XcelApp.Cells[1, top_i + 5] = topheader.ProcessNames;

                    //        top_i++;
                    //    }
                    //    // Column Header 2
                    //    int get_date_column = 0;
                    //    for (int i = 1; i < dataGridView3.Columns.Count + 1; i++)
                    //    {
                    //        XcelApp.Cells[2, 1] = "LotNo.";
                    //        //XcelApp.Cells[2, 2] = "Customer Code";
                    //        //XcelApp.Cells[2, 3] = "Customer Name";
                    //        //XcelApp.Cells[2, 4] = "Item Code";
                    //        //XcelApp.Cells[2, 5] = "Item Name";
                    //        if (Date_column_names.Contains(dataGridView3.Columns[i - 1].HeaderText) == false)
                    //        {
                    //            XcelApp.Cells[2, i + 2] = dataGridView3.Columns[i - 1].HeaderText;
                    //        }
                    //        else if (Date_column_names.Contains(dataGridView3.Columns[i - 1].HeaderText) == true)
                    //        {
                    //            XcelApp.Cells[2, i + 2] = dataGridView3.Columns[i - 1].HeaderText;
                    //            Date_column_index.Add(get_date_column);
                    //        }
                    //        get_date_column++;
                    //    }
                    //    // Row header 
                    //    for (int i = 1; i < dataGridView3.Rows.Count + 1; i++)
                    //    {
                    //        XcelApp.Cells[i + 2, 1] = dataGridView3.Rows[i - 1].HeaderCell.Value.ToString();
                    //    }
                    //    // Row general details 
                    //    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    //    {
                    //        for (int j = 0; j < dataGridView3.Columns.Count; j++)
                    //        {
                    //            if (Convert.ToString(dataGridView3.Rows[i].Cells[j].Value) != string.Empty)
                    //            {
                    //                //XcelApp.Cells[3 + i, 2] = dataGridView3.Rows[i].Cells[j].Value.ToString(); ;
                    //                //XcelApp.Cells[3 + i, 3] = txtCustomerNameF.Text;
                    //                //XcelApp.Cells[3 + i, 4] = textItemCode.Text;
                    //                //XcelApp.Cells[3 + i, 5] = txt_itemname.Text;

                    //                if (Date_column_index.Contains(j) == false)
                    //                {
                    //                    XcelApp.Cells[i + 3, j + 3] = dataGridView3.Rows[i].Cells[j].Value.ToString();
                    //                }
                    //                else if (Date_column_index.Contains(j) == true)
                    //                {
                    //                    if (Convert.ToString(dataGridView3.Rows[i].Cells[j].Value) != string.Empty && Convert.ToString(dataGridView3.Rows[i].Cells[j].Value) != "-")
                    //                    {
                    //                        string date_val = dataGridView3.Rows[i].Cells[j].Value.ToString();
                    //                        DateTimePicker dt = new DateTimePicker();
                    //                        dt.Value = Convert.ToDateTime(date_val,
                    //                        System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    //                        DateTime convertformateDate = Convert.ToDateTime(date_val.Replace("\"", ""), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    //                        //XcelApp.Cells[i + 3, j + 3] = dt.Value.ToShortDateString();
                    //                        XcelApp.Cells[i + 3, j + 3] =convertformateDate;
                    //                        //date_val = oDate.ToShortDateString();
                    //                        //XcelApp.Cells[i + 3, j + 6] = date_val;
                    //                    }
                    //                    else
                    //                    {
                    //                        XcelApp.Cells[i + 3, j + 3] = dataGridView3.Rows[i].Cells[j].Value.ToString();
                    //                    }
                    //                    Excel.Range d1 = ws.Cells[i + 3, j + 3];
                    //                    Excel.Range d2 = ws.Cells[i + 3, j + 3];
                    //                    //XcelApp.Range[d1, d2].Style.HorizontalAlignment = HorizontalAlignType.Left;                                        
                    //                    XcelApp.Range[d1, d2].EntireColumn.NumberFormat = "dd-mm-yyyy";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                XcelApp.Cells[i + 3, j + 3] = string.Empty;
                    //            }
                    //        }
                    //    }
                    //    Excel.Range copyRange_B = XcelApp.Range["B:B"];
                    //    Excel.Range DeleteRange_D = XcelApp.Range["D:D"];
                    //    DeleteRange_D.Delete();
                    //   // Excel.Range insertRange_C = XcelApp.Range["C:C"];
                    //    // insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_B.Cut());
                    //    //insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_B.Copy());
                    //    copyRange_B.Delete();
                    //    //DeleteRange_H.Delete();
                    //    //DeleteRange_I.Delete();
                    //    //DeleteRange_J.Delete();

                    //    // Auto fit automatically adjust the width of columns of Excel  in givien range .  
                    //    XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView3.Rows.Count, dataGridView3.Columns.Count]].EntireColumn.AutoFit();
                    //    XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView3.Columns.Count + 5]].Font.Bold = true;
                    //    XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[dataGridView3.Columns.Count + 5]].Font.Bold = true;
                    //    XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dataGridView3.Columns.Count]].Font.Size = 13;
                    //    XcelApp.Range[XcelApp.Cells[2, 1], XcelApp.Cells[1, dataGridView3.Columns.Count]].Font.Size = 12;
                    //    XcelApp.Columns.Borders.Color = Color.Black;
                    //    XcelApp.Columns.AutoFit();
                    //    XcelApp.Visible = true;
                    //    DateTime current_date = DateTime.Now;
                    //    DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                    //    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    //    string CreateFolder = "C:\\TMPS";
                    //    string FileName = CreateFolder +"\\Shipment Details";
                    //    CheckDirectory(CreateFolder);                       
                    //    string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                    //    string compinepath = "\\Shipping Status List -" + datetime;
                    //    string newFileName = CreateFolder + compinepath;
                    //    // Now save this file.
                    //    ws.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12);
                    //    Cursor.Current = Cursors.Default;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("No Record To Export !!!", "Info");
                    //}




                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void copyAlltoClipboard()
        {
            dataGridView3.SelectAll();
            DataObject dataObj = dataGridView3.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void FormProductionStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnSearch.PerformClick();
            }
            if (e.KeyCode == Keys.F6)
            {
                btn_productinfolist.PerformClick();
            }          
            if (e.KeyCode == Keys.F8)
            {
                btn_shippeddetails.PerformClick();
            }
            if (e.KeyCode == Keys.F9)
            {
                btnclose.PerformClick();
            }
            if (e.KeyCode == Keys.F7)
            {
                if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])//your specific tabname
                {
                    btn_productinfolist.PerformClick();
                }
                if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])//your specific tabname
                {
                    btn_lotinfo_download.PerformClick();
                }
                if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage3"])//your specific tabname
                {
                    btn_shippeddetails.PerformClick();
                }


            }
        }

        private void textLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_lotno_frm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_lotno_to_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCustomerCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void txtCustomerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtCustomerCode.Text != "" && txtCustomerCode.Text != "000000")
                {
                    DataSet ds = helper.GetDatasetByClientcodeNames(txtCustomerCode.Text, string.Empty);
                    DataTable dt = new DataTable();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                        txtCustomerNameS.Text = dt.Rows[0]["fullname"].ToString();
                        chk_customer.Checked = true;
                        helper.CloseConnection();
                    }
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
                        DataSet ds = helper.GetDatasetByClientcodeNames(txtCustomerCode.Text, string.Empty);
                        DataTable dt = new DataTable();

                        string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
                        string[] obj = { txtCustomerCode.Text, string.Empty, textItemCode.Text, "GetDataCustomerItem" };
                        ds = helper.GetDatasetByCommandString("product_view", str, obj);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dt = ds.Tables[0];
                            textItemName.Text = dt.Rows[0]["itemname"].ToString();
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

        private void textLotNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomerCode_Leave(object sender, EventArgs e)
        {
            if (txtCustomerCode.Text != string.Empty)
            {
               // int formate_type = Convert.ToInt32(txtCustomerCode.Text);
               // txtCustomerCode.Text = formate_type.ToString("D7");
            }
        }

        private void textLotNoAdd_TextChanged(object sender, EventArgs e)
        {
            int check_lotchild_value = 0;
            int check_lot_value = 0;
            if (textLotNoAdd.Text != string.Empty)
            {
                check_lot_value = Convert.ToInt32(textLotNoAdd.Text);
            }
            if (textLotNoChild.Text != string.Empty)
            {
                check_lotchild_value = Convert.ToInt32(textLotNoChild.Text);
            }
            if (check_lot_value > 0 && check_lotchild_value > 0)
            {
                color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
            }
        }

        private void textLotNoChild_TextChanged(object sender, EventArgs e)
        {
            int check_lotchild_value = 0;
            if (textLotNoChild.Text != string.Empty)
            {
                check_lotchild_value = Convert.ToInt32(textLotNoChild.Text);
            }
            if (check_lotchild_value > 0)
            {
                color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
            }

        }
        public void shipment_gridbind_single_lot(string lotno, string lotno_frm, string lotno_to, string manfdt_frm, string manfdt_to, string customer_cd, string item_cd, string GetActionType, string ship_tabActionType, string sp_name, string GetAcutionType2)
        {
            try
            {
                terminal_addlist_loadgrid_call("shipment_others");
                ///
                List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
              
                string get_pattern_type = string.Empty;
                string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                string[] obj = { lotno, lotno_frm, lotno_to, manfdt_frm, manfdt_to, customer_cd, item_cd, GetActionType };
                DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                if (dtable_spm.Rows.Count > 0)
                {
                    List<string> already_exits_row = new List<string>();
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
                ///
                already_exits_row_header = new List<string>();
                already_exits_row_header_lotno_only = new List<string>();
                row_header_lotno_all_combined = new List<string>();
                already_exits_row_columns = new List<string>();
                if (get_cust_itemcd.Count > 0)
                {
                    foreach (var get_cd in get_cust_itemcd)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);

                        // insert_lotinfo_value_assign_gridbind(ship_tabActionType, get_cd.lotno, textLotNoChild.Text, textLotNoChild.Text, dateTimePicker_ship_frm.Value.ToString("yyyy-MM-dd"), dateTimePicker_ship_to.Value.ToString("yyyy-MM-dd"), get_cd.customer_code, get_cd.item_code, GetAcutionType2, sp_name, get_cd.customer_name, get_cd.item_name);

                        insert_lotinfo_value_assign_gridbind(ship_tabActionType, get_cd.lotno, textLotNoChild.Text, textLotNoChild.Text, manfdt_frm, manfdt_to, get_cd.customer_code, get_cd.item_code, GetAcutionType2, sp_name, get_cd.customer_name, get_cd.item_name);
                    }
                }
                else
                {
                    MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerCode.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void shipment_gridbind(string lotno, string lotno_frm,string lotno_to,string manfdt_frm,string manfdt_to,string customer_cd,string item_cd,string GetActionType,string ship_tabActionType,string sp_name,string GetAcutionType2)
        {
            try
            {                
                shipment_gridbind_dataLoad = true;
                shipment_gridbind_with_shpfilter_dataLoad = false;
                terminal_addlist_loadgrid_call("shipment_others");
                ///
                List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
               
                string get_pattern_type = string.Empty;
                string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                string[] obj = { lotno, lotno_frm,lotno_to,manfdt_frm, manfdt_to, customer_cd, item_cd, GetActionType };
                DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                if (dtable_spm.Rows.Count > 0)
                {
                    List<string> already_exits_row = new List<string>();
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
       
                ///
                already_exits_row_header = new List<string>();
                already_exits_row_header_lotno_only = new List<string>();
                row_header_lotno_all_combined = new List<string>();
                already_exits_row_columns = new List<string>();
                if (get_cust_itemcd.Count > 0)
                {
                    //for (int i = 0; i < get_cust_itemcd.Count; i = i + 100)
                    //{
                    //    var items = get_cust_itemcd.Skip(i).Take(100);
                    //    // Do something with 100 or remaining items
                    //}
                    // Add the list need to click next page...
                    CommonClass.Runtime_Store_Print_details = get_cust_itemcd.ToList();
                    CommonClass.ship_tabActionType_nxtPg = ship_tabActionType;
                    CommonClass.lotno_nxtPg = ship_tabActionType;
                    CommonClass.lotno_child_frm_nxtPg = lotno_frm;
                    CommonClass.lotno_child_to_nxtPg = lotno_to;
                    CommonClass.manfdt_frm_nxtPg = manfdt_frm;
                    CommonClass.manfdt_to_nxtPg = manfdt_to;
                    CommonClass.actionTyp2_nxtPg = GetAcutionType2;
                    CommonClass.spname_nxtPg = sp_name;
                    CommonClass.curentPageNo_nxtPg = PageNumber;
                    CommonClass.curentPageSize_nxtPg = PageSize;                   
                    // 
                    var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);
                    
                    if (Get_records.IsLastPage)
                    {
                        btn_nextPg.Enabled = false;
                    }
                    
                    foreach (var get_cd in Get_records)
                    {
                        helper.CloseConnection();
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);                        
                        insert_lotinfo_value_assign_gridbind(ship_tabActionType, get_cd.lotno, lotno_frm, lotno_to, manfdt_frm, manfdt_to, get_cd.customer_code, get_cd.item_code, GetAcutionType2, sp_name, get_cd.customer_name, get_cd.item_name);
                    }
                }
                else
                {
                    MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerCode.Focus();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void shipment_gridbind_with_shpfilter(string lotno, string lotno_frm, string lotno_to, string manfdt_frm, string manfdt_to, string customer_cd, string item_cd, string GetActionType, string ship_tabActionType, string sp_name, string GetAcutionType2,string shipment_date_frm,string shipment_date_to,string round_lotno)
        {
            try
            {
                shipment_gridbind_with_shpfilter_dataLoad = true;
                shipment_gridbind_dataLoad = false;                
                terminal_addlist_loadgrid_call("shipment_others");          
                List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();

                string get_pattern_type = string.Empty;
                string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@shipdt_frm", "@shipdt_to", "@ActionType", "@rlotno" };
                string[] obj = { lotno, lotno_frm, lotno_to, manfdt_frm, manfdt_to, customer_cd, item_cd,shipment_date_frm,shipment_date_to,GetActionType,round_lotno };
                DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno_with_shipmentdt", str, obj);
                if (dtable_spm.Rows.Count > 0)
                {
                    List<string> already_exits_row = new List<string>();
                    foreach (DataRow drow in dtable_spm.Rows)
                    {
                        shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                        model.customer_code = drow["customer_code"].ToString();
                        model.item_code = drow["item_code"].ToString();
                        //model.lotno = drow["lot_no"].ToString();
                        model.lotno = drow["lotnumber"].ToString();
                        model.customer_name = drow["customername"].ToString();
                        model.item_name = drow["item_name"].ToString();
                        get_cust_itemcd.Add(model);
                    }
                }
                // Add the list need to click next page...
                CommonClass.Runtime_Store_Print_details = get_cust_itemcd.ToList();
                CommonClass.ship_tabActionType_nxtPg = ship_tabActionType;
                CommonClass.lotno_nxtPg = ship_tabActionType;
                CommonClass.lotno_child_frm_nxtPg = lotno_frm;
                CommonClass.lotno_child_to_nxtPg = lotno_to;
                CommonClass.manfdt_frm_nxtPg = manfdt_frm;
                CommonClass.manfdt_to_nxtPg = manfdt_to;
                CommonClass.actionTyp2_nxtPg = GetAcutionType2;
                CommonClass.spname_nxtPg = sp_name;
                CommonClass.curentPageNo_nxtPg = PageNumber;
                CommonClass.curentPageSize_nxtPg = PageSize;
                CommonClass.ship_frmdt_nxtPg = shipment_date_frm;
                CommonClass.ship_todt_nxtPg = shipment_date_to;
                CommonClass.round_lotno_nxtPg = round_lotno;
            
                var Get_records = get_cust_itemcd.ToPagedList(PageNumber, PageSize);

                if (Get_records.IsLastPage)
                {
                    btn_nextPg.Enabled = false;
                }
                already_exits_row_header = new List<string>();
                already_exits_row_header_lotno_only = new List<string>();
                row_header_lotno_all_combined = new List<string>();
                already_exits_row_columns = new List<string>();
                if (Get_records.Count > 0)
                {
                   // Console.WriteLine("terminal_addlist_loadgrid_call insert : start time : " + DateTime.Now.ToString("HH:mm:ss"));
                    foreach (var get_cd in Get_records)
                    {
                        helper.CloseConnection();
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                        insert_lotinfo_value_assign_gridbind_shpmentdt(ship_tabActionType, get_cd.lotno, txt_lotno_frm.Text, txt_lotno_to.Text, manfdt_frm, manfdt_to, get_cd.customer_code, get_cd.item_code, GetAcutionType2, sp_name, get_cd.customer_name, get_cd.item_name,shipment_date_frm,shipment_date_to,round_lotno);                                                                
                    }
                   // Console.WriteLine("terminal_addlist_loadgrid_call insert : end time : " + DateTime.Now.ToString("HH:mm:ss"));
                }
                else
                {
                    MessageBox.Show("No Records Found ....", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerCode.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = helper.GetDatasetByBOMView_Pro_input_shipment(txtCustomerCode.Text, textItemCode.Text, ActionType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
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
            LoadDataGrid_shipment();
        }
        private void LoadDataGrid_shipment()
        {
            try
            {
                dataGridView3.DataSource = null;
                int total_process = CommonClass.Process_name_gridbind_shipping.Count;
                //int total_process = CommonClass.Process_name_gridbind_columns_shipping_runtime_filter.Count;
                // grid bind start
                int totalgroup = total_process;
                int year = DateTime.Now.Year;
                daysInMonths_d3 = new int[totalgroup]; // check line 129
                GroupLabel_d3 = new string[totalgroup];
                LabelString_d3 = new string[totalgroup, 10];
                LabelSize_d3 = new int[totalgroup, 10];
                List<KeyValuePair<int, string>> kvpList = new List<KeyValuePair<int, string>>();
                List<PI_Process> module = new List<PI_Process>();
                int i = 0;
                this.dataGridView3.Columns.Clear();
                foreach (var itm in CommonClass.Process_name_gridbind_shipping)
                //foreach(var itm in CommonClass.Process_name_gridbind_columns_shipping_runtime_filter)
                {
                    string getid = itm.id;
                    //if (selected_dgProduct_partnumber == getid || getid == "XXX")                    
                    //{
                    int key = Convert.ToInt16(itm.PaternType);
                    string values = itm.ProcessNames;
                    kvpList.Add(new KeyValuePair<int, string>(key, values));
                    if (i > 0)
                    {
                        GroupLabel_d3[i] = itm.ProcessNames;
                        if (key == 1)
                        {
                            LabelString_d3[i, 0] = "Part No.";
                            LabelString_d3[i, 1] = "Lot No.";
                            LabelString_d3[i, 2] = "Planting Date";
                            LabelString_d3[i, 3] = "Quantity";
                            LabelString_d3[i, 4] = "Pb";
                            LabelSize_d3[i, 0] = 80;
                            LabelSize_d3[i, 1] = 80;
                            LabelSize_d3[i, 2] = 120;
                            LabelSize_d3[i, 3] = 80;
                            LabelSize_d3[i, 4] = 80;
                        }
                        else if (key == 2)
                        {
                            LabelString_d3[i, 0] = "Process Date";
                            LabelString_d3[i, 1] = "Control No.";
                            LabelString_d3[i, 2] = "Sheet LotNo.";
                            LabelString_d3[i, 3] = "Quantity";
                            LabelSize_d3[i, 0] = 120;
                            LabelSize_d3[i, 1] = 120;
                            LabelSize_d3[i, 2] = 100;
                            LabelSize_d3[i, 3] = 80;
                        }
                        else if (key == 3)
                        {
                            LabelString_d3[i, 0] = "Process Date";
                            LabelString_d3[i, 1] = "Quantity";
                            LabelSize_d3[i, 0] = 120;
                            LabelSize_d3[i, 1] = 60;
                        }
                        else if (key == 4)
                        {
                            LabelString_d3[i, 0] = "Part No";
                            LabelString_d3[i, 1] = "Lot No";
                            LabelString_d3[i, 2] = "Quantity";
                            LabelSize_d3[i, 0] = 80;
                            LabelSize_d3[i, 1] = 80;
                            LabelSize_d3[i, 2] = 60;
                        }
                    }
                    else if (i == 0)
                    {
                        //GroupLabel_d3[0] = "TERMINAL BOARD INFO";
                        //LabelString_d3[0, 0] = "Shipment Date";
                        //LabelString_d3[0, 1] = "";
                        //LabelString_d3[0, 2] = "Customer Name";
                        //LabelString_d3[0, 3] = "Item Code";
                        //LabelString_d3[0, 4] = "Item Name";
                        //LabelString_d3[0, 5] = "Quantity";
                        //LabelString_d3[0, 6] = "Manufacturing Date";
                        //LabelString_d3[0, 7] = "Expiry Date";
                        //LabelSize_d3[0, 0] = 150;
                        //LabelSize_d3[0, 1] = 30;
                        //LabelSize_d3[0, 2] = 150;
                        //LabelSize_d3[0, 3] = 150;
                        //LabelSize_d3[0, 4] = 150;
                        //LabelSize_d3[0, 5] = 150;
                        //LabelSize_d3[0, 6] = 150;
                        //LabelSize_d3[0, 7] = 150;

                        GroupLabel_d3[0] = "TERMINAL BOARD INFO";
                        LabelString_d3[0, 0] = "Shipment Date";
                        LabelString_d3[0, 1] = "";
                        LabelString_d3[0, 2] = "Customer Code";
                        LabelString_d3[0, 3] = "Customer Name";
                        LabelString_d3[0, 4] = "Item Code";
                        LabelString_d3[0, 5] = "Item Name";
                        LabelString_d3[0, 6] = "Quantity";
                        LabelString_d3[0, 7] = "Manufacturing Date";
                        LabelString_d3[0, 8] = "Expiry Date";
                        LabelSize_d3[0, 0] = 150;
                        LabelSize_d3[0, 1] = 30;
                        LabelSize_d3[0, 2] = 150;
                        LabelSize_d3[0, 3] = 150;
                        LabelSize_d3[0, 4] = 150;
                        LabelSize_d3[0, 5] = 150;
                        LabelSize_d3[0, 6] = 150;
                        LabelSize_d3[0, 7] = 150;
                        LabelSize_d3[0, 8] = 150;
                    }

                    //}

                    i++;

                }
                daysInMonths_d3 = new int[GroupLabel_d3.Count()];
                // Add a column for each day of the year; where
                // column name = the date (creates all unique column names)
                // column header text = the numeric day of the month
                for (int month = 1; month <= kvpList.Count; month++)
                {
                    var element = kvpList.ElementAt(month - 1);
                    var Key = element.Key;
                    var Value = element.Value;
                    if (Key == 1)
                    {
                        daysInMonths_d3[month - 1] = 5;
                    }
                    else if (Key == 2)
                    {
                        daysInMonths_d3[month - 1] = 4;
                    }
                    else if (Key == 3)
                    {
                        daysInMonths_d3[month - 1] = 2;
                    }
                    else if (Key == 4)
                    {
                        daysInMonths_d3[month - 1] = 3;
                    }
                    else if (Key == 5)
                    {
                        //daysInMonths_d3[month - 1] = 8;
                        daysInMonths_d3[month - 1] = 9;
                    }
                    for (int day = 1; day <= daysInMonths_d3[month - 1]; day++)
                    {
                        //DateTime date = new DateTime(year, month, day);

                        string colname = "";
                        string colheadname = "";
                        int colsize = 120;

                        if (month <= totalgroup)
                        {
                            colname = LabelString_d3[month - 1, day - 1];
                            colheadname = LabelString_d3[month - 1, day - 1];
                            colsize = LabelSize_d3[month - 1, day - 1];

                        }
                        else
                        {
                            //colname = date.ToString();
                            //colheadname = day.ToString();
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
                            this.dataGridView3.Columns.Add(col);
                        }
                        else if (colname == string.Empty)
                        {
                            DataGridViewCheckBoxColumn col_chk = new DataGridViewCheckBoxColumn()
                            {
                                //Name = colname,
                                //HeaderText = colheadname,
                                Width = colsize

                            };
                            this.dataGridView3.Columns.Add(col_chk);
                        }

                    }
                }
                this.dataGridView3.AllowUserToAddRows = false;
                this.dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                this.dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                this.dataGridView3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                this.dataGridView3.Paint += dataGridView3_Paint;
                this.dataGridView3.Scroll += dataGridView3_Scroll;
                this.dataGridView3.ColumnWidthChanged += dataGridView3_ColumnWidthChanged;
                this.dataGridView3.Resize += dataGridView3_Resize;

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = helper.GetDatasetByBOMView_Pro_input_shipment(custcd, itemcd, ActionType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
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
        public void insert_lotinfo_value_assign_gridbind(string ActionTypeTwo, string lotn, string lotn_frm, string lotn_to, string manf_dt_frm, string manf_dt_to, string customer_code, string item_code, string auctionrole, string sp_name, string common_cust_name, string common_item_name)
        {
            try
            {
                if (chk_machine_no.Checked)
                {
                    string machine_no = "^" + lotn;
                    lotn = machine_no;
                }
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                List<Lotinfo_gridbind_common> list_lotinfo_Common = new List<Lotinfo_gridbind_common>();
                // lot information grid data's
                // p1
                string index = string.Empty;
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p1 = { ActionType_p1, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };

                //DataSet ds_pattern1 = helper.GetDatasetByCommandString(sp_name, str_p1, obj_p1);
                MySqlDataReader ds_pattern1 = helper.GetReaderByCmd(sp_name, str_p1, obj_p1);
                List<Lotinfo_gridbind_common_pattern_new_ship> m_model_p1 = LocalReportExtensions.GetList<Lotinfo_gridbind_common_pattern_new_ship>(ds_pattern1);
                List<Lotinfo_gridbind_common_pattern> clist_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.processName = "TERMINAL BOARD INFO";
                c_model.pattern_type = "5";
                list_cmodel.Add(c_model);
                //if(ds_pattern1.Tables[0].Rows.Count > 0)
                if(m_model_p1.Count > 0)
                {
                    Lotinfo_gridbind_common model_p1 = new Lotinfo_gridbind_common();
                    //foreach (DataRow dr in ds_pattern1.Tables[0].Rows)
                    m_model_p1.ForEach(dr =>
                    {
                        
                        string lotno_split = dr.lotnojoin_p1.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        //foreach (var lot in lotnumbers)
                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p1"].ToString();
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
                            //
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
                            //c_model.shipment_date = string.Empty;
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            list_cmodel.Add(c_model);
                        });
                    });
                }
                helper.CloseConnection();
                string ActionType_p2 = "p2view";
                string[] str_p2 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p2 = { ActionType_p2, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };
                //DataSet ds_pattern2 = helper.GetDatasetByCommandString(sp_name, str_p2, obj_p2);
                MySqlDataReader ds_pattern2 = helper.GetReaderByCmd(sp_name, str_p2, obj_p2);
                List<Lotinfo_gridbind_p2_ship> m_model_p2 = LocalReportExtensions.GetList<Lotinfo_gridbind_p2_ship>(ds_pattern2);
                if (m_model_p2.Count > 0)
                //if (ds_pattern2.Tables[0].Rows.Count > 0)
                {
                    Lotinfo_gridbind_common model_p2 = new Lotinfo_gridbind_common();
                    m_model_p2.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p2.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        //foreach (var lot in lotnumbers)
                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p2"].ToString();
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
                            //
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
                            //c_model.shipment_date = string.Empty;
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            list_cmodel.Add(c_model);
                        });
                    });

                }
                helper.CloseConnection();
                string ActionType_p3 = "p3view";
                string[] str_p3 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p3 = { ActionType_p3, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };
                //string[] obj_p3 = { ActionType_p3, textLotNoAdd.Text, cmbProcess.SelectedValue.ToString(), string.Empty };
                //DataSet ds_pattern3 = helper.GetDatasetByCommandString(sp_name, str_p3, obj_p3);
                //if (ds_pattern3.Tables[0].Rows.Count > 0)
                MySqlDataReader ds_pattern3 = helper.GetReaderByCmd(sp_name, str_p3, obj_p3);
                List<Lotinfo_gridbind_p3_ship> m_model_p3 = LocalReportExtensions.GetList<Lotinfo_gridbind_p3_ship>(ds_pattern3);
                if (m_model_p3.Count > 0)
                {
                    //  Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                    Lotinfo_gridbind_common model_p3 = new Lotinfo_gridbind_common();
                    //foreach (DataRow dr in ds_pattern3.Tables[0].Rows)
                    m_model_p3.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p3.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p3"].ToString();
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
                            //
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
                            list_cmodel.Add(c_model);
                        }
                    });

                }
                helper.CloseConnection();
                string ActionType_p4 = "p4view";
                string[] str_p4 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@Actionrole", "@Actionroletwo" };
                string[] obj_p4 = { ActionType_p4, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, auctionrole, ActionTypeTwo };
                //string[] obj_p4 = { ActionType_p4, textLotNoAdd.Text, cmbProcess.SelectedValue.ToString(), string.Empty };
                //DataSet ds_pattern4 = helper.GetDatasetByCommandString(sp_name, str_p4, obj_p4);
                //if (ds_pattern4.Tables[0].Rows.Count > 0)
                MySqlDataReader ds_pattern4 = helper.GetReaderByCmd(sp_name, str_p4, obj_p4);
                List<Lotinfo_gridbind_p4_ship> m_model_p4 = LocalReportExtensions.GetList<Lotinfo_gridbind_p4_ship>(ds_pattern4);
                if (m_model_p4.Count > 0)
                {
                    Lotinfo_gridbind_common model_p4 = new Lotinfo_gridbind_common();
                    //foreach (DataRow dr in ds_pattern4.Tables[0].Rows)
                    m_model_p4.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p4.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p4"].ToString();
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
                            //
                            c_model.processId = dr.processId_p4.ToString();
                            c_model.processName = dr.processName_p4.ToString();
                            c_model.partno = dr.partno_p4.ToString();
                            c_model.qty = dr.quantity_p4.ToString();
                            c_model.tb_manuf_dt = dr.tb_manuf_dt_p4.ToString();
                            c_model.tb_expairy_dt = dr.tb_expairy_dt_p4.ToString();
                            c_model.tb_qty = dr.tb_qty_p4.ToString();
                            //060623 c_model.lotno_p4 = dr.lotno_p4_p4.ToString();
                            c_model.lotno_p4 = dr.lotno_p4.ToString();
                            c_model.material_code = dr.materialcd.ToString();
                            //c_model.shipment_date = string.Empty;
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            list_cmodel.Add(c_model);
                        }

                    });

                }
                helper.CloseConnection();
                list_cmodel = list_cmodel.OrderBy(o => o.lotnojoin).ToList();
                // shipment date check after 2month means not show

                if (dataGridView3.Rows.Count >= 0)
                {
                    if (list_cmodel.Count > 1)
                    {                        
                        //already_exits_row_columns.AddRange(already_exits_row_header);
                        int header_lot_index = 0;
                        // Grid row header data get in list
                        foreach (var lotno in list_cmodel)
                        {
                            if (header_lot_index > 0)
                            {
                                if (!chk_expirydt.Checked)
                                {

                                    if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                    {
                                        row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        already_exits_row_header.Add(lotno.lotnojoin);
                                    }

                                }
                                else if (chk_expirydt.Checked)
                                {
                                    DateTime compare_date = DateTime.Parse(lotno.tb_expairy_dt.Split(',')[0]);

                                    int grater_than = DateTime.Compare(nowdate, compare_date);
                                    if (grater_than >= 0)
                                    {
                                        only_expiry_datas.Add(lotno.lotnojoin);
                                        //row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        //already_exits_row_header.Add(lotno.lotnojoin);
                                    }

                                    if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                    {
                                        row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        already_exits_row_header.Add(lotno.lotnojoin);
                                    }
                                }
                            }
                            header_lot_index++;
                        }
                        
                        dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        ///9022022
                        ///grid row header bind
                        ///list_cmodel.OrderBy(o => o.lotnojoin).ToList();                        
                       // row_header_lotno_all_combined = row_header_lotno_all_combined.OrderBy(i => i).ToList();
                        foreach (var rowheader in row_header_lotno_all_combined)
                        {
                            DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                            //// add  lot number  
                            First_row_dynamic_colm.CreateCells(this.dataGridView3);
                            First_row_dynamic_colm.HeaderCell.Value = rowheader;
                            this.dataGridView3.Rows.Add(First_row_dynamic_colm);
                            if (chk_expirydt.Checked)
                            {
                                if (only_expiry_datas.Contains(rowheader) == false)
                                {
                                    only_expiry_datas_row_index.Add(dataGridView3.CurrentRow.Index);
                                    only_expiry_datas_row_lotnojoin.Add(rowheader);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    row_header_lotno_all_combined = new List<string>();
                    this.dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                    this.dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                   // this.dataGridView3.Sort(this.dataGridView3.Columns[5], ListSortDirection.Ascending);
                }

                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);
                string mapped_processname = string.Empty;
                if (list_cmodel.Count > 1)
                {
                    int ck = 1;
                    foreach (var item in list_cmodel)
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = item.processName.Split(',');

                            int chk_index = 0;
                            List<Lotinfo_gridbind_common_pattern> toBeUpdated = list_cmodel.Where(c => CommonClass.Process_name.Any(d => c.processId == d.process_id)).ToList();
                            if (already_exits_row_columns.Contains(item.lotnojoin) == false)
                            {
                                foreach (var chk in split_process_name)
                                {
                                    string patern_type = item.pattern_type;
                                    //foreach (var itm in CommonClass.Process_name)
                                    //foreach (var itm in CommonClass.Process_name_gridbind_columns_shipping)
                                    foreach (var itm in CommonClass.Process_name_gridbind_columns_shipping_runtime)
                                    {
                                        string patern_type_list = itm.PaternType;
                                        string processId = itm.process_id; 
                                        Console.WriteLine("Index : " + chk_index + " pname " + chk + " mcode " + item.material_code.Split(',')[chk_index] +" lot no "+ item.lotnojoin );
                                        if (itm.ProcessNames == chk && itm.materialcode == item.material_code.Split(',')[chk_index])
                                        {
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
                                                //columun_count_v = columun_count_v + 4;
                                                columun_count_v = columun_count_v + 10;
                                            }
                                            else if (patern_type_list == "3")
                                            {
                                                columun_count_v = columun_count_v + 2;
                                            }
                                            else if (patern_type_list == "4")
                                            {
                                                columun_count_v = columun_count_v + 3;
                                            }
                                            else if (patern_type_list == "5")
                                            {
                                                //columun_count_v = columun_count_v + 8;
                                                //080623 columun_count_v = columun_count_v + 7;
                                                columun_count_v = columun_count_v + 9;
                                            }
                                        }

                                    }
                                    // List compare submited button name wise
                                    string current_procesname = item.processName.Split(',')[0];
                                    int dataGridview1_row_index = 1;
                                    //int dataGridview1_row_count = 0;
                                    dataGridView3.Refresh();
                                    string shipment_date = string.Empty;
                                    
                                    foreach (DataGridViewRow row in dataGridView3.Rows)
                                    {
                                        int row_index = row.Index;
                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();
                                            if (Compare_lotNo == item.lotnojoin)
                                            {
                                                
                                                // int index_column = list_index;
                                                //if(Compare_lotNo== "0530113-05")
                                                //{
                                                //    Console.WriteLine("Last lot no" + Compare_lotNo + " " + ck);
                                                //    if(ck==86)
                                                //    {

                                                //    }
                                                //    ck++;
                                                //}
                                                int index_column = columun_count_v;
                                                //if(!shipment_date_already_get)
                                                //{
                                                if (!string.IsNullOrEmpty(item.shipment_date))
                                                {
                                                    shipment_date = item.shipment_date.Split(',')[0];
                                                    row.Cells[0].Value = shipment_date;
                                                    dataGridView3.Rows[row_index].DefaultCellStyle.BackColor = Color.LightGray;
                                                    //dataGridView3.Rows[row_index].Cells[1].Value = false;
                                                    shipment_date_already_get = true;
                                                }
                                                else
                                                {
                                                    shipment_date = "-";
                                                    row.Cells[0].Value = shipment_date;
                                                    dataGridView3.Rows[row_index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                                    dataGridView3.Rows[row_index].Cells[1].Value = true;

                                                }
                                                //}
                                                row.Cells[2].Value = item.customer_code;
                                                row.Cells[3].Value = item.customer_name;

                                                row.Cells[4].Value = item.item_code;
                                                row.Cells[5].Value = item.item_name;


                                                row.Cells[6].Value = item.tb_qty.Split(',')[chk_index];

                                                row.Cells[7].Value = item.tb_manuf_dt.Split(',')[chk_index];
                                                // compare to current date
                                                DateTime from_dt = Convert.ToDateTime(item.tb_expairy_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                DateTime to_dt = DateTime.Now;
                                                int result = DateTime.Compare(from_dt, to_dt);
                                                if (result >= 1)
                                                {
                                                    row.Cells[8].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                }
                                                else
                                                {
                                                    row.Cells[8].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                    dataGridView3.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
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
                                                    //row.Cells[columun_count_v].Value = item.lotno;
                                                    row.Cells[columun_count_v].Value = item.lotno_p4.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }

                                                //

                                            }
                                            dataGridview1_row_index++;
                                        }
                                        //dataGridview1_row_count++;
                                    }
                                    chk_index++;
                                }
                                //2806string already_ex = item.lotnojoin.ToString();
                                //2806already_exits_row_columns.Add(already_ex);
                            }
                        }
                        list_index++;
                    }
                    //
                    //   lot_number_only_row_common("onlylotview");
                    int row_index_grid = 0;
                    foreach (DataGridViewRow row in dataGridView3.Rows)
                    {
                        if (Convert.ToString(dataGridView3.Rows[row_index_grid].Cells[0].Value) != string.Empty)
                        {
                            string shipmentdate = dataGridView3.Rows[row_index_grid].Cells[0].Value.ToString();
                            if (shipmentdate == "-")
                            {
                                // checked the row 
                                bool flag = false;

                                //foreach (DataGridViewRow row in dataGridView2.Rows)
                                //{
                                DataGridViewCheckBoxCell chk_val = (DataGridViewCheckBoxCell)row.Cells[1];
                                if (chk_val.Value == chk_val.TrueValue)
                                {
                                    chk_val.Value = chk_val.FalseValue;
                                }
                                else
                                {
                                    chk_val.Value = chk_val.TrueValue;
                                }
                                chk_val.Value = !(chk_val.Value == null ? false : (bool)chk_val.Value); //because chk.Value is initialy null
                                if (Convert.ToBoolean(chk_val.Value))
                                {
                                    //if (Convert.ToString(row.Cells[0].Value) == string.Empty || Convert.ToString(row.Cells[11].Value) == string.Empty || Convert.ToString(row.Cells[12].Value) == string.Empty)
                                    if (Convert.ToString(row.Cells[0].Value) == string.Empty)
                                    {
                                        flag = true;
                                    }
                                    if (!flag)
                                    {

                                        shippingUpdate model = new shippingUpdate();
                                        string lotnoandchild = row.HeaderCell.Value.ToString();
                                        if (already_exits_shipment_lotnochild.Contains(lotnoandchild) == false)
                                        {
                                            model.lotno_child = lotnoandchild;
                                            model.lotno = lotnoandchild.Split('-')[0];
                                            model.lotno_from = lotnoandchild.Split('-')[1]; 
                                            CommonClass.shipping_update_lotno.Add(model);
                                            already_exits_shipment_lotnochild.Add(model.lotno_child);
                                        }
                                    }
                                    else
                                    {
                                        dataGridView3.Rows[row_index_grid].Cells[1].Value = false;
                                    }
                                }
                                else if (!Convert.ToBoolean(chk_val.Value))
                                {
                                    string lotnoandchild = row.Cells[0].Value.ToString();
                                    string lotno = lotnoandchild.Split('-')[0];
                                    string lotno_from = lotnoandchild.Split('-')[1];
                                    CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotno && x.lotno_from == lotno_from);
                                    CommonClass.shipping_update_lotno.Distinct().ToList();
                                }
                                // }
                            }
                            else
                            {
                                dataGridView3.Rows[row_index_grid].Cells[1].Value = false;
                                dataGridView3.Rows[row_index_grid].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                            row_index_grid++;
                        }

                    }
                }
                else if (list_cmodel.Count == 1)
                {
                    //   lot_number_only_row_common("onlylotview");
                }

            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
            }
        }
        public void insert_lotinfo_value_assign_gridbind_shpmentdt(string ActionTypeTwo, string lotn, string lotn_frm, string lotn_to, string manf_dt_frm, string manf_dt_to, string customer_code, string item_code, string auctionrole, string sp_name, string common_cust_name, string common_item_name,string shipdt_frm,string shipdt_to,string round_lotnumber)
        {
            try
            {
                //if(chk_machine_no.Checked)
                //{
                //    string machine_no = "^" + lotn;
                //    lotn = machine_no;
                //}
                string first_digits = lotn.Substring(0, 1);
                if(first_digits =="0")
                {
                    int con = Convert.ToInt32(lotn);
                    lotn = con.ToString();
                }
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                List<Lotinfo_gridbind_common> list_lotinfo_Common = new List<Lotinfo_gridbind_common>();
                // lot information grid data's
                // p1
                string index = string.Empty;
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@shipdt_frm", "@shipdt_to", "@Actionrole", "@Actionroletwo", "@rlotno" };
                string[] obj_p1 = { ActionType_p1, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code,shipdt_frm,shipdt_to, auctionrole, ActionTypeTwo,round_lotnumber };

                //DataSet ds_pattern1 = helper.GetDatasetByCommandString(sp_name, str_p1, obj_p1);
                MySqlDataReader already_exist = helper.GetReaderByCmd(sp_name, str_p1, obj_p1);

                
                //return connection.Query<Person>("usp_myStoredProcedure", new { lastName }, commandType: CommandType.StoredProcedure).ToList();

                List<Lotinfo_gridbind_p1> m_model_p1 = LocalReportExtensions.GetList<Lotinfo_gridbind_p1>(already_exist);
                //List<Lotinfo_gridbind_common_pattern> clist_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.processName = "TERMINAL BOARD INFO";
                c_model.pattern_type = "5";
                list_cmodel.Add(c_model);
                // if (ds_pattern1.Tables[0].Rows.Count > 0)
                if (m_model_p1.Count > 0)
                {                    
                    //foreach (DataRow dr in ds_pattern1.Tables[0].Rows)
                    m_model_p1.ForEach(dr =>
                    {
                        //string lotno_split = dr["lotnojoin_p1"].ToString();
                        string lotno_split = dr.lotnojoin_p1.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        //foreach (var lot in lotnumbers)
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
                            //
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
                            //c_model.shipment_date = string.Empty;
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            list_cmodel.Add(c_model);
                        });
                    });
                }
                helper.CloseConnection();
                string ActionType_p2 = "p2view";
                string[] str_p2 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd","@shipdt_frm", "@shipdt_to", "@Actionrole", "@Actionroletwo", "@rlotno" };
                string[] obj_p2 = { ActionType_p2, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, shipdt_frm, shipdt_to, auctionrole, ActionTypeTwo, round_lotnumber };
               // DataSet ds_pattern2 = helper.GetDatasetByCommandString(sp_name, str_p2, obj_p2);
                MySqlDataReader ds_pattern2 = helper.GetReaderByCmd(sp_name, str_p2, obj_p2);
                List<Lotinfo_gridbind_p2_ship> m_model_p2 = LocalReportExtensions.GetList<Lotinfo_gridbind_p2_ship>(ds_pattern2);
                if (m_model_p2.Count > 0)
                //if (ds_pattern2.Tables[0].Rows.Count > 0)
                {
                    Lotinfo_gridbind_common model_p2 = new Lotinfo_gridbind_common();
                    //foreach (DataRow dr in ds_pattern2.Tables[0].Rows)
                    m_model_p2.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p2.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        //foreach (var lot in lotnumbers)
                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p2"].ToString();
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
                            //
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
                            //c_model.shipment_date = string.Empty;
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            list_cmodel.Add(c_model);
                        });

                    });

                }
                helper.CloseConnection();
                string ActionType_p3 = "p3view";
                string[] str_p3 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd", "@shipdt_frm", "@shipdt_to", "@Actionrole", "@Actionroletwo", "@rlotno" };
                string[] obj_p3 = { ActionType_p3, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, shipdt_frm, shipdt_to, auctionrole, ActionTypeTwo, round_lotnumber };
                //string[] obj_p3 = { ActionType_p3, textLotNoAdd.Text, cmbProcess.SelectedValue.ToString(), string.Empty };
                //DataSet ds_pattern3 = helper.GetDatasetByCommandString(sp_name, str_p3, obj_p3);
                //if (ds_pattern3.Tables[0].Rows.Count > 0)
                //{
                //    //  Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                //    Lotinfo_gridbind_common model_p3 = new Lotinfo_gridbind_common();
                //    foreach (DataRow dr in ds_pattern3.Tables[0].Rows)
                //    {
                //        string lotno_split = dr["lotnojoin_p3"].ToString();
                //        string[] lotnumbers = lotno_split.Split(',');
                //        foreach (var lot in lotnumbers)
                //        {
                //            c_model = new Lotinfo_gridbind_common_pattern();
                //            c_model.pattern_type = dr["pattern_type"].ToString();
                //           // lot no format change                        
                //            string dG1joinlotno = lot;
                //            string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                //            string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                //            int convert_lotno = Convert.ToInt32(lotno_spl);
                //            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                //            string lotno_format = convert_lotno.ToString("D7");
                //            string lotnochld_format = convert_lotnochld.ToString("D2");

                //            if (lotno_format != string.Empty)
                //            {
                //                int formate_type = Convert.ToInt32(lotno_format);
                //                lotno_format = formate_type.ToString("D7");
                //            }
                //            if (lotnochld_format != string.Empty)
                //            {
                //                int formate_type = Convert.ToInt32(lotnochld_format);
                //                lotnochld_format = formate_type.ToString("D2");
                //            }
                //            dG1joinlotno = lotno_format + "-" + lotnochld_format;
                //            c_model.lotno = lotno_format;
                //            c_model.lotnojoin = dG1joinlotno;
                //            //
                //            c_model.processId = dr["processId_p3"].ToString();
                //            c_model.processName = dr["processName_p3"].ToString();
                //            c_model.plantingdate = dr["process_date_p3"].ToString();
                //            c_model.qty = dr["quantity_p3"].ToString();
                //            c_model.tb_manuf_dt = dr["tb_manuf_dt_p3"].ToString();
                //            c_model.tb_expairy_dt = dr["tb_expairy_dt_p3"].ToString();
                //            c_model.tb_qty = dr["tb_qty_p3"].ToString();
                //            c_model.material_code = dr["materialcd"].ToString();
                //            c_model.shipment_date = dr["shipment_date"].ToString();
                //            c_model.customer_name = common_cust_name;
                //            c_model.item_name = common_item_name;
                //            c_model.customer_code = customer_code;
                //            c_model.item_code = item_code;
                //            list_cmodel.Add(c_model);
                //        }


                //    }

                //}
                //DataSet ds_pattern3 = helper.GetDatasetByCommandString(sp_name, str_p3, obj_p3);
                MySqlDataReader ds_pattern3 = helper.GetReaderByCmd(sp_name, str_p3, obj_p3);
                List<Lotinfo_gridbind_p3_ship> m_model_p3 = LocalReportExtensions.GetList<Lotinfo_gridbind_p3_ship>(ds_pattern3);
                if (m_model_p3.Count > 0)
                //if (ds_pattern3.Tables[0].Rows.Count > 0)
                {
                    //  Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                    Lotinfo_gridbind_common model_p3 = new Lotinfo_gridbind_common();
                    //foreach (DataRow dr in ds_pattern3.Tables[0].Rows)
                    m_model_p3.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p3.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        foreach (var lot in lotnumbers)
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
                            //
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
                            list_cmodel.Add(c_model);
                        }


                    });

                }
                helper.CloseConnection();
                string ActionType_p4 = "p4view";
                string[] str_p4 = { "@ActionType", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@manf_date_frm", "@manf_date_to", "@Customercd", "@proc_id", "@itmcd","@shipdt_frm", "@shipdt_to", "@Actionrole", "@Actionroletwo", "@rlotno" };
                string[] obj_p4 = { ActionType_p4, lotn, lotn_frm, lotn_to, manf_dt_frm, manf_dt_to, customer_code, string.Empty, item_code, shipdt_frm, shipdt_to, auctionrole, ActionTypeTwo,round_lotnumber};
                //string[] obj_p4 = { ActionType_p4, textLotNoAdd.Text, cmbProcess.SelectedValue.ToString(), string.Empty };
                //DataSet ds_pattern4 = helper.GetDatasetByCommandString(sp_name, str_p4, obj_p4);
                //if (ds_pattern4.Tables[0].Rows.Count > 0)
                //{
                //    Lotinfo_gridbind_common model_p4 = new Lotinfo_gridbind_common();
                //    foreach (DataRow dr in ds_pattern4.Tables[0].Rows)
                //    {
                //        string lotno_split = dr["lotnojoin_p4"].ToString();
                //        string[] lotnumbers = lotno_split.Split(',');
                //        foreach (var lot in lotnumbers)
                //        {
                //            c_model = new Lotinfo_gridbind_common_pattern();
                //            c_model.pattern_type = dr["pattern_type"].ToString();
                //            //c_model.lotno = dr["lotno"].ToString();
                //            //c_model.lotnojoin = dr["lotnojoin_p4"].ToString();
                //            // lot no format change                        
                //            string dG1joinlotno = lot;
                //            string lotno_spl = dG1joinlotno.Split('-')[0].ToString();
                //            string lotno_spl_chld = dG1joinlotno.Split('-')[1].ToString();
                //            int convert_lotno = Convert.ToInt32(lotno_spl);
                //            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                //            string lotno_format = convert_lotno.ToString("D7");
                //            string lotnochld_format = convert_lotnochld.ToString("D2");

                //            if (lotno_format != string.Empty)
                //            {
                //                int formate_type = Convert.ToInt32(lotno_format);
                //                lotno_format = formate_type.ToString("D7");
                //            }
                //            if (lotnochld_format != string.Empty)
                //            {
                //                int formate_type = Convert.ToInt32(lotnochld_format);
                //                lotnochld_format = formate_type.ToString("D2");
                //            }
                //            dG1joinlotno = lotno_format + "-" + lotnochld_format;
                //            c_model.lotno = lotno_format;
                //            c_model.lotnojoin = dG1joinlotno;
                //            //
                //            c_model.processId = dr["processId_p4"].ToString();
                //            c_model.processName = dr["processName_p4"].ToString();
                //            c_model.partno = dr["partno_p4"].ToString();
                //            c_model.qty = dr["quantity_p4"].ToString();
                //            c_model.tb_manuf_dt = dr["tb_manuf_dt_p4"].ToString();
                //            c_model.tb_expairy_dt = dr["tb_expairy_dt_p4"].ToString();
                //            c_model.tb_qty = dr["tb_qty_p4"].ToString();
                //            //060623 c_model.lotno_p4 = dr["lotno_p4_p4"].ToString();
                //            c_model.lotno_p4 = dr["lotno_p4"].ToString();
                //            c_model.material_code = dr["materialcd"].ToString();
                //            //c_model.shipment_date = string.Empty;
                //            c_model.shipment_date = dr["shipment_date"].ToString();
                //            c_model.customer_name = common_cust_name;
                //            c_model.item_name = common_item_name;
                //            c_model.customer_code = customer_code;
                //            c_model.item_code = item_code;
                //            list_cmodel.Add(c_model);
                //        }

                //    }

                //}
                MySqlDataReader ds_pattern4 = helper.GetReaderByCmd(sp_name, str_p4, obj_p4);
                List<Lotinfo_gridbind_p4_ship> m_model_p4 = LocalReportExtensions.GetList<Lotinfo_gridbind_p4_ship>(ds_pattern4);
                if (m_model_p4.Count > 0)
                {
                    Lotinfo_gridbind_common model_p4 = new Lotinfo_gridbind_common();
                    //foreach (DataRow dr in ds_pattern4.Tables[0].Rows)
                    m_model_p4.ForEach(dr =>
                    {
                        string lotno_split = dr.lotnojoin_p4.ToString();
                        string[] lotnumbers = lotno_split.Split(',');
                        //foreach (var lot in lotnumbers)
                        lotnumbers.ToList().ForEach(lot =>
                        {
                            c_model = new Lotinfo_gridbind_common_pattern();
                            c_model.pattern_type = dr.pattern_type.ToString();
                            //c_model.lotno = dr["lotno"].ToString();
                            //c_model.lotnojoin = dr["lotnojoin_p4"].ToString();
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
                            //
                            c_model.processId = dr.processId_p4.ToString();
                            c_model.processName = dr.processName_p4.ToString();
                            c_model.partno = dr.partno_p4.ToString();
                            c_model.qty = dr.quantity_p4.ToString();
                            c_model.tb_manuf_dt = dr.tb_manuf_dt_p4.ToString();
                            c_model.tb_expairy_dt = dr.tb_expairy_dt_p4.ToString();
                            c_model.tb_qty = dr.tb_qty_p4.ToString();
                            //060623 c_model.lotno_p4 = dr["lotno_p4_p4"].ToString();
                            c_model.lotno_p4 = dr.lotno_p4.ToString();
                            c_model.material_code = dr.materialcd.ToString();
                            //c_model.shipment_date = string.Empty;
                            c_model.shipment_date = dr.shipment_date.ToString();
                            c_model.customer_name = common_cust_name;
                            c_model.item_name = common_item_name;
                            c_model.customer_code = customer_code;
                            c_model.item_code = item_code;
                            list_cmodel.Add(c_model);
                        });
                    });

                }
                helper.CloseConnection();
                list_cmodel = list_cmodel.OrderBy(o => o.lotnojoin).ToList();
                // shipment date check after 2month means not show
               
                if (dataGridView3.Rows.Count >= 0)
                {
                    if (list_cmodel.Count > 1)
                    {
                        //already_exits_row_columns.AddRange(already_exits_row_header);
                        int header_lot_index = 0;
                        // Grid row header data get in list
                        foreach (var lotno in list_cmodel)
                        {
                            if (header_lot_index > 0)
                            {
                                if (!chk_expirydt.Checked)
                                {

                                    if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                    {
                                        row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        already_exits_row_header.Add(lotno.lotnojoin);
                                    }

                                }
                                else if (chk_expirydt.Checked)
                                {
                                    DateTime compare_date = DateTime.Parse(lotno.tb_expairy_dt.Split(',')[0]);

                                    int grater_than = DateTime.Compare(nowdate, compare_date);
                                    if (grater_than >= 0)
                                    {
                                        only_expiry_datas.Add(lotno.lotnojoin);
                                        //row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        //already_exits_row_header.Add(lotno.lotnojoin);
                                    }
                                   
                                    if (already_exits_row_header.Contains(lotno.lotnojoin) == false)
                                    {
                                        row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                        already_exits_row_header.Add(lotno.lotnojoin);
                                    }
                                }
                            }
                            header_lot_index++;
                        }
                        only_expiry_datas.Distinct().ToList();
                        ///9022022
                        ///grid row header bind
                        ///list_cmodel.OrderBy(o => o.lotnojoin).ToList(); 
                        /// bind fast 
                        /// 
                       
                        dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        row_header_lotno_all_combined = row_header_lotno_all_combined.OrderBy(i => i).ToList();
                        foreach (var rowheader in row_header_lotno_all_combined)
                        {
                            DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();                          
                            //// add  lot number  
                            First_row_dynamic_colm.CreateCells(this.dataGridView3);
                            First_row_dynamic_colm.HeaderCell.Value = rowheader;
                            this.dataGridView3.Rows.Add(First_row_dynamic_colm);
                            if(chk_expirydt.Checked)
                            {
                                if (only_expiry_datas.Contains(rowheader) == false)
                                {
                                    only_expiry_datas_row_index.Add(dataGridView3.CurrentRow.Index);
                                    only_expiry_datas_row_lotnojoin.Add(rowheader);
                                }
                                else
                                {

                                }
                            }
                            
                        }
                        this.dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                        this.dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                       
                    }
                    row_header_lotno_all_combined = new List<string>();

                }

                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);
                string mapped_processname = string.Empty;
                Console.WriteLine("insert  main  data bind : start time : " + DateTime.Now.ToString("HH:mm:ss"));
                if (list_cmodel.Count > 1)
                {
                    foreach (var item in list_cmodel)
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = item.processName.Split(',');

                            int chk_index = 0;
                            List<Lotinfo_gridbind_common_pattern> toBeUpdated = list_cmodel.Where(c => CommonClass.Process_name.Any(d => c.processId == d.process_id)).ToList();
                            if (already_exits_row_columns.Contains(item.lotnojoin) == false)
                            {
                                foreach (var chk in split_process_name)
                                {
                                    string patern_type = item.pattern_type;
                                    //foreach (var itm in CommonClass.Process_name)
                                    //foreach (var itm in CommonClass.Process_name_gridbind_columns_shipping)
                                    foreach (var itm in CommonClass.Process_name_gridbind_columns_shipping_runtime)
                                    {
                                        string patern_type_list = itm.PaternType;
                                        string processId = itm.process_id;
                                        //if (itm.ProcessNames == chk)
                                        if (itm.ProcessNames == chk && itm.materialcode == item.material_code.Split(',')[chk_index])
                                        {
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
                                                //columun_count_v = columun_count_v + 4;
                                                columun_count_v = columun_count_v + 10;
                                            }
                                            else if (patern_type_list == "3")
                                            {
                                                columun_count_v = columun_count_v + 2;
                                            }
                                            else if (patern_type_list == "4")
                                            {
                                                columun_count_v = columun_count_v + 3;
                                            }
                                            else if (patern_type_list == "5")
                                            {
                                                //columun_count_v = columun_count_v + 8;
                                                // 080623 columun_count_v = columun_count_v + 7;
                                                columun_count_v = columun_count_v + 9;
                                            }
                                        }

                                    }
                                    // List compare submited button name wise
                                    string current_procesname = item.processName.Split(',')[0];
                                    int dataGridview1_row_index = 1;
                                    //int dataGridview1_row_count = 0;

                                    string shipment_date = string.Empty;
                                    foreach (DataGridViewRow row in dataGridView3.Rows)
                                    {
                                        int row_index = row.Index;
                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();
                                            if (Compare_lotNo == item.lotnojoin)
                                            {
                                                // int index_column = list_index;
                                                int index_column = columun_count_v;
                                                //if(!shipment_date_already_get)
                                                //{
                                                if (!string.IsNullOrEmpty(item.shipment_date))
                                                {
                                                    shipment_date = item.shipment_date.Split(',')[0];
                                                    row.Cells[0].Value = shipment_date;
                                                    dataGridView3.Rows[row_index].DefaultCellStyle.BackColor = Color.LightGray;
                                                    //dataGridView3.Rows[row_index].Cells[1].Value = false;
                                                    shipment_date_already_get = true;
                                                }
                                                else
                                                {
                                                    shipment_date = "-";
                                                    row.Cells[0].Value = shipment_date;
                                                    dataGridView3.Rows[row_index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                                    dataGridView3.Rows[row_index].Cells[1].Value = true;

                                                }
                                                //}
                                                row.Cells[2].Value = item.customer_code;
                                                row.Cells[3].Value = item.customer_name;

                                                row.Cells[4].Value = item.item_code;
                                                row.Cells[5].Value = item.item_name;


                                                row.Cells[6].Value = item.tb_qty.Split(',')[chk_index];

                                                row.Cells[7].Value = item.tb_manuf_dt.Split(',')[chk_index];
                                                // compare to current date
                                                DateTime from_dt = Convert.ToDateTime(item.tb_expairy_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                DateTime to_dt = DateTime.Now;
                                                int result = DateTime.Compare(from_dt, to_dt);
                                                if (result >= 1)
                                                {
                                                    row.Cells[8].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                }
                                                else
                                                {
                                                    row.Cells[8].Value = item.tb_expairy_dt.Split(',')[chk_index];
                                                    dataGridView3.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
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
                                                    //row.Cells[columun_count_v].Value = item.lotno;
                                                    row.Cells[columun_count_v].Value = item.lotno_p4.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = item.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }

                                                //

                                            }
                                            dataGridview1_row_index++;
                                        }
                                        //dataGridview1_row_count++;
                                    }
                                    chk_index++;
                                }
                            }
                        }
                        list_index++;
                    }
                    //
                    //   lot_number_only_row_common("onlylotview");
                    int row_index_grid = 0;
                    foreach (DataGridViewRow row in dataGridView3.Rows)
                    {
                        if (Convert.ToString(dataGridView3.Rows[row_index_grid].Cells[0].Value) != string.Empty)
                        {
                            string shipmentdate = dataGridView3.Rows[row_index_grid].Cells[0].Value.ToString();
                            if (shipmentdate == "-")
                            {
                                // checked the row 
                                bool flag = false;

                                //foreach (DataGridViewRow row in dataGridView2.Rows)
                                //{
                                DataGridViewCheckBoxCell chk_val = (DataGridViewCheckBoxCell)row.Cells[1];
                                if (chk_val.Value == chk_val.TrueValue)
                                {
                                    chk_val.Value = chk_val.FalseValue;
                                }
                                else
                                {
                                    chk_val.Value = chk_val.TrueValue;
                                }
                                chk_val.Value = !(chk_val.Value == null ? false : (bool)chk_val.Value); //because chk.Value is initialy null
                                if (Convert.ToBoolean(chk_val.Value))
                                {
                                    //if (Convert.ToString(row.Cells[0].Value) == string.Empty || Convert.ToString(row.Cells[11].Value) == string.Empty || Convert.ToString(row.Cells[12].Value) == string.Empty)
                                    if (Convert.ToString(row.Cells[0].Value) == string.Empty)
                                    {
                                        flag = true;
                                    }
                                    if (!flag)
                                    {

                                        shippingUpdate model = new shippingUpdate();
                                        string lotnoandchild = row.HeaderCell.Value.ToString();
                                        if (already_exits_shipment_lotnochild.Contains(lotnoandchild) == false)
                                        {
                                            model.lotno_child = lotnoandchild;
                                            model.lotno = lotnoandchild.Split('-')[0];
                                            model.lotno_from = lotnoandchild.Split('-')[1];
                                            CommonClass.shipping_update_lotno.Add(model);
                                            already_exits_shipment_lotnochild.Add(model.lotno_child);
                                        }
                                    }
                                    else
                                    {
                                        dataGridView3.Rows[row_index_grid].Cells[1].Value = false;
                                    }
                                }
                                else if (!Convert.ToBoolean(chk_val.Value))
                                {
                                    string lotnoandchild = row.Cells[0].Value.ToString();
                                    string lotno = lotnoandchild.Split('-')[0];
                                    string lotno_from = lotnoandchild.Split('-')[1];
                                    CommonClass.shipping_update_lotno.RemoveAll(x => x.lotno == lotno && x.lotno_from == lotno_from);
                                    CommonClass.shipping_update_lotno.Distinct().ToList();
                                }
                                // }
                            }
                            else
                            {
                                dataGridView3.Rows[row_index_grid].Cells[1].Value = false;
                                dataGridView3.Rows[row_index_grid].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                            row_index_grid++;
                        }

                    }

                    Console.WriteLine("insert  main  data bind : end time : " + DateTime.Now.ToString("HH:mm:ss"));
                }
                else if (list_cmodel.Count == 1)
                {
                    //   lot_number_only_row_common("onlylotview");
                }

            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                Console.WriteLine("Line " + line);
            }
        }
        private void InvalidateHeader_d3()
        {
            System.Drawing.Rectangle rtHeader = this.dataGridView3.DisplayRectangle;
            rtHeader.Height = this.dataGridView3.ColumnHeadersHeight / 2;
            this.dataGridView3.Invalidate(rtHeader);
        }
        private void dataGridView3_Paint(object sender, PaintEventArgs e)
        {
            int col = 0;
            int count = 0;
            if (daysInMonths_d3 != null)
            {
                // For each month, create the display rectangle for the main title and draw it.
                foreach (int daysInMonth in daysInMonths_d3)
                {
                    System.Drawing.Rectangle r1 = this.dataGridView3.GetCellDisplayRectangle(col, -1, true);

                    // Start the rectangle from the first visible day of the month,
                    // and add the width of the column for each following day.
                    for (int day = 0; day < daysInMonth; day++)
                    {
                        System.Drawing.Rectangle r2 = this.dataGridView3.GetCellDisplayRectangle(col + day, -1, true);

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

                    using (Brush back = new SolidBrush(this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor))
                    using (Brush fore = new SolidBrush(this.dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor))
                    using (Pen p = new Pen(this.dataGridView3.GridColor))
                    using (StringFormat format = new StringFormat())
                    {                        
                        Console.WriteLine(col);
                        string month = GroupLabel_d3[count];
                        count++;

                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;

                        e.Graphics.FillRectangle(back, r1);
                        e.Graphics.DrawRectangle(p, r1);
                        e.Graphics.DrawString(month, this.dataGridView3.ColumnHeadersDefaultCellStyle.Font, fore, r1, format);
                    }

                    col += daysInMonth; // Move to the first column of the next month.
                }
            }

        }

        private void dataGridView3_Scroll(object sender, ScrollEventArgs e)
        {
            InvalidateHeader_d3();
        }

        private void dataGridView3_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            InvalidateHeader_d3();
        }

        private void dataGridView3_Resize(object sender, EventArgs e)
        {
            InvalidateHeader_d3();
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
        public static void CheckDirectory(string logFolder)
        {
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

        }

        private void chk_machine_no_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_machine_no.Checked)
            {
                if (txt_machine_no.Text == "00" || txt_machine_no.Text == string.Empty)
                {
                    chk_machine_no.Checked = false;
                    MessageBox.Show("Enter the Machine no..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chk_machine_no.Focus();
                    return;
                }
                else if (chk_lotno.Checked)
                {                    
                    MessageBox.Show("Already Lot Number Choose, uncheck..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chk_lotno.Focus();
                    return;
                }
            }
        }

        private void btn_previous_Click(object sender, EventArgs e)
        {
            try
            {
                int cPageNo = CommonClass.curentPageNo_nxtPg -1;
                int cPageSize = CommonClass.curentPageSize_nxtPg -40;
                var Get_records = CommonClass.Runtime_Store_Print_details.ToPagedList(cPageNo, PageSize);

                foreach (var get_cd in Get_records)
                {
                    terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);                  

                    insert_lotinfo_value_assign_gridbind(CommonClass.ship_tabActionType_nxtPg, get_cd.lotno, CommonClass.lotno_child_frm_nxtPg, CommonClass.lotno_child_to_nxtPg, CommonClass.manfdt_frm_nxtPg, CommonClass.manfdt_to_nxtPg, get_cd.customer_code, get_cd.item_code, CommonClass.actionTyp2_nxtPg, CommonClass.spname_nxtPg, get_cd.customer_name, get_cd.item_name);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        private void btn_nextPg_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                dataGridView3.ClearSelection();
                dataGridView3.Refresh();
                 int cPageNo = CommonClass.curentPageNo_nxtPg + 1;
                var Get_records = CommonClass.Runtime_Store_Print_details.ToPagedList(cPageNo, PageSize);
                CommonClass.curentPageNo_nxtPg = Get_records.PageNumber;
                if (Get_records.HasPreviousPage)
                {
                    btn_previous.Enabled = true;
                }
                else if (!Get_records.HasPreviousPage)
                {
                    btn_previous.Enabled = false;
                }
                if (Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = true;
                }
                else if (!Get_records.HasNextPage)
                {
                    btn_nextPg.Enabled = false;
                }
                if(shipment_gridbind_with_shpfilter_dataLoad)
                {
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);                        
                        insert_lotinfo_value_assign_gridbind_shpmentdt(CommonClass.ship_tabActionType_nxtPg, get_cd.lotno, CommonClass.lotno_child_frm_nxtPg, CommonClass.lotno_child_to_nxtPg, CommonClass.manfdt_frm_nxtPg, CommonClass.manfdt_to_nxtPg, get_cd.customer_code, get_cd.item_code, CommonClass.actionTyp2_nxtPg, CommonClass.spname_nxtPg, get_cd.customer_name, get_cd.item_name,CommonClass.ship_frmdt_nxtPg, CommonClass.ship_todt_nxtPg,CommonClass.round_lotno_nxtPg);
                    }
                }
                else if(shipment_gridbind_dataLoad)
                {
                    foreach (var get_cd in Get_records)
                    {
                        terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                        insert_lotinfo_value_assign_gridbind(CommonClass.ship_tabActionType_nxtPg, get_cd.lotno, CommonClass.lotno_child_frm_nxtPg, CommonClass.lotno_child_to_nxtPg, CommonClass.manfdt_frm_nxtPg, CommonClass.manfdt_to_nxtPg, get_cd.customer_code, get_cd.item_code, CommonClass.actionTyp2_nxtPg, CommonClass.spname_nxtPg, get_cd.customer_name, get_cd.item_name);
                    }
                }
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetDisplayedRowsCount()
        {
            int count = dataGridView3.Rows[dataGridView3.FirstDisplayedScrollingRowIndex].Height;
            count = dataGridView3.Height / count;
            return count;
        }

        private void dataGridView3_Scroll_1(object sender, ScrollEventArgs e)
        {
            try
            {
           
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void txt_machine_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txt_machine_no_Leave(object sender, EventArgs e)
        {
            if (txt_machine_no.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txt_machine_no.Text);
                txt_machine_no.Text = formate_type.ToString("D2");
            }
        }
    }
}
