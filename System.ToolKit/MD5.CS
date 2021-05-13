using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace System.ToolKit
{
    public class MD5
    {
        /// <summary>
        /// 使用MD5加密字符串
        /// </summary>
        /// <param name="encypStr">明文</param>
        /// <returns>密文</returns>
        public static string TextToMd5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] inputBye;
            byte[] outputBye;
            inputBye = System.Text.Encoding.ASCII.GetBytes(encypStr);
            outputBye = md5.ComputeHash(inputBye);
            retStr = Convert.ToBase64String(outputBye);
            return (retStr);
        }
    }
}
