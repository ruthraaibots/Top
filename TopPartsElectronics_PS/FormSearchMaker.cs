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
    public partial class FormSearchMaker : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;
        public FormSearchMaker()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSearchMaker_Load(object sender, EventArgs e)
        {
            try
            {
                FetchMakerDetails();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        public void FetchMakerDetails()
        {
            dGMaker.Refresh();
            ActionType = "GetData";
            string[] str = { "@idmak", "@makercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", "", "", "", "", "", ActionType };

            ds = helper.GetDatasetByCommandString("maker_crud", str, obj);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGMaker.DataSource = null;
                dGMaker.AutoGenerateColumns = false;

                //Set Columns Count
                dGMaker.ColumnCount = 4;

                //Add Columns
                dGMaker.Columns[0].Name = "sno";
                dGMaker.Columns[0].DataPropertyName = "sno";

                dGMaker.Columns[1].Name = "makercode";
                dGMaker.Columns[1].DataPropertyName = "makercode";

                dGMaker.Columns[2].Name = "shortname";
                dGMaker.Columns[2].DataPropertyName = "shortname";

                dGMaker.Columns[3].Name = "fullname";
                dGMaker.Columns[3].DataPropertyName = "fullname";

                dGMaker.DataSource = dt;

            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGMaker.DataSource = dt;
                //MessageBox.Show("No Records Found");
            }
        }

        private void dGMaker_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGMaker.Rows[rowIndex];
            if (MysqlHelper.call_from_search_material==false)
            {     
                ((FormMaterial)this.Owner).SetSearchId(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
            else if(MysqlHelper.call_from_search_material==true)
            {
                ((FormSearchMaterial)this.Owner).SetSearchId(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
          
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FetchMakerSingle();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckInput()
        {
            bool result = true;
            if (txtMakerCode.Text == "000000" || txtMakerNameS.Text=="")
            {
                if(txtMakerCode.Text.Trim() == "" && txtMakerNameS.Text == "")
                {
                    MessageBox.Show("Atleast Fill anyone of this Maker Code Or Short Name..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMakerNameS.Focus();
                    result = false;
                }
               
            } 
            return result;
        }
        public void ResetInput()
        {
            txtMakerCode.Text = string.Empty;
            txtMakerNameS.Text = string.Empty;      
        }
        public void FetchMakerSingle()
        {
            if (CheckInput())
            {
                dGMaker.Refresh();
                ActionType = "GetDataSingle";
                string[] str = { "@idmak", "@makercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
                string[] obj = { "0", txtMakerCode.Text, "", txtMakerNameS.Text, "", "", ActionType };

                ds = helper.GetDatasetByCommandString("maker_crud", str, obj);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dGMaker.DataSource = null;
                    dGMaker.AutoGenerateColumns = false;

                    //Set Columns Count
                    dGMaker.ColumnCount = 4;

                    //Add Columns
                    dGMaker.Columns[0].Name = "sno";
                    dGMaker.Columns[0].DataPropertyName = "sno";

                    dGMaker.Columns[1].Name = "makercode";
                    dGMaker.Columns[1].DataPropertyName = "makercode";

                    dGMaker.Columns[2].Name = "shortname";
                    dGMaker.Columns[2].DataPropertyName = "shortname";

                    dGMaker.Columns[3].Name = "fullname";
                    dGMaker.Columns[3].DataPropertyName = "fullname";

                    dGMaker.DataSource = dt;

                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dGMaker.DataSource = dt;
                    ResetInput();
                    FetchMakerDetails();
                    MessageBox.Show("No Records Matched..");
                }
            }
        }

        private void FormSearchMaker_KeyDown(object sender, KeyEventArgs e)
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
    }
}
