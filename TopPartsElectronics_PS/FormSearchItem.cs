using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YourApp.Data;

namespace TopPartsElectronics_PS
{
    public partial class FormSearchItem : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DataSet ds = new DataSet();
        string ActionType = string.Empty;   
        public FormSearchItem()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSearchItem_Load(object sender, EventArgs e)
        {
            txtCustomerCode.Text = CustomerCode;
            txtCustomerNameS.Text = CustomerNames;
            txtCustomerNameF.Text = CustomerNameF;

            FetchProductDetails(CustomerCode, "");

        }       
        public void FetchProductDetails(string custcd, string shortname)
        {
            dGProcess.Refresh();
            ActionType = "GetData";
            string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
            string[] obj = { custcd, shortname, "", ActionType };
            ds = helper.GetDatasetByCommandString("product_view", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                txtCustomerCode.Text = dt.Rows[0]["customercode"].ToString();
                txtCustomerNameF.Text = dt.Rows[0]["fullname"].ToString();
                txtCustomerNameS.Text = dt.Rows[0]["shortname"].ToString();

                dGProcess.DataSource = null;
                dGProcess.AutoGenerateColumns = false;

                //Set Columns Count
                dGProcess.ColumnCount = 17;

                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";

                dGProcess.Columns[1].Name = "customercode";
                dGProcess.Columns[1].DataPropertyName = "customercode";


                dGProcess.Columns[2].Name = "itemcode";
                dGProcess.Columns[2].DataPropertyName = "itemcode";

                dGProcess.Columns[3].Name = "itemname";
                dGProcess.Columns[3].DataPropertyName = "itemname";

                dGProcess.Columns[4].Name = "unitprice";
                dGProcess.Columns[4].DataPropertyName = "unitprice";

                dGProcess.Columns[5].Name = "boxqty";
                dGProcess.Columns[5].DataPropertyName = "boxqty";

                dGProcess.Columns[6].Name = "additional_code";
                dGProcess.Columns[6].DataPropertyName = "additional_code";

                dGProcess.Columns[7].Name = "mark_1";
                dGProcess.Columns[7].DataPropertyName = "mark_1";

                dGProcess.Columns[8].Name = "mark_2";
                dGProcess.Columns[8].DataPropertyName = "mark_2";

                dGProcess.Columns[9].Name = "mark_3";
                dGProcess.Columns[9].DataPropertyName = "mark_3";

                dGProcess.Columns[10].Name = "mark_4";
                dGProcess.Columns[10].DataPropertyName = "mark_4";

                dGProcess.Columns[11].Name = "labletype";
                dGProcess.Columns[11].DataPropertyName = "labletype";

                dGProcess.Columns[12].Name = "idproduct";
                dGProcess.Columns[12].DataPropertyName = "idproduct";
                dGProcess.Columns[12].Visible = false;


                dGProcess.Columns[13].Name = "unitprice_drp";
                dGProcess.Columns[13].DataPropertyName = "unitprice_drp";
                dGProcess.Columns[13].Visible = false;

                dGProcess.Columns[14].Name = "fullname";
                dGProcess.Columns[14].DataPropertyName = "fullname";
                dGProcess.Columns[14].Visible = false;

                dGProcess.Columns[15].Name = "shortname";
                dGProcess.Columns[15].DataPropertyName = "shortname";
                dGProcess.Columns[15].Visible = false;

                dGProcess.Columns[16].Name = "edit_allow_flag";
                dGProcess.Columns[16].DataPropertyName = "edit_allow_flag";
                dGProcess.Columns[16].Visible = false;
                dGProcess.DataSource = dt;

            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = dt;             
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            dGProcess.Refresh();    
            gridbind("GetDataSingle",string.Empty);           
        }

        private void dGProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGProcess.Rows[rowIndex];
            if (MysqlHelper.call_from_search_bom)
            {
                ((FormBOM)this.Owner).SetSearchId_Item(row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
            else if (MysqlHelper.call_from_productionInput_to_item)
            {
                ((FormProductionInput)this.Owner).SetSearchId_Item(txtCustomerCode.Text, row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
            else if (MysqlHelper.call_from_shipping_to_item)
            {
                ((FormShipping)this.Owner).SetSearchId_Item(txtCustomerCode.Text, row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
            else if (MysqlHelper.call_from_ProductionStatus_to_item)
            {
                ((FormProductionStatus)this.Owner).SetSearchId_Item(txtCustomerCode.Text, row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
            else if(MysqlHelper.call_from_lotinfomation_status_to_item)
            {
                ((FormLotInformationStatus)this.Owner).SetSearchId_Item_lotinfo_sts(txtCustomerCode.Text, row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
        }

        private void FormSearchItem_FormClosed(object sender, FormClosedEventArgs e)
        {
            MysqlHelper.call_from_search_bom = false;
            MysqlHelper.call_from_productionInput_to_item = false;
            MysqlHelper.call_from_shipping_to_item = false;
            MysqlHelper.call_from_ProductionStatus_to_item = false;
        }
        public void gridbind(string ActionType_all,string customercode)
        {
            try
            {
                string[] str = { "@custcd", "@sname", "@itmcd", "@ActionType" };
                string[] obj = { customercode, string.Empty, textItemCode.Text, ActionType_all };
                ds = helper.GetDatasetByCommandString("product_view", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = null;
                    dGProcess.AutoGenerateColumns = false;
                    //Set Columns Count
                    dGProcess.ColumnCount = 4;
                    //Add Columns
                    dGProcess.Columns[0].Name = "sno";
                    dGProcess.Columns[0].DataPropertyName = "sno";

                    dGProcess.Columns[1].Name = "customercode";
                    dGProcess.Columns[1].DataPropertyName = "customercode";

                    dGProcess.Columns[2].Name = "itemcode";
                    dGProcess.Columns[2].DataPropertyName = "itemcode";

                    dGProcess.Columns[3].Name = "itemname";
                    dGProcess.Columns[3].DataPropertyName = "itemname";
                    dGProcess.DataSource = dt;

                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = dt;              
                }

            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("gridbind",ex);
            }
        }

        private void textItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textItemCode.Text != string.Empty && txtCustomerCode.Text != string.Empty)
            {
                    gridbind("GetDataCustomerItem",txtCustomerCode.Text);
         
            }
        }
    }
}
