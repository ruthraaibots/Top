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
    public partial class FromPatern2BulkUpt : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        public FromPatern2BulkUpt()
        {
            InitializeComponent();
        }
        private void FromPatern2BulkUpt_Load(object sender, EventArgs e)
        {
            this.Text = processName;
            dateTimePicker2.Value = DateTime.Today.AddDays(-1);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to Update Patern ?", "UPDATE PATTERN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        // Lot master  
                        bool result = false;
                        foreach (var itm in CommonClass.list_bmodel_lotmaster)
                        {                            
                            string exp_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            DateTime oDate = Convert.ToDateTime(exp_date);
                            DateTime nextYear = oDate.AddYears(+1);
                            exp_date = nextYear.ToString("yyyy-MM-dd");
                            string manf_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");                          
                            string[] str = {  "@pk_Id",
                                        "@lotnumber",
                                        "@lotnumberchild",
                                         "@lotnumberchild_to",
                                        "@manf_dt",
                                        "@exp_dt",
                                        "@lotqty",
                                        "@manf_time",
                                        "@ActionType"

                            };
                            string[] obj = { itm.pk_lot_mast,
                                     itm.lot_no,
                                        LotNo_child_frm,
                                     LotNo_child_to,
                                    manf_date,
                                    exp_date,
                                    string.Empty,
                                    string.Empty,
                                    "lotinfo_master"
                                  };
                            MySqlDataReader sdrs = helper.GetReaderByCmd("bulkData_update_lotinfo_tbl", str, obj);
                            if (sdrs.Read())
                            {
                                result = true;
                                sdrs.Close();
                                helper.CloseConnection();
                            }
                            else
                            {
                                sdrs.Close();
                                helper.CloseConnection();

                            } 
                        }
                        // Lot master only
                        foreach (var itm in CommonClass.list_bmodel_lotmaster_only)
                        {
                            //if (BtnProcessId == itm.process_id)
                            //{
                            string exp_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            DateTime oDate = Convert.ToDateTime(exp_date);
                            DateTime nextYear = oDate.AddYears(+1);
                            exp_date = nextYear.ToString("yyyy-MM-dd");
                            string manf_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            //
                            string bomcode_gen = string.Empty;
                            string[] str = {  "@pk_Id",
                                        "@lotnumber",
                                        "@lotnumberchild",
                                        "@lotnumberchild_to",
                                        "@manf_dt",
                                        "@exp_dt",
                                        "@lotqty",
                                        "@manf_time",
                                        "@ActionType"

                            };
                            string[] obj = { itm.pk_lot_mast,
                                     itm.lot_no,
                                     LotNo_child_frm,
                                     LotNo_child_to,
                                    manf_date,
                                    exp_date,
                                    string.Empty,
                                    string.Empty,
                                    "lotinfo_only_master"
                                  };
                            MySqlDataReader sdrs = helper.GetReaderByCmd("bulkData_update_lotinfo_tbl", str, obj);
                            if (sdrs.Read())
                            {
                                result = true;
                                sdrs.Close();
                                helper.CloseConnection();
                            }
                            else
                            {
                                sdrs.Close();
                                helper.CloseConnection();

                            }
                            //}

                        }
                        // P2
                        foreach (var itm in CommonClass.list_bmodel_p2)
                        {
                            //if (BtnProcessId == itm.process_id_two)
                            //{
                            string exp_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            DateTime oDate = Convert.ToDateTime(exp_date);
                            DateTime nextYear = oDate.AddYears(+1);
                            exp_date = nextYear.ToString("yyyy-MM-dd");
                            string manf_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            //
                           
                            string bomcode_gen = string.Empty;
                            string[] str = {  "@pk_Id",
                                        "@lotnumber",
                                        "@lotnumberchild",
                                        "@lotnumberchild_frm",
                                        "@lotnumberchild_to",
                                        "@manf_dt",
                                        "@exp_dt",
                                        "@lotqty",
                                        "@manf_time",
                                        "@p1_lotno",
                                        "@p1_partno",
                                        "@p1_qty",
                                        "@p1_planting_dt",
                                        "@p1_pbdt",
                                        "@p2_procedt",
                                        "@p2_ctrlno",
                                        "@p2_sheetlot",
                                        "@p2_qty",
                                        "@p3_procdt",
                                        "@p3_qty",
                                        "@p4_lotno",
                                        "@p4_partno",
                                        "@p4_qty",
                                        "@ActionType",
                                        "@procId"

                            };
                            string[] obj = { itm.pk_idpattern_two,
                                     itm.lot_no,
                                     itm.lot_no_child,
                                     LotNo_child_frm,
                                     LotNo_child_to,
                                    manf_date,
                                    exp_date,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                                    txtCtrlNo.Text,
                                    txtLotNo.Text,
                                    txt_patern2_qty.Text,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    "p2view",
                                    BtnProcessId
                                  };
                            MySqlDataReader sdrs = helper.GetReaderByCmd("bulkData_update_pattern_tbl", str, obj);
                            if (sdrs.Read())
                            {
                                result = true;
                                sdrs.Close();
                                helper.CloseConnection();
                            }
                            else
                            {
                                sdrs.Close();
                                helper.CloseConnection();

                            }
                            //}

                        }
                        // compare two list
                        foreach (var itm in CommonClass.list_bmodel_unmatch_p2)
                        {
                            //260523 Get print data ,is null or not.
                            // Printed date get and use insert
                            string printed_date = null;
                            string printed_status = null;
                            string printed_person_name = null;
                            string printed_date_join = null;
                            string printed_names_join = null;
                            string printed_copy_join = null;
                            string bproduct = null;
                            string onhold = null;
                            string scrap = null;
                            string reason = null;
                            List<Get_printed_date> pdate_maintbl = new List<Get_printed_date>();
                            string[] str_exist_pdate = { "@ltno", "@ltcno", "@ActionType" };
                            string[] obj_exist_pdate = { itm.lot_no, itm.lot_no_child, "pdate" };
                            MySqlDataReader already_exist_pdate = helper.GetReaderByCmd("Printed_date_get", str_exist_pdate, obj_exist_pdate);
                            pdate_maintbl = LocalReportExtensions.GetList<Get_printed_date>(already_exist_pdate);
                            helper.CloseConnection();
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
                            // bproduct null check and data insert in db null
                            if (!string.IsNullOrWhiteSpace(itm.Bproduct))
                            {
                                bproduct = itm.Bproduct;
                            }
                            // onhold null check and data insert in db null
                            if (!string.IsNullOrWhiteSpace(itm.onHold))
                            {
                                onhold = itm.onHold;
                            }
                            // scrap null check and data insert in db null
                            if (!string.IsNullOrWhiteSpace(itm.scrap))
                            {
                                scrap = itm.scrap;
                            }
                            // reason null check and data insert in db null
                            if (!string.IsNullOrWhiteSpace(itm.reason))
                            {
                                reason = itm.reason;
                            }
                            string exp_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            DateTime oDate = Convert.ToDateTime(exp_date);
                            DateTime nextYear = oDate.AddYears(+1);
                            exp_date = nextYear.ToString("yyyy-MM-dd");
                            string manf_date = dateTimePicker2.Value.ToString("yyyy-MM-dd"); 
                            string[] str = {  "@lno",
                                        "@lotnoc",
                                        "@custcd",
                                        "@procid",
                                        "@itemcd",
                                        "@itmname",
                                        "@lot_qty",
                                        "@manfdate",
                                        "@expdt",
                                        "@manftime",
                                        "@materialcd",
                                        "@procname",
                                        "@p2_procedt",
                                        "@p2_ctrlno",
                                        "@p2_sheetlot",
                                        "@p2_qty",                                      
                                        "@created_at",
                                        "@ActionType",
                                        "@bpro",
                                        "@onhld",
                                        "@scrp",
                                        "@resn",
                                        "@printed_date",
                                        "@Printed_status",
                                        "@Printed_pnam",
                                        "@printed_dt_jn",
                                        "@printed_nam_jn",
                                        "@printed_cpy_jn"
                            };
                            string[] obj = { itm.lot_no,
                                     itm.lot_no_child,
                                     customerCode,
                                     BtnProcessId,
                                     itemCode,
                                    itemName,
                                    lotQty,
                                    manf_date,
                                    exp_date,
                                    manufacturingTime,
                                    materialCode,
                                    processName,
                                    dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                                    txtCtrlNo.Text,
                                    txtLotNo.Text,                                
                                    txt_patern2_qty.Text,
                                    nowdate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    "masterp2",
                                    bproduct,
                                    onhold,
                                    scrap,
                                    reason,
                                    printed_date,
                                    printed_status,
                                    printed_person_name,
                                    printed_date_join,
                                    printed_names_join,
                                    printed_copy_join
                                  };
                            MySqlDataReader sdrs = helper.GetReaderByCmd("bulk_patterntwo_insert_main_new", str, obj);
                            if (sdrs.Read())
                            {
                                sdrs.Close();
                                helper.CloseConnection();
                                result = true;
                            }
                            else
                            {
                                sdrs.Close();
                                helper.CloseConnection();

                            }
                        }
                        // Lotinformation tbl no IN lot master tbl
                        foreach (var miss_itm in CommonClass.list_bmodel_lotmaster_notIn_lotmasterOnly)
                        {
                            string[] str = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@custcd", "@itemcd", "@ActionType" };
                            string[] obj = { miss_itm.lot_no,
                                     miss_itm.lot_no_child,
                                    BtnProcessId,
                                    customerCode,
                                    itemCode,
                                    "lotinfo_master_chk_p2",
                                  };
                            MySqlDataReader sdrs = helper.GetReaderByCmd("bulkData_Get_Lotinfms_unmatch", str, obj);
                            if (sdrs.Read())
                            {
                                result = true;
                                sdrs.Close();
                                helper.CloseConnection();
                            }
                            else
                            {
                                sdrs.Close();
                                helper.CloseConnection();
                                //260523 Get print data ,is null or not.
                                // Printed date get and use insert
                                string printed_date = null;
                                string printed_status = null;
                                string printed_person_name = null;
                                string printed_date_join = null;
                                string printed_names_join = null;
                                string printed_copy_join = null;
                                string bproduct = null;
                                string onhold = null;
                                string scrap = null;
                                string reason = null;
                                List<Get_printed_date> pdate_maintbl = new List<Get_printed_date>();
                                string[] str_exist_pdate = { "@ltno", "@ltcno", "@ActionType" };
                                string[] obj_exist_pdate = { miss_itm.lot_no, miss_itm.lot_no_child, "pdate" };
                                MySqlDataReader already_exist_pdate = helper.GetReaderByCmd("Printed_date_get", str_exist_pdate, obj_exist_pdate);
                                pdate_maintbl = LocalReportExtensions.GetList<Get_printed_date>(already_exist_pdate);
                                helper.CloseConnection();
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
                                // bproduct null check and data insert in db null
                                if (!string.IsNullOrWhiteSpace(miss_itm.Bproduct))
                                {
                                    bproduct = miss_itm.Bproduct;
                                }
                                // onhold null check and data insert in db null
                                if (!string.IsNullOrWhiteSpace(miss_itm.onHold))
                                {
                                    onhold = miss_itm.onHold;
                                }
                                // scrap null check and data insert in db null
                                if (!string.IsNullOrWhiteSpace(miss_itm.scrap))
                                {
                                    scrap = miss_itm.scrap;
                                }
                                // reason null check and data insert in db null
                                if (!string.IsNullOrWhiteSpace(miss_itm.reason))
                                {
                                    reason = miss_itm.reason;
                                }
                                // insert p2 tbl
                                string exp_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                                DateTime oDate = Convert.ToDateTime(exp_date);
                                DateTime nextYear = oDate.AddYears(+1);
                                exp_date = nextYear.ToString("yyyy-MM-dd");
                                string manf_date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                                string[] str_P2 = {  "@lno",
                                        "@lotnoc",
                                        "@custcd",
                                        "@procid",
                                        "@itemcd",
                                        "@itmname",
                                        "@lot_qty",
                                        "@manfdate",
                                        "@expdt",
                                        "@manftime",
                                        "@materialcd",
                                        "@procname",
                                        "@p2_procedt",
                                        "@p2_ctrlno",
                                        "@p2_sheetlot",
                                        "@p2_qty",
                                        "@created_at",
                                        "@ActionType",
                                        "@bpro",
                                        "@onhld",
                                        "@scrp",
                                        "@resn",
                                        "@printed_date",
                                        "@Printed_status",
                                        "@Printed_pnam",
                                        "@printed_dt_jn",
                                        "@printed_nam_jn",
                                        "@printed_cpy_jn"
                            };
                                string[] obj_P2 = { miss_itm.lot_no,
                                     miss_itm.lot_no_child,
                                     customerCode,
                                     BtnProcessId,
                                     itemCode,
                                    itemName,
                                    lotQty,
                                    manf_date,
                                    exp_date,
                                    manufacturingTime,
                                    materialCode,
                                    processName,
                                    dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                                    txtCtrlNo.Text,
                                    txtLotNo.Text,
                                    txt_patern2_qty.Text,
                                    nowdate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    "masterp2",
                                    bproduct,
                                    onhold,
                                    scrap,
                                    reason,
                                    printed_date,
                                    printed_status,
                                    printed_person_name,
                                    printed_date_join,
                                    printed_names_join,
                                    printed_copy_join
                                  };
                                MySqlDataReader sdrs_P2 = helper.GetReaderByCmd("bulk_patterntwo_insert_main_new", str_P2, obj_P2);
                                if (sdrs_P2.Read())
                                {
                                    sdrs_P2.Close();
                                    helper.CloseConnection();
                                    result = true;
                                }
                                else
                                {
                                    sdrs_P2.Close();
                                    helper.CloseConnection();

                                }


                            }
                        }

                        if (result)
                        {
                            MessageBox.Show("Bulk Lot Information Update Sucessfully..", "INFROMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Cursor.Current = Cursors.WaitCursor;
                            ((FormBulkUpdate)Owner).btnSearch_Click(sender, e);
                            Cursor.Current = Cursors.Default;
                            this.Close();
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("btnSave_Click",ex);
            }
        }
        public bool checkInput()
        {
            bool result = true;
            if (txtCtrlNo.Text.Trim() == "" || txtCtrlNo.Text == "000")
            {
                MessageBox.Show("Control No. is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCtrlNo.Focus();
                result = false;
                return result;
            }
            else if (txtCtrlNo.Text != "")
            {
                int check_val = Convert.ToInt32(txtCtrlNo.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Control No. is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCtrlNo.Focus();
                    result = false;
                    return result;
                }
            }
            if (txtLotNo.Text.Trim() == "")
            {
                MessageBox.Show("Sheet Lot No. is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLotNo.Focus();
                result = false;
                return result;
            }
            else if (txtLotNo.Text != "")
            {
                int check_val = Convert.ToInt32(txtLotNo.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Sheet Lot No. is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLotNo.Focus();
                    result = false;
                    return result;
                }
            }
            if (txt_patern2_qty.Text.Trim() == "")
            {
                MessageBox.Show("Qty is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCtrlNo.Focus();
                result = false;
                return result;
            }
            else if (txt_patern2_qty.Text != "")
            {
                int check_val = Convert.ToInt32(txt_patern2_qty.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Qty is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_patern2_qty.Focus();
                    result = false;
                    return result;
                }
            }
            return result;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCtrlNo_Leave(object sender, EventArgs e)
        {
            if (txtCtrlNo.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtCtrlNo.Text);
                txtCtrlNo.Text = formate_type.ToString("D3");
            }
        }

        private void txtLotNo_Leave(object sender, EventArgs e)
        {
            if (txtLotNo.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtLotNo.Text);
                txtLotNo.Text = formate_type.ToString("D7");
            }
        }

        private void txtCtrlNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_patern2_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FromPatern2BulkUpt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btnSave.PerformClick();
            }
            else if (e.KeyCode == Keys.F9)
            {
                btnClose.PerformClick();
            }
        }
    }
}
