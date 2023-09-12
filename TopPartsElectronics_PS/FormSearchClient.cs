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
    public partial class FormSearchClient : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        public FormSearchClient()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSearchClient_Load(object sender, EventArgs e)
        {
            FetchClientDetails();
        }
        public void FetchClientDetails()
        {
            dGClient.Refresh();
            ActionType = "GetData";
            string[] str = { "@idcust", "@customercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", "", "", "", "", "", ActionType };

            ds = helper.GetDatasetByCommandString("customer_crud", str, obj);
   
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGClient.DataSource = null;
                dGClient.AutoGenerateColumns = false;

                //Set Columns Count
                dGClient.ColumnCount = 4;

                //Add Columns
                dGClient.Columns[0].Name = "sno";
                dGClient.Columns[0].DataPropertyName = "sno";

                dGClient.Columns[1].Name = "customercode";
                dGClient.Columns[1].DataPropertyName = "customercode";

                dGClient.Columns[2].Name = "shortname";
                dGClient.Columns[2].DataPropertyName = "shortname";

                dGClient.Columns[3].Name = "fullname";
                dGClient.Columns[3].DataPropertyName = "fullname";

                dGClient.DataSource = dt;
                helper.CloseConnection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGClient.DataSource = dt;
                helper.CloseConnection();             
            }
        }

        private void dGClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGClient.Rows[rowIndex];
            if (!MysqlHelper.call_from_ProductionStatus_to_client)
            {
                if (!MysqlHelper.call_from_shipping_to_client)
                {
                    if (!MysqlHelper.call_from_productionInput_to_client)
                    {
                        if(!MysqlHelper.call_from_lotinfomation_status_to_client)
                        {
                            if (!MysqlHelper.call_from_search_client)
                            {
                                ((FormProduct)this.Owner).SetSearchId(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                                this.Close();
                            }
                            else if (MysqlHelper.call_from_search_client)
                            {
                                ((FormBOM)this.Owner).SetSearchId(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                                this.Close();
                            }
                        }
                        else if(MysqlHelper.call_from_lotinfomation_status_to_client)
                        {
                            ((FormLotInformationStatus)this.Owner).SetSearchId_customer(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                            this.Close();
                        }
                        
                    }
                    else if (MysqlHelper.call_from_productionInput_to_client)
                    {                        
                        ((FormProductionInput)this.Owner).SetSearchId(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                        MysqlHelper.call_from_productionInput_to_client = false;
                        this.Close();
                    }
                }
                else if (MysqlHelper.call_from_shipping_to_client)
                {
                    ((FormShipping)this.Owner).SetSearchId(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                    this.Close();
                }
            }
            else if (MysqlHelper.call_from_ProductionStatus_to_client)
            {
                MysqlHelper.call_from_ProductionStatus_to_client = false;
                ((FormProductionStatus)this.Owner).SetSearchClientId(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }             
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(CheckInput())
            {
                ds = helper.GetDatasetByClientcodeNames(txtCustomerCode.Text, txtCustomerNameS.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dGClient.DataSource = null;
                    dGClient.AutoGenerateColumns = false;
                    //Set Columns Count
                    dGClient.ColumnCount = 4;
                    //Add Columns
                    dGClient.Columns[0].Name = "sno";
                    dGClient.Columns[0].DataPropertyName = "sno";

                    dGClient.Columns[1].Name = "customercode";
                    dGClient.Columns[1].DataPropertyName = "customercode";

                    dGClient.Columns[2].Name = "shortname";
                    dGClient.Columns[2].DataPropertyName = "shortname";

                    dGClient.Columns[3].Name = "fullname";
                    dGClient.Columns[3].DataPropertyName = "fullname";

                    dGClient.DataSource = dt;
                    ResetInput();
                    helper.CloseConnection();
                }
            }
            
        }
        public bool CheckInput()
        {
            bool result = true;
            if (txtCustomerCode.Text == "000000" || txtCustomerNameS.Text == "")
            {
                if (txtCustomerCode.Text.Trim() == "" && txtCustomerNameS.Text == "")
                {
                    MessageBox.Show("Atleast Fill anyone of this Maker Code Or Short Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerCode.Focus();
                    result = false;
                }

            }
            return result;
        }
        public void ResetInput()
        {
            txtCustomerCode.Text = "000000";
            txtCustomerNameS.Text = string.Empty;
        }

        private void FormSearchClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnSearch.PerformClick();
            }           
            if (e.KeyCode == Keys.F9)
            {
                btnClose.PerformClick();
            }
        }

        private void FormSearchClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            // production Input - search
            if (MysqlHelper.call_from_search_client==true)
            {
                MysqlHelper.call_from_search_client = false;
            }
            // production Input 
            MysqlHelper.call_from_productionInput_to_client = false;
            // Shipping  
            MysqlHelper.call_from_shipping_to_client = false;
            // production status
            MysqlHelper.call_from_ProductionStatus_to_client = false;
        }

        private void txtCustomerCode_Leave(object sender, EventArgs e)
        {
            if (txtCustomerCode.Text != string.Empty)
            {
                int formate_type = Convert.ToInt32(txtCustomerCode.Text);
                txtCustomerCode.Text = formate_type.ToString("D6");
            }
        }
    }
}
