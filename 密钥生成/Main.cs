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
            textBox2.Text = Encrypt(textBox1.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();//黏贴
            textBox1.Text = (String)iData.GetData(DataFormats.Text); 
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, textBox2.Text);//复制
        }

    }




}


