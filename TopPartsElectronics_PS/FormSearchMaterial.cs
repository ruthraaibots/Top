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
    public partial class FormSearchMaterial : Form
    {
        MysqlHelper helper = new MysqlHelper();
        DateTime nowdate = DateTime.Now;
        DataSet ds = new DataSet();
        string ActionType = string.Empty;   
        public FormSearchMaterial()
        {
            InitializeComponent();
        }

        private void btnSearchMaker_Click(object sender, EventArgs e)
        {
            FormSearchMaker frm = new FormSearchMaker();
            MysqlHelper.call_from_search_material = true;
            frm.Owner = this;
            frm.OwnerName = this.Name;           
            frm.ShowDialog();

        }

        public void btnClose_Click(object sender, EventArgs e)
        {
            MysqlHelper.call_from_search_material = false;
            MysqlHelper.call_from_search_bom = false;
            this.Close();
        }
        public void SetSearchId(string code, string makername)
        {
            txtMakerCode.Text = code;
            txtMakerName.Text = makername;
            FetchMaterialDetails_Makercode_wise(code,string.Empty,string.Empty);
        }
        public void FetchMaterialDetails_Makercode_wise(string maker_code, string material_code,string fullname)
        {
            dGProcess.Refresh();
            ActionType = "GetDataSingleor";
            string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@fname", "@price", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", maker_code, material_code, "", fullname, "", "", "", ActionType };

            ds = helper.GetDatasetByCommandString("material_crud", str, obj);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = null;
                dGProcess.AutoGenerateColumns = false;

                //Set Columns Count
                dGProcess.ColumnCount = 7;

                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";

                dGProcess.Columns[1].Name = "makercode";
                dGProcess.Columns[1].DataPropertyName = "makercode";

                dGProcess.Columns[2].Name = "materialcode";
                dGProcess.Columns[2].DataPropertyName = "materialcode";

                dGProcess.Columns[3].Name = "material_fullname";
                dGProcess.Columns[3].DataPropertyName = "material_fullname";

                dGProcess.Columns[4].Name = "maker_fullname";
                dGProcess.Columns[4].DataPropertyName = "maker_fullname";

                dGProcess.Columns[5].Name = "classification";
                dGProcess.Columns[5].DataPropertyName = "classification";

                dGProcess.Columns[6].Name = "price";
                dGProcess.Columns[6].DataPropertyName = "price";

                dGProcess.DataSource = dt;

                txtMakerCode.Text = dt.Rows[0]["makercode"].ToString();
                txtMakerName.Text = dt.Rows[0]["maker_fullname"].ToString();
                txtMaterialCode.Text = dt.Rows[0]["materialcode"].ToString();
                txtMaterialNameS.Text = dt.Rows[0]["material_fullname"].ToString();

            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = dt;
                //MessageBox.Show("No Records Found");
            }
        }
        private void FormSearchMaterial_Load(object sender, EventArgs e)
        {
            FetchMaterialDetails();
        }
        public void FetchMaterialDetails()
        {
            dGProcess.Refresh();
            ActionType = "GetData";
            string[] str = { "@idmat", "@makercd", "@materialcd", "@clasfy", "@fname", "@price", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", "", "", "", "", "","","", ActionType };

            ds = helper.GetDatasetByCommandString("material_crud", str, obj);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = null;
                dGProcess.AutoGenerateColumns = false;

                //Set Columns Count
                dGProcess.ColumnCount = 7;

                //Add Columns
                dGProcess.Columns[0].Name = "sno";
                dGProcess.Columns[0].DataPropertyName = "sno";

                dGProcess.Columns[1].Name = "makercode";
                dGProcess.Columns[1].DataPropertyName = "makercode";

                dGProcess.Columns[2].Name = "materialcode";
                dGProcess.Columns[2].DataPropertyName = "materialcode";

                dGProcess.Columns[3].Name = "material_fullname";
                dGProcess.Columns[3].DataPropertyName = "material_fullname";

                dGProcess.Columns[4].Name = "maker_fullname";
                dGProcess.Columns[4].DataPropertyName = "maker_fullname";

                dGProcess.Columns[5].Name = "classification";
                dGProcess.Columns[5].DataPropertyName = "classification";

                dGProcess.Columns[6].Name = "price";
                dGProcess.Columns[6].DataPropertyName = "price";

                dGProcess.DataSource = dt;

            }
            else
            {
                DataTable dt = ds.Tables[0];
                dGProcess.DataSource = dt;
                //MessageBox.Show("No Records Found");
            }
        }
        private void FormSearchMaterial_FormClosed(object sender, FormClosedEventArgs e)
        {
            MysqlHelper.call_from_search_material = false;
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                ds = helper.GetDatasetByMaterialcodeNames(txtMaterialCode.Text, txtMakerCode.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dGProcess.DataSource = null;
                    dGProcess.AutoGenerateColumns = false;

                    //Set Columns Count
                    dGProcess.ColumnCount = 7;

                    //Add Columns
                    dGProcess.Columns[0].Name = "sno";
                    dGProcess.Columns[0].DataPropertyName = "sno";

                    dGProcess.Columns[1].Name = "makercode";
                    dGProcess.Columns[1].DataPropertyName = "makercode";

                    dGProcess.Columns[2].Name = "materialcode";
                    dGProcess.Columns[2].DataPropertyName = "materialcode";                   

                    dGProcess.Columns[3].Name = "material_fullname";
                    dGProcess.Columns[3].DataPropertyName = "material_fullname";

                    dGProcess.Columns[4].Name = "maker_fullname";
                    dGProcess.Columns[4].DataPropertyName = "maker_fullname";

                    dGProcess.Columns[5].Name = "classification";
                    dGProcess.Columns[5].DataPropertyName = "classification";

                    dGProcess.Columns[6].Name = "price";
                    dGProcess.Columns[6].DataPropertyName = "price";

                    dGProcess.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No Records Match..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaterialCode.Focus();
                }

            }
        }
        public bool CheckInput()
        {
            bool result = true;
            if (txtMaterialCode.Text == "000000" || txtMakerCode.Text == "000000")
            {
                if (txtMaterialCode.Text.Trim() == "" && txtMakerCode.Text == "")
                {
                    MessageBox.Show("Atleast Fill anyone of this Material Code Or Maker code..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaterialCode.Focus();
                    result = false;
                }

            }
            return result;
        }

        private void dGProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dGProcess.Rows[rowIndex];
            if (MysqlHelper.call_from_search_bom==true)
            {
                ((FormBOM)this.Owner).SetSearchId_material(row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                this.Close();
            }
        }

        private void txtMakerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_Makercode())
                {
                    FetchMaterialDetails_Makercode_wise(txtMakerCode.Text, "","");

                }
            }
        }
        public bool CheckInput_Makercode()
        {
            bool result = true;
            if (txtMakerCode.Text == "000000" || txtMakerCode.Text == "")
            {
                if (txtMakerCode.Text.Trim() == "" && txtMakerCode.Text == "")
                {
                    MessageBox.Show("Maker Code is null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMakerCode.Focus();
                    txtMakerCode.Text = string.Empty;
                    txtMakerCode.Text = string.Empty;
                    txtMakerCode.Text = string.Empty;
                    result = false;
                }

            }
            return result;
        }
        public bool CheckInput_Materialfullname()
        {
            bool result = true;
         
                if (txtMaterialNameS.Text == "")
                {
                    MessageBox.Show("Material short name is null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaterialNameS.Focus();
                    txtMaterialNameS.Text = string.Empty;
                    txtMaterialNameS.Text = string.Empty;
                    txtMaterialNameS.Text = string.Empty;
                    result = false;
                }

            
            return result;
        }
        public bool CheckInput_Materialcode()
        {
            bool result = true;
            if (txtMaterialCode.Text == "000000" || txtMaterialCode.Text == "")
            {
                if (txtMaterialCode.Text.Trim() == "" && txtMaterialCode.Text == "")
                {
                    MessageBox.Show("Material Code is null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaterialCode.Focus();
                    txtMaterialCode.Text = string.Empty;
                    txtMaterialCode.Text = string.Empty;
                    txtMaterialCode.Text = string.Empty;
                    result = false;
                }

            }
            return result;
        }

        private void txtMaterialCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_Materialcode())
                {
                    FetchMaterialDetails_Makercode_wise(string.Empty, txtMaterialCode.Text, string.Empty);

                }
            }
        }

        private void txtMaterialNameS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckInput_Materialfullname())
                {
                    FetchMaterialDetails_Makercode_wise(string.Empty, string.Empty, txtMaterialNameS.Text);

                }
            }
        }
    }
}
