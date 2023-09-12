using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using YourApp.Data;
using System.Security.Cryptography;
using TopPartsElectronics_PS.Helper;
using System.Management;
namespace TopPartsElectronics_PS
{
    public partial class FormLogin : Form
    {     
        private bool Allowlogin = false;
        DateTime nowdate = DateTime.Now;
        public string get_user_roll = string.Empty;
        public string get_user_roll_id = string.Empty;
        public string get_user_id = string.Empty;
        MysqlHelper helper = new MysqlHelper();
        public FormLogin()
        {
            InitializeComponent();
        }
      
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(checkInput())
            {
                string[] str_usname = { "@usname", "@ActionType" };
                string[] obj_usname = { textUserID.Text , "GetUser" };
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
                        get_user_roll = sdr_usname["roll"].ToString();
                        CommonClass.logged_Id= sdr_usname["idusers"].ToString();
                        get_user_roll_id = sdr_usname["roll_id"].ToString();
                        get_user_id = sdr_usname["idusers"].ToString();
                        sdr_usname.Close();
                        helper.CloseConnection();
                        // insert the mac address
                        CommonClass.MacAddress = GetMACAddress();
                        string[] str_insmac = { "@mac_addr","@uid", "@lgdate", "@creat_at", "@upd_at", "@ActionType", "@desp" };
                        string[] obj_insmac = { CommonClass.MacAddress, CommonClass.logged_Id, nowdate.ToString("yyyy-MM-dd"), nowdate.ToString("yyyy-MM-dd HH:mm:ss"),string.Empty, "SaveUserlog", "userlogin" };
                        MySqlDataReader sdrs = helper.GetReaderByCmd("userlog_information", str_insmac, obj_insmac);
                        if (sdrs.Read())
                        {
                            sdrs.Close();
                            helper.CloseConnection();
                        }
                        Allowlogin = true;
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
        public bool readlogin(string passwordText,string db_pwd,string db_pwd_key)
        {
            byte[] dbpwd = Encoding.ASCII.GetBytes(db_pwd);
            byte[] dbpwdkey = Encoding.ASCII.GetBytes(db_pwd_key);
            using (var hmac = new HMACSHA512(dbpwdkey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));
                for(int i=0;i<passwordHash.Length;i++)
                {
                    if (passwordHash[i] != dbpwd[i])
                        return false;
                }
            }
            return true;
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
        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btnLogin.PerformClick();
            }
        }
        private void text_enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.AliceBlue;
        }
        private void text_leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
        }
        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!Allowlogin)
            {
                ((Form1)this.Owner).CloseThisApp();
            }
            else if(Allowlogin)
            {
                ((Form1)this.Owner).UserRoll(get_user_roll,get_user_roll_id,get_user_id);
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
                    if ((bool)mo["IPEnabled"]) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
    }
}
