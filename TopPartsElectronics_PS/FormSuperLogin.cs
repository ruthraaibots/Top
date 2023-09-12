using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopPartsElectronics_PS.Helper;
using YourApp.Data;

namespace TopPartsElectronics_PS
{
    public partial class FormSuperLogin : Form
    {
       
        DateTime nowdate = DateTime.Now;       
        MysqlHelper helper = new MysqlHelper();
        bool close_button_not_click = false;
        public FormSuperLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    string[] str_usname = { "@usname", "@ActionType" };
                    string[] obj_usname = { textUserID.Text , "GetSpUser" };
                    MySqlDataReader sdr_usname = helper.GetReaderByCmd("get_user_name", str_usname, obj_usname);
                    if (sdr_usname.Read())
                    {
                        string get_user_pwd = sdr_usname["pwd"].ToString();
                        byte[] hashBytes = Convert.FromBase64String(get_user_pwd);
                        byte[] salt = new byte[16];
                        Array.Copy(hashBytes, 0, salt, 0, 16);
                        var pbkdf2 = new Rfc2898DeriveBytes(textPassword.Text, salt, 10000);
                        byte[] hash = pbkdf2.GetBytes(20);
                        int ok = 1;
                        for (int i = 0; i < 20; i++)
                        {
                            if (hashBytes[i + 16] != hash[i])
                            {
                                ok = 0;
                            }
                        }
                        if (ok == 1)
                        {
                            //get_user_roll = sdr_usname["roll"].ToString();
                            CommonClass.logged_Id = sdr_usname["idusers"].ToString();
                            sdr_usname.Close();
                            helper.CloseConnection();
                            DateTime current_date_time = DateTime.Now;
                            // insert the mac address
                            CommonClass.MacAddress = GetMACAddress();
                            string[] str_insmac = { "@mac_addr", "@uid", "@lgdate", "@creat_at", "@upd_at", "@ActionType", "@desp" };
                            string[] obj_insmac = { CommonClass.MacAddress, CommonClass.logged_Id, nowdate.ToString("yyyy-MM-dd"), current_date_time.ToString("yyyy-MM-dd HH:mm:ss"), string.Empty, "SaveUserlog","superlogin" };
                            MySqlDataReader sdrs = helper.GetReaderByCmd("userlog_information", str_insmac, obj_insmac);
                            if (sdrs.Read())
                            {
                                sdrs.Close();
                                helper.CloseConnection();
                            }
                            CommonClass.Superlogin_allow = true;
                            close_button_not_click = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Enter The Valid User Name And Password", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        sdr_usname.Close();
                        helper.CloseConnection();
                    }
                    sdr_usname.Close();
                    helper.CloseConnection();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
        public bool checkInput()
        {

            bool result = true;
            if (textUserID.Text == "")
            {
                MessageBox.Show("User Id is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textUserID.Focus();
                result = false;
            }
            else if (textPassword.Text == "")
            {
                MessageBox.Show("Password is Null..", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textPassword.Focus();
                result = false;
            }
            return result;
        }

        private void FormSuperLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!close_button_not_click)
            {
                CommonClass.Superlogin_close_btn_click = true;
            }
            this.Close();
            
        }

        private void FormSuperLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btnLogin.PerformClick();
            }
        }

        private void FormSuperLogin_Load(object sender, EventArgs e)
        {
            textPassword.Text = string.Empty;
        }
    }
}
