using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopPartsElectronics_PS.Helper;
using YourApp.Data;
using static TopPartsElectronics_PS.Helper.GeneralModelClass;
using iTextSharp.text.html.simpleparser;
using Ubiety.Dns.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Spire.Xls;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using System.Globalization;
using AutoMapper;
using System.Diagnostics;
using System.Runtime.InteropServices;
using PagedList;

namespace TopPartsElectronics_PS
{
    public partial class FormProductionInput : Form
    {
        DataTable dt = new DataTable();
        DataTable dt_view_lotno_only = new DataTable();
        DataTable new_dt = new DataTable();  
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        int dataGridView1_grid_selectedRow = -1;
        int dgProduct_grid_selectedRow = 0;
        bool production_detail_already_add_main_list = false;
        int product_code = 1;
        string selected_dgProduct_partnumber = string.Empty;   
        int columun_count = 0;
        List<Lotinfo_gridbind_common_pattern> lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
        string Print_label_type = string.Empty;
        string Print_customer_name = string.Empty;
        string Print_Item_code = string.Empty;
        string Print_Item_name = string.Empty;
        string Print_Qty = string.Empty;
        string Print_lotno = string.Empty;
        string Print_date_expiry = string.Empty;
        string Print_date_manfdt = string.Empty;
        string Print_material_code = string.Empty;      
        string Print_M1 = string.Empty;
        string Print_M2 = string.Empty;
        string Print_M3 = string.Empty;
        string Print_M4 = string.Empty;
        string Printed_date_join = string.Empty;
        string Print_person_name_join = string.Empty;
        string Print_date_old_colm = string.Empty;
        string Print_copy_join = string.Empty;
        /// Pass the value pattern popup      
        /// pattern 1
        string pass_pattern1_LotNo = "0000000";
        string pass_pattern1_PartNo = "00000";
        string pass_pattern1_PlantingDate = string.Empty;
        string pass_pattern1_Qty = "0000";
        string pass_pattern1_PbDate = "00";
        /// pattern 2      
        string pass_pattern2_ProcessDate = string.Empty;
        string pass_pattern2_Controlno = "000";
        string pass_pattern2_Sheetlotno = "00000";
        string pass_pattern2_Qty = "0000";
        /// pattern 3     
        string pass_pattern3_ProcessDate = string.Empty;
        string pass_pattern3_Qty = "0000";
        /// pattern 4      
        string pass_pattern4_PartNo = "00000";
        string pass_pattern4_Lotno = "00000";
        string pass_pattern4_Qty = "0000";
        ///
        /// <Check lot no and lotno child and item code same or not >
        string mapped_itemcode = string.Empty;
        bool refresh_btn_click = false;
        bool mapped_itemcode_exist = false;
        bool one_time_assign_dgProduct_header = false;
        bool lot_information_changed_without_grid = false;
        bool Bproduct_changed = false;
        bool lotnumber_changed_add_pi_tbl = false;
        bool lotnumber_only_changed_add_pi_tbl = false;    
        bool product_addBtn_click = false;
        int PageNumber = 1;
        int PageSize = 8;
        /// </summary>
        public FormProductionInput()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private int[] daysInMonths;
        private string[] GroupLabel;
        private string[,] LabelString;
        private int[,] LabelSize;
        private void FormProductionInput_Load(object sender, EventArgs e)
        {
            try
            {                
                txt_reason_hs.Text = "Remarks";
                txt_reason_hs.ForeColor = Color.LightGray;
                groupBox5.AutoSize = true;
                groupBox5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                CommonClass.Process_name_gridbind = new List<PI_Process>();
                dataGridView2.AutoGenerateColumns = false;
                 // dropdown 
                 one_time_assign_dgProduct_header = true;
                if (!refresh_btn_click)
                {
                    DataTable drp = helper.ProcessList();
                    cmbProcess.Items.Clear();
                    cmbProcess.DisplayMember = "fullname";
                    cmbProcess.ValueMember = "processcode";
                    cmbProcess.DataSource = drp;
                }
                cmbProcess.Enabled = false;
                this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 2;
                // lot information
                DateTime current_time = DateTime.Now;
                txt_manf_time.Text = current_time.ToString("HH:mm:ss");
                // temp table truncate
                CommonClass.p1 = false;
                CommonClass.p2 = false;
                CommonClass.p3 = false;
                CommonClass.p4 = false;
                CommonClass.up_p1 = false;
                CommonClass.up_p2 = false;
                CommonClass.up_p3 = false;
                CommonClass.up_p4 = false;
                // max id get for production input
                max_productinput_id();
                //Get all available printers and add them to the combo box  
                foreach (String printer in PrinterSettings.InstalledPrinters)
                {
                    comboBox_printernames.Items.Add(printer.ToString());
                }
                dateTimePicker_Manf.CustomFormat = " ";
                this.dateTimePicker_Manf.CustomFormat = "dd-MM-yyyy";//or "MM/dd/yyyy"
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("View_time_update", ex);
            }
        }
        public static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        public void datatable_create()
        {
            dt.Columns.Add("No");
            dt.Columns.Add("Customer Code");
            dt.Columns.Add("Prodcut Code");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Currency");
            dt.Columns.Add("Price");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Additional Code");
            dt.Columns.Add("Label Type");
            dt.Columns.Add("M1");
            dt.Columns.Add("M2");
            dt.Columns.Add("M3");
            dt.Columns.Add("M4");
            dGProduct.DataSource = dt;
            dGProduct.Columns[0].Width = 50;
            dGProduct.Columns[1].Width = 100;
            dGProduct.Columns[2].Visible = false;
            dGProduct.Columns[3].Width = 150;
            dGProduct.Columns[4].Visible = false;
            dGProduct.Columns[5].Visible = false;
            dGProduct.Columns[6].Visible = false;
            dGProduct.Columns[7].Visible = false;
            dGProduct.Columns[8].Visible = false;
            dGProduct.Columns[9].Visible = false;
            dGProduct.Columns[10].Visible = false;
            dGProduct.Columns[11].Visible = false;
            dGProduct.Columns[12].Visible = false;
            dGProduct.Columns[13].Visible = false;
        }
        public void datatable_create_new()
        {
            dGProduct.Columns.Add("sno", "Sno");
            dGProduct.Columns.Add("idpi_product_information", "idpi_product_information");
            dGProduct.Columns["idpi_product_information"].Visible = false;
            dGProduct.Columns.Add("lotno", "lotno");
            dGProduct.Columns["lotno"].Visible = false;
            dGProduct.Columns.Add("customercode", "Customer code");
            dGProduct.Columns.Add("customershort_name", "customershort_name");
            dGProduct.Columns["customershort_name"].Visible = false;
            dGProduct.Columns.Add("customerfull_name", "customerfull_name");
            dGProduct.Columns["customerfull_name"].Visible = false;
            dGProduct.Columns.Add("item_code", "Item code");
            dGProduct.Columns.Add("item_name", "item_name");
            dGProduct.Columns["item_name"].Visible = false;
            dGProduct.Columns.Add("unit_price_country_shortcd", "unit_price_country_shortcd");
            dGProduct.Columns["unit_price_country_shortcd"].Visible = false;
            dGProduct.Columns.Add("unit_price", "unit_price");
            dGProduct.Columns["unit_price"].Visible = false;
            dGProduct.Columns.Add("box_qty", "box_qty");
            dGProduct.Columns["box_qty"].Visible = false;
            dGProduct.Columns.Add("additional_code", "additional_code");
            dGProduct.Columns["additional_code"].Visible = false;
            dGProduct.Columns.Add("lable_typ", "lable_typ");
            dGProduct.Columns["lable_typ"].Visible = false;
            dGProduct.Columns.Add("m1", "m1");
            dGProduct.Columns["m1"].Visible = false;
            dGProduct.Columns.Add("m2", "m2");
            dGProduct.Columns["m2"].Visible = false;
            dGProduct.Columns.Add("m3", "m3");
            dGProduct.Columns["m3"].Visible = false;
            dGProduct.Columns.Add("m4", "m4");
            dGProduct.Columns["m4"].Visible = false;
        }
        public void max_productinput_id()
        {
            string ActionRole = string.Empty;
            if (CommonClass.view_enable)
            {
                ActionRole = "get_existid";
            }
            else
            {
                ActionRole = "get_maxid";
            }
            string ActionType = "productinput";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType, ActionRole, textSearchLotNo.Text, string.Empty };

            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dts = ds.Tables[0];
                string p_code = dts.Rows[0]["pi_id"].ToString();

