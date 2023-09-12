using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TopPartsElectronics_PS.Helper.GeneralModelClass;

namespace TopPartsElectronics_PS.Helper
{
    class CommonClass
    {
        public static List<PI_Process> Process_name = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_columns = new List<PI_Process>();
        //
        public static List<PI_Process> Process_name_gridbind_shipping = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_columns_shipping = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_shipping_runtime = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_columns_shipping_runtime = new List<PI_Process>();

        public static List<PI_Process> Process_name_gridbind_shipping_runtime_filter = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_columns_shipping_runtime_filter = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_columns_shipping_runtime_filter_final = new List<PI_Process>();

        // Process name
        public static List<PI_Process> Process_name_Status = new List<PI_Process>();
        public static List<PI_Process> Process_name_gridbind_Status = new List<PI_Process>();
        //
        public static List<PI_master_use_insert> PI_insert_data_temp = new List<PI_master_use_insert>();
        public static List<PI_master_use_insert> PI_insert_data= new List<PI_master_use_insert>();
        public static List<PI_master_use_insert> PI_insert_data_copyOf = new List<PI_master_use_insert>();
        public static List<PI_master_use_insert> PI_insert_data_samecustomer_diff_lotno = new List<PI_master_use_insert>();

        // lot information grid bind use the list 
        public static List<Lotinfo_gridbind_common> Lotinfo_p1 = new List<Lotinfo_gridbind_common>();
        public static List<Lotinfo_gridbind_common> Lotinfo_p2 = new List<Lotinfo_gridbind_common>();
        public static List<Lotinfo_gridbind_common> Lotinfo_p3 = new List<Lotinfo_gridbind_common>();
        public static List<Lotinfo_gridbind_common> Lotinfo_p4 = new List<Lotinfo_gridbind_common>();
        public static List<Lotinfo_gridbind_common> Lotinfo_common = new List<Lotinfo_gridbind_common>();
        public static List<Lotinfo_gridbind_common_pattern> Lotinfo_common_pt = new List<Lotinfo_gridbind_common_pattern>();
        //bar code list 
        public static List<barcode1_details> bacode1_list = new List<barcode1_details>();
        public static List<qrcode_details> qrcode_list = new List<qrcode_details>();
        public static List<qrcode_details> list_qrcode = new List<qrcode_details>();
        public static List<barcode1_details> list_bar1code = new List<barcode1_details>();
        //
        public static List<shippingUpdate> shipping_update_lotno = new List<shippingUpdate>();
        //
        public static string current_button_id = string.Empty;
        public static string logged_Id = string.Empty;
        public static string MacAddress = string.Empty;
        //
        public static bool p1 = false;
        public static bool p2 = false;
        public static bool p3 = false;
        public static bool p4 = false;
        //
        public static bool up_p1 = false;
        public static bool up_p2 = false;
        public static bool up_p3 = false;
        public static bool up_p4 = false;
        public static bool view_enable = false;
        ///
        public static string qr_companyname = null;
        public static string qr_partno = null;
        public static string qr_partname = null;
        public static string qr_manf = null;
        public static string qr_expiry = null;
        public static string qr_qty = null;
        public static string qr_pcs = null;
        public static string qr_lotno = null;
        public static string qr_materialcode = null;
        // Bard code values
        public static string barcode_companyname = null;
        public static string barcode_partno = null;
        public static string barcode_partname = null;
        public static string barcode_expiry = null;
        public static string barcode_qty = null;
        public static string barcode_pcs = null;
        public static string barcode_lotno = null;
        public static string barcode_materialcode = null;
        public static string barcode_input_1 = null;
        public static string barcode_input_2 = null;
        public static string pattern_temp_random_number =string.Empty;
        /// 
        public static bool lot_info_changes = false;
        public static bool Superlogin_allow = false;
        public static bool Superlogin_close_btn_click = false;
        //
        public static List<Bulkdata_get_lotmaster> list_bmodel_lotmaster = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_lotmaster_notIn_lotmasterOnly = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_lotmaster_gpby_processid = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_lotmaster_CompareTo_lotmasterOnly = new List<Bulkdata_get_lotmaster>();
        //public static List<Bulkdata_get_lotmaster_only> list_bmodel_lotmaster_only = new List<Bulkdata_get_lotmaster_only>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_lotmaster_only = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_pattern_one> list_bmodel_p1 = new List<Bulkdata_get_pattern_one>();
        public static List<Bulkdata_get_pattern_two> list_bmodel_p2 = new List<Bulkdata_get_pattern_two>();
        public static List<Bulkdata_get_pattern_three> list_bmodel_p3 = new List<Bulkdata_get_pattern_three>();
        public static List<Bulkdata_get_pattern_four> list_bmodel_p4 = new List<Bulkdata_get_pattern_four>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_unmatch_p1 = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_unmatch_p2 = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_unmatch_p3 = new List<Bulkdata_get_lotmaster>();
        public static List<Bulkdata_get_lotmaster> list_bmodel_unmatch_p4 = new List<Bulkdata_get_lotmaster>();
        public static List<PI_Process> Process_name_bulkdata = new List<PI_Process>();
        // Run time next page datagridview use this
        public static List<shipping_custcd_itemcd> Runtime_Store_Print_details = new List<shipping_custcd_itemcd>();
        public static volatile List<shipping_custcd_itemcd> Runtime_Store_Shipping_details = new List<shipping_custcd_itemcd>();
        public static volatile List<shipping_custcd_itemcd> Runtime_Store_PI_lotInfo_details = new List<shipping_custcd_itemcd>();
        public static volatile List<list_of_lotnumbers> Runtime_Store_LI_Infostatus = new List<list_of_lotnumbers>();
        public static string ship_tabActionType_nxtPg = string.Empty;
        public static string lotno_nxtPg = string.Empty;
        public static string lotno_child_frm_nxtPg = string.Empty;
        public static string lotno_child_to_nxtPg = string.Empty;
        public static string manfdt_frm_nxtPg = string.Empty;
        public static string manfdt_to_nxtPg = string.Empty;
        public static string actionTyp2_nxtPg = string.Empty;
        public static string spname_nxtPg = string.Empty;
        public static string ship_frmdt_nxtPg = string.Empty;
        public static string ship_todt_nxtPg = string.Empty;
        public static int curentPageNo_nxtPg = 0;
        public static int curentPageSize_nxtPg = 0;
        public static string round_lotno_nxtPg =string.Empty;

