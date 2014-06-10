using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Common;

namespace NTS.WEB.WebSite
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Response.Write(double.Parse("12.333333").ToString("f2"));
           var str= Decrypt(
                "A247FF928CF85A7722127D4F970A552AED4867E0A084284F8497DA9EC6E8EC795CA0328F03A705ECCB3BBA913CD16925",
                "njtsbecm");
           Response.Write(str);
        }

        public static string Decrypt(string pToDecrypt, string sKey)
        {
            if (!string.IsNullOrEmpty(pToDecrypt))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);　//建立加密对象的密钥和偏移量，此值重要，不能修改
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            return string.Empty;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           // System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
          //string username=  HttpUtility.UrlDecode(Utils.GetCookie("userid"), enc);
         // string username =Utils.GetCookie("userid");
            string username = Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(Utils.GetCookie("userid")));
            Label1.Text = username;
        }
    }
}