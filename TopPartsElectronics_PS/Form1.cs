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
using YourApp.Data;

namespace TopPartsElectronics_PS
{
    public partial class Form1 : Form
    {
        MysqlHelper helper = new MysqlHelper();
        public Form1()
        {
            InitializeComponent();            
        }

        private void endProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to exit ?", "Exit Application", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormLogin frm = new FormLogin();
            frm.Owner = this;
            frm.ShowDialog();
        }
        public void CloseThisApp()
        {
            Application.Exit();
        }
        public void UserRoll(string get_roll,string get_roll_Id,string get_user_Id)
        {
            bool sub_menu_m = false;
            bool sub_menu_master = false;
            if (get_roll=="admin")
            {
                mainMenuToolStripMenuItem.Visible = true;
            }
            else if(get_roll=="user")
            {               
                ToolStripItemCollection mainMenu = ((ToolStripMenuItem)menuStrip1.Items[0]).DropDownItems;
                ToolStripItemCollection masterMenu = ((ToolStripMenuItem)menuStrip1.Items[1]).DropDownItems;
                foreach(ToolStripItem menuItem in mainMenu)
                {
                    string current_formId = string.Empty;
                    current_formId = get_formId(menuItem.Text);
                    string[] str_rights = { "@usid", "@rlid", "@fmid", "@ActionType" };
                    string[] obj_rights = { get_user_Id, get_roll_Id, current_formId, "GetRights" };
                    MySqlDataReader sdr_rights = helper.GetReaderByCmd("formsrights_get", str_rights, obj_rights);
                    if (sdr_rights.Read())
                    {
                        string Is_active = sdr_rights["Isactive"].ToString();
                       
                        if (Is_active == "1")
                        {
                            menuItem.Visible = true;
                            sub_menu_m = true;
                        }
                        else if (Is_active == "0")
                        {
                            menuItem.Visible = false;
                        }                       
                    }
                    else
                    {
                        menuItem.Visible = false;
                    }
                    sdr_rights.Close();
                    helper.CloseConnection();
                }
                foreach (ToolStripItem menuItem in masterMenu)
                {
                    string current_formId = string.Empty;
                    current_formId = get_formId(menuItem.Text);
                    string[] str_rights = { "@usid", "@rlid", "@fmid", "@ActionType" };
                    string[] obj_rights = { get_user_Id, get_roll_Id, current_formId, "GetRights" };
                    MySqlDataReader sdr_rights = helper.GetReaderByCmd("formsrights_get", str_rights, obj_rights);
                    if (sdr_rights.Read())
                    {
                        string Is_active = sdr_rights["Isactive"].ToString();                      
                        if (Is_active == "1")
                        {
                            menuItem.Visible = true;
                            sub_menu_master = true;
                        }
                        else if (Is_active == "0")
                        {
                            menuItem.Visible = false;
                        }

                    }
                    else
                    {
                        menuItem.Visible = false;
                    }
                    sdr_rights.Close();
                    helper.CloseConnection();
                }
                if(!sub_menu_m)
                {
                    mainMenuToolStripMenuItem.Visible = false;
                }
                if(!sub_menu_master)
                {
                    masterSetupToolStripMenuItem.Visible = false;
                }
            }
        }
        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUser userw = new FormUser();
            userw.MdiParent = this;
            userw.Show();
            userToolStripMenuItem.Enabled = false;
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClient clientw = new FormClient();
            clientw.MdiParent = this;
            clientw.Show();
            clientToolStripMenuItem.Enabled = false;
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProduct productw = new FormProduct();
            productw.MdiParent = this;
            productw.Show();
            productToolStripMenuItem.Enabled = false;
        }

        private void materialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaterial materialw = new FormMaterial();
            materialw.MdiParent = this;
            materialw.Show();
            materialToolStripMenuItem.Enabled = false;
        }

        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProcess processw = new FormProcess();
            processw.MdiParent = this;
            processw.Show();
            processToolStripMenuItem.Enabled = false;
        }

        private void partsCompositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBOM bomw = new FormBOM();
            bomw.MdiParent = this;
            bomw.Show();
            partsCompositionToolStripMenuItem.Enabled = false;
        }

        private void makerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaker makerw = new FormMaker();
            makerw.MdiParent = this;
            makerw.Show();
            makerToolStripMenuItem.Enabled = false;
        }

        private void productionStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductionStatus statusw = new FormProductionStatus();
            statusw.MdiParent = this;
            statusw.Show();
            productionStatusToolStripMenuItem.Enabled = false;

        }

        private void productionInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductionInput inputw = new FormProductionInput();        
            inputw.MdiParent = this;   
            inputw.Show();
            productionInputToolStripMenuItem.Enabled = false;
        }
        public void reopen()
        {
            FormProductionInput inputw = new FormProductionInput();
            inputw.MdiParent = this;
            inputw.Show();
            productionInputToolStripMenuItem.Enabled = false;
        }

        private void shippingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormShipping shippingw = new FormShipping();
            shippingw.MdiParent = this;
            shippingw.Show();
            shippingToolStripMenuItem.Enabled = false;
        }
        public string get_formId(string formName)
        {
            try
            {
                string formId = string.Empty;
                // user tbl form id get 
                string[] str_usnames = { "@usname", "@ActionType" };
                string[] obj_usnames = { formName, "GetFormName" };
                MySqlDataReader sdr_usnames = helper.GetReaderByCmd("get_user_name", str_usnames, obj_usnames);
                if (sdr_usnames.Read())
                {
                    formId = sdr_usnames["idForms"].ToString();
                }
                sdr_usnames.Close();
                helper.CloseConnection();
                return formId;
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("get_formId",ex);
            }
        }

        private void lotinfostatusStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLotInformationStatus shippingw = new FormLotInformationStatus();
            shippingw.MdiParent = this;
            shippingw.Show();
            lotinfostatusStripMenuItem.Enabled = false;
        }
    }
}
