using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BEEWiN.Models
{
    public class Encryption
    {
        public string SHA256(string pwd)
        {
            //=======================================================
            // SHA256 加密
            //建立一個SHA256
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            //將字串轉為Byte[]
            byte[] source = Encoding.Default.GetBytes(pwd);
            //進行SHA256加密
            byte[] crypto = sha256.ComputeHash(source);
            //把加密後的字串從Byte[]轉為字串
            string result = Convert.ToBase64String(crypto);
            return (result);
        }
    }
}