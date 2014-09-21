using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Security.Cryptography;
using System.IO;

namespace 被加密程序实例
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string DiskId, CpuId, EcryptId;
        private void button1_Click(object sender, EventArgs e)
        {
            CpuId=GetCpuInfo();
        }

        private string GetCpuInfo()
        {
            try
            {
                string cpuInfo = "";//cpu序列号 
                ManagementClass cimobject = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = cimobject.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                return cpuInfo;
            }
            catch
            {
               // this.senRegeditID.Enabled = false;
               // this.GetId.Enabled = true;
                return "";
            }

            return "";
        }

        static public string Encrypt(string PlainText)
        {
            string KEY_64 = "dafei250";
            string IV_64 = "DAFEI500";

            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(PlainText);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        //解密 
        public static string Decrypt(string CypherText)
        {
            //string KEY_64 = "haeren55"; //必须是8个字符(64Bit) 
            //string IV_64 = "HAEREN55";  //必须8个字符(64Bit) 
            string KEY_64 = "dafei250";
            string IV_64 = "DAFEI500";


            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

                byte[] byEnc;
                try
                {
                    byEnc = Convert.FromBase64String(CypherText);
                }
                catch
                {
                    return null;
                }

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                return sr.ReadToEnd();
            }
            catch { return "无法解密!"; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EcryptId = Encrypt(CpuId);
            label3.Text = EcryptId;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == Encrypt(GetCpuInfo()))
            {
                label4.Text = "结果：验证通过！";
            }
            else
            {
                label4.Text = "结果：密钥错误！";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = GetCpuInfo();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, GetCpuInfo());//复制
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();//黏贴
            textBox2.Text = (String)iData.GetData(DataFormats.Text); 
        }

    }




}