                if (p_code == "")
                {
                    product_code = 1;
                }
                else
                {
                    product_code = Convert.ToInt32(dts.Rows[0]["pi_id"]);
                }
            }
            helper.CloseConnection();
        }      
        public void max_lotno_id()
        {      
            string ActionType = "lotinfo_cust_sno";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType, string.Empty, txtCustomerCode.Text, textItemCode.Text };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                string LotNoAdd = dtbl.Rows[0]["lot_no"].ToString();
                if (LotNoAdd != string.Empty)
                {
                    int formate_type = Convert.ToInt32(LotNoAdd);
                    textLotNoAdd.Text = formate_type.ToString("D7");
                    //Child
                    string Lotnochild_formate_change = dtbl.Rows[0]["lotno_child"].ToString();
                    string Lotno_create_at = dtbl.Rows[0]["created_at"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type_child = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild.Text = formate_type_child.ToString("D2");
                    }
                    helper.CloseConnection();
                    max_id_only_lotno(textLotNoAdd.Text, textLotNoChild.Text, Lotno_create_at);
                }
                else
                {
                    helper.CloseConnection();
                    max_id_only_lotno(textLotNoAdd.Text, textLotNoChild.Text,"");                  
                }
            }
        }
        public string max_lotno_with_lot_maintbl(string customercd, string lotnumber)
        {
            string lot_number_child_maintbl = "0";                   
            string ActionType = "lot_cust_lotno_sno";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" }; 
            string[] obj = { ActionType, textItemCode.Text, lotnumber, customercd };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                lot_number_child_maintbl = dtbl.Rows[0]["lotno_child"].ToString();          
                if (string.IsNullOrEmpty(lot_number_child_maintbl))
                {
                    lot_number_child_maintbl = "0";
                }
            }
            return lot_number_child_maintbl;
        }
        public string max_lotno_with_lot_maintbl_leave(string customercd, string lotnumber)
        {
            string lot_number_child_maintbl = "0";
            string createat = "0";
            string ActionType = "lot_cust_lotno_sno";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType, textItemCode.Text, lotnumber, customercd };

            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                lot_number_child_maintbl = dtbl.Rows[0]["lotno_child"].ToString();
                createat = dtbl.Rows[0]["created_at"].ToString();
                if (string.IsNullOrEmpty(lot_number_child_maintbl))
                {
                    lot_number_child_maintbl = "0";
                }
                if (string.IsNullOrEmpty(createat))
                {
                    createat = "0";
                }
            }
            return lot_number_child_maintbl + "," + createat;
        }
        public void common_max_lotnoid()
        {
            string ActionType = "lotinfo_cust_common";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };           
            string[] obj = { ActionType, string.Empty, txtCustomerCode.Text, textItemCode.Text };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                string LotNoAdd = dtbl.Rows[0]["lot_no"].ToString();
                if (!string.IsNullOrEmpty(LotNoAdd) && LotNoAdd != "")
                {
                    int formate_type = Convert.ToInt32(LotNoAdd);
                    textLotNoAdd.Text = formate_type.ToString("D7");
                    //Child
                    string Lotnochild_formate_change = dt.Rows[0]["lotno_child"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type_child = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild.Text = formate_type_child.ToString("D2");
                    }
                    helper.CloseConnection();
                }
                else
                {
                    helper.CloseConnection();
                    int formate_type = Convert.ToInt32("0");
                    textLotNoAdd.Text = formate_type.ToString("D7");
                    //Child
                    int formate_type_child = Convert.ToInt32("0");
                    textLotNoChild.Text = formate_type_child.ToString("D2");
                }
            }
        }
        public void max_id_only_lotno(string lotno, string lotnochild,string create_at)
        {            
            string ActionType = "lotinfo_only_tbl_sno";
            string common_lotno = string.Empty;
            string common_lotno_child = string.Empty;
            string[] str = { "@ActionType", "@custcd","@itmcd", "@lotnumber", "@lotnumchild" };
            string[] obj = { ActionType, txtCustomerCode.Text,textItemCode.Text, lotno, lotnochild };
            DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                common_lotno = dtbl.Rows[0]["lot_no"].ToString();
                string lot_only_tbl_create_at = dtbl.Rows[0]["created_at"].ToString();
                if (!string.IsNullOrEmpty(common_lotno) && common_lotno != "")
                {
                    int formate_type = Convert.ToInt32(common_lotno);
                    common_lotno = formate_type.ToString("D7");
                    common_lotno_child = dtbl.Rows[0]["lotno_child"].ToString();
                    
                    helper.CloseConnection();
                    if (common_lotno_child != string.Empty)
                    {
                        int formate_type_child_equal = Convert.ToInt32(common_lotno_child);
                        common_lotno_child = formate_type_child_equal.ToString("D2");
                        // Main table lot number not equal to zero 
                        int conv_lotno = Convert.ToInt32(lotno);
                        if(conv_lotno >0 && create_at !=string.Empty)
                        {
                            if (lotno == common_lotno && lotnochild == common_lotno_child)
                            {
                                // child                                
                                textLotNoChild.Text = formate_type_child_equal.ToString("D2");
                                // mani tbl
                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                if (result)
                                {
                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text,lotno, "lotno_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                }
                                // only lot tbl
                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                if (result_only_tbl)
                                {
                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                }
                            }
                            else if (lotno == common_lotno)
                            {
                                string get_lotnochild = max_id_with_lotnumber_lotonlytbl(txtCustomerCode.Text, lotno);
                                string get_lotnochild_maintbl = max_lotno_with_lot_maintbl(txtCustomerCode.Text, lotno);
                                if (string.IsNullOrEmpty(get_lotnochild))
                                {
                                    get_lotnochild_maintbl = "0";
                                }
                                if (string.IsNullOrEmpty(get_lotnochild_maintbl))
                                {
                                    get_lotnochild_maintbl = "0";
                                }
                                // convert integer 
                                int chk_lotchild = Convert.ToInt32(get_lotnochild);
                                int chk_comlotchild = Convert.ToInt32(get_lotnochild_maintbl);
                                DateTime lot_main_tbl = DateTime.Parse(create_at);
                                DateTime lot_only_tbl = DateTime.Parse(lot_only_tbl_create_at);
                                // Date compare 
                                bool date_equal = DateTime.Equals(lot_only_tbl.Date, lot_main_tbl.Date);
                                if(date_equal)
                                {                                    
                                    // Time compare 
                                    int grater_than = TimeSpan.Compare(lot_only_tbl.TimeOfDay, lot_main_tbl.TimeOfDay);
                                    if (grater_than > 0)
                                    {
                                        // 200323
                                        textLotNoChild.Text = chk_lotchild.ToString("D2");      
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {     
                                            string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }

                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lotchild > chk_comlotchild)
                                        {
                                            textLotNoChild.Text = chk_lotchild.ToString("D2");
         
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");                                                
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            textLotNoChild.Text = chk_comlotchild.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        textLotNoChild.Text = chk_comlotchild.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                           string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                }
                                else if(!date_equal)
                                {
                                    int grater_than = DateTime.Compare(lot_only_tbl.Date, lot_main_tbl.Date);
                                    if (grater_than > 0)
                                    {
                                        textLotNoChild.Text = chk_lotchild.ToString("D2");
                                        
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lotchild > chk_comlotchild)
                                        {
                                            textLotNoChild.Text = chk_lotchild.ToString("D2");
                                            
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            textLotNoChild.Text = chk_comlotchild.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {                                      
                                        textLotNoChild.Text = chk_comlotchild.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                }                                
                            }
                            else if (lotno != common_lotno)
                            {
                                // Pass lot number main table for both  ( lotno ) 
                                string get_lotnochild_maintbl_lotno_ps = max_lotno_with_lot_maintbl(txtCustomerCode.Text, lotno);
                                string get_lotnochild_maintbl_lotno_ps_lotonly_tbl = max_id_with_lotnumber_lotonlytbl(txtCustomerCode.Text, common_lotno);

                                if (string.IsNullOrEmpty(get_lotnochild_maintbl_lotno_ps))
                                {
                                    get_lotnochild_maintbl_lotno_ps_lotonly_tbl = "0";
                                }
                                if (string.IsNullOrEmpty(get_lotnochild_maintbl_lotno_ps_lotonly_tbl))
                                {
                                    get_lotnochild_maintbl_lotno_ps_lotonly_tbl = "0";
                                }
                                int chk_lot_main_tbl = Convert.ToInt32(get_lotnochild_maintbl_lotno_ps);
                                int chk_lot_only_tbl = Convert.ToInt32(get_lotnochild_maintbl_lotno_ps_lotonly_tbl);
                                
                                // end 
                                // Date convert 
                                DateTime lot_main_tbl = DateTime.Parse(create_at);
                                DateTime lot_only_tbl = DateTime.Parse(lot_only_tbl_create_at);
                                // Date compare 
                                bool date_equal = DateTime.Equals(lot_only_tbl.Date, lot_main_tbl.Date);
                                if (date_equal)
                                {
                                    // Time compare 
                                    int grater_than = TimeSpan.Compare(lot_only_tbl.TimeOfDay, lot_main_tbl.TimeOfDay);
                                    if (grater_than > 0)
                                    {
                                        textLotNoAdd.Text = common_lotno;
                                        textLotNoChild.Text = chk_lot_only_tbl.ToString("D2");                                       
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lot_main_tbl > chk_lot_only_tbl)
                                        {
                                            textLotNoAdd.Text = common_lotno;
                                            textLotNoChild.Text = chk_lot_only_tbl.ToString("D2");
                                            
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            textLotNoAdd.Text = lotno;
                                            textLotNoChild.Text = chk_lot_main_tbl.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        textLotNoAdd.Text = lotno;
                                        textLotNoChild.Text = chk_lot_main_tbl.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                }
                                else if (!date_equal)
                                {
                                    int grater_than = DateTime.Compare(lot_only_tbl.Date, lot_main_tbl.Date);
                                    if (grater_than > 0)
                                    {                               
                                        // 200323
                                        textLotNoAdd.Text = common_lotno;
                                        textLotNoChild.Text = chk_lot_only_tbl.ToString("D2");
                                        
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lot_main_tbl > chk_lot_only_tbl)
                                        {                                           
                                            textLotNoAdd.Text = common_lotno;
                                            textLotNoChild.Text = chk_lot_only_tbl.ToString("D2");                                            
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            textLotNoAdd.Text = lotno;
                                            textLotNoChild.Text = chk_lot_main_tbl.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        textLotNoAdd.Text = lotno;
                                        textLotNoChild.Text = chk_lot_main_tbl.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                }  
                            }
                        }
                        else
                        {
                            textLotNoAdd.Text = common_lotno;
                            textLotNoChild.Text = common_lotno_child;
                        }                     
                    }
                    helper.CloseConnection();
                }
                else
                {
                    helper.CloseConnection();
                    common_max_lotnoid();
                }
            }
            helper.CloseConnection();
        }
        public string max_id_with_lotnumber_lotonlytbl(string customercd, string lotnumber)
        {
            try
            {
                string lotno_child = "0";
                string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
                string[] obj = { "lotinfo_only_tbl_wt_lotnumber_sno", customercd,textItemCode.Text, lotnumber, string.Empty };
                DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];
                    lotno_child = dtbl.Rows[0]["lotno_child"].ToString();
                    if (string.IsNullOrEmpty(lotno_child))
                    {
                        lotno_child = "0";
                    }
                }
                return lotno_child;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("process_id_exist_check", ex);
            }
        }
        public string max_id_with_lotnumber_lotonlytbl_leave(string customercd, string lotnumber)
        {
            try
            {
                string lotno_child = "0";
                string createat = "0";
                string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
                string[] obj = { "lotinfo_only_tbl_wt_lotnumber_sno", customercd, textItemCode.Text, lotnumber, string.Empty };
                DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];
                    lotno_child = dtbl.Rows[0]["lotno_child"].ToString();
                    createat = dtbl.Rows[0]["created_at"].ToString();
                    if (string.IsNullOrEmpty(lotno_child))
                    {
                        lotno_child = "0";
                    }
                    if (string.IsNullOrEmpty(createat))
                    {
                        createat = "0";
                    }
                }
                return lotno_child + "," + createat;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("max_id_with_lotnumber_lotonlytbl_leave", ex);
            }
        }
        public string lotonlytbl_already_exist(string customercd,string itemcd, string lotnumber,string lotnumber_child,string auctionType)
        {
            try
            {
                int lotno_child = 0;
                string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
                string[] obj = { auctionType, customercd, itemcd, lotnumber, lotnumber_child };
                DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];
                    string getlotchild = string.Empty;
                    getlotchild = dtbl.Rows[0]["lotno_child"].ToString();
                    int increment_lotno_child = Convert.ToInt32(getlotchild);
                    lotno_child = increment_lotno_child + 1;
                }
                return lotno_child.ToString();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("lotonlytbl_already_exist", ex);
            }
        }
        public string max_lotno_manitbl(string custcd,string itmcd,string lotnum,string ActionType)
        {
            string lot_number_child_maintbl = "0";         
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };         
            string[] obj = { ActionType, itmcd, lotnum, custcd };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                lot_number_child_maintbl = dtbl.Rows[0]["lotno_child"].ToString();
            
                if (string.IsNullOrEmpty(lot_number_child_maintbl))
                {
                    lot_number_child_maintbl = "0";
                }              
            }
            return lot_number_child_maintbl;
        }
        public string max_lotno_onlytbl(string custcd,string itmcd,string lotnum,string ActionType)
        {
            string lotno_child = "0";          
            string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
            string[] obj = { ActionType, custcd, itmcd,lotnum, string.Empty };
            DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtbl = ds.Tables[0];
                lotno_child = dtbl.Rows[0]["lotno_child"].ToString();               
                if (string.IsNullOrEmpty(lotno_child))
                {
                    lotno_child = "0";
                }               
            }
            return lotno_child;
        }
        public class Data
        {
            public string Key { get; set; }
            public int Value { get; set; }
        }
        private void LoadDataGrid()
        {
            try
            {
                if(!product_addBtn_click)
                {
                    dataGridView1.DataSource = null;
                    int total_process = CommonClass.Process_name_gridbind.Count;
                    // grid bind start
                    int totalgroup = total_process;
                    daysInMonths = new int[totalgroup]; // check line 129
                    GroupLabel = new string[totalgroup];
                    LabelString = new string[totalgroup, 10];
                    LabelSize = new int[totalgroup, 10];
                    List<KeyValuePair<int, string>> kvpList = new List<KeyValuePair<int, string>>();          
                    int i = 0;
                    this.dataGridView1.Columns.Clear();
                    foreach (var itm in CommonClass.Process_name_gridbind)
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
                                LabelString[i, 2] = "Plating Date";
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
                            LabelString[0, 0] = "B";
                            LabelString[0, 1] = "H";
                            LabelString[0, 2] = "S";
                            LabelString[0, 3] = "Remarks";
                            LabelString[0, 4] = "Quantity";
                            LabelString[0, 5] = "Manufacturing Date";
                            LabelString[0, 6] = "Expiry Date";
                            LabelString[0, 7] = "Lotno";
                            LabelString[0, 8] = "LotnoChild";
                            LabelSize[0, 0] = 40;
                            LabelSize[0, 1] = 40;
                            LabelSize[0, 2] = 40;
                            LabelSize[0, 3] = 80;
                            LabelSize[0, 4] = 80;
                            LabelSize[0, 5] = 150;
                            LabelSize[0, 6] = 150;
                            LabelSize[0, 7] = 40;
                            LabelSize[0, 8] = 40;
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
                            daysInMonths[month - 1] = 9;
                        }
                        for (int day = 1; day <= daysInMonths[month - 1]; day++)
                        {
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
                                colname = string.Empty;
                                colheadname = string.Empty;
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
                
                    dataGridView1.Columns[5].ValueType = typeof(DateTime);
                    
                    dataGridView1.Columns[5].DefaultCellStyle.Format = "dd-MM-yyyy";               

                    this.dataGridView1.AllowUserToAddRows = false;                  
                    this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    this.dataGridView1.Paint += DataGridView1_Paint;
                    this.dataGridView1.Scroll += DataGridView1_Scroll;
                    this.dataGridView1.ColumnWidthChanged += DataGridView1_ColumnWidthChanged;
                    this.dataGridView1.Resize += DataGridView1_Resize;
                    product_addBtn_click = false;
                }
                else
                {
                    dataGridView1.Refresh();
                }      
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("LoadDataGrid", ex);
            }
        }       
        
        private void CreateDataGridHeader()
        {
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = textLotNoAdd.Text + "$-" + textLotNoChild.Text;
            dataGridView1.Rows[n].Cells[1].Value = "10";
            dataGridView1.Rows[n].Cells[2].Value = "25-05-2022";
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
        public void ChangeProcessColor(string processName, string btnId)
        {
            DataTable drp = helper.ProcessList();
            for (int i = 0; i < drp.Rows.Count; i++)
            {             
                string values = drp.Rows[i]["fullname"].ToString();
                if (processName == values)
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = " Quantity = '" + btnId + "'";
                    dataGridView1.DataSource = dv;
                }              
            }

        }

        public void dynamic_data_add_gridview(string btnName, string partno, string lotno, string plantingdate, string tb_qty, string qty, string pb_date, string btnId, string new_pb_date, string p1_lotno_spl, string p2_lotno_spl, string material_cd,string bproduct,string onhold, string scrap, string reason_hs)
        {            
            bool lotno_already_exist_gridview = false;
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (row.HeaderCell.Value.Equals(lotno))
                    {
                        // row exists
                        lotno_already_exist_gridview = true;
                        break;
                    }
                }
                // Lot number not exist means go to if statement
                if (lotno_already_exist_gridview)
                {
                    girdrow_add(btnName, partno, lotno, plantingdate, tb_qty, qty, pb_date, new_pb_date, p1_lotno_spl, p2_lotno_spl, material_cd,bproduct,onhold,scrap,reason_hs);
                }
                else if (!lotno_already_exist_gridview)
                {
                    //// add new lot number ( Header cell )
                    DataGridViewRow row_dynamic_colm = new DataGridViewRow();
                    row_dynamic_colm.CreateCells(this.dataGridView1);
                    row_dynamic_colm.HeaderCell.Value = lotno;
                    row_dynamic_colm.HeaderCell.Style.ForeColor = Color.MediumBlue;
                    this.dataGridView1.Rows.Insert(0, row_dynamic_colm);               
                    girdrow_add(btnName, partno, lotno, plantingdate, tb_qty, qty, pb_date, new_pb_date, p1_lotno_spl, p2_lotno_spl, material_cd,bproduct, onhold, scrap, reason_hs);
                }
            }
            else if (dataGridView1.Rows.Count == 0)
            {
                DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                //// add  lot number  
                First_row_dynamic_colm.CreateCells(this.dataGridView1);
                First_row_dynamic_colm.HeaderCell.Value = lotno;
                First_row_dynamic_colm.HeaderCell.Style.ForeColor = Color.MediumBlue;
                this.dataGridView1.Rows.Insert(0,First_row_dynamic_colm);
                girdrow_add(btnName, partno, lotno, plantingdate, tb_qty, qty, pb_date, new_pb_date, p1_lotno_spl, p2_lotno_spl, material_cd,bproduct,onhold,scrap,reason_hs);
            }
            change_color_button(btnId);
            dataGridView1.Sort(dataGridView1.Columns[7], ListSortDirection.Descending); 
        }
        public void girdrow_add(string btnName, string partno, string lotno, string plantingdate, string tb_qty, string qty, string pb_date, string new_pb_date, string p1_lotno_spl, string p2_lotno_spl, string material_cd,string bproduct,string onhold,string scrap, string reason_hs)
        {
            string LotNo = textLotNoAdd.Text + "-" + textLotNoChild.Text;   
            string Compare_lotNo = "";
            int list_index = 0;      
            columun_count = 0;
            Color new_row_color = Color.MediumBlue;            
            foreach (var item in CommonClass.Process_name_gridbind)
            {
                string patern_type = item.PaternType;
                // List compare submited button name wise
                if (item.ProcessNames == btnName && item.materialcode == material_cd)
                {
                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            Compare_lotNo = row.HeaderCell.Value.ToString();
                            if (Compare_lotNo == LotNo)
                            {                                                             
                                if (bproduct =="B")
                                {
                                    row.Cells[0].Value = "B";
                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                }
                                else if (string.IsNullOrEmpty(bproduct))
                                {
                                    row.Cells[0].Value = string.Empty;
                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = DefaultBackColor;
                                }
                                if (onhold=="H")
                                {
                                    row.Cells[1].Value = "H";
                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                }
                                else if (string.IsNullOrEmpty(onhold))
                                {
                                    row.Cells[1].Value = string.Empty;
                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = DefaultBackColor;
                                }
                                if (scrap =="S")
                                {
                                    row.Cells[2].Value = "S";
                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                }
                                else if (string.IsNullOrEmpty(scrap))
                                {
                                    row.Cells[2].Value = string.Empty;
                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = DefaultBackColor;
                                }
                                if(reason_hs!="Remarks")
                                {
                                    row.Cells[3].Value = reason_hs;
                                }
                                else
                                {
                                    row.Cells[3].Value = null;
                                }
                                row.Cells[0].Style.ForeColor = new_row_color;
                                row.Cells[1].Style.ForeColor = new_row_color;
                                row.Cells[2].Style.ForeColor = new_row_color;
                                row.Cells[3].Style.ForeColor = new_row_color;
                                row.Cells[4].Value = tb_qty;
                                row.Cells[4].Style.ForeColor = new_row_color;
                                if (pb_date != "")
                                {
                                    DateTime oDate_insert = Convert.ToDateTime(pb_date);
                                    row.Cells[5].Value = oDate_insert;
                                    row.Cells[5].Style.ForeColor = new_row_color;
                                    DateTime oDate = Convert.ToDateTime(pb_date);
                                    DateTime nextYear = oDate.AddYears(+1);
                                    row.Cells[6].Value = nextYear.ToString("dd-MM-yyyy");
                                    row.Cells[6].Style.ForeColor = new_row_color;
                                    string manf_dte = oDate_insert.ToString("yyyyMMdd");
                                    row.Cells[7].Value = manf_dte + Compare_lotNo.Split('-')[0] + Compare_lotNo.Split('-')[1];

                                }
                                row.Cells[8].Value = Compare_lotNo.Split('-')[1];
                                if (patern_type == "1")
                                {
                                    row.Cells[columun_count].Value = partno;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = p1_lotno_spl;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = plantingdate;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = qty;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = new_pb_date;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    if (CommonClass.view_enable)
                                    {
                                        view_time_update(lotno);
                                    }
                                }
                                else if (patern_type == "2")
                                {
                                    row.Cells[columun_count].Value = plantingdate;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = partno;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = p2_lotno_spl;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = qty;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    if (CommonClass.view_enable)
                                    {
                                        view_time_update(lotno);
                                    }
                                }
                                else if (patern_type == "3")
                                {
                                    row.Cells[columun_count].Value = plantingdate;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = qty;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    if (CommonClass.view_enable)
                                    {
                                        view_time_update(lotno);
                                    }
                                }
                                else if (patern_type == "4")
                                {
                                    row.Cells[columun_count].Value = partno;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = p1_lotno_spl;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    columun_count = columun_count + 1;
                                    row.Cells[columun_count].Value = qty;
                                    row.Cells[columun_count].Style.ForeColor = new_row_color;
                                    if (CommonClass.view_enable)
                                    {
                                        view_time_update(lotno);
                                    }
                                }
                                dataGridView1_grid_selectedRow = row.Index;
                                return;
                            }
                        }

                    }
                }
                else
                {
                    if (patern_type == "1")
                    {
                        columun_count = columun_count + 5;
                    }
                    else if (patern_type == "2")
                    {
                        columun_count = columun_count + 4;
                    }
                    else if (patern_type == "3")
                    {
                        columun_count = columun_count + 2;
                    }
                    else if (patern_type == "4")
                    {
                        columun_count = columun_count + 3;
                    }
                    else if (patern_type == "5")
                    {
                        columun_count = columun_count + 9;
                    }
                }
                list_index++;

            }
        }
        public void view_time_update(string lotno)
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.lotnojoin = lotno;
                list_cmodel.Add(c_model);
                lotview_list_cmodel_grid.AddRange(list_cmodel);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("View_time_update", ex);
            }
        }
           
        public void dGProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            // 300523 delete pattern temp table data delete use ( cmid )
            truncate_pattern_temp();
            // 300523 end
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            dgProduct_grid_selectedRow = rowIndex;
            DataGridViewRow row = dGProduct.Rows[rowIndex];
            lotnumber_only_changed_add_pi_tbl = false;
            selected_dgProduct_partnumber = row.Cells[1].Value.ToString();
            txtCustomerCode.Text = row.Cells[3].Value.ToString();
            textItemCode.Text = row.Cells[6].Value.ToString();
            textItemName.Text = row.Cells[7].Value.ToString();
            textCurrency.Text = row.Cells[8].Value.ToString();
            textPrice.Text = row.Cells[9].Value.ToString();
            textQuantity.Text = row.Cells[10].Value.ToString();
            textAdditionalCode.Text = row.Cells[11].Value.ToString();
            textLabelType.Text = row.Cells[12].Value.ToString();
            textMark1.Text = row.Cells[13].Value.ToString();
            textMark2.Text = row.Cells[14].Value.ToString();
            textMark3.Text = row.Cells[15].Value.ToString();
            textMark4.Text = row.Cells[16].Value.ToString();           
            FetchBOMDetails(txtCustomerCode.Text, textItemCode.Text);  
            if (CommonClass.view_enable)
            {
                CommonClass.Process_name = new List<PI_Process>();
                CommonClass.Process_name_gridbind = new List<PI_Process>();
                CommonClass.Process_name_gridbind_columns = new List<PI_Process>();
                PI_Process models = new PI_Process();
                models.id = "XXX";
                models.ProcessNames = "TERMINAL BOARD INFO";
                models.PaternType = "5";                
                models.process_id = "0";
                CommonClass.Process_name.Add(models);
                int i = 1;
                foreach (DataGridViewRow val in dGProcess.Rows)
                {
                    PI_Process model = new PI_Process();
                    model.id = i.ToString();
                    model.ProcessNames = val.Cells[5].Value.ToString();
                    model.PaternType = val.Cells[14].Value.ToString();
                    model.process_id = val.Cells[15].Value.ToString();
                    model.itemcode = val.Cells[2].Value.ToString();
                    model.materialcode = val.Cells[6].Value.ToString();
                    CommonClass.Process_name.Add(model);
                    i++;
                }
                CommonClass.Process_name_gridbind.AddRange(CommonClass.Process_name);
                CommonClass.Process_name_gridbind_columns.AddRange(CommonClass.Process_name);                        
                if (!production_detail_already_add_main_list)
                {                    
                    CommonClass.PI_insert_data.AddRange(CommonClass.PI_insert_data_temp);
                    production_detail_already_add_main_list = true;                   
                }
            }
            else if (!CommonClass.view_enable)
            {
                CommonClass.Process_name_gridbind = new List<PI_Process>();
                CommonClass.Process_name_gridbind_columns = new List<PI_Process>();
                CommonClass.Process_name_gridbind.AddRange(CommonClass.Process_name);
                CommonClass.Process_name_gridbind.RemoveAll(x => x.id != selected_dgProduct_partnumber && x.id != "XXX");
                CommonClass.Process_name_gridbind_columns.AddRange(CommonClass.Process_name);
                CommonClass.Process_name_gridbind_columns.RemoveAll(x => x.id != selected_dgProduct_partnumber && x.id != "XXX");
                if (!production_detail_already_add_main_list)
                {    
                    // Production information insert details 
                    CommonClass.PI_insert_data.AddRange(CommonClass.PI_insert_data_temp);
                    CommonClass.PI_insert_data_temp = new List<PI_master_use_insert>();
                    production_detail_already_add_main_list = true;                    
                }
            }
            // lot information 
            txt_lotinfo_itm_nam.Text = textItemName.Text;
            txt_lotinfo_itemcode.Text = textItemCode.Text;
            // print label
            txt_pi_itemname.Text = textItemName.Text;
            txt_pl_itemcode.Text = textItemCode.Text;
            // Button create and load grid header   
            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dynamic_button();
            // get lot numbers 
            List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
            List<shipping_custcd_itemcd> get_cust_itemcd_lot_only = new List<shipping_custcd_itemcd>();            
            List<string> lot_numbers_uq = new List<string>();
            string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
            string[] obj = { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, txtCustomerCode.Text, textItemCode.Text, "cust_item" };
            MySqlDataReader sdr = helper.GetReaderByCmd("get_custcd_itemcd_vs_lotno", str, obj);
            while (sdr.Read())// using read() method to read all rows one-by-one
            {
                string current_lotno = sdr["lot_no"].ToString();
                if (!lot_numbers_uq.Contains(current_lotno))
                {
                    shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                    model.customer_code = sdr["customer_code"].ToString();
                    model.item_code = sdr["item_code"].ToString();
                    model.lotno = sdr["lot_no"].ToString();
                    model.customer_name = sdr["customername"].ToString();
                    model.item_name = sdr["item_name"].ToString();
                    model.manfdt = Convert.ToDateTime(sdr["manufacturing_date"]);
                    get_cust_itemcd.Add(model);
                    lot_numbers_uq.Add(sdr["lot_no"].ToString());
                }
            }
            sdr.NextResult();
            while (sdr.Read())// using read() method to read all rows one-by-one
            {
                string current_lotno = sdr["lotno"].ToString();
                if (!lot_numbers_uq.Contains(current_lotno))
                {
                    shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                    model.customer_code = sdr["customercode"].ToString();
                    model.item_code = sdr["item_code"].ToString();
                    model.lotno = sdr["lotno"].ToString();
                    model.customer_name = sdr["customername"].ToString();
                    model.manfdt = Convert.ToDateTime(sdr["manufacturing_date"]);
                    get_cust_itemcd_lot_only.Add(model);
                    lot_numbers_uq.Add(sdr["lotno"].ToString());
                }
            }
            sdr.Close();
            helper.CloseConnection();            
            var only_lotno = (from std in get_cust_itemcd_lot_only
                      select new { std.lotno,std.customer_code,std.item_code })
                      .Except(get_cust_itemcd.Select(x => new { x.lotno, x.customer_code, x.item_code })).ToList();

            
            //DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
            //if (dtable_spm.Rows.Count > 0)
            //{
            //    foreach (DataRow drow in dtable_spm.Rows)
            //    {
            //        string current_lotno = drow["lot_no"].ToString();
            //        if (!lot_numbers_uq.Contains(current_lotno))
            //        {
            //            shipping_custcd_itemcd model = new shipping_custcd_itemcd();
            //            model.customer_code = drow["customer_code"].ToString();
            //            model.item_code = drow["item_code"].ToString();
            //            model.lotno = drow["lot_no"].ToString();
            //            model.customer_name = drow["customername"].ToString();
            //            model.item_name = drow["item_name"].ToString();
            //            get_cust_itemcd.Add(model);
            //            lot_numbers_uq.Add(drow["lot_no"].ToString());
            //        }
            //    }
            //}

            //List<shipping_custcd_itemcd> LotNo_list = (from DataRow dr in dtable_spm.Rows                                                     
            //                                     select new shipping_custcd_itemcd()
            //                                     {
            //                                         customer_code = dr["customer_code"].ToString(),
            //                                         item_code = dr["item_code"].ToString(),
            //                                         lotno = dr["lot_no"].ToString(),
            //                                         customer_name = dr["customername"].ToString(),
            //                                         item_name = dr["item_name"].ToString()
            //                                     }).ToList();

            // Pagination data below 
            CommonClass.Runtime_Store_PI_lotInfo_details = new List<shipping_custcd_itemcd>();
            CommonClass.PI_lotInfo_curentPageNo_nxtPg = PageNumber;
            foreach(var drow in only_lotno)
            {
                shipping_custcd_itemcd models = new shipping_custcd_itemcd();
                models.customer_code = drow.customer_code;
                models.item_code = drow.item_code;
                models.lotno = drow.lotno;
                var get_manfdt = get_cust_itemcd_lot_only.Where(x => x.lotno == drow.lotno)
                                    .Select(x => x.manfdt).FirstOrDefault();
                                      
                models.manfdt = get_manfdt;
                get_cust_itemcd.Add(models);
            }
          
            CommonClass.Runtime_Store_PI_lotInfo_details = get_cust_itemcd.OrderByDescending(c=>c.manfdt).ToList();
            
            var Get_records = get_cust_itemcd.OrderByDescending(c=>c.manfdt).ToPagedList(PageNumber, PageSize);
            if (Get_records.IsLastPage)
            {
                btn_nextPg.Enabled = false;
            }
            else if (Get_records.HasNextPage)
            {
                btn_nextPg.Enabled = true;
            }

            // lot no vs search time if condition go...
            if (CommonClass.view_enable)
            {
                if (chkExclude.Checked)
                {
                    if (!view_time_lotno_changed)
                    {                       
                        view_lotinfo_value_assign_gridbind_without_process();                      
                    }
                    else if (view_time_lotno_changed)
                    {
                        foreach(var get_details in Get_records)
                        {
                            Console.WriteLine("list lot no : " + get_details.lotno);
                            insert_lotinfo_value_assign_gridbind(get_details.customer_code,get_details.item_code,get_details.lotno);
                        }
                    }
                }
                else if (!chkExclude.Checked)
                {
                    if (!view_time_lotno_changed)
                    {
                            view_lotinfo_value_assign_gridbind();
                    
                    }
                    else if (view_time_lotno_changed)
                    {
                       
                        foreach (var get_details in Get_records)
                        {
                            Console.WriteLine("list lot no : " + get_details.lotno);
                            insert_lotinfo_value_assign_gridbind(get_details.customer_code, get_details.item_code, get_details.lotno);
                        }                        
                        dataGridView1.Refresh();
                       
                    }
                }
                max_lotno_id();
            }
            else if (!CommonClass.view_enable)
            {
             
                foreach (var get_details in Get_records)
                {
                    if(get_details.lotno== "1320720")
                    {
                        Console.WriteLine("list lot no : " + get_details.lotno);
                    }
                   
                    insert_lotinfo_value_assign_gridbind(get_details.customer_code, get_details.item_code, get_details.lotno);

                }
                dataGridView1.Refresh();
                
                max_lotno_id(); 
                
            }
            
            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);   
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;        
            lot_information_changed_without_grid = false;            
            Cursor.Current = Cursors.Default;          
        }
        public bool check_lotno_lotnoChild_itemCode()
        {
            try
            {
                bool result = false;
                string ActionType = "GetMappedItemCode";
                string split_lotno = textLotNoAdd.Text;
                string split_lotno_child = textLotNoChild.Text;
                string[] str_exist = { "@lno", "@lcno", "@ActionType", "custcd", "itemcd" };
                string[] obj_exist = { split_lotno, split_lotno_child, ActionType, txtCustomerCode.Text, txt_lotinfo_itemcode.Text };

                MySqlDataReader mapped_itemcode_srd = helper.GetReaderByCmd("get_mapped_itemcode", str_exist, obj_exist);
                if (mapped_itemcode_srd.Read())
                {
                    mapped_itemcode_exist = true;
                    mapped_itemcode = mapped_itemcode_srd["itemcode"].ToString();
                    if (mapped_itemcode != textItemCode.Text)
                    {
                        result = true;
                    }
                    mapped_itemcode_srd.Close();
                    helper.CloseConnection();
                }
                else
                {
                    mapped_itemcode_exist = false;
                    mapped_itemcode_srd.Close();
                    helper.CloseConnection();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("check_lotno_lotnoChild_itemCode", ex);
            }
        }
        public bool check_lotno_lotnoChild_already_exist(string ActionType)
        {
            try
            {
                bool result = false;                
                string split_lotno = textLotNoAdd.Text;
                string split_lotno_child = textLotNoChild.Text;
                string[] str_exist = { "@lno", "@lcno", "@ActionType", "custcd", "itemcd" };
                string[] obj_exist = { split_lotno, split_lotno_child, ActionType ,txtCustomerCode.Text,txt_lotinfo_itemcode.Text};
                MySqlDataReader mapped_itemcode_srd = helper.GetReaderByCmd("get_mapped_itemcode", str_exist, obj_exist);
                if (mapped_itemcode_srd.Read())
                {
                    result = true;
                    mapped_itemcode_srd.Close();
                    helper.CloseConnection();
                }
                else
                {
                    result = false;
                    mapped_itemcode_srd.Close();
                    helper.CloseConnection();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("check_lotno_lotnoChild_already_exist", ex);
            }
        }

        public bool check_input()
        {
            bool result = false;

            return result;
        }
        public string Patern_material_code(string processId)
        {
            string result = string.Empty;
            try
            {
                string ActionType = "GetData";
                string[] str_exist = { "@cust_cd", "@item_cd", "@proc_id","@lotnumber", "@ActionType" };
                string[] obj_exist = { txtCustomerCode.Text, txt_lotinfo_itemcode.Text, processId,textLotNoAdd.Text, ActionType };
                MySqlDataReader materialcode_srd = helper.GetReaderByCmd("patternone_materialid_runtime", str_exist, obj_exist);
                if (materialcode_srd.Read())
                {
                    string materialcd = materialcode_srd["materialcd"].ToString();
                    string p4_lotno = materialcode_srd["p1_lotno"].ToString();
                    string pb = materialcode_srd["p1_pb"].ToString();
                    string p3_qty = materialcode_srd["p3_qty"].ToString();
                    string p3_process_dt = materialcode_srd["p3_pdate"].ToString();
                    string notequalzero = "0";
                    // lot number
                    if (notequalzero == materialcode_srd["p1_lotno_temp"].ToString())
                    {
                        p4_lotno = materialcode_srd["p1_lotno"].ToString();
                    }
                    else
                    {
                        p4_lotno = materialcode_srd["p1_lotno_temp"].ToString();
                    
                    }
                    // db 
                    if(notequalzero== materialcode_srd["p1_pb_temp"].ToString())
                    {
                        pb = materialcode_srd["p1_pb"].ToString();
                    }
                    else
                    {
                        pb = materialcode_srd["p1_pb_temp"].ToString();
                    }
                    // p3 qty  
                    if (notequalzero == materialcode_srd["p3_qty_temp"].ToString())
                    {
                        p3_qty = materialcode_srd["p3_qty"].ToString();
                    }
                    else
                    {
                        p3_qty = materialcode_srd["p3_qty_temp"].ToString();
                    }
                    // p3 process date
                    if (notequalzero == materialcode_srd["p3_pdate_temp"].ToString())
                    {
                        p3_process_dt = materialcode_srd["p3_pdate"].ToString();
                    }
                    else
                    {
                        p3_process_dt = materialcode_srd["p3_pdate_temp"].ToString();
                    }                   
                    result = materialcd + "," + p4_lotno+","+pb + "," + p3_qty + "," + p3_process_dt;
                    materialcode_srd.Close();
                    helper.CloseConnection();

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("Patern_material_code", ex);
            }
        }
        private void Patern_Click(object sender, EventArgs e)
        {
            int lotno = 0;
            if (string.IsNullOrEmpty(textLotNoAdd.Text))
            {
                lotno = Convert.ToInt32(textLotNoAdd.Text);
            }
            if (textLotNoAdd.Text != "0000000" && textLotNoChild.Text != "00")
            {
                if (!check_lotno_lotnoChild_itemCode())
                {
                    string patern_type = ((Button)sender).Name.Split('#')[1];
                    string patern_Name = ((Button)sender).Name.Split('#')[2];
                    string process_id = ((Button)sender).Name.Split('#')[3];
                    string Material_code_selected = ((Button)sender).Name.Split('#')[5];
                    string current_btncolor = ((Button)sender).BackColor.Name;           
                    if (patern_type == "1")
                    {
                        dataGridView1_selected_items(dataGridView1_grid_selectedRow, patern_Name, process_id, 1, Material_code_selected);
                        string material_code = Patern_material_code(patern_Name);
                        FormPatern1 frm = new FormPatern1();
                        frm.Owner = this;
                        frm.OwnerName = this.Name;
                        frm.Search_lotNo = textSearchLotNo.Text;
                        frm.Material_code_selected = Material_code_selected;
                        if (string.IsNullOrEmpty(pass_pattern1_PartNo) || pass_pattern1_PartNo == "00000")
                        {
                            frm.Part_No = material_code.Split(',')[0];
                        }
                        else
                        {
                            frm.Part_No = pass_pattern1_PartNo;
                        }
                        frm.ProcessName = patern_Name;
                        frm.ProcessId = process_id;
                        frm.Sender_button = ((Button)sender).Name;
                        frm.Name = ((Button)sender).Name;
                        frm.SelectedPartNumber = selected_dgProduct_partnumber;
                        frm.SelectedLotNumber = textLotNoAdd.Text + "-" + textLotNoChild.Text;
                        if (string.IsNullOrEmpty(pass_pattern1_LotNo) || pass_pattern1_LotNo == "0000000")
                        {
                            frm.SelectedLotNumber_spl = material_code.Split(',')[1];
                        }
                        else
                        {
                            frm.SelectedLotNumber_spl = pass_pattern1_LotNo;
                        }
                        frm.SelectedManfDate = dateTimePicker_Manf.Value.ToShortDateString();
                        frm.SelectedManfDate_use_insert = dateTimePicker_Manf.Value.ToString("yyyy-MM-dd");
                        frm.SelectedManfTime = txt_manf_time.Text;
                        frm.SelectedQuantity = txt_lotinfo_quantity.Text;
                        frm.itemcode = txt_lotinfo_itemcode.Text;
                        frm.itemname = txt_lotinfo_itm_nam.Text;
                        frm.Customer_code = txtCustomerCode.Text;
                        frm.Get_planting_dt = pass_pattern1_PlantingDate;
                        frm.Get_Qty = pass_pattern1_Qty;
                        if (string.IsNullOrEmpty(pass_pattern1_PbDate) || pass_pattern1_PbDate == "00")
                        {
                            frm.Get_PbDate = material_code.Split(',')[2];
                        }
                        else
                        {
                            frm.Get_PbDate = pass_pattern1_PbDate;
                        }  
                        frm.Current_button_color = current_btncolor;
                        if (chk_bproduct.Checked)
                        {
                            frm.Bproduct_p1 = "B";
                        }
                        else if (!chk_bproduct.Checked)
                        {
                            frm.Bproduct_p1 = null;
                        }
                        if (chk_onhold.Checked)
                        {
                            frm.Onhold_p1 = "H";
                        }
                        else if (!chk_onhold.Checked)
                        {
                            frm.Onhold_p1 = null;
                        }
                        if (chkbx_scrap.Checked)
                        {
                            frm.Scrap_p1 = "S";
                            frm.Onhold_p1 = null;
                        }
                        else if (!chkbx_scrap.Checked)
                        {
                            frm.Scrap_p1 = null;
                        }
                        
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                        {
                            frm.reason_hs_p1 = txt_reason_hs.Text;
                        }
                        else
                        {
                            frm.reason_hs_p1 = null;
                        }
                        
                        frm.ShowDialog();

                    }
                    else if (patern_type == "2")
                    {
                        dataGridView1_selected_items(dataGridView1_grid_selectedRow, patern_Name, process_id, 2, Material_code_selected);
                        FormPatern2 frm = new FormPatern2();
                        frm.Owner = this;
                        frm.OwnerName = this.Name;
                        frm.Search_lotNo = textSearchLotNo.Text;
                        frm.Material_code_selected = Material_code_selected;
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
                        frm.Get_process_dt = pass_pattern2_ProcessDate;
                        frm.Get_CtrlNo = pass_pattern2_Controlno;
                        frm.Get_sheet_lotno = pass_pattern2_Sheetlotno;
                        frm.Get_Qty = pass_pattern2_Qty;
                        frm.Current_button_color = current_btncolor;
                        if (chk_bproduct.Checked)
                        {
                            frm.Bproduct_p2 = "B";
                        }
                        else if (!chk_bproduct.Checked)
                        {
                            frm.Bproduct_p2 = null;
                        }
                        if (chk_onhold.Checked)
                        {
                            frm.Onhold_p2 = "H";
                        }
                        else if (!chk_onhold.Checked)
                        {
                            frm.Onhold_p2 = null;
                        }
                        if (chkbx_scrap.Checked)
                        {
                            frm.Scrap_p2 = "S";
                            frm.Onhold_p2 = null;
                        }
                        else if (!chkbx_scrap.Checked)
                        {
                            frm.Scrap_p2 = null;
                        }                      
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                        {
                            frm.reason_hs_p2 = txt_reason_hs.Text;
                        }
                        else
                        {
                            frm.reason_hs_p2 = null;
                        }
                        frm.ShowDialog();
                    }
                    else if (patern_type == "3")
                    {
                        dataGridView1_selected_items(dataGridView1_grid_selectedRow, patern_Name, process_id, 3, Material_code_selected);
                        string p3_qty_pdate = Patern_material_code(patern_Name);
                        FormPatern3 frm = new FormPatern3();
                        frm.Owner = this;
                        frm.OwnerName = this.Name;
                        frm.ProcessName = patern_Name;
                        frm.Search_lotNo = textSearchLotNo.Text;
                        frm.Material_code_selected = Material_code_selected;
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
                        if (string.IsNullOrEmpty(pass_pattern3_ProcessDate) || pass_pattern3_ProcessDate == "00")
                        {
                            frm.Get_process_dt_p3 = p3_qty_pdate.Split(',')[4];
                        }
                        else
                        {
                            frm.Get_process_dt_p3 = pass_pattern3_ProcessDate;
                        }
                        if (string.IsNullOrEmpty(pass_pattern3_Qty) || pass_pattern3_Qty == "0000")
                        {
                            frm.Get_Qty_p3 = p3_qty_pdate.Split(',')[3];
                        }
                        else
                        {
                            frm.Get_Qty_p3 = pass_pattern3_Qty;
                        }
                        frm.Current_button_color = current_btncolor;
                        if (chk_bproduct.Checked)
                        {
                            frm.Bproduct_p3 = "B";
                        }
                        else if (!chk_bproduct.Checked)
                        {
                            frm.Bproduct_p3 = null;
                        }
                        if (chk_onhold.Checked)
                        {
                            frm.Onhold_p3 = "H";
                        }
                        else if (!chk_onhold.Checked)
                        {
                            frm.Onhold_p3 = null;
                        }
                        if (chkbx_scrap.Checked)
                        {
                            frm.Scrap_p3 = "S";
                            frm.Onhold_p3 = null;
                        }
                        else if (!chkbx_scrap.Checked)
                        {
                            frm.Scrap_p3 = null;
                        }
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                        {
                            frm.reason_hs_p3 = txt_reason_hs.Text;
                        }
                        else
                        {
                            frm.reason_hs_p3 = null;
                        }
                        frm.ShowDialog();
                    }
                    else if (patern_type == "4")
                    {
                        dataGridView1_selected_items(dataGridView1_grid_selectedRow, patern_Name, process_id, 4, Material_code_selected);
                        FormPatern4 frm = new FormPatern4();
                        frm.Owner = this;
                        frm.OwnerName = this.Name;
                        frm.Search_lotNo = textSearchLotNo.Text;
                        frm.Material_code_selected = Material_code_selected;
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
                        frm.Get_Lotno_p4 = pass_pattern4_Lotno;
                        frm.Get_Partno_p4 = pass_pattern4_PartNo;
                        frm.Get_Qty_p4 = pass_pattern4_Qty;
                        frm.Current_button_color = current_btncolor;
                        if(chk_bproduct.Checked)
                        {
                            frm.Bproduct_p4 = "B";
                        }
                        else if(!chk_bproduct.Checked)
                        {
                            frm.Bproduct_p4 = null;
                        }
                        if (chk_onhold.Checked)
                        {
                            frm.Onhold_p4 = "H";
                        }
                        else if (!chk_onhold.Checked)
                        {
                            frm.Onhold_p4 = string.Empty;
                        }
                        if (chkbx_scrap.Checked)
                        {
                            frm.Scrap_p4 = "S";
                            frm.Onhold_p4 = null;
                        }
                        else if (!chkbx_scrap.Checked)
                        {
                            frm.Scrap_p4 = null;
                        }
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                        {
                            frm.reason_hs_p4 = txt_reason_hs.Text;
                        }
                        else
                        {
                            frm.reason_hs_p4 = null;
                        }
                        frm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Lot No. and Lot No Child already mapped in some other item code..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lot No. Or Lot No Child is Null..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textLotNoAdd.Focus();
            }


        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            FormSearchClient frm = new FormSearchClient();
            MysqlHelper.call_from_productionInput_to_client = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ShowDialog();
        }
        public void SetSearchId(string code, string shortname, string fullname)
        {
            txtCustomerCode.Text = code;
            txtCustomerNameS.Text = shortname;
            txtCustomerNameF.Text = fullname;
            textItemCode.Text = string.Empty;
            textItemName.Text = string.Empty;
            textCurrency.Text = string.Empty;
            textPrice.Text = string.Empty;
            textQuantity.Text = string.Empty;
            textAdditionalCode.Text = string.Empty;
            textLabelType.Text = string.Empty;
            textMark1.Text = string.Empty;
            textMark2.Text = string.Empty;
            textMark3.Text = string.Empty;
            textMark4.Text = string.Empty;
        }
        public void FetchBOMDetails(string customercode, string itemcode)
        {
            dGProcess.Refresh();
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            ds = helper.GetDatasetByBOMView_Pro_input(customercode, itemcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtbl = ds.Tables[0];
                dGProcess.DataSource = null;
                dGProcess.AutoGenerateColumns = false;
                txtCustomerNameF.Text = dtbl.Rows[0]["customer_fullnam"].ToString();
                txtCustomerNameS.Text = dtbl.Rows[0]["customer_shortname"].ToString();
                txtCustomerCode.Text = dtbl.Rows[0]["customercode"].ToString();
                textItemCode.Text = dtbl.Rows[0]["itemcode"].ToString();
                //Set Columns Count
                dGProcess.ColumnCount = 16;
                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";
                dGProcess.Columns[0].Width = 50;
                dGProcess.Columns[1].Name = "customercode";
                dGProcess.Columns[1].DataPropertyName = "customercode";
                dGProcess.Columns[1].Width = 150;
                dGProcess.Columns[2].Name = "itemcode";
                dGProcess.Columns[2].DataPropertyName = "itemcode";
                dGProcess.Columns[2].Width = 150;
                dGProcess.Columns[3].Name = "itemname";
                dGProcess.Columns[3].DataPropertyName = "itemname";
                dGProcess.Columns[3].Width = 150;
                dGProcess.Columns[4].Name = "process_order";
                dGProcess.Columns[4].DataPropertyName = "process_order";
                dGProcess.Columns[4].Width = 150;
                dGProcess.Columns[5].Name = "process";
                dGProcess.Columns[5].DataPropertyName = "process";
                dGProcess.Columns[5].Width = 150;
                dGProcess.Columns[6].Name = "material_code";
                dGProcess.Columns[6].DataPropertyName = "material_code";
                dGProcess.Columns[6].Width = 150;
                dGProcess.Columns[7].Name = "Material Name";
                dGProcess.Columns[7].DataPropertyName = "material_name";
                dGProcess.Columns[7].Width = 150;
                dGProcess.Columns[8].Name = "Customer Name (Full)";
                dGProcess.Columns[8].DataPropertyName = "customer_fullnam";
                dGProcess.Columns[8].Visible = false;
                dGProcess.Columns[9].Name = "Customer Name (Short)";
                dGProcess.Columns[9].DataPropertyName = "customer_shortname";
                dGProcess.Columns[9].Visible = false;
                dGProcess.Columns[10].Name = "edit_allow_flag";
                dGProcess.Columns[10].DataPropertyName = "edit_allow_flag";
                dGProcess.Columns[10].Visible = false;
                dGProcess.Columns[11].Name = "idbom";
                dGProcess.Columns[11].DataPropertyName = "idbom";
                dGProcess.Columns[11].Visible = false;
                dGProcess.Columns[12].Name = "bomcode";
                dGProcess.Columns[12].DataPropertyName = "bomcode";
                dGProcess.Columns[12].Visible = false;
                dGProcess.Columns[13].Name = "inputscreentyp";
                dGProcess.Columns[13].DataPropertyName = "inputscreentyp";
                dGProcess.Columns[13].Visible = false;
                dGProcess.Columns[14].Name = "inputscreentyp_id";
                dGProcess.Columns[14].DataPropertyName = "inputscreentyp_id";
                dGProcess.Columns[14].Visible = false;
                dGProcess.Columns[15].Name = "processcode";
                dGProcess.Columns[15].DataPropertyName = "processcode";
                dGProcess.Columns[15].Visible = false;
                dGProcess.DataSource = dtbl;
                helper.CloseConnection();
            }
            else
            {
                dtbl = ds.Tables[0];
                dGProcess.DataSource = dtbl;
                dGProcess.DataSource = null;
                helper.CloseConnection();
            }
        }

        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            FormSearchItem frm = new FormSearchItem();
            MysqlHelper.call_from_productionInput_to_item = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.CustomerCode = txtCustomerCode.Text;
            frm.CustomerNames = txtCustomerNameS.Text;
            frm.CustomerNameF = txtCustomerNameF.Text;
            frm.ShowDialog();
        }
        public void SetSearchId_Item(string customercode, string itemcode, string fullname)
        {

            textItemCode.Text = itemcode;
            textItemName.Text = fullname;
            string ActionType = "GetDataCustomerItem";
            string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
            string[] obj = { customercode, "", itemcode, ActionType };
            MySqlDataReader sdr = helper.GetReaderByCmd("product_view", str, obj);
            while (sdr.Read())// using read() method to read all rows one-by-one
            {
                textItemCode.Text = sdr["itemcode"].ToString();
                textItemName.Text = sdr["itemname"].ToString();
                textCurrency.Text = sdr["unitprice_drp"].ToString();
                textPrice.Text = sdr["unitprice"].ToString();
                textQuantity.Text = sdr["boxqty"].ToString();
                textAdditionalCode.Text = sdr["additional_code"].ToString();
                textLabelType.Text = sdr["labeltype"].ToString();
                textMark1.Text = sdr["mark_1"].ToString();
                textMark2.Text = sdr["mark_2"].ToString();
                textMark3.Text = sdr["mark_3"].ToString();
                textMark4.Text = sdr["mark_4"].ToString();
            }
            sdr.Close();
            helper.CloseConnection();
            FetchBOMDetails(txtCustomerCode.Text, textItemCode.Text);
        }

        private void FormProductionInput_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void btnPIadd_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to ADD ProductionInput ?", "CREATE PRODUCTIONINPUT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {    
                Cursor.Current = Cursors.WaitCursor;
                if (CheckInput())
                {                       
                    truncate_pattern_temp();
                    Random rnd = new Random();
                    int rno = rnd.Next(100, 900);
                    CommonClass.pattern_temp_random_number = rno.ToString();
                    if (dGProcess.Rows.Count > 0)
                    {
                        dGProduct.ClearSelection();                     
                        lotinformation_addList_bindDatarow();                    
                    }                    
                }
                Cursor.Current = Cursors.Default;
                Console.WriteLine("end time : " +DateTime.Now.ToString("HH:mm:ss"));
            }
        }
        public void truncate_pattern_temp()
        {
            string[] str_exist = { "@ActionType","@commonid" };
            string[] obj_exist = { "patternall",CommonClass.pattern_temp_random_number };
            MySqlDataReader temp_tbl_truncate = helper.GetReaderByCmd("patterntbl_truncate_temp", str_exist, obj_exist);
           
            temp_tbl_truncate.Close();
            helper.CloseConnection();
        }
        public void lotinformation_addList_bindDatarow()
        {
            try
            {   
                int dG_product_row_count = 0;
                // Get pi_productinformation_master tbl max id =1 assign product_code refer below 
                max_productinput_id();              
                string rowId = product_code.ToString();             
                if (CommonClass.Process_name.Count == 0)
                {
                    PI_Process models = new PI_Process();
                    models.id = "XXX";
                    models.ProcessNames = "TERMINAL BOARD INFO";
                    models.PaternType = "5";
                    models.process_id = "0";
                    CommonClass.Process_name.Add(models);
                }                
                if (!CommonClass.view_enable)
                {
                    dG_product_row_count = dGProduct.Rows.Count + 1;
                    foreach (DataGridViewRow val in dGProcess.Rows)
                    {
                        PI_Process model = new PI_Process();
                        model.id = dG_product_row_count.ToString();
                        model.ProcessNames = val.Cells[5].Value.ToString();
                        model.PaternType = val.Cells[14].Value.ToString();
                        model.process_id = val.Cells[15].Value.ToString();
                        model.materialcode = val.Cells[6].Value.ToString();
                        model.itemcode = val.Cells[2].Value.ToString();
                        CommonClass.Process_name.Add(model);
                    }
                    int i = dGProduct.Rows.Count;
                    if (one_time_assign_dgProduct_header)
                    {
                        if (dGProduct.Rows.Count == 0)
                        {
                            dGProduct.AutoGenerateColumns = true;
                            datatable_create_new();
                        }
                        one_time_assign_dgProduct_header = false;
                    }
                    dGProduct.Rows.Add();
                    dGProduct.Rows[i].Cells[0].Value = dGProduct.Rows.Count;
                    dGProduct.Rows[i].Cells[1].Value = dG_product_row_count;                  
                    dGProduct.Rows[i].Cells[2].Value = 0;
                    dGProduct.Rows[i].Cells[3].Value = txtCustomerCode.Text;
                    dGProduct.Rows[i].Cells[4].Value = txtCustomerNameS.Text;
                    dGProduct.Rows[i].Cells[5].Value = txtCustomerNameF.Text;
                    dGProduct.Rows[i].Cells[6].Value = textItemCode.Text;
                    dGProduct.Rows[i].Cells[7].Value = textItemName.Text;
                    dGProduct.Rows[i].Cells[8].Value = textCurrency.Text;
                    dGProduct.Rows[i].Cells[9].Value = textPrice.Text;
                    dGProduct.Rows[i].Cells[10].Value = textQuantity.Text;
                    dGProduct.Rows[i].Cells[11].Value = textAdditionalCode.Text;
                    dGProduct.Rows[i].Cells[12].Value = textLabelType.Text;
                    dGProduct.Rows[i].Cells[13].Value = textMark1.Text;
                    dGProduct.Rows[i].Cells[14].Value = textMark2.Text;
                    dGProduct.Rows[i].Cells[15].Value = textMark3.Text;
                    dGProduct.Rows[i].Cells[16].Value = textMark4.Text;
                    dGProduct.AutoGenerateColumns = false;
                    this.dGProduct.Refresh();
                    // Product information Master details 
                    PI_master_use_insert pi_insert = new PI_master_use_insert();
                    pi_insert.id = rowId;
                    pi_insert.Customercode = txtCustomerCode.Text;
                    pi_insert.CustomerFnam = txtCustomerNameF.Text;
                    pi_insert.CustomerSnam = txtCustomerNameS.Text;
                    pi_insert.Itemcode = textItemCode.Text;
                    pi_insert.Itemnam = textItemName.Text;
                    pi_insert.Unittype = textCurrency.Text;
                    pi_insert.Unitprice = textPrice.Text;
                    pi_insert.Boxqty = textQuantity.Text;
                    pi_insert.Addcd = textAdditionalCode.Text;
                    pi_insert.lbltype = textLabelType.Text;
                    pi_insert.m1 = textMark1.Text;
                    pi_insert.m2 = textMark2.Text;
                    pi_insert.m3 = textMark3.Text;
                    pi_insert.m4 = textMark4.Text;
                    CommonClass.PI_insert_data_temp.Add(pi_insert);
                    production_detail_already_add_main_list = false;                   
                    dGProduct.Rows[i].Selected = true;
                    // Producation Information Tab grid row selected method
                    dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                    // or even better, use .DisableResizing. Most time consuming enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                    // set it to false if not needed
                    dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, i));                    
                }
                else if (CommonClass.view_enable)
                {
                    int irow = 1;
                    foreach (DataGridViewRow val in dGProcess.Rows)
                    {
                        PI_Process model = new PI_Process();
                        model.id = irow.ToString();
                        model.ProcessNames = val.Cells[5].Value.ToString();
                        model.PaternType = val.Cells[14].Value.ToString();
                        model.process_id = val.Cells[15].Value.ToString();
                        model.materialcode = val.Cells[6].Value.ToString();
                        model.itemcode = val.Cells[2].Value.ToString();
                        CommonClass.Process_name.Add(model);
                        irow++;
                    }
                    int i = dGProduct.Rows.Count;               
                    if (dGProduct.Rows.Count == 0)
                    {
                        datatable_create_new();
                    }
                    dGProduct.Rows.Add();
                    dGProduct.Rows[i].Cells[0].Value = dGProduct.Rows.Count;
                    dGProduct.Rows[i].Cells[1].Value = dG_product_row_count;
                    dGProduct.Rows[i].Cells[2].Value = 0;
                    dGProduct.Rows[i].Cells[3].Value = txtCustomerCode.Text;
                    dGProduct.Rows[i].Cells[4].Value = txtCustomerNameS.Text;
                    dGProduct.Rows[i].Cells[5].Value = txtCustomerNameF.Text;
                    dGProduct.Rows[i].Cells[6].Value = textItemCode.Text;
                    dGProduct.Rows[i].Cells[7].Value = textItemName.Text;
                    dGProduct.Rows[i].Cells[8].Value = textCurrency.Text;
                    dGProduct.Rows[i].Cells[9].Value = textPrice.Text;
                    dGProduct.Rows[i].Cells[10].Value = textQuantity.Text;
                    dGProduct.Rows[i].Cells[11].Value = textAdditionalCode.Text;
                    dGProduct.Rows[i].Cells[12].Value = textLabelType.Text;
                    dGProduct.Rows[i].Cells[13].Value = textMark1.Text;
                    dGProduct.Rows[i].Cells[14].Value = textMark2.Text;
                    dGProduct.Rows[i].Cells[15].Value = textMark3.Text;
                    dGProduct.Rows[i].Cells[16].Value = textMark4.Text;
                    // Product information Master details 
                    PI_master_use_insert pi_insert = new PI_master_use_insert();
                    pi_insert.id = rowId;
                    pi_insert.Customercode = txtCustomerCode.Text;
                    pi_insert.CustomerFnam = txtCustomerNameF.Text;
                    pi_insert.CustomerSnam = txtCustomerNameS.Text;
                    pi_insert.Itemcode = textItemCode.Text;
                    pi_insert.Itemnam = textItemName.Text;
                    pi_insert.Unittype = textCurrency.Text;
                    pi_insert.Unitprice = textPrice.Text;
                    pi_insert.Boxqty = textQuantity.Text;
                    pi_insert.Addcd = textAdditionalCode.Text;
                    pi_insert.lbltype = textLabelType.Text;
                    pi_insert.m1 = textMark1.Text;
                    pi_insert.m2 = textMark2.Text;
                    pi_insert.m3 = textMark3.Text;
                    pi_insert.m4 = textMark4.Text;
                    CommonClass.PI_insert_data_temp.Add(pi_insert);
                    production_detail_already_add_main_list = false;
                    //080922 view time add the new customer means
                    view_time_lotno_changed = true;
                    dGProduct.Rows[i].Selected = true;
                    dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, i));                    
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("lotinformation_addList_bindDatarow", ex);
            }
        }
        public void dgProduct_add(int dG_product_row_count)
        {
            DataRow dr;
            dr = new_dt.NewRow();
            dr[0] = dGProduct.Rows.Count + 1;
            dr[1] = dG_product_row_count;
            dr[2] = 0;
            dr[3] = txtCustomerCode.Text;
            dr[4] = txtCustomerNameS.Text;
            dr[5] = txtCustomerNameF.Text;
            dr[6] = textItemCode.Text;
            dr[7] = textItemName.Text;
            dr[8] = textCurrency.Text;
            dr[9] = textPrice.Text;
            dr[10] = textQuantity.Text;
            dr[11] = textAdditionalCode.Text;
            dr[12] = textLabelType.Text;
            dr[13] = textMark1.Text;
            dr[14] = textMark2.Text;
            dr[15] = textMark3.Text;
            dr[16] = textMark4.Text;
            new_dt.Rows.Add(dr);
            product_code++;
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
            else if (textPrice.Text.Trim() == "")
            {
                MessageBox.Show("Price is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textPrice.Focus();
                result = false;
            }
            else if (textQuantity.Text == "")
            {
                MessageBox.Show("BoxQuantity is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textQuantity.Focus();
                result = false;
            }
            else if (textLabelType.Text.Trim() == "")
            {
                MessageBox.Show("Lable Type is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textLabelType.Focus();
                result = false;
            }            
            else if (dGProcess.Rows.Count == 0)
            {
                MessageBox.Show("Process Details Datagrid view is Empty..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dGProcess.Focus();
                result = false;
            }
            foreach (DataGridViewRow row in this.dGProduct.Rows)
            {
                if (row.Cells[3].Value.Equals(txtCustomerCode.Text) && row.Cells[6].Value.Equals(textItemCode.Text))
                {
                    // row exists
                    result = false;
                    MessageBox.Show("Customer Code and Item code already exist", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textItemCode.Focus();
                    break;
                }
            }
            return result;
        }
        public bool CheckInputLotInfoTab()
        {
            bool result = true;
            if (textLotNoAdd.Text.Trim() == "" || textLotNoAdd.Text == "0000000")
            {
                MessageBox.Show("LotNo is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textLotNoAdd.Focus();
                result = false;
            }
            else if (textLotNoChild.Text.Trim() == "" || textLotNoChild.Text == "00")
            {
                MessageBox.Show("LotNo Child is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textLotNoChild.Focus();
                result = false;
            }
            else if (txt_manf_time.Text.Trim() == "" || txt_manf_time.Text == "00:00:00")
            {
                MessageBox.Show("Manufacturing Time is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_manf_time.Focus();
                result = false;
            }
            else if (txt_lotinfo_quantity.Text.Trim() == "")
            {
                MessageBox.Show("Quantity is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_lotinfo_quantity.Focus();
                result = false;
            }
            else if (txt_lotinfo_itm_nam.Text.Trim() == "")
            {
                MessageBox.Show("Item name is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_lotinfo_itm_nam.Focus();
                result = false;
            }
            return result;
        }
        public void resetInput()
        {
            this.tabControl1.SelectedTab = tabPage1;
            dGProcess.DataSource = null;
            txtCustomerCode.Text = string.Empty;
            txtCustomerNameF.Text = string.Empty;
            txtCustomerNameS.Text = string.Empty;
            textItemCode.Text = string.Empty;
            textItemName.Text = string.Empty;
            textPrice.Text = string.Empty;
            textQuantity.Text = string.Empty;
            textAdditionalCode.Text = string.Empty;
            textLabelType.Text = string.Empty;
            textMark1.Text = string.Empty;
            textMark2.Text = string.Empty;
            textMark3.Text = string.Empty;
            textMark4.Text = string.Empty;
            textCurrency.Text = string.Empty;
            panel1.Controls.OfType<Button>().ToList().ForEach(btn => btn.Dispose());            
            CommonClass.Process_name_gridbind = new List<PI_Process>();
        }
        public void resetInputLotInfoTab()
        {
            dataGridView1.DataSource = null;
            textLotNoAdd.Text = "0000000";
            textLotNoChild.Text = "00";
            txt_lotinfo_quantity.Text = "0000";
            DateTime current_time = DateTime.Now;
            txt_manf_time.Text = current_time.ToString("HH:mm:ss");
            txt_lotinfo_itm_nam.Text = string.Empty;
            CommonClass.p1 = false;
            CommonClass.p2 = false;
            CommonClass.p3 = false;
            CommonClass.p4 = false;
            CommonClass.up_p1 = false;
            CommonClass.up_p2 = false;
            CommonClass.up_p3 = false;
            CommonClass.up_p4 = false;
        }
        public void resetInputPrintLabelTab()
        {
            dataGridView1.DataSource = null;
            txt_pl_lotno.Text = "0000000";
            txt_pl_frm_lotc.Text = "01";
            txt_pl_to_lotc.Text = "00";
            txt_pl_itemcode.Text = string.Empty;
            txt_pi_itemname.Text = string.Empty;
            CommonClass.p1 = false;
            CommonClass.p2 = false;
            CommonClass.p3 = false;
            CommonClass.p4 = false;
            CommonClass.up_p1 = false;
            CommonClass.up_p2 = false;
            CommonClass.up_p3 = false;
            CommonClass.up_p4 = false;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TabPage selectedTab = tabControl1.SelectedTab;
                tabControl1.SelectedTab = selectedTab;               
                if (tabControl1.SelectedTab.Text == "Lot Information")
                {
                    dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                    string boxQty = textQuantity.Text;
                    boxQty = boxQty.Replace(",", "");
                    txt_lotinfo_quantity.Text = boxQty;                
                }
                else if (tabControl1.SelectedTab.Text == "Print Label")
                {
                    date_print_lable_picker.Value = DateTime.Today.AddDays(-1);
                    date_print_lable_picker_to.Value = DateTime.Today.AddDays(-1);
                    store_printer_name_get();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("tabControl1_SelectedIndexChanged", ex);
            }

        }
        private void Patern3(string formname)
        {
            FormPatern3 frm = new FormPatern3();
            frm.Owner = this;
            frm.OwnerName = this.Name;
            frm.ProcessName = this.Name;
            frm.ShowDialog();
        }
        public void change_color_button(string id)
        {
            try
            {
                panel1.Controls.Find(id, true)[0].BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("change_color_button", ex);
            }

        }
        private void dynamic_button()
        {
            try
            {
                int i = 10;
                int x = -1;
                panel1.Controls.Clear();
                int total_process = CommonClass.Process_name.Count;
                CommonClass.Process_name = CommonClass.Process_name.OrderBy(o => o.process_id).ToList();
                foreach (var itm in CommonClass.Process_name)
                {
                    Color back_clr = System.Drawing.Color.Red;
                    Color fore_clr = System.Drawing.Color.White;
                    string getid = itm.id;
                    // insert time 
                    if (!CommonClass.view_enable)
                    {
                        // Production information tab : selected partnumber only button create
                        if (selected_dgProduct_partnumber == getid)
                        {
                            //This block dynamically creates a Button and adds it to the form
                            Button btn = new Button();
                            btn.BackColor = back_clr;
                            btn.ForeColor = fore_clr;
                            btn.Location = new System.Drawing.Point(19, 29);
                            btn.Name = itm.id + "#" + itm.PaternType + "#" + itm.ProcessNames + "#" + itm.process_id + "#" + itm.itemcode + "#" + itm.materialcode;
                            btn.Size = new System.Drawing.Size(80, 60);
                            btn.TabIndex = 103;
                            btn.Text = itm.ProcessNames;
                            btn.UseVisualStyleBackColor = false;
                            btn.Click += new System.EventHandler(this.Patern_Click);
                            btn.Location = new Point(i, x);
                            panel1.AutoScroll = true;
                            panel1.Controls.Add(btn);
                            i += 100;
                        }
                    }
                    else if (CommonClass.view_enable && getid != "XXX")
                    {
                        
                            //This block dynamically creates a Button and adds it to the form
                            Button btn = new Button();
                            btn.BackColor = System.Drawing.Color.Red;
                            btn.ForeColor = System.Drawing.Color.White;
                            btn.Location = new System.Drawing.Point(19, 29);
                            btn.Name = itm.id + "#" + itm.PaternType + "#" + itm.ProcessNames + "#" + itm.process_id + "#" + itm.itemcode + "#" + itm.materialcode;
                            btn.Size = new System.Drawing.Size(80, 60);
                            btn.TabIndex = 103;
                            btn.Text = itm.ProcessNames;
                            btn.UseVisualStyleBackColor = false;
                            btn.Click += new System.EventHandler(this.Patern_Click);
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
                throw new ArgumentNullException("dynamic_button", ex);
            }
        }
        private void txtCustomerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtCustomerCode.Text != "" && txtCustomerCode.Text != "000000")
            {
                    FetchBOMDetails(txtCustomerCode.Text, "");
           
            }
        }

        private void btnPIDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to Delete Selected Product ?", "DELETE PRODUCT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dGProduct.RowCount > 0)
                {
                    int rowIndex = dGProduct.CurrentCell.RowIndex;
                    // remove temp list 
                    DataGridViewRow row = dGProduct.Rows[rowIndex];                   
                    string deleted_id = row.Cells[1].Value.ToString();
                    string customer_cd = row.Cells[3].Value.ToString();
                    string item_cd = row.Cells[6].Value.ToString();
                    CommonClass.Process_name.RemoveAll(x => x.id == deleted_id);
                    CommonClass.Process_name_gridbind.RemoveAll(x => x.id == deleted_id);
                    CommonClass.Process_name_gridbind_columns.RemoveAll(x => x.id == deleted_id);
                    CommonClass.PI_insert_data.RemoveAll(x => x.Customercode == customer_cd && x.Itemcode == item_cd);
                    // remove gridview
                    dGProduct.Rows.RemoveAt(rowIndex);
                    resetInput();
                    resetInputLotInfoTab();
                    
                }
                Cursor.Current = Cursors.Default;
            }
        }
        public void AddPaternDetails(string customercode, string shortname)
        {
            dataGridView1.Refresh();
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            ds = helper.GetDatasetByBOMView(customercode, shortname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtbl = ds.Tables[0];
                dataGridView1.DataSource = null;
                dataGridView1.AutoGenerateColumns = false;
                txtCustomerNameF.Text = dtbl.Rows[0]["customer_fullnam"].ToString();
                txtCustomerNameS.Text = dtbl.Rows[0]["customer_shortname"].ToString();
                txtCustomerCode.Text = dtbl.Rows[0]["customercode"].ToString();
                //Set Columns Count
                dGProcess.ColumnCount = 15;
                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";
                dGProcess.Columns[0].Width = 50;
                dGProcess.Columns[1].Name = "customercode";
                dGProcess.Columns[1].DataPropertyName = "customercode";
                dGProcess.Columns[1].Width = 150;
                dGProcess.Columns[2].Name = "itemcode";
                dGProcess.Columns[2].DataPropertyName = "itemcode";
                dGProcess.Columns[2].Width = 150;
                dGProcess.Columns[3].Name = "itemname";
                dGProcess.Columns[3].DataPropertyName = "itemname";
                dGProcess.Columns[3].Width = 150;
                dGProcess.Columns[4].Name = "process_order";
                dGProcess.Columns[4].DataPropertyName = "process_order";
                dGProcess.Columns[4].Width = 150;
                dGProcess.Columns[5].Name = "process";
                dGProcess.Columns[5].DataPropertyName = "process";
                dGProcess.Columns[5].Width = 150;
                dGProcess.Columns[6].Name = "material_code";
                dGProcess.Columns[6].DataPropertyName = "material_code";
                dGProcess.Columns[6].Width = 150;
                dGProcess.Columns[7].Name = "material_name";
                dGProcess.Columns[7].DataPropertyName = "material_name";
                dGProcess.Columns[7].Width = 150;
                dGProcess.Columns[8].Name = "customer_fullnam";
                dGProcess.Columns[8].DataPropertyName = "customer_fullnam";
                dGProcess.Columns[8].Visible = false;
                dGProcess.Columns[9].Name = "customer_shortname";
                dGProcess.Columns[9].DataPropertyName = "customer_shortname";
                dGProcess.Columns[9].Visible = false;
                dGProcess.Columns[10].Name = "edit_allow_flag";
                dGProcess.Columns[10].DataPropertyName = "edit_allow_flag";
                dGProcess.Columns[10].Visible = false;
                dGProcess.Columns[11].Name = "idbom";
                dGProcess.Columns[11].DataPropertyName = "idbom";
                dGProcess.Columns[11].Visible = false;
                dGProcess.Columns[12].Name = "bomcode";
                dGProcess.Columns[12].DataPropertyName = "bomcode";
                dGProcess.Columns[12].Visible = false;
                dGProcess.Columns[13].Name = "inputscreentyp";
                dGProcess.Columns[13].DataPropertyName = "inputscreentyp";
                dGProcess.Columns[13].Visible = false;
                dGProcess.Columns[14].Name = "inputscreentyp_id";
                dGProcess.Columns[14].DataPropertyName = "inputscreentyp_id";
                dGProcess.Columns[14].Visible = false;
                dGProcess.DataSource = dtbl;
                helper.CloseConnection();
            }
            else
            {
                dtbl = ds.Tables[0];
                dGProcess.DataSource = dtbl;
                dGProcess.DataSource = null;
                helper.CloseConnection();
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {           
            if (!CommonClass.lot_info_changes)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Close This Form?", "CLOSE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    CommonClass.lot_info_changes = false;
                    this.Close();
                }
            }
            else if (CommonClass.lot_info_changes)
            {
                DialogResult dialogResult = MessageBox.Show("Lot Information Didn't Save, Do you want to Close This Form Means Lost the Data?", "CLOSE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this.Close();
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void btn_lotinfo_add_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (CommonClass.lot_info_changes)
                {
                    if (CheckInputLotInfoTab())
                    {
                        int grid_count = dataGridView1.Rows.Count;
                        if (grid_count > 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("Do you want to Add LotInformation ?", "ADD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                string ActionType = "master";                    
                                string ActionType_pattern_p1 = string.Empty;
                                string ActionType_pattern_p2 = string.Empty;
                                string ActionType_pattern_p3 = string.Empty;
                                string ActionType_pattern_p4 = string.Empty;
                                if (CommonClass.p1)
                                {
                                    ActionType_pattern_p1 = "p1";
                                }
                                if (CommonClass.p2)
                                {
                                    ActionType_pattern_p2 = "p2";
                                }
                                if (CommonClass.p3)
                                {
                                    ActionType_pattern_p3 = "p3";
                                }
                                if (CommonClass.p4)
                                {
                                    ActionType_pattern_p4 = "p4";
                                }
                                DateTime current_time = DateTime.Now;
                                string[] str_exist = { "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@manftime", "@stus", "@created_at", "@ActionType", "@ActionType_p1", "@ActionType_p2", "@ActionType_p3", "@ActionType_p4", "@commonId" };
                                string[] obj_exist = { textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Text, txt_manf_time.Text, "1", current_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType, ActionType_pattern_p1, ActionType_pattern_p2, ActionType_pattern_p3, ActionType_pattern_p4, CommonClass.pattern_temp_random_number };
                                MySqlDataReader all_patern = helper.GetReaderByCmd("allpatern_insert_main_new", str_exist, obj_exist);
                                if (all_patern.Read())
                                {
                                    all_patern.Close();     
                                    helper.CloseConnection();
                                    string[] str_upt = { "@lno", "@lcno", "@ActionType", "@ActionType_p1", "@ActionType_p2", "@ActionType_p3", "@ActionType_p4", "@commonId" };
                                    string[] obj_up = { textLotNoAdd.Text, textLotNoChild.Text, ActionType, ActionType_pattern_p1, ActionType_pattern_p2, ActionType_pattern_p3, ActionType_pattern_p4, CommonClass.pattern_temp_random_number };
                                    MySqlDataReader allpatter_upt = helper.GetReaderByCmd("allpattern_update_new", str_upt, obj_up);
                                   
                                    allpatter_upt.Close();
                                    helper.CloseConnection();
                                    if (lot_information_changed_without_grid)
                                    {
                                        string exp_date = dateTimePicker_Manf.Value.ToShortDateString();
                                        DateTime oDate = Convert.ToDateTime(exp_date);
                                        DateTime nextYear = oDate.AddYears(+1);
                                        exp_date = nextYear.ToString("yyyy-MM-dd");
                                        string Bproduct = null;
                                        string Onhold = null;
                                        string scrap = null;
                                        if (chk_bproduct.Checked)
                                        {
                                            Bproduct = "B";
                                        }
                                        if (chk_onhold.Checked)
                                        {
                                            Onhold= "H";
                                        }
                                        if(chkbx_scrap.Checked)
                                        {
                                            scrap = "S";
                                            Onhold = null;
                                        }
                                        string reason = null;
                                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                                        {
                                            reason = txt_reason_hs.Text;
                                        }
                                        DateTime current_date_time = DateTime.Now;
                                        string ActionType_upt = "all";
                                        string[] str_updlotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@expirydate", "@manftime", "@bpro", "@updatedat", "@ActionType","@hld","@uid","@scrp","@reason" };
                                        string[] obj_updlotinfo = { txtCustomerCode.Text, textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text, Bproduct, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType_upt,Onhold,CommonClass.logged_Id,scrap,reason };
                                        MySqlDataReader all_patern_upd = helper.GetReaderByCmd("allpatern_update_lotinfo_only", str_updlotinfo, obj_updlotinfo);
                                 
                                        all_patern_upd.Close();
                                        helper.CloseConnection();
                                        lot_information_changed_without_grid = false;                                        
                                    }                           
                                    CommonClass.p1 = false;
                                    CommonClass.p2 = false;
                                    CommonClass.p3 = false;
                                    CommonClass.p4 = false;
                                    CommonClass.lot_info_changes = false;
                                    // product information data insert 
                                    product_inforamtion_insert();                                   
                                    max_lotno_id();                               
                                    MessageBox.Show("Add LotInformation Insert Successfully", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);                                 
                                    dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, dgProduct_grid_selectedRow));
                                    dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);                                    
                                    txt_manf_time.Text = current_time.ToString("HH:mm:ss");
                                    chkbx_scrap.Checked = false;
                                    chk_onhold.Checked = false;
                                    chk_bproduct.Checked = false;
                                    txt_reason_hs.Text = "Remarks";
                                    txt_reason_hs.ForeColor = Color.Gray;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Production Input Grid is null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }  
                    }
                }
                else
                {
                    MessageBox.Show("No Changes Right now..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textSearchLotNo.Focus();                    
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_lotinfo_add_Click", ex);
            }
        }
        public void product_inforamtion_insert()
        {
            try
            {
                if (CommonClass.PI_insert_data.Count > 0)
                {
                    string ActionType = "productinfo";
                    foreach (var item in CommonClass.PI_insert_data)
                    {
                        if (item.lotno != null)
                        {
                            if (!pinfo_id_already_exist(item.lotno, item.Customercode, item.Itemcode, "pi_info_master_with_lotno"))
                            {
                                DateTime current_date_time = DateTime.Now;
                                string[] str = { "@lotno", "@lotnoc", "@cust_cd", "@cust_snam", "@cust_fnam", "@item_cd", "@itmname", "@unit_price_ctry_cd", "@unit_price", "@box_qty", "@add_cd", "@lbl_typ", "@m1", "@m2", "@m3", "@m4", "@stus_pi", "@created_at", "@ActionType", };
                                string[] obj = { item.lotno, textLotNoChild.Text, item.Customercode, item.CustomerSnam, item.CustomerFnam, item.Itemcode, item.Itemnam, item.Unittype, item.Unitprice, item.Boxqty, item.Addcd, item.lbltype, item.m1, item.m2, item.m3, item.m4, "1", current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType };
                                MySqlDataReader PI_insert = helper.GetReaderByCmd("pi_lotinfo_master", str, obj);
                                if (PI_insert.Read())
                                {
                                    PI_insert.Close();
                                    helper.CloseConnection();
                                }
                            }

                        }
                    }                    
                    CommonClass.PI_insert_data_temp = new List<PI_master_use_insert>();
                }

                // lotnumber change so insert the pi table 
                if (lotnumber_changed_add_pi_tbl)
                {
                    if (!pinfo_id_already_exist(textLotNoAdd.Text, txtCustomerCode.Text, textItemCode.Text, "pi_info_master_with_lotno"))
                    {
                        DateTime current_date_time = DateTime.Now;
                        string[] str = { "@lotno", "@lotnoc", "@cust_cd", "@cust_snam", "@cust_fnam", "@item_cd", "@itmname", "@unit_price_ctry_cd", "@unit_price", "@box_qty", "@add_cd", "@lbl_typ", "@m1", "@m2", "@m3", "@m4", "@stus_pi", "@created_at", "@ActionType", };
                        string[] obj = {
                        textLotNoAdd.Text,
                        textLotNoChild.Text,
                        txtCustomerCode.Text,
                        txtCustomerNameF.Text,
                        txtCustomerNameS.Text,
                        textItemCode.Text,
                        textItemName.Text,
                        textCurrency.Text,
                        textPrice.Text,
                        textQuantity.Text,
                        textAdditionalCode.Text,
                        textLabelType.Text,
                        textMark1.Text,
                        textMark2.Text,
                        textMark3.Text,
                        textMark4.Text,
                        "1",
                        current_date_time.ToString("yyyy-MM-dd HH:mm:ss"),
                        "productinfo" };
                        MySqlDataReader PI_insert = helper.GetReaderByCmd("pi_lotinfo_master", str, obj);
                        if (PI_insert.Read())
                        {
                            PI_insert.Close();
                            helper.CloseConnection();
                        }
                    }
                    lotnumber_changed_add_pi_tbl = false;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("product_inforamtion_insert", ex);
            }
        }
        public void product_inforamtion_insert_only_lotno_addtime()
        {
            try
            {
                // lotnumber change so insert the pi table 
                if (lotnumber_only_changed_add_pi_tbl)
                {
                    if (!pinfo_id_already_exist(textLotNoAdd.Text, txtCustomerCode.Text, textItemCode.Text, "pi_info_master_with_lotno"))
                    {
                        DateTime current_date_time = DateTime.Now;
                        string[] str = { "@lotno", "@lotnoc", "@cust_cd", "@cust_snam", "@cust_fnam", "@item_cd", "@itmname", "@unit_price_ctry_cd", "@unit_price", "@box_qty", "@add_cd", "@lbl_typ", "@m1", "@m2", "@m3", "@m4", "@stus_pi", "@created_at", "@ActionType", };
                        string[] obj = {
                        textLotNoAdd.Text,
                        textLotNoChild.Text,
                        txtCustomerCode.Text,
                        txtCustomerNameF.Text,
                        txtCustomerNameS.Text,
                        textItemCode.Text,
                        textItemName.Text,
                        textCurrency.Text,
                        textPrice.Text,
                        textQuantity.Text,
                        textAdditionalCode.Text,
                        textLabelType.Text,
                        textMark1.Text,
                        textMark2.Text,
                        textMark3.Text,
                        textMark4.Text,
                        "1",
                        current_date_time.ToString("yyyy-MM-dd HH:mm:ss"),
                        "productinfo" };
                        MySqlDataReader PI_insert = helper.GetReaderByCmd("pi_lotinfo_master", str, obj);
                        if (PI_insert.Read())
                        {
                            PI_insert.Close();
                            helper.CloseConnection();
                        }
                    }
                    lotnumber_only_changed_add_pi_tbl = false;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("product_inforamtion_insert_only_lotno_addtime", ex);
            }
        }
        public bool pinfo_id_already_exist(string lotno, string cust_cd, string item_cd, string ActionType_exist)
        {
            bool result = false;            
            string[] str_exist = { "@val1", "@val2", "@val3", "@ActionType_exist" };
            string[] obj_exist = { lotno, cust_cd, item_cd, ActionType_exist };
            MySqlDataReader already_exist = helper.GetReaderByCmd("already_exist_common", str_exist, obj_exist);
            if (already_exist.Read())
            {
                string exist = already_exist["lotno"].ToString();

                if (exist != "0")
                {
                    already_exist.Close();
                    helper.CloseConnection();
                    result = true;
                }
                else
                {
                    already_exist.Close();
                    helper.CloseConnection();
                }
            }
            else
            {
                already_exist.Close();
                helper.CloseConnection();

            }
            return result;
        }
        private void textItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textItemCode.Text != "" && textItemCode.Text != "000000" && txtCustomerCode.Text.Trim() != string.Empty && txtCustomerCode.Text != "000000")
                {
                    SetSearchId_Item(txtCustomerCode.Text, textItemCode.Text, "");
                }
                else
                {
                    MessageBox.Show("Check the Customer code or Item code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerCode.Focus();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!search_inputcheck())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //2306
                    truncate_pattern_temp();
                    Random rnd = new Random();
                    int rno = rnd.Next(100, 900);
                    CommonClass.pattern_temp_random_number = rno.ToString();                    
                    txt_print_person_name.Text = "Name";
                    txt_print_person_name.ForeColor = Color.Gray;
                    lotnumber_only_changed_add_pi_tbl = false;
                    view_time_lotno_changed = false;
                    production_detail_already_add_main_list = false;
                    resetInput();
                    resetInputLotInfoTab();
                    truncate_pattern_temp();
                    CommonClass.view_enable = true;
                    //add button visiable false 
                    btn_lotinfo_add.Visible = false;
                    //save button visiable true 
                    btn_lotinfo_save.Visible = true;
                    dt = new DataTable();
                    dt_view_lotno_only = new DataTable();
                    new_dt = new DataTable();
                    dGProduct.DataSource = null;
                    dGProduct.Columns.Clear();
                    daysInMonths = new int[] { };
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.Refresh();
                    max_productinput_id();
                    Selected_patternTyp = helper.Process_pattern_id(cmbProcess.SelectedValue.ToString());                  
                    string rowId = product_code.ToString();
                    CommonClass.Process_name = new List<PI_Process>();
                    CommonClass.Process_name_gridbind = new List<PI_Process>();
                    CommonClass.Process_name_gridbind_columns = new List<PI_Process>();
                    string ActionType = "pifetch";
                    string[] str = { "@ActionType", "@lotno" };
                    string[] obj = { ActionType, textSearchLotNo.Text };
                    DataSet ds = helper.GetDatasetByCommandString("pi_productinfo_fetch", str, obj);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dGProduct.DataSource = null;
                        dGProduct.AutoGenerateColumns = false;
                        new_dt = ds.Tables[0];
                        txtCustomerCode.Text = dt.Rows[0]["customercode"].ToString();
                        textItemCode.Text = dt.Rows[0]["item_code"].ToString();
                        textItemName.Text = dt.Rows[0]["item_name"].ToString();
                        textCurrency.Text = dt.Rows[0]["unit_price_country_shortcd"].ToString();
                        textPrice.Text = dt.Rows[0]["unit_price"].ToString();
                        textQuantity.Text = dt.Rows[0]["box_qty"].ToString();
                        textAdditionalCode.Text = dt.Rows[0]["additional_code"].ToString();
                        textLabelType.Text = dt.Rows[0]["lable_typ"].ToString();
                        textMark1.Text = dt.Rows[0]["m1"].ToString();
                        textMark2.Text = dt.Rows[0]["m2"].ToString();
                        textMark3.Text = dt.Rows[0]["m3"].ToString();
                        textMark4.Text = dt.Rows[0]["m4"].ToString();                 
                        int i = 0;             
                        if (dGProduct.Rows.Count == 0)
                        {
                            datatable_create_new();
                        }
                        foreach (DataRow drow in new_dt.Rows)
                        {
                            dGProduct.Rows.Add();
                            dGProduct.Rows[i].Cells[0].Value = i + 1;
                            dGProduct.Rows[i].Cells[1].Value = drow["idpi_product_information"];
                            dGProduct.Rows[i].Cells[2].Value = drow["lotno"].ToString();
                            dGProduct.Rows[i].Cells[3].Value = drow["customercode"].ToString();
                            dGProduct.Rows[i].Cells[4].Value = drow["customershort_name"].ToString();
                            dGProduct.Rows[i].Cells[5].Value = drow["customerfull_name"].ToString();
                            dGProduct.Rows[i].Cells[6].Value = drow["item_code"].ToString();
                            dGProduct.Rows[i].Cells[7].Value = drow["item_name"].ToString();
                            dGProduct.Rows[i].Cells[8].Value = drow["unit_price_country_shortcd"].ToString();
                            dGProduct.Rows[i].Cells[9].Value = drow["unit_price"].ToString();
                            dGProduct.Rows[i].Cells[10].Value = drow["box_qty"].ToString();
                            dGProduct.Rows[i].Cells[11].Value = drow["additional_code"].ToString();
                            dGProduct.Rows[i].Cells[12].Value = drow["lable_typ"].ToString();
                            dGProduct.Rows[i].Cells[13].Value = drow["m1"].ToString();
                            dGProduct.Rows[i].Cells[14].Value = drow["m2"].ToString();
                            dGProduct.Rows[i].Cells[15].Value = drow["m3"].ToString();
                            dGProduct.Rows[i].Cells[16].Value = drow["m4"].ToString();
                            // view time need to add list for insert tbl pi 
                            PI_master_use_insert pi_insert = new PI_master_use_insert();
                            pi_insert.id = rowId;
                            pi_insert.Customercode = txtCustomerCode.Text;
                            pi_insert.CustomerFnam = txtCustomerNameF.Text;
                            pi_insert.CustomerSnam = txtCustomerNameS.Text;
                            pi_insert.Itemcode = textItemCode.Text;
                            pi_insert.Itemnam = textItemName.Text;
                            pi_insert.Unittype = textCurrency.Text;
                            pi_insert.Unitprice = textPrice.Text;
                            pi_insert.Boxqty = textQuantity.Text;
                            pi_insert.Addcd = textAdditionalCode.Text;
                            pi_insert.lbltype = textLabelType.Text;
                            pi_insert.m1 = textMark1.Text;
                            pi_insert.m2 = textMark2.Text;
                            pi_insert.m3 = textMark3.Text;
                            pi_insert.m4 = textMark4.Text;
                            pi_insert.lotno = drow["lotno"].ToString();
                            CommonClass.PI_insert_data_temp.Add(pi_insert);                          
                            i++;
                        }
                        helper.CloseConnection();

                        dGProduct.Rows[0].Selected = true;
                        dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, 0));
                        lot_information_changed_without_grid = false;                      
                    }                    
                    else
                    {
                        MessageBox.Show("No Records Found..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textSearchLotNo.Focus();
                        CommonClass.view_enable = false;
                        btn_lotinfo_add.Visible = true;
                        one_time_assign_dgProduct_header = true;
                        btn_refresh_Click(sender, e);
                    }
                    helper.CloseConnection();

                    Cursor.Current = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnSearch_Click", ex);
            }
        }
        public bool search_inputcheck()
        {
            bool result = false;
            int search_lotno = 0;
            if (!string.IsNullOrEmpty(textSearchLotNo.Text))
            {
                search_lotno = Convert.ToInt32(textSearchLotNo.Text);
            }
            if (textSearchLotNo.Text.Trim() == "0000000" || search_lotno <= 0)
            {
                MessageBox.Show("Search Lot No. is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textSearchLotNo.Focus();
                result = true;
            }
            else if (!chkExclude.Checked && cmbProcess.SelectedIndex == -1)
            {
                    MessageBox.Show("Must Choose any one process..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbProcess.Focus();
                    result = true;              
            }
            return result;
        }
        public void lotinfo_value_assign_gridbind()
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                
                string ActionType = "pilotinfo";
                string[] str = { "@ActionType", "@lotno" };
                string[] obj = { ActionType, textSearchLotNo.Text };
                // lot information common data's 
                DataSet ds = helper.GetDatasetByCommandString("pi_lotinfo_fetch", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];
                    
                    string LotNoAdd = dtbl.Rows[0]["lot_no"].ToString();
                    if (LotNoAdd != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(LotNoAdd);
                        textLotNoAdd.Text = formate_type.ToString("D7");
                    }
                    string Lotnochild_formate_change = dtbl.Rows[0]["lot_no_child"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild.Text = formate_type.ToString("D2");
                    }
                    dateTimePicker_Manf.Text = dtbl.Rows[0]["manufacturing_date"].ToString();
                    txt_manf_time.Text = dtbl.Rows[0]["manufacturing_time"].ToString();
                    txt_lotinfo_itm_nam.Text = dtbl.Rows[0]["item_name"].ToString();
                    txt_lotinfo_quantity.Text = dtbl.Rows[0]["lotqty"].ToString();
                }
                helper.CloseConnection();
                // lot information grid data's
                // p1
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotno", "@proc_id", "@itmcd" };
                string[] obj_p1 = { ActionType_p1, textSearchLotNo.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };
                DataSet ds_pattern1 = helper.GetDatasetByCommandString("allpattern_view", str_p1, obj_p1);                
                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.processName = "TERMINAL BOARD INFO";
                c_model.pattern_type = "5";
                list_cmodel.Add(c_model);
                if (ds_pattern1.Tables[0].Rows.Count > 0)
                {              
                    foreach (DataRow dr in ds_pattern1.Tables[0].Rows)
                    {
                        c_model = new Lotinfo_gridbind_common_pattern();
                        c_model.pattern_type = dr["pattern_type"].ToString();                     
                        // lot no format change                        
                        string dG1joinlotno = dr["lotnojoin_p1"].ToString();
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
                        c_model.processId = dr["processId_p1"].ToString();
                        c_model.processName = dr["processName_p1"].ToString();
                        c_model.partno = dr["partno_p1"].ToString();
                        c_model.qty = dr["quantity_p1"].ToString();
                        c_model.plantingdate = dr["planting_p1"].ToString();
                        c_model.pb_date = dr["pb_dt_p1"].ToString();
                        c_model.tb_manuf_dt = dr["tb_manuf_dt_p1"].ToString();
                        c_model.tb_expairy_dt = dr["tb_expairy_dt_p1"].ToString();
                        c_model.tb_qty = dr["tb_qty_p1"].ToString();
                        list_cmodel.Add(c_model);
                    }
                }
                helper.CloseConnection();
                string ActionType_p2 = "p2view";
                string[] str_p2 = { "@ActionType", "@lotno", "@proc_id", "@itmcd" };
                string[] obj_p2 = { ActionType_p2, textSearchLotNo.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };
                DataSet ds_pattern2 = helper.GetDatasetByCommandString("allpattern_view", str_p2, obj_p2);
                if (ds_pattern2.Tables[0].Rows.Count > 0)
                {                    
                    foreach (DataRow dr in ds_pattern2.Tables[0].Rows)
                    {
                        c_model = new Lotinfo_gridbind_common_pattern();
                        c_model.pattern_type = dr["pattern_type"].ToString();
                        // lot no format change                        
                        string dG1joinlotno = dr["lotnojoin_p2"].ToString();
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
                        c_model.processId = dr["processId_p2"].ToString();
                        c_model.processName = dr["processName_p2"].ToString();
                        c_model.plantingdate = dr["process_date_p2"].ToString();
                        c_model.partno = dr["contorlno_p2"].ToString();
                        c_model.lotno = dr["slot_no_p2"].ToString();
                        c_model.qty = dr["quantity_p2"].ToString();
                        c_model.tb_manuf_dt = dr["tb_manuf_dt_p2"].ToString();
                        c_model.tb_expairy_dt = dr["tb_expairy_dt_p2"].ToString();
                        c_model.tb_qty = dr["tb_qty_p2"].ToString();
                        list_cmodel.Add(c_model);
                    }
                }
                helper.CloseConnection();
                string ActionType_p3 = "p3view";
                string[] str_p3 = { "@ActionType", "@lotno", "@proc_id", "@itmcd" };
                string[] obj_p3 = { ActionType_p3, textSearchLotNo.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };
                DataSet ds_pattern3 = helper.GetDatasetByCommandString("allpattern_view", str_p3, obj_p3);
                if (ds_pattern3.Tables[0].Rows.Count > 0)
                {      
                    foreach (DataRow dr in ds_pattern3.Tables[0].Rows)
                    {
                        c_model = new Lotinfo_gridbind_common_pattern();
                        c_model.pattern_type = dr["pattern_type"].ToString();
                        // lot no format change                        
                        string dG1joinlotno = dr["lotnojoin_p3"].ToString();
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
                        c_model.processId = dr["processId_p3"].ToString();
                        c_model.processName = dr["processName_p3"].ToString();
                        c_model.plantingdate = dr["process_date_p3"].ToString();
                        c_model.qty = dr["quantity_p3"].ToString();
                        c_model.tb_manuf_dt = dr["tb_manuf_dt_p3"].ToString();
                        c_model.tb_expairy_dt = dr["tb_expairy_dt_p3"].ToString();
                        c_model.tb_qty = dr["tb_qty_p3"].ToString();
                        list_cmodel.Add(c_model);
                    }

                }
                helper.CloseConnection();
                string ActionType_p4 = "p4view";
                string[] str_p4 = { "@ActionType", "@lotno", "@proc_id", "@itmcd" };
                string[] obj_p4 = { ActionType_p4, textSearchLotNo.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };
                DataSet ds_pattern4 = helper.GetDatasetByCommandString("allpattern_view", str_p4, obj_p4);
                if (ds_pattern4.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_pattern4.Tables[0].Rows)
                    {
                        c_model = new Lotinfo_gridbind_common_pattern();
                        c_model.pattern_type = dr["pattern_type"].ToString();
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
                        c_model.processId = dr["processId_p4"].ToString();
                        c_model.processName = dr["processName_p4"].ToString();
                        c_model.partno = dr["partno_p4"].ToString();
                        c_model.qty = dr["quantity_p4"].ToString();
                        c_model.tb_manuf_dt = dr["tb_manuf_dt_p4"].ToString();
                        c_model.tb_expairy_dt = dr["tb_expairy_dt_p4"].ToString();
                        c_model.tb_qty = dr["tb_qty_p4"].ToString();
                        list_cmodel.Add(c_model);
                    }
                }
                helper.CloseConnection();

                if (dataGridView1.Rows.Count == 0)
                {
                    foreach (var lotnu in list_cmodel)
                    {
                        if (lotnu.lotnojoin != null)
                        {
                            DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                            //// add  lot number  
                            First_row_dynamic_colm.CreateCells(this.dataGridView1);
                            First_row_dynamic_colm.HeaderCell.Value = lotnu.lotnojoin;
                            this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                        }

                    }
                }
                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);
                foreach (var item in list_cmodel)
                {
                    string patern_type = item.pattern_type;
                    foreach (var itm in CommonClass.Process_name)
                    {
                        string patern_type_list = itm.PaternType;
                        if (itm.ProcessNames == item.processName)
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
                        }
                    }
                    // List compare submited button name wise                  
                    if (cmbProcess.Text == item.processName)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                Compare_lotNo = row.HeaderCell.Value.ToString();
                                if (Compare_lotNo == item.lotnojoin)
                                {  
                                    row.Cells[0].Value = item.tb_qty;
                                    row.Cells[1].Value = item.tb_manuf_dt;
                                    row.Cells[2].Value = item.tb_expairy_dt;
                                    if (patern_type == "1")
                                    {
                                        row.Cells[columun_count_v].Value = item.partno;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.lotno;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.plantingdate;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.qty;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.pb_date;
                                        columun_count_v = 0;
                                    }
                                    else if (patern_type == "2")
                                    {
                                        row.Cells[columun_count_v].Value = item.plantingdate;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.partno;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.lotno;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.qty;
                                        columun_count_v = 0;
                                    }
                                    else if (patern_type == "3")
                                    {
                                        row.Cells[columun_count_v].Value = item.plantingdate;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.qty;
                                        columun_count_v = 0;
                                    }
                                    else if (patern_type == "4")
                                    {
                                        row.Cells[columun_count_v].Value = item.partno;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.lotno;
                                        columun_count_v = columun_count_v + 1;
                                        row.Cells[columun_count_v].Value = item.qty;
                                        columun_count_v = 0;
                                    }                                   
                                }
                            }
                        }
                    }
                    list_index++;
                }    
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("lotinfo_value_assign_gridbind", ex);
            }
        }
        public void insert_lotinfo_value_assign_gridbind(string custcd,string itmcd,string lotnumber)
        {
            try
            {       
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();                
                string ActionType = "pilotinfo";
                string[] str = { "@ActionType", "@lotno" };
                string[] obj = { ActionType, textLotNoAdd.Text };
                // lot information common data's 
                DataSet ds = helper.GetDatasetByCommandString("pi_lotinfo_fetch", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];
                    string LotNoAdd = dtbl.Rows[0]["lot_no"].ToString();
                    if (LotNoAdd != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(LotNoAdd);
                        textLotNoAdd.Text = formate_type.ToString("D7");
                    }
                    string Lotnochild_formate_change = dtbl.Rows[0]["lot_no_child"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild.Text = formate_type.ToString("D2");
                    }                    
                    dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                    txt_manf_time.Text = dtbl.Rows[0]["manufacturing_time"].ToString();            
                }
                helper.CloseConnection();
                // lot information grid data's
                // p1               
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p1 = { ActionType_p1, lotnumber, custcd, cmbProcess.SelectedValue.ToString(), itmcd };
                MySqlDataReader already_exist = helper.GetReaderByCmd("allpattern_view_itemcode", str_p1, obj_p1);
                List<Lotinfo_gridbind_common_pattern_new> m_model_p1 = LocalReportExtensions.GetList<Lotinfo_gridbind_common_pattern_new>(already_exist);
                Lotinfo_gridbind_common_pattern c_model = new Lotinfo_gridbind_common_pattern();
                c_model.processName = "TERMINAL BOARD INFO";
                c_model.pattern_type = "5";
                list_cmodel.Add(c_model);            
                // linq and model list                 
                if (m_model_p1.Count>0)
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
                            c_model.shipment_date = string.Empty;
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
                string[] str_p2 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p2 = { ActionType_p2, lotnumber, custcd, cmbProcess.SelectedValue.ToString(), itmcd };
                MySqlDataReader ds_pattern2 = helper.GetReaderByCmd("allpattern_view_itemcode", str_p2, obj_p2);
                List<Lotinfo_gridbind_p2> m_model_p2 = LocalReportExtensions.GetList<Lotinfo_gridbind_p2>(ds_pattern2);
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
                            c_model.shipment_date = string.Empty;
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
                string[] str_p3 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p3 = { ActionType_p3, lotnumber, custcd, cmbProcess.SelectedValue.ToString(), itmcd };
               

                MySqlDataReader ds_pattern3 = helper.GetReaderByCmd("allpattern_view_itemcode", str_p3, obj_p3);
                List<Lotinfo_gridbind_p3> m_model_p3 = LocalReportExtensions.GetList<Lotinfo_gridbind_p3>(ds_pattern3);
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
                string[] str_p4 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p4 = { ActionType_p4, lotnumber, custcd, cmbProcess.SelectedValue.ToString(), itmcd };
                MySqlDataReader ds_pattern4 = helper.GetReaderByCmd("allpattern_view_itemcode", str_p4, obj_p4);
                List<Lotinfo_gridbind_p4> m_model_p4 = LocalReportExtensions.GetList<Lotinfo_gridbind_p4>(ds_pattern4);
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
                            c_model.shipment_date = string.Empty;
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
                List<string> already_exits_row_header = new List<string>();
                List<string> already_exits_row_header_lotno_only = new List<string>();
                List<string> row_header_lotno_all_combined = new List<string>();
                List<string> already_exits_row_columns = new List<string>();
                if (dataGridView1.Rows.Count >= 0)
                {
                    if(list_cmodel.Count > 1)
                    {
                        // shipment date expiry date checking    
                        // LINQ shipment date expiry date checking
                        list_cmodel.ForEach(expiry_ship_dt =>
                        {
                            if (!string.IsNullOrEmpty(expiry_ship_dt.shipment_date))
                            {
                                var ship_date_ = expiry_ship_dt.shipment_date.Split(',');
                                ship_date_.ToList().ForEach(split_dt =>
                                {
                                    DateTime compare_date = DateTime.Parse(split_dt);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        already_exits_row_header.Add(expiry_ship_dt.lotnojoin);
                                        //continue;
                                    }
                                });
                            }
                            
                        }
                        );                        
                        // shipment date expiry date store the list
                        already_exits_row_columns.AddRange(already_exits_row_header);
                        int header_lot_index = 0;                       
                        // LINQ Grid row header data get in list
                        list_cmodel.ForEach(lotno => 
                        {
                            if (header_lot_index > 0 && !already_exits_row_header.Contains(lotno.lotnojoin))
                            {
                                if(lotno.lotnojoin == "1320715-12")
                                {

                                }
                                    row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                    already_exits_row_header.Add(lotno.lotnojoin);                           
                            }
                            header_lot_index++;
                        }
                        );                       
                        /////////////////////////////
                        // only lot number table refer              
                        string ActionType_only_lot = "onlylotview";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        //150823string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textSearchLotNo.Text };
                        string[] obj_only_lot = { ActionType_only_lot,custcd, itmcd, lotnumber };

                        //150823 DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);                       
                        DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view_witlot", str_only_lot, obj_only_lot);
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
                                string print_shipmentdate = dr["shipment_date"].ToString();
                                // shipment expiry date check
                                if (!string.IsNullOrEmpty(print_shipmentdate))
                                {
                                    DateTime compare_date = DateTime.Parse(print_shipmentdate);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {                                       
                                        already_exits_row_header_lotno_only.Add(dG1joinlotno);
                                        continue;
                                    }
                                }                         
                                already_exits_row_columns.AddRange(already_exits_row_header_lotno_only);
                                // header bind 
                                if (!already_exits_row_header.Contains(dG1joinlotno))
                                {
                                    if (dG1joinlotno == "1320715-12")
                                    {

                                    }
                                    row_header_lotno_all_combined.Add(dG1joinlotno);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }                            

                        }                      
                        ///9022022
                        ///grid row header bind                          
                        row_header_lotno_all_combined = row_header_lotno_all_combined.OrderBy(i => i).ToList();
                     
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;                                    
                        DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                        row_header_lotno_all_combined.ForEach(rowheader =>
                        {                          
                            First_row_dynamic_colm = new DataGridViewRow();                           
                            First_row_dynamic_colm.HeaderCell.Value = rowheader;
                            this.dataGridView1.Rows.Add(First_row_dynamic_colm);                    
                        });               
                        this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    }
                    else if (list_cmodel.Count == 1)
                    {
                        string ActionType_only_lot = "onlylotview";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        //150823string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textSearchLotNo.Text };
                        string[] obj_only_lot = { ActionType_only_lot, custcd, itmcd, lotnumber };
                        //150823DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);
                        DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view_witlot", str_only_lot, obj_only_lot);
                        int count_header = ds_only_lot.Tables[0].Rows.Count;
                        Console.WriteLine("Row :" + count_header);
                        if (ds_only_lot.Tables[0].Rows.Count > 0)
                        {
                             DataView view =  ds_only_lot.Tables[0].DefaultView;
                             view.Sort = "lotno,lot_no_child ASC";
                            foreach (DataRow dr in ds_only_lot.Tables[0].Rows)
                            {                                
                               string lotno_join = dr["lotnoJoin"].ToString();
                                string dG1joinlotno = lotno_join;
                                Console.WriteLine("Row lotnojoin :" + dG1joinlotno);
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
                                string print_shipmentdate = dr["shipment_date"].ToString();
                                //// shipment expiry date check
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
                                if (!already_exits_row_header.Contains(dG1joinlotno))
                                {
                                    DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                    //// add  lot number  
                                    First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                    First_row_dynamic_colm.HeaderCell.Value = dG1joinlotno;
                                    this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }
                            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        }
                    }
                }
              
                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);         
                if (list_cmodel.Count > 1)
                {
                    list_cmodel.ForEach(items =>
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = items.processName.Split(',');

                            int chk_index = 0;
                            
                            if (!already_exits_row_columns.Contains(items.lotnojoin))
                            {
                                if(items.lotnojoin== "1320715-01")
                                {

                                }
                                split_process_name.ToList().ForEach(chk =>
                                {
                                    string patern_type = items.pattern_type;                                   
                                    foreach (var itm in CommonClass.Process_name_gridbind_columns)
                                    {
                                        string patern_type_list = itm.PaternType;
                                        if (itm.ProcessNames == chk && itm.materialcode == items.material_code.Split(',')[chk_index])
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
                                            // TERMINAL BOARD INFO 
                                            else if (patern_type_list == "5")
                                            {
                                                columun_count_v = columun_count_v + 9;
                                            }
                                        }

                                    }
                                    // List compare submited button name wise
                                    int dataGridview1_row_index = 1;
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {                                  
      
                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();
                                            if (Compare_lotNo == items.lotnojoin)
                                            {
                                                if (chk_index < items.tb_bproduct.Length)
                                                {
                                                    row.Cells[0].Value = items.tb_bproduct.Split(',')[chk_index];
                                                }
                                                if (chk_index < items.onhold.Length)
                                                {
                                                    if (!string.IsNullOrEmpty(items.onhold.Split(',')[chk_index]))
                                                    {
                                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                                    }
                                                    row.Cells[1].Value = items.onhold.Split(',')[chk_index];
                                                }
                                                if (chk_index < items.scrap.Length)
                                                {
                                                    if (!string.IsNullOrEmpty(items.scrap.Split(',')[chk_index]))
                                                    {
                                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                    }
                                                    row.Cells[2].Value = items.scrap.Split(',')[chk_index];
                                                }
                                                if (chk_index < items.reason_hs.Length)
                                                {
                                                    row.Cells[3].Value = items.reason_hs.Split(',')[chk_index];
                                                }
                                                row.Cells[4].Value = items.tb_qty.Split(',')[chk_index];
                                                DateTime manuf_dt = Convert.ToDateTime(items.tb_manuf_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);                                                
                                                row.Cells[5].Value = manuf_dt;
                                                // compare to current date
                                                DateTime from_dt = Convert.ToDateTime(items.tb_expairy_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                DateTime to_dt = DateTime.Now;
                                                int result = DateTime.Compare(from_dt, to_dt);
                                                if (result >= 1)
                                                {
                                                    row.Cells[6].Value = items.tb_expairy_dt.Split(',')[chk_index];
                                                }
                                                else
                                                {
                                                    row.Cells[6].Value = items.tb_expairy_dt.Split(',')[chk_index];
                                                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                }                                           
                                                string manf_dte = manuf_dt.ToString("yyyyMMdd"); 
                                             
                                                row.Cells[7].Value = manf_dte + items.lotno +items.lotnojoin.Split('-')[1];
                                                row.Cells[8].Value = items.lotnojoin.Split('-')[1];
                                                if (patern_type == "1")
                                                {
                                                    row.Cells[columun_count_v].Value = items.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.lotno_p1.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.plantingdate.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.qty.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.pb_date.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }
                                                else if (patern_type == "2")
                                                {
                                                    row.Cells[columun_count_v].Value = items.plantingdate.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.sheetlotno_p2.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }
                                                else if (patern_type == "3")
                                                {
                                                    if(Convert.ToString(columun_count_v) =="0")
                                                    {

                                                    }
                                                    row.Cells[columun_count_v].Value = items.plantingdate.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }
                                                else if (patern_type == "4")
                                                {
                                                    row.Cells[columun_count_v].Value = items.partno.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.lotno_p4.Split(',')[chk_index];
                                                    columun_count_v = columun_count_v + 1;
                                                    row.Cells[columun_count_v].Value = items.qty.Split(',')[chk_index];
                                                    columun_count_v = 0;
                                                }

                                            }
                                            dataGridview1_row_index++;
                                        }
                                    }                                 
                                    chk_index++;
                                });
                            }
                        }
                        list_index++;

                    });
                    // 2nd time loop. skip existing
                    already_exits_row_columns.AddRange(already_exits_row_header);
                    //lot_number_only_row_common("onlylotview"); 
                    lot_number_only_row_common("onlylotview_lotno",lotnumber);


                }
                else if (list_cmodel.Count == 1)
                {
                    // 2nd time loop. skip existing
                    already_exits_row_columns.AddRange(already_exits_row_header);
                    //lot_number_only_row_common("onlylotview");
                    lot_number_only_row_common("onlylotview_lotno", lotnumber);

                }                                           
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("insert_lotinfo_value_assign_gridbind", ex);
            }
        }
        public void view_lotinfo_value_assign_gridbind_without_process()
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();            
                string ActionType = "pilotinfo";
                string[] str = { "@ActionType", "@lotno" };
                string[] obj = { ActionType, textSearchLotNo.Text };
                // lot information common data's 
                DataSet ds = helper.GetDatasetByCommandString("pi_lotinfo_fetch", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];  
                    string LotNoAdd = dtbl.Rows[0]["lot_no"].ToString();
                    if (LotNoAdd != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(LotNoAdd);
                        textLotNoAdd.Text = formate_type.ToString("D7");
                    }
                    string Lotnochild_formate_change = dtbl.Rows[0]["lot_no_child"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild.Text = formate_type.ToString("D2");
                    }
                    dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                    txt_manf_time.Text = dtbl.Rows[0]["manufacturing_time"].ToString();
                }
                helper.CloseConnection();
                // lot information grid data's
                // p1
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p1 = { ActionType_p1, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };                              
                MySqlDataReader already_exist = helper.GetReaderByCmd("allpattern_view_itemcode_lotno", str_p1, obj_p1);
                List<Lotinfo_gridbind_common_pattern_new> m_model_p1 = LocalReportExtensions.GetList<Lotinfo_gridbind_common_pattern_new>(already_exist);               
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
                            c_model.shipment_date = string.Empty;
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
                string[] str_p2 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p2 = { ActionType_p2, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };                               
                MySqlDataReader ds_pattern2 = helper.GetReaderByCmd("allpattern_view_itemcode_lotno", str_p2, obj_p2);
                List<Lotinfo_gridbind_p2> m_model_p2 = LocalReportExtensions.GetList<Lotinfo_gridbind_p2>(ds_pattern2);
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
                            c_model.shipment_date = string.Empty;
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
                string[] str_p3 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p3 = { ActionType_p3, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };                                          
                MySqlDataReader ds_pattern3 = helper.GetReaderByCmd("allpattern_view_itemcode_lotno", str_p3, obj_p3);
                List<Lotinfo_gridbind_p3> m_model_p3 = LocalReportExtensions.GetList<Lotinfo_gridbind_p3>(ds_pattern3);
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
                string[] str_p4 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p4 = { ActionType_p4, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };                                              
                MySqlDataReader ds_pattern4 = helper.GetReaderByCmd("allpattern_view_itemcode_lotno", str_p4, obj_p4);
                List<Lotinfo_gridbind_p4> m_model_p4 = LocalReportExtensions.GetList<Lotinfo_gridbind_p4>(ds_pattern4);
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
                            c_model.shipment_date = string.Empty;
                            c_model.tb_bproduct = dr.bproduct_p4.ToString();
                            c_model.onhold = dr.onhold_p4.ToString();
                            c_model.scrap = dr.scrap_p4.ToString();
                            c_model.reason_hs = dr.reason_hs_p4.ToString();
                            dr.lotno_p4.ToString();
                            list_cmodel.Add(c_model);
                        });
                    });
                }
                helper.CloseConnection();
                list_cmodel = list_cmodel.OrderBy(o => o.lotnojoin).ToList();
                // shipment date check after 2month means not show
                List<string> already_exits_row_header = new List<string>();
                List<string> already_exits_row_header_lotno_only = new List<string>();
                List<string> row_header_lotno_all_combined = new List<string>();
                List<string> already_exits_row_columns = new List<string>();
                if (dataGridView1.Rows.Count == 0)                
                {
                    if (list_cmodel.Count > 1)
                    {
                        // shipment expiry date check
                        // LINQ shipment date expiry date checking
                        list_cmodel.ForEach(expiry_ship_dt =>
                        {
                            if (!string.IsNullOrEmpty(expiry_ship_dt.shipment_date))
                            {
                                string[] ship_date = expiry_ship_dt.shipment_date.Split(',');
                                ship_date.ToList().ForEach(split_dt =>
                                {
                                    DateTime compare_date = DateTime.Parse(split_dt);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        already_exits_row_header.Add(expiry_ship_dt.lotnojoin);
                                        //continue;
                                    }
                                });

                            }
                        });
                        already_exits_row_columns.AddRange(already_exits_row_header);
                        // check process completed or not 
                        if (chk_exclude_data_process.Checked)
                        {
                            int process_check_index = 0;
                            list_cmodel.ForEach(lotno =>
                            {
                                if (process_check_index > 0 && !already_exits_row_header.Contains(lotno.lotnojoin))
                                {
                                        string[] lot_numbers = lotno.lotnojoin.Split(',');
                                        lot_numbers.ToList().ForEach(check_lotnu =>
                                        {
                                            string lot_number_prt = lotno.lotnojoin.Split('-')[0].ToString();
                                            string lot_number_child = lotno.lotnojoin.Split('-')[1].ToString();
                                            bool get_result = all_process_completed_check(lot_number_prt, lot_number_child);
                                            if (!get_result)
                                            {
                                                already_exits_row_header.Add(lotno.lotnojoin);
                                            }
                                        });                              
                                }
                                process_check_index++;
                            });
                            already_exits_row_columns.AddRange(already_exits_row_header);

                        }
                        int header_lot_index = 0;
                        // Grid row header 
                        list_cmodel.ForEach(lotno =>
                        {
                            if (header_lot_index > 0 && !already_exits_row_header.Contains(lotno.lotnojoin))
                            {
                                    row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                    already_exits_row_header.Add(lotno.lotnojoin);
                            }
                            header_lot_index++;
                        });
                        
                        /////////////////////////////
                        // only lot number table refer 
                        string ActionType_only_lot = "onlylotview_lotno";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text,textSearchLotNo.Text };
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
                                string print_shipmentdate = dr["shipment_date"].ToString();
                                // shipment expiry date check
                                if (!string.IsNullOrEmpty(print_shipmentdate))
                                {
                                    DateTime compare_date = DateTime.Parse(print_shipmentdate);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        already_exits_row_header_lotno_only.Add(dG1joinlotno);
                                        continue;
                                    }
                                }                               
                                already_exits_row_columns.AddRange(already_exits_row_header_lotno_only);
                                // header bind 
                                if (!already_exits_row_header.Contains(dG1joinlotno))
                                {
                                    row_header_lotno_all_combined.Add(dG1joinlotno);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }
                        }
                        ///9022022
                        ///grid row header bind                                             
                        row_header_lotno_all_combined = row_header_lotno_all_combined.OrderBy(i => i).ToList();
                        row_header_lotno_all_combined.ForEach(rowheader =>
                        {
                            DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                            //// add  lot number  
                            First_row_dynamic_colm.CreateCells(this.dataGridView1);
                            First_row_dynamic_colm.HeaderCell.Value = rowheader;
                            this.dataGridView1.Rows.Add(First_row_dynamic_colm);

                        });
                        this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    }
                    else if (list_cmodel.Count == 1)
                    {
                        string ActionType_only_lot = string.Empty;
                        if (!CommonClass.view_enable)
                        {
                            ActionType_only_lot = "onlylotview";
                        }
                        else if (CommonClass.view_enable)
                        {
                            ActionType_only_lot = "onlylotview_lotno";
                        }
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text,textSearchLotNo.Text };

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
                                if (!already_exits_row_header.Contains(dG1joinlotno))
                                {
                                    DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                    //// add  lot number  
                                    First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                    First_row_dynamic_colm.HeaderCell.Value = dG1joinlotno;
                                    this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }
                            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                        }
                        else
                        {
                            MessageBox.Show("Lot Information Tab No Records found..", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }

                }

                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);
                if (list_cmodel.Count > 1)
                {
                    list_cmodel.ForEach(item =>
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = item.processName.Split(',');

                            int chk_index = 0;                            
                            if (!already_exits_row_columns.Contains(item.lotnojoin))
                            {
                                split_process_name.ToList().ForEach(chk =>
                                {
                                    string patern_type = item.pattern_type;
                                    foreach (var itm in CommonClass.Process_name_gridbind_columns)
                                    {
                                        if(item.lotnojoin == "1530327-03")
                                        {

                                        }
                                        Console.WriteLine("last lot number : "+item.lotnojoin);
                                        string patern_type_list = itm.PaternType;
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
                                            // TERMINAL BOARD INFO 
                                            else if (patern_type_list == "5")
                                            {
                                                columun_count_v = columun_count_v + 9;
                                            }
                                        }

                                    }
                                    // List compare submited button name wise
                                    int dataGridview1_row_index = 1;
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();
                                            if (Compare_lotNo == item.lotnojoin)
                                            {                                               
                                                if (chk_index < item.tb_bproduct.Length)
                                                {
                                                    row.Cells[0].Value = item.tb_bproduct.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.tb_bproduct.Length)
                                                {
                                                    row.Cells[0].Value = item.tb_bproduct.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.onhold.Length)
                                                {
                                                    if (!string.IsNullOrEmpty(item.onhold.Split(',')[chk_index]))
                                                    {
                                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                                    }
                                                    row.Cells[1].Value = item.onhold.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.scrap.Length)
                                                {
                                                    if (!string.IsNullOrEmpty(item.scrap.Split(',')[chk_index]))
                                                    {
                                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                    }
                                                    row.Cells[2].Value = item.scrap.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.reason_hs.Length)
                                                {
                                                    row.Cells[3].Value = item.reason_hs.Split(',')[chk_index];
                                                }
                                                row.Cells[4].Value = item.tb_qty.Split(',')[chk_index];
                                                DateTime manuf_dt = Convert.ToDateTime(item.tb_manuf_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                row.Cells[5].Value = manuf_dt;
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
                                                string manf_dte = manuf_dt.ToString("yyyyMMdd");
                                                row.Cells[7].Value = manf_dte + item.lotno + item.lotnojoin.Split('-')[1];                                              
                                                row.Cells[8].Value = manf_dte + item.lotno + item.lotnojoin.Split('-')[1];
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
                                });
                            }

                        }
                        list_index++;

                    });
                    lot_number_only_row_common("onlylotview_lotno", textSearchLotNo.Text);
                }
                else if (list_cmodel.Count == 1)
                {
                    lot_number_only_row_common("onlylotview_lotno", textSearchLotNo.Text);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("view_lotinfo_value_assign_gridbind_without_process", ex);
            }
        }
        public void view_lotinfo_value_assign_gridbind()
        {
            try
            {
                List<Lotinfo_gridbind_common_pattern> list_cmodel = new List<Lotinfo_gridbind_common_pattern>();
                

                string ActionType = "pilotinfo";
                string[] str = { "@ActionType", "@lotno" };
                string[] obj = { ActionType, textSearchLotNo.Text };
                // lot information common data's 
                DataSet ds = helper.GetDatasetByCommandString("pi_lotinfo_fetch", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtbl = ds.Tables[0];           
                    string LotNoAdd = dtbl.Rows[0]["lot_no"].ToString();
                    if (LotNoAdd != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(LotNoAdd);
                        textLotNoAdd.Text = formate_type.ToString("D7");
                    }
                    string Lotnochild_formate_change = dtbl.Rows[0]["lot_no_child"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild.Text = formate_type.ToString("D2");
                    }                   
                    dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                    txt_manf_time.Text = dtbl.Rows[0]["manufacturing_time"].ToString();              
                }
                helper.CloseConnection();
                // lot information grid data's
                // p1                
                string Compare_lotNo = "";
                int list_index = 0;
                string ActionType_p1 = "p1view";
                string[] str_p1 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };

                string[] obj_p1 = { ActionType_p1, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };
                MySqlDataReader already_exist = helper.GetReaderByCmd("allpattern_view_wout_itemcode", str_p1, obj_p1);
                List<Lotinfo_gridbind_common_pattern_new> m_model_p1 = LocalReportExtensions.GetList<Lotinfo_gridbind_common_pattern_new>(already_exist);                
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
                            c_model.shipment_date = string.Empty;
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
                string[] str_p2 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };

                string[] obj_p2 = { ActionType_p2, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };                
                MySqlDataReader ds_pattern2 = helper.GetReaderByCmd("allpattern_view_wout_itemcode", str_p2, obj_p2);
                List<Lotinfo_gridbind_p2> m_model_p2 = LocalReportExtensions.GetList<Lotinfo_gridbind_p2>(ds_pattern2);

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
                            c_model.shipment_date = string.Empty;
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
                string[] str_p3 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p3 = { ActionType_p3, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };               
                MySqlDataReader ds_pattern3 = helper.GetReaderByCmd("allpattern_view_wout_itemcode", str_p3, obj_p3);
                List<Lotinfo_gridbind_p3> m_model_p3 = LocalReportExtensions.GetList<Lotinfo_gridbind_p3>(ds_pattern3);

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
                string[] str_p4 = { "@ActionType", "@lotnumber", "@Customercd", "@proc_id", "@itmcd" };
                string[] obj_p4 = { ActionType_p4, textSearchLotNo.Text, txtCustomerCode.Text, cmbProcess.SelectedValue.ToString(), txt_lotinfo_itemcode.Text };
              
                MySqlDataReader ds_pattern4 = helper.GetReaderByCmd("allpattern_view_wout_itemcode", str_p4, obj_p4);
                List<Lotinfo_gridbind_p4> m_model_p4 = LocalReportExtensions.GetList<Lotinfo_gridbind_p4>(ds_pattern4);

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
                            c_model.shipment_date = string.Empty;
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
                List<string> already_exits_row_header = new List<string>();
                List<string> already_exits_row_header_lotno_only = new List<string>();
                List<string> row_header_lotno_all_combined = new List<string>();
                List<string> already_exits_row_columns = new List<string>();
                if (dataGridView1.Rows.Count == 0)
                //if (dataGridView1.Rows.Count >= 0)
                {
                    if (list_cmodel.Count > 1)
                    {
                        list_cmodel.ToList().ForEach(expiry_ship_dt =>
                        {
                            if (!string.IsNullOrEmpty(expiry_ship_dt.shipment_date))
                            {
                                string[] ship_date = expiry_ship_dt.shipment_date.Split(',');
                                foreach (var split_dt in ship_date)
                                {
                                    DateTime compare_date = DateTime.Parse(split_dt);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        already_exits_row_header.Add(expiry_ship_dt.lotnojoin);
                                        continue;
                                    }
                                }

                            }
                        });
                        already_exits_row_columns.AddRange(already_exits_row_header);
                        int header_lot_index = 0;
                        // check process completed or not 
                        if (chk_exclude_data_process.Checked)
                        {
                            int process_check_index = 0;
                            list_cmodel.ForEach(lotno =>
                            {
                                if (process_check_index > 0 && !already_exits_row_header.Contains(lotno.lotnojoin))
                                {
                                   
                                        string[] lot_numbers = lotno.lotnojoin.Split(',');
                                        foreach (var check_lotnu in lot_numbers)
                                        {
                                            string lot_number_prt = lotno.lotnojoin.Split('-')[0].ToString();
                                            string lot_number_child = lotno.lotnojoin.Split('-')[1].ToString();

                                            bool get_result = all_process_completed_check(lot_number_prt, lot_number_child);
                                            if (!get_result)
                                            {
                                                already_exits_row_header.Add(lotno.lotnojoin);
                                            }
                                        }
                                    
                                }
                                process_check_index++;
                            });
                            already_exits_row_columns.AddRange(already_exits_row_header);

                        }
                        // check process id exist or not 
                        if (!chkExclude.Checked)
                        {
                            int process_check_index = 0;
                            list_cmodel.ForEach(lotno =>
                            {
                                if (process_check_index > 0 && !already_exits_row_header.Contains(lotno.lotnojoin))
                                {
                                                                       
                                        string lot_number_prt = lotno.lotnojoin.Split('-')[0].ToString();
                                        string lot_number_child = lotno.lotnojoin.Split('-')[1].ToString();                                     
                                        bool get_result_process_id_isnull = process_id_exist_check(lot_number_prt, lot_number_child, cmbProcess.SelectedValue.ToString(), Selected_patternTyp);

                                        if (get_result_process_id_isnull)
                                        {                                            
                                            already_exits_row_header.Add(lotno.lotnojoin);
                                        }                                      
                                  
                                }
                                process_check_index++;
                            });
                            already_exits_row_columns.AddRange(already_exits_row_header);

                        }
                        // Grid row header 
                        list_cmodel.ForEach(lotno =>
                        {
                            if (header_lot_index > 0 && !already_exits_row_header.Contains(lotno.lotnojoin))
                            {
                                    row_header_lotno_all_combined.Add(lotno.lotnojoin);
                                    already_exits_row_header.Add(lotno.lotnojoin);                                
                            }
                            header_lot_index++;
                        });

                        /////////////////////////////
                        // only lot number table refer 
                        string ActionType_only_lot = "onlylotview_lotno";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textSearchLotNo.Text };

                        DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view_witlot", str_only_lot, obj_only_lot);
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
                                string print_shipmentdate = dr["shipment_date"].ToString();
                                // shipment expiry date check
                                if (!string.IsNullOrEmpty(print_shipmentdate))
                                {
                                    DateTime compare_date = DateTime.Parse(print_shipmentdate);
                                    DateTime Result = compare_date.AddMonths(+2);
                                    int grater_than = DateTime.Compare(Result, nowdate);
                                    if (grater_than <= 0)
                                    {
                                        already_exits_row_header_lotno_only.Add(dG1joinlotno);
                                        continue;
                                    }
                                }
                                already_exits_row_columns.AddRange(already_exits_row_header_lotno_only);
                                // header bind 
                                if (!already_exits_row_header.Contains(dG1joinlotno))
                                {                                   
                                    row_header_lotno_all_combined.Add(dG1joinlotno);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }

                        }                                               
                        row_header_lotno_all_combined = row_header_lotno_all_combined.OrderBy(i => i).ToList();
                        row_header_lotno_all_combined.ForEach(rowheader =>
                        {
                            DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                            //// add  lot number  
                            First_row_dynamic_colm.CreateCells(this.dataGridView1);
                            First_row_dynamic_colm.HeaderCell.Value = rowheader;
                            this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                        });
                        this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                    }
                    else if (list_cmodel.Count == 1)
                    {
                        string ActionType_only_lot = "onlylotview_lotno";
                        string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                        string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, textSearchLotNo.Text };

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
                                if (!already_exits_row_header.Contains(dG1joinlotno))
                                {
                                    DataGridViewRow First_row_dynamic_colm = new DataGridViewRow();
                                    //// add  lot number  
                                    First_row_dynamic_colm.CreateCells(this.dataGridView1);
                                    First_row_dynamic_colm.HeaderCell.Value = dG1joinlotno;

                                    this.dataGridView1.Rows.Add(First_row_dynamic_colm);
                                    already_exits_row_header.Add(dG1joinlotno);
                                }
                            }
                            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        }
                    }
                }
                int columun_count_v = 0;
                lotview_list_cmodel_grid = new List<Lotinfo_gridbind_common_pattern>();
                lotview_list_cmodel_grid.AddRange(list_cmodel);   
                if (list_cmodel.Count > 1)
                {
                    list_cmodel.ForEach(item =>
                    {
                        if (list_index != 0)
                        {
                            string[] split_process_name = item.processName.Split(',');

                            int chk_index = 0;
                            if (!already_exits_row_columns.Contains(item.lotnojoin))
                            {
                                split_process_name.ToList().ForEach(chk =>
                                {
                                    string patern_type = item.pattern_type;

                                    //foreach (var itm in CommonClass.Process_name)
                                    foreach (var itm in CommonClass.Process_name_gridbind_columns)
                                    {
                                        string patern_type_list = itm.PaternType;
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
                                            // TERMINAL BOARD INFO 
                                            else if (patern_type_list == "5")
                                            {
                                                columun_count_v = columun_count_v + 9;
                                            }
                                        }

                                    }
                                    // List compare submited button name wise
                              
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        if (!row.IsNewRow)
                                        {
                                            Compare_lotNo = row.HeaderCell.Value.ToString();
                                            if (Compare_lotNo == item.lotnojoin)
                                            {    
                                               
                                                if (chk_index < item.tb_bproduct.Length)
                                                {
                                                    row.Cells[0].Value = item.tb_bproduct.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.onhold.Length)
                                                {
                                                    if (!string.IsNullOrEmpty(item.onhold.Split(',')[chk_index]))
                                                    {
                                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.PeachPuff;
                                                    }
                                                    row.Cells[1].Value = item.onhold.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.scrap.Length)
                                                {
                                                    if (!string.IsNullOrEmpty(item.scrap.Split(',')[chk_index]))
                                                    {
                                                        dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                                                    }
                                                    row.Cells[2].Value = item.scrap.Split(',')[chk_index];
                                                }
                                                if (chk_index < item.reason_hs.Length)
                                                {
                                                    row.Cells[3].Value = item.reason_hs.Split(',')[chk_index];
                                                }
                                                row.Cells[4].Value = item.tb_qty.Split(',')[chk_index];
                                                DateTime manuf_dt = Convert.ToDateTime(item.tb_manuf_dt.Split(',')[chk_index],
                                                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);                                                
                                                row.Cells[5].Value = manuf_dt;
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
                                                string manf_dte = manuf_dt.ToString("yyyyMMdd");

                                                row.Cells[7].Value = manf_dte + item.lotno + item.lotnojoin.Split('-')[1];
                                                row.Cells[8].Value = manf_dte + item.lotno + item.lotnojoin.Split('-')[1];
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
                                        }



                                    }

                                    chk_index++;
                                });
                            }
                        }
                        list_index++;

                    });                   
                    lot_number_only_row_common("onlylotview_lotno", textSearchLotNo.Text);
                }
                else if (list_cmodel.Count == 1)
                {                   
                    lot_number_only_row_common("onlylotview_lotno", textSearchLotNo.Text);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("view_lotinfo_value_assign_gridbind", ex);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            try
            {
                txt_print_person_name.Text = "Name";
                txt_print_person_name.ForeColor = Color.Gray;
                txt_reason_hs.Text = "Remarks";
                btnPrintlblSelectall.Text = "Select All";
                CommonClass.PI_insert_data = new List<PI_master_use_insert>();
                lotnumber_changed_add_pi_tbl = false;
                lotnumber_only_changed_add_pi_tbl = false;
                view_time_lotno_changed = false;
                refresh_btn_click = false;
                CommonClass.view_enable = false;
                CommonClass.lot_info_changes = false;
                Cursor.Current = Cursors.WaitCursor;
                dt = new DataTable();
                //140722 dGProduct.DataSource = null;
                dGProduct.DataSource = dt;
                dGProduct.DataSource = null;               
                daysInMonths = new int[] { };
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                resetInput();
                resetInputLotInfoTab();
                CommonClass.Process_name = new List<PI_Process>();
                CommonClass.Process_name_gridbind = new List<PI_Process>();

                dataGridView2.DataSource = null;
                dataGridView2.Text.DefaultIfEmpty();
                dataGridView2.Refresh();
                resetInputPrintLabelTab();
                // 300523
                truncate_pattern_temp();
                DataTable dt_new = new DataTable();
                dataGridView2.DataSource = dt_new;

                textSearchLotNo.Text = "0000000";
                //add button visiable true 
                btn_lotinfo_add.Visible = true;
                //save button visiable false 
                btn_lotinfo_save.Visible = false;
                refresh_btn_click = true;
                lot_information_changed_without_grid = false;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_refresh_Click", ex);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[rowIndex];
            // use pattern popup open
            dataGridView1_grid_selectedRow = rowIndex;
            string lotsplit = dataGridView1.CurrentRow.HeaderCell.Value.ToString();
            textLotNoAdd.Text = lotsplit.Split('-')[0];
            textLotNoChild.Text = lotsplit.Split('-')[1];
            /// print label 
            txt_pl_lotno.Text = lotsplit.Split('-')[0];
            txt_pl_frm_lotc.Text = lotsplit.Split('-')[1];
            ///
            dateTimePicker_Manf.Value = Convert.ToDateTime(row.Cells[1].Value.ToString(),
            System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            txt_lotinfo_quantity.Text = row.Cells[0].Value.ToString();
            color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
            //datagrid view 2 
            DataTable dt = new DataTable();
            dataGridView2.DataSource = dt;
            Cursor.Current = Cursors.Default;
        }

        private void btn_lotinfo_save_Click(object sender, EventArgs e)
        {
            try
            {
                // any changes means its go to if 
                if (CommonClass.lot_info_changes)
                {
                    if (textSearchLotNo.Text != "0000000")
                    {
                        DialogResult dialogResult = MessageBox.Show("Do you want to Save LotInformation ?", "SAVE LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            string ActionType_exist = "all";
                            int update_row_count = 0;
                            //// product information data insert 
                            //product_inforamtion_insert();  
                            string ActionType = "master";
                            string ActionType_PI = "productinfo";
                            string ActionType_pattern_p1 = string.Empty;
                            string ActionType_pattern_p2 = string.Empty;
                            string ActionType_pattern_p3 = string.Empty;
                            string ActionType_pattern_p4 = string.Empty;
                            if (CommonClass.p1 == true)
                            {
                                ActionType_pattern_p1 = "p1";
                            }
                            if (CommonClass.p2 == true)
                            {
                                ActionType_pattern_p2 = "p2";
                            }
                            if (CommonClass.p3 == true)
                            {
                                ActionType_pattern_p3 = "p3";
                            }
                            if (CommonClass.p4 == true)
                            {
                                ActionType_pattern_p4 = "p4";
                            }
                            DateTime current_date_time = DateTime.Now;
                            string[] str_exist = { "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@manftime", "@stus", "@created_at", "@ActionType", "@ActionType_p1", "@ActionType_p2", "@ActionType_p3", "@ActionType_p4", "@commonId" };
                            string[] obj_exist = { textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Text, txt_manf_time.Text, "1", current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType, ActionType_pattern_p1, ActionType_pattern_p2, ActionType_pattern_p3, ActionType_pattern_p4, CommonClass.pattern_temp_random_number };
                            MySqlDataReader all_patern = helper.GetReaderByCmd("allpatern_insert_main_new", str_exist, obj_exist);
                            if (all_patern.Read())
                            {
                                all_patern.Close();
                                helper.CloseConnection();
                                //
                                string[] str_upt = { "@lno", "@lcno", "@ActionType", "@ActionType_p1", "@ActionType_p2", "@ActionType_p3", "@ActionType_p4", "@commonId" };
                                string[] obj_up = { textLotNoAdd.Text, textLotNoChild.Text, ActionType, ActionType_pattern_p1, ActionType_pattern_p2, ActionType_pattern_p3, ActionType_pattern_p4, CommonClass.pattern_temp_random_number };
                                MySqlDataReader allpatter_upt = helper.GetReaderByCmd("allpattern_update_new", str_upt, obj_up);
                                if (allpatter_upt.Read())
                                {

                                }
                                allpatter_upt.Close();
                                helper.CloseConnection();
                                if (lot_information_changed_without_grid)
                                {
                                    string exp_date = dateTimePicker_Manf.Value.ToShortDateString();
                                    DateTime oDate = Convert.ToDateTime(exp_date);
                                    DateTime nextYear = oDate.AddYears(+1);
                                    exp_date = nextYear.ToString("yyyy-MM-dd");
                                    string ActionType_upt = "all";
                                    string Bproduct = null;
                                    if (chk_bproduct.Checked)
                                    {
                                        Bproduct = "B";
                                    }
                                    string Onhold = null;
                                    if (chk_onhold.Checked)
                                    {
                                        Onhold= "H";
                                    }
                                    string scrap = null;
                                    if (chkbx_scrap.Checked)
                                    {
                                        scrap = "S";
                                        Onhold = null;
                                    }
                                    string reason = null;
                                    if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                                    {
                                        reason = txt_reason_hs.Text;
                                    }                                    
                                    string[] str_updlotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@expirydate", "@manftime", "@bpro", "@updatedat", "@ActionType","@hld","@uid","@scrp","@reason" };
                                    string[] obj_updlotinfo = { txtCustomerCode.Text, textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text, Bproduct, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType_upt,Onhold,CommonClass.logged_Id, scrap,reason };
                                    MySqlDataReader all_patern_upd = helper.GetReaderByCmd("allpatern_update_lotinfo_only", str_updlotinfo, obj_updlotinfo);
                                    if (all_patern_upd.Read())
                                    {
                                    }
                                    all_patern_upd.Close();
                                    helper.CloseConnection();
                                    lot_information_changed_without_grid = false;
                                                                       
                                }                               
                                CommonClass.p1 = false;
                                CommonClass.p2 = false;
                                CommonClass.p3 = false;
                                CommonClass.p4 = false;
                                CommonClass.lot_info_changes = false;
                                // product information data insert 
                                product_inforamtion_insert();                                
                                max_lotno_id();
                                MessageBox.Show("Lot Information Save successfully..", "UPDATE Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Information);                                
                                view_time_lotno_changed = true;
                                dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, dgProduct_grid_selectedRow));
                                dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                                DateTime cnowdate = DateTime.Now;
                                txt_manf_time.Text = cnowdate.ToString("HH:mm:ss");

                                chkbx_scrap.Checked = false;
                                chk_onhold.Checked = false;
                                chk_bproduct.Checked = false;
                                txt_reason_hs.Text = "Remarks";
                                txt_reason_hs.ForeColor = Color.Gray;
                                Cursor.Current = Cursors.Default;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lot No. is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textSearchLotNo.Focus();
                    }
                }
                else
                {                   
                    MessageBox.Show("No Changes Right now..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textSearchLotNo.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_lotinfo_save_Click", ex);
            }
        }

        private void btn_lotinfo_del_Click(object sender, EventArgs e)
        {
            try
            {
                if (textLotNoAdd.Text != "" && textLotNoAdd.Text != "0000000" && textLotNoChild.Text != "" && textLotNoChild.Text != "00")
                {
                    if(Lotno_LotnoChild_already_exist(textLotNoAdd.Text,textLotNoChild.Text, "chk_lotno"))
                    {
                        DialogResult dialogResult = MessageBox.Show("Do you want to Delete LotInformation " + textLotNoAdd.Text + "-" + textLotNoChild.Text + " ?", "DELETE LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            string[] str_del = { "@lotn", "@lotn_chld" };
                            string[] obj_del = { textLotNoAdd.Text, textLotNoChild.Text };
                            MySqlDataReader already_del = helper.GetReaderByCmd("delete_lotno", str_del, obj_del);
                            if (already_del.Read())
                            {
                                CommonClass.lot_info_changes = false;
                                already_del.Close();
                                helper.CloseConnection();
                                dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, dgProduct_grid_selectedRow));
                                MessageBox.Show("Lot Number Deleted successfully....", "DELETE Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textLotNoAdd.Focus();

                            }
                            already_del.Close();
                            helper.CloseConnection();
                            chkbx_scrap.Checked = false;
                            chk_onhold.Checked = false;
                            chk_bproduct.Checked = false;
                            txt_reason_hs.Text = "Remarks";
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Check the Lot Number....", "DELETE Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textSearchLotNo.Focus();
                    }
                

                }
                else
                {
                    MessageBox.Show("Check the Lot Number....", "DELETE Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textSearchLotNo.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_lotinfo_del_Click", ex);
            }
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void btn_lotinfo_download_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download LotInformation ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                    for(int i= 0; i <dataGridView1.Rows.Count;i++)
                    {
                        XcelApp.Cells[3 + i, 2] = txtCustomerCode.Text;
                        XcelApp.Cells[3 + i, 3] = txtCustomerNameF.Text;
                        XcelApp.Cells[3 + i, 4] = txt_lotinfo_itemcode.Text;
                        XcelApp.Cells[3 + i, 5] = txt_lotinfo_itm_nam.Text;
                    }    
                    Excel.Range oRng;                  
                    XcelApp.DisplayAlerts = false;          
                    int top_i = 8;
                    // Column Header 1 
                    List<ObjColumns> array = new List<ObjColumns>();              
                    array.Add(new ObjColumns("A1", "F1"));
                    oRng = ws.get_Range("A1", "F1");
                    oRng.Value2 = "";
                    oRng.Merge(Missing.Value);
                    foreach (var topheader in CommonClass.Process_name_gridbind_columns)
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
                            Excel.Range c1 = ws.Cells[1, 6];
                            //top_i = top_i + 2;
                            top_i = top_i + 4;
                            Excel.Range c2 = ws.Cells[1, top_i];
                            oRng = (Excel.Range)ws.get_Range(c1, c2);
                            oRng.Value2 = topheader.ProcessNames;
                            oRng.Merge(Missing.Value);
                        }
                        top_i++;
                    }              
                    int get_date_column = 0;
                    bool skip_columns_lotno = false;
                    bool skip_columns_after = false;
                    bool skip_columns_lotnochild = false;
                    int reduct_count_two = 0;
                    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                    {
                        int cell_count = i;
                        XcelApp.Cells[2, 1] = "LotNo.";
                        XcelApp.Cells[2, 2] = "Customer Code";
                        XcelApp.Cells[2, 3] = "Customer Name";
                        XcelApp.Cells[2, 4] = "Item Code";
                        XcelApp.Cells[2, 5] = "Item Name";
                        string skip_Lotno = dataGridView1.Columns[i - 1].HeaderText;
                        
                        if(skip_Lotno == "Lotno" || skip_Lotno == "LotnoChild")
                        {
                            skip_columns_lotno = true;
                            if(skip_Lotno == "LotnoChild")
                            {
                                skip_columns_after = true;                        
                            }
                        } 
                        if(!skip_columns_after)
                        {
                            if (!skip_columns_lotno)
                            {
                                XcelApp.Cells[2, i + 5] = dataGridView1.Columns[i - 1].HeaderText;
                            }
                            else if (skip_columns_lotno)
                            {
                                reduct_count_two = cell_count + 1;
                            }
                        }
                        else if(skip_columns_after && !skip_columns_lotno)
                        {
                            reduct_count_two = cell_count + 1;
                            skip_columns_lotno = false;
                        }
                        else if (skip_columns_after && skip_columns_lotno)
                        {
                            if(skip_columns_lotnochild)
                            {
                                XcelApp.Cells[2, i + 3] = dataGridView1.Columns[i-1].HeaderText;
                            }
                            else
                            {
                                skip_columns_lotnochild = true;
                            }
                            
                        }                        
                        get_date_column++;
                    }                    
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
                    string compinepath = "\\Lot Information -" + datetime;
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
                throw new ArgumentNullException("btn_lotinfo_download_Click", ex);
            }
        }
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void btnPrintLableSearch_Click(object sender, EventArgs e)
        {
            try
            {
                    Cursor cursor = Cursors.WaitCursor;
                    DataTable dt = new DataTable();
                    dataGridView2.DataSource = dt;
                    printLable_gridbind();
                    dataGridView2.ColumnHeadersHeight = 60;
                    // more than one time print means looping skip 
                    CommonClass.Superlogin_allow = false;
                    cursor = Cursors.Default;               
               
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnPrintLableSearch_Click", ex);
            }
        }

        public void printLable_gridbind()
        {
            try
            {
                List<lotinfo_main_table> lot_maintbl = new List<lotinfo_main_table>();
                List<lotinfo_main_table> lot_onlytbl = new List<lotinfo_main_table>();
                CommonClass.list_bar1code = new List<barcode1_details>();
                CommonClass.list_qrcode = new List<qrcode_details>();
                dataSetQR1 = new DataSetQR();
                dataSetBarCode1 = new DataSetBarCode();               
                DataSet ds_view = new DataSet();
                DataSet ds_view_tbl_only_lotno = new DataSet();
                string ActionType = string.Empty;
                string ActionType_role = string.Empty;
                string print_lable_status = string.Empty;
                if (ckbox_pl_excludeP.Checked)
                {
                    print_lable_status = "No"; 
                     ActionType = "printlable_gb_ex";
                }
                else if (!ckbox_pl_excludeP.Checked)
                {
                    print_lable_status = "Yes";
                    ActionType = "printlable_gb";
                }
                if (chk_pl_lotno.Checked)
                {
                    ActionType_role = "print_lotno";
                    string[] str_view_lot = { "@custcd", "@lotno", "@lotno_child_frm", "@lotno_child_to", "@itemcd", "@print_lblstatus", "@manf_frm", "@manf_to", "@ActionType", "@ActionType_role" };
                    string[] obj_view_lot = { txtCustomerCode.Text, txt_pl_lotno.Text, txt_pl_frm_lotc.Text, txt_pl_to_lotc.Text, txt_pl_itemcode.Text, print_lable_status, date_print_lable_picker.Value.ToString("yyyy-MM-dd"), date_print_lable_picker_to.Value.ToString("yyyy-MM-dd"), ActionType, ActionType_role };
                    MySqlDataReader lotinfo_main_tbl = helper.GetReaderByCmd("pi_printlable_gridbind", str_view_lot, obj_view_lot);
                    lot_maintbl = LocalReportExtensions.GetList<lotinfo_main_table>(lotinfo_main_tbl);
                    helper.CloseConnection();
                    MySqlDataReader lotinfo_only_tbl = helper.GetReaderByCmd("pi_printlable_gridbind_lotinfo_only", str_view_lot, obj_view_lot);
                    lot_onlytbl = LocalReportExtensions.GetList<lotinfo_main_table>(lotinfo_only_tbl);
                    helper.CloseConnection();
                   /// var Unmatch = lot_onlytbl.Where(n => lot_maintbl.Any(o => o.lot_no == n.lot_no && o.lot_no_child != n.lot_no_child)).ToList();
                    var Unmatch = lot_onlytbl.Where(n => lot_maintbl.All(o => o.lot_no == n.lot_no && o.lot_no_child != n.lot_no_child)).ToList();
                    foreach (var onlytbl in lot_onlytbl)
                    {
                        lotinfo_main_table model = new lotinfo_main_table();
                        foreach (var maintbl in lot_maintbl)
                        {
                            //if(onlytbl.lot_no==maintbl.lot_no && onlytbl.lot_no_child != maintbl.lot_no_child)
                            //{
                            //    model.lot_no = onlytbl.lot_no;
                            //    model.lot_no_child = onlytbl.lot_no_child;
                            //    model.printdate = onlytbl.printdate;
                            //    model.printed_copy_join = onlytbl.printed_copy_join;
                            //    model.printed_date_join = onlytbl.printed_date_join;
                            //    model.printed_names_join = onlytbl.printed_names_join;
                            //    model.print_person_name = onlytbl.print_person_name;
                            //}
                            if (onlytbl.lot_no == maintbl.lot_no && onlytbl.lot_no_child == maintbl.lot_no_child)
                            {
                                if (maintbl.printdate == "-" && onlytbl.printdate != "-")
                                {
                                    maintbl.printdate = onlytbl.printdate;
                                    maintbl.printed_copy_join = onlytbl.printed_copy_join;
                                    maintbl.printed_date_join = onlytbl.printed_date_join;
                                    maintbl.printed_names_join = onlytbl.printed_names_join;
                                    maintbl.print_person_name = onlytbl.print_person_name;

                                }
                            }
                        }

                    }
                    lot_maintbl.AddRange(Unmatch);                    
                }
                else if (chk_pl_lotno.Checked == false)
                {
                    ActionType_role = "print_without_lotno";
                    string[] str_view = { "@custcd","@lotno", "@lotno_child_frm", "@lotno_child_to", "@itemcd", "@print_lblstatus", "@manf_frm", "@manf_to", "@ActionType", "@ActionType_role" };
                    string[] obj_view = { txtCustomerCode.Text,string.Empty, string.Empty, string.Empty, txt_pl_itemcode.Text, print_lable_status, date_print_lable_picker.Value.ToString("yyyy-MM-dd"), date_print_lable_picker_to.Value.ToString("yyyy-MM-dd"), ActionType, ActionType_role };

                    MySqlDataReader lotinfo_main_tbl = helper.GetReaderByCmd("pi_printlable_gridbind_without_lotno", str_view, obj_view);
                    lot_maintbl = LocalReportExtensions.GetList<lotinfo_main_table>(lotinfo_main_tbl);
                    helper.CloseConnection();
                    MySqlDataReader lotinfo_only_tbl = helper.GetReaderByCmd("pi_printlable_gridbind_without_lotno_lotinfo_only", str_view, obj_view);
                    lot_onlytbl = LocalReportExtensions.GetList<lotinfo_main_table>(lotinfo_only_tbl);
                    helper.CloseConnection();                  
                    //var Unmatch = lot_onlytbl.Where(n => lot_maintbl.All(o => o.lot_no == n.lot_no && o.lot_no_child != n.lot_no_child)).ToList();
                    var Unmatch = lot_onlytbl.Where(n => lot_maintbl.All(o => o.lot_no == n.lot_no && o.lot_no_child != n.lot_no_child)).ToList();

                    foreach (var onlytbl in lot_onlytbl)
                    {
                        lotinfo_main_table model = new lotinfo_main_table();
                        foreach (var maintbl in lot_maintbl)
                        {
                            //if(onlytbl.lot_no==maintbl.lot_no && onlytbl.lot_no_child != maintbl.lot_no_child)
                            //{
                            //    model.lot_no = onlytbl.lot_no;
                            //    model.lot_no_child = onlytbl.lot_no_child;
                            //    model.printdate = onlytbl.printdate;
                            //    model.printed_copy_join = onlytbl.printed_copy_join;
                            //    model.printed_date_join = onlytbl.printed_date_join;
                            //    model.printed_names_join = onlytbl.printed_names_join;
                            //    model.print_person_name = onlytbl.print_person_name;
                            //}
                            if (onlytbl.lot_no == maintbl.lot_no && onlytbl.lot_no_child == maintbl.lot_no_child)
                            {
                                if (maintbl.printdate == "-" && onlytbl.printdate != "-")
                                {
                                    maintbl.printdate = onlytbl.printdate;
                                    maintbl.printed_copy_join = onlytbl.printed_copy_join;
                                    maintbl.printed_date_join = onlytbl.printed_date_join;
                                    maintbl.printed_names_join = onlytbl.printed_names_join;
                                    maintbl.print_person_name = onlytbl.print_person_name;

                                }
                            }
                        }

                    }
                    lot_maintbl.AddRange(Unmatch);
                }
                dataGridView2.Refresh();
                
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                dataGridView2.DataSource = null;
                dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.AllowUserToAddRows = true;
                this.dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                this.dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                

                if (lot_maintbl.Count>0)
                {                  
                    foreach(var onetime in lot_maintbl)
                    {
                        Print_label_type = onetime.lable_typ;
                        Print_customer_name = onetime.customerfull_name;
                        Print_Item_code = onetime.item_code; 
                        Print_Item_name = onetime.item_name;
                        Print_Qty = onetime.lotqty;
                        txt_lotinfo_itm_nam.Text = onetime.item_name;
                        break;
                    }
                    //if (dt.Rows.Count > 0)
                    //{
                        int index = 0;
                        List<string> already_exits_row = new List<string>();
                       
                        foreach (var drow in lot_maintbl)
                        {
                            string lotno = drow.Lotno;
                            // lot no format change 
                            string lotno_spl = lotno.Split('-')[0].ToString();
                            string lotno_spl_chld = lotno.Split('-')[1].ToString();
                            int convert_lotno = Convert.ToInt32(lotno_spl);
                            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                            string lotno_format = convert_lotno.ToString("D7");
                            string lotnochld_format = convert_lotnochld.ToString("D2");
                            //
                            string manufacturing_date = drow.manufacturing_date;
                            string lotqty = drow.lotqty;
                            string expairy_dt = drow.expairy_dt;
                            string additional_code = drow.additional_code;
                            //if (additional_code != string.Empty && additional_code != "-")
                            //{
                            //    int formate_type = Convert.ToInt32(additional_code);
                            //    additional_code = formate_type.ToString("D5");
                            //}
                            string m1 = drow.m1;
                            string m2 = drow.m2;
                            string m3 = drow.m3;
                            string m4 = drow.m4;
                            string grid_bind_m1 = drow.m1;
                            string grid_bind_m2 = drow.m2;
                            string grid_bind_m3 = drow.m3;
                            string grid_bind_m4 = drow.m4;
                            if (string.IsNullOrEmpty(m1) || m1=="Null")
                            {
                                m1 = " ";
                            }
                            if (string.IsNullOrEmpty(m2) || m2 == "Null")
                            {
                                m2 = " ";
                            }
                            if (string.IsNullOrEmpty(m3) || m3 == "Null")
                            {
                                m3 = " ";
                            }
                            if (string.IsNullOrEmpty(m4) || m4 == "Null")
                            {
                                m4 = " ";
                            }
                            string printdate = drow.printdate;                 
                            string lot_qty = drow.lotqty;
                            string pk_lotinfo_id = drow.idproduction_input_master;
                            string customercode = drow.customercode;
                            string customershortname = drow.customershort_name;
                            string customerfullname = drow.customerfull_name;
                            string itemcode = drow.item_code;
                            string itemname = drow.lot_item_name;
                            string print_person_name = drow.print_person_name;
                            string print_date_join = drow.printed_date_join;
                            string print_pname_join = drow.printed_names_join;
                            string print_copy_join = drow.printed_copy_join;
                            string print_date_one = string.Empty;
                            string print_person_one = string.Empty;
                            string print_copy_one = string.Empty;
                            string print_date_two = string.Empty;
                            string print_person_two = string.Empty;
                            string print_copy_two = string.Empty;
                            string print_date_three = string.Empty;
                            string print_person_three = string.Empty;
                            string print_copy_three = string.Empty;
                            string last_three_pdate;
                            string last_three_pname;
                            string last_three_copy;
                            string[] array_last_three_pdate;
                            string[] array_last_three_pname;
                            string[] array_last_three_copy;
                            if (!string.IsNullOrEmpty(print_date_join) && !string.IsNullOrEmpty(print_pname_join))
                            {
                                // last 3 records get 
                                last_three_pdate = string.Join(",", print_date_join.Split(',').Reverse().Take(3).Reverse());
                                last_three_pname = string.Join(",", print_pname_join.Split(',').Reverse().Take(3).Reverse());

                                // 
                                array_last_three_pdate = last_three_pdate.Split(',');
                                array_last_three_pname = last_three_pname.Split(',');

                                // date assign string
                                if (array_last_three_pdate.Length == 3)
                                {
                                    print_date_one = last_three_pdate.Split(',')[0];
                                    print_date_two = last_three_pdate.Split(',')[1];
                                    print_date_three = last_three_pdate.Split(',')[2];
                                }
                                else if (array_last_three_pdate.Length == 2)
                                {
                                    print_date_one = last_three_pdate.Split(',')[0];
                                    print_date_two = last_three_pdate.Split(',')[1];
                                }
                                else if (array_last_three_pdate.Length == 1)
                                {
                                    print_date_one = last_three_pdate.Split(',')[0];
                                }
                                // name assign string 
                                if (array_last_three_pname.Length == 3)
                                {
                                    print_person_one = last_three_pname.Split(',')[0];
                                    print_person_two = last_three_pname.Split(',')[1];
                                    print_person_three = last_three_pname.Split(',')[2];
                                }
                                else if (array_last_three_pname.Length == 2)
                                {
                                    print_person_one = last_three_pname.Split(',')[0];
                                    print_person_two = last_three_pname.Split(',')[1];
                                }
                                else if (array_last_three_pname.Length == 1)
                                {
                                    print_person_one = last_three_pname.Split(',')[0];
                                }
                            }
                            else if (string.IsNullOrEmpty(print_date_join) && string.IsNullOrEmpty(print_pname_join))
                            {
                                print_date_one = printdate;
                                print_person_one = print_person_name;
                                // null but already date formate column data avaiable
                                print_date_join = printdate;
                                print_pname_join = print_person_name;
                            }

                            // Print number of copy 
                            if (!string.IsNullOrEmpty(print_copy_join))
                            {
                                // last 3 records get 
                                last_three_copy = string.Join(",", print_copy_join.Split(',').Reverse().Take(3).Reverse());
                                // 
                                array_last_three_copy = last_three_copy.Split(',');
                                // number of copy
                                if (array_last_three_copy.Length == 3)
                                {
                                    print_copy_one = last_three_copy.Split(',')[0];
                                    print_copy_two = last_three_copy.Split(',')[1];
                                    print_copy_three = last_three_copy.Split(',')[2];
                                }
                                else if (array_last_three_copy.Length == 2)
                                {
                                    print_copy_one = last_three_copy.Split(',')[0];
                                    print_copy_two = last_three_copy.Split(',')[1];
                                }
                                else if (array_last_three_copy.Length == 1)
                                {
                                    print_copy_one = last_three_copy.Split(',')[0];
                                }
                            }
                            else if (string.IsNullOrEmpty(print_copy_join))
                            {
                                //  print_copy_one = cmbPrintCopy.Text;
                            }
                            dataGridView2.Rows.Add();
                            // auto select newly print rows 
                            if (printdate == "-")
                            {
                                //dataGridView2.Rows[index].Cells[1].Value = System.Windows.Forms.CheckState.Checked;
                                dataGridView2.Rows[index].Cells[1].Value = true;
                                dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                //grid_checkbox_checked(lotno_format + "-" + lotnochld_format, string.Empty, additional_code, expairy_dt, lot_qty, additional_code,
                                //    manufacturing_date, pk_lotinfo_id, m1, m2, m3, m4);
                                grid_checkbox_checked(lotno_format + "-" + lotnochld_format, expairy_dt, additional_code, expairy_dt, lot_qty, additional_code,
                                    manufacturing_date, pk_lotinfo_id, m1, m2, m3, m4, print_date_join, print_pname_join, itemcode, print_copy_join);
                                btnPrintlblSelectall.Text = "Un-Select All";
                            }
                            else
                            {
                                dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                                btnPrintlblSelectall.Text = "Select All";
                            }
                            dataGridView2.Rows[index].Cells[0].Value = lotno_format + "-" + lotnochld_format;
                            dataGridView2.Rows[index].Cells[2].Value = manufacturing_date;
                            dataGridView2.Rows[index].Cells[3].Value = lotqty;
                            dataGridView2.Rows[index].Cells[4].Value = expairy_dt;
                            dataGridView2.Rows[index].Cells[5].Value = additional_code;
                            dataGridView2.Rows[index].Cells[6].Value = grid_bind_m1;
                            dataGridView2.Rows[index].Cells[7].Value = grid_bind_m2;
                            dataGridView2.Rows[index].Cells[8].Value = grid_bind_m3;
                            dataGridView2.Rows[index].Cells[9].Value = grid_bind_m4;
                            //dataGridView2.Rows[index].Cells[10].Value = printdate;
                            dataGridView2.Rows[index].Cells[10].Value = print_date_one;
                            dataGridView2.Rows[index].Cells[11].Value = box_qty;
                            dataGridView2.Rows[index].Cells[12].Value = pk_lotinfo_id;
                            dataGridView2.Rows[index].Cells[13].Value = customercode;
                            dataGridView2.Rows[index].Cells[14].Value = customershortname;
                            dataGridView2.Rows[index].Cells[15].Value = customerfullname;
                            dataGridView2.Rows[index].Cells[16].Value = itemcode;
                            dataGridView2.Rows[index].Cells[17].Value = itemname;
                            dataGridView2.Rows[index].Cells[18].Value = print_person_one;
                            dataGridView2.Rows[index].Cells[19].Value = print_copy_one;
                            dataGridView2.Rows[index].Cells[20].Value = print_date_two;
                            dataGridView2.Rows[index].Cells[21].Value = print_person_two;
                            dataGridView2.Rows[index].Cells[22].Value = print_copy_two;
                            dataGridView2.Rows[index].Cells[23].Value = print_date_three;
                            dataGridView2.Rows[index].Cells[24].Value = print_person_three;
                            dataGridView2.Rows[index].Cells[25].Value = print_copy_three;
                            dataGridView2.Rows[index].Cells[26].Value = print_pname_join;
                            dataGridView2.Rows[index].Cells[27].Value = print_date_join;
                            dataGridView2.Rows[index].Cells[28].Value = print_copy_join;
                            index++;
                        }
                        //this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Ascending);
                        dataGridView2.Refresh();
                        dataGridView2.Sort(dataGridView2.Columns[2], ListSortDirection.Descending);
                        this.dataGridView2.AllowUserToAddRows = false;
                   // }

                   // else
                   // {
                   //     MessageBox.Show("No Records Found..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   // }

                    helper.CloseConnection();
                    // get printer name to store the var
                    store_printer_name_get();
                }
                else
                {
                    if (lot_onlytbl.Count > 0)                    
                    {
                        
                        foreach(var onetime in lot_onlytbl)
                        {
                            Print_label_type = onetime.lable_typ;
                            Print_customer_name = onetime.customerfull_name;
                            Print_Item_code = onetime.item_code;
                            Print_Item_name = onetime.item_name;
                            Print_Qty = onetime.lotqty;
                            txt_lotinfo_itm_nam.Text = onetime.lot_item_name;
                            break;
                        }
                        //if (dt.Rows.Count > 0)
                        //{
                            int index = 0;
                            List<string> already_exits_row = new List<string>();                         
                            foreach (var drow in lot_onlytbl)
                            {
                                string lotno = drow.Lotno;
                                // lot no format change 
                                string lotno_spl = lotno.Split('-')[0].ToString();
                                string lotno_spl_chld = lotno.Split('-')[1].ToString();
                                int convert_lotno = Convert.ToInt32(lotno_spl);
                                int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
                                string lotno_format = convert_lotno.ToString("D7");
                                string lotnochld_format = convert_lotnochld.ToString("D2");
                                //
                                string manufacturing_date = drow.manufacturing_date;
                                string lotqty = drow.lotqty;
                                string expairy_dt = drow.expairy_dt;
                                string additional_code = drow.additional_code;
                                string m1 = drow.m1;
                                string m2 = drow.m2;
                                string m3 = drow.m3;
                                string m4 = drow.m4;
                                string grid_bind_m1 = drow.m1;
                                string grid_bind_m2 = drow.m2;
                                string grid_bind_m3 = drow.m3;
                                string grid_bind_m4 = drow.m4;
                                if (string.IsNullOrEmpty(drow.m1) || m1 == "Null")
                                {
                                    m1 = " ";
                                }
                                if (string.IsNullOrEmpty(drow.m2) || m2 == "Null")
                                {
                                    m2 = " ";
                                }
                                if (string.IsNullOrEmpty(drow.m3) || m3 == "Null")
                                {
                                    m3 = " ";
                                }
                                if (string.IsNullOrEmpty(drow.m4) || m4 == "Null")
                                {
                                    m4 = " ";
                                }
                                string printdate = drow.printdate;                            
                                string lot_qty = drow.lotqty;                                                       
                                string pk_lotinfo_id = drow.idproduction_input_master;
                                string customercode = drow.customercode;
                                string customershortname = drow.customershort_name;
                                string customerfullname = drow.customerfull_name;
                                string itemcode = drow.item_code;
                                string itemname = drow.lot_item_name;
                                string print_person_name = drow.print_person_name;
                                string print_date_join = drow.printed_date_join;
                                string print_pname_join = drow.printed_names_join;
                                string print_copy_join = drow.printed_copy_join;
                                string print_date_one = string.Empty;
                                string print_person_one = string.Empty;
                                string print_date_two = string.Empty;
                                string print_person_two = string.Empty;
                                string print_date_three = string.Empty;
                                string print_person_three = string.Empty;
                                string last_three_pdate;
                                string last_three_pname;
                                string[] array_last_three_pdate;
                                string[] array_last_three_pname;
                                if (!string.IsNullOrEmpty(print_date_join) && !string.IsNullOrEmpty(print_pname_join))
                                {
                                    // last 3 records get 
                                    last_three_pdate = string.Join(",", print_date_join.Split(',').Reverse().Take(3).Reverse());
                                    last_three_pname = string.Join(",", print_pname_join.Split(',').Reverse().Take(3).Reverse());
                                    // 
                                    array_last_three_pdate = last_three_pdate.Split(',');
                                    array_last_three_pname = last_three_pname.Split(',');
                                    // date assign string
                                    if (array_last_three_pdate.Length == 3)
                                    {
                                        print_date_one = last_three_pdate.Split(',')[0];
                                        print_date_two = last_three_pdate.Split(',')[1];
                                        print_date_three = last_three_pdate.Split(',')[2];
                                    }
                                    else if (array_last_three_pdate.Length == 2)
                                    {
                                        print_date_one = last_three_pdate.Split(',')[0];
                                        print_date_two = last_three_pdate.Split(',')[1];
                                    }
                                    else if (array_last_three_pdate.Length == 1)
                                    {
                                        print_date_one = last_three_pdate.Split(',')[0];
                                    }
                                    // name assign string 
                                    if (array_last_three_pname.Length == 3)
                                    {
                                        print_person_one = last_three_pname.Split(',')[0];
                                        print_person_two = last_three_pname.Split(',')[1];
                                        print_person_three = last_three_pname.Split(',')[2];
                                    }
                                    else if (array_last_three_pname.Length == 2)
                                    {
                                        print_person_one = last_three_pname.Split(',')[0];
                                        print_person_two = last_three_pname.Split(',')[1];
                                    }
                                    else if (array_last_three_pname.Length == 1)
                                    {
                                        print_person_one = last_three_pname.Split(',')[0];
                                    }
                                }
                                else if (string.IsNullOrEmpty(print_date_join) && string.IsNullOrEmpty(print_pname_join))
                                {
                                    print_date_one = printdate;
                                    print_person_one = print_person_name;
                                    // null but already date formate column data avaiable
                                    print_date_join = printdate;
                                    print_pname_join = print_person_name;
                                }

                                dataGridView2.Rows.Add();

                                if (printdate == "-")
                                {
                                    //dataGridView2.Rows[index].Cells[1].Value = System.Windows.Forms.CheckState.Checked;

                                    dataGridView2.Rows[index].Cells[1].Value = true;
                                    dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.GreenYellow;
                                    grid_checkbox_checked(lotno_format + "-" + lotnochld_format, string.Empty, additional_code, expairy_dt, lot_qty, additional_code,
                                        manufacturing_date, pk_lotinfo_id, m1, m2, m3, m4, print_date_join, print_pname_join, itemcode, print_copy_join);
                                    btnPrintlblSelectall.Text = "Un-Select All";
                                }
                                else
                                {
                                    dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                                    btnPrintlblSelectall.Text = "Select All";
                                }
                                dataGridView2.Rows[index].Cells[0].Value = lotno_format + "-" + lotnochld_format;
                                dataGridView2.Rows[index].Cells[2].Value = manufacturing_date;
                                dataGridView2.Rows[index].Cells[3].Value = lotqty;
                                dataGridView2.Rows[index].Cells[4].Value = expairy_dt;
                                dataGridView2.Rows[index].Cells[5].Value = additional_code;
                                dataGridView2.Rows[index].Cells[6].Value = grid_bind_m1;
                                dataGridView2.Rows[index].Cells[7].Value = grid_bind_m2;
                                dataGridView2.Rows[index].Cells[8].Value = grid_bind_m3;
                                dataGridView2.Rows[index].Cells[9].Value = grid_bind_m4;
                                //dataGridView2.Rows[index].Cells[10].Value = printdate;
                                dataGridView2.Rows[index].Cells[10].Value = print_date_one;
                                dataGridView2.Rows[index].Cells[11].Value = box_qty;
                                dataGridView2.Rows[index].Cells[12].Value = pk_lotinfo_id;
                                dataGridView2.Rows[index].Cells[13].Value = customercode;
                                dataGridView2.Rows[index].Cells[14].Value = customershortname;
                                dataGridView2.Rows[index].Cells[15].Value = customerfullname;
                                dataGridView2.Rows[index].Cells[16].Value = itemcode;
                                dataGridView2.Rows[index].Cells[17].Value = itemname;
                                dataGridView2.Rows[index].Cells[18].Value = print_person_one;
                                dataGridView2.Rows[index].Cells[19].Value = print_date_two;
                                dataGridView2.Rows[index].Cells[20].Value = print_person_two;
                                dataGridView2.Rows[index].Cells[21].Value = print_date_three;
                                dataGridView2.Rows[index].Cells[22].Value = print_person_three;
                                dataGridView2.Rows[index].Cells[23].Value = print_pname_join;
                                dataGridView2.Rows[index].Cells[24].Value = print_date_join;
                                index++;

                            }
                            this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Ascending);
                            this.dataGridView2.AllowUserToAddRows = false;
                       // } 
                        helper.CloseConnection();
                        // get printer name to store the var
                        store_printer_name_get();
                    }
                    else
                    {    
                        helper.CloseConnection();
                        MessageBox.Show("No Records Found....", "Print Lable-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_pl_lotno.Focus();
                    }

                }
                this.dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                this.dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("printLable_gridbind", ex);
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
        private void printLabl_Click(object sender, EventArgs e)
        {
            try
            {              
             
                if (txt_print_person_name.Text != "Name" && !string.IsNullOrEmpty(txt_print_person_name.Text))
                {
                    if (cmbPrintCopy.SelectedIndex != -1)
                    {
                        CommonClass.Superlogin_close_btn_click = false;
                        if (Current_PrinterName == "")
                        {
                            if (comboBox_printernames.SelectedIndex != -1)
                            {
                                LocalReportExtensions.SelectedPrinterName = comboBox_printernames.SelectedItem.ToString();
                                store_printer_name(comboBox_printernames.SelectedItem.ToString(), "Yes");

                            }
                            else
                            {
                                MessageBox.Show("Previously the printer name was not selected....", "PRINTER NAMES", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                comboBox_printernames.Focus();
                                return;
                            }
                        }

                        DialogResult dialogResult = MessageBox.Show("Do you want to Print ?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            if (Current_PrinterName == "")
                            {
                                LocalReportExtensions.SelectedPrinterName = comboBox_printernames.SelectedItem.ToString();
                            }
                            else if (Current_PrinterName != "")
                            {
                                if (comboBox_printernames.SelectedIndex != -1)
                                {
                                    if (Current_PrinterName != comboBox_printernames.SelectedItem.ToString())
                                    {
                                        LocalReportExtensions.SelectedPrinterName = comboBox_printernames.SelectedItem.ToString();
                                        store_printer_name(comboBox_printernames.SelectedItem.ToString(), "Yes");
                                    }
                                    else
                                    {
                                        LocalReportExtensions.SelectedPrinterName = Current_PrinterName;
                                    }
                                }
                                else
                                {
                                    LocalReportExtensions.SelectedPrinterName = Current_PrinterName;
                                }

                            }
                            if (Print_label_type == "2")
                            {
                                // values pass print page
                                this.dataSetBarCode1.Clear();
                                // qr code
                                if (CommonClass.list_bar1code.Count > 0)
                                {
                                    int i = 1;
                                    string ActionType = "UpdateData";
                                    foreach (var item in CommonClass.list_bar1code)
                                    {
                                        // 1st time print 
                                        // print copy assign
                                        bool print_cpy_null = false;
                                        if (item.print_copy == null && item.printed_date != null)
                                        {
                                            item.print_copy = cmbPrintCopy.Text;
                                            print_cpy_null = true;
                                        }
                                        else if (item.print_copy != null)
                                        {
                                            string join_count = item.print_copy + "," + cmbPrintCopy.Text;
                                            item.print_copy = join_count;
                                        }
                                        if (!print_cpy_null)
                                        {
                                            int check_count_cpy_morethan_one = Convert.ToInt32(item.print_copy.Split(',').Count());
                                            if (check_count_cpy_morethan_one > 1)
                                            {
                                                // already enroll the password means its false
                                                if (!CommonClass.Superlogin_allow)
                                                {

                                                    FormSuperLogin frm = new FormSuperLogin();
                                                    frm.Owner = this;
                                                    frm.ShowDialog();
                                                    Cursor.Current = Cursors.WaitCursor;
                                                }
                                                if (CommonClass.Superlogin_close_btn_click)
                                                {
                                                    CommonClass.Superlogin_close_btn_click = false;
                                                    print_return();
                                                    return;
                                                }

                                            }
                                            print_cpy_null = false;
                                        }
                                        dataSetBarCode1.Barcode.AddBarcodeRow(i.ToString(), item.barcode_companyname,
                                        item.barcode_partno,
                                        item.barcode_partname,
                                        item.barcode_expiry,
                                        item.barcode_qty,
                                        item.barcode_pcs,
                                        item.barcode_lotno,
                                        item.barcode_materialcode,
                                        item.barcode_input_1,
                                        item.barcode_input_2,
                                        item.imageUrl_barcode_1,
                                        item.imageUrl_barcode_2,
                                        item.barcode_m1,
                                        item.barcode_m2,
                                        item.barcode_m3,
                                        item.barcode_m4);
                                        if(cmbPrintCopy.Text=="2")
                                        {
                                            i++;
                                        dataSetBarCode1.Barcode.AddBarcodeRow(i.ToString(), item.barcode_companyname,
                                        item.barcode_partno,
                                        item.barcode_partname,
                                        item.barcode_expiry,
                                        item.barcode_qty,
                                        item.barcode_pcs,
                                        item.barcode_lotno,
                                        item.barcode_materialcode,
                                        item.barcode_input_1,
                                        item.barcode_input_2,
                                        item.imageUrl_barcode_1,
                                        item.imageUrl_barcode_2,
                                        item.barcode_m1,
                                        item.barcode_m2,
                                        item.barcode_m3,
                                        item.barcode_m4);
                                        }
                                        i++;
                                        // update lotinfo _tble 
                                        DateTime current_date_time = DateTime.Now;
                                        string splt_lotno = item.barcode_lotno.Split('-')[0];
                                        string splt_lotno_child = item.barcode_lotno.Split('-')[1];
                                        // 1st time print 
                                        // print date assign 
                                        if (item.print_person_name == null && item.printed_date != null)
                                        {
                                            item.print_person_name = txt_print_person_name.Text;
                                        }
                                        else if (item.print_person_name != null)
                                        {
                                            string join_names = item.print_person_name + "," + txt_print_person_name.Text;
                                            item.print_person_name = join_names;
                                        }
                                        // check count 
                                        int check_count = Convert.ToInt32(item.print_person_name.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_person_name = string.Join(",", item.print_person_name.Split(',').Reverse().Take(3).Reverse());
                                            item.printed_date = string.Join(",", item.printed_date.Split(',').Reverse().Take(3).Reverse());
                                        }                                        
                                        // check count cpy
                                        int check_count_cpy = Convert.ToInt32(item.print_copy.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_copy = string.Join(",", item.print_copy.Split(',').Reverse().Take(3).Reverse());
                                        }
                                        string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname", "@print_nam_jn", "@print_dt_jn", "@print_nocpy" };
                                        string[] obj_upt = { ActionType, item.pk_lotinfo_id, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], "Yes", nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), txt_print_person_name.Text, item.print_person_name, item.printed_date, item.print_copy };
                                        MySqlDataReader upt_print_status = helper.GetReaderByCmd("pi_lotinfo_print_status_upd", str_upt, obj_upt);
                                        if (upt_print_status.Read())
                                        {
                                           
                                        }
                                        upt_print_status.Close();
                                        helper.CloseConnection();
                                        //
                                        bool already_exist = pinfo_id_already_exist(string.Empty, splt_lotno, splt_lotno_child, "lot_info_only");
                                        if (already_exist)
                                        {
                                            update_lotinformation_only_master("lotinfo_only", item.pk_lotinfo_id, splt_lotno, splt_lotno_child, item.printed_date, item.print_person_name,item.print_copy);
                                        }

                                    }                                   
                                    LocalReport localReport = new LocalReport();
                                    localReport.ReportPath = Application.StartupPath + "\\barcode.rdlc";
                                    localReport.DisplayName = "BC";
                                    ReportDataSource reportDataSource = new ReportDataSource();
                                    reportDataSource.Name = "DataSetBarCode";
                                    reportDataSource.Value = dataSetBarCode1.Barcode;
                                    localReport.DataSources.Clear();
                                    localReport.DataSources.Add(reportDataSource);
                                    localReport.PrintToPrinter();
                                    print_return();
                                }
                                else
                                {
                                    MessageBox.Show("Atleast one Checked the Lot Number....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView2.Focus();
                                }


                            }
                            else if (Print_label_type == "1")
                            {
                                this.dataSetQR1.Clear();
                                // qr code
                                if (CommonClass.list_qrcode.Count > 0)
                                {
                                    string ActionType = "UpdateData";
                                    int i = 1;
                                    foreach (var item in CommonClass.list_qrcode)
                                    {
                                        // 1st time print 
                                        // print copy assign
                                        bool print_cpy_null = false;
                                        if (item.print_copy == null && item.printed_date != null)
                                        {
                                            item.print_copy = cmbPrintCopy.Text;
                                            print_cpy_null = true;
                                        }
                                        else if (item.print_copy != null)
                                        {
                                            string join_count = item.print_copy + "," + cmbPrintCopy.Text;
                                            item.print_copy = join_count;
                                        }
                                        if (!print_cpy_null)
                                        {
                                            int check_count_cpy_morethan_one = Convert.ToInt32(item.print_copy.Split(',').Count());
                                            if (check_count_cpy_morethan_one > 1)
                                            {
                                                // already enroll the password means its false
                                                if (!CommonClass.Superlogin_allow)
                                                {

                                                    FormSuperLogin frm = new FormSuperLogin();
                                                    frm.Owner = this;
                                                    frm.ShowDialog();
                                                    Cursor.Current = Cursors.WaitCursor;
                                                }
                                                if (CommonClass.Superlogin_close_btn_click)
                                                {
                                                    CommonClass.Superlogin_close_btn_click = false;
                                                    print_return();
                                                    return;
                                                }

                                            }
                                            print_cpy_null = false;
                                        }
                                        string qr_lotno = item.qr_lotno;
                                        string qr_companyname = item.qr_companyname;
                                        string qr_partno = item.qr_partno;
                                        string qr_partname = item.qr_partname;
                                        string qr_manf = item.qr_manf;
                                        string qr_expiry = item.qr_expiry;
                                        string qr_qty = item.qr_qty;
                                        string qr_pcs = item.qr_pcs;
                                        string qr_materialcode = item.qr_materialcode;
                                        byte[] qr_imageurl = item.qr_imageurl;
                                        //
                                        string qr_m1 = item.qr_m1;
                                        string qr_m2 = item.qr_m2;
                                        string qr_m3 = item.qr_m3;
                                        string qr_m4 = item.qr_m4;
                                        dataSetQR1.QRcode.AddQRcodeRow(i, qr_lotno, qr_companyname,
                                            qr_partno, qr_partname, item.qr_manf,
                                            qr_expiry,
                                            qr_qty, item.qr_pcs,
                                            qr_materialcode, qr_imageurl,
                                            qr_m1, qr_m2, qr_m3, qr_m4);
                                        if(cmbPrintCopy.Text=="2")
                                        {
                                            i++;
                                            dataSetQR1.QRcode.AddQRcodeRow(i, qr_lotno, qr_companyname,
                                            qr_partno, qr_partname, item.qr_manf,
                                            qr_expiry,
                                            qr_qty, item.qr_pcs,
                                            qr_materialcode, qr_imageurl,
                                            qr_m1, qr_m2, qr_m3, qr_m4);
                                        }
                                        i++;
                                        DateTime current_date_time = DateTime.Now;
                                        // 1st time print 
                                        // print date assign 
                                        if (item.print_person_name == null && item.printed_date != null)
                                        {
                                            item.print_person_name = txt_print_person_name.Text;
                                        }
                                        else if (item.print_person_name != null)
                                        {
                                            string join_names = item.print_person_name + "," + txt_print_person_name.Text;
                                            item.print_person_name = join_names;
                                        }
                                        // check count 
                                        int check_count = Convert.ToInt32(item.print_person_name.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_person_name = string.Join(",", item.print_person_name.Split(',').Reverse().Take(3).Reverse());
                                            item.printed_date = string.Join(",", item.printed_date.Split(',').Reverse().Take(3).Reverse());
                                        }
                                       
                                        // check count cpy
                                        int check_count_cpy = Convert.ToInt32(item.print_copy.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_copy = string.Join(",", item.print_copy.Split(',').Reverse().Take(3).Reverse());
                                        }
                                        // update lotinfo _tble 
                                        string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname", "@print_nam_jn", "@print_dt_jn", "@print_nocpy" };
                                        string[] obj_upt = { ActionType, item.pk_lotinfo_id, item.qr_lotno.Split('-')[0], item.qr_lotno.Split('-')[1], "Yes", nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), txt_print_person_name.Text, item.print_person_name, item.printed_date,item.print_copy };
                                        MySqlDataReader upt_print_status = helper.GetReaderByCmd("pi_lotinfo_print_status_upd", str_upt, obj_upt);
                                        if (upt_print_status.Read())
                                        {                                           
                                        }
                                        upt_print_status.Close();
                                        helper.CloseConnection();
                                        //
                                        bool already_exist = pinfo_id_already_exist(string.Empty, item.qr_lotno.Split('-')[0], item.qr_lotno.Split('-')[1], "lot_info_only");
                                        if (already_exist)
                                        {
                                            update_lotinformation_only_master("lotinfo_only", item.pk_lotinfo_id, item.qr_lotno.Split('-')[0], item.qr_lotno.Split('-')[1], item.printed_date, item.print_person_name,item.print_copy);
                                        }
                                    }                                  
                                    LocalReport localReport = new LocalReport();
                                    localReport.ReportPath = Application.StartupPath + "\\qrcode.rdlc";
                                    localReport.DisplayName = "QR";
                                    ReportDataSource reportDataSource = new ReportDataSource();
                                    reportDataSource.Name = "DataSetQR";
                                    reportDataSource.Value = dataSetQR1.QRcode;
                                    localReport.DataSources.Clear();
                                    localReport.DataSources.Add(reportDataSource);
                                    localReport.PrintToPrinter();
                                    print_return();

                                }
                                else
                                {
                                    MessageBox.Show("Atleast one Checked the Lot Number....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView2.Focus();
                                }

                            }
                            else if (Print_label_type == "3")
                            {
                                // values pass print page
                                this.dataSetBarCode1.Clear();
                                if (CommonClass.list_bar1code.Count > 0)
                                {
                                    string ActionType = "UpdateData";
                                    // qr code
                                    int i = 1;
                                    foreach (var item in CommonClass.list_bar1code)
                                    {
                                        // 1st time print 
                                        // print copy assign
                                        bool print_cpy_null = false;
                                        if (item.print_copy == null && item.printed_date != null)
                                        {
                                            item.print_copy = cmbPrintCopy.Text;
                                            print_cpy_null = true;
                                        }
                                        else if (item.print_copy != null)
                                        {
                                            string join_count = item.print_copy + "," + cmbPrintCopy.Text;
                                            item.print_copy = join_count;
                                        }
                                        if (!print_cpy_null)
                                        {
                                            int check_count_cpy_morethan_one = Convert.ToInt32(item.print_copy.Split(',').Count());
                                            if (check_count_cpy_morethan_one > 1)
                                            {
                                                // already enroll the password means its false
                                                if (!CommonClass.Superlogin_allow)
                                                {

                                                    FormSuperLogin frm = new FormSuperLogin();
                                                    frm.Owner = this;
                                                    frm.ShowDialog();
                                                    Cursor.Current = Cursors.WaitCursor;
                                                }
                                                if (CommonClass.Superlogin_close_btn_click)
                                                {
                                                    CommonClass.Superlogin_close_btn_click = false;
                                                    print_return();
                                                    return;
                                                }

                                            }
                                            print_cpy_null = false;
                                        }
                                        dataSetBarCode1.Barcode.AddBarcodeRow(i.ToString(), item.barcode_companyname,
                                            item.barcode_partno,
                                            item.barcode_partname,
                                            item.barcode_expiry,
                                            item.barcode_qty,
                                            item.barcode_pcs,
                                            item.barcode_lotno,
                                            item.barcode_materialcode,
                                            item.barcode_input_1,
                                            item.barcode_input_2,
                                            item.imageUrl_barcode_1,
                                            item.imageUrl_barcode_2,
                                        item.barcode_m1,
                                        item.barcode_m2,
                                        item.barcode_m3,
                                        item.barcode_m4);
                                        if(cmbPrintCopy.Text=="2")
                                        {
                                            i++;
                                            dataSetBarCode1.Barcode.AddBarcodeRow(i.ToString(), item.barcode_companyname,
                                            item.barcode_partno,
                                            item.barcode_partname,
                                            item.barcode_expiry,
                                            item.barcode_qty,
                                            item.barcode_pcs,
                                            item.barcode_lotno,
                                            item.barcode_materialcode,
                                            item.barcode_input_1,
                                            item.barcode_input_2,
                                            item.imageUrl_barcode_1,
                                            item.imageUrl_barcode_2,
                                        item.barcode_m1,
                                        item.barcode_m2,
                                        item.barcode_m3,
                                        item.barcode_m4);
                                        }
                                        i++;
                                        DateTime current_date_time = DateTime.Now;
                                        // 1st time print 
                                        // print date assign 
                                        if (item.print_person_name == null && item.printed_date != null)
                                        {
                                            item.print_person_name = txt_print_person_name.Text;
                                        }
                                        else if (item.print_person_name != null)
                                        {
                                            string join_names = item.print_person_name + "," + txt_print_person_name.Text;
                                            item.print_person_name = join_names;
                                        }
                                        // check count 
                                        int check_count = Convert.ToInt32(item.print_person_name.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_person_name = string.Join(",", item.print_person_name.Split(',').Reverse().Take(3).Reverse());
                                            item.printed_date = string.Join(",", item.printed_date.Split(',').Reverse().Take(3).Reverse());
                                        }                                       
                                        // check count cpy
                                        int check_count_cpy = Convert.ToInt32(item.print_copy.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_copy = string.Join(",", item.print_copy.Split(',').Reverse().Take(3).Reverse());
                                        }
                                        // update lotinfo _tble 
                                        //string[] str_upt = { "@ActionType", "@pk_lotid", "@lotno", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname" };
                                        //string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname" };
                                        // 240323 2 parameters add 
                                        //string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname" };
                                        // 130423 1 paranertes add
                                        //string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname", "@print_nam_jn", "@print_dt_jn" };
                                        string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname", "@print_nam_jn", "@print_dt_jn", "@print_nocpy" };
                                        string[] obj_upt = { ActionType, item.pk_lotinfo_id, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], "Yes", nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), txt_print_person_name.Text, item.print_person_name, item.printed_date,item.print_copy };
                                        MySqlDataReader upt_print_status = helper.GetReaderByCmd("pi_lotinfo_print_status_upd", str_upt, obj_upt);
                                        if (upt_print_status.Read())
                                        {
                                            // MessageBox.Show("Lot Information Updated successfully....", "UPDATE Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            // textSearchLotNo.Focus();
                                        }
                                        upt_print_status.Close();
                                        helper.CloseConnection();
                                        //
                                        bool already_exist = pinfo_id_already_exist(string.Empty, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], "lot_info_only");
                                        if (already_exist)
                                        {
                                            update_lotinformation_only_master("lotinfo_only", item.pk_lotinfo_id, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], item.printed_date, item.print_person_name, item.print_copy);
                                        }
                                    }
                                    LocalReport localReport = new LocalReport();
                                    localReport.ReportPath = Application.StartupPath + "\\barcode1.rdlc";
                                    localReport.DisplayName = "BC";
                                    ReportDataSource reportDataSource = new ReportDataSource();
                                    reportDataSource.Name = "DataSetBarCode";
                                    reportDataSource.Value = dataSetBarCode1.Barcode;
                                    localReport.DataSources.Clear();
                                    localReport.DataSources.Add(reportDataSource);
                                    localReport.PrintToPrinter();
                                    print_return();
                                    //BarcodePrintOne frm = new BarcodePrintOne(this.dataSetBarCode1.Barcode);
                                    //frm.Owner = this;
                                    //frm.ShowDialog();
                                }
                                else
                                {
                                    MessageBox.Show("Atleast one Checked the Lot Number....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView2.Focus();
                                }

                            }
                            else if (Print_label_type == "4")
                            {
                                // values pass print page
                                this.dataSetBarCode1.Clear();
                                if (CommonClass.list_bar1code.Count > 0)
                                {
                                    string ActionType = "UpdateData";
                                    // qr code
                                    int i = 1;
                                    string print_date = string.Empty;
                                    string print_person_name = string.Empty;
                                    foreach (var item in CommonClass.list_bar1code)
                                    {
                                        // 1st time print 
                                        // print copy assign
                                        bool print_cpy_null = false;
                                        if (item.print_copy == null && item.printed_date != null)
                                        {
                                            item.print_copy = cmbPrintCopy.Text;
                                            print_cpy_null = true;
                                        }
                                        else if (item.print_copy != null)
                                        {
                                            string join_count = item.print_copy + "," + cmbPrintCopy.Text;
                                            item.print_copy = join_count;
                                        }
                                        if (!print_cpy_null)
                                        {
                                            int check_count_cpy_morethan_one = Convert.ToInt32(item.print_copy.Split(',').Count());
                                            if (check_count_cpy_morethan_one > 1)
                                            {
                                                // already enroll the password means its false
                                                if (!CommonClass.Superlogin_allow)
                                                {

                                                    FormSuperLogin frm = new FormSuperLogin();
                                                    frm.Owner = this;
                                                    frm.ShowDialog();
                                                    Cursor.Current = Cursors.WaitCursor;
                                                }
                                                if (CommonClass.Superlogin_close_btn_click)
                                                {
                                                    CommonClass.Superlogin_close_btn_click = false;
                                                    print_return();
                                                    return;
                                                }

                                            }
                                            print_cpy_null = false;
                                        }
                                        dataSetBarCode1.Barcode.AddBarcodeRow(i.ToString(), item.barcode_companyname,
                                            item.barcode_partno,
                                            item.barcode_partname,
                                            item.barcode_expiry,
                                            item.barcode_qty,
                                            item.barcode_pcs,
                                            item.barcode_lotno,
                                            item.barcode_materialcode,
                                            item.barcode_input_1,
                                            item.barcode_input_2,
                                            item.imageUrl_barcode_1,
                                            item.imageUrl_barcode_2,
                                            item.barcode_m1,
                                            item.barcode_m2,
                                            item.barcode_m3,
                                            item.barcode_m4);
                                        // Number of copys
                                        if(cmbPrintCopy.Text=="2")
                                        {
                                            i++;
                                            dataSetBarCode1.Barcode.AddBarcodeRow(i.ToString(), item.barcode_companyname,
                                            item.barcode_partno,
                                            item.barcode_partname,
                                            item.barcode_expiry,
                                            item.barcode_qty,
                                            item.barcode_pcs,
                                            item.barcode_lotno,
                                            item.barcode_materialcode,
                                            item.barcode_input_1,
                                            item.barcode_input_2,
                                            item.imageUrl_barcode_1,
                                            item.imageUrl_barcode_2,
                                        item.barcode_m1,
                                        item.barcode_m2,
                                        item.barcode_m3,
                                        item.barcode_m4);
                                        }
                                        i++;
                                        DateTime current_date_time = DateTime.Now;
                                        // 1st time print 
                                        // print date assign 
                                        if (item.print_person_name == null && item.printed_date != null)
                                        {
                                            item.print_person_name = txt_print_person_name.Text;
                                        }
                                        else if (item.print_person_name != null)
                                        {
                                            string join_names = item.print_person_name + "," + txt_print_person_name.Text;
                                            item.print_person_name = join_names;
                                        }
                                        // check count 
                                        int check_count = Convert.ToInt32(item.print_person_name.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_person_name = string.Join(",", item.print_person_name.Split(',').Reverse().Take(3).Reverse());
                                            item.printed_date = string.Join(",", item.printed_date.Split(',').Reverse().Take(3).Reverse());
                                        }
                                     
                                        // check count cpy
                                        int check_count_cpy = Convert.ToInt32(item.print_copy.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_copy = string.Join(",", item.print_copy.Split(',').Reverse().Take(3).Reverse());
                                        }
                                        string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname", "@print_nam_jn", "@print_dt_jn", "@print_nocpy" };
                                        string[] obj_upt = { ActionType, item.pk_lotinfo_id, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], "Yes", nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), txt_print_person_name.Text, item.print_person_name, item.printed_date, item.print_copy };
                                        MySqlDataReader upt_print_status = helper.GetReaderByCmd("pi_lotinfo_print_status_upd", str_upt, obj_upt);
                                        if (upt_print_status.Read())
                                        {                                          
                                        }
                                        upt_print_status.Close();
                                        helper.CloseConnection();
                                        //
                                        bool already_exist = pinfo_id_already_exist(string.Empty, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], "lot_info_only");
                                        if (already_exist)
                                        {
                                            update_lotinformation_only_master("lotinfo_only", item.pk_lotinfo_id, item.barcode_lotno.Split('-')[0], item.barcode_lotno.Split('-')[1], item.printed_date, item.print_person_name, item.print_copy);
                                        }
                                    }
                                    LocalReport localReport = new LocalReport();
                                    localReport.ReportPath = Application.StartupPath + "\\barcode4.rdlc";
                                    localReport.DisplayName = "BC";
                                    ReportDataSource reportDataSource = new ReportDataSource();
                                    reportDataSource.Name = "DataSetBarCode";
                                    reportDataSource.Value = dataSetBarCode1.Barcode;
                                    localReport.DataSources.Clear();
                                    localReport.DataSources.Add(reportDataSource);
                                    localReport.PrintToPrinter();
                                    print_return();
                                    txt_print_person_name.Text = "Name";                                    
                                }
                                else
                                {
                                    MessageBox.Show("Atleast one Checked the Lot Number....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView2.Focus();
                                }

                            }
                            else if (Print_label_type == "5")
                            {
                                this.dataSetQR1.Clear();
                                // qr code
                                if (CommonClass.list_qrcode.Count > 0)
                                {
                                    string ActionType = "UpdateData";
                                    int i = 1;
                                    foreach (var item in CommonClass.list_qrcode)
                                    {
                                        // 1st time print 
                                        // print copy assign
                                        bool print_cpy_null = false;
                                        if (item.print_copy == null && item.printed_date != null)
                                        {
                                            item.print_copy = cmbPrintCopy.Text;
                                            print_cpy_null = true;
                                        }
                                        else if (item.print_copy != null)
                                        {
                                            string join_count = item.print_copy + "," + cmbPrintCopy.Text;
                                            item.print_copy = join_count;
                                        }
                                        if(!print_cpy_null)
                                        {
                                            int check_count_cpy_morethan_one = Convert.ToInt32(item.print_copy.Split(',').Count());
                                            if (check_count_cpy_morethan_one > 1)
                                            {
                                                // already enroll the password means its false
                                                if(!CommonClass.Superlogin_allow)
                                                {                                                    
                                                    FormSuperLogin frm = new FormSuperLogin();
                                                    frm.Owner = this;
                                                    frm.ShowDialog();
                                                    Cursor.Current = Cursors.WaitCursor;
                                                }
                                                // user close btn click means its true 
                                                if (CommonClass.Superlogin_close_btn_click)
                                                {
                                                    CommonClass.Superlogin_close_btn_click = false;
                                                    print_return();
                                                    return;
                                                }
                                                
                                            }
                                            print_cpy_null = false;
                                        }
                                        
                                        string qr_lotno = item.qr_lotno;
                                        string qr_companyname = item.qr_companyname;
                                        string qr_partno = item.qr_partno;
                                        string qr_partname = item.qr_partname;
                                        string qr_manf = item.qr_manf;
                                        string qr_expiry = item.qr_expiry;
                                        string qr_qty = item.qr_qty;
                                        string qr_pcs = item.qr_pcs;
                                        string qr_materialcode = item.qr_materialcode;
                                        byte[] qr_imageurl = item.qr_imageurl;
                                        //
                                        string qr_m1 = item.qr_m1;
                                        string qr_m2 = item.qr_m2;
                                        string qr_m3 = item.qr_m3;
                                        string qr_m4 = item.qr_m4;
                                        dataSetQR1.QRcode.AddQRcodeRow(i, qr_lotno, qr_companyname,
                                            qr_partno, qr_partname, item.qr_manf,
                                            qr_expiry,
                                            qr_qty, item.qr_pcs,
                                            qr_materialcode, qr_imageurl,
                                            qr_m1, qr_m2, qr_m3, qr_m4);
                                        
                                        if(cmbPrintCopy.Text=="2")
                                        {
                                            i++;
                                            dataSetQR1.QRcode.AddQRcodeRow(i, qr_lotno, qr_companyname,
                                            qr_partno, qr_partname, item.qr_manf,
                                            qr_expiry,
                                            qr_qty, item.qr_pcs,
                                            qr_materialcode, qr_imageurl,
                                            qr_m1, qr_m2, qr_m3, qr_m4);
                                        
                                        }
                                        i++;
                                        DateTime current_date_time = DateTime.Now;
                                        //// 1st time print 
                                        //// print date assign 
                                        if (item.print_person_name == null && item.printed_date != null)
                                        {
                                            item.print_person_name = txt_print_person_name.Text;
                                        }
                                        else if (item.print_person_name != null)
                                        {
                                            string join_names = item.print_person_name + "," + txt_print_person_name.Text;
                                            item.print_person_name = join_names;
                                        }
                                        // check count 
                                        int check_count = Convert.ToInt32(item.print_person_name.Split(',').Count());
                                        if (check_count > 4)
                                        {
                                            item.print_person_name = string.Join(",", item.print_person_name.Split(',').Reverse().Take(3).Reverse());
                                            item.printed_date = string.Join(",", item.printed_date.Split(',').Reverse().Take(3).Reverse());
                                        }
                                        
                                        // check count 
                                        int check_count_cpy = Convert.ToInt32(item.print_copy.Split(',').Count());
                                        if (check_count_cpy > 4)
                                        {
                                            item.print_copy = string.Join(",", item.print_copy.Split(',').Reverse().Take(3).Reverse());                                            
                                        }
                                        string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname", "@print_nam_jn", "@print_dt_jn","@print_nocpy" };
                                        string[] obj_upt = { ActionType, item.pk_lotinfo_id, item.qr_lotno.Split('-')[0], item.qr_lotno.Split('-')[1], "Yes", nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), txt_print_person_name.Text, item.print_person_name, item.printed_date,item.print_copy};
                                        MySqlDataReader upt_print_status = helper.GetReaderByCmd("pi_lotinfo_print_status_upd", str_upt, obj_upt);
                                        if (upt_print_status.Read())
                                        {                                           
                                        }
                                        upt_print_status.Close();
                                        helper.CloseConnection();
                                        //
                                        bool already_exist = pinfo_id_already_exist(string.Empty, item.qr_lotno.Split('-')[0], item.qr_lotno.Split('-')[1], "lot_info_only");
                                        if (already_exist)
                                        {
                                            update_lotinformation_only_master("lotinfo_only", item.pk_lotinfo_id, item.qr_lotno.Split('-')[0], item.qr_lotno.Split('-')[1], item.printed_date, item.print_person_name, item.print_copy);
                                        }
                                    }                                   
                                    LocalReport localReport = new LocalReport();
                                    localReport.ReportPath = Application.StartupPath + "\\qrcodeTyp5.rdlc";
                                    localReport.DisplayName = "QR";
                                    ReportDataSource reportDataSource = new ReportDataSource();
                                    reportDataSource.Name = "DataSetQR";
                                    reportDataSource.Value = dataSetQR1.QRcode;
                                    localReport.DataSources.Clear();
                                    localReport.DataSources.Add(reportDataSource);
                                    localReport.PrintToPrinter();
                                    print_return();
                                    CommonClass.Superlogin_allow = false;
                                }
                                else
                                {
                                    MessageBox.Show("Atleast one Checked the Lot Number....", "Lot-Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView2.Focus();
                                }

                            }
                            txt_print_person_name.Text = "Name";
                            CommonClass.Superlogin_allow = false;
                            cmbPrintCopy.SelectedIndex = -1;
                            Cursor.Current = Cursors.Default;

                        }
                    }
                    else
                    {
                        MessageBox.Show("Choose the Number of copy to Print..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbPrintCopy.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Must Enter Print Person Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_print_person_name.Focus();
                }
               
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("printLabl_Click", ex);
            }
        }
        public void update_lotinformation_only_master(string ActionType,string pk_lotinfo_id,string lotno,string lotnochild,string printed_dt_jn,string printed_nms_jn,string printed_cpy_jn)
        {
            try
            {
                DateTime current_date_time = DateTime.Now;
                string[] str_upt = { "@ActionType", "@pk_lotid", "@lotnumber", "@lotno_child", "@print_lbl_status", "@printed_date", "@updated_at", "@print_pname","@print_nam_jn", "@print_dt_jn", "@print_nocpy" };
                string[] obj_upt = { ActionType, pk_lotinfo_id, lotno, lotnochild, "Yes", nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"),txt_print_person_name.Text, printed_nms_jn,printed_dt_jn, printed_cpy_jn };
                MySqlDataReader upt_print_status = helper.GetReaderByCmd("pi_lotinfo_print_status_upd", str_upt, obj_upt);
                if (upt_print_status.Read())
                {                    
                }
                upt_print_status.Close();
                helper.CloseConnection();
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("update_lotinformation_only_master", ex);
            }
        }
        public void store_printer_name(string pname, string flg)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string ActionType = "SaveData";
                DateTime current_date_time = DateTime.Now;
                string[] str_exist = { "@pname", "@flg", "@create_at", "@ActionType" };
                string[] obj_exist = { pname, flg, current_date_time.ToString(), ActionType };
                MySqlDataReader already_exist = helper.GetReaderByCmd("printer_details", str_exist, obj_exist);
                if (already_exist.Read())
                {
                    already_exist.Close();
                    helper.CloseConnection();
                }
                else
                {
                    already_exist.Close();
                    helper.CloseConnection();
                }


            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("store_printer_name", ex);
            }
        }
        public void store_printer_name_get()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string ActionType = "PrtViewData";
                string[] str_exist = { "@pname", "@flg", "@create_at", "@ActionType" };
                string[] obj_exist = { string.Empty, "yes", string.Empty, ActionType };
                MySqlDataReader already_exist = helper.GetReaderByCmd("printer_details", str_exist, obj_exist);
                if (already_exist.Read())
                {
                    Current_PrinterName = already_exist["printer_name"].ToString();
                    comboBox_printernames.Text = already_exist["printer_name"].ToString();
                    already_exist.Close();
                    helper.CloseConnection();
                }
                else
                {
                    already_exist.Close();
                    helper.CloseConnection();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("store_printer_name_get", ex);
            }
        }
        private void btnPrintlblSelectall_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                Cursor.Current = Cursors.WaitCursor;
                int rowIndex = 1;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    //CommonClass.list_qrcode = new List<qrcode_details>();
                    CommonClass.bacode1_list = new List<barcode1_details>();
                    //DataGridViewCheckBoxCell checkBox = (row.Cells[rowIndex] as DataGridViewCheckBoxCell);
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
                    chk.Value = chk.Value == null || !((bool)chk.Value);
                    this.dataGridView2.RefreshEdit();
                    this.dataGridView2.NotifyCurrentCellDirty(true);
                    if (Convert.ToBoolean(chk.Value))
                    {
                        if (Convert.ToString(row.Cells["Lotno"].Value) == string.Empty)
                        {
                            flag = true;
                        }
                        if (!flag)
                        {
                            Print_lotno = row.Cells["Lotno"].Value.ToString();
                            Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();
                            Print_date_manfdt = row.Cells["manufacturing_date"].Value.ToString();
                            Print_material_code = row.Cells["additional_code"].Value.ToString();
                            Print_M1 = row.Cells["m1"].Value.ToString();
                            Print_M2 = row.Cells["m2"].Value.ToString();
                            Print_M3 = row.Cells["m3"].Value.ToString();
                            Print_M4 = row.Cells["m4"].Value.ToString();
                            Printed_date_join = row.Cells["printdatejoin"].Value.ToString();                            
                            Print_person_name_join = row.Cells["PrintPersonjoin"].Value.ToString();
                            Print_copy_join = row.Cells["printcpjoin"].Value.ToString();
                            //030423
                            Print_Item_code =row.Cells["itemcode"].Value.ToString();
                            if (string.IsNullOrEmpty(Printed_date_join) && string.IsNullOrEmpty(Print_person_name_join))
                            {
                                Printed_date_join = row.Cells["printed"].Value.ToString();
                            }
                            if (string.IsNullOrEmpty(row.Cells["m1"].Value.ToString()) || Print_M1 == "Null")
                            {
                                Print_M1 = " ";
                            }
                            if (string.IsNullOrEmpty(row.Cells["m2"].Value.ToString()) || Print_M2 == "Null")
                            {
                                Print_M2 = " ";
                            }
                            if (string.IsNullOrEmpty(row.Cells["m3"].Value.ToString()) || Print_M3 == "Null")
                            {
                                Print_M3 = " ";
                            }
                            if (string.IsNullOrEmpty(row.Cells["m4"].Value.ToString()) || Print_M4 == "Null")
                            {
                                Print_M4 = " ";
                            }

                            if (Print_label_type == "2")
                            {
                                Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();
                                Print_date_manfdt = row.Cells["manufacturing_date"].Value.ToString();                                
                                string lot_qty = string.Empty;
                                string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                                if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                                {
                                    int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                    lot_qty = String.Format("{0:n0}", convert_int);

                                }
                                string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                                string additional_code = row.Cells["additional_code"].Value.ToString();
                                grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                    Print_date_expiry, lot_qty, additional_code, Print_date_manfdt, pk_lotinfo_id,
                                    Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                            }
                            else if (Print_label_type == "1")
                            {
                                Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();                                
                                string lot_qty = string.Empty;
                                string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                                if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                                {
                                    int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                    lot_qty = String.Format("{0:n0}", convert_int);

                                }
                                string manufacturing_date = row.Cells["manufacturing_date"].Value.ToString();
                                string additional_code = row.Cells["additional_code"].Value.ToString();
                                string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                                grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                    Print_date_expiry, lot_qty, additional_code, manufacturing_date, pk_lotinfo_id,
                                    Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                            }
                            else if (Print_label_type == "3")
                            {
                                Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();                             
                                string lot_qty = string.Empty;
                                string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                                if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                                {
                                    int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                    lot_qty = String.Format("{0:n0}", convert_int);

                                }
                                string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                                string additional_code = row.Cells["additional_code"].Value.ToString();
                                grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                    Print_date_expiry, lot_qty, additional_code, Print_date_manfdt, pk_lotinfo_id,
                                    Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                            }
                            else if (Print_label_type == "4")
                            {
                                Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();                            
                                string lot_qty = string.Empty;
                                string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                                if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                                {
                                    int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                    lot_qty = String.Format("{0:n0}", convert_int);

                                }
                                string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                                string additional_code = row.Cells["additional_code"].Value.ToString();                             

                                grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                    Print_date_expiry, lot_qty, additional_code, Print_date_manfdt, pk_lotinfo_id,
                                    Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                            }
                            else if (Print_label_type == "5")
                            {
                                Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();
                                string lot_qty = string.Empty;
                                string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                                if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                                {
                                    int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                    lot_qty = String.Format("{0:n0}", convert_int);

                                }
                                string manufacturing_date = row.Cells["manufacturing_date"].Value.ToString();
                                string additional_code = row.Cells["additional_code"].Value.ToString();
                                string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                                grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                    Print_date_expiry, lot_qty, additional_code, manufacturing_date, pk_lotinfo_id,
                                    Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                            }
                            btnPrintlblSelectall.Text = "Un-Select All";
                        }

                    }
                    else if (!Convert.ToBoolean(chk.Value))
                    {
                        if (Print_label_type == "2")
                        {
                            if (CommonClass.list_bar1code.Count > 0)
                            {
                                CommonClass.list_bar1code.RemoveAll(x => x.barcode_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_bar1code.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "1")
                        {
                            if (CommonClass.list_qrcode.Count > 0)
                            {
                                CommonClass.list_qrcode.RemoveAll(x => x.qr_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_qrcode.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "3")
                        {
                            if (CommonClass.list_bar1code.Count > 0)
                            {
                                CommonClass.list_bar1code.RemoveAll(x => x.barcode_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_bar1code.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "4")
                        {
                            if (CommonClass.list_bar1code.Count > 0)
                            {
                                CommonClass.list_bar1code.RemoveAll(x => x.barcode_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_bar1code.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "5")
                        {
                            if (CommonClass.list_qrcode.Count > 0)
                            {
                                CommonClass.list_qrcode.RemoveAll(x => x.qr_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_qrcode.Distinct().ToList();
                            }
                        }
                        btnPrintlblSelectall.Text = "Select All";
                    }
                    rowIndex++;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnPrintlblSelectall_Click", ex);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView2.RefreshEdit();


            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("process_id_exist_check", ex);
            }


        }
        public void Qrcode_typ2_print()
        {
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
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

                    if (cell != null && !cell.ReadOnly)
                    {
                        if (Convert.ToBoolean(cell.Value))
                        {
                            cell.Value = false;
                            //   cell.Value = cell.Value == null || !((bool)cell.Value);
                            this.dataGridView2.RefreshEdit();
                            this.dataGridView2.NotifyCurrentCellDirty(false);
                        }
                        else if (!Convert.ToBoolean(cell.Value))
                        {
                            cell.Value = true;
                            //cell.Value = cell.Value == null || !((bool)cell.Value);
                            this.dataGridView2.RefreshEdit();
                            this.dataGridView2.NotifyCurrentCellDirty(true);
                        }

                    }
                    ///
                    if (Convert.ToBoolean(cell.Value))
                    {
                        Print_lotno = row.Cells["Lotno"].Value.ToString();
                        Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();
                        Print_material_code = row.Cells["additional_code"].Value.ToString();
                        Print_M1 = row.Cells["m1"].Value.ToString();
                        Print_M2 = row.Cells["m2"].Value.ToString();
                        Print_M3 = row.Cells["m3"].Value.ToString();
                        Print_M4 = row.Cells["m4"].Value.ToString();
                        Print_date_manfdt = row.Cells["manufacturing_date"].Value.ToString();
                        Print_person_name_join = row.Cells["PrintPersonjoin"].Value.ToString();
                        Printed_date_join = row.Cells["printdatejoin"].Value.ToString();
                        Print_date_old_colm = row.Cells["printed"].Value.ToString();
                        Print_Item_code = row.Cells["itemcode"].Value.ToString();
                        Print_copy_join = row.Cells["printcpjoin"].Value.ToString();
                        //030423

                        if (string.IsNullOrEmpty(row.Cells["m1"].Value.ToString()) || Print_M1 == "Null")
                        {
                            Print_M1 = " ";
                        }
                        if (string.IsNullOrEmpty(row.Cells["m2"].Value.ToString()) || Print_M2 == "Null")
                        {
                            Print_M2 = " ";
                        }
                        if (string.IsNullOrEmpty(row.Cells["m3"].Value.ToString()) || Print_M3 == "Null")
                        {
                            Print_M3 = " ";
                        }
                        if (string.IsNullOrEmpty(row.Cells["m4"].Value.ToString()) || Print_M4 == "Null")
                        {
                            Print_M4 = " ";
                        }
                       
                        if (Print_label_type == "2")
                        {
                            Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();                         
                            string lot_qty = string.Empty;
                            string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                            if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                            {
                                int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                lot_qty = String.Format("{0:n0}", convert_int);
                            }
                            string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                            string additional_code = row.Cells["additional_code"].Value.ToString();
                            grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                      Print_date_expiry, lot_qty, additional_code, Print_date_manfdt, pk_lotinfo_id,
                                      Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                        }
                        else if (Print_label_type == "1")
                        {
                            Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();                          
                            string lot_qty = string.Empty;
                            string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                            if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                            {
                                int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                lot_qty = String.Format("{0:n0}", convert_int);

                            }
                            string manufacturing_date = row.Cells["manufacturing_date"].Value.ToString();
                            string additional_code = row.Cells["additional_code"].Value.ToString();
                            string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                            grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                      Print_date_expiry, lot_qty, additional_code, manufacturing_date, pk_lotinfo_id,
                                      Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);
                        }
                        else if (Print_label_type == "3")
                        {
                            Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();
                            string lot_qty = string.Empty;
                            string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                            if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                            {
                                int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                lot_qty = String.Format("{0:n0}", convert_int);
                            }
                            string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                            string additional_code = row.Cells["additional_code"].Value.ToString();
                            grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                     Print_date_expiry, lot_qty, additional_code, Print_date_manfdt, pk_lotinfo_id,
                                     Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);

                        }
                        else if (Print_label_type == "4")
                        {
                            string imageUrl_barcode_1 = null;
                            string imageUrl_barcode_2 = null;                         
                            string lot_qty = string.Empty;
                            string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                            if(lot_qty_nocomma!=string.Empty && lot_qty_nocomma != "-")
                            {
                                int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                lot_qty = String.Format("{0:n0}", convert_int);

                            }
                            string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                            // BAR CODE
                            PictureBox bar_pcbox = new PictureBox();
                            PictureBox pictureBox_barcode_2 = new PictureBox();
                            bar_pcbox.SizeMode = PictureBoxSizeMode.CenterImage;
                            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                            string divided_by_boxqty = string.Empty;
                            if (lot_qty != string.Empty)
                            {
                                string comma_replace = lot_qty.Replace(",", "");
                                Decimal convert_boxqty = Convert.ToInt32(comma_replace);
                                Decimal dividedby = convert_boxqty / 1000;
                                Decimal dividedby_roundoff = Math.Round((Decimal)dividedby, 2, MidpointRounding.AwayFromZero);
                                divided_by_boxqty = Convert.ToString(dividedby_roundoff);
                            }
                            string auto_generate_formula_1 = "*(3N)1" + " " + Print_Item_code + " " + divided_by_boxqty + "*";                            
                            if (Print_material_code == "-")
                            {
                                Print_material_code = string.Empty;
                            }
                            string auto_generate_formula_2 = "*(3N)2" + " " + Print_lotno + " " + Print_material_code + "*";
                            bar_pcbox.Image = barcode.Draw(auto_generate_formula_1, 200);
                            pictureBox_barcode_2.Image = barcode.Draw(auto_generate_formula_2, 200);
                            string remove_star_auto_generate_formula_1 = "(3N)1" + " " + Print_Item_code + " " + divided_by_boxqty;
                            string remove_star_auto_generate_formula_2 = "(3N)2" + " " + Print_lotno + " " + Print_material_code;
                            System.Drawing.Image img1 = barcode.Draw(remove_star_auto_generate_formula_1, 200);
                            System.Drawing.Image img2 = barcode.Draw(remove_star_auto_generate_formula_2, 200);                          
                            Random rnd = new Random();
                            int num = rnd.Next();
                            string path_bar_1 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                            bar_pcbox.Image.Save(
                                path_bar_1 + "\\" + "scanedbar_1" + ".jpg",
                                ImageFormat.Jpeg);

                            imageUrl_barcode_1 = path_bar_1 + "\\" + "scanedbar_1" + num + ".jpg";

                            // barcode 2
                            string path_bar_2 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                            pictureBox_barcode_2.Image.Save(
                                path_bar_2 + "\\" + "scanedbar_2" + ".jpg",
                                ImageFormat.Jpeg);

                            imageUrl_barcode_2 = path_bar_2 + "\\" + "scanedbar_2" + num + ".jpg";
                            MemoryStream ms2 = new MemoryStream();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img1.Save(ms, ImageFormat.Png);
                                img2.Save(ms2, ImageFormat.Png);
                                barcode1_details bar1_model = new barcode1_details();
                                bar1_model.barcode_companyname = Print_customer_name;
                                bar1_model.barcode_partno = Print_Item_code;
                                bar1_model.barcode_partname = Print_Item_name;                                
                                bar1_model.barcode_expiry = row.Cells["manufacturing_date"].Value.ToString();                     
                                bar1_model.barcode_qty = divided_by_boxqty;
                                bar1_model.barcode_pcs = Print_Qty;
                                bar1_model.barcode_lotno = Print_lotno;
                                bar1_model.barcode_materialcode = row.Cells["additional_code"].Value.ToString();
                                bar1_model.barcode_input_1 = ms.ToArray();
                                bar1_model.barcode_input_2 = ms2.ToArray();
                                bar1_model.imageUrl_barcode_1 = auto_generate_formula_1;
                                bar1_model.imageUrl_barcode_2 = auto_generate_formula_2;
                                bar1_model.pk_lotinfo_id = pk_lotinfo_id;
                                bar1_model.barcode_m1 = Print_M1;
                                bar1_model.barcode_m2 = Print_M2;
                                bar1_model.barcode_m3 = Print_M3;
                                bar1_model.barcode_m4 = Print_M4;
                                if(!string.IsNullOrEmpty(Printed_date_join) || !string.IsNullOrEmpty(Print_person_name_join))
                                {
                                    // print date assign 
                                    if (string.IsNullOrEmpty(Printed_date_join) || Printed_date_join == "-")
                                    {
                                        bar1_model.printed_date = nowdate.ToString("dd/MM/yyyy");
                                    }
                                    else if (!string.IsNullOrEmpty(Printed_date_join) && Printed_date_join != "-")
                                    {
                                        bar1_model.printed_date = Printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");
                                    }
                                    // print person name assign
                                    if (!string.IsNullOrEmpty(Print_person_name_join))
                                    {
                                        bar1_model.print_person_name = Print_person_name_join;
                                    }
                                }
                                else if(string.IsNullOrEmpty(Printed_date_join) && string.IsNullOrEmpty(Print_person_name_join))
                                {
                                    if(!string.IsNullOrEmpty(Print_date_old_colm))
                                    {
                                        Printed_date_join = Print_date_old_colm;
                                        // print date assign 
                                        if (string.IsNullOrEmpty(Printed_date_join) || Printed_date_join == "-")
                                        {
                                            bar1_model.printed_date = nowdate.ToString("dd/MM/yyyy");
                                        }
                                        else if (!string.IsNullOrEmpty(Printed_date_join) && Printed_date_join != "-")
                                        {
                                            bar1_model.printed_date = Printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");
                                        }
                                        // print person name assign
                                        if (!string.IsNullOrEmpty(Print_person_name_join))
                                        {
                                            bar1_model.print_person_name = Print_person_name_join;
                                        }
                                    }
                                    
                                }
                                // print copy assign
                                if (!string.IsNullOrEmpty(Print_copy_join))
                                {
                                    bar1_model.print_copy = Print_copy_join;
                                }
                                CommonClass.list_bar1code.Add(bar1_model);
                            }
                        }
                        else if (Print_label_type == "5")
                        {
                            Print_date_expiry = row.Cells["expairy_dt"].Value.ToString();
                            string lot_qty = string.Empty;
                            string lot_qty_nocomma = row.Cells["lotqty"].Value.ToString();
                            if (lot_qty_nocomma != string.Empty && lot_qty_nocomma != "-")
                            {
                                int convert_int = Convert.ToInt32(lot_qty_nocomma);
                                lot_qty = String.Format("{0:n0}", convert_int);

                            }
                            string pk_lotinfo_id = row.Cells["idproduction_input_master"].Value.ToString();
                            string additional_code = row.Cells["additional_code"].Value.ToString();
                            grid_checkbox_checked(Print_lotno, Print_date_expiry, Print_material_code,
                                     Print_date_expiry, lot_qty, additional_code, Print_date_manfdt, pk_lotinfo_id,
                                     Print_M1, Print_M2, Print_M3, Print_M4, Printed_date_join, Print_person_name_join, Print_Item_code, Print_copy_join);

                        }
                        btnPrintlblSelectall.Text = "Un-Select All";
                    }
                    else if (!Convert.ToBoolean(cell.Value))
                    {
                        if (Print_label_type == "2")
                        {
                            if (CommonClass.list_bar1code.Count > 0)
                            {
                                CommonClass.list_bar1code.RemoveAll(x => x.barcode_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_bar1code.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "1")
                        {
                            if (CommonClass.list_qrcode.Count > 0)
                            {
                                CommonClass.list_qrcode.RemoveAll(x => x.qr_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_qrcode.Distinct().ToList();                                
                            }
                        }
                        else if (Print_label_type == "3")
                        {
                            if (CommonClass.list_bar1code.Count > 0)
                            {
                                CommonClass.list_bar1code.RemoveAll(x => x.barcode_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_bar1code.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "4")
                        {
                            if (CommonClass.list_bar1code.Count > 0)
                            {
                                CommonClass.list_bar1code.RemoveAll(x => x.barcode_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_bar1code.Distinct().ToList();
                            }
                        }
                        else if (Print_label_type == "5")
                        {
                            if (CommonClass.list_qrcode.Count > 0)
                            {
                                CommonClass.list_qrcode.RemoveAll(x => x.qr_lotno == row.Cells[0].Value.ToString());
                                CommonClass.list_qrcode.Distinct().ToList();
                            }
                        }
                        btnPrintlblSelectall.Text = "Select All";
                    }
                }
                dataGridView2.RefreshEdit();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("dataGridView2_CellClick", ex);
            }

        }
        public void grid_checkbox_checked(string Print_lotno, string Print_date_expiry,
            string Print_material_code, string expairy_dt, string box_qty, string additional_code,
            string manufacturing_date, string pk_lotinfo_id, string print_m1, string print_m2, string print_m3, string print_m4,string printed_date_join, string Print_person_name_join,string Item_code, string Print_copy_join)
        {
            // lot no format change 
            string lotno_spl = Print_lotno.Split('-')[0].ToString();
            string lotno_spl_chld = Print_lotno.Split('-')[1].ToString();
            int convert_lotno = Convert.ToInt32(lotno_spl);
            int convert_lotnochld = Convert.ToInt32(lotno_spl_chld);
            string lotno_format = convert_lotno.ToString("D7");
            string lotnochld_format = convert_lotnochld.ToString("D2");
            Print_lotno = lotno_format + "-" + lotnochld_format;
            //
            if (Print_label_type == "2")
            {
                string imageUrl_barcode_1 = null;
                string imageUrl_barcode_2 = null;
                // BAR CODE
                PictureBox bar_pcbox = new PictureBox();
                PictureBox pictureBox_barcode_2 = new PictureBox();
                bar_pcbox.SizeMode = PictureBoxSizeMode.CenterImage;
                Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                // result image 
                //string auto_generate_formula_1 = "*3N1" + " " + Print_Item_code + " " + expairy_dt + "*";
                string bqty = box_qty.Replace(",", "");
                string auto_generate_formula_1 = "*3N1" + Print_Item_code + " " + bqty + "*";
                if(Print_material_code=="-")
                {
                    Print_material_code = string.Empty;
                }
                string auto_generate_formula_2 = "*3N2" + " " + Print_lotno + " " + Print_material_code + "*";
                bar_pcbox.Image = barcode.Draw(auto_generate_formula_1, 200);
                pictureBox_barcode_2.Image = barcode.Draw(auto_generate_formula_2, 200);

                //
                string remove_star_auto_generate_formula_1 = "3N1" + Print_Item_code + " " + bqty;
                string remove_star_auto_generate_formula_2 = "3N2" + " " + Print_lotno + " " + Print_material_code;
                System.Drawing.Image img1 = barcode.Draw(remove_star_auto_generate_formula_1, 200);
                System.Drawing.Image img2 = barcode.Draw(remove_star_auto_generate_formula_2, 200);
                //
                Random rnd = new Random();
                int num = rnd.Next();
                string path_bar_1 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                bar_pcbox.Image.Save(
                    path_bar_1 + "\\" + "scanedbar_1" + ".jpg",
                    ImageFormat.Jpeg);

                imageUrl_barcode_1 = path_bar_1 + "\\" + "scanedbar_1" + num + ".jpg";

                // barcode 2
                string path_bar_2 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                pictureBox_barcode_2.Image.Save(
                    path_bar_2 + "\\" + "scanedbar_2" + ".jpg",
                    ImageFormat.Jpeg);

                imageUrl_barcode_2 = path_bar_2 + "\\" + "scanedbar_2" + num + ".jpg";
                MemoryStream ms2 = new MemoryStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    img1.Save(ms, ImageFormat.Png);
                    img2.Save(ms2, ImageFormat.Png);
                    barcode1_details bar1_model = new barcode1_details();
                    bar1_model.barcode_companyname = Print_customer_name;
                    bar1_model.barcode_partno = Print_Item_code;
                    bar1_model.barcode_partname = Print_Item_name;
                    //bar1_model.barcode_expiry = expairy_dt;
                    bar1_model.barcode_expiry = manufacturing_date;
                    bar1_model.barcode_qty = box_qty;
                    bar1_model.barcode_pcs = Print_Qty;
                    bar1_model.barcode_lotno = Print_lotno;
                    bar1_model.barcode_materialcode = additional_code;
                    bar1_model.barcode_input_1 = ms.ToArray();
                    bar1_model.barcode_input_2 = ms2.ToArray();
                    bar1_model.imageUrl_barcode_1 = auto_generate_formula_1;
                    bar1_model.imageUrl_barcode_2 = auto_generate_formula_2;
                    bar1_model.pk_lotinfo_id = pk_lotinfo_id;
                    bar1_model.barcode_m1 = print_m1;
                    bar1_model.barcode_m2 = print_m2;
                    bar1_model.barcode_m3 = print_m3;
                    bar1_model.barcode_m4 = print_m4;
                    // print date assign 
                    if (string.IsNullOrEmpty(printed_date_join) || printed_date_join == "-")
                    {
                        bar1_model.printed_date = nowdate.ToString("dd/MM/yyyy");
                    }
                    else if (!string.IsNullOrEmpty(printed_date_join) && printed_date_join != "-")
                    {
                        bar1_model.printed_date = printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");
                    }
                    // print person name assign
                    if (!string.IsNullOrEmpty(Print_person_name_join))
                    {
                        bar1_model.print_person_name = Print_person_name_join;
                    }
                    // print copy assign
                    if (!string.IsNullOrEmpty(Print_copy_join))
                    {
                        bar1_model.print_copy = Print_copy_join;
                    }
                    CommonClass.list_bar1code.Add(bar1_model);
                }
            }
            else if (Print_label_type == "1")
            {
                // QR code
                // result image
                string print_date_remove_spl_char = string.Empty;
                if (Print_date_expiry!=string.Empty)
                {
                    DateTime changeformate = Convert.ToDateTime(Print_date_expiry);
                    print_date_remove_spl_char = changeformate.ToString("yyMMdd");
                }
                //string auto_generate_formula = "1P" + Print_Item_code + "9K" + Print_lotno + " " + Print_material_code + "D" + Print_date_expiry;
                string auto_generate_formula = "1P" + Print_Item_code + "9K" + Print_lotno + " " + Print_material_code + "D" + print_date_remove_spl_char;
                string show_qr_code = auto_generate_formula;
                PictureBox qr_pcbox = new PictureBox();
                Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                // result image 
                System.Drawing.Image img = qrcode.Draw(auto_generate_formula, 200);
                qr_pcbox.Image = qrcode.Draw(auto_generate_formula, 200);
                Random rnd = new Random();
                int num = rnd.Next();

                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Png);

                    qrcode_details qr_model = new qrcode_details();
                    qr_model.qr_companyname = Print_customer_name;
                    qr_model.qr_partno = Print_Item_code;
                    qr_model.qr_partname = Print_Item_name;
                    qr_model.qr_manf = manufacturing_date;
                    qr_model.qr_expiry = expairy_dt;
                    qr_model.qr_qty = box_qty;
                    qr_model.qr_pcs = Print_Qty;
                    qr_model.qr_lotno = Print_lotno;
                    qr_model.qr_materialcode = additional_code;
                    qr_model.qr_imageurl = ms.ToArray();
                    qr_model.pk_lotinfo_id = pk_lotinfo_id;
                    qr_model.qr_m1 = print_m1;
                    qr_model.qr_m2 = print_m2;
                    qr_model.qr_m3 = print_m3;
                    qr_model.qr_m4 = print_m4;
                    // print date assign 
                    if (string.IsNullOrEmpty(printed_date_join) || printed_date_join == "-")
                    {
                        qr_model.printed_date = nowdate.ToString("dd/MM/yyyy");
                    }
                    else if (!string.IsNullOrEmpty(printed_date_join) && printed_date_join != "-")
                    {
                        qr_model.printed_date = printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");
                    }
                    // print person name assign
                    if (!string.IsNullOrEmpty(Print_person_name_join))
                    {
                        qr_model.print_person_name = Print_person_name_join;
                    }
                    // print copy assign
                    if (!string.IsNullOrEmpty(Print_copy_join))
                    {
                        qr_model.print_copy = Print_copy_join;
                    }
                    CommonClass.list_qrcode.Add(qr_model);
                }
            }
            else if (Print_label_type == "3")
            {
                string imageUrl_barcode_1 = null;
                string imageUrl_barcode_2 = null;
                // BAR CODE
                PictureBox bar_pcbox = new PictureBox();
                PictureBox pictureBox_barcode_2 = new PictureBox();
                bar_pcbox.SizeMode = PictureBoxSizeMode.CenterImage;
                Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                // result image 
                //string auto_generate_formula_1 = "*3N1" + " " + Print_Item_code + " " + expairy_dt + "*";
                string bqty = box_qty.Replace(",", "");
                string auto_generate_formula_1 = "*3N1" + Print_Item_code + " " + bqty + "*";
                if (Print_material_code == "-")
                {
                    Print_material_code = string.Empty;
                }
                string auto_generate_formula_2 = "*3N2" + " " + Print_lotno + " " + Print_material_code + "*";
                bar_pcbox.Image = barcode.Draw(auto_generate_formula_1, 200);
                pictureBox_barcode_2.Image = barcode.Draw(auto_generate_formula_2, 200);

                //
                string remove_auto_generate_formula_1 = "3N1" + Print_Item_code + " " + bqty;
                string remove_auto_generate_formula_2 = "3N2" + " " + Print_lotno + " " + Print_material_code;
                System.Drawing.Image img1 = barcode.Draw(remove_auto_generate_formula_1, 200);
                System.Drawing.Image img2 = barcode.Draw(remove_auto_generate_formula_2, 200);
                //
                Random rnd = new Random();
                int num = rnd.Next();
                string path_bar_1 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                bar_pcbox.Image.Save(
                    path_bar_1 + "\\" + "scanedbar_1" + ".jpg",
                    ImageFormat.Jpeg);

                imageUrl_barcode_1 = path_bar_1 + "\\" + "scanedbar_1" + num + ".jpg";

                // barcode 2
                string path_bar_2 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                pictureBox_barcode_2.Image.Save(
                    path_bar_2 + "\\" + "scanedbar_2" + ".jpg",
                    ImageFormat.Jpeg);

                imageUrl_barcode_2 = path_bar_2 + "\\" + "scanedbar_2" + num + ".jpg";
                MemoryStream ms2 = new MemoryStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    img1.Save(ms, ImageFormat.Png);
                    img2.Save(ms2, ImageFormat.Png);
                    barcode1_details bar1_model = new barcode1_details();
                    bar1_model.barcode_companyname = Print_customer_name;
                    bar1_model.barcode_partno = Print_Item_code;
                    bar1_model.barcode_partname = Print_Item_name;
                    //bar1_model.barcode_expiry = expairy_dt;
                    bar1_model.barcode_expiry = manufacturing_date;
                    bar1_model.barcode_qty = box_qty;
                    bar1_model.barcode_pcs = Print_Qty;
                    bar1_model.barcode_lotno = Print_lotno;
                    bar1_model.barcode_materialcode = additional_code;
                    bar1_model.barcode_input_1 = ms.ToArray();
                    bar1_model.barcode_input_2 = ms2.ToArray();
                    bar1_model.imageUrl_barcode_1 = auto_generate_formula_1;
                    bar1_model.imageUrl_barcode_2 = auto_generate_formula_2;
                    bar1_model.pk_lotinfo_id = pk_lotinfo_id;
                    bar1_model.barcode_m1 = print_m1;
                    bar1_model.barcode_m2 = print_m2;
                    bar1_model.barcode_m3 = print_m3;
                    bar1_model.barcode_m4 = print_m4;
                    // print date assign 
                    if (string.IsNullOrEmpty(printed_date_join) || printed_date_join == "-")
                    {
                        bar1_model.printed_date = nowdate.ToString("dd/MM/yyyy");
                    }
                    else if (!string.IsNullOrEmpty(printed_date_join) && printed_date_join != "-")
                    {
                        bar1_model.printed_date = printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");
                    }
                    // print person name assign
                    if (!string.IsNullOrEmpty(Print_person_name_join))
                    {
                        bar1_model.print_person_name = Print_person_name_join;
                    }
                    // print copy assign
                    if (!string.IsNullOrEmpty(Print_copy_join))
                    {
                        bar1_model.print_copy = Print_copy_join;
                    }
                    CommonClass.list_bar1code.Add(bar1_model);
                }
            }
            else if (Print_label_type == "4")
            {
                string imageUrl_barcode_1 = null;
                string imageUrl_barcode_2 = null;
                string divided_by_boxqty = string.Empty;
                // BAR CODE
                PictureBox bar_pcbox = new PictureBox();
                PictureBox pictureBox_barcode_2 = new PictureBox();
                bar_pcbox.SizeMode = PictureBoxSizeMode.CenterImage;
                Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                  
                if (box_qty != string.Empty)
                {
                    string comma_replace = box_qty.Replace(",", "");       
                    Decimal convert_boxqty = Convert.ToInt32(comma_replace);
                    Decimal dividedby = convert_boxqty / 1000;
                    Decimal dividedby_roundoff = Math.Round((Decimal)dividedby, 2, MidpointRounding.AwayFromZero);
                    divided_by_boxqty =Convert.ToString(dividedby_roundoff);
                }
                // result image 
                //string auto_generate_formula_1 = "*3N1" + " " + Print_Item_code + " " + expairy_dt + "*";
                // 24032023 no need ( ) again 
                //string auto_generate_formula_1 = "*(3N)1" + " " + Print_Item_code + " " + divided_by_boxqty + "*";
                string auto_generate_formula_1 = "*3N1" + " " + Print_Item_code + " " + divided_by_boxqty + "*";

                if (Print_material_code == "-")
                {
                    Print_material_code = string.Empty;
                }
                //string auto_generate_formula_2 = "*3N2" + " " + Print_lotno + " " + Print_material_code + "*";
                // 24032023 no need ( ) again 
                //string auto_generate_formula_2 = "*(3N)2" + " " + Print_lotno + " " + Print_material_code + "*";
                string auto_generate_formula_2 = "*3N2" + " " + Print_lotno + " " + Print_material_code + "*";
                bar_pcbox.Image = barcode.Draw(auto_generate_formula_1, 200);
                pictureBox_barcode_2.Image = barcode.Draw(auto_generate_formula_2, 200);

                // 24032023 no need ( ) again 
                //string remove_star_auto_generate_formula_1 = "(3N)1" + " " + Print_Item_code + " " + divided_by_boxqty;
                // string remove_star_auto_generate_formula_2 = "(3N)2" + " " + Print_lotno + " " + Print_material_code;
                string remove_star_auto_generate_formula_1 = "3N1" + " " + Print_Item_code + " " + divided_by_boxqty;
                string remove_star_auto_generate_formula_2 = "3N2" + " " + Print_lotno + " " + Print_material_code;

                System.Drawing.Image img1 = barcode.Draw(remove_star_auto_generate_formula_1, 200);
                System.Drawing.Image img2 = barcode.Draw(remove_star_auto_generate_formula_2, 200);
                //
                Random rnd = new Random();
                int num = rnd.Next();
                string path_bar_1 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                bar_pcbox.Image.Save(
                    path_bar_1 + "\\" + "scanedbar_1" + ".jpg",
                    ImageFormat.Jpeg);

                imageUrl_barcode_1 = path_bar_1 + "\\" + "scanedbar_1" + num + ".jpg";

                // barcode 2
                string path_bar_2 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                pictureBox_barcode_2.Image.Save(
                    path_bar_2 + "\\" + "scanedbar_2" + ".jpg",
                    ImageFormat.Jpeg);

                imageUrl_barcode_2 = path_bar_2 + "\\" + "scanedbar_2" + num + ".jpg";
                MemoryStream ms2 = new MemoryStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    img1.Save(ms, ImageFormat.Png);
                    img2.Save(ms2, ImageFormat.Png);
                    barcode1_details bar1_model = new barcode1_details();
                    bar1_model.barcode_companyname = Print_customer_name;
                    bar1_model.barcode_partno = Print_Item_code;
                    bar1_model.barcode_partname = Print_Item_name;
                    //bar1_model.barcode_expiry = expairy_dt;
                    bar1_model.barcode_expiry = manufacturing_date;
                    //bar1_model.barcode_qty = box_qty;
                    bar1_model.barcode_qty = divided_by_boxqty;
                    bar1_model.barcode_pcs = Print_Qty;
                    bar1_model.barcode_lotno = Print_lotno;
                    bar1_model.barcode_materialcode = additional_code;
                    bar1_model.barcode_input_1 = ms.ToArray();
                    bar1_model.barcode_input_2 = ms2.ToArray();
                    bar1_model.imageUrl_barcode_1 = auto_generate_formula_1;
                    bar1_model.imageUrl_barcode_2 = auto_generate_formula_2;
                    bar1_model.pk_lotinfo_id = pk_lotinfo_id;
                    bar1_model.barcode_m1 = print_m1;
                    bar1_model.barcode_m2 = print_m2;
                    bar1_model.barcode_m3 = print_m3;
                    bar1_model.barcode_m4 = print_m4;
                    // print date assign 
                    if(string.IsNullOrEmpty(printed_date_join) || printed_date_join == "-")
                    {
                        bar1_model.printed_date = nowdate.ToString("dd/MM/yyyy"); 
                    }
                    else if(!string.IsNullOrEmpty(printed_date_join) && printed_date_join != "-")
                    {
                        bar1_model.printed_date = printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");                                               
                    }
                    // print person name assign
                    if (!string.IsNullOrEmpty(Print_person_name_join))
                    {
                        bar1_model.print_person_name = Print_person_name_join;
                    }
                    // print copy assign
                    if (!string.IsNullOrEmpty(Print_copy_join))
                    {
                        bar1_model.print_copy = Print_copy_join;
                    }
                    CommonClass.list_bar1code.Add(bar1_model);
                }

            }
            else if (Print_label_type == "5")
            {
                // QR code
                // result image
                string print_date_remove_spl_char_exp = string.Empty;
                if (Print_date_expiry != string.Empty)
                {
                    DateTime changeformate = Convert.ToDateTime(Print_date_expiry);
                    print_date_remove_spl_char_exp = changeformate.ToString("yyyyMMdd");
                }
                string print_date_remove_spl_char_manf = string.Empty;
                if (manufacturing_date != string.Empty)
                {
                    DateTime changeformate = Convert.ToDateTime(manufacturing_date);
                    print_date_remove_spl_char_manf = changeformate.ToString("yyyyMMdd");
                }
                //string auto_generate_formula = "1P" + Print_Item_code + "9K" + Print_lotno + " " + Print_material_code + "D" + Print_date_expiry;
                //string auto_generate_formula = "1P" + Print_Item_code + "9K" + Print_lotno + " " + Print_material_code + "D" + print_date_remove_spl_char;
                string auto_generate_formula = Print_Item_code + "," + additional_code + "," +Print_Qty + "," +"PCS"+","+ print_date_remove_spl_char_manf + "," + print_date_remove_spl_char_exp + "," + Print_lotno;
                string show_qr_code = auto_generate_formula;
                PictureBox qr_pcbox = new PictureBox();
                Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                // result image 
                System.Drawing.Image img = qrcode.Draw(auto_generate_formula, 200);
                qr_pcbox.Image = qrcode.Draw(auto_generate_formula, 200);
                Random rnd = new Random();
                int num = rnd.Next();

                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Png);

                    qrcode_details qr_model = new qrcode_details();
                    qr_model.qr_companyname = Print_customer_name;
                    qr_model.qr_partno = Print_Item_code;
                    qr_model.qr_partname = Print_Item_name;
                    qr_model.qr_manf = manufacturing_date;
                    qr_model.qr_expiry = expairy_dt;
                    qr_model.qr_qty = box_qty;
                    qr_model.qr_pcs = Print_Qty;
                    qr_model.qr_lotno = Print_lotno;
                    qr_model.qr_materialcode = additional_code;
                    qr_model.qr_imageurl = ms.ToArray();
                    qr_model.pk_lotinfo_id = pk_lotinfo_id;
                    qr_model.qr_m1 = print_m1;
                    qr_model.qr_m2 = print_m2;
                    qr_model.qr_m3 = print_m3;
                    qr_model.qr_m4 = print_m4;
                    // print date assign 
                    if (string.IsNullOrEmpty(printed_date_join) || printed_date_join == "-")
                    {
                        qr_model.printed_date = nowdate.ToString("dd/MM/yyyy");
                    }
                    else if (!string.IsNullOrEmpty(printed_date_join) && printed_date_join != "-")
                    {
                        qr_model.printed_date = printed_date_join + "," + nowdate.ToString("dd/MM/yyyy");
                    }
                    // print person name assign
                    if (!string.IsNullOrEmpty(Print_person_name_join))
                    {
                        qr_model.print_person_name = Print_person_name_join;
                    }
                    // print copy assign
                    if (!string.IsNullOrEmpty(Print_copy_join))
                    {
                        qr_model.print_copy = Print_copy_join;
                    }
                    CommonClass.list_qrcode.Add(qr_model);
                }
            }
        }
      
        public void print_return()
        {
            try
            {
                DataTable dt = new DataTable();
                dataGridView2.DataSource = dt;
                printLable_gridbind();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("print_return", ex);
            }
        }
        public void color_change_dynamic_button(string lotno, string lotno_child)
        {
            int i = 10;
            int x = -1;
            panel1.Controls.Clear();
            int total_process = CommonClass.Process_name.Count;

            foreach (var itm in CommonClass.Process_name)
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
                // insert time 
                if (!CommonClass.view_enable)
                {
                    // Production information tab : selected partnumber only button create
                    if (selected_dgProduct_partnumber == getid)
                    {
                        //This block dynamically creates a Button and adds it to the form
                        Button btn = new Button();
                        btn.BackColor = back_clr;
                        btn.ForeColor = fore_clr;
                        btn.Location = new System.Drawing.Point(19, 29);
                        btn.Name = itm.id + "#" + itm.PaternType + "#" + itm.ProcessNames + "#" + itm.process_id + "#" + itm.itemcode + "#" + itm.materialcode;
                        btn.Size = new System.Drawing.Size(80, 60);
                        btn.TabIndex = 103;
                        btn.Text = itm.ProcessNames;
                        btn.UseVisualStyleBackColor = false;
                        btn.Click += new System.EventHandler(this.Patern_Click);
                        btn.Location = new Point(i, x);
                        panel1.AutoScroll = true;
                        panel1.Controls.Add(btn);
                        i += 100;
                    }
                }
                else if (CommonClass.view_enable)
                {
                    // Production information tab : selected partnumber only button create
                    if (getid != "XXX")
                    {
                        //This block dynamically creates a Button and adds it to the form
                        Button btn = new Button();
                        btn.BackColor = back_clr;
                        btn.ForeColor = fore_clr;
                        btn.Location = new System.Drawing.Point(19, 29);
                        btn.Name = itm.id + "#" + itm.PaternType + "#" + itm.ProcessNames + "#" + itm.process_id + "#" + itm.itemcode + "#" + itm.materialcode;
                        btn.Size = new System.Drawing.Size(80, 60);
                        btn.TabIndex = 103;
                        btn.Text = itm.ProcessNames;
                        btn.UseVisualStyleBackColor = false;
                        btn.Click += new System.EventHandler(this.Patern_Click);
                        btn.Location = new Point(i, x);
                        panel1.AutoScroll = true;
                        panel1.Controls.Add(btn);
                        i += 100;
                    }
                }

            }
            int findIndexvalue = 0;
            foreach (DataGridViewRow chk in dataGridView1.Rows)
            {
                string joinLotno = textLotNoAdd.Text + "-" + textLotNoChild.Text;
                string current_headertext = chk.HeaderCell.Value.ToString();
                if (joinLotno == current_headertext)
                {
                    dataGridView1_grid_selectedRow = findIndexvalue;
                    break;
                }
                else
                {
                    dataGridView1_grid_selectedRow = -1;
                }
                findIndexvalue++;
            }
        }

        private void textLotNoChild_TextChanged(object sender, EventArgs e)
        {
            try
            {            
                color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
                lot_information_changed_without_grid = true;
                lotnumber_only_changed_add_pi_tbl = true;
                btn_add_only_lotno.Text = "    Add New Lot                                 [F2]";                
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("textLotNoChild_TextChanged", ex);
            }
        }
        private void chkExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExclude.Checked == false)
            {
                cmbProcess.Enabled = true;
            }
            else if (chkExclude.Checked == true)
            {
                cmbProcess.Enabled = false;
            }
        }
        private void FormProductionInput_Activated(object sender, EventArgs e)
        {
        }
        public void dataGridView1_selected_items(int rowindex, string patternname, string process_id, int pattern_type, string material_cd)
        {
            try
            {
                if (rowindex != -1)
                {
                    DataGridViewRow row = dataGridView1.Rows[rowindex];
                    DataTable drp = helper.ProcessList();
                    int columun_count_v = 0;
                    foreach (var itm in CommonClass.Process_name_gridbind_columns)
                    {
                        if (itm.process_id == process_id && itm.materialcode == material_cd)
                        {
                            break;
                        }
                        else
                        {
                            if (itm.PaternType == "1")
                            {
                                columun_count_v = columun_count_v + 5;
                            }
                            else if (itm.PaternType == "2")
                            {
                                columun_count_v = columun_count_v + 4;
                            }
                            else if (itm.PaternType == "3")
                            {
                                columun_count_v = columun_count_v + 2;
                            }
                            else if (itm.PaternType == "4")
                            {
                                columun_count_v = columun_count_v + 3;
                            }
                            else if (itm.PaternType == "5")
                            {
                                columun_count_v = columun_count_v + 9;
                            }
                        }

                    }

                    if (pattern_type == 1)
                    {

                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern1_PartNo = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern1_PartNo = "00000";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                             pass_pattern1_LotNo = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern1_LotNo = "0000000";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern1_PlantingDate = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern1_PlantingDate = nowdate.ToString("dd-MM-yyyy");
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern1_Qty = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {                           
                            pass_pattern1_Qty = txt_lotinfo_quantity.Text;
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern1_PbDate = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern1_PbDate = "00";
                        }
                        columun_count_v = 0;
                    }
                    else if (pattern_type == 2)
                    {
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern2_ProcessDate = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern2_ProcessDate = nowdate.ToString("dd-MM-yyyy");
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern2_Controlno = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern2_Controlno = "000";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern2_Sheetlotno = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern2_Sheetlotno = "0000000";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern2_Qty = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            //pass_pattern2_Qty = "0000";
                            pass_pattern2_Qty = txt_lotinfo_quantity.Text;
                        }
                        columun_count_v = 0;
                    }
                    else if (pattern_type == 3)
                    {
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern3_ProcessDate = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            //pass_pattern3_ProcessDate = nowdate.ToString("dd-MM-yyyy");
                            pass_pattern3_ProcessDate = "00";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern3_Qty = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern3_Qty = "0000";                            
                        }
                        columun_count_v = 0;
                    }
                    else if (pattern_type == 4)
                    {
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern4_Lotno = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern4_Lotno = "0000000";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern4_PartNo = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {
                            pass_pattern4_PartNo = "0000000";
                        }
                        columun_count_v = columun_count_v + 1;
                        if (Convert.ToString(row.Cells[columun_count_v].Value) != string.Empty)
                        {
                            pass_pattern4_Qty = row.Cells[columun_count_v].Value.ToString();
                        }
                        else
                        {                          
                            pass_pattern4_Qty = txt_lotinfo_quantity.Text;
                        }
                        columun_count_v = 0;
                    }
                }
                else
                {
                  if(patternname == "INSPECTION" || patternname == "CLEANING")
                    {
                        pass_pattern1_Qty = "0000";
                        pass_pattern2_Qty = "0000";
                        pass_pattern3_Qty = "0000";
                        pass_pattern4_Qty = "0000";
                    }
                    else
                    {
                        pass_pattern1_Qty = txt_lotinfo_quantity.Text;
                        pass_pattern2_Qty = txt_lotinfo_quantity.Text;
                        pass_pattern3_Qty = txt_lotinfo_quantity.Text;
                        pass_pattern4_Qty = txt_lotinfo_quantity.Text;

                    }

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("dataGridView1_selected_items", ex);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[rowIndex];
            // use pattern popup open
            dataGridView1_grid_selectedRow = rowIndex;
            string lotsplit = dataGridView1.CurrentRow.HeaderCell.Value.ToString();
            textLotNoAdd.Text = lotsplit.Split('-')[0];
            textLotNoChild.Text = lotsplit.Split('-')[1];
            /// print label 
            txt_pl_lotno.Text = lotsplit.Split('-')[0];
            txt_pl_to_lotc.Text = lotsplit.Split('-')[1];
            string get_bproduct = string.Empty;
            ///
            if (row.Cells[0].Value != null)
            {
                get_bproduct = row.Cells[0].Value.ToString();
            }
            else
            {
                get_bproduct = string.Empty;
            }
            if(get_bproduct =="B")
            {
                chk_bproduct.Checked=true;
            }
            else
            {
                chk_bproduct.Checked = false;
            }
            ////
            
            string get_onhold = string.Empty;
            if(row.Cells[1].Value != null)
            {
                get_onhold = row.Cells[1].Value.ToString();
            }
            else
            {
                get_onhold = string.Empty;
            }
            if (get_onhold == "H")
            {
                chk_onhold.Checked = true;
            }
            else
            {
                chk_onhold.Checked = false;
            }
            string get_scrap = string.Empty;
            if (row.Cells[2].Value != null)
            {
                get_scrap = row.Cells[2].Value.ToString();
            }
            else
            {
                get_scrap = string.Empty;
            }
            if (get_scrap == "S")
            {
                chkbx_scrap.Checked = true;
            }
            else
            {
                chkbx_scrap.Checked = false;
            }
            if(row.Cells[3].Value!=null)
            {                
                txt_reason_hs.Text = row.Cells[3].Value.ToString();
            }
            else
            {
                txt_reason_hs.Text = string.Empty;
            }
            if (Convert.ToString(row.Cells[5].Value) != string.Empty)
            {
                dateTimePicker_Manf.Value = Convert.ToDateTime(row.Cells[5].Value.ToString(),
                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            }  
            txt_lotinfo_quantity.Text = row.Cells[4].Value.ToString();
            color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
            btn_add_only_lotno.Text = "     Update Lot                              [F2]";

            txt_reason_hs.ForeColor = Color.Black;
            //datagrid view 2 
            DataTable dt = new DataTable();
            dataGridView2.DataSource = dt;
            lot_information_changed_without_grid = false;
            Cursor.Current = Cursors.Default;
        }

        private void dateTimePicker_Manf_ValueChanged(object sender, EventArgs e)
        {
            lot_information_changed_without_grid = true;
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void txt_manf_time_TextChanged(object sender, EventArgs e)
        {
            lot_information_changed_without_grid = true;
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void txt_lotinfo_quantity_TextChanged(object sender, EventArgs e)
        {
            lot_information_changed_without_grid = true;
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void txtCustomerCode_TabIndexChanged(object sender, EventArgs e)
        {
            if (txtCustomerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtCustomerCode.Text);
                txtCustomerCode.Text = formate_type.ToString("D7");
            }
        }

        private void textLotNoAdd_Leave(object sender, EventArgs e)
        {          
            string get_lotnochild = max_id_with_lotnumber_lotonlytbl_leave(txtCustomerCode.Text, textLotNoAdd.Text);
            string get_lotnochild_maintbl = max_lotno_with_lot_maintbl_leave(txtCustomerCode.Text, textLotNoAdd.Text);

            if (get_lotnochild.Split(',')[0]=="0" && get_lotnochild_maintbl.Split(',')[0]=="0")
            {
                textLotNoChild.Text = "01";
            }
            else if (get_lotnochild.Split(',')[0] != "0" && get_lotnochild_maintbl.Split(',')[0] == "0")
            {
                int chk_lotchild_ = Convert.ToInt32(get_lotnochild.Split(',')[0]);          
                textLotNoChild.Text = chk_lotchild_.ToString("D2");
                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                if (result_only_tbl)
                {
                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                }
                else
                {
                    // mani tbl
                    bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                    if (result)
                    {
                        string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                        int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                        textLotNoChild.Text = formate_child_equal.ToString("D2");
                    }
                }
            }
            else if (get_lotnochild.Split(',')[0] == "0" && get_lotnochild_maintbl.Split(',')[0] != "0")
            {
                int chk_lotchild_ = Convert.ToInt32(get_lotnochild_maintbl.Split(',')[0]);               
                textLotNoChild.Text = chk_lotchild_.ToString("D2");
                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                if (result_only_tbl)
                {
                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                }
                else
                {
                    // mani tbl
                    bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                    if (result)
                    {
                        string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                        int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                        textLotNoChild.Text = formate_child_equal.ToString("D2");
                    }
                }
            }
            else if (get_lotnochild.Split(',')[0] != "0" && get_lotnochild_maintbl.Split(',')[0] != "0")
            {
                // convert integer lot number child
                int chk_lotchild = Convert.ToInt32(get_lotnochild.Split(',')[0]);
                int chk_comlotchild = Convert.ToInt32(get_lotnochild_maintbl.Split(',')[0]);              

                DateTime lot_main_tbl = DateTime.Parse(get_lotnochild_maintbl.Split(',')[1]);
                DateTime lot_only_tbl = DateTime.Parse(get_lotnochild.Split(',')[1]);

                // Date compare 
                bool date_equal = DateTime.Equals(lot_only_tbl.Date, lot_main_tbl.Date);
                if (date_equal)
                {
                    // Time compare 
                    int grater_than = TimeSpan.Compare(lot_only_tbl.TimeOfDay, lot_main_tbl.TimeOfDay);
                    if (grater_than > 0)
                    {                        
                        textLotNoChild.Text = chk_lotchild.ToString("D2");
                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                        if (result_only_tbl)
                        {
                            string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                        }
                        else
                        {
                            // mani tbl
                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                            if (result)
                            {
                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                        }
                    }
                    // equal means its go . date and time both are equal 
                    else if (grater_than >= 0)
                    {
                        if (chk_lotchild > chk_comlotchild)
                        {
                            //chk_lotchild = chk_lotchild + 1;
                            textLotNoChild.Text = chk_lotchild.ToString("D2");
                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                            if (result_only_tbl)
                            {
                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                            else
                            {
                                // mani tbl
                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                if (result)
                                {
                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                }
                            }
                        }
                        else
                        {
                            textLotNoChild.Text = chk_comlotchild.ToString("D2");
                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                            if (result)
                            {
                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                            else
                            {
                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                if (result_only_tbl)
                                {
                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                }
                            }
                        }
                    }
                    else
                    {
                        textLotNoChild.Text = chk_comlotchild.ToString("D2");
                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                        if (result)
                        {
                            string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                        }
                        else
                        {
                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                            if (result_only_tbl)
                            {
                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                        }
                    }
                }
                else if (!date_equal)
                {
                    int grater_than = DateTime.Compare(lot_only_tbl.Date, lot_main_tbl.Date);
                    if (grater_than > 0)
                    {
                        //chk_lotchild = chk_lotchild + 1;
                        // 200323
                        textLotNoChild.Text = chk_lotchild.ToString("D2");
                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                        if (result_only_tbl)
                        {
                            string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                        }
                        else
                        {
                            // mani tbl
                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                            if (result)
                            {
                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                        }
                    }
                    // equal means its go . date and time both are equal 
                    else if (grater_than >= 0)
                    {
                        if (chk_lotchild > chk_comlotchild)
                        {
                            //chk_lotchild = chk_lotchild + 1;
                            textLotNoChild.Text = chk_lotchild.ToString("D2");
                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                            if (result_only_tbl)
                            {
                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                            else
                            {
                                // mani tbl
                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                if (result)
                                {
                                    string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                }
                            }
                        }
                        else
                        {
                            textLotNoChild.Text = chk_comlotchild.ToString("D2");
                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                            if (result)
                            {
                                string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                            else
                            {
                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                if (result_only_tbl)
                                {
                                    string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild.Text = formate_child_equal.ToString("D2");
                                }
                            }
                        }
                    }
                    else
                    {
                        textLotNoChild.Text = chk_comlotchild.ToString("D2");
                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                        if (result)
                        {
                            string get_max_lotnumber_child = max_lotno_manitbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotno_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild.Text = formate_child_equal.ToString("D2");
                        }
                        else
                        {
                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                            if (result_only_tbl)
                            {
                                string get_max_lotnumber_child = max_lotno_onlytbl(txtCustomerCode.Text, textItemCode.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild.Text = formate_child_equal.ToString("D2");
                            }
                        }
                    }
                }                
            }  
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
            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
            if (result)
            {
                //btn_add_only_lotno.Text = "     Update Lot                              [F2]";
                MessageBox.Show("Lot Number and Lot Number Child Already Exist..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textLotNoChild.Text = "00";
                textLotNoChild.Focus();
            }
            else if (!result)
            {
                bool result_lotonly = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                if (result_lotonly)
                {
                    MessageBox.Show("Lot Number and Lot Number Child Already Exist..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textLotNoChild.Text = "00";
                    textLotNoChild.Focus();
                }
                else
                {
                    color_change_dynamic_button(textLotNoAdd.Text, textLotNoChild.Text);
                }
            }
            DateTime current_time = DateTime.Now;
            txt_manf_time.Text = current_time.ToString("HH:mm:ss");
            dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
        }

        private void txt_lotinfo_quantity_Leave(object sender, EventArgs e)
        {
        }

        private void textSearchLotNo_Leave(object sender, EventArgs e)
        {
            if (textSearchLotNo.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(textSearchLotNo.Text);
                textSearchLotNo.Text = formate_type.ToString("D7");
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
        public void Get_Item_material_details(string ActionType_all, string customercode)
        {
            try
            {
                string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
                string[] obj = { customercode, string.Empty, textItemCode.Text, ActionType_all };
                DataSet ds = helper.GetDatasetByCommandString("product_view", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = dt;    
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("Get_Item_material_details", ex);
            }
        }

        private void btnproductinfo_down_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Product Information List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (dGProcess.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        List<string> Date_column_names = new List<string>();
                        List<int> Date_column_index = new List<int>();
                        Date_column_names.Add("Lot no");
                        Microsoft.Office.Interop.Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();                        
                        Excel._Workbook oWB;
                        Excel._Worksheet ws;
                        XcelApp.DisplayAlerts = false;
                        oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                        ws = (Excel._Worksheet)oWB.ActiveSheet;
                        int get_date_column = 0;
                        for (int i = 1; i < dGProcess.Columns.Count - 5; i++)
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
                            for (int j = 0; j < dGProcess.Columns.Count - 6; j++)
                            {
                                if (Convert.ToString(dGProcess.Rows[i].Cells[j].Value) != string.Empty)
                                {
                                    // check Lotno column or not 
                                    if (Date_column_index.Contains(j) == false)
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = dGProcess.Rows[i].Cells[j].Value.ToString();

                                    }
                                    else if (Date_column_index.Contains(j) == true)
                                    {
                                        int formate_type = Convert.ToInt32(dGProcess.Rows[i].Cells[j].Value.ToString());
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
                        Excel.Range copyRange_I = XcelApp.Range["I:I"];
                        Excel.Range copyRange_J = XcelApp.Range["J:J"];
                        Excel.Range insertRange_C = XcelApp.Range["C:C"];
                        Excel.Range insertRange_D = XcelApp.Range["D:D"];
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_I.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_J.Cut());
                        // Auto fit automatically adjust the width of columns of Excel  in givien range .  
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess.Rows.Count, dGProcess.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dGProcess.Columns.Count]].Font.Bold = true;
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dGProcess.Columns.Count]].Font.Size = 13;

                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Product Information List -" + datetime;
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
                throw new ArgumentNullException("btnproductinfo_down_Click", ex);
            }
        }

        private void btn_printlbl_dwn_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to Download Shippment List ?", "DOWNLOAD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    DataGridViewRow row = dataGridView2.Rows[0];
                    if (dataGridView2.Rows.Count > 0 && Convert.ToString(row.Cells[0].Value) != string.Empty)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        List<string> Date_column_names = new List<string>();
                        List<int> Date_column_index = new List<int>();
                        Date_column_names.Add("Manufacturing Date");
                        Date_column_names.Add("Expiry Date");
                        Date_column_names.Add("Printed Date");
                        Microsoft.Office.Interop.Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                        Excel._Workbook oWB;
                        Excel._Worksheet ws;
                        XcelApp.DisplayAlerts = false;
                        oWB = (Excel._Workbook)(XcelApp.Workbooks.Add(Missing.Value));
                        ws = (Excel._Worksheet)oWB.ActiveSheet;
                        int get_date_column = 0;
                        for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
                        {
                            if (i != 2)
                            {
                                if (Date_column_names.Contains(dataGridView2.Columns[i - 1].HeaderText) == false)
                                {
                                    XcelApp.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
                                }
                                else if (Date_column_names.Contains(dataGridView2.Columns[i - 1].HeaderText) == true)
                                {
                                    XcelApp.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
                                    Date_column_index.Add(get_date_column);
                                }
                            }
                            get_date_column++;
                        }
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView2.Columns.Count; j++)
                            {
                                if (j != 1)
                                {
                                    if (Convert.ToString(dataGridView2.Rows[i].Cells[j].Value) != string.Empty)
                                    {
                                        // check Date column or not 
                                        if (Date_column_index.Contains(j) == false)
                                        {
                                            XcelApp.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                                        }
                                        else if (Date_column_index.Contains(j) == true && Convert.ToString(dataGridView2.Rows[i].Cells[j].Value) != "-")
                                        {
                                            string date_val = dataGridView2.Rows[i].Cells[j].Value.ToString();
                                            DateTimePicker dt = new DateTimePicker();
                                            dt.Value = Convert.ToDateTime(date_val,
                                            System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                            DateTime convertformateDate = Convert.ToDateTime(date_val.Replace("\"", ""), System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);                                            
                                            XcelApp.Cells[i + 2, j + 1] = convertformateDate;
                                            Excel.Range d1 = ws.Cells[i + 2, j + 1];
                                            Excel.Range d2 = ws.Cells[i + 2, j + 1];
                                            XcelApp.Range[d1, d2].EntireColumn.NumberFormat = "dd-mm-yyyy";
                                        }
                                        else if (Convert.ToString(dataGridView2.Rows[i].Cells[j].Value) == "-")
                                        {
                                            XcelApp.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value;
                                        }
                                    }
                                    else
                                    {
                                        XcelApp.Cells[i + 2, j + 1] = string.Empty;
                                    }
                                }
                            }
                        }
                        for (int i = 1; i <= dataGridView2.Rows.Count+1; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range cel = (Excel.Range)XcelApp.Cells[i, 2];
                            cel.Delete();
                        }
                        Excel.Range copyRange_K = XcelApp.Range["K:K"];
                        Excel.Range copyRange_L = XcelApp.Range["L:L"];
                        Excel.Range copyRange_M = XcelApp.Range["M:M"];
                        Excel.Range copyRange_N = XcelApp.Range["N:N"];
                        Excel.Range copyRange_O = XcelApp.Range["O:O"];
                        Excel.Range copyRange_P = XcelApp.Range["P:P"];
                        Excel.Range copyRange_Q = XcelApp.Range["Q:Q"];
                        Excel.Range copyRange_S = XcelApp.Range["W:W"];
                        Excel.Range copyRange_T = XcelApp.Range["X:X"];
                        Excel.Range insertRange_C = XcelApp.Range["B:B"];
                        Excel.Range insertRange_D = XcelApp.Range["D:D"];
                        Excel.Range insertRange_E = XcelApp.Range["E:E"];
                        Excel.Range DeleteRange_Z = XcelApp.Range["Z:Z"];
                        Excel.Range DeleteRange_AA = XcelApp.Range["AA:AA"];
                        Excel.Range DeleteRange_AB = XcelApp.Range["AB:AB"];                       
                        DeleteRange_AB.Delete();
                        DeleteRange_AA.Delete();
                        DeleteRange_Z.Delete();
                        insertRange_E.Delete(copyRange_K.Delete());
                        insertRange_D.Delete(copyRange_L.Delete());
                  
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_M.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_N.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_O.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_P.Cut());
                        insertRange_C.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, copyRange_Q.Cut());

                        // Auto fit automatically adjust the width of columns of Excel  in givien range .  
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView2.Rows.Count, dataGridView2.Columns.Count]].EntireColumn.AutoFit();
                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[dataGridView2.Columns.Count]].Font.Bold = true;

                        XcelApp.Range[XcelApp.Cells[1, 1], XcelApp.Cells[1, dataGridView2.Columns.Count]].Font.Size = 13;

                        XcelApp.Columns.Borders.Color = Color.Black;
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;                     
                        DateTime current_date = DateTime.Now;
                        DateTime current_datetime = new DateTime(current_date.Year, current_date.Month, current_date.Day, current_date.Hour, current_date.Minute, current_date.Second, DateTimeKind.Utc);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string datetime = current_datetime.ToString("dd-MM-yyyy hh-mm-ss");
                        string compinepath = "\\Print Lable -" + datetime;
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
                throw new ArgumentNullException("btn_printlbl_dwn_Click", ex);
            }
        }

        private void textLotNoAdd_TextChanged(object sender, EventArgs e)
        {
            lot_information_changed_without_grid = true;
            lotnumber_changed_add_pi_tbl = true;
            lotnumber_only_changed_add_pi_tbl = true;
            btn_add_only_lotno.Text = "    Add New Lot                                 [F2]";
           
        }

        private void txt_pl_lotno_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_pl_frm_lotc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_pl_to_lotc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textLotNoChild_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textLotNoAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_lotinfo_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textSearchLotNo_KeyPress(object sender, KeyPressEventArgs e)
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
        public bool all_process_completed_check(string lotno, string lotno_child)
        {
            try
            {
                bool result = false;
                int rowindex = 0;
                foreach (var itm in CommonClass.Process_name)
                {
                    if (rowindex > 0)
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
                            if (get_processid == "0")
                            {
                                back_clr = Color.Red;
                                result = true;
                                getColor.Close();
                                helper.CloseConnection();
                                return result;
                            }
                        }
                        getColor.Close();
                        helper.CloseConnection();
                    }
                    rowindex++;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("all_process_completed_check", ex);
            }
        }
        public bool process_id_exist_check(string lotno, string lotno_child, string processid, string pattern_type)
        {
            try
            {
                bool result = false;

                string[] str_exist = { "@lotnumber", "@lotnumber_child", "@proc_id", "@ActionType" };
                string[] obj_exist = { lotno, lotno_child, processid, pattern_type };
                MySqlDataReader getDate = helper.GetReaderByCmd("check_processid_exist_viewtime", str_exist, obj_exist);
                if (getDate.Read())
                {
                    string get_processid = getDate["pid_exist"].ToString();
                    if (get_processid != "0")
                    {
                        result = true;
                        getDate.Close();
                        helper.CloseConnection();
                        return result;
                    }
                }
                getDate.Close();
                helper.CloseConnection();
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("process_id_exist_check", ex);
            }
        }
        public void lot_number_only_row_common(string ActionType_only_lot,string LotNum)
        {
            try
            {
                string Compare_lotNo;
                //string ActionType_only_lot = "onlylotview";
                string[] str_only_lot = { "@ActionType", "@Customercd", "@itmcd", "@lotnumber" };
                string[] obj_only_lot = { ActionType_only_lot, txtCustomerCode.Text, txt_lotinfo_itemcode.Text, LotNum };

                DataSet ds_only_lot = helper.GetDatasetByCommandString("lotinfo_only_view", str_only_lot, obj_only_lot);
                int count_data = ds_only_lot.Tables[0].Rows.Count;        
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
                                    //if(bproduct=="B")
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
                                    row.Cells[7].Value = manf_dte + lotno+ lotnochld_format;
                                    row.Cells[8].Value = lotnochld_format;
                                }
                            }
                        }
                        // }
                    }                  
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("lot_number_only_row_common", ex);
            }
        }
        public void lotno_only_tbl_insert()
        {
            try
            {
                bool result_lotonly = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                if (result_lotonly)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to LotInformation Only Update?", "UPDATE LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string exp_date = dateTimePicker_Manf.Value.ToShortDateString();
                        DateTime oDate = Convert.ToDateTime(exp_date);
                        DateTime nextYear = oDate.AddYears(+1);
                        exp_date = nextYear.ToString("yyyy-MM-dd");
                        string ActionType = "all";
                        string Bproduct = null;
                        string Onhold = null;
                        string Scrap = null;
                        if (chk_bproduct.Checked)
                        {
                            Bproduct = "B";
                        }
                        if (chk_onhold.Checked)
                        {
                            Onhold = "H";
                        }
                        if(chkbx_scrap.Checked)
                        {
                            Scrap = "S";
                            Onhold = null;
                        }
                        string reason = null;
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                        {
                            reason = txt_reason_hs.Text;
                        }

                        DateTime current_date_time = DateTime.Now;
                        string[] str_updlotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@expirydate", "@manftime","@bpro", "@updatedat", "@ActionType","@hld","@uid", "@scrp","@reason" };
                        string[] obj_updlotinfo = { txtCustomerCode.Text, textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text,Bproduct, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType, Onhold, CommonClass.logged_Id, Scrap,reason };
                        MySqlDataReader all_patern_upd = helper.GetReaderByCmd("lotinfo_only_update", str_updlotinfo, obj_updlotinfo);
                        if (all_patern_upd.Read())
                        {
                            all_patern_upd.Close();
                            helper.CloseConnection();
                            MessageBox.Show("Lot Information Only Updated..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btn_add_only_lotno.Text = "    Add New Lot                                 [F2]";
                            dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, dgProduct_grid_selectedRow));
                        }
                        all_patern_upd.Close();
                        helper.CloseConnection();
                        lot_information_changed_without_grid = false;
                    }
                }
                //  insert : pi_lotinformation_only_master tbl only
                else if (!result_lotonly)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to LotInformation Only Insert?", "ADD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string exp_date = dateTimePicker_Manf.Value.ToShortDateString();
                        DateTime oDate = Convert.ToDateTime(exp_date);
                        DateTime nextYear = oDate.AddYears(+1);
                        exp_date = nextYear.ToString("yyyy-MM-dd");
                        string ActionType = "ins";
                        string Bproduct = null;
                        if (chk_bproduct.Checked)
                        {
                            Bproduct = "B";
                        }
                        string Onhold = null;
                        if (chk_onhold.Checked)
                        {
                            Onhold = "H";
                        }
                        string Scrap = null;
                        if (chkbx_scrap.Checked)
                        {
                            Scrap = "S";
                            Onhold = null;
                        }
                        string reason = null;
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                        {
                            reason = txt_reason_hs.Text;
                        }
                        DateTime current_date_time = DateTime.Now;
                        string[] str_inslotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@lot_qty", "@manfdate", "@expirydate", "@manftime","@bpro" ,"@createdat", "@ActionType","@hld","@uid","@scrp", "@reason" };
                        string[] obj_inslotinfo = { txtCustomerCode.Text, textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text, Bproduct, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType,Onhold,CommonClass.logged_Id,Scrap, reason };
                        MySqlDataReader all_patern_ins = helper.GetReaderByCmd("lotinfo_only_insert", str_inslotinfo, obj_inslotinfo);
                        if (all_patern_ins.Read())
                        {
                            all_patern_ins.Close();
                            helper.CloseConnection();                           
                            product_inforamtion_insert_only_lotno_addtime();
                            MessageBox.Show("Lot Information Only Insert Sucessfully..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, dgProduct_grid_selectedRow));
                        }
                        all_patern_ins.Close();
                        helper.CloseConnection();
                        lot_information_changed_without_grid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("lotno_only_tbl_insert", ex);
            }
        }

        private void btn_add_only_lotno_Click(object sender, EventArgs e)
        {
            try
            {
                // any changes means its go to if 
                Cursor.Current = Cursors.WaitCursor;
                if(textLotNoAdd.Text!= "0000000" && textLotNoChild.Text!="00")
                {                   
                    if (!CommonClass.lot_info_changes)
                    {
                        if (lot_information_changed_without_grid)
                        {
                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                            if (result)
                            {
                                DialogResult dialogResult = MessageBox.Show("Do you want to LotInformation Only Update?", "UPDATE LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if(dialogResult == DialogResult.Yes)
                                {
                                    string exp_date = dateTimePicker_Manf.Value.ToShortDateString();
                                    DateTime oDate = Convert.ToDateTime(exp_date);
                                    DateTime nextYear = oDate.AddYears(+1);
                                    exp_date = nextYear.ToString("yyyy-MM-dd");
                                    string ActionType = "all";
                                    string Bproduct = null;
                                    if (chk_bproduct.Checked)
                                    {
                                        Bproduct = "B";
                                    }
                                    string Onhold = null;
                                    if (chk_onhold.Checked)
                                    {
                                        Onhold = "H";
                                    }
                                    string scrap = null;
                                    if (chkbx_scrap.Checked)
                                    {
                                        scrap = "S";
                                        Onhold = null;
                                    }
                                    string reason = null;
                                    if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text!="Remarks")
                                    {
                                        reason = txt_reason_hs.Text;
                                    }
                                    DateTime current_date_time = DateTime.Now;
                                    string[] str_updlotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@expirydate", "@manftime", "@bpro", "@updatedat", "@ActionType","@hld","@uid","@scrp","@reason" };
                                    string[] obj_updlotinfo = { txtCustomerCode.Text, textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text, Bproduct, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType,Onhold,CommonClass.logged_Id,scrap,reason };
                                    MySqlDataReader all_patern_upd = helper.GetReaderByCmd("allpatern_update_lotinfo_only", str_updlotinfo, obj_updlotinfo);
                                    if (all_patern_upd.Read())
                                    {
                                        all_patern_upd.Close();
                                        helper.CloseConnection();
                                        MessageBox.Show("Lot Information Only Updated..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                       
                                        dGProduct_CellContentClick(this.dGProduct, new DataGridViewCellEventArgs(0, dgProduct_grid_selectedRow));
                                    }
                                    all_patern_upd.Close();
                                    helper.CloseConnection();
                                    lot_information_changed_without_grid = false;
                                    // manufacturing date refresh 
                                    dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                                    DateTime cnowdate = DateTime.Now;
                                    txt_manf_time.Text = cnowdate.ToString("HH:mm:ss");
                                
                                }
                            }
                            else if (!result)
                            {     
                             
                                lotno_only_tbl_insert();
                                // manufacturing date refresh 
                                dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
                                DateTime cnowdate = DateTime.Now;
                                txt_manf_time.Text = cnowdate.ToString("HH:mm:ss");
                            }
                            chkbx_scrap.Checked = false;
                            chk_onhold.Checked = false;
                            chk_bproduct.Checked = false;
                            txt_reason_hs.Text = "Remarks";
                            txt_reason_hs.ForeColor = Color.Gray;

                        }
                        else
                        {
                            MessageBox.Show("No Changes Right now..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textLotNoChild.Focus();
                        }

                    }
                    else if (CommonClass.lot_info_changes)
                    {
                        if (lot_information_changed_without_grid)
                        {
                            DialogResult dialogResult_confirm = MessageBox.Show("Recently New Process details added continue means process details data only lose..?", "ADD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult_confirm == DialogResult.Yes)
                            {
                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                // 240223 false !result 
                                if (!result)
                                {
                                    truncate_pattern_temp();                               
                                    lotno_only_tbl_insert();
                                }                                
                                chkbx_scrap.Checked = false;
                                chk_onhold.Checked = false;
                                chk_bproduct.Checked = false;
                                txt_reason_hs.Text = "Remarks";
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Changes Right now..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textSearchLotNo.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lot No and Lot No child is null..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textSearchLotNo.Focus();
                }                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_add_only_lotno_Click", ex);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chk_bproduct_CheckedChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
            lot_information_changed_without_grid = true;
            if(!chk_onhold.Checked && !chkbx_scrap.Checked)
            {
                txt_reason_hs.Text = "Remarks";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }
        public void Bproduct_update()
        {
            try
            {
                string exp_date = dateTimePicker_Manf.Value.ToShortDateString();
                DateTime oDate = Convert.ToDateTime(exp_date);
                DateTime nextYear = oDate.AddYears(+1);
                exp_date = nextYear.ToString("yyyy-MM-dd");
                string ActionType_upt = "all_bp";
                string Bproduct = null;
                if (chk_bproduct.Checked)
                {
                    Bproduct = "B";
                }
                string Onhold = null;
                if (chk_onhold.Checked)
                {
                    Onhold = "H";
                }
                string scrap = null;
                if (chkbx_scrap.Checked)
                {
                    scrap = "S";
                    Onhold = null;
                }
                string reason = null;
                if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Remarks")
                {
                    reason = txt_reason_hs.Text;
                }
                DateTime current_date_time = DateTime.Now;
                string[] str_updlotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@itmname", "@lot_qty", "@manfdate", "@expirydate", "@manftime", "@bpro", "@updatedat", "@ActionType","@hld","@uid","@scrp", "@reason" };
                string[] obj_updlotinfo = { txtCustomerCode.Text, textLotNoAdd.Text, textLotNoChild.Text, txt_lotinfo_itemcode.Text, txt_lotinfo_itm_nam.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text, Bproduct, current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), ActionType_upt,Onhold,CommonClass.logged_Id,scrap ,reason};
                MySqlDataReader all_patern_upd = helper.GetReaderByCmd("allpatern_update_lotinfo_only", str_updlotinfo, obj_updlotinfo);
                if (all_patern_upd.Read())
                {
                }
                all_patern_upd.Close();
                helper.CloseConnection();
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("Bproduct_update", ex);
            }
        }

        private void btn_bulkupt_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!CommonClass.lot_info_changes)
                {
                    if (check_input_bulkupdate())
                    {
                        FormBulkUpdate frm = new FormBulkUpdate();
                        frm.Owner = this;
                        frm.customerCode = txtCustomerCode.Text;
                        frm.itemCode = textItemCode.Text;
                        frm.itemName = textItemName.Text;
                        frm.lotQty = txt_lotinfo_quantity.Text;
                        frm.manufacturingTime = txt_manf_time.Text;
                        frm.grid_selected_row = dgProduct_grid_selectedRow;
                        frm.ShowDialog();
                    }
                }
                else if(CommonClass.lot_info_changes)
                {
                    DialogResult dialogResult_confirm = MessageBox.Show("Recently New Process details added continue means process details data only lose..?", "ADD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult_confirm == DialogResult.Yes)
                    {
                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                        if (!result)
                        {
                            truncate_pattern_temp();
                            if (check_input_bulkupdate())
                            {
                                FormBulkUpdate frm = new FormBulkUpdate();
                                frm.Owner = this;
                                frm.customerCode = txtCustomerCode.Text;
                                frm.itemCode = textItemCode.Text;
                                frm.itemName = textItemName.Text;
                                frm.lotQty = txt_lotinfo_quantity.Text;
                                frm.manufacturingTime = txt_manf_time.Text;
                                frm.grid_selected_row = dgProduct_grid_selectedRow;
                                frm.ShowDialog();
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("btn_bulkupt_Click", ex);
            }
        }
        public bool check_input_bulkupdate()
        {
            bool result = true;
            if (txtCustomerCode.Text.Trim() == "" || txtCustomerCode.Text.Trim() == "000000")
            {
                int check_val = Convert.ToInt32(txtCustomerCode.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Choose any one Customer", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerCode.Focus();
                    result = false;
                    return result;
                }
            }
            else if (textItemCode.Text == "")
            {              
                    MessageBox.Show("Item Code is Null and Check the Lot Information", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textItemCode.Focus();
                    result = false;
                    return result;
            }
            return result;
        }

        private void FormProductionInput_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CommonClass.Process_name = new List<PI_Process>();
            CommonClass.PI_insert_data = new List<PI_master_use_insert>();
            CommonClass.PI_insert_data_temp = new List<PI_master_use_insert>();
            truncate_pattern_temp();
            CommonClass.view_enable = false;
            CommonClass.lot_info_changes = false;
            ((Form1)MdiParent).productionInputToolStripMenuItem.Enabled = true;
            Cursor.Current = Cursors.Default;
        }
        public bool Lotno_LotnoChild_already_exist(string lotno, string lotno_chld, string ActionType_exist)
        {
            bool result = false;            
            string[] str_exist = { "@lotnumber", "@lotnochld", "@ActionType" };
            string[] obj_exist = { lotno, lotno_chld, ActionType_exist };
            MySqlDataReader already_exist = helper.GetReaderByCmd("check_lotno_child_exist", str_exist, obj_exist);
            if (already_exist.Read())
            {
                string lotinfo_mast = already_exist["lotinfo_mast"].ToString();
                string lotinfo_mast_temp = already_exist["lotinfo_mast_temp"].ToString();
                string lotinfo_only_mast = already_exist["lotinfo_only_mast"].ToString();

                if (lotinfo_mast != "0" || lotinfo_mast_temp !="0" || lotinfo_only_mast !="0")
                {
                    already_exist.Close();
                    helper.CloseConnection();
                    result = true;
                }
                else
                {
                    already_exist.Close();
                    helper.CloseConnection();
                    result =false;
                }

            }
            else
            {
                already_exist.Close();
                helper.CloseConnection();
                result = false;
            }
            return result;
        }

        private void chk_onhold_CheckedChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
            lot_information_changed_without_grid = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
            lot_information_changed_without_grid = true;
            if (!chk_onhold.Checked && !chkbx_scrap.Checked)
            {
                txt_reason_hs.Text = "Remarks";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }

        private void txt_reason_hs_Enter(object sender, EventArgs e)
        {
            //if(chk_onhold.Checked || chkbx_scrap.Checked)
            //{
                if (txt_reason_hs.Text == "Remarks")
                {
                    txt_reason_hs.Text = string.Empty;
                    txt_reason_hs.ForeColor = Color.Black;
                }                
            
        }

        private void txt_reason_hs_Leave(object sender, EventArgs e)
        {
            if (txt_reason_hs.Text == string.Empty)
            {
                txt_reason_hs.Text = "Remarks";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }

        private void chkbx_scrap_Click(object sender, EventArgs e)
        {
            if (!chk_onhold.Checked && chkbx_scrap.Checked)
            {

                MessageBox.Show("On Hold Lot No. Only Move to Scrap", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerCode.Focus();
                chkbx_scrap.Checked = false;
            }
        }

        private void txt_reason_hs_TextChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
            lot_information_changed_without_grid = true;

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

        }
        public void remining_databind()
        {
        }

        private void dateTimePicker_Manf_KeyDown(object sender, KeyEventArgs e)
        {
            Bproduct_changed = true;
            lot_information_changed_without_grid = true;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {          
          
        }
        private void sortButton_Click()
        {
            // Check which column is selected, otherwise set NewColumn to null.
            DataGridViewColumn newColumn =
                dataGridView1.Columns.GetColumnCount(
                DataGridViewElementStates.Selected) == 1 ?
                dataGridView1.SelectedColumns[5] : null;

            DataGridViewColumn oldColumn = dataGridView1.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (oldColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldColumn == newColumn &&
                    dataGridView1.SortOrder == System.Windows.Forms.SortOrder.Descending)
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Descending;
                    oldColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Descending;
            }

            // If no column has been selected, display an error dialog  box.
            if (newColumn == null)
            {
                MessageBox.Show("Select a single column and try again.",
                    "Error: Invalid Selection", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                dataGridView1.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                    direction == ListSortDirection.Descending ?
                    System.Windows.Forms.SortOrder.Descending : System.Windows.Forms.SortOrder.Ascending;
            }
        }

        private void btn_bulkLotno_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (check_input_bulkupdate())
                {
                    FormBulkLotNo frm = new FormBulkLotNo();
                    frm.Owner = this;
                    frm.customerCode = txtCustomerCode.Text;
                    frm.customerNameFull = txtCustomerNameF.Text;
                    frm.customerNameShort = txtCustomerNameS.Text;
                    frm.Currency = textCurrency.Text;
                    frm.unitprice = textPrice.Text;
                    frm.boxQty = textQuantity.Text;
                    frm.additionCode = textAdditionalCode.Text;
                    frm.labelTyp = textLabelType.Text;
                    frm.m1 = textMark1.Text;
                    frm.m2 = textMark2.Text;
                    frm.m3 = textMark3.Text;
                    frm.m4 = textMark4.Text;
                    frm.itemCode = textItemCode.Text;
                    frm.itemName = textItemName.Text;
                    frm.lotQty = txt_lotinfo_quantity.Text;
                    frm.grid_selected_row = dgProduct_grid_selectedRow;
                    frm.ShowDialog();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_bulkLotno_Click", ex);
            }
        }
        public void Refesh_manfDt_time()
        {
            DateTime current_time = DateTime.Now;
            dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
            txt_manf_time.Text = current_time.ToString("HH:mm:ss");
        }

        private void btn_nextPg_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //090823  dataGridView1.ClearSelection();
                dataGridView1.Refresh();
                int cPageNo = CommonClass.PI_lotInfo_curentPageNo_nxtPg + 1;
                var Get_records = CommonClass.Runtime_Store_PI_lotInfo_details.ToPagedList(cPageNo, PageSize);
                CommonClass.PI_lotInfo_curentPageNo_nxtPg = Get_records.PageNumber; 
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
                    insert_lotinfo_value_assign_gridbind(get_details.customer_code, get_details.item_code, get_details.lotno);
                }
                dataGridView1.Refresh();
                dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
                dataGridView1.RefreshEdit();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btn_nextPg_Click", ex);
            }
        }
    }
}
