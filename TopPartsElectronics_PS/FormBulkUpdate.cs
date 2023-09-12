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
    public partial class FormBulkUpdate : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        public FormBulkUpdate()
        {
            InitializeComponent();
        }
        private void FormBulkUpdate_Load(object sender, EventArgs e)
        {
            CommonClass.list_bmodel_lotmaster = new List<Bulkdata_get_lotmaster>();
            CommonClass.list_bmodel_lotmaster_gpby_processid = new List<Bulkdata_get_lotmaster>();
            //CommonClass.list_bmodel_lotmaster_only = new List<Bulkdata_get_lotmaster_only>();
            CommonClass.list_bmodel_lotmaster_only = new List<Bulkdata_get_lotmaster>();
            CommonClass.list_bmodel_p1 = new List<Bulkdata_get_pattern_one>();
            CommonClass.list_bmodel_p2 = new List<Bulkdata_get_pattern_two>();
            CommonClass.list_bmodel_p3 = new List<Bulkdata_get_pattern_three>();
            CommonClass.list_bmodel_p4 = new List<Bulkdata_get_pattern_four>();
            lbl_customercode_bulk.Text = customerCode;
            lblItemcd_bulk.Text = itemCode;
            lblitemname_bulk.Text = itemName;
            max_lotno_id();
            // 280423 update so change default 01
            textLotNoChild_frm.Text = "01";
        }
        private void btn_lotinfo_save_Click(object sender, EventArgs e)
        {
            FromPatern1BulkUpt frm = new FromPatern1BulkUpt();
            frm.Owner = this;
            frm.ShowDialog();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    CommonClass.list_bmodel_lotmaster = new List<Bulkdata_get_lotmaster>();
                    CommonClass.list_bmodel_lotmaster_gpby_processid = new List<Bulkdata_get_lotmaster>();
                    //CommonClass.list_bmodel_lotmaster_only = new List<Bulkdata_get_lotmaster_only>();
                    CommonClass.list_bmodel_lotmaster_only = new List<Bulkdata_get_lotmaster>();
                    CommonClass.list_bmodel_p1 = new List<Bulkdata_get_pattern_one>();
                    CommonClass.list_bmodel_p2 = new List<Bulkdata_get_pattern_two>();
                    CommonClass.list_bmodel_p3 = new List<Bulkdata_get_pattern_three>();
                    CommonClass.list_bmodel_p4 = new List<Bulkdata_get_pattern_four>();
                    CommonClass.list_bmodel_unmatch_p1 = new List<Bulkdata_get_lotmaster>();
                    CommonClass.list_bmodel_unmatch_p2 = new List<Bulkdata_get_lotmaster>();
                    CommonClass.list_bmodel_unmatch_p3 = new List<Bulkdata_get_lotmaster>();
                    CommonClass.list_bmodel_unmatch_p4 = new List<Bulkdata_get_lotmaster>();
                    CommonClass.list_bmodel_lotmaster_notIn_lotmasterOnly = new List<Bulkdata_get_lotmaster>();
                    Bulkdata_get_lotmaster bulk_model_lotmaster = new Bulkdata_get_lotmaster();
                    Bulkdata_get_lotmaster bulk_model_lotmaster_processid = new Bulkdata_get_lotmaster();
                    //Bulkdata_get_lotmaster_only bulk_model_only = new Bulkdata_get_lotmaster_only();
                    Bulkdata_get_lotmaster bulk_model_only = new Bulkdata_get_lotmaster();
                    Bulkdata_get_pattern_one bulk_model_p1 = new Bulkdata_get_pattern_one();
                    Bulkdata_get_pattern_two bulk_model_p2 = new Bulkdata_get_pattern_two();
                    Bulkdata_get_pattern_three bulk_model_p3 = new Bulkdata_get_pattern_three();
                    Bulkdata_get_pattern_four bulk_model_p4 = new Bulkdata_get_pattern_four();
                    Bulkdata_get_lotmaster bulk_model_unmatch_Onlylot = new Bulkdata_get_lotmaster();
                    // LotinfoMaster Table get data 
                    string AuctionType = "lotinfo_master";
                    string[] str_exist = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@ActionType" };
                    string[] obj_exist = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, AuctionType };
                    DataSet ds_LotinfoMaster = helper.GetDatasetByCommandString("bulkData_Get_Lotno", str_exist, obj_exist);
                    if (ds_LotinfoMaster.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds_LotinfoMaster.Tables[0].Rows)
                        {
                            bulk_model_lotmaster = new Bulkdata_get_lotmaster();
                            bulk_model_lotmaster.pk_lot_mast = dr["pk_lot_mast"].ToString();
                            bulk_model_lotmaster.lot_no = dr["lot_no"].ToString();
                            bulk_model_lotmaster.lot_no_child = dr["lot_no_child"].ToString();
                            bulk_model_lotmaster.customer_code = dr["customer_code"].ToString();
                            bulk_model_lotmaster.process_id = dr["process_id"].ToString();
                            bulk_model_lotmaster.material_code = dr["material_code"].ToString();
                            bulk_model_lotmaster.processname = dr["processname"].ToString();
                            bulk_model_lotmaster.pattern_type = dr["pattern_type"].ToString();
                            CommonClass.list_bmodel_lotmaster.Add(bulk_model_lotmaster);
                        }
                    }
                    helper.CloseConnection();
                    // LotinfoMaster_only Table get data 
                    string AuctionType_Master_only = "lotinfo_only_master";
                    string[] str_Master_only = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@ActionType" };
                    string[] obj_Master_only = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, AuctionType_Master_only };
                    DataSet ds_LotinfoMaster_only = helper.GetDatasetByCommandString("bulkData_Get_Lotno", str_Master_only, obj_Master_only);
                    if (ds_LotinfoMaster_only.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds_LotinfoMaster_only.Tables[0].Rows)
                        {
                            //bulk_model_only = new Bulkdata_get_lotmaster_only();
                            bulk_model_only = new Bulkdata_get_lotmaster();
                            bulk_model_only.pk_lot_mast = dr["idpi_production_master_lotinfo_only"].ToString();
                            bulk_model_only.lot_no = dr["lotno"].ToString();
                            bulk_model_only.lot_no_child = dr["lot_no_child"].ToString();
                            bulk_model_only.customer_code = dr["customercode"].ToString();
                            CommonClass.list_bmodel_lotmaster_only.Add(bulk_model_only);                            
                        }
                    }
                    helper.CloseConnection();
                  
                    if(ds_LotinfoMaster.Tables[0].Rows.Count > 0 || ds_LotinfoMaster_only.Tables[0].Rows.Count>0)
                    {
                        // Pattern one Table get data 
                        string AuctionType_p1 = "p1view";
                        string[] str_p1 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@ActionType" };
                        string[] obj_p1 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, AuctionType_p1 };
                        DataSet ds_p1 = helper.GetDatasetByCommandString("bulkData_Get_Lotno", str_p1, obj_p1);
                        if (ds_p1.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds_p1.Tables[0].Rows)
                            {
                                bulk_model_p1 = new Bulkdata_get_pattern_one();
                                bulk_model_p1.pk_idpattern_one = dr["idpattern"].ToString();
                                bulk_model_p1.lot_no = dr["lotno"].ToString();
                                bulk_model_p1.lot_no_child = dr["lotno_child"].ToString();
                                bulk_model_p1.customer_code = dr["customer_code"].ToString();
                                bulk_model_p1.process_id_one = dr["process_id"].ToString();
                                bulk_model_p1.material_code_one = dr["material_code"].ToString();
                                CommonClass.list_bmodel_p1.Add(bulk_model_p1);
                            }
                        }
                        helper.CloseConnection();
                        // Pattern two Table get data 
                        string AuctionType_p2 = "p2view";
                        string[] str_p2 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@ActionType" };
                        string[] obj_p2 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, AuctionType_p2 };
                        DataSet ds_p2 = helper.GetDatasetByCommandString("bulkData_Get_Lotno", str_p2, obj_p2);
                        if (ds_p2.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds_p2.Tables[0].Rows)
                            {
                                bulk_model_p2 = new Bulkdata_get_pattern_two();
                                bulk_model_p2.pk_idpattern_two = dr["idpatterntwo"].ToString();
                                bulk_model_p2.lot_no = dr["lotno"].ToString();
                                bulk_model_p2.lot_no_child = dr["lotno_child"].ToString();
                                bulk_model_p2.customer_code = dr["customer_code"].ToString();
                                bulk_model_p2.process_id_two = dr["process_id"].ToString();
                                bulk_model_p2.material_code_two = dr["material_code"].ToString();
                                CommonClass.list_bmodel_p2.Add(bulk_model_p2);
                            }
                        }
                        helper.CloseConnection();
                        // Pattern three Table get data 
                        string AuctionType_p3 = "p3view";
                        string[] str_p3 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@ActionType" };
                        string[] obj_p3 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, AuctionType_p3 };
                        DataSet ds_p3 = helper.GetDatasetByCommandString("bulkData_Get_Lotno", str_p3, obj_p3);
                        if (ds_p3.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds_p3.Tables[0].Rows)
                            {
                                bulk_model_p3 = new Bulkdata_get_pattern_three();
                                bulk_model_p3.pk_idpattern_three = dr["idpatternthree"].ToString();
                                bulk_model_p3.lot_no = dr["lotno"].ToString();
                                bulk_model_p3.lot_no_child = dr["lotno_child"].ToString();
                                bulk_model_p3.customer_code = dr["customer_code"].ToString();
                                bulk_model_p3.process_id_three = dr["process_id"].ToString();
                                bulk_model_p3.material_code_three = dr["material_code"].ToString();
                                CommonClass.list_bmodel_p3.Add(bulk_model_p3);
                            }
                        }
                        helper.CloseConnection();
                        // Pattern three Table get data 
                        string AuctionType_p4 = "p4view";
                        string[] str_p4 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@ActionType" };
                        string[] obj_p4 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, AuctionType_p4 };

                        DataSet ds_p4 = helper.GetDatasetByCommandString("bulkData_Get_Lotno", str_p4, obj_p4);
                        if (ds_p4.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds_p4.Tables[0].Rows)
                            {
                                bulk_model_p4 = new Bulkdata_get_pattern_four();
                                bulk_model_p4.pk_idpattern_four = dr["idpattern"].ToString();
                                bulk_model_p4.lot_no = dr["lotno"].ToString();
                                bulk_model_p4.lot_no_child = dr["lotno_child"].ToString();
                                bulk_model_p4.customer_code = dr["customer_code"].ToString();
                                bulk_model_p4.process_id_four = dr["process_id"].ToString();
                                bulk_model_p4.material_code_four = dr["material_code"].ToString();
                                CommonClass.list_bmodel_p4.Add(bulk_model_p4);
                            }
                        }
                        helper.CloseConnection();
                        // Process id unique
                        List<shipping_custcd_itemcd> get_cust_itemcd = new List<shipping_custcd_itemcd>();
                        string get_pattern_type = string.Empty;
                        string[] str = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                        string[] obj = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, string.Empty, string.Empty, string.Empty, string.Empty, "lotno" };
                        DataTable dtable_spm = helper.GetDatasetByCommandString_dt("get_custcd_itemcd_vs_lotno", str, obj);
                        if (dtable_spm.Rows.Count > 0)
                        {
                            List<string> already_exits_row = new List<string>();
                            foreach (DataRow drow in dtable_spm.Rows)
                            {
                                shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                                model.customer_code = drow["customer_code"].ToString();

                                model.item_code = drow["item_code"].ToString();
                                model.customer_name = drow["customername"].ToString();
                                model.item_name = drow["item_name"].ToString();
                                get_cust_itemcd.Add(model);
                            }
                        }
                        else
                        {
                            if (dtable_spm.Rows.Count == 0)
                            {
                                string[] str_lot_oly_ms = { "@lotno", "@lotno_frm", "@lotno_to", "@manfdt_frm", "@manfdt_to", "@custcd", "@itemcd", "@ActionType" };
                                string[] obj_lot_oly_ms = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, string.Empty, string.Empty, lbl_customercode_bulk.Text, lblItemcd_bulk.Text, "lotno_onlyms" };
                                //DataTable dtable_lot_oly_ms = new DataTable();
                                DataSet dtable_lot_oly_ms = helper.GetDatasetByCommandString("bulk_get_custcd_itemcd_vs_lotno", str_lot_oly_ms, obj_lot_oly_ms);
                                if (dtable_lot_oly_ms.Tables[0].Rows.Count > 0)
                                {
                                    List<string> already_exits_row = new List<string>();
                                    foreach (DataRow drow in dtable_lot_oly_ms.Tables[0].Rows)
                                    {
                                        shipping_custcd_itemcd model = new shipping_custcd_itemcd();
                                        model.customer_code = drow["customercode"].ToString();

                                        model.item_code = drow["item_code"].ToString();
                                        model.customer_name = drow["customername"].ToString();
                                        //model.item_name = drow["item_name"].ToString();
                                        get_cust_itemcd.Add(model);
                                    }
                                }
                            }
                        }
                        if (get_cust_itemcd.Count > 0)
                        {
                            foreach (var get_cd in get_cust_itemcd)
                            {
                                terminal_addlist_loadgrid_call_loop("GetData", get_cd.customer_code, get_cd.item_code);
                            }
                        }
                        helper.CloseConnection();
                        // lotinforamtion master tbl not IN lotinforamtion Only tbl 
                        string AuctionType_unmatch_Onlylot = "get_Lotinfms_lotno";
                        string[] str_unmatch_Onlylot = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@custcd", "@itemcd", "@ActionType" };
                        string[] obj_unmatch_Onlylot = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text, customerCode, itemCode, AuctionType_unmatch_Onlylot };
                        DataSet ds_unmatch_Onlylot = helper.GetDatasetByCommandString("bulkData_Get_Lotinfms_unmatch", str_unmatch_Onlylot, obj_unmatch_Onlylot);
                        if (ds_unmatch_Onlylot.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds_unmatch_Onlylot.Tables[0].Rows)
                            {
                                bulk_model_unmatch_Onlylot = new Bulkdata_get_lotmaster();
                                bulk_model_unmatch_Onlylot.pk_lot_mast = dr["idproduction_input_master"].ToString();
                                bulk_model_unmatch_Onlylot.lot_no = dr["lot_no"].ToString();
                                bulk_model_unmatch_Onlylot.lot_no_child = dr["lot_no_child"].ToString();
                                bulk_model_unmatch_Onlylot.customer_code = dr["process_id"].ToString();
                                CommonClass.list_bmodel_lotmaster_notIn_lotmasterOnly.Add(bulk_model_unmatch_Onlylot);
                            }
                        }

                        helper.CloseConnection();
                        dynamic_button();
                    }
                    else
                    {
                           MessageBox.Show("No Records Found.In this sequence " + textLotNoAdd.Text+" Between " +textLotNoChild_frm.Text +" to "+txtLotnoChild_to.Text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                           textLotNoAdd.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void terminal_addlist_loadgrid_call_loop(string ActionType, string custcd, string itemcd)
        {
            CommonClass.Process_name_bulkdata = new List<PI_Process>();           
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
                    CommonClass.Process_name_bulkdata.Add(model);
                    i++;
                }                
            }
        }
        public bool checkInput()
        {
            bool result = true;
            if (textLotNoAdd.Text.Trim() == "")
            {
                int check_val = Convert.ToInt32(textLotNoAdd.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Lot Number is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textLotNoAdd.Focus();
                    result = false;
                    return result;
                }
            }
            else if (textLotNoChild_frm.Text != "")
            {
                int check_val = Convert.ToInt32(textLotNoChild_frm.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Lot Number From Child is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textLotNoChild_frm.Focus();
                    result = false;
                    return result;
                }
            }
            else if (txtLotnoChild_to.Text != "")
            {
                int check_val = Convert.ToInt32(txtLotnoChild_to.Text);
                if (check_val <= 0)
                {
                    MessageBox.Show("Lot Number To Child is 0", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textLotNoChild_frm.Focus();
                    result = false;
                    return result;
                }
            }
            return result;
        }      
        public void dynamic_button()
        {
            try
            {
                int i = 10;
                int x = -1;
                panel1.Controls.Clear();
                Color back_clr = System.Drawing.Color.Gray;
                Color fore_clr = System.Drawing.Color.Black;
                // order by process Id
                CommonClass.Process_name_bulkdata = CommonClass.Process_name_bulkdata.OrderBy(y => y.process_id).ToList();
                foreach (var itm in CommonClass.Process_name_bulkdata)
                {
                    //This block dynamically creates a Button and adds it to the form
                    Button btn = new Button();            
                    btn.Location = new System.Drawing.Point(19, 29);
                    btn.Name = itm.process_id + "#" + itm.ProcessNames + "#" + itm.PaternType;
                    btn.Size = new System.Drawing.Size(80, 60);
                    btn.TabIndex = 103;
                    btn.Text = itm.ProcessNames;
                    btn.UseVisualStyleBackColor = false;
                    btn.Click += new System.EventHandler(this.Patern_Click);
                    btn.Location = new Point(i, x);
                    btn.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btn.UseVisualStyleBackColor = true;
                    panel1.AutoScroll = true;
                    panel1.Controls.Add(btn);
                    i += 100;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Patern_Click(object sender, EventArgs e)
        {
            string patern_type = ((Button)sender).Name.Split('#')[2];
            string patern_Name = ((Button)sender).Name.Split('#')[1];
            string process_id = ((Button)sender).Name.Split('#')[0];
            string current_btncolor = ((Button)sender).BackColor.Name;
            string Material_code_get = Patern_material_code(customerCode, itemCode, patern_Name);
            Bulkdata_get_lotmaster bulk_model_unmatch = new Bulkdata_get_lotmaster();
            // Button btn = (Button)sender;
            if (patern_type == "1")
            {
                // unmatch record lotinformation only Table get data to INSERT 
                string AuctionType_unmatch_p1 = "get_unmatch_lotno_p1";
                string[] str_unmatch_p1 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@procId", "@ActionType" };
                string[] obj_unmatch_p1 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text,process_id, AuctionType_unmatch_p1 };

                DataSet ds_unmatch_p1 = helper.GetDatasetByCommandString("bulkData_Get_Lotno_unmatch", str_unmatch_p1, obj_unmatch_p1);
                if (ds_unmatch_p1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_unmatch_p1.Tables[0].Rows)
                    {
                        bulk_model_unmatch = new Bulkdata_get_lotmaster();
                        bulk_model_unmatch.pk_lot_mast = dr["idpi_production_master_lotinfo_only"].ToString();
                        bulk_model_unmatch.lot_no = dr["lotno"].ToString();
                        bulk_model_unmatch.lot_no_child = dr["lot_no_child"].ToString();
                        bulk_model_unmatch.customer_code = dr["customercode"].ToString();
                        //300323
                        bulk_model_unmatch.Bproduct = dr["bproduct"].ToString();
                        bulk_model_unmatch.onHold = dr["onhold"].ToString();
                        bulk_model_unmatch.scrap = dr["scrap"].ToString();
                        bulk_model_unmatch.reason = dr["reason_hs"].ToString();
                        CommonClass.list_bmodel_unmatch_p1.Add(bulk_model_unmatch);
                    }
                }
                helper.CloseConnection();
                FromPatern1BulkUpt frm = new FromPatern1BulkUpt();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.BtnProcessId = process_id;
                frm.processName = patern_Name;
                frm.LotNo = textLotNoAdd.Text;
                frm.LotNo_child_frm = textLotNoChild_frm.Text;
                frm.LotNo_child_to = txtLotnoChild_to.Text;
                // Insert details pass
                frm.customerCode = lbl_customercode_bulk.Text;
                frm.itemCode = lblItemcd_bulk.Text;
                frm.itemName = lblitemname_bulk.Text;
                frm.materialCode=Material_code_get.Split(',')[0];
                frm.Part_No = Material_code_get.Split(',')[0];
                frm.LotNo = Material_code_get.Split(',')[1];
                frm.Pb = Material_code_get.Split(',')[2];
                frm.lotQty = lotQty;
                frm.manufacturingTime = manufacturingTime;
                //
                frm.Name = ((Button)sender).Name;
                frm.ShowDialog();               

            }
            else if (patern_type == "2")
            {
                // unmatch p2 record lotinformation only Table get data to INSERT 
                string AuctionType_unmatch_p2 = "get_unmatch_lotno_p2";
                string[] str_unmatch_p2 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@procId", "@ActionType" };
                string[] obj_unmatch_p2 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text,process_id, AuctionType_unmatch_p2 };

                DataSet ds_unmatch_p2 = helper.GetDatasetByCommandString("bulkData_Get_Lotno_unmatch", str_unmatch_p2, obj_unmatch_p2);
                if (ds_unmatch_p2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_unmatch_p2.Tables[0].Rows)
                    {
                        bulk_model_unmatch = new Bulkdata_get_lotmaster();
                        bulk_model_unmatch.pk_lot_mast = dr["idpi_production_master_lotinfo_only"].ToString();
                        bulk_model_unmatch.lot_no = dr["lotno"].ToString();
                        bulk_model_unmatch.lot_no_child = dr["lot_no_child"].ToString();
                        bulk_model_unmatch.customer_code = dr["customercode"].ToString();
                        //300323
                        bulk_model_unmatch.Bproduct = dr["bproduct"].ToString();
                        bulk_model_unmatch.onHold = dr["onhold"].ToString();
                        bulk_model_unmatch.scrap = dr["scrap"].ToString();
                        bulk_model_unmatch.reason = dr["reason_hs"].ToString();
                        CommonClass.list_bmodel_unmatch_p2.Add(bulk_model_unmatch);
                    }
                }
                helper.CloseConnection();
                FromPatern2BulkUpt frm = new FromPatern2BulkUpt();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.BtnProcessId = process_id;
                frm.processName = patern_Name;
                frm.LotNo = textLotNoAdd.Text;
                frm.LotNo_child_frm = textLotNoChild_frm.Text;
                frm.LotNo_child_to = txtLotnoChild_to.Text;
                // Insert details pass
                frm.customerCode = lbl_customercode_bulk.Text;
                frm.itemCode = lblItemcd_bulk.Text;
                frm.itemName = lblitemname_bulk.Text;
                frm.materialCode = Material_code_get.Split(',')[0];
                frm.lotQty = lotQty;
                frm.manufacturingTime = manufacturingTime;
                frm.Name = ((Button)sender).Name;
                frm.ShowDialog();
            }
            else if (patern_type == "3")
            {
                // unmatch p3 record lotinformation only Table get data to INSERT 
                string AuctionType_unmatch_p3 = "get_unmatch_lotno_p3";
                string[] str_unmatchp3 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@procId", "@ActionType" };
                string[] obj_unmatchp3 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text,process_id, AuctionType_unmatch_p3 };

                DataSet ds_unmatchp3 = helper.GetDatasetByCommandString("bulkData_Get_Lotno_unmatch", str_unmatchp3, obj_unmatchp3);
                if (ds_unmatchp3.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_unmatchp3.Tables[0].Rows)
                    {
                        bulk_model_unmatch = new Bulkdata_get_lotmaster();
                        bulk_model_unmatch.pk_lot_mast = dr["idpi_production_master_lotinfo_only"].ToString();
                        bulk_model_unmatch.lot_no = dr["lotno"].ToString();
                        bulk_model_unmatch.lot_no_child = dr["lot_no_child"].ToString();
                        bulk_model_unmatch.customer_code = dr["customercode"].ToString();
                        //300323
                        bulk_model_unmatch.Bproduct = dr["bproduct"].ToString();
                        bulk_model_unmatch.onHold = dr["onhold"].ToString();
                        bulk_model_unmatch.scrap = dr["scrap"].ToString();
                        bulk_model_unmatch.reason = dr["reason_hs"].ToString();
                        CommonClass.list_bmodel_unmatch_p3.Add(bulk_model_unmatch);
                    }
                }
                helper.CloseConnection();
                FromPatern3BulkUpt frm = new FromPatern3BulkUpt();
                frm.Owner = this;
                frm.OwnerName = this.Name;
                frm.BtnProcessId = process_id;
                frm.processName = patern_Name;
                frm.LotNo = textLotNoAdd.Text;
                frm.LotNo_child_frm = textLotNoChild_frm.Text;
                frm.LotNo_child_to = txtLotnoChild_to.Text;
                // Insert details pass
                frm.customerCode = lbl_customercode_bulk.Text;
                frm.itemCode = lblItemcd_bulk.Text;
                frm.itemName = lblitemname_bulk.Text;
                frm.materialCode = Material_code_get.Split(',')[0];
                frm.lotQty = lotQty;
                frm.manufacturingTime = manufacturingTime;
                frm.Get_process_dt_p3= Material_code_get.Split(',')[4];
                frm.Get_qty_p3 = Material_code_get.Split(',')[3];
                frm.Name = ((Button)sender).Name;
                frm.ShowDialog();
            }
            else if (patern_type == "4")
            {
                // unmatch p4 record lotinformation only Table get data to INSERT 
                string AuctionType_unmatch_p4 = "get_unmatch_lotno_p4";
                string[] str_unmatch_p4 = { "@lotnumber", "@lotnumberchild_frm", "@lotnumberchild_to", "@procId", "@ActionType" };
                string[] obj_unmatch_p4 = { textLotNoAdd.Text, textLotNoChild_frm.Text, txtLotnoChild_to.Text,process_id, AuctionType_unmatch_p4 };

                DataSet ds_unmatch_p4 = helper.GetDatasetByCommandString("bulkData_Get_Lotno_unmatch", str_unmatch_p4, obj_unmatch_p4);
                if (ds_unmatch_p4.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_unmatch_p4.Tables[0].Rows)
                    {
                        bulk_model_unmatch = new Bulkdata_get_lotmaster();
                        bulk_model_unmatch.pk_lot_mast = dr["idpi_production_master_lotinfo_only"].ToString();
                        bulk_model_unmatch.lot_no = dr["lotno"].ToString();
                        bulk_model_unmatch.lot_no_child = dr["lot_no_child"].ToString();
                        bulk_model_unmatch.customer_code = dr["customercode"].ToString();
                        //300323
                        bulk_model_unmatch.Bproduct = dr["bproduct"].ToString();
                        bulk_model_unmatch.onHold = dr["onhold"].ToString();
                        bulk_model_unmatch.scrap = dr["scrap"].ToString();
                        bulk_model_unmatch.reason = dr["reason_hs"].ToString();
                        CommonClass.list_bmodel_unmatch_p4.Add(bulk_model_unmatch);
                    }
                }
                helper.CloseConnection();
                FromPatern4BulkUpt frm = new FromPatern4BulkUpt();
                frm.Owner = this;          
                frm.OwnerName = this.Name;
                frm.BtnProcessId = process_id;
                frm.processName = patern_Name;
                frm.LotNo = textLotNoAdd.Text;
                frm.LotNo_child_frm = textLotNoChild_frm.Text;
                frm.LotNo_child_to = txtLotnoChild_to.Text;
                // Insert details pass
                frm.customerCode = lbl_customercode_bulk.Text;
                frm.itemCode = lblItemcd_bulk.Text;
                frm.itemName = lblitemname_bulk.Text;
                frm.materialCode = Material_code_get.Split(',')[0];
                frm.lotQty = lotQty;
                frm.manufacturingTime = manufacturingTime;
                frm.Name = ((Button)sender).Name;
                frm.ShowDialog();
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

        private void textLotNoChild_frm_Leave(object sender, EventArgs e)
        {
            if (textLotNoChild_frm.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(textLotNoChild_frm.Text);
                textLotNoChild_frm.Text = formate_type.ToString("D2");
            }
        }

        private void txtLotnoChild_to_Leave(object sender, EventArgs e)
        {
            if (txtLotnoChild_to.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtLotnoChild_to.Text);
                txtLotnoChild_to.Text = formate_type.ToString("D2");
            }
        }
        public string Patern_material_code(string custcd,string itemcd,string processId)
        {
            string result = string.Empty;
            try
            {
                string ActionType = "GetData";
                string[] str_exist = { "@cust_cd", "@item_cd", "@proc_id", "@lotnumber", "@ActionType" };
                string[] obj_exist = { custcd, itemcd, processId, textLotNoAdd.Text, ActionType };
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
                    if (notequalzero == materialcode_srd["p1_pb_temp"].ToString())
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
                    result = materialcd + "," + p4_lotno + "," + pb + "," + p3_qty + "," + p3_process_dt;
                    materialcode_srd.Close();
                    helper.CloseConnection();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FormBulkUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ((FormProductionInput)this.Owner).dGProduct_CellContentClick(sender, new DataGridViewCellEventArgs(0, grid_selected_row));
            this.Close();
            Cursor.Current = Cursors.Default;        
        }

        private void FormBulkUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnSearch.PerformClick();
            }
        }
        public void max_lotno_id()
        {            
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
                                        //060423
                                        //textLotNoChild_frm.Text = chk_comlotchild.ToString("D2");
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
                                        //chk_lotchild = chk_lotchild + 1;
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