        public static volatile int PI_lotInfo_curentPageNo_nxtPg = 0;
        public static volatile int shipping_curentPageNo_nxtPg = 0;
        public static volatile int shipping_curentPageSize_nxtPg = 0;
        public static volatile string shipping_ship_tabActionType_nxtPg = string.Empty;
        public static volatile string shipping_lotno_nxtPg = string.Empty;
        public static volatile string shipping_lotno_child_frm_nxtPg = string.Empty;
        public static volatile string shipping_lotno_child_to_nxtPg = string.Empty;
        public static volatile string shipping_manfdt_frm_nxtPg = string.Empty;
        public static volatile string shipping_manfdt_to_nxtPg = string.Empty;
        public static volatile string shipping_actionTyp2_nxtPg = string.Empty;
        public static volatile string shipping_actionTyp1_nxtPg = string.Empty;
        public static volatile string shipping_spname_nxtPg = string.Empty;
        public static volatile string shipping_ship_frmdt_nxtPg = string.Empty;
        public static volatile string shipping_ship_todt_nxtPg = string.Empty;

        public static volatile List<PI_Process> Process_name_gridbind_lotinfostatus = new List<PI_Process>();
        public static volatile List<PI_Process> Process_name_gridbind_columns_lotinfostatus = new List<PI_Process>();

        public static volatile List<PI_Process> Process_name_gridbind_lotinfostatus_runtime = new List<PI_Process>();
        public static volatile List<PI_Process> Process_name_gridbind_columns_lotinfostatus_runtime = new List<PI_Process>();

        public static volatile int lotInfo_status_curentPageNo_nxtPg = 0;
        public static volatile string lotInfo_status_actionTyp_nxtPg = string.Empty;
        public static volatile string lotInfo_status_spname_nxtPg = string.Empty;

    }
}

