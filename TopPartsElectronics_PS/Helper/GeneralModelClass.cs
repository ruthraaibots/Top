using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopPartsElectronics_PS.Helper
{
    class GeneralModelClass
    {
        
        public class PI_Process
        {
            public string ProcessNames { get; set; }
            public string id { get; set; }
            public string PaternType { get; set; }
            public string process_id { get; set; }

            public string view_lotno { get; set; }
            public string view_lotno_child { get; set; }
            public string button_color { get; set; }
            public string itemcode { get; set; }
            public string materialcode { get; set; }
            //   
        }
        public class PI_master_use_insert
        {
            public string id { get; set; }
            public string lotno { get; set; }
            public string Customercode { get; set; }
            public string CustomerFnam { get; set; }
            public string CustomerSnam { get; set; }
            public string Itemcode { get; set; }
            public string Itemnam { get; set; }
            public string Unittype { get; set; }
            public string Unitprice { get; set; }
            public string Boxqty { get; set; }
            public string Addcd { get; set; }
            public string lbltype { get; set; }
            public string m1 { get; set; }
            public string m2 { get; set; }
            public string m3 { get; set; }
            public string m4 { get; set; }

            //   
        }
        public class PatientNoteGrid2_Insert
        {
            public int id { get; set; }
            public string patientNote_uniqueId { get; set; }
            public string icPasspoartNo { get; set; }
            public string VN { get; set; }
            public string clientid { get; set; }
            public string packid { get; set; }
            public string testid { get; set; }
            public string testname { get; set; }

            public string gender { get; set; }
            public string tage { get; set; }
            public string discription { get; set; }
            public string price { get; set; }
            public string category { get; set; }
            public string Enroll_date { get; set; }
            public string UserId { get; set; }

            public string manuallyAdd { get; set; }
        }

        public class Lotinfo_gridbind_common
        {
            public string pattern_type { get; set; }
            // pattern 1
            public int lotno { get; set; }
            public string lotnojoin_p1 { get; set; }
            public string processId_p1 { get; set; }
            public string processName_p1 { get; set; }
            public string partno_p1 { get; set; }
            public string quantity_p1 { get; set; }
            public string planting_p1 { get; set; }
            public string pb_dt_p1 { get; set; }
            // pattern 2 
            public string lotnojoin_p2 { get; set; }
            public string processId_p2 { get; set; }
            public string processName_p2 { get; set; }
            public string process_date_p2 { get; set; }
            public string contorlno_p2 { get; set; }       
            public string slot_no_p2 { get; set; }
            public string quantity_p2 { get; set; }
            // pattern 3 
            public string lotnojoin_p3 { get; set; }
            public string processId_p3 { get; set; }
            public string processName_p3 { get; set; }
            public string process_date_p3 { get; set; }
            public string quantity_p3 { get; set; }
            // pattern 4
            public string lotnojoin_p4 { get; set; }
            public string processId_p4 { get; set; }
            public string processName_p4 { get; set; }
            public string partno_p4 { get; set; }
            public string quantity_p4 { get; set; }
        }
        public class Lotinfo_gridbind_common_pattern_new
        {

            public int lotno { get; set; }
            public string lotnojoin_p1 { get; set; }
            public string processId_p1 { get; set; }
            public string processName_p1 { get; set; }
            public string partno_p1 { get; set; }
            public string quantity_p1 { get; set; }
            public string planting_p1 { get; set; }
            public string pb_dt_p1 { get; set; }
            public string pattern_type { get; set; }

            public string tb_manuf_dt_p1 { get; set; }
            public string tb_expairy_dt_p1 { get; set; }
            public string tb_qty_p1 { get; set; }
            public string lotno_p1 { get; set; }
            public string materialcd { get; set; }
            public string bproduct_p1 { get; set; }
            public string onhold_p1 { get; set; }
            public string scrap_p1 { get; set; }
            public string reason_hs_p1 { get; set; }

            //


        }
        public class Lotinfo_gridbind_common_pattern_new_ship
        {

            public int lotno { get; set; }
            public string lotnojoin_p1 { get; set; }
            public string processId_p1 { get; set; }
            public string processName_p1 { get; set; }
            public string partno_p1 { get; set; }
            public string quantity_p1 { get; set; }
            public string planting_p1 { get; set; }
            public string pb_dt_p1 { get; set; }
            public string pattern_type { get; set; }

            public string tb_manuf_dt_p1 { get; set; }
            public string tb_expairy_dt_p1 { get; set; }
            public string tb_qty_p1 { get; set; }
            public string lotno_p1 { get; set; }
            public string materialcd { get; set; }
            public string bproduct_p1 { get; set; }
            public string onhold_p1 { get; set; }
            public string scrap_p1 { get; set; }
            public string reason_hs_p1 { get; set; }

            public string shipment_date { get; set; }
            //


        }
        public class Lotinfo_gridbind_common_pattern_new_
        {
            public string pattern_type { get; set; }
            public string lotnojoin { get; set; }
            public string processId { get; set; }
            public string processName { get; set; }
            // pattern 1
            public string partno { get; set; }
            public string lotno { get; set; }
            public string lotno_p1 { get; set; }
            public string plantingdate { get; set; }
            public string qty { get; set; }
            public string controlno { get; set; }
            public string pb_date { get; set; }
            //terminal board 
            public string tb_manuf_dt { get; set; }
            public string tb_expairy_dt { get; set; }
            public string tb_qty { get; set; }
            public string tb_bproduct { get; set; }
            public string onhold { get; set; }
            public string scrap { get; set; }
            public string reason_hs { get; set; }


        }
        public class Lotinfo_gridbind_common_pattern
        {

            public string pattern_type { get; set; }
            public string lotnojoin { get; set; }
            public string processId { get; set; }
            public string processName { get; set; }
            // pattern 1
            public string partno { get; set; }
            public string lotno { get; set; }
            public string lotno_p1 { get; set; }
            public string plantingdate { get; set; }
            public string qty { get; set; }
            public string controlno { get; set; }
            public string pb_date { get; set; }
            //terminal board 
            public string tb_manuf_dt { get; set; }
            public string tb_expairy_dt { get; set; }
            public string tb_qty { get; set; }
            public string tb_bproduct { get; set; }
            public string onhold { get; set; }
            public string scrap { get; set; }
            public string reason_hs { get; set; }
            public string logged_Id { get; set; }
            //pattern 4 
            public string lotno_p4 { get; set; }
            //pattern 2
            public string sheetlotno_p2 { get; set; }
            //
            public string material_code { get; set; }
            //pattern 3
            public string shipment_date { get; set; }
            //general
            public string customer_code { get; set; }
            public string item_code { get; set; }
            public string customer_name { get; set; }
            public string item_name { get; set; }
        }
        public class Lotinfo_gridbind_common_pattern_ship
        {

            public string pattern_type { get; set; }
            public string lotnojoin_p1 { get; set; }
            public string processId_p1 { get; set; }
            public string processName_p1 { get; set; }
            // pattern 1
            public string partno_p1 { get; set; }
            public string lotno { get; set; }
            public string lotno_p1 { get; set; }
            public string planting_p1 { get; set; }
            public string quantity_p1 { get; set; }
            public string controlno { get; set; }
            public string pb_dt_p1 { get; set; }
            //terminal board 
            public string tb_manuf_dt_p1 { get; set; }
            public string tb_expairy_dt_p1 { get; set; }
            public string tb_qty_p1 { get; set; }
            public string tb_bproduct { get; set; }
            public string onhold { get; set; }
            public string scrap { get; set; }
            public string reason_hs { get; set; }
            public string logged_Id { get; set; }
            //pattern 4 
            public string lotno_p4 { get; set; }
            //pattern 2
            public string sheetlotno_p2 { get; set; }
            //
            public string materialcd { get; set; }
            //pattern 3
            public string shipment_date { get; set; }
            //general
            public string customer_code { get; set; }
            public string item_code { get; set; }
            public string customer_name { get; set; }
            public string item_name { get; set; }
        }
        public class Lotinfo_gridbind_p1
        {
            // pattern 1
            public int lotno { get; set; }
            public string pk_p1 { get; set; }
            public string pk_lotinfo_mast { get; set; }
            public string lotnojoin_p1 { get; set; }
            public string processId_p1 { get; set; }
            public string processName_p1 { get; set; }
            public string partno_p1 { get; set; }
            public string quantity_p1 { get; set; }
            public string planting_p1 { get; set; }
            public string pb_dt_p1 { get; set; }
            public string pattern_type { get; set; }
            public string tb_manuf_dt_p1 { get; set; }
            public string tb_expairy_dt_p1 { get; set; }
            public string tb_qty_p1 { get; set; }
            public string lotno_p1 { get; set; }
            public string materialcd { get; set; }
            public string shipment_date { get; set; }
            public string bproduct_p1 { get; set; }
            public string onhold_p1 { get; set; }
            public string scrap_p1 { get; set; }
            //public string remarks_p1 { get; set; }
            public string reason_hs_p1 { get; set; }
        }
        public class Lotinfo_gridbind_p2
        {
            // pattern 2 
            public string lotno { get; set; }
            public string pattern_type { get; set; }
            public string lotnojoin_p2 { get; set; }
            public string processId_p2 { get; set; }
            public string processName_p2 { get; set; }
            public string process_date_p2 { get; set; }
            public string contorlno_p2 { get; set; }
            public string slot_no_p2 { get; set; }
            public string quantity_p2 { get; set; }         
           // public string planting_p2 { get; set; }
           // public string pb_dt_p2 { get; set; }  
            public string tb_manuf_dt_p2 { get; set; }
            public string tb_expairy_dt_p2 { get; set; }
            public string tb_qty_p2 { get; set; }
            //public string lotno_p2 { get; set; }
            // public string materialcd { get; set; }
            public string bproduct_p2 { get; set; }
            public string onhold_p2 { get; set; }
            public string scrap_p2 { get; set; }
            public string reason_hs_p2 { get; set; }
            public string sheet_lotno_p2 { get; set; }
            public string materialcd { get; set; }

        }
        public class Lotinfo_gridbind_p3
        {
            // pattern 3
            public string lotno { get; set; }
            public string pattern_type { get; set; }
            public string lotnojoin_p3 { get; set; }
            public string processId_p3 { get; set; }
            public string processName_p3 { get; set; }
            public string process_date_p3 { get; set; }
            public string quantity_p3 { get; set; }

           // public string planting_p3 { get; set; }
            //public string pb_dt_p3 { get; set; }

            public string tb_manuf_dt_p3 { get; set; }
            public string tb_expairy_dt_p3 { get; set; }
            public string tb_qty_p3 { get; set; }
           // public string lotno_p3 { get; set; }
             public string materialcd { get; set; }
            public string bproduct_p3 { get; set; }
            public string onhold_p3 { get; set; }
            public string scrap_p3 { get; set; }
            public string shipment_date { get; set; }
            public string reason_hs_p3 { get; set; }

        }
        public class Lotinfo_gridbind_p4
        {
            // pattern 4
            public string  lotno{ get; set; }
            public string pattern_type { get; set; }
            public string lotnojoin_p4 { get; set; }
            public string processId_p4 { get; set; }
            public string processName_p4 { get; set; }
            public string partno_p4 { get; set; }
            public string quantity_p4 { get; set; }

           // public string planting_p4 { get; set; }
            //public string pb_dt_p4{ get; set; }

            public string tb_manuf_dt_p4 { get; set; }
            public string tb_expairy_dt_p4 { get; set; }
            public string tb_qty_p4 { get; set; }
            public string lotno_p4 { get; set; }
             public string materialcd { get; set; }
            public string bproduct_p4 { get; set; }
            public string onhold_p4 { get; set; }
            public string scrap_p4 { get; set; }
            public string reason_hs_p4 { get; set; }
        }
        public class Lotinfo_gridbind_p2_ship
        {     
            // pattern 2 
            public string lotno { get; set; }
            public string lotnojoin_p2 { get; set; }
            public string pattern_type { get; set; }
            public string processId_p2 { get; set; }
            public string processName_p2 { get; set; }
            public string process_date_p2 { get; set; }
            public string contorlno_p2 { get; set; }
            public string slot_no_p2 { get; set; }
            public string quantity_p2 { get; set; }    
            public string tb_manuf_dt_p2 { get; set; }
            public string tb_expairy_dt_p2 { get; set; }
            public string tb_qty_p2 { get; set; }
            public string sheet_lotno_p2 { get; set; }
            public string materialcd { get; set; }
            public string shipment_date { get; set; }
            public string bproduct_p2 { get; set; }
            public string onhold_p2 { get; set; }
            public string scrap_p2 { get; set; }
            public string reason_hs_p2 { get; set; }

        }
        public class Lotinfo_gridbind_p3_ship
        {
            // pattern 3
            public string lotno { get; set; }
            public string pk_p3 { get; set; }
            public string pk_lotinfo_mast { get; set; }
       
            public string lotnojoin_p3 { get; set; }
            public string processId_p3 { get; set; }
            public string processName_p3 { get; set; }
            public string process_date_p3 { get; set; }

            public string quantity_p3 { get; set; }
            public string pattern_type { get; set; }      

            public string tb_manuf_dt_p3 { get; set; }
            public string tb_expairy_dt_p3 { get; set; }
            public string tb_qty_p3 { get; set; }
            // public string lotno_p3 { get; set; }
            public string shipment_date { get; set; }
            public string materialcd { get; set; }
            public string bproduct_p3 { get; set; }
            public string onhold_p3 { get; set; }
            public string scrap_p3 { get; set; }
            public string reason_hs_p3 { get; set; }

            //public string reason_hs_p3 { get; set; }

        }
        public class Lotinfo_gridbind_p4_ship
        {
            // pattern 4
            public string lotno { get; set; }
            public string pk_p4 { get; set; }
            public string pk_lotinfo_mast { get; set; }
            
            public string lotnojoin_p4 { get; set; }
            public string processId_p4 { get; set; }
            public string processName_p4 { get; set; }
            public string partno_p4 { get; set; }
            public string quantity_p4 { get; set; }
            public string pattern_type { get; set; }
            // public string planting_p4 { get; set; }
            //public string pb_dt_p4{ get; set; }

            public string tb_manuf_dt_p4 { get; set; }
            public string tb_expairy_dt_p4 { get; set; }
            public string tb_qty_p4 { get; set; }
            public string lotno_p4 { get; set; }
            public string materialcd { get; set; }
            public string bproduct_p4 { get; set; }
            public string onhold_p4 { get; set; }
            public string scrap_p4 { get; set; }
            public string reason_hs_p4 { get; set; }
           // public string remarks_p4 { get; set; }
            public string shipment_date { get; set; }
            
        }
        public class Lotinfo_only_tbl
        {
            public string lotno { get; set; }
            public string lot_no_child { get; set; }
            public string lotnoJoin { get; set; }
            public string itemcode { get; set; }
            public string manufacturing_date { get; set; }
            public string expairy_date { get; set; }
            public string manufacturing_time { get; set; }
            public string lotqty { get; set; }
            public string flag_only_lotno { get; set; }
            public string print_label_status { get; set; }
            public string print_lable_date { get; set; }
            public string shipment_flag { get; set; }
            public string shipment_date { get; set; }
            public string bproduct { get; set; }    
            public string onhold { get; set; }
            public string scrap { get; set; }
            public string reason_hs { get; set; }
        }
        public class qrcode_details
        {
            public string pk_lotinfo_id { get; set; }
            public string qr_companyname { get; set; }
            public string qr_partno { get; set; }
            public string qr_partname { get; set; }
            public string qr_manf { get; set; }
            public string qr_expiry { get; set; }
            public string qr_qty { get; set; }
            public string qr_pcs { get; set; }
            public string qr_lotno { get; set; }
            public string qr_materialcode { get; set; }
            public byte[] qr_imageurl { get; set; }
            public string qr_m1 { get; set; }
            public string qr_m2 { get; set; }
            public string qr_m3 { get; set; }
            public string qr_m4 { get; set; }
            public string print_person_name { get; set; }
            public string printed_date { get; set; }
            public string print_copy { get; set; }
        }

        public class barcode1_details
        {
            public string pk_lotinfo_id { get; set; }
            public string barcode_companyname { get; set; }
            public string barcode_partno { get; set; }
            public string barcode_partname { get; set; }
            public string barcode_expiry { get; set; }
            public string barcode_qty { get; set; }
            public string barcode_pcs { get; set; }
            public string barcode_lotno { get; set; }
            public string barcode_materialcode { get; set; }
            public byte[] barcode_input_1 { get; set; }
            public byte[] barcode_input_2 { get; set; }
            public string imageUrl_barcode_1 { get; set; }
            public string imageUrl_barcode_2 { get; set; }
            public string barcode_m1 { get; set; }
            public string barcode_m2 { get; set; }
            public string barcode_m3 { get; set; }
            public string barcode_m4 { get; set; }
            public string print_person_name { get; set; }
            public string printed_date { get; set; }
            public string print_copy { get; set; }

        }
        public class shippingUpdate
        {
            public string lotno { get; set; }
            public string lotno_from { get; set; }
            public string lotno_to { get; set; }
            public string pk_p3 { get; set; }
            public string pk_lotinfo_ms { get; set; }
            public string lotno_child { get; set; }
        }
        public class all_pattern_and_termialboard
        {
            public string lotno { get; set; }
            public string lotno_from { get; set; }
            public string lotno_to { get; set; }
            public string join_lotno { get; set; }
            //Terminal Board info
            public string ter_qty { get; set; }
            public string ter_manf_dt { get; set; }
            public string ter_expy_dt { get; set; }
            // Cleaning
            public string cle_proc_dt { get; set; }
            public string cle_qty { get; set; }
            // Insception
            public string ins_proc_dt { get; set; }
            public string ins_qty { get; set; }
            //Terminal 1
            public string ter1_lotno { get; set; }
            public string ter1_plat_dt { get; set; }
            public string ter1_qty_dt { get; set; }
            public string ter1_pb_date { get; set; }
            //Terminal 1
            public string ter2_lotno { get; set; }
            public string ter2_plat_dt { get; set; }
            public string ter2_qty_dt { get; set; }
            public string ter2_pb_date { get; set; }
            //Terminal 1
            public string ter3_lotno { get; set; }
            public string ter3_plat_dt { get; set; }
            public string ter3_qty_dt { get; set; }
            public string ter3_pb_date { get; set; }
            //Terminal 1
            public string ter4_lotno { get; set; }
            public string ter4_plat_dt { get; set; }
            public string ter4_qty_dt { get; set; }
            public string ter4_pb_date { get; set; }
            // punched backlift
            public string pun_proc_dt { get; set; }
            public string pun_ctrl_no { get; set; }
            public string pun_sheet_lno { get; set; }
            public string pun_qty { get; set; }
            // Washer 1
            public string wh1_part_no { get; set; }
            public string wh1_lot_no { get; set; }
            public string wh1_qty { get; set; }
            // Washer 2
            public string wh2_part_no { get; set; }
            public string wh2_lot_no { get; set; }
            public string wh2_qty { get; set; }
            // pp sheet
            public string pps_part_no { get; set; }
            public string pps_lot_no { get; set; }
            public string pps_qty { get; set; }

        }
        public class shipping_custcd_itemcd
        {
            public string customer_code { get; set; }
            public string customer_name { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string lotno { get; set; }
            public DateTime manfdt { get; set; }
        }
        public class shipping_unique_processid_find
        {
            public string customer_code { get; set; }
            public string item_code { get; set; }
            public string lotno { get; set; }
            public string processId { get; set; }
        }
        public class Bulkdata_get_lotmaster
        {
            public string pk_lot_mast { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string customer_code { get; set; }
            public string process_id { get; set; }
            public string material_code { get; set; }
            public string processname { get; set; }
            public string pattern_type { get; set; }
            public string Bproduct { get; set; }
            public string onHold { get; set; }
            public string scrap { get; set; }
            public string reason { get; set; }
        }        
        public class Bulkdata_get_lotmaster_only
        {
            public string pk_lot_mast { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string customer_code { get; set; }
            public string process_id { get; set; }
            public string material_code { get; set; }
        }
        public class Bulkdata_get_pattern_one
        {
            public string pk_idpattern_one { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string customer_code { get; set; }
            public string process_id_one { get; set; }
            public string material_code_one { get; set; }
        }
        public class Bulkdata_get_pattern_two
        {
            public string pk_idpattern_two { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string customer_code { get; set; }
            public string process_id_two { get; set; }
            public string material_code_two { get; set; }
        }
        public class Bulkdata_get_pattern_three
        {
            public string pk_idpattern_three { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string customer_code { get; set; }
            public string process_id_three { get; set; }
            public string material_code_three { get; set; }
        }
        public class Bulkdata_get_pattern_four
        {
            public string pk_idpattern_four { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string customer_code { get; set; }
            public string process_id_four { get; set; }
            public string material_code_four { get; set; }
        }
        public class Get_Range_values
        {
            public string pi_lotms_child { get; set; }
            public string pi_lotonly_child { get; set; }
        }
        public class lotinfo_main_table
        {

            public int sno { get; set; }
            public int lot_no { get; set; }
            public int lot_no_child { get; set; }
            public string Lotno { get; set; }
            public string idproduction_input_master { get; set; }
            public string manufacturing_date { get; set; }
            public string lotqty { get; set; }
            public string printdate { get; set; }
            public string additional_code { get; set; }
            public string expairy_dt { get; set; }

            public string m1 { get; set; }
            public string m2 { get; set; }
            public string m3 { get; set; }
            public string m4 { get; set; }
            public string lable_typ { get; set; }
            public string customerfull_name { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string lot_item_name { get; set; }

            public string box_qty { get; set; }
            public string customercode { get; set; }
            public string customershort_name { get; set; }
            public string print_person_name { get; set; }
            public string printed_date_join { get; set; }
            public string printed_names_join { get; set; }
            public string printed_copy_join { get; set; }
            
            //


        }
        public class Get_printed_date
        { 
            public string id { get; set; }
            public string lot_no { get; set; }
            public string lot_no_child { get; set; }
            public string print_lable_date { get; set; } = null;
            public string print_lable_status { get; set; } = null;
            public string print_person_name { get; set; } = null;
            public string printed_date_join { get; set; } = null;
            public string printed_names_join { get; set; } = null;
            public string printed_copy_join { get; set; } = null;

        }
        public class productlist
        {
            public string sno { get; set; }
            public string lotno { get; set; }
            public string customercode { get; set; }
            public string customershort_name { get; set; }
            public string customerfull_name { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string unit_price_country_shortcd { get; set; }
            public string unit_price { get; set; }
            public string box_qty { get; set; }
            public string lable_typ { get; set; }
            public string m1 { get; set; }
            public string m2 { get; set; }
            public string m3 { get; set; }
            public string m4 { get; set; }
            public string additional_code { get; set; }
            public string idpi_product_information { get; set; }
          
        }
        public class lotnumbers
        {
            public string lotno { get; set; }
        }
        public class list_of_lotnumbers
        {
            public string lotno { get; set; }
            public string customercode { get; set; }
            public string item_code { get; set; }
            public DateTime manf_dt { get; set; }
            public DateTime manf_dt_lotonly { get; set; }
        }
    }
}
