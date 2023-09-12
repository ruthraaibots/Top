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
using TopPartsElectronics_PS.Helper;
using YourApp.Data;
using static TopPartsElectronics_PS.Helper.GeneralModelClass;

namespace TopPartsElectronics_PS
{
    public partial class FormPatern3 : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        string patern3_Lotno = string.Empty;
        public FormPatern3()
        {
            InitializeComponent();
        }

        private void FormPatern3_Load(object sender, EventArgs e)
        {
            this.Text = ProcessName;
            patern3_Lotno = SelectedHiddenLotNo;
            if (Current_button_color != "Red")
            {
                if (Get_process_dt_p3 != string.Empty)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(Get_process_dt_p3,
                    System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                }
            }
            else if(Current_button_color == "Red" && Get_process_dt_p3 != "0" && Get_process_dt_p3 != string.Empty)
            {
                    dateTimePicker1.Value = Convert.ToDateTime(Get_process_dt_p3,
                System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            }
            txt_patern3_qty.Text = Get_Qty_p3;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(checkInput())
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to Add Patern ?", "ADD PATTERN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {                        
                        string ActionType = "SaveDatathree";
                        string ActionType_lot = string.Empty;
                        Cursor.Current = Cursors.WaitCursor;
                        string ActionType_exist = "three";
                        string split_lotno = SelectedHiddenLotNo.Split('-')[0];
                        string split_lotno_child = SelectedHiddenLotNo.Split('-')[1];                    
                        if(Search_lotNo !=split_lotno)
                        {
                           FormProductionInput.view_time_lotno_changed = true;
                        }                     
                        string update_enable_status = string.Empty;
                        string[] str_exist = { "@lno", "@lcno", "@pro_id", "@pro_nam", "@ActionType_exist" };
                        string[] obj_exist = { split_lotno, split_lotno_child, ProcessId, Material_code_selected, ActionType_exist };
                        MySqlDataReader already_exist = helper.GetReaderByCmd("patern_already_exist", str_exist, obj_exist);
                        if (already_exist.Read())
                        {
                            // Printed date get and use insert
                            string printed_date = null;
                            string printed_status = string.Empty;
                            string printed_person_name = string.Empty;
                            string printed_date_join = string.Empty;
                            string printed_names_join = string.Empty;
                            string printed_copy_join = string.Empty;
                            List<Get_printed_date> pdate_maintbl = new List<Get_printed_date>();
                            List<Get_printed_date> pdate_onlytbl = new List<Get_printed_date>();
                            // pattern main table and pattern temp table check already exits 
                            string exist_maintbl_p3 = already_exist["found_maintable"].ToString();
                            string exist_temtbl_p3 = already_exist["found_temp"].ToString();
                            // lot main table and lot temp table check already exits 
                            string exist_lotinfo = already_exist["found_lotinfo"].ToString();
                            string exist_temp_lotinfo_temp = already_exist["found_lotinfo_temp"].ToString();
                            /// pattern temp table and lot info temp table check                          
                            if (exist_temtbl_p3 == "0" && exist_temp_lotinfo_temp == "0")
                            {
                                already_exist.Close();
                                helper.CloseConnection();
                                if (exist_maintbl_p3 != "0")
                                {
                                    update_enable_status = "Yes";
                                }
                                else if (exist_maintbl_p3 == "0")
                                {
                                    update_enable_status = "New";
                                    //260523 Get print data ,is null or not.
                                    string[] str_exist_pdate = { "@ltno", "@ltcno", "@ActionType" };
                                    string[] obj_exist_pdate = { split_lotno, split_lotno_child, "pdate" };
                                    MySqlDataReader already_exist_pdate = helper.GetReaderByCmd("Printed_date_get", str_exist_pdate, obj_exist_pdate);
                                    pdate_maintbl = LocalReportExtensions.GetList<Get_printed_date>(already_exist_pdate);
                                    helper.CloseConnection();
                                    string tbl_pk_id = pdate_maintbl.Select(x => x.id).FirstOrDefault();
                                    if (string.IsNullOrWhiteSpace(tbl_pk_id))
                                    {
                                        string[] str_exist_pdate_only = { "@ltno", "@ltcno", "@ActionType" };
                                        string[] obj_exist_pdate_only = { split_lotno, split_lotno_child, "pdate_onlytbl" };
                                        MySqlDataReader already_exist_pdate_only = helper.GetReaderByCmd("Printed_date_get", str_exist_pdate_only, obj_exist_pdate_only);
                                        pdate_onlytbl = LocalReportExtensions.GetList<Get_printed_date>(already_exist_pdate_only);
                                        helper.CloseConnection();
                                        pdate_maintbl = new List<Get_printed_date>();
                                        pdate_maintbl.AddRange(pdate_onlytbl);
                                    } 
                                    if (pdate_maintbl.Count > 0)
                                    {
                                        printed_date = pdate_maintbl.Select(x => x.print_lable_date).FirstOrDefault();
                                        printed_status = pdate_maintbl.Select(x => x.print_lable_status).FirstOrDefault();
                                        printed_person_name = pdate_maintbl.Select(x => x.print_person_name).FirstOrDefault();
                                        printed_date_join = pdate_maintbl.Select(x => x.printed_date_join).FirstOrDefault();
                                        printed_names_join = pdate_maintbl.Select(x => x.printed_names_join).FirstOrDefault();
                                        printed_copy_join = pdate_maintbl.Select(x => x.printed_copy_join).FirstOrDefault();

                                        if (!string.IsNullOrWhiteSpace(printed_date))
                                        {                                            
                                            DateTime pdate = Convert.ToDateTime(printed_date);
                                            printed_date = pdate.ToString("yyyy-MM-dd");                                           
                                            printed_status = "Yes";
                                        }
                                        else
                                        {
                                            printed_date = null;
                                            printed_status = null;
                                        }
                                        // printed_person_name null check and data insert in db null
                                        if (string.IsNullOrWhiteSpace(printed_person_name))
                                        {
                                            printed_person_name = null;
                                        }
                                        // printed_date_join null check and data insert in db null
                                        if (string.IsNullOrWhiteSpace(printed_date_join))
                                        {
                                            printed_date_join = null;
                                        }
                                        // printed_names_join null check and data insert in db null
                                        if (string.IsNullOrWhiteSpace(printed_names_join))
                                        {
                                            printed_names_join = null;
                                        }
                                        // printed_copy_join null check and data insert in db null
                                        if (string.IsNullOrWhiteSpace(printed_copy_join))
                                        {
                                            printed_copy_join = null;
                                        }
                                    }
                                    already_exist_pdate.Close();
                                    helper.CloseConnection();
                                }
                                if (exist_lotinfo != "0")
                                {
                                    ActionType_lot = "UpdateLotInfo";
                                }
                                else if (exist_lotinfo == "0")
                                {
                                    ActionType_lot = "SaveLotInfo";
                                }                                  
                                // temp table data insert with status
                                string exp_date = SelectedManfDate;
                                DateTime oDate = Convert.ToDateTime(exp_date);
                                DateTime nextYear = oDate.AddYears(+1);
                                exp_date = nextYear.ToString("yyyy-MM-dd");
                                string[] str = {  "@lotno",
                                        "@lcno",
                                        "@Customercd",
                                        "@pro_id",
                                        "@pro_nam",
                                        "@lotno_p1",
                                        "@pat_no",
                                        "@pla_dt",
                                        "@qty",
                                        "@pb_dt",
                                        "@manfdate",
                                        "@expairy_dt",
                                        "@manftime",
                                        "@itemcd",
                                        "@itmname",
                                        "@common_qty",
                                        "@update_enable_status",
                                        "@material_cd",
                                        "@created_at",
                                        "@ActionType",
                                        "@ActionType_lot",
                                        "@bpro",
                                        "@hld",
                                        "@uid",
                                        "@scrp",
                                        "@reason",
                                        "@printed_date",
                                        "@Printed_status",
                                        "@Printed_pnam",
                                        "@printed_dt_jn",
                                        "@printed_nam_jn",
                                        "@printed_cpy_jn",
                                        "@commonId"
                            };
                                string[] obj = { split_lotno,
                                     split_lotno_child,
                                     Customer_code,
                                    ProcessId,
                                    this.Text,
                                    string.Empty,
                                    "",
                                    dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                                    txt_patern3_qty.Text,
                                    "",
                                    SelectedManfDate_use_insert,
                                    exp_date,
                                    SelectedManfTime,
                                    itemcode,
                                    itemname,
                                    SelectedQuantity,
                                    update_enable_status,
                                    Material_code_selected,
                                    nowdate.ToString(),
                                    ActionType,
                                    ActionType_lot,
                                    Bproduct_p3,
                                    Onhold_p3,                                    
                                    CommonClass.logged_Id,
                                    Scrap_p3,
                                    reason_hs_p3,
                                    printed_date,
                                    printed_status,
                                    printed_person_name,
                                    printed_date_join,
                                    printed_names_join,
                                    printed_copy_join,
                                    CommonClass.pattern_temp_random_number
                                  };
                                MySqlDataReader sdrs = helper.GetReaderByCmd("pattern_ins_temp", str, obj);
                                if (sdrs.Read())
                                {                                   
                                    int id = CommonClass.PI_insert_data.Count + 1;                                   
                                        foreach (var copyof in CommonClass.PI_insert_data)
                                        {                                 
                                        var check_already_list = CommonClass.PI_insert_data.FirstOrDefault(o => o.lotno == split_lotno && o.Customercode == copyof.Customercode && o.Itemcode == copyof.Itemcode);
                                        if (check_already_list==null && Customer_code == copyof.Customercode && itemcode == copyof.Itemcode)
                                        {
                                                if (copyof.lotno == null)
                                                {
                                                    CommonClass.PI_insert_data.Where(w => w.Customercode == Customer_code && w.Itemcode == itemcode).ToList().ForEach(s => s.lotno = split_lotno);
                                                    break;
                                                }
                                                else if (copyof.lotno != split_lotno)
                                                {
                                                    PI_master_use_insert pi_insert = new PI_master_use_insert();
                                                    pi_insert.id = id.ToString();
                                                    pi_insert.Customercode = copyof.Customercode;
                                                    pi_insert.CustomerFnam = copyof.CustomerFnam;
                                                    pi_insert.CustomerSnam = copyof.CustomerSnam;
                                                    pi_insert.Itemcode = copyof.Itemcode;
                                                    pi_insert.Itemnam = copyof.Itemnam;
                                                    pi_insert.Unittype = copyof.Unittype;
                                                    pi_insert.Unitprice = copyof.Unitprice;
                                                    pi_insert.Boxqty = copyof.Boxqty;
                                                    pi_insert.Addcd = copyof.Addcd;
                                                    pi_insert.lbltype = copyof.lbltype;
                                                    pi_insert.m1 = copyof.m1;
                                                    pi_insert.m2 = copyof.m2;
                                                    pi_insert.m3 = copyof.m3;
                                                    pi_insert.m4 = copyof.m4;
                                                    pi_insert.lotno = split_lotno;
                                                    CommonClass.PI_insert_data_samecustomer_diff_lotno.Add(pi_insert);
                                                    // already get the data same time new lot number insert in list 
                                                    break;
                                                }
                                            
                                        }                                       
                                        }
                                    CommonClass.PI_insert_data.AddRange(CommonClass.PI_insert_data_samecustomer_diff_lotno);
                                    CommonClass.PI_insert_data_samecustomer_diff_lotno = new List<PI_master_use_insert>();
                                    sdrs.Close();
                                    helper.CloseConnection();
                                    string btnId = this.Sender_button;
                                    CommonClass.p3 = true;
                                    CommonClass.lot_info_changes = true;
                                    ((FormProductionInput)Owner).dynamic_data_add_gridview(this.Text, "", patern3_Lotno, dateTimePicker1.Text, SelectedQuantity, txt_patern3_qty.Text, SelectedManfDate, btnId, string.Empty, string.Empty, string.Empty, Material_code_selected,Bproduct_p3, Onhold_p3, Scrap_p3, reason_hs_p3);
                                    this.Close();
                                }
                                else
                                {
                                    sdrs.Close();
                                    helper.CloseConnection();

                                }
                            }
                            else
                            {
                                already_exist.Close();
                                helper.CloseConnection();
                                // update the temp table 
                                // temp table data insert with status
                                string exp_date = SelectedManfDate;
                                DateTime oDate = Convert.ToDateTime(exp_date);
                                DateTime nextYear = oDate.AddYears(+1);
                                exp_date = nextYear.ToString("yyyy-MM-dd");
                                string[] str = {  "@lotno",
                                        "@lcno",
                                        "@Customercd",
                                        "@pro_id",
                                        "@pro_nam",
                                        "@lotno_p1",
                                        "@pat_no",
                                        "@pla_dt",
                                        "qty",
                                        "@pb_dt",
                                        "@manfdate",
                                        "@expairy_dt",
                                        "@manftime",
                                         "@itemcd",
                                         "@itmname",
                                        "@common_qty",
                                        "@update_enable_status",
                                        "@material_cd",
                                        "@created_at",
                                        "@ActionType",
                                        "@ActionType_lot",
                                        "@bpro",
                                        "@hld",
                                        "@uid",
                                        "@scrp",
                                        "@reason"
                            };
                                string[] obj = { split_lotno,
                                     split_lotno_child,
                                     Customer_code,
                                    ProcessId,
                                    this.Text,
                                    string.Empty,
                                    "",
                                    dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                                    txt_patern3_qty.Text,
                                    "",
                                    SelectedManfDate_use_insert,
                                    exp_date,
                                    SelectedManfTime,
                                    itemcode,
                                    itemname,
                                    SelectedQuantity,
                                    update_enable_status,
                                    Material_code_selected,
                                    nowdate.ToString(),
                                    ActionType,
                                    ActionType_lot,
                                    Bproduct_p3,
                                    Onhold_p3,                                    
                                    CommonClass.logged_Id,
                                    Scrap_p3,
                                    reason_hs_p3
                                  };
                                MySqlDataReader sdrs = helper.GetReaderByCmd("pattern_upd_temp", str, obj);
                                if (sdrs.Read())
                                {
                                    sdrs.Close();
                                    helper.CloseConnection();
                                    string btnId = Sender_button;
                                    CommonClass.p3 = true;
                                    CommonClass.lot_info_changes = true;
                                    ((FormProductionInput)Owner).dynamic_data_add_gridview(this.Text, "", patern3_Lotno, dateTimePicker1.Text, SelectedQuantity, txt_patern3_qty.Text, SelectedManfDate, btnId, string.Empty, string.Empty, string.Empty,Material_code_selected, Bproduct_p3, Onhold_p3, Scrap_p3, reason_hs_p3);
                                    this.Close();
                                }
                                else
                                {
                                    sdrs.Close();
                                    helper.CloseConnection();

                                }
                            }
                        }
                    }
                }
               
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("btnSave_Click",ex);
            }
            
        }
        public bool checkInput()
        {
            bool result = true;
            if (txt_patern3_qty.Text.Trim() == "")
            {
                MessageBox.Show("Quantity is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_patern3_qty.Focus();
                result = false;
                return result;
            }
            else if (txt_patern3_qty.Text != "")
            {
                int check_val = Convert.ToInt32(txt_patern3_qty.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Qty is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_patern3_qty.Focus();
                    result = false;
                    return result;
                }
            }
            return result;
        }

        private void txt_patern3_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
