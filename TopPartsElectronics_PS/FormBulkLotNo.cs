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
    public partial class FormBulkLotNo : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        bool Bproduct_changed = false;
        bool lotnumber_only_changed_add_pi_tbl = false;
        public int grid_selected_row = 0;
        public FormBulkLotNo()
        {
            InitializeComponent();
        }

        private void FormBulkLotNo_Load(object sender, EventArgs e)
        {
            lbl_customercode_bulk.Text = customerCode;
            lblItemcd_bulk.Text = itemCode;
            lblitemname_bulk.Text = itemName;
            txt_lotinfo_quantity.Text = lotQty;
            txt_manf_time.Text = nowdate.ToString("HH:mm:ss");
            dateTimePicker_Manf.Value = DateTime.Today.AddDays(-1);
            max_lotno_id();
        }
        public void max_lotno_id()
        {
            //string ActionType = "lotinfo_cust";
            string ActionType = "lotinfo_cust_sno";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType, string.Empty, lbl_customercode_bulk.Text, lblItemcd_bulk.Text };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string LotNoAdd = dt.Rows[0]["lot_no"].ToString();
                if (LotNoAdd != string.Empty)
                {
                    int formate_type = Convert.ToInt32(LotNoAdd);
                    textLotNoAdd.Text = formate_type.ToString("D7");
                    //Child
                    string Lotnochild_formate_change = dt.Rows[0]["lotno_child"].ToString();
                    string Lotno_create_at = dt.Rows[0]["created_at"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type_child = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild_frm.Text = formate_type_child.ToString("D2");
                    }
                    helper.CloseConnection();
                    max_id_only_lotno(textLotNoAdd.Text, textLotNoChild_frm.Text, Lotno_create_at);
                }
                else
                {
                    helper.CloseConnection();
                    max_id_only_lotno(textLotNoAdd.Text, textLotNoChild_frm.Text, "");
                }
            }
        }
        public void max_id_only_lotno(string lotno, string lotnochild, string create_at)
        {
            string lotnoandchild = string.Empty;
            //string ActionType = "lotinfo_only_tbl";
            string ActionType = "lotinfo_only_tbl_sno";
            string common_lotno = string.Empty;
            string common_lotno_child = string.Empty;
            string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
            string[] obj = { ActionType, lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, lotnochild };
            DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                common_lotno = dt.Rows[0]["lot_no"].ToString();
                string lot_only_tbl_create_at = dt.Rows[0]["created_at"].ToString();
                if (!string.IsNullOrEmpty(common_lotno) && common_lotno != "")
                {
                    int formate_type = Convert.ToInt32(common_lotno);
                    common_lotno = formate_type.ToString("D7");
                    common_lotno_child = dt.Rows[0]["lotno_child"].ToString();

                    helper.CloseConnection();
                    if (common_lotno_child != string.Empty)
                    {
                        int formate_type_child_equal = Convert.ToInt32(common_lotno_child);
                        common_lotno_child = formate_type_child_equal.ToString("D2");
                        // Main table lot number not equal to zero 
                        int conv_lotno = Convert.ToInt32(lotno);
                        if (conv_lotno > 0 && create_at != string.Empty)
                        {
                            if (lotno == common_lotno && lotnochild == common_lotno_child)
                            {
                                // child                           
                                textLotNoChild_frm.Text = formate_type_child_equal.ToString("D2");
                                // mani tbl
                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                if (result)
                                {
                                    string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                }
                                // only lot tbl
                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                if (result_only_tbl)
                                {
                                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                }
                            }
                            else if (lotno == common_lotno)
                            {
                                string get_lotnochild = max_id_with_lotnumber_lotonlytbl(lbl_customercode_bulk.Text, lotno);
                                string get_lotnochild_maintbl = max_lotno_with_lot_maintbl(lbl_customercode_bulk.Text, lotno);
                                if (string.IsNullOrEmpty(get_lotnochild))
                                {
                                    get_lotnochild_maintbl = "0";
                                }
                                if (string.IsNullOrEmpty(get_lotnochild_maintbl))
                                {
                                    get_lotnochild_maintbl = "0";
                                }
                                int chk_lotchild = Convert.ToInt32(get_lotnochild);
                                int chk_comlotchild = Convert.ToInt32(get_lotnochild_maintbl);
                         
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
                                        //chk_lotchild = chk_lotchild + 1;
                                        // 200323
                                        textLotNoChild_frm.Text = chk_lotchild.ToString("D2");
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text,lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }

                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lotchild > chk_comlotchild)
                                        {
                                            //chk_lotchild = chk_lotchild + 1;
                                            textLotNoChild_frm.Text = chk_lotchild.ToString("D2");
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text,lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //060423                                       
                                        textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
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
                                        textLotNoChild_frm.Text = chk_lotchild.ToString("D2");
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }

                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lotchild > chk_comlotchild)
                                        {                                           
                                            textLotNoChild_frm.Text = chk_lotchild.ToString("D2");
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                }                                
                            }
                            else if (lotno != common_lotno)
                            {
                                // Pass lot number main table for both  ( lotno ) 
                                string get_lotnochild_maintbl_lotno_ps = max_lotno_with_lot_maintbl(lbl_customercode_bulk.Text, lotno);
                                string get_lotnochild_maintbl_lotno_ps_lotonly_tbl = max_id_with_lotnumber_lotonlytbl(lbl_customercode_bulk.Text, common_lotno);
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
                                        //chk_lotchild = chk_lotchild + 1;
                                        // 200323
                                        textLotNoAdd.Text = common_lotno;
                                        textLotNoChild_frm.Text = chk_lot_only_tbl.ToString("D2");
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }

                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lot_main_tbl > chk_lot_only_tbl)
                                        {
                                            //chk_lotchild = chk_lotchild + 1;
                                            textLotNoAdd.Text = common_lotno;
                                            textLotNoChild_frm.Text = chk_lot_only_tbl.ToString("D2");
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            textLotNoAdd.Text = lotno;
                                            textLotNoChild_frm.Text = chk_lot_main_tbl.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        textLotNoAdd.Text = lotno;
                                        textLotNoChild_frm.Text = chk_lot_main_tbl.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
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
                                        textLotNoAdd.Text = common_lotno;
                                        textLotNoChild_frm.Text = chk_lot_only_tbl.ToString("D2");
                                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                        if (result_only_tbl)
                                        {
                                            string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            // mani tbl
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }

                                        }
                                    }
                                    // equal means its go . date and time both are equal 
                                    else if (grater_than >= 0)
                                    {
                                        if (chk_lot_main_tbl > chk_lot_only_tbl)
                                        {
                                            //chk_lotchild = chk_lotchild + 1;
                                            textLotNoAdd.Text = common_lotno;
                                            textLotNoChild_frm.Text = chk_lot_only_tbl.ToString("D2");
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                // mani tbl
                                                bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                                if (result)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            textLotNoAdd.Text = lotno;
                                            textLotNoChild_frm.Text = chk_lot_main_tbl.ToString("D2");
                                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                            if (result)
                                            {
                                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                            else
                                            {
                                                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                                if (result_only_tbl)
                                                {
                                                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        textLotNoAdd.Text = lotno;
                                        textLotNoChild_frm.Text = chk_lot_main_tbl.ToString("D2");
                                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                                        if (result)
                                        {
                                            string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotno_max");
                                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                        }
                                        else
                                        {
                                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                                            if (result_only_tbl)
                                            {
                                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, lotno, "lotinfo_only_max");
                                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                                            }
                                        }
                                    }
                                }                                
                            }
                        }
                        else
                        {
                            textLotNoAdd.Text = common_lotno;
                            textLotNoChild_frm.Text = common_lotno_child;
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
        public void common_max_lotnoid()
        {
            string ActionType = "lotinfo_cust_common";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType, string.Empty, lbl_customercode_bulk.Text, lblItemcd_bulk.Text };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string LotNoAdd = dt.Rows[0]["lot_no"].ToString();
                if (!string.IsNullOrEmpty(LotNoAdd) && LotNoAdd != "")
                {
                    int formate_type = Convert.ToInt32(LotNoAdd);
                    textLotNoAdd.Text = formate_type.ToString("D7");
                    //Child
                    string Lotnochild_formate_change = dt.Rows[0]["lotno_child"].ToString();
                    if (Lotnochild_formate_change != string.Empty)
                    {
                        int formate_type_child = Convert.ToInt32(Lotnochild_formate_change);
                        textLotNoChild_frm.Text = formate_type_child.ToString("D2");
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
                    textLotNoChild_frm.Text = formate_type_child.ToString("D2");
                }
            }
        }
        public string max_lotno_with_lot_maintbl(string customercd, string lotnumber)
        {
            string lot_number_child_maintbl = "0";
            string createat = "0";
            //string ActionType = "lotinfo_cust_lotno";
            string ActionType = "lot_cust_lotno_sno";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            //string[] obj = { ActionType, string.Empty, lotnumber, customercd };
            string[] obj = { ActionType, lblItemcd_bulk.Text, lotnumber, customercd };

            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lot_number_child_maintbl = dt.Rows[0]["lotno_child"].ToString();
                createat = dt.Rows[0]["created_at"].ToString();
                if (string.IsNullOrEmpty(lot_number_child_maintbl))
                {
                    lot_number_child_maintbl = "0";
                }
            }
            return lot_number_child_maintbl;
        }
        public string max_id_with_lotnumber_lotonlytbl(string customercd, string lotnumber)
        {
            try
            {
                string lotno_child = "0";
                string createat = "0";
                string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
                string[] obj = { "lotinfo_only_tbl_wt_lotnumber_sno", customercd, lblItemcd_bulk.Text, lotnumber, string.Empty };
                DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    lotno_child = dt.Rows[0]["lotno_child"].ToString();
                    createat = dt.Rows[0]["created_at"].ToString();
                    if (string.IsNullOrEmpty(lotno_child))
                    {
                        lotno_child = "0";
                    }
                }
                return lotno_child;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string max_id_with_lotnumber_lotonlytbl_leave(string customercd, string lotnumber)
        {
            try
            {
                string lotno_child = "0";
                string createat = "0";
                string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
                string[] obj = { "lotinfo_only_tbl_wt_lotnumber_sno", customercd, lblItemcd_bulk.Text, lotnumber, string.Empty };
                DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    lotno_child = dt.Rows[0]["lotno_child"].ToString();
                    createat = dt.Rows[0]["created_at"].ToString();
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
                throw ex;
            }
        }
        private void btnAddLotNo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (check_input_bulkupdate())
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to LotInformation Only Insert?", "ADD LOT-INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string exp_date = dateTimePicker_Manf.Value.ToString();
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
                        if (txt_reason_hs.Text != string.Empty && txt_reason_hs.Text != "Reason")
                        {
                            reason = txt_reason_hs.Text;
                        }
                        int frm_lot = Convert.ToInt32(textLotNoChild_frm.Text);
                        int to_lot = Convert.ToInt32(txtLotnoChild_to.Text);
                        for(int i = frm_lot; i<=to_lot;i++)
                        {
                            string[] str_inslotinfo = { "@custcd", "@lno", "@lotnoc", "@itemcd", "@lot_qty", "@manfdate", "@expirydate", "@manftime", "@bpro", "@createdat", "@ActionType", "@hld", "@uid", "@scrp", "@reason" };
                            string[] obj_inslotinfo = { customerCode, textLotNoAdd.Text, i.ToString(), lblItemcd_bulk.Text, txt_lotinfo_quantity.Text, dateTimePicker_Manf.Value.ToString("yyyy-MM-dd"), exp_date, txt_manf_time.Text, Bproduct, nowdate.ToString("yyyy-MM-dd HH:mm:ss"), ActionType, Onhold, CommonClass.logged_Id, Scrap, reason };
                            MySqlDataReader all_patern_ins = helper.GetReaderByCmd("lotinfo_only_insert", str_inslotinfo, obj_inslotinfo);
                            if (all_patern_ins.Read())
                            {
                                all_patern_ins.Close();
                                helper.CloseConnection();
                                product_inforamtion_insert_only_lotno_addtime();
                                //MessageBox.Show("Lot Information Only Insert Sucessfully..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            all_patern_ins.Close();
                            helper.CloseConnection();
                        }
                        FormBulkLotNo_Load(sender, e);
                        MessageBox.Show("Lot Information Only Insert Sucessfully..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtLotnoChild_to.Text = "00";
                        chkbx_scrap.Checked = false;
                        chk_onhold.Checked = false;
                        chk_bproduct.Checked = false;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void product_inforamtion_insert_only_lotno_addtime()
        {
            try
            {
                // lotnumber change so insert the pi table 
                if (lotnumber_only_changed_add_pi_tbl)
                {
                    if (!pinfo_id_already_exist(textLotNoAdd.Text, customerCode, itemCode, "pi_info_master_with_lotno"))
                    {
                        string[] str = { "@lotno", "@lotnoc", "@cust_cd", "@cust_snam", "@cust_fnam", "@item_cd", "@itmname", "@unit_price_ctry_cd", "@unit_price", "@box_qty", "@add_cd", "@lbl_typ", "@m1", "@m2", "@m3", "@m4", "@stus_pi", "@created_at", "@ActionType", };
                        string[] obj = {
                        textLotNoAdd.Text,
                        textLotNoChild_frm.Text,
                        customerCode,
                        customerNameFull,
                        customerNameShort,
                        itemCode,
                        itemName,
                        Currency,
                        unitprice,
                        boxQty,
                        additionCode,
                        labelTyp,
                        m1,
                        m2,
                        m3,
                        m4,
                        "1",
                        nowdate.ToString("yyyy-MM-dd HH:mm:ss"),
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
                throw ex;
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
        private void txt_manf_time_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_reason_hs_TextChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
       
        }

        private void chk_bproduct_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txt_lotinfo_quantity_TextChanged(object sender, EventArgs e)
        {

        }
        public bool check_input_bulkupdate()
        {
            bool result = true;
            if (textLotNoAdd.Text.Trim() == "" || textLotNoAdd.Text.Trim() == "0000000")
            {
                int check_val = Convert.ToInt32(lbl_customercode_bulk.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Enter the Lot Number", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lbl_customercode_bulk.Focus();
                    result = false;
                    return result;
                }
            }
            else if (textLotNoChild_frm.Text == "" || textLotNoChild_frm.Text.Trim() == "00")
            {
                MessageBox.Show("Enter the From Lot Number child", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textLotNoChild_frm.Focus();
                result = false;
                return result;
            }
            else if (txtLotnoChild_to.Text == "" || txtLotnoChild_to.Text.Trim() == "00")
            {
                MessageBox.Show("Enter the From Lot Number child", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLotnoChild_to.Focus();
                result = false;
                return result;
            }
            else if (txt_lotinfo_quantity.Text.Trim() == "")
            {                
                MessageBox.Show("Enter the Lot Qty", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLotnoChild_to.Focus();
                result = false;
                return result;
            }
            else if(textLotNoChild_frm.Text!="" && txtLotnoChild_to.Text!="")
            {
               int frm=Convert.ToInt32(textLotNoChild_frm.Text);
                int to = Convert.ToInt32(txtLotnoChild_to.Text);
                int minus_value = to - frm;
                minus_value = minus_value + 1;
                if(minus_value >100)
                {
                    MessageBox.Show("Lot Number Rage below hundrend only allow", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLotnoChild_to.Focus();
                    result = false;
                    return result;
                }
                else if(minus_value < 0)
                {
                    MessageBox.Show("Lot Number Rage Negative", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLotnoChild_to.Focus();
                    txtLotnoChild_to.Text = "00";
                    result = false;
                    return result;
                }
                // range check 
                string[] str_range_lotinfo = { "@lotnumber", "@lotnumber_chld_frm", "@lotnumber_chld_to", "@ActionType", "@cust_id", "@itmcd" };
                string[] obj_range_lotinfo = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, "GetRange",lbl_customercode_bulk.Text,lblItemcd_bulk.Text};
                MySqlDataReader range_check = helper.GetReaderByCmd("bulk_lotno_rangecheck", str_range_lotinfo, obj_range_lotinfo);              
                List<Get_Range_values> get_range_values = LocalReportExtensions.GetList<Get_Range_values>(range_check);
                string max_range_lotms_lotnumber = string.Empty;
                string max_range_lotonly_lotnumber = string.Empty;
                if (get_range_values.Count>0)
                {                    
                    foreach(var range in get_range_values)
                    {
                        max_range_lotms_lotnumber = range.pi_lotms_child;
                        max_range_lotonly_lotnumber = range.pi_lotonly_child;
                        break;
                    }
                    range_check.Close();
                    helper.CloseConnection();
                    if(max_range_lotms_lotnumber != string.Empty && max_range_lotonly_lotnumber !=string.Empty)
                    {
                        MessageBox.Show("Already in this "+ max_range_lotms_lotnumber + " Range exist " + max_range_lotonly_lotnumber + " in data base..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        result = false;
                        textLotNoChild_frm.Focus();
                        return result;
                    }
                    else if (max_range_lotms_lotnumber != string.Empty && max_range_lotonly_lotnumber == string.Empty)
                    {
                        MessageBox.Show("Already in this " + max_range_lotms_lotnumber + " Range exist in data base..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        result = false;
                        textLotNoChild_frm.Focus();
                        return result;
                    }
                    else if (max_range_lotms_lotnumber == string.Empty && max_range_lotonly_lotnumber != string.Empty)
                    {
                        MessageBox.Show("Already in this " + max_range_lotonly_lotnumber + " Range exist in data base..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        result = false;
                        textLotNoChild_frm.Focus();
                        return result;
                    }
                    else
                    {
                        result = true;
                        textLotNoChild_frm.Focus();
                        return result;
                    }
                                       
                }
                range_check.Close();
                helper.CloseConnection();
            }
            return result;
        }

        private void chk_bproduct_CheckedChanged_1(object sender, EventArgs e)
        {
            Bproduct_changed = true;
           
            if (!chk_onhold.Checked && !chkbx_scrap.Checked)
            {
                txt_reason_hs.Text = "Reason";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }

        private void chk_onhold_CheckedChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
          
            if (!chk_onhold.Checked && !chkbx_scrap.Checked)
            {
                txt_reason_hs.Text = "Reason";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }

        private void chkbx_scrap_CheckedChanged(object sender, EventArgs e)
        {
            Bproduct_changed = true;
       
            if (!chk_onhold.Checked && !chkbx_scrap.Checked)
            {
                txt_reason_hs.Text = "Reason";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }

        private void textLotNoAdd_Leave(object sender, EventArgs e)
        {
            string get_lotnochild = max_id_with_lotnumber_lotonlytbl_leave(lbl_customercode_bulk.Text, textLotNoAdd.Text);
            string get_lotnochild_maintbl = max_lotno_with_lot_maintbl_leave(lbl_customercode_bulk.Text, textLotNoAdd.Text);

            if (get_lotnochild.Split(',')[0] == "0" && get_lotnochild_maintbl.Split(',')[0] == "0")
            {
                textLotNoChild_frm.Text = "01";
            }
            else if (get_lotnochild.Split(',')[0] != "0" && get_lotnochild_maintbl.Split(',')[0] == "0")
            {
                int chk_lotchild_ = Convert.ToInt32(get_lotnochild.Split(',')[0]);
                textLotNoChild_frm.Text = chk_lotchild_.ToString("D2");
                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                if (result_only_tbl)
                {
                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text,lblItemcd_bulk.Text, textLotNoAdd.Text, "lotinfo_only_max");
                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                }
                else
                {
                    // mani tbl
                    bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                    if (result)
                    {
                        string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotno_max");
                        int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                        textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                    }
                }
            }
            else if (get_lotnochild.Split(',')[0] == "0" && get_lotnochild_maintbl.Split(',')[0] != "0")
            {
                int chk_lotchild_ = Convert.ToInt32(get_lotnochild_maintbl.Split(',')[0]);
                //chk_lotchild_ = chk_lotchild_ + 1;
                textLotNoChild_frm.Text = chk_lotchild_.ToString("D2");
                bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                if (result_only_tbl)
                {
                    string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotinfo_only_max");
                    int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                    textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                }
                else
                {
                    // mani tbl
                    bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                    if (result)
                    {
                        string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotno_max");
                        int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                        textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                    }
                }
            }
            else if (get_lotnochild.Split(',')[0] != "0" && get_lotnochild_maintbl.Split(',')[0] != "0")
            {
                // lot number child
                int chk_lotchild = Convert.ToInt32(get_lotnochild.Split(',')[0]);
                int chk_comlotchild = Convert.ToInt32(get_lotnochild_maintbl.Split(',')[0]);
                // create at date 
                DateTime lot_main_tbl = DateTime.Parse(get_lotnochild_maintbl.Split(',')[1]);
                DateTime lot_only_tbl = Convert.ToDateTime(get_lotnochild.Split(',')[1]);
                int grater_than = DateTime.Compare(lot_main_tbl, lot_only_tbl);
                if (grater_than < 0)
                {
                    //chk_lotchild = chk_lotchild + 1;
                    textLotNoChild_frm.Text = chk_lotchild.ToString("D2");
                    bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                    if (result_only_tbl)
                    {
                        string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotinfo_only_max");
                        int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                        textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                    }
                    else
                    {
                        // mani tbl
                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                        if (result)
                        {
                            string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotno_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                        }
                    }
                }
                // equal means its go 
                else if (grater_than <= 0)
                {
                    if (chk_lotchild > chk_comlotchild)
                    {
                        //chk_lotchild = chk_lotchild + 1;
                        textLotNoChild_frm.Text = chk_lotchild.ToString("D2");
                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                        if (result_only_tbl)
                        {
                            string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotinfo_only_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                        }
                        else
                        {
                            // mani tbl
                            bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                            if (result)
                            {
                                string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotno_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                            }
                        }
                    }
                    else
                    {
                        textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
                        bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                        if (result)
                        {
                            string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotno_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                        }
                        else
                        {
                            bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                            if (result_only_tbl)
                            {
                                string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotinfo_only_max");
                                int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                                textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                            }
                        }
                    }
                }
                else
                {
                    textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
                    bool result = check_lotno_lotnoChild_already_exist("GetMappedItemCode");
                    if (result)
                    {
                        string get_max_lotnumber_child = max_lotno_manitbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotno_max");
                        int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                        textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
                    }
                    else
                    {
                        bool result_only_tbl = check_lotno_lotnoChild_already_exist("GetMappedItemCode_lotonly_tbl");
                        if (result_only_tbl)
                        {
                            string get_max_lotnumber_child = max_lotno_onlytbl(lbl_customercode_bulk.Text, lblItemcd_bulk.Text, textLotNoAdd.Text, "lotinfo_only_max");
                            int formate_child_equal = Convert.ToInt32(get_max_lotnumber_child);
                            textLotNoChild_frm.Text = formate_child_equal.ToString("D2");
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
       
        public string max_lotno_with_lot_maintbl_leave(string customercd, string lotnumber)
        {
            string lot_number_child_maintbl = "0";
            string createat = "0";            
            string ActionType = "lot_cust_lotno_sno";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };            
            string[] obj = { ActionType, lblItemcd_bulk.Text, lotnumber, customercd };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lot_number_child_maintbl = dt.Rows[0]["lotno_child"].ToString();
                createat = dt.Rows[0]["created_at"].ToString();
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

        private void chkbx_scrap_Click(object sender, EventArgs e)
        {
            if (!chk_onhold.Checked && chkbx_scrap.Checked)
            {
                MessageBox.Show("On Hold Lot No. Only Move to Scrap", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chk_onhold.Focus();
                chkbx_scrap.Checked = false;
            }
        }

        private void txt_reason_hs_Enter(object sender, EventArgs e)
        {
            if (chk_onhold.Checked || chkbx_scrap.Checked)
            {
                if (txt_reason_hs.Text == "Reason")
                {
                    txt_reason_hs.Text = string.Empty;
                    txt_reason_hs.ForeColor = Color.Black;
                }
            }
            else
            {
                MessageBox.Show("Must Choose Anyone of this OnHold or Scrap..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_reason_hs.Text = "Reason";
                txt_reason_hs.ForeColor = Color.Gray;
                chk_onhold.Focus();
            }
        }

        private void txt_reason_hs_Leave(object sender, EventArgs e)
        {
            if (txt_reason_hs.Text == string.Empty)
            {
                txt_reason_hs.Text = "Reason";
                txt_reason_hs.ForeColor = Color.Gray;
            }
        }

        private void txt_reason_hs_TextChanged_1(object sender, EventArgs e)
        {
            Bproduct_changed = true;         
        }

        private void textLotNoAdd_TextChanged(object sender, EventArgs e)
        {
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void dateTimePicker_Manf_ValueChanged(object sender, EventArgs e)
        {
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void txt_manf_time_TextChanged_1(object sender, EventArgs e)
        {
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void txt_lotinfo_quantity_TextChanged_1(object sender, EventArgs e)
        {
            lotnumber_only_changed_add_pi_tbl = true;
        }

        private void FormBulkLotNo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ((FormProductionInput)this.Owner).dGProduct_CellContentClick(sender, new DataGridViewCellEventArgs(0, grid_selected_row));
            ((FormProductionInput)this.Owner).Refesh_manfDt_time();
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        public bool check_lotno_lotnoChild_already_exist(string ActionType)
        {
            try
            {
                bool result = false;
                string split_lotno = textLotNoAdd.Text;
                string split_lotno_child = textLotNoChild_frm.Text;
                string[] str_exist = { "@lno", "@lcno", "@ActionType", "custcd", "itemcd" };
                string[] obj_exist = { split_lotno, split_lotno_child, ActionType, lbl_customercode_bulk.Text, lblItemcd_bulk.Text };
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
                throw ex;
            }
        }
        public string max_lotno_manitbl(string custcd, string itmcd, string lotnum, string ActionType)
        {
            string lot_number_child_maintbl = "0";
            string[] str = { "@ActionType", "@ActionRole", "@searchLotno", "@input2" };
            string[] obj = { ActionType, itmcd, lotnum, custcd };
            DataSet ds = helper.GetDatasetByCommandString("max_id", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lot_number_child_maintbl = dt.Rows[0]["lotno_child"].ToString();

                if (string.IsNullOrEmpty(lot_number_child_maintbl))
                {
                    lot_number_child_maintbl = "0";
                }
            }
            return lot_number_child_maintbl;
        }
        public string max_lotno_onlytbl(string custcd, string itmcd, string lotnum, string ActionType)
        {
            string lotno_child = "0";
            string[] str = { "@ActionType", "@custcd", "@itmcd", "@lotnumber", "@lotnumchild" };
            string[] obj = { ActionType, custcd, itmcd, lotnum, string.Empty };
            DataSet ds = helper.GetDatasetByCommandString("max_id_onlylotno", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lotno_child = dt.Rows[0]["lotno_child"].ToString();
                if (string.IsNullOrEmpty(lotno_child))
                {
                    lotno_child = "0";
                }
            }
            return lotno_child;
        }
    }
}
